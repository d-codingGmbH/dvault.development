using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfileDataVaultBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;

  public CustomerProfileDataVaultBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage)
      : this(provider, strategy, loadTimestampStorage, BenchmarkHashKeyVariant.Default) {
  }

  public CustomerProfileDataVaultBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
    _hashKeyVariant = hashKeyVariant;
  }

  public string ScenarioName => "customer-profile-history";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

  public string DatasetSize => "1 customer, 2 profile states";

  public string ChangeRatio => "50% repeat-change history";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileDataVaultContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy, _hashKeyVariant);

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
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        customerHashKey,
        providerCapabilities,
        "Customer hub hash key must use the active stable-hash shape.");
    DataVaultBenchmarkHelpers.AssertHashKeyStorageMapping(
        context,
        "HubCustomer",
        "CustomerHashKey",
        providerCapabilities,
        "Customer hub hash key must project the active storage profile.");
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
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel(), ProviderCapabilities);
    }
  }
}
