using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable EF1003 // Benchmark index variants use fixed produced table and index names with local quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class LatestSatelliteLookupIndexBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
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
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;
  private readonly LatestSatelliteLookupIndexVariant _indexVariant;
  private readonly LatestSatelliteLookupWorkload _workload;

  public LatestSatelliteLookupIndexBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      LatestSatelliteLookupIndexVariant indexVariant,
      LatestSatelliteLookupWorkload workload)
      : this(
          provider,
          strategy,
          loadTimestampStorage,
          BenchmarkHashKeyVariant.Default,
          indexVariant,
          workload) {
  }

  public LatestSatelliteLookupIndexBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant,
      LatestSatelliteLookupIndexVariant indexVariant,
      LatestSatelliteLookupWorkload workload) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);
    ArgumentNullException.ThrowIfNull(indexVariant);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
    _hashKeyVariant = hashKeyVariant;
    _indexVariant = indexVariant;
    _workload = workload;
  }

  public string ScenarioName => _workload == LatestSatelliteLookupWorkload.UnchangedReplay
      ? "latest-satellite-lookup-replay"
      : "latest-satellite-lookup-change";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant) +
      "/" +
      _indexVariant.BaselineName;

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

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
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy, _hashKeyVariant);

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
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        sampleHashKey,
        providerCapabilities,
        "The latest satellite lookup customer hash key must use the active stable-hash shape.");
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
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel(), ProviderCapabilities);
    }
  }
}
