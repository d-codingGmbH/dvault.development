using System.Diagnostics;
using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultActivityTracingTests {
  private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 20, 8, 30, 0, TimeSpan.Zero);

  [Fact]
  public async Task SaveActivityTracingEmitsOneSpanForEachPublicSaveBoundary() {
    using var capture = new DataVaultActivityCapture();
    var saveService = new DefaultDataVaultSaveService(
        new TestStableHashService(),
        new TestStableHashNormalizer(),
        [DefaultDataVaultLoadTimestampResolver.Instance],
        [DefaultDataVaultRecordSourceResolver.Instance],
        [new ReturningSaveStrategy()],
        []);
    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var singleRequest = CreateMixedSaveRequest("crm-import");
    var bulkRequest = new DataVaultBulkSaveRequest([
        singleRequest,
        CreateHubOnlySaveRequest("crm-replay"),
    ]);
    var chunkedRequest = new DataVaultChunkedSaveRequest([
        new DataVaultSaveChunk([singleRequest]),
        new DataVaultSaveChunk([CreateHubOnlySaveRequest("crm-replay")]),
    ]);
    var asyncChunks = CreateAsyncChunks([
        new DataVaultSaveChunk([singleRequest]),
        new DataVaultSaveChunk([CreateHubOnlySaveRequest("crm-replay")]),
    ]);

    await saveService.SaveAsync(context, singleRequest);
    await saveService.SaveAsync(context, bulkRequest);
    await saveService.SaveAsync(context, chunkedRequest);
    await saveService.SaveAsync(context, asyncChunks);

    Assert.Collection(
        capture.Activities,
        activity => AssertSaveActivity(
            activity,
            DataVaultActivityTracing.SaveSingleRequestOperation,
            DataVaultSaveTelemetryOperationKind.SingleRequest,
            requestCount: 1,
            operationCount: 3,
            rowCount: 7,
            chunkCount: null,
            processedChunkCount: null),
        activity => AssertSaveActivity(
            activity,
            DataVaultActivityTracing.SaveBulkRequestOperation,
            DataVaultSaveTelemetryOperationKind.BulkRequest,
            requestCount: 2,
            operationCount: 4,
            rowCount: 7,
            chunkCount: null,
            processedChunkCount: null),
        activity => AssertSaveActivity(
            activity,
            DataVaultActivityTracing.SaveChunkedRequestOperation,
            DataVaultSaveTelemetryOperationKind.ChunkedRequest,
            requestCount: 2,
            operationCount: 4,
            rowCount: 14,
            chunkCount: 2,
            processedChunkCount: 2),
        activity => AssertSaveActivity(
            activity,
            DataVaultActivityTracing.SaveChunkedRequestOperation,
            DataVaultSaveTelemetryOperationKind.ChunkedRequest,
            requestCount: 2,
            operationCount: 4,
            rowCount: 14,
            chunkCount: 2,
            processedChunkCount: 2));
  }

  [Fact]
  public async Task ReadActivityTracingEmitsOneTerminalSpanForDefaultReadPaths() {
    using var capture = new DataVaultActivityCapture();
    IDataVaultReadService readService = new DefaultDataVaultReadService(
        [new ReturningLatestSatelliteReadStrategy()],
        [new ReturningPitReadStrategy()],
        [new ReturningBridgeReadStrategy()]);
    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var latestRequest = CreateLatestSatelliteRequest(["customer-hk"]);
    var asOfRequest = CreateLatestSatelliteRequest(["customer-hk"], LoadTimestamp);
    var pitRequest = CreatePitReadRequest(["customer-hk"]);
    var bridgeRequest = CreateBridgeReadRequest(["customer-hk"]);

    await readService.ReadLatestSatelliteRowsAsync(context, latestRequest);
    await readService.ReadLatestSatelliteAsync(
        context,
        asOfRequest,
        row => row.RequiredString("Name"));
    await readService.ReadPitAsync(
        context,
        pitRequest,
        row => row.RequiredString("ParentHashKey"));
    await readService.ReadBridgeRowsAsync(context, bridgeRequest);
    await readService.ReadBridgeAsync(
        context,
        bridgeRequest,
        row => row.RequiredString("CustomerHashKey"));

    Assert.Collection(
        capture.Activities,
        activity => AssertReadActivity(
            activity,
            DataVaultActivityTracing.ReadLatestSatelliteOperation,
            DataVaultReadTelemetryFamily.LatestSatellite,
            DataVaultActivityTracing.ReadModeCurrent),
        activity => AssertReadActivity(
            activity,
            DataVaultActivityTracing.ReadLatestSatelliteOperation,
            DataVaultReadTelemetryFamily.LatestSatellite,
            DataVaultActivityTracing.ReadModeAsOf),
        activity => AssertReadActivity(
            activity,
            DataVaultActivityTracing.ReadPitOperation,
            DataVaultReadTelemetryFamily.Pit,
            DataVaultActivityTracing.ReadModeAsOf),
        activity => AssertReadActivity(
            activity,
            DataVaultActivityTracing.ReadBridgeOperation,
            DataVaultReadTelemetryFamily.Bridge,
            DataVaultActivityTracing.ReadModeTraversal),
        activity => AssertReadActivity(
            activity,
            DataVaultActivityTracing.ReadBridgeOperation,
            DataVaultReadTelemetryFamily.Bridge,
            DataVaultActivityTracing.ReadModeTraversal));
  }

  [Fact]
  public async Task BridgeActivityTracingCoversDirectPipelineFallbackFailuresWithoutRawFailureText() {
    using var capture = new DataVaultActivityCapture();
    IDataVaultReadService readService = new ExternalReadService();
    await using var context = new EmptyBridgeModelContext(
        new DbContextOptionsBuilder<EmptyBridgeModelContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);
    var request = CreateBridgeReadRequest(["customer-hk"]);

    await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadBridgeRowsAsync(context, request));
    await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadBridgeAsync(
            context,
            request,
            row => row.RequiredString("CustomerHashKey")));

    Assert.Collection(
        capture.Activities,
        AssertBridgeFallbackFailureActivity,
        AssertBridgeFallbackFailureActivity);
  }

  private static void AssertSaveActivity(
      Activity activity,
      string operationName,
      DataVaultSaveTelemetryOperationKind saveMode,
      int requestCount,
      int operationCount,
      int rowCount,
      int? chunkCount,
      int? processedChunkCount) {
    var tags = GetTags(activity);

    Assert.Equal(operationName, activity.OperationName);
    Assert.Equal(ActivityKind.Internal, activity.Kind);
    Assert.Equal(ActivityStatusCode.Ok, activity.Status);
    Assert.Equal(operationName, tags["dvault.operation"]);
    Assert.Equal("success", tags["dvault.outcome"]);
    Assert.Equal(saveMode.ToString(), tags["dvault.save.mode"]);
    Assert.Equal(requestCount, tags["dvault.request.count"]);
    Assert.Equal(operationCount, tags["dvault.operation.count"]);
    Assert.Equal(rowCount, tags["dvault.row.count"]);
    Assert.Equal("ProviderStrategySelected", tags["dvault.strategy.status"]);
    Assert.Equal(nameof(ReturningSaveStrategy), tags["dvault.strategy.type"]);
    Assert.True(tags.ContainsKey("dvault.duration.bucket"));

    if (chunkCount is null) {
      Assert.False(tags.ContainsKey("dvault.chunk.count"));
      Assert.False(tags.ContainsKey("dvault.processed_chunk.count"));
    }
    else {
      Assert.Equal(chunkCount.Value, tags["dvault.chunk.count"]);
      Assert.Equal(processedChunkCount!.Value, tags["dvault.processed_chunk.count"]);
    }
  }

  private static void AssertReadActivity(
      Activity activity,
      string operationName,
      DataVaultReadTelemetryFamily family,
      string readMode) {
    var tags = GetTags(activity);

    Assert.Equal(operationName, activity.OperationName);
    Assert.Equal(ActivityKind.Internal, activity.Kind);
    Assert.Equal(ActivityStatusCode.Ok, activity.Status);
    Assert.Equal(operationName, tags["dvault.operation"]);
    Assert.Equal("success", tags["dvault.outcome"]);
    Assert.Equal(family.ToString(), tags["dvault.read.family"]);
    Assert.Equal(readMode, tags["dvault.read.mode"]);
    Assert.Equal(1, tags["dvault.requested_key.count"]);
    Assert.Equal(1, tags["dvault.returned_row.count"]);
    Assert.Equal("ProviderStrategySelected", tags["dvault.strategy.status"]);
    Assert.NotNull(tags["dvault.strategy.type"]);
    Assert.True(tags.ContainsKey("dvault.duration.bucket"));
  }

  private static void AssertBridgeFallbackFailureActivity(Activity activity) {
    var tags = GetTags(activity);

    Assert.Equal(DataVaultActivityTracing.ReadBridgeOperation, activity.OperationName);
    Assert.Equal(ActivityKind.Internal, activity.Kind);
    Assert.Equal(ActivityStatusCode.Error, activity.Status);
    Assert.Equal(DataVaultActivityTracing.ReadBridgeOperation, tags["dvault.operation"]);
    Assert.Equal("fault", tags["dvault.outcome"]);
    Assert.Equal("fault", tags["dvault.failure.kind"]);
    Assert.Equal("unsupported_shape", tags["dvault.failure.class"]);
    Assert.Equal(nameof(InvalidOperationException), tags["dvault.exception.type"]);
    Assert.Equal(DataVaultReadTelemetryFamily.Bridge.ToString(), tags["dvault.read.family"]);
    Assert.Equal(DataVaultActivityTracing.ReadModeTraversal, tags["dvault.read.mode"]);
    Assert.All(
        tags.Values,
        value => Assert.DoesNotContain("BridgeCustomerOrder", value?.ToString() ?? string.Empty, StringComparison.Ordinal));
  }

  private static IReadOnlyDictionary<string, object?> GetTags(Activity activity) {
    return activity.TagObjects.ToDictionary(tag => tag.Key, tag => tag.Value, StringComparer.Ordinal);
  }

  private static DataVaultSaveRequest CreateMixedSaveRequest(string recordSource) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var profile = new DataVaultSatelliteMetadata("Profile", customer.ToReference(), ["Name"]);

    return new DataVaultSaveRequest(
        LoadTimestamp,
        recordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-100")])],
        [new DataVaultLinkSaveOperation(customerOrder, [new("Customer", "customer-hk"), new("Order", "order-hk")])],
        [new DataVaultSatelliteSaveOperation(profile, "customer-hk", [new("Name", "Alice")], "profile-hash")]);
  }

  private static DataVaultSaveRequest CreateHubOnlySaveRequest(string recordSource) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultSaveRequest(
        LoadTimestamp.AddMinutes(5),
        recordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-200")])],
        []);
  }

  private static async IAsyncEnumerable<DataVaultSaveChunk> CreateAsyncChunks(
      IReadOnlyList<DataVaultSaveChunk> chunks,
      [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    foreach (var chunk in chunks) {
      cancellationToken.ThrowIfCancellationRequested();
      await Task.Yield();
      yield return chunk;
    }
  }

  private static DataVaultLatestSatelliteReadRequest CreateLatestSatelliteRequest(
      IEnumerable<string> parentHashKeys,
      DateTimeOffset? asOf = null) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata("Profile", customer.ToReference(), ["Name"]);

    return new DataVaultLatestSatelliteReadRequest(profile, parentHashKeys, asOf);
  }

  private static DataVaultPitAsOfReadRequest CreatePitReadRequest(IEnumerable<string> parentHashKeys) {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

    return new DataVaultPitAsOfReadRequest(pit, parentHashKeys, LoadTimestamp);
  }

  private static DataVaultBridgeReadRequest CreateBridgeReadRequest(IEnumerable<string> endpointHashKeys) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    return new DataVaultBridgeReadRequest(
        bridge,
        DataVaultBridgeTraversalEndpoint.From,
        endpointHashKeys);
  }

  private sealed class DataVaultActivityCapture : IDisposable {
    private readonly ActivityListener _listener;

    public DataVaultActivityCapture() {
      _listener = new ActivityListener {
        ShouldListenTo = source => source.Name == DataVaultActivityTracing.SourceName,
        Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
        ActivityStopped = activity => Activities.Add(activity),
      };
      ActivitySource.AddActivityListener(_listener);
    }

    public List<Activity> Activities { get; } = [];

    public void Dispose() {
      _listener.Dispose();
    }
  }

  private sealed class ReturningSaveStrategy : IDataVaultProviderSaveStrategy {
    public int Priority => 100;

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(requests);

      return true;
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult(new DataVaultSaveResult(
          7,
          [new DataVaultSavedRecord(DataVaultTableKind.Hub, "Customer", "HubCustomer", "customer-hk")]));
    }
  }

  private sealed class ReturningLatestSatelliteReadStrategy : IDataVaultProviderReadStrategy {
    public int Priority => 100;

    public bool CanReadLatestSatelliteRows(
        DbContext dbContext,
        DataVaultLatestSatelliteReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return true;
    }

    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteReadRecord>>([
          new DataVaultSatelliteReadRecord(
              "Profile",
              "SatCustomerProfile",
              "customer-hk",
              new Dictionary<string, string>(StringComparer.Ordinal),
              "profile-hash",
              LoadTimestamp,
              "crm-import",
              new Dictionary<string, string>(StringComparer.Ordinal) {
                ["Name"] = "Alice",
              }),
      ]);
    }

    public Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteProjectionRow>>([
          new DataVaultSatelliteProjectionRow(
              "Profile",
              new Dictionary<string, DataVaultSatelliteProjectionValue>(StringComparer.Ordinal) {
                ["ParentHashKey"] = DataVaultSatelliteProjectionValue.Present("customer-hk"),
                ["HashDiff"] = DataVaultSatelliteProjectionValue.Present("profile-hash"),
                ["LoadTimestamp"] = DataVaultSatelliteProjectionValue.Present(LoadTimestamp),
                ["RecordSource"] = DataVaultSatelliteProjectionValue.Present("crm-import"),
                ["Name"] = DataVaultSatelliteProjectionValue.Present("Alice"),
              }),
      ]);
    }
  }

  private sealed class ReturningPitReadStrategy : IDataVaultProviderPitReadStrategy {
    public int Priority => 100;

    public bool CanReadPitRows(DbContext dbContext, DataVaultPitAsOfReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return true;
    }

    public Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
        DataVaultProviderPitReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult<IReadOnlyList<DataVaultPitReadRecord>>([
          new DataVaultPitReadRecord(
              "customer-hk",
              LoadTimestamp,
              new Dictionary<string, string>(StringComparer.Ordinal),
              []),
      ]);
    }
  }

  private sealed class ReturningBridgeReadStrategy : IDataVaultProviderBridgeReadStrategy {
    public int Priority => 100;

    public bool CanReadBridgeRows(DbContext dbContext, DataVaultBridgeReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return true;
    }

    public Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
        DataVaultProviderBridgeReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult<IReadOnlyList<DataVaultBridgeReadRecord>>([
          new DataVaultBridgeReadRecord(
              "CustomerOrder",
              "BridgeCustomerOrder",
              [
                  new DataVaultBridgeEndpointReadValue(
                      DataVaultBridgeTraversalEndpoint.From,
                      "Customer",
                      "CustomerHashKey",
                      "customer-hk"),
                  new DataVaultBridgeEndpointReadValue(
                      DataVaultBridgeTraversalEndpoint.To,
                      "Order",
                      "OrderHashKey",
                      "order-hk"),
              ],
              traversalDepth: null),
      ]);
    }

    public Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
        DataVaultProviderBridgeReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult<IReadOnlyList<DataVaultBridgeProjectionRow>>([
          new DataVaultBridgeProjectionRow(
              "CustomerOrder",
              new Dictionary<string, DataVaultBridgeProjectionValue>(StringComparer.Ordinal) {
                ["CustomerHashKey"] = DataVaultBridgeProjectionValue.Present("customer-hk"),
                ["OrderHashKey"] = DataVaultBridgeProjectionValue.Present("order-hk"),
              }),
      ]);
    }
  }

  private sealed class ExternalReadService : IDataVaultReadService {
    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DbContext dbContext,
        DataVaultLatestSatelliteReadRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("External read service is not used by bridge extension fallback.");
    }

    public Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
        DbContext dbContext,
        DataVaultPitAsOfReadRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("External read service is not used by bridge extension fallback.");
    }
  }

  private sealed class EmptyBridgeModelContext(DbContextOptions<EmptyBridgeModelContext> options) : DbContext(options) {
  }

  private sealed class TestStableHashService : IStableHashService {
    public string AlgorithmId => "test-sha256-v1";

    public StableHashDigest ComputeHash(string normalizedInput) {
      ArgumentNullException.ThrowIfNull(normalizedInput);

      return new StableHashDigest(AlgorithmId, new string('a', 64));
    }
  }

  private sealed class TestStableHashNormalizer : IStableHashNormalizer {
    public string NormalizeValue(object? value) {
      return value?.ToString() ?? string.Empty;
    }

    public string NormalizeFields(IEnumerable<KeyValuePair<string, object?>> fields) {
      ArgumentNullException.ThrowIfNull(fields);

      return string.Join(
          "\n",
          fields.Select(field => field.Key + "=" + NormalizeValue(field.Value)));
    }
  }
}
