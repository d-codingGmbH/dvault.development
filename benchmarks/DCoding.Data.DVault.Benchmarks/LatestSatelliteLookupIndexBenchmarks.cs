using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable EF1003 // Benchmark index variants use fixed produced table and index names with local quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class LatestSatelliteLookupIndexBenchmark : IScenarioBenchmark {
  private const int CustomerCount = 100;
  private const int SeededHistoryStateCount = 20;
  private const string CustomerHashKeyColumnName = "CustomerHashKey";
  private const string LoadTimestampColumnName = "LoadTimestamp";
  private const string HashDiffColumnName = "HashDiff";
  private const string SatelliteTableName = "SatCustomerProfile";
  private const string DefaultSatelliteParentIndexName = "IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp";
  private static readonly DateTimeOffset BaseTimestamp = new(2026, 5, 8, 8, 0, 0, TimeSpan.Zero);

  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly LatestSatelliteLookupIndexVariant _indexVariant;
  private readonly LatestSatelliteLookupWorkload _workload;

  public LatestSatelliteLookupIndexBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      LatestSatelliteLookupIndexVariant indexVariant,
      LatestSatelliteLookupWorkload workload) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(indexVariant);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
    _indexVariant = indexVariant;
    _workload = workload;
  }

  public string ScenarioName => _workload == LatestSatelliteLookupWorkload.UnchangedReplay
      ? "latest-satellite-lookup-replay"
      : "latest-satellite-lookup-change";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy) +
      "/" +
      _indexVariant.BaselineName;

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize =>
      CustomerCount.ToString(CultureInfo.InvariantCulture) +
      " customers, " +
      SeededHistoryStateCount.ToString(CultureInfo.InvariantCulture) +
      " existing profile states each";

  public string ChangeRatio => _workload == LatestSatelliteLookupWorkload.UnchangedReplay
      ? "unchanged replay, " + _indexVariant.Description
      : "changed replay, " + _indexVariant.Description;

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<LatestSatelliteLookupDataVaultContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    try {
      await using (var context = new LatestSatelliteLookupDataVaultContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
        await _indexVariant.ApplyAsync(context, cancellationToken).ConfigureAwait(false);
        await SeedHistoryAsync(context, saveService, cancellationToken).ConfigureAwait(false);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new LatestSatelliteLookupDataVaultContext(options, providerCapabilities);
        var customerHashKeys = await LoadCustomerHashKeysAsync(context, cancellationToken).ConfigureAwait(false);
        var operations = Enumerable.Range(0, CustomerCount)
            .Select(customerIndex => CreateMeasuredSatelliteOperation(customerIndex, customerHashKeys[customerIndex]))
            .ToArray();

        await saveService.SaveAsync(
            context,
            new DataVaultSaveRequest(
                BaseTimestamp.AddMinutes(SeededHistoryStateCount + 1),
                "latest-index-measure",
                [],
                [],
                operations),
            cancellationToken).ConfigureAwait(false);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, cancellationToken).ConfigureAwait(false);

      var expectedRowCount = ExpectedSatelliteRowCount.ToString(CultureInfo.InvariantCulture);
      return new ScenarioBenchmarkResult(
          elapsed,
          expectedRowCount + " profile satellite rows after " + _workload.ToDisplayText() + " latest lookup");
    }
    finally {
      await using var cleanupContext = new LatestSatelliteLookupDataVaultContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private async Task SeedHistoryAsync(
      DbContext context,
      IDataVaultSaveService saveService,
      CancellationToken cancellationToken) {
    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            BaseTimestamp,
            "latest-index-seed",
            Enumerable.Range(0, CustomerCount)
                .Select(customerIndex => new DataVaultHubSaveOperation(
                    ScenarioContracts.CustomerHub,
                    [new("Customer Id", CreateBusinessKey(customerIndex))]))
                .ToArray(),
            []),
        cancellationToken).ConfigureAwait(false);
    var customerHashKeys = hubResult.SavedRecords
        .Select((record, customerIndex) => new {
          CustomerIndex = customerIndex,
          record.HashKey,
        })
        .ToDictionary(value => value.CustomerIndex, value => value.HashKey);

    foreach (var historyChunkStart in Enumerable.Range(0, SeededHistoryStateCount).Chunk(5).Select(chunk => chunk[0])) {
      var requests = Enumerable.Range(historyChunkStart, Math.Min(5, SeededHistoryStateCount - historyChunkStart))
          .Select(historyIndex => new DataVaultSaveRequest(
              BaseTimestamp.AddMinutes(historyIndex + 1),
              "latest-index-seed",
              [],
              [],
              Enumerable.Range(0, CustomerCount)
                  .Select(customerIndex => CreateSeedSatelliteOperation(
                      customerIndex,
                      historyIndex,
                      customerHashKeys[customerIndex]))
                  .ToArray()))
          .ToArray();

      await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(requests),
          cancellationToken).ConfigureAwait(false);
    }
  }

  private async Task<Dictionary<int, string>> LoadCustomerHashKeysAsync(
      LatestSatelliteLookupDataVaultContext context,
      CancellationToken cancellationToken) {
    var hubRows = await context.Set<Dictionary<string, object>>("HubCustomer")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    return hubRows.ToDictionary(
        row => ParseBusinessKey((string)row["CustomerId"]),
        row => (string)row["CustomerHashKey"]);
  }

  private async Task VerifyOutcomeAsync(
      DbContextOptions<LatestSatelliteLookupDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CancellationToken cancellationToken) {
    await using var context = new LatestSatelliteLookupDataVaultContext(options, providerCapabilities);
    var satelliteRows = await context.Set<Dictionary<string, object>>(SatelliteTableName)
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var customerHashKeys = await LoadCustomerHashKeysAsync(context, cancellationToken).ConfigureAwait(false);
    var sampleHashKey = customerHashKeys[42];
    var sampleLatestRow = satelliteRows
        .Where(row => string.Equals((string)row[CustomerHashKeyColumnName], sampleHashKey, StringComparison.Ordinal))
        .OrderByDescending(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row))
        .First();

    BenchmarkAssert.Equal(
        ExpectedSatelliteRowCount,
        satelliteRows.Count,
        "The latest satellite lookup benchmark persisted an unexpected satellite row count.");
    BenchmarkAssert.Equal(
        ExpectedLatestHashDiff(42),
        (string)sampleLatestRow[HashDiffColumnName],
        "The latest satellite lookup benchmark selected an unexpected latest hash diff.");
  }

  private DataVaultSatelliteSaveOperation CreateSeedSatelliteOperation(
      int customerIndex,
      int historyIndex,
      string customerHashKey) {
    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.CustomerProfileSatellite,
        customerHashKey,
        [
            new("customer_name", "Customer " + customerIndex.ToString("0000", CultureInfo.InvariantCulture)),
            new("customer_status", "state-" + historyIndex.ToString("00", CultureInfo.InvariantCulture)),
        ],
        CreateSeedHashDiff(customerIndex, historyIndex));
  }

  private DataVaultSatelliteSaveOperation CreateMeasuredSatelliteOperation(
      int customerIndex,
      string customerHashKey) {
    var historyIndex = SeededHistoryStateCount - 1;
    var customerName = "Customer " + customerIndex.ToString("0000", CultureInfo.InvariantCulture);
    var customerStatus = _workload == LatestSatelliteLookupWorkload.UnchangedReplay
        ? "state-" + historyIndex.ToString("00", CultureInfo.InvariantCulture)
        : "changed";

    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.CustomerProfileSatellite,
        customerHashKey,
        [
            new("customer_name", customerName),
            new("customer_status", customerStatus),
        ],
        _workload == LatestSatelliteLookupWorkload.UnchangedReplay
            ? CreateSeedHashDiff(customerIndex, historyIndex)
            : CreateChangedHashDiff(customerIndex));
  }

  private int ExpectedSatelliteRowCount => CustomerCount * SeededHistoryStateCount +
      (_workload == LatestSatelliteLookupWorkload.ChangedReplay ? CustomerCount : 0);

  private string ExpectedLatestHashDiff(int customerIndex) {
    return _workload == LatestSatelliteLookupWorkload.UnchangedReplay
        ? CreateSeedHashDiff(customerIndex, SeededHistoryStateCount - 1)
        : CreateChangedHashDiff(customerIndex);
  }

  private static string CreateBusinessKey(int customerIndex) {
    return "C-LATEST-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture);
  }

  private static int ParseBusinessKey(string businessKey) {
    return int.Parse(businessKey["C-LATEST-".Length..], CultureInfo.InvariantCulture);
  }

  private static string CreateSeedHashDiff(int customerIndex, int historyIndex) {
    return "latest-" +
        customerIndex.ToString("0000", CultureInfo.InvariantCulture) +
        "-" +
        historyIndex.ToString("00", CultureInfo.InvariantCulture);
  }

  private static string CreateChangedHashDiff(int customerIndex) {
    return "latest-changed-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture);
  }

  private sealed class LatestSatelliteLookupDataVaultContext(
      DbContextOptions<LatestSatelliteLookupDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel(), providerCapabilities);
    }
  }
}

internal enum LatestSatelliteLookupWorkload {
  UnchangedReplay,
  ChangedReplay,
}

internal static class LatestSatelliteLookupWorkloadExtensions {
  public static string ToDisplayText(this LatestSatelliteLookupWorkload workload) {
    return workload switch {
      LatestSatelliteLookupWorkload.UnchangedReplay => "unchanged replay",
      LatestSatelliteLookupWorkload.ChangedReplay => "changed replay",
      _ => throw new ArgumentOutOfRangeException(nameof(workload), workload, "Unsupported latest lookup workload."),
    };
  }
}

internal sealed class LatestSatelliteLookupIndexVariant {
  private const string ParentDescIndexName = "IX_DV_LATEST_DESC";
  private const string CoveringIndexName = "IX_DV_LATEST_COVER";
  private const string CompressedIndexName = "IX_DV_LATEST_COMP";

  private LatestSatelliteLookupIndexVariant(
      string baselineName,
      string description,
      Func<DbContext, CancellationToken, Task> applyAsync) {
    BaselineName = baselineName;
    Description = description;
    ApplyAsync = applyAsync;
  }

  public string BaselineName { get; }

  public string Description { get; }

  public Func<DbContext, CancellationToken, Task> ApplyAsync { get; }

  public static IReadOnlyList<LatestSatelliteLookupIndexVariant> GetVariants(string providerName) {
    var variants = new List<LatestSatelliteLookupIndexVariant>
    {
        new("latest-index-default", "current model index", (_, _) => Task.CompletedTask),
        new("latest-index-parent-desc", "parent plus descending timestamp index", ApplyParentDescIndexAsync),
        new("latest-index-covering", "parent plus descending timestamp plus hash-diff index", ApplyCoveringIndexAsync),
    };

    if (string.Equals(providerName, BenchmarkExternalProviderDefinitions.Oracle.ProviderName, StringComparison.Ordinal)) {
      variants.Add(new(
          "latest-index-covering-compress1",
          "Oracle compressed parent plus descending timestamp plus hash-diff index",
          ApplyOracleCompressedIndexAsync));
    }

    return variants;
  }

  private static async Task ApplyParentDescIndexAsync(DbContext context, CancellationToken cancellationToken) {
    await DropDefaultSatelliteParentIndexAsync(context, cancellationToken).ConfigureAwait(false);
    await ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: "CREATE INDEX " + QuoteSqlite(ParentDescIndexName) + " ON " + QuoteSqlite("SatCustomerProfile") +
            " (" + QuoteSqlite("CustomerHashKey") + ", " + QuoteSqlite("LoadTimestamp") + " DESC);",
        postgres: "CREATE INDEX " + QuotePostgres(ParentDescIndexName) + " ON " + QuotePostgres("SatCustomerProfile") +
            " (" + QuotePostgres("CustomerHashKey") + ", " + QuotePostgres("LoadTimestamp") + " DESC);",
        sqlServer: "CREATE INDEX " + QuoteSqlServer(ParentDescIndexName) + " ON " + QuoteSqlServer("SatCustomerProfile") +
            " (" + QuoteSqlServer("CustomerHashKey") + ", " + QuoteSqlServer("LoadTimestamp") + " DESC);",
        mySql: "CREATE INDEX " + QuoteMySql(ParentDescIndexName) + " ON " + QuoteMySql("SatCustomerProfile") +
            " (" + QuoteMySql("CustomerHashKey") + ", " + QuoteMySql("LoadTimestamp") + " DESC);",
        oracle: "CREATE INDEX " + QuoteOracle(ParentDescIndexName) + " ON " + QuoteOracle("SatCustomerProfile") +
            " (" + QuoteOracle("CustomerHashKey") + ", " + QuoteOracle("LoadTimestamp") + " DESC)").ConfigureAwait(false);
  }

  private static async Task ApplyCoveringIndexAsync(DbContext context, CancellationToken cancellationToken) {
    await DropDefaultSatelliteParentIndexAsync(context, cancellationToken).ConfigureAwait(false);
    await ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: "CREATE INDEX " + QuoteSqlite(CoveringIndexName) + " ON " + QuoteSqlite("SatCustomerProfile") +
            " (" + QuoteSqlite("CustomerHashKey") + ", " + QuoteSqlite("LoadTimestamp") + " DESC, " + QuoteSqlite("HashDiff") + ");",
        postgres: "CREATE INDEX " + QuotePostgres(CoveringIndexName) + " ON " + QuotePostgres("SatCustomerProfile") +
            " (" + QuotePostgres("CustomerHashKey") + ", " + QuotePostgres("LoadTimestamp") + " DESC) INCLUDE (" + QuotePostgres("HashDiff") + ");",
        sqlServer: "CREATE INDEX " + QuoteSqlServer(CoveringIndexName) + " ON " + QuoteSqlServer("SatCustomerProfile") +
            " (" + QuoteSqlServer("CustomerHashKey") + ", " + QuoteSqlServer("LoadTimestamp") + " DESC) INCLUDE (" + QuoteSqlServer("HashDiff") + ");",
        mySql: "CREATE INDEX " + QuoteMySql(CoveringIndexName) + " ON " + QuoteMySql("SatCustomerProfile") +
            " (" + QuoteMySql("CustomerHashKey") + ", " + QuoteMySql("LoadTimestamp") + " DESC, " + QuoteMySql("HashDiff") + ");",
        oracle: "CREATE INDEX " + QuoteOracle(CoveringIndexName) + " ON " + QuoteOracle("SatCustomerProfile") +
            " (" + QuoteOracle("CustomerHashKey") + ", " + QuoteOracle("LoadTimestamp") + " DESC, " + QuoteOracle("HashDiff") + ")")
        .ConfigureAwait(false);
  }

  private static async Task ApplyOracleCompressedIndexAsync(DbContext context, CancellationToken cancellationToken) {
    await DropDefaultSatelliteParentIndexAsync(context, cancellationToken).ConfigureAwait(false);
    await ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: null,
        postgres: null,
        sqlServer: null,
        mySql: null,
        oracle: "CREATE INDEX " + QuoteOracle(CompressedIndexName) + " ON " + QuoteOracle("SatCustomerProfile") +
            " (" + QuoteOracle("CustomerHashKey") + ", " + QuoteOracle("LoadTimestamp") + " DESC, " + QuoteOracle("HashDiff") + ") COMPRESS 1")
        .ConfigureAwait(false);
  }

  private static Task DropDefaultSatelliteParentIndexAsync(DbContext context, CancellationToken cancellationToken) {
    return ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: "DROP INDEX IF EXISTS " + QuoteSqlite("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") + ";",
        postgres: "DROP INDEX IF EXISTS " + QuotePostgres("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") + ";",
        sqlServer: "DROP INDEX IF EXISTS " + QuoteSqlServer("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") +
            " ON " + QuoteSqlServer("SatCustomerProfile") + ";",
        mySql: "DROP INDEX " + QuoteMySql("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") +
            " ON " + QuoteMySql("SatCustomerProfile") + ";",
        oracle: "BEGIN " +
            "FOR index_record IN (" +
            "SELECT index_name FROM user_indexes WHERE table_name = 'SatCustomerProfile' AND index_type <> 'LOB' AND index_name NOT IN (" +
            "SELECT constraint_name FROM user_constraints WHERE table_name = 'SatCustomerProfile' AND constraint_type = 'P'" +
            ")) LOOP " +
            "EXECUTE IMMEDIATE 'DROP INDEX \"' || REPLACE(index_record.index_name, '\"', '\"\"') || '\"'; " +
            "END LOOP; " +
            "END;");
  }

  private static async Task ExecuteProviderSqlAsync(
      DbContext context,
      CancellationToken cancellationToken,
      string? sqlite,
      string? postgres,
      string? sqlServer,
      string? mySql,
      string? oracle) {
    var commandText = context.Database.ProviderName switch {
      "Microsoft.EntityFrameworkCore.Sqlite" => sqlite,
      "Npgsql.EntityFrameworkCore.PostgreSQL" => postgres,
      "Microsoft.EntityFrameworkCore.SqlServer" => sqlServer,
      "Pomelo.EntityFrameworkCore.MySql" or "MySql.EntityFrameworkCore" => mySql,
      "Oracle.EntityFrameworkCore" => oracle,
      _ => throw new NotSupportedException(
          "Latest satellite lookup index benchmarks do not support provider '" + context.Database.ProviderName + "'."),
    };

    if (string.IsNullOrWhiteSpace(commandText)) {
      return;
    }

    await context.Database.ExecuteSqlRawAsync(commandText, cancellationToken).ConfigureAwait(false);
  }

  private static string QuoteSqlite(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string QuotePostgres(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string QuoteSqlServer(string identifier) {
    return "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }

  private static string QuoteMySql(string identifier) {
    return "`" + identifier.Replace("`", "``", StringComparison.Ordinal) + "`";
  }

  private static string QuoteOracle(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}

#pragma warning restore EF1003
