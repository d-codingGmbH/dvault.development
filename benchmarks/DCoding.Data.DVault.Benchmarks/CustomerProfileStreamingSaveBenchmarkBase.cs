using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal abstract class CustomerProfileStreamingSaveBenchmarkBase : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  protected const int CustomerCount = 20;
  protected const int EventCountPerCustomer = 3;
  protected const int RequestCount = CustomerCount * EventCountPerCustomer;
  protected const int ExpectedProfileSatelliteRows = CustomerCount * 2;
  protected const int ExpectedRowsWritten = CustomerCount + ExpectedProfileSatelliteRows;

  private static readonly DateTimeOffset BaseTimestamp = new(2026, 5, 25, 9, 0, 0, TimeSpan.Zero);

  protected CustomerProfileStreamingSaveBenchmarkBase()
      : this(BenchmarkHashKeyVariant.Default) {
  }

  protected CustomerProfileStreamingSaveBenchmarkBase(BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    Provider = BenchmarkDatabaseProviders.Sqlite;
    Strategy = DataVaultBenchmarkStrategy.ProviderNeutralFallback;
    LoadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault;
    HashKeyVariant = hashKeyVariant;
  }

  public string ScenarioName => "customer-profile-streaming-save";

  public string ProviderName => Provider.ProviderName;

  public abstract string BaselineName { get; }

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(Strategy);

  public string DatasetSize => "20 customers, 60 ordered explicit requests";

  public string ChangeRatio => "3 profile events per customer with one unchanged replay";

  protected BenchmarkDatabaseProvider Provider { get; }

  protected DataVaultBenchmarkStrategy Strategy { get; }

  protected DataVaultLoadTimestampStorage LoadTimestampStorage { get; }

  public BenchmarkHashKeyVariant HashKeyVariant { get; }

  public abstract Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken);

  protected static CustomerProfileStreamingScenario CreateScenario(IServiceProvider provider) {
    ArgumentNullException.ThrowIfNull(provider);

    var customerHashKeys = Enumerable.Range(0, CustomerCount)
        .Select(customerIndex => ComputeCustomerHashKey(provider, CreateBusinessKey(customerIndex)))
        .ToArray();
    var requests = new List<DataVaultSaveRequest>(RequestCount);

    for (var eventIndex = 0; eventIndex < EventCountPerCustomer; eventIndex++) {
      for (var customerIndex = 0; customerIndex < CustomerCount; customerIndex++) {
        var customerEvent = CreateEvent(customerIndex, eventIndex);
        requests.Add(new DataVaultSaveRequest(
            customerEvent.ChangedAtUtc,
            customerEvent.RecordSource,
            eventIndex == 0
                ? [new DataVaultHubSaveOperation(
                    ScenarioContracts.CustomerHub,
                    [new("Customer Id", customerEvent.CustomerBusinessKey)])]
                : [],
            [],
            [CreateSatelliteSaveOperation(customerEvent, customerHashKeys[customerIndex])]));
      }
    }

    return new CustomerProfileStreamingScenario(requests, customerHashKeys);
  }

  protected static async Task InitializeDatabaseAsync(
      IBenchmarkDatabase database,
      DbContextOptions<CustomerProfileStreamingDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileStreamingDataVaultContext(options, providerCapabilities);
    await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
    await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
  }

  protected static async Task CleanupDatabaseAsync(
      IBenchmarkDatabase database,
      DbContextOptions<CustomerProfileStreamingDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    await using var cleanupContext = new CustomerProfileStreamingDataVaultContext(options, providerCapabilities);
    await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
  }

  protected static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileStreamingDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CustomerProfileStreamingScenario scenario,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileStreamingDataVaultContext(options, providerCapabilities);
    var hubRows = await context.Set<Dictionary<string, object>>("HubCustomer")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var profileRows = await context.Set<Dictionary<string, object>>("SatCustomerProfile")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var sampleCustomerIndex = CustomerCount / 2;
    var sampleBusinessKey = CreateBusinessKey(sampleCustomerIndex);
    var sampleCustomerRow = BenchmarkAssert.Single(
        hubRows.Where(row => string.Equals(ReadString(row, "CustomerId"), sampleBusinessKey, StringComparison.Ordinal)),
        "The streaming-save benchmark must persist the sample customer hub row.");
    var sampleCustomerHashKey = ReadString(sampleCustomerRow, "CustomerHashKey");
    var sampleProfileRows = profileRows
        .Where(row => string.Equals(ReadString(row, "CustomerHashKey"), sampleCustomerHashKey, StringComparison.Ordinal))
        .OrderBy(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row))
        .ToArray();

    BenchmarkAssert.Equal(CustomerCount, hubRows.Count, "The streaming-save benchmark must persist every customer hub row.");
    BenchmarkAssert.Equal(
        ExpectedProfileSatelliteRows,
        profileRows.Count,
        "The streaming-save benchmark must skip unchanged replays and persist initial plus changed profile states.");
    BenchmarkAssert.Equal(2, sampleProfileRows.Length, "The streaming-save benchmark must persist two sample profile states.");
    BenchmarkAssert.Equal(
        scenario.CustomerHashKeys[sampleCustomerIndex],
        sampleCustomerHashKey,
        "The streaming-save benchmark customer hash key drifted.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        sampleCustomerHashKey,
        providerCapabilities,
        "Streaming-save customer hub hash key must use the active stable-hash shape.");

    AssertProfileSatelliteRow(sampleProfileRows[0], sampleCustomerHashKey, CreateEvent(sampleCustomerIndex, 0));
    AssertProfileSatelliteRow(sampleProfileRows[1], sampleCustomerHashKey, CreateEvent(sampleCustomerIndex, 2));
  }

  protected static DataVaultSaveTelemetrySummary AssertSingleSaveSummary(CapturingTelemetryObserver telemetryObserver) {
    ArgumentNullException.ThrowIfNull(telemetryObserver);

    var summary = BenchmarkAssert.Single(
        telemetryObserver.SaveSummaries,
        "The streaming-save benchmark must emit exactly one save telemetry summary.");
    telemetryObserver.SaveSummaries.Clear();

    return summary;
  }

  protected static string CreateTelemetryExecutionDetail(
      IScenarioBenchmark benchmark,
      DataVaultSaveTelemetrySummary summary,
      string chunkBoundary,
      int? chunkSize,
      string? savePath = null,
      string? sourceShape = null) {
    ArgumentNullException.ThrowIfNull(benchmark);
    ArgumentNullException.ThrowIfNull(summary);
    ArgumentException.ThrowIfNullOrWhiteSpace(chunkBoundary);

    return BenchmarkExecutionDetails.CreatePlanned(benchmark) +
        "; savePath=" + (savePath ?? "IDataVaultSaveService.SaveAsync(" + summary.OperationKind + ")") +
        "; operationKind=" + summary.OperationKind +
        "; saveStrategyStatus=" + summary.StrategyStatus +
        "; provider=" + (summary.ProviderName ?? "<none>") +
        "; selectedStrategy=" + (summary.SelectedStrategyName ?? "<none>") +
        "; fallbackCauses=" + FormatNames(summary.FallbackCauseKinds) +
        "; requestCount=" + summary.RequestCount.ToString(CultureInfo.InvariantCulture) +
        "; hubOperations=" + summary.HubOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; linkOperations=" + summary.LinkOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; satelliteOperations=" + summary.SatelliteOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; rowsWritten=" + summary.RowsWritten.ToString(CultureInfo.InvariantCulture) +
        "; savedRecordCount=" + summary.SavedRecordCount.ToString(CultureInfo.InvariantCulture) +
        "; chunkBoundary=" + chunkBoundary +
        "; chunkSize=" + (chunkSize?.ToString(CultureInfo.InvariantCulture) ?? "materialized") +
        "; chunkCount=" + summary.ChunkCount.ToString(CultureInfo.InvariantCulture) +
        "; processedChunkCount=" + summary.ProcessedChunkCount.ToString(CultureInfo.InvariantCulture) +
        "; retainedStateHighWater=" + summary.RetainedStateHighWaterCount.ToString(CultureInfo.InvariantCulture) +
        FormatSourceShape(sourceShape) +
        "; chunkedFallbackCauses=" + FormatNames(summary.ChunkedStateFallbackCauseKinds) +
        "; unsupportedShapes=" + FormatNames(summary.UnsupportedShapeKinds);
  }

  protected static int ExpectedChunkCount(int chunkSize) {
    return (RequestCount + chunkSize - 1) / chunkSize;
  }

  protected static int RequireChunkSize(int chunkSize) {
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chunkSize);
    return chunkSize;
  }

  protected static IEnumerable<DataVaultSaveChunk> CreateChunks(
      IReadOnlyList<DataVaultSaveRequest> requests,
      int chunkSize) {
    ArgumentNullException.ThrowIfNull(requests);

    return requests
        .Chunk(chunkSize)
        .Select(chunk => new DataVaultSaveChunk(chunk));
  }

  protected static async IAsyncEnumerable<DataVaultSaveChunk> CreateAsyncChunks(
      IReadOnlyList<DataVaultSaveRequest> requests,
      int chunkSize,
      [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    foreach (var chunk in CreateChunks(requests, chunkSize)) {
      cancellationToken.ThrowIfCancellationRequested();
      await Task.Yield();
      yield return chunk;
    }
  }

  private static DataVaultSatelliteSaveOperation CreateSatelliteSaveOperation(
      CustomerProfileStreamingEvent customerEvent,
      string customerHashKey) {
    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.CustomerProfileSatellite,
        customerHashKey,
        [
            new("customer_name", customerEvent.CustomerName),
            new("customer_status", customerEvent.CustomerStatus),
        ],
        customerEvent.HashDiff);
  }

  private static CustomerProfileStreamingEvent CreateEvent(int customerIndex, int eventIndex) {
    var businessKey = CreateBusinessKey(customerIndex);
    var changedAtUtc = BaseTimestamp
        .AddHours(eventIndex)
        .AddSeconds(customerIndex);
    var initialHashDiff = "profile-stream-initial-" + customerIndex.ToString("000", CultureInfo.InvariantCulture);

    return eventIndex switch {
      0 => new CustomerProfileStreamingEvent(
          businessKey,
          "Customer " + customerIndex.ToString("000", CultureInfo.InvariantCulture),
          "prospect",
          changedAtUtc,
          "streaming-initial",
          initialHashDiff),
      1 => new CustomerProfileStreamingEvent(
          businessKey,
          "Customer " + customerIndex.ToString("000", CultureInfo.InvariantCulture),
          "prospect",
          changedAtUtc,
          "streaming-replay",
          initialHashDiff),
      2 => new CustomerProfileStreamingEvent(
          businessKey,
          "Customer " + customerIndex.ToString("000", CultureInfo.InvariantCulture),
          "active",
          changedAtUtc,
          "streaming-change",
          "profile-stream-changed-" + customerIndex.ToString("000", CultureInfo.InvariantCulture)),
      _ => throw new ArgumentOutOfRangeException(nameof(eventIndex), eventIndex, "Unsupported streaming event index."),
    };
  }

  private static void AssertProfileSatelliteRow(
      Dictionary<string, object> row,
      string customerHashKey,
      CustomerProfileStreamingEvent expected) {
    BenchmarkAssert.Equal(customerHashKey, ReadString(row, "CustomerHashKey"), "Streaming profile satellite parent hash key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, ReadString(row, "CustomerName"), "Streaming profile satellite customer name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, ReadString(row, "CustomerStatus"), "Streaming profile satellite status drifted.");
    BenchmarkAssert.Equal(expected.HashDiff, ReadString(row, "HashDiff"), "Streaming profile satellite hash diff drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, DataVaultBenchmarkHelpers.ReadLoadTimestamp(row), "Streaming profile satellite load timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, ReadString(row, "RecordSource"), "Streaming profile satellite record source drifted.");
  }

  private static string CreateBusinessKey(int customerIndex) {
    return "C-STREAM-" + customerIndex.ToString("000", CultureInfo.InvariantCulture);
  }

  private static string ComputeCustomerHashKey(
      IServiceProvider provider,
      string customerBusinessKey) {
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    var hashService = provider.GetRequiredService<IStableHashService>();
    var normalized = normalizer.NormalizeFields(
        [new KeyValuePair<string, object?>("Customer Id", customerBusinessKey)]);

    return hashService.ComputeHash(normalized).Value;
  }

  private static string ReadString(Dictionary<string, object> row, string columnName) {
    return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("Expected column '" + columnName + "' to contain a non-null value.");
  }

  private static string FormatNames<TValue>(IEnumerable<TValue> values) {
    var names = values.Select(value => value?.ToString()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
    return names.Length == 0 ? "none" : string.Join("|", names);
  }

  private static string FormatSourceShape(string? sourceShape) {
    return string.IsNullOrWhiteSpace(sourceShape) ? string.Empty : "; sourceShape=" + sourceShape;
  }

  protected sealed class CapturingTelemetryObserver : IDataVaultTelemetryObserver {
    public List<DataVaultSaveTelemetrySummary> SaveSummaries { get; } = [];

    public void RecordSave(DataVaultSaveTelemetrySummary summary) {
      SaveSummaries.Add(summary);
    }

    public void RecordRead(DataVaultReadTelemetrySummary summary) {
    }
  }
}
