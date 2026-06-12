using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class PitAsOfReadBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;
  private readonly CustomerProfileBulkScenarioDefinition _scenario = CustomerProfileBulkScenarios.ChangeHeavy;

  public PitAsOfReadBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage)
      : this(provider, strategy, loadTimestampStorage, BenchmarkHashKeyVariant.Default) {
  }

  public PitAsOfReadBenchmark(
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

  public string ScenarioName => "pit-as-of-read";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

  public string DatasetSize => "100 customers, 100 PIT rows, 2 satellite segments";

  public string ChangeRatio => "as-of read after latest profile/status snapshots";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<PitAsOfReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy, _hashKeyVariant);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    try {
      await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var customerHashKeys = await ReadBenchmarkServices
          .SeedCustomerProfileHistoryAsync(
              options,
              providerCapabilities,
              saveService,
              _scenario,
              cancellationToken)
          .ConfigureAwait(false);
      await SeedStatusAndPitRowsAsync(
          options,
          providerCapabilities,
          saveService,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);

      IReadOnlyList<DataVaultPitReadRecord> readRows = [];
      var request = new DataVaultPitAsOfReadRequest(
          PitReadScenario.Metadata.Pit,
          customerHashKeys,
          PitReadScenario.AsOf);
      DataVaultDiagnosticsResult diagnostics;
      await using (var diagnosticsContext = new PitAsOfReadContext(options, providerCapabilities)) {
        diagnostics = readDiagnostics.Analyze(diagnosticsContext, request);
        ReadBenchmarkServices.AssertReadStrategySelection(
            _strategy,
            ScenarioName,
            diagnostics);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new PitAsOfReadContext(options, providerCapabilities);
        readRows = await readService
            .ReadPitRowsAsync(
                context,
                request,
                cancellationToken)
            .ConfigureAwait(false);
      }).ConfigureAwait(false);

      VerifyPitRows(readRows, customerHashKeys);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " PIT as-of rows read across profile and status satellite snapshots",
          BenchmarkExecutionDetails.CreateReadStrategyDetail(this, diagnostics));
    }
    finally {
      await using var cleanupContext = new PitAsOfReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private async Task SeedStatusAndPitRowsAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    var statusTimestamp = _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount);
    await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
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

    await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
      var pitRows = context.Set<Dictionary<string, object>>("PitCustomerProfileStatus");
      var profileSnapshotTimestamp = _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount - 1);
      var storedPitTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
          providerCapabilities,
          DataVaultLogicalPropertyKind.LoadTimestamp,
          PitReadScenario.PitTimestamp);
      var storedProfileTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
          providerCapabilities,
          DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
          profileSnapshotTimestamp);
      var storedStatusTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
          providerCapabilities,
          DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
          statusTimestamp);

      foreach (var customerHashKey in customerHashKeys) {
        pitRows.Add(new Dictionary<string, object>(StringComparer.Ordinal) {
          ["CustomerHashKey"] = customerHashKey,
          ["LoadTimestamp"] = storedPitTimestamp,
          ["ProfileLoadTimestamp"] = storedProfileTimestamp,
          ["StatusLoadTimestamp"] = storedStatusTimestamp,
        });
      }

      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
  }

  private void VerifyPitRows(
      IReadOnlyList<DataVaultPitReadRecord> readRows,
      IReadOnlyList<string> customerHashKeys) {
    BenchmarkAssert.Equal(
        _scenario.CustomerCount,
        readRows.Count,
        "The PIT read benchmark must return one row per seeded customer.");

    var sampleHashKey = customerHashKeys[_scenario.SampleCustomerIndex];
    var sampleRow = BenchmarkAssert.Single(
        readRows.Where(row => string.Equals(row.ParentHashKey, sampleHashKey, StringComparison.Ordinal)),
        "The PIT read benchmark must return the sample customer row.");
    var profile = sampleRow.SatelliteSnapshotsByName["Profile"];
    var status = sampleRow.SatelliteSnapshotsByName["Status"];
    var expectedProfile = _scenario.CreateEvent(_scenario.SampleCustomerIndex, _scenario.ChangeCount - 1);

    BenchmarkAssert.Equal(PitReadScenario.PitTimestamp, sampleRow.LoadTimestamp, "The PIT read load timestamp drifted.");
    BenchmarkAssert.True(profile.IsPresent, "The PIT read benchmark must materialize a profile snapshot.");
    BenchmarkAssert.True(status.IsPresent, "The PIT read benchmark must materialize a status snapshot.");
    BenchmarkAssert.Equal(expectedProfile.HashDiff, profile.HashDiff, "The PIT read profile hash diff drifted.");
    BenchmarkAssert.Equal(expectedProfile.CustomerName, profile.PayloadValues["customer_name"], "The PIT read profile name drifted.");
    BenchmarkAssert.Equal(expectedProfile.CustomerStatus, profile.PayloadValues["customer_status"], "The PIT read profile status drifted.");
    BenchmarkAssert.Equal("status-0042", status.HashDiff, "The PIT read status hash diff drifted.");
    BenchmarkAssert.Equal("Active", status.PayloadValues["status_code"], "The PIT read status code drifted.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        sampleHashKey,
        _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant),
        "The PIT read parent hash key must use the active stable-hash shape.");
  }
}
