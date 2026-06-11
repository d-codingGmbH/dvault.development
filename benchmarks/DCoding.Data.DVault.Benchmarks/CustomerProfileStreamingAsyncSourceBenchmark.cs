using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfileStreamingAsyncSourceBenchmark(int chunkSize) : CustomerProfileStreamingSaveBenchmarkBase {
  private const string AsyncSourceShape = "IAsyncEnumerable<DataVaultSaveChunk>";

  public override string BaselineName =>
      DataVaultBenchmarkHelpers.GetDataVaultBaselineName(Strategy) +
      "/async-source-bounded-" +
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
        var chunks = CreateAsyncChunks(scenario.Requests, ChunkSize, cancellationToken);
        var result = await saveService.SaveAsync(context, chunks, cancellationToken).ConfigureAwait(false);

        BenchmarkAssert.Equal(ExpectedRowsWritten, result.RowsWritten, "The async streaming-save row count drifted.");
        executionDetail = CreateTelemetryExecutionDetail(
            this,
            AssertSingleSaveSummary(telemetryObserver),
            chunkBoundary: "async bounded request chunks",
            ChunkSize,
            savePath: "IDataVaultSaveService.SaveAsync(" + AsyncSourceShape + ")",
            sourceShape: AsyncSourceShape);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customer hubs and " +
          ExpectedProfileSatelliteRows.ToString(CultureInfo.InvariantCulture) +
          " profile satellite rows from " +
          RequestCount.ToString(CultureInfo.InvariantCulture) +
          " async-streamed explicit requests across " +
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
