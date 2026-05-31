using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides async source save helpers over the explicit DVault save service.
/// </summary>
public static class DataVaultSaveServiceAsyncExtensions {
  /// <summary>
  /// Maps async source values to explicit save requests, groups them into bounded chunks, and persists them through the
  /// async chunked save boundary without materializing the full source first.
  /// </summary>
  /// <typeparam name="TSource">The source DTO or domain type mapped by <paramref name="requestFactory" />.</typeparam>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="sources">The async source values to map in caller-supplied order.</param>
  /// <param name="requestFactory">The caller-owned mapping from one source value to one explicit save request.</param>
  /// <param name="chunkSize">The maximum number of mapped save requests to include in each generated chunk.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while enumerating, mapping, and saving chunks.</param>
  /// <returns>The persisted row summary, including saved hash-key values in source and chunk order.</returns>
  /// <exception cref="ArgumentOutOfRangeException">
  /// Thrown when <paramref name="chunkSize" /> is less than one.
  /// </exception>
  /// <exception cref="InvalidOperationException">
  /// Thrown when request mapping fails. The exception message identifies the source CLR type and zero-based batch index
  /// while preserving the underlying validation reason in the inner exception.
  /// </exception>
  public static Task<DataVaultSaveResult> SaveAsync<TSource>(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      IAsyncEnumerable<TSource> sources,
      Func<TSource, DataVaultSaveRequest> requestFactory,
      int chunkSize,
      CancellationToken cancellationToken = default)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(requestFactory);

    return SaveMappedAsync(
        saveService,
        dbContext,
        sources,
        (source, batchIndex) => CreateExplicitSaveRequest(source, requestFactory, batchIndex),
        chunkSize,
        cancellationToken);
  }

  internal static Task<DataVaultSaveResult> SaveMappedAsync<TSource>(
      IDataVaultSaveService saveService,
      DbContext dbContext,
      IAsyncEnumerable<TSource> sources,
      Func<TSource, int, DataVaultSaveRequest> requestFactory,
      int chunkSize,
      CancellationToken cancellationToken)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(sources);
    ArgumentNullException.ThrowIfNull(requestFactory);
    RequireValidChunkSize(chunkSize);

    return saveService.SaveAsync(
        dbContext,
        CreateMappedRequestChunksAsync(sources, requestFactory, chunkSize, cancellationToken),
        cancellationToken);
  }

  private static async IAsyncEnumerable<DataVaultSaveChunk> CreateMappedRequestChunksAsync<TSource>(
      IAsyncEnumerable<TSource> sources,
      Func<TSource, int, DataVaultSaveRequest> requestFactory,
      int chunkSize,
      [EnumeratorCancellation] CancellationToken cancellationToken)
      where TSource : notnull {
    var requests = new List<DataVaultSaveRequest>(chunkSize);
    var batchIndex = 0;

    await foreach (var source in sources.WithCancellation(cancellationToken).ConfigureAwait(false)) {
      cancellationToken.ThrowIfCancellationRequested();
      requests.Add(requestFactory(source, batchIndex));
      batchIndex++;

      if (requests.Count == chunkSize) {
        yield return new DataVaultSaveChunk(requests);
        requests = new List<DataVaultSaveRequest>(chunkSize);
      }
    }

    if (requests.Count > 0) {
      yield return new DataVaultSaveChunk(requests);
    }
  }

  private static DataVaultSaveRequest CreateExplicitSaveRequest<TSource>(
      TSource source,
      Func<TSource, DataVaultSaveRequest> requestFactory,
      int batchIndex)
      where TSource : notnull {
    try {
      var request = requestFactory(source);
      if (request is null) {
        throw new InvalidOperationException("The async Data Vault request factory returned null.");
      }

      return request;
    }
    catch (Exception exception) when (ShouldWrapAssemblyException(exception)) {
      throw CreateAssemblyException(source, batchIndex, exception);
    }
  }

  internal static void RequireValidChunkSize(int chunkSize) {
    if (chunkSize < 1) {
      throw new ArgumentOutOfRangeException(
          nameof(chunkSize),
          chunkSize,
          "Async Data Vault save helper chunk size must be greater than zero.");
    }
  }

  private static InvalidOperationException CreateAssemblyException<TSource>(
      TSource source,
      int batchIndex,
      Exception innerException)
      where TSource : notnull {
    return new InvalidOperationException(
        "Failed to assemble async Data Vault save request from source type '" +
        GetStableSourceTypeName(source) +
        "' at batch index " +
        batchIndex.ToString(CultureInfo.InvariantCulture) +
        ". Reason: " +
        innerException.Message,
        innerException);
  }

  private static string GetStableSourceTypeName<TSource>(TSource source)
      where TSource : notnull {
    object? boxedSource = source;
    var sourceType = boxedSource?.GetType() ?? typeof(TSource);
    return sourceType.FullName ?? sourceType.Name;
  }

  private static bool ShouldWrapAssemblyException(Exception exception) {
    return exception is not OperationCanceledException;
  }
}
