using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfileBulkDataVaultBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly CustomerProfileBulkScenarioDefinition _scenario;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;

  public CustomerProfileBulkDataVaultBenchmark(
      BenchmarkDatabaseProvider provider,
      CustomerProfileBulkScenarioDefinition scenario,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(scenario);

    _provider = provider;
    _scenario = scenario;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
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
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();

    try {
      await using (var context = new CustomerProfileBulkDataVaultContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var executionDetail = BenchmarkExecutionDetails.CreatePlanned(this);
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
        var satelliteBulkRequest = new DataVaultBulkSaveRequest(satelliteRequests);
        var strategyDiagnostics = diagnostics.Analyze(context, satelliteBulkRequest);
        executionDetail = BenchmarkExecutionDetails.CreateSaveStrategyDetail(
            this,
            strategyDiagnostics,
            satelliteRequests.Length,
            hubOperationCount: 0,
            linkOperationCount: 0,
            satelliteOperationCount: _scenario.TotalChangeCount);

        await saveService.SaveAsync(
            context,
            satelliteBulkRequest,
            cancellationToken).ConfigureAwait(false);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, _scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customer hubs and " +
          _scenario.TotalChangeCount.ToString(CultureInfo.InvariantCulture) +
          " profile satellite rows",
          executionDetail);
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
