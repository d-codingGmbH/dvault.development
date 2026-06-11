using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfileStreamingChunkedBenchmark(int chunkSize) : CustomerProfileStreamingSaveBenchmarkBase {
  public override string BaselineName =>
      DataVaultBenchmarkHelpers.GetDataVaultBaselineName(Strategy) +
      "/chunked-save-bounded-" +
      ChunkSize.ToString(CultureInfo.InvariantCulture);

  private int ChunkSize { get; } = RequireChunkSize(chunkSize);

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
        var request = new DataVaultChunkedSaveRequest(CreateChunks(scenario.Requests, ChunkSize));
        var result = await saveService.SaveAsync(context, request, cancellationToken).ConfigureAwait(false);

        BenchmarkAssert.Equal(ExpectedRowsWritten, result.RowsWritten, "The chunked streaming-save row count drifted.");
        executionDetail = CreateTelemetryExecutionDetail(
            this,
            AssertSingleSaveSummary(telemetryObserver),
            chunkBoundary: "bounded request chunks",
            ChunkSize);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customer hubs and " +
          ExpectedProfileSatelliteRows.ToString(CultureInfo.InvariantCulture) +
          " profile satellite rows from " +
          RequestCount.ToString(CultureInfo.InvariantCulture) +
          " explicit requests across " +
          ExpectedChunkCount(ChunkSize).ToString(CultureInfo.InvariantCulture) +
          " chunks of " +
          ChunkSize.ToString(CultureInfo.InvariantCulture),
          executionDetail);
    }
    finally {
      await CleanupDatabaseAsync(database, options, providerCapabilities).ConfigureAwait(false);
    }
  }
}
