using System.Globalization;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class PitFullRebuildMaintenanceBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  private const string Scenario = "pit-full-rebuild-maintenance";
  private const string MaintenanceScope = "FullRebuild";
  private const string PitShapeBoundary = "clean-ordinary-hub-parent";
  private const int StalePitRowsPerCustomer = 1;

  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;
  private readonly CustomerProfileBulkScenarioDefinition _scenario = CustomerProfileBulkScenarios.ChangeHeavy;

  public PitFullRebuildMaintenanceBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage)
      : this(provider, strategy, loadTimestampStorage, BenchmarkHashKeyVariant.Default) {
  }

  public PitFullRebuildMaintenanceBenchmark(
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

  public string ScenarioName => Scenario;

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

  public string DatasetSize =>
      _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
      " customers, " +
      ExpectedRowsWritten.ToString(CultureInfo.InvariantCulture) +
      " rebuilt PIT rows, 2 satellite segments";

  public string ChangeRatio => "full rebuild after 90% repeat-change history plus status snapshot";

  internal string ExecutionPathDetail => _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
      ? "DVault SQL Server PIT full-rebuild maintenance path; maintenanceScope=" + MaintenanceScope +
          "; selectedStrategy=SqlServerDataVaultPitMaintenanceService; pitShapeBoundary=" + PitShapeBoundary
      : "DVault provider-neutral PIT full-rebuild maintenance path; maintenanceScope=" + MaintenanceScope +
          "; selectedStrategy=<none>; pitShapeBoundary=" + PitShapeBoundary;

  private int ExpectedRowsDeleted => _scenario.CustomerCount * StalePitRowsPerCustomer;

  private int ExpectedRowsWritten => _scenario.CustomerCount * (_scenario.ChangeCount + 1);

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<PitAsOfReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy, _hashKeyVariant);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();

    try {
      await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
      }

      var customerHashKeys = await ReadBenchmarkServices
          .SeedCustomerProfileHistoryAsync(
              options,
              providerCapabilities,
              saveService,
              _scenario,
              cancellationToken)
          .ConfigureAwait(false);
      await SeedStatusHistoryAsync(
          options,
          providerCapabilities,
          saveService,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);
      await SeedStalePitRowsAsync(
          options,
          providerCapabilities,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);

      DataVaultPitMaintenanceResult? maintenanceResult = null;
      var executionDetail = BenchmarkExecutionDetails.CreatePlanned(this);
      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new PitAsOfReadContext(options, providerCapabilities);
        maintenanceResult = await maintenanceService
            .RebuildAsync(
                context,
                new DataVaultPitRebuildRequest(PitReadScenario.Metadata.Pit),
                cancellationToken)
            .ConfigureAwait(false);
        executionDetail = CreateCompletedExecutionDetail(
            context.Database.ProviderName,
            maintenanceService.GetType().Name,
            maintenanceResult);
      }).ConfigureAwait(false);

      VerifyMaintenanceResult(maintenanceResult);
      await VerifyRebuiltPitRowsAsync(
          options,
          providerCapabilities,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          ExpectedRowsDeleted.ToString(CultureInfo.InvariantCulture) +
          " stale PIT rows replaced by " +
          ExpectedRowsWritten.ToString(CultureInfo.InvariantCulture) +
          " rebuilt PIT rows for " +
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customers",
          executionDetail);
    }
    finally {
      await using var cleanupContext = new PitAsOfReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private async Task SeedStatusHistoryAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    var statusTimestamp = _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount);
    await using var context = new PitAsOfReadContext(options, providerCapabilities);
    var statusRequests = Enumerable.Range(0, _scenario.CustomerCount)
        .Select(customerIndex => new DataVaultSaveRequest(
            statusTimestamp,
            _scenario.RecordSource,
            [],
            [],
            [
                new DataVaultSatelliteSaveOperation(
                    PitReadScenario.Metadata.Status,
                    customerHashKeys[customerIndex],
                    [new("status_code", customerIndex % 2 == 0 ? "Active" : "Review")],
                    "status-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture)),
            ]))
        .ToArray();

    await saveService
        .SaveAsync(context, new DataVaultBulkSaveRequest(statusRequests), cancellationToken)
        .ConfigureAwait(false);
  }

  private async Task SeedStalePitRowsAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    await using var context = new PitAsOfReadContext(options, providerCapabilities);
    var pitRows = context.Set<Dictionary<string, object>>("PitCustomerProfileStatus");
    var storedPitTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
        providerCapabilities,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        PitReadScenario.PitTimestamp);
    var storedProfileTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
        providerCapabilities,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        _scenario.BaseTimestamp);

    foreach (var customerHashKey in customerHashKeys) {
      pitRows.Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["CustomerHashKey"] = customerHashKey,
        ["LoadTimestamp"] = storedPitTimestamp,
        ["ProfileLoadTimestamp"] = storedProfileTimestamp,
        ["StatusLoadTimestamp"] = null!,
      });
    }

    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  private void VerifyMaintenanceResult(DataVaultPitMaintenanceResult? maintenanceResult) {
    if (maintenanceResult is null) {
      throw new InvalidOperationException("The PIT maintenance benchmark did not capture a maintenance result.");
    }

    BenchmarkAssert.Equal(
        _scenario.CustomerCount,
        maintenanceResult.ParentHashKeyCount,
        "The PIT full-rebuild benchmark parent count drifted.");
    BenchmarkAssert.Equal(
        ExpectedRowsDeleted,
        maintenanceResult.RowsDeleted,
        "The PIT full-rebuild benchmark must delete the seeded stale PIT rows.");
    BenchmarkAssert.Equal(
        ExpectedRowsWritten,
        maintenanceResult.RowsWritten,
        "The PIT full-rebuild benchmark rebuilt row count drifted.");
    BenchmarkAssert.Equal(
        "PitCustomerProfileStatus",
        maintenanceResult.TableName,
        "The PIT full-rebuild benchmark table name drifted.");
  }

  private async Task VerifyRebuiltPitRowsAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    await using var context = new PitAsOfReadContext(options, providerCapabilities);
    var rows = await context.Set<Dictionary<string, object>>("PitCustomerProfileStatus")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    BenchmarkAssert.Equal(
        ExpectedRowsWritten,
        rows.Count,
        "The PIT full-rebuild benchmark must persist the expected rebuilt PIT row count.");
    BenchmarkAssert.True(
        !rows.Any(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row) == PitReadScenario.PitTimestamp),
        "The PIT full-rebuild benchmark must replace the seeded stale PIT timestamp.");

    var sampleHashKey = customerHashKeys[_scenario.SampleCustomerIndex];
    var sampleRows = rows
        .Where(row => string.Equals(ReadString(row, "CustomerHashKey"), sampleHashKey, StringComparison.Ordinal))
        .OrderBy(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row))
        .ToArray();
    BenchmarkAssert.Equal(
        _scenario.ChangeCount + 1,
        sampleRows.Length,
        "The PIT full-rebuild benchmark must rebuild each sample customer timeline point.");

    var finalRow = sampleRows[^1];
    BenchmarkAssert.Equal(
        _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount),
        DataVaultBenchmarkHelpers.ReadLoadTimestamp(finalRow),
        "The PIT full-rebuild benchmark final PIT load timestamp drifted.");
    BenchmarkAssert.Equal(
        _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount - 1),
        DataVaultBenchmarkHelpers.ReadLoadTimestamp(finalRow, "ProfileLoadTimestamp"),
        "The PIT full-rebuild benchmark profile snapshot timestamp drifted.");
    BenchmarkAssert.Equal(
        _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount),
        DataVaultBenchmarkHelpers.ReadLoadTimestamp(finalRow, "StatusLoadTimestamp"),
        "The PIT full-rebuild benchmark status snapshot timestamp drifted.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        sampleHashKey,
        providerCapabilities,
        "The PIT full-rebuild benchmark parent hash key must use the active stable-hash shape.");
  }

  private string CreateCompletedExecutionDetail(
      string? providerName,
      string selectedMaintenanceServiceName,
      DataVaultPitMaintenanceResult result) {
    var selectedStrategy = _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
        ? selectedMaintenanceServiceName
        : "<none>";
    var strategyStatus = _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
        ? "ProviderStrategySelected"
        : "ProviderNeutralFallback";
    var fallbackCauses = _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
        ? "none"
        : "NoProviderSpecificStrategyRegistered";

    return BenchmarkExecutionDetails.CreatePlanned(this) +
        "; maintenanceStrategyStatus=" + strategyStatus +
        "; provider=" + (providerName ?? "<none>") +
        "; maintenanceService=" + selectedMaintenanceServiceName +
        "; selectedStrategy=" + selectedStrategy +
        "; fallbackCauses=" + fallbackCauses +
        "; parentHashKeys=" + result.ParentHashKeyCount.ToString(CultureInfo.InvariantCulture) +
        "; rowsDeleted=" + result.RowsDeleted.ToString(CultureInfo.InvariantCulture) +
        "; rowsWritten=" + result.RowsWritten.ToString(CultureInfo.InvariantCulture);
  }

  private static string ReadString(Dictionary<string, object> row, string columnName) {
    return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("Expected column '" + columnName + "' to contain a non-null value.");
  }
}
