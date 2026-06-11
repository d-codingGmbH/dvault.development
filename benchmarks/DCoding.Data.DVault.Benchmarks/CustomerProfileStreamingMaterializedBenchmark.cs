using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfileStreamingMaterializedBenchmark : CustomerProfileStreamingSaveBenchmarkBase {
  public override string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(Strategy) + "/materialized-explicit-bulk";

  public override async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = Provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileStreamingDataVaultContext>();
    var providerCapabilities = Provider.GetProviderCapabilities(LoadTimestampStorage);
    var telemetryObserver = new CapturingTelemetryObserver();
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, Strategy);
    services.AddSingleton<IDataVaultTelemetryObserver>(telemetryObserver);

    using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
    var saveService = serviceProvider.GetRequiredService<IDataVaultSaveService>();
    var scenario = CreateScenario(serviceProvider);

    try {
      await InitializeDatabaseAsync(database, options, providerCapabilities, cancellationToken).ConfigureAwait(false);

      var executionDetail = BenchmarkExecutionDetails.CreatePlanned(this);
      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new CustomerProfileStreamingDataVaultContext(options, providerCapabilities);
        var request = new DataVaultBulkSaveRequest(scenario.Requests);
        var result = await saveService.SaveAsync(context, request, cancellationToken).ConfigureAwait(false);

        BenchmarkAssert.Equal(ExpectedRowsWritten, result.RowsWritten, "The materialized streaming-save row count drifted.");
        executionDetail = CreateTelemetryExecutionDetail(
            this,
            AssertSingleSaveSummary(telemetryObserver),
            chunkBoundary: "materialized ordered request set",
            chunkSize: null);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customer hubs and " +
          ExpectedProfileSatelliteRows.ToString(CultureInfo.InvariantCulture) +
          " profile satellite rows from " +
          RequestCount.ToString(CultureInfo.InvariantCulture) +
          " materialized explicit requests",
          executionDetail);
    }
    finally {
      await CleanupDatabaseAsync(database, options, providerCapabilities).ConfigureAwait(false);
    }
  }
}
