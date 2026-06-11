using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class LatestSatelliteReadBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly CustomerProfileBulkScenarioDefinition _scenario = CustomerProfileBulkScenarios.ChangeHeavy;

  public LatestSatelliteReadBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(provider);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
  }

  public string ScenarioName => "latest-satellite-read";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize => _scenario.DatasetSize;

  public string ChangeRatio => "90% repeat-change history latest read";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    try {
      await using (var context = new CustomerProfileReadContext(options, providerCapabilities)) {
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
      IReadOnlyList<DataVaultSatelliteReadRecord> readRows = [];
      var request = new DataVaultLatestSatelliteReadRequest(
          ScenarioContracts.CustomerProfileSatellite,
          customerHashKeys);
      DataVaultDiagnosticsResult diagnostics;
      await using (var diagnosticsContext = new CustomerProfileReadContext(options, providerCapabilities)) {
        diagnostics = readDiagnostics.Analyze(diagnosticsContext, request);
        ReadBenchmarkServices.AssertReadStrategySelection(
            _strategy,
            ScenarioName,
            diagnostics);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new CustomerProfileReadContext(options, providerCapabilities);
        readRows = await readService
            .ReadLatestSatelliteRowsAsync(
                context,
                request,
                cancellationToken)
            .ConfigureAwait(false);
      }).ConfigureAwait(false);

      VerifyLatestRows(readRows, customerHashKeys);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " latest profile satellite rows read from " +
          _scenario.TotalChangeCount.ToString(CultureInfo.InvariantCulture) +
          " seeded profile states",
          BenchmarkExecutionDetails.CreateReadStrategyDetail(this, diagnostics));
    }
    finally {
      await using var cleanupContext = new CustomerProfileReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private void VerifyLatestRows(
      IReadOnlyList<DataVaultSatelliteReadRecord> readRows,
      IReadOnlyList<string> customerHashKeys) {
    BenchmarkAssert.Equal(
        _scenario.CustomerCount,
        readRows.Count,
        "The latest satellite read benchmark must return one row per seeded customer.");

    var sampleHashKey = customerHashKeys[_scenario.SampleCustomerIndex];
    var sampleRow = BenchmarkAssert.Single(
        readRows.Where(row => string.Equals(row.ParentHashKey, sampleHashKey, StringComparison.Ordinal)),
        "The latest satellite read benchmark must return the sample customer row.");
    var expected = _scenario.CreateEvent(_scenario.SampleCustomerIndex, _scenario.ChangeCount - 1);

    BenchmarkAssert.Equal("Profile", sampleRow.MetadataName, "The latest satellite read metadata name drifted.");
    BenchmarkAssert.Equal("SatCustomerProfile", sampleRow.TableName, "The latest satellite read table name drifted.");
    BenchmarkAssert.Equal(expected.HashDiff, sampleRow.HashDiff, "The latest satellite read hash diff drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, sampleRow.LoadTimestamp, "The latest satellite read timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, sampleRow.RecordSource, "The latest satellite read record source drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, sampleRow.PayloadValues["customer_name"], "The latest satellite read name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, sampleRow.PayloadValues["customer_status"], "The latest satellite read status drifted.");
    BenchmarkAssert.True(
        readRows.All(row => customerHashKeys.Contains(row.ParentHashKey, StringComparer.Ordinal)),
        "The latest satellite read benchmark returned an unseeded parent hash key.");
  }
}
