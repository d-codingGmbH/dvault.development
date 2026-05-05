using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfilePlainEfBenchmark : IScenarioBenchmark {
  public string ScenarioName => "customer-profile-history";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => "conventional-ef";

  public string StrategyFamily => DataVaultBenchmarkHelpers.ClassicEfStrategyFamily;

  public string DatasetSize => "1 customer, 2 profile states";

  public string ChangeRatio => "50% repeat-change history";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var options = new DbContextOptionsBuilder<CustomerProfileHistoryContext>()
        .UseSqlite(database.ConnectionString)
        .Options;

    await using (var context = new CustomerProfileHistoryContext(options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    }

    var elapsed = await BenchmarkClock.MeasureAsync(async () => {
      await using var context = new CustomerProfileHistoryContext(options);

      foreach (var customerProfileEvent in ScenarioContracts.CustomerProfileEvents) {
        context.CustomerProfileHistoryRows.Add(new CustomerProfileHistoryRow {
          CustomerBusinessKey = customerProfileEvent.CustomerBusinessKey,
          CustomerName = customerProfileEvent.CustomerName,
          CustomerStatus = customerProfileEvent.CustomerStatus,
          ChangedAtUtc = customerProfileEvent.ChangedAtUtc,
          RecordSource = customerProfileEvent.RecordSource,
        });

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      }
    }).ConfigureAwait(false);

    await VerifyOutcomeAsync(options, cancellationToken).ConfigureAwait(false);

    return new ScenarioBenchmarkResult(elapsed, "2 customer profile history rows for C-100");
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileHistoryContext> options,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileHistoryContext(options);
    var rows = (await context.CustomerProfileHistoryRows
        .AsNoTracking()
        .Where(row => row.CustomerBusinessKey == ScenarioContracts.CustomerBusinessKey)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => row.ChangedAtUtc)
        .ThenBy(row => row.Id)
        .ToArray();

    BenchmarkAssert.Equal(
        ScenarioContracts.CustomerProfileEvents.Length,
        await context.CustomerProfileHistoryRows.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false),
        "The conventional EF customer profile benchmark must persist exactly the contracted rows.");
    BenchmarkAssert.Equal(
        ScenarioContracts.CustomerProfileEvents.Length,
        rows.Length,
        "The conventional EF customer profile benchmark must persist rows for C-100.");

    for (var index = 0; index < ScenarioContracts.CustomerProfileEvents.Length; index++) {
      AssertHistoryRow(rows[index], ScenarioContracts.CustomerProfileEvents[index]);
    }
  }

  private static void AssertHistoryRow(CustomerProfileHistoryRow row, CustomerProfileEvent expected) {
    BenchmarkAssert.Equal(expected.CustomerBusinessKey, row.CustomerBusinessKey, "Customer profile business key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, row.CustomerName, "Customer profile name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, row.CustomerStatus, "Customer profile status drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, row.ChangedAtUtc, "Customer profile timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, row.RecordSource, "Customer profile record source drifted.");
  }

  private sealed class CustomerProfileHistoryContext(DbContextOptions<CustomerProfileHistoryContext> options)
      : DbContext(options) {
    public DbSet<CustomerProfileHistoryRow> CustomerProfileHistoryRows => Set<CustomerProfileHistoryRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<CustomerProfileHistoryRow>(entity => {
        entity.ToTable("CustomerProfileHistory");
        entity.HasKey(row => row.Id);
        entity.Property(row => row.CustomerBusinessKey).IsRequired();
        entity.Property(row => row.CustomerName).IsRequired();
        entity.Property(row => row.CustomerStatus).IsRequired();
        entity.Property(row => row.ChangedAtUtc).IsRequired();
        entity.Property(row => row.RecordSource).IsRequired();
        entity.HasIndex(row => new { row.CustomerBusinessKey, row.ChangedAtUtc });
      });
    }
  }

  private sealed class CustomerProfileHistoryRow {
    public long Id { get; set; }

    public string CustomerBusinessKey { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerStatus { get; set; } = string.Empty;

    public DateTimeOffset ChangedAtUtc { get; set; }

    public string RecordSource { get; set; } = string.Empty;
  }
}

internal sealed class CustomerProfileDataVaultBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;

  public CustomerProfileDataVaultBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy) {
    ArgumentNullException.ThrowIfNull(provider);

    _provider = provider;
    _strategy = strategy;
  }

  public string ScenarioName => "customer-profile-history";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize => "1 customer, 2 profile states";

  public string ChangeRatio => "50% repeat-change history";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileDataVaultContext>();
    var providerCapabilities = _provider.ProviderCapabilities;
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    try {
      await using (var context = new CustomerProfileDataVaultContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        string customerHashKey;
        var firstEvent = ScenarioContracts.CustomerProfileEvents[0];
        await using (var context = new CustomerProfileDataVaultContext(options, providerCapabilities)) {
          var firstHubResult = await saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  firstEvent.ChangedAtUtc,
                  firstEvent.RecordSource,
                  [new(ScenarioContracts.CustomerHub, [new("Customer Id", firstEvent.CustomerBusinessKey)])],
                  []),
              cancellationToken).ConfigureAwait(false);
          customerHashKey = DataVaultBenchmarkHelpers.GetHashKey(firstHubResult, DataVaultTableKind.Hub, "Customer");

          await saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  firstEvent.ChangedAtUtc,
                  firstEvent.RecordSource,
                  [],
                  [],
                  [CreateSatelliteSaveOperation(firstEvent, customerHashKey)]),
              cancellationToken).ConfigureAwait(false);
        }

        var secondEvent = ScenarioContracts.CustomerProfileEvents[1];
        await using (var context = new CustomerProfileDataVaultContext(options, providerCapabilities)) {
          await saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  secondEvent.ChangedAtUtc,
                  secondEvent.RecordSource,
                  [new(ScenarioContracts.CustomerHub, [new("Customer Id", secondEvent.CustomerBusinessKey)])],
                  []),
              cancellationToken).ConfigureAwait(false);

          await saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  secondEvent.ChangedAtUtc,
                  secondEvent.RecordSource,
                  [],
                  [],
                  [CreateSatelliteSaveOperation(secondEvent, customerHashKey)]),
              cancellationToken).ConfigureAwait(false);
        }
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(elapsed, "1 customer hub row and 2 profile satellite rows for C-100");
    }
    finally {
      await using var cleanupContext = new CustomerProfileDataVaultContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static DataVaultSatelliteSaveOperation CreateSatelliteSaveOperation(
      CustomerProfileEvent customerProfileEvent,
      string customerHashKey) {
    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.CustomerProfileSatellite,
        customerHashKey,
        [
            new("customer_name", customerProfileEvent.CustomerName),
            new("customer_status", customerProfileEvent.CustomerStatus),
        ],
        customerProfileEvent.HashDiff);
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileDataVaultContext(options, providerCapabilities);
    var customerRows = await context.Set<Dictionary<string, object>>("HubCustomer")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var profileRows = (await context.Set<Dictionary<string, object>>("SatCustomerProfile")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row))
        .ToArray();
    var customerRow = BenchmarkAssert.Single(customerRows, "The DVault customer benchmark must persist one customer hub row.");
    var customerHashKey = (string)customerRow["CustomerHashKey"];

    BenchmarkAssert.Equal(ScenarioContracts.CustomerBusinessKey, (string)customerRow["CustomerId"], "Customer hub business key drifted.");
    BenchmarkAssert.True(DataVaultBenchmarkHelpers.IsLowercaseSha256(customerHashKey), "Customer hub hash key must use the stable SHA-256 shape.");
    BenchmarkAssert.Equal(ScenarioContracts.CustomerProfileEvents.Length, profileRows.Length, "The DVault customer benchmark must persist two profile satellite rows.");

    for (var index = 0; index < ScenarioContracts.CustomerProfileEvents.Length; index++) {
      AssertProfileSatelliteRow(profileRows[index], customerHashKey, ScenarioContracts.CustomerProfileEvents[index]);
    }
  }

  private static void AssertProfileSatelliteRow(
      Dictionary<string, object> row,
      string customerHashKey,
      CustomerProfileEvent expected) {
    BenchmarkAssert.Equal(customerHashKey, (string)row["CustomerHashKey"], "Profile satellite parent hash key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, (string)row["CustomerName"], "Profile satellite customer name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, (string)row["CustomerStatus"], "Profile satellite status drifted.");
    BenchmarkAssert.Equal(expected.HashDiff, (string)row["HashDiff"], "Profile satellite hash diff drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, DataVaultBenchmarkHelpers.ReadLoadTimestamp(row), "Profile satellite load timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, (string)row["RecordSource"], "Profile satellite record source drifted.");
  }

  private sealed class CustomerProfileDataVaultContext(
      DbContextOptions<CustomerProfileDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel(), providerCapabilities);
    }
  }
}

internal sealed class CustomerProfileBulkPlainEfBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly CustomerProfileBulkScenarioDefinition _scenario;

  public CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarioDefinition scenario) {
    ArgumentNullException.ThrowIfNull(scenario);

    _provider = BenchmarkDatabaseProviders.Sqlite;
    _scenario = scenario;
  }

  public CustomerProfileBulkPlainEfBenchmark(
      BenchmarkDatabaseProvider provider,
      CustomerProfileBulkScenarioDefinition scenario) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(scenario);

    _provider = provider;
    _scenario = scenario;
  }

  public string ScenarioName => _scenario.ScenarioName;

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => "conventional-ef-bulk";

  public string StrategyFamily => DataVaultBenchmarkHelpers.ClassicEfStrategyFamily;

  public string DatasetSize => _scenario.DatasetSize;

  public string ChangeRatio => _scenario.ChangeRatio;

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileBulkHistoryContext>();

    try {
      await using (var context = new CustomerProfileBulkHistoryContext(options)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new CustomerProfileBulkHistoryContext(options);
        var rowId = 0L;

        foreach (var customerProfileEvent in _scenario.CreateEvents()) {
          context.CustomerProfileHistoryRows.Add(new CustomerProfileBulkHistoryRow {
            Id = ++rowId,
            CustomerBusinessKey = customerProfileEvent.CustomerBusinessKey,
            CustomerName = customerProfileEvent.CustomerName,
            CustomerStatus = customerProfileEvent.CustomerStatus,
            ChangedAtUtc = customerProfileEvent.ChangedAtUtc,
            RecordSource = customerProfileEvent.RecordSource,
          });
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, _scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.TotalChangeCount.ToString(CultureInfo.InvariantCulture) +
          " customer profile history rows for " +
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customers");
    }
    finally {
      await using var cleanupContext = new CustomerProfileBulkHistoryContext(options);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileBulkHistoryContext> options,
      CustomerProfileBulkScenarioDefinition scenario,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileBulkHistoryContext(options);
    var sampleBusinessKey = scenario.CreateBusinessKey(scenario.SampleCustomerIndex);
    var rows = (await context.CustomerProfileHistoryRows
        .AsNoTracking()
        .Where(row => row.CustomerBusinessKey == sampleBusinessKey)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => row.ChangedAtUtc)
        .ThenBy(row => row.Id)
        .ToArray();

    BenchmarkAssert.Equal(
        scenario.TotalChangeCount,
        await context.CustomerProfileHistoryRows.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false),
        "The conventional EF bulk customer profile benchmark must persist every contracted history row.");
    BenchmarkAssert.Equal(
        scenario.ChangeCount,
        rows.Length,
        "The conventional EF bulk customer profile benchmark must persist every sample customer state.");

    AssertHistoryRow(rows[0], scenario.CreateEvent(scenario.SampleCustomerIndex, 0));
    AssertHistoryRow(
        rows[^1],
        scenario.CreateEvent(
            scenario.SampleCustomerIndex,
            scenario.ChangeCount - 1));
  }

  private static void AssertHistoryRow(CustomerProfileBulkHistoryRow row, CustomerProfileBulkEvent expected) {
    BenchmarkAssert.Equal(expected.CustomerBusinessKey, row.CustomerBusinessKey, "Bulk customer profile business key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, row.CustomerName, "Bulk customer profile name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, row.CustomerStatus, "Bulk customer profile status drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, row.ChangedAtUtc, "Bulk customer profile timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, row.RecordSource, "Bulk customer profile record source drifted.");
  }

  private sealed class CustomerProfileBulkHistoryContext(DbContextOptions<CustomerProfileBulkHistoryContext> options)
      : DbContext(options) {
    public DbSet<CustomerProfileBulkHistoryRow> CustomerProfileHistoryRows => Set<CustomerProfileBulkHistoryRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<CustomerProfileBulkHistoryRow>(entity => {
        entity.ToTable("CustomerProfileBulkHistory");
        entity.HasKey(row => row.Id);
        entity.Property(row => row.Id).ValueGeneratedNever();
        entity.Property(row => row.CustomerBusinessKey).IsRequired();
        entity.Property(row => row.CustomerName).IsRequired();
        entity.Property(row => row.CustomerStatus).IsRequired();
        entity.Property(row => row.ChangedAtUtc).IsRequired();
        entity.Property(row => row.RecordSource).IsRequired();
        entity.HasIndex(row => new { row.CustomerBusinessKey, row.ChangedAtUtc });
      });
    }
  }

  private sealed class CustomerProfileBulkHistoryRow {
    public long Id { get; set; }

    public string CustomerBusinessKey { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerStatus { get; set; } = string.Empty;

    public DateTimeOffset ChangedAtUtc { get; set; }

    public string RecordSource { get; set; } = string.Empty;
  }
}

internal sealed class CustomerProfileBulkDataVaultBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly CustomerProfileBulkScenarioDefinition _scenario;
  private readonly DataVaultBenchmarkStrategy _strategy;

  public CustomerProfileBulkDataVaultBenchmark(
      BenchmarkDatabaseProvider provider,
      CustomerProfileBulkScenarioDefinition scenario,
      DataVaultBenchmarkStrategy strategy) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(scenario);

    _provider = provider;
    _scenario = scenario;
    _strategy = strategy;
  }

  public string ScenarioName => _scenario.ScenarioName;

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize => _scenario.DatasetSize;

  public string ChangeRatio => _scenario.ChangeRatio;

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileBulkDataVaultContext>();
    var providerCapabilities = _provider.ProviderCapabilities;
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    try {
      await using (var context = new CustomerProfileBulkDataVaultContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new CustomerProfileBulkDataVaultContext(options, providerCapabilities);
        var hubResult = await saveService.SaveAsync(
            context,
            new DataVaultSaveRequest(
                _scenario.BaseTimestamp,
                _scenario.RecordSource,
                Enumerable.Range(0, _scenario.CustomerCount)
                    .Select(customerIndex => new DataVaultHubSaveOperation(
                        ScenarioContracts.CustomerHub,
                        [new("Customer Id", _scenario.CreateBusinessKey(customerIndex))]))
                    .ToArray(),
                []),
            cancellationToken).ConfigureAwait(false);
        var customerHashKeys = hubResult.SavedRecords
            .Select((record, customerIndex) => new {
              BusinessKey = _scenario.CreateBusinessKey(customerIndex),
              record.HashKey,
            })
            .ToDictionary(value => value.BusinessKey, value => value.HashKey, StringComparer.Ordinal);

        var satelliteRequests = Enumerable.Range(0, _scenario.ChangeCount)
            .Select(changeIndex => new DataVaultSaveRequest(
                _scenario.BaseTimestamp.AddMinutes(changeIndex),
                _scenario.RecordSource,
                [],
                [],
                Enumerable.Range(0, _scenario.CustomerCount)
                    .Select(customerIndex => {
                      var customerProfileEvent = _scenario.CreateEvent(customerIndex, changeIndex);
                      return CreateSatelliteSaveOperation(
                          customerProfileEvent,
                          customerHashKeys[customerProfileEvent.CustomerBusinessKey]);
                    })
                    .ToArray()))
            .ToArray();

        await saveService.SaveAsync(
            context,
            new DataVaultBulkSaveRequest(satelliteRequests),
            cancellationToken).ConfigureAwait(false);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, _scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customer hubs and " +
          _scenario.TotalChangeCount.ToString(CultureInfo.InvariantCulture) +
          " profile satellite rows");
    }
    finally {
      await using var cleanupContext = new CustomerProfileBulkDataVaultContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static DataVaultSatelliteSaveOperation CreateSatelliteSaveOperation(
      CustomerProfileBulkEvent customerProfileEvent,
      string customerHashKey) {
    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.CustomerProfileSatellite,
        customerHashKey,
        [
            new("customer_name", customerProfileEvent.CustomerName),
            new("customer_status", customerProfileEvent.CustomerStatus),
        ],
        customerProfileEvent.HashDiff);
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileBulkDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CustomerProfileBulkScenarioDefinition scenario,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileBulkDataVaultContext(options, providerCapabilities);
    var hubRows = await context.Set<Dictionary<string, object>>("HubCustomer")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var profileRows = await context.Set<Dictionary<string, object>>("SatCustomerProfile")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var sampleBusinessKey = scenario.CreateBusinessKey(scenario.SampleCustomerIndex);
    var sampleCustomerRow = BenchmarkAssert.Single(
        hubRows.Where(row => string.Equals((string)row["CustomerId"], sampleBusinessKey, StringComparison.Ordinal)),
        "The DVault bulk customer benchmark must persist the sample customer hub row.");
    var sampleCustomerHashKey = (string)sampleCustomerRow["CustomerHashKey"];
    var sampleProfileRows = profileRows
        .Where(row => string.Equals((string)row["CustomerHashKey"], sampleCustomerHashKey, StringComparison.Ordinal))
        .OrderBy(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row))
        .ToArray();

    BenchmarkAssert.Equal(
        scenario.CustomerCount,
        hubRows.Count,
        "The DVault bulk customer benchmark must persist every customer hub row.");
    BenchmarkAssert.Equal(
        scenario.TotalChangeCount,
        profileRows.Count,
        "The DVault bulk customer benchmark must persist every profile satellite row.");
    BenchmarkAssert.Equal(
        scenario.ChangeCount,
        sampleProfileRows.Length,
        "The DVault bulk customer benchmark must persist every sample profile state.");

    AssertProfileSatelliteRow(
        sampleProfileRows[0],
        sampleCustomerHashKey,
        scenario.CreateEvent(scenario.SampleCustomerIndex, 0));
    AssertProfileSatelliteRow(
        sampleProfileRows[^1],
        sampleCustomerHashKey,
        scenario.CreateEvent(
            scenario.SampleCustomerIndex,
            scenario.ChangeCount - 1));
  }

  private static void AssertProfileSatelliteRow(
      Dictionary<string, object> row,
      string customerHashKey,
      CustomerProfileBulkEvent expected) {
    BenchmarkAssert.Equal(customerHashKey, (string)row["CustomerHashKey"], "Bulk profile satellite parent hash key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, (string)row["CustomerName"], "Bulk profile satellite customer name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, (string)row["CustomerStatus"], "Bulk profile satellite status drifted.");
    BenchmarkAssert.Equal(expected.HashDiff, (string)row["HashDiff"], "Bulk profile satellite hash diff drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, DataVaultBenchmarkHelpers.ReadLoadTimestamp(row), "Bulk profile satellite load timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, (string)row["RecordSource"], "Bulk profile satellite record source drifted.");
  }

  private sealed class CustomerProfileBulkDataVaultContext(
      DbContextOptions<CustomerProfileBulkDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel(), providerCapabilities);
    }
  }
}

internal static class CustomerProfileBulkScenarios {
  private const int CustomerCount = 100;
  private const int SampleCustomerIndex = 42;
  private static readonly DateTimeOffset BaseTimestamp = new(2026, 4, 29, 10, 0, 0, TimeSpan.Zero);
  private static readonly int[] ScaleCustomerCounts = [10, 100, 1000];

  public static readonly CustomerProfileBulkScenarioDefinition InsertOnly = new(
      "customer-profile-bulk-insert-only",
      "100 customers, 1 profile state each",
      "0% repeat-change history",
      CustomerCount,
      1,
      SampleCustomerIndex,
      "bulk-insert-benchmark",
      BaseTimestamp);

  public static readonly CustomerProfileBulkScenarioDefinition ChangeHeavy = new(
      "customer-profile-bulk-history",
      "100 customers, 10 profile states each",
      "90% repeat-change history",
      CustomerCount,
      10,
      SampleCustomerIndex,
      "bulk-history-benchmark",
      BaseTimestamp);

  public static IReadOnlyList<CustomerProfileBulkScenarioDefinition> ScaleMatrix { get; } =
  [
      .. ScaleCustomerCounts.Select(customerCount => CreateScale(customerCount, changeCount: 1)),
      .. ScaleCustomerCounts.Select(customerCount => CreateScale(customerCount, changeCount: 10)),
  ];

  private static CustomerProfileBulkScenarioDefinition CreateScale(int customerCount, int changeCount) {
    var changeRatio = changeCount == 1
        ? "0% repeat-change history"
        : (((changeCount - 1) * 100) / changeCount).ToString(CultureInfo.InvariantCulture) + "% repeat-change history";

    return new CustomerProfileBulkScenarioDefinition(
        "customer-profile-scale-" +
        customerCount.ToString(CultureInfo.InvariantCulture) +
        "x" +
        changeCount.ToString(CultureInfo.InvariantCulture),
        customerCount.ToString(CultureInfo.InvariantCulture) +
        " customers, " +
        changeCount.ToString(CultureInfo.InvariantCulture) +
        " profile state" +
        (changeCount == 1 ? string.Empty : "s") +
        " each",
        changeRatio,
        customerCount,
        changeCount,
        Math.Min(SampleCustomerIndex, customerCount - 1),
        "scale-" +
        customerCount.ToString(CultureInfo.InvariantCulture) +
        "x" +
        changeCount.ToString(CultureInfo.InvariantCulture) +
        "-benchmark",
        BaseTimestamp);
  }
}

internal sealed record CustomerProfileBulkScenarioDefinition(
    string ScenarioName,
    string DatasetSize,
    string ChangeRatio,
    int CustomerCount,
    int ChangeCount,
    int SampleCustomerIndex,
    string RecordSource,
    DateTimeOffset BaseTimestamp) {
  public int TotalChangeCount => CustomerCount * ChangeCount;

  public IEnumerable<CustomerProfileBulkEvent> CreateEvents() {
    return Enumerable.Range(0, CustomerCount)
        .SelectMany(customerIndex => Enumerable.Range(0, ChangeCount)
            .Select(changeIndex => CreateEvent(customerIndex, changeIndex)));
  }

  public CustomerProfileBulkEvent CreateEvent(int customerIndex, int changeIndex) {
    var customerBusinessKey = CreateBusinessKey(customerIndex);
    var customerNumber = customerIndex.ToString("0000", CultureInfo.InvariantCulture);
    var changeNumber = changeIndex.ToString("00", CultureInfo.InvariantCulture);

    return new CustomerProfileBulkEvent(
        customerBusinessKey,
        "Customer " + customerNumber + " v" + changeNumber,
        changeIndex == 0 ? "prospect" : "active",
        BaseTimestamp.AddMinutes(changeIndex),
        RecordSource,
        "profile-" + customerNumber + "-" + changeNumber);
  }

  public string CreateBusinessKey(int customerIndex) {
    return "C-BULK-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture);
  }
}

internal sealed record CustomerProfileBulkEvent(
    string CustomerBusinessKey,
    string CustomerName,
    string CustomerStatus,
    DateTimeOffset ChangedAtUtc,
    string RecordSource,
    string HashDiff);
