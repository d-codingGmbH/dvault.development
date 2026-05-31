using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultAsyncSaveHelperTests {
  [Fact]
  public async Task AsyncRequestMapperHelperPreservesOrderAndChunkBoundaries() {
    var saveService = new CapturingSaveService();
    await using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);

    await saveService.SaveAsync(
        dbContext,
        CreateAsyncSources([1, 2, 3, 4, 5]),
        CreateCustomerSaveRequest,
        2);

    Assert.Equal([2, 2, 1], saveService.Chunks.Select(chunk => chunk.Count).ToArray());
    Assert.Equal(
        ["source-1", "source-2", "source-3", "source-4", "source-5"],
        saveService.Chunks
            .SelectMany(chunk => chunk)
            .Select(request => request.RecordSource)
            .ToArray());
  }

  [Fact]
  public async Task AsyncRequestMapperHelperTreatsEmptySourceAsNoOpChunkSequence() {
    var saveService = new CapturingSaveService();
    await using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);

    await saveService.SaveAsync(
        dbContext,
        CreateAsyncSources<int>([]),
        CreateCustomerSaveRequest,
        2);

    Assert.Empty(saveService.Chunks);
  }

  [Fact]
  public async Task AsyncRequestMapperHelperWrapsFactoryFailuresWithBatchContext() {
    var saveService = new CapturingSaveService();
    await using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => saveService.SaveAsync(
        dbContext,
        CreateAsyncSources([1, 2, 3]),
        source => source == 3
            ? throw new ArgumentException("mapped failure reason", nameof(source))
            : CreateCustomerSaveRequest(source),
        2));

    Assert.Contains(typeof(int).FullName!, exception.Message, StringComparison.Ordinal);
    Assert.Contains("batch index 2", exception.Message, StringComparison.Ordinal);
    Assert.Contains("mapped failure reason", exception.Message, StringComparison.Ordinal);
    Assert.IsType<ArgumentException>(exception.InnerException);
    Assert.Single(saveService.Chunks);
  }

  [Fact]
  public async Task AsyncRequestMapperHelperObservesCancellationBeforeLaterChunks() {
    using var cancellation = new CancellationTokenSource();
    var requestedSources = new List<int>();
    var saveService = new CancellingAfterFirstChunkSaveService(cancellation);
    await using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);

    await Assert.ThrowsAsync<OperationCanceledException>(() => saveService.SaveAsync(
        dbContext,
        CreateCountingAsyncSources([1, 2, 3, 4], requestedSources.Add),
        CreateCustomerSaveRequest,
        2,
        cancellation.Token));

    Assert.Equal([1, 2], requestedSources.ToArray());
    Assert.Single(saveService.Chunks);
  }

  [Fact]
  public async Task TypedAsyncHubHelperResolvesRegistryAndPreservesChunkOrder() {
    var saveService = new CapturingSaveService();
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id", "Region Code"])],
        [],
        []);
    await using var dbContext = new DbContext(
        new DbContextOptionsBuilder()
            .UseSqlite("Data Source=:memory:")
            .UseDataVaultMetadata(metadataModel)
            .Options);

    await saveService.SaveHubsAsync(
        dbContext,
        CreateAsyncSources([
            new CustomerSource("C-100", "DE"),
            new CustomerSource("C-200", "US"),
            new CustomerSource("C-300", "FR"),
        ]),
        new CustomerHubMapper(),
        new DateTimeOffset(2026, 5, 30, 10, 0, 0, TimeSpan.Zero),
        "typed-async-import",
        2);

    Assert.Equal([2, 1], saveService.Chunks.Select(chunk => chunk.Count).ToArray());
    Assert.Equal(
        ["C-100", "C-200", "C-300"],
        saveService.Chunks
            .SelectMany(chunk => chunk)
            .Select(request => Assert.Single(request.HubOperations).BusinessKeyValues["Customer Id"])
            .ToArray());
  }

  private static DataVaultSaveRequest CreateCustomerSaveRequest(int source) {
    return new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 30, 10, 0, 0, TimeSpan.Zero),
        "source-" + source,
        [
            new DataVaultHubSaveOperation(
                new DataVaultHubMetadata("Customer", ["Customer Id"]),
                [new("Customer Id", "C-" + source.ToString("000", null))]),
        ],
        []);
  }

  private static async IAsyncEnumerable<T> CreateAsyncSources<T>(
      IReadOnlyList<T> sources,
      [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    foreach (var source in sources) {
      cancellationToken.ThrowIfCancellationRequested();
      await Task.Yield();
      yield return source;
    }
  }

  private static async IAsyncEnumerable<int> CreateCountingAsyncSources(
      IReadOnlyList<int> sources,
      Action<int> onSourceRequested,
      [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    foreach (var source in sources) {
      cancellationToken.ThrowIfCancellationRequested();
      onSourceRequested(source);
      await Task.Yield();
      yield return source;
    }
  }

  private sealed class CustomerHubMapper : IDataVaultHubMapper<CustomerSource> {
    public DataVaultRegistryHubSaveOperation Map(CustomerSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistryHubSaveOperation(
          "Customer",
          [
              new("Customer Id", source.CustomerId),
              new("Region Code", source.RegionCode),
          ]);
    }
  }

  private sealed class CapturingSaveService : IDataVaultSaveService {
    public List<IReadOnlyList<DataVaultSaveRequest>> Chunks { get; } = [];

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultBulkSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultChunkedSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public async Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        IAsyncEnumerable<DataVaultSaveChunk> chunks,
        CancellationToken cancellationToken = default) {
      await foreach (var chunk in chunks.WithCancellation(cancellationToken).ConfigureAwait(false)) {
        Chunks.Add(chunk.Requests.ToArray());
      }

      return new DataVaultSaveResult(0, []);
    }
  }

  private sealed class CancellingAfterFirstChunkSaveService(CancellationTokenSource cancellation) : IDataVaultSaveService {
    public List<IReadOnlyList<DataVaultSaveRequest>> Chunks { get; } = [];

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultBulkSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultChunkedSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public async Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        IAsyncEnumerable<DataVaultSaveChunk> chunks,
        CancellationToken cancellationToken = default) {
      await foreach (var chunk in chunks.WithCancellation(cancellationToken).ConfigureAwait(false)) {
        Chunks.Add(chunk.Requests.ToArray());
        cancellation.Cancel();
      }

      return new DataVaultSaveResult(0, []);
    }
  }

  private sealed record CustomerSource(string CustomerId, string RegionCode);
}
