using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides typed row-mapper save helpers over the explicit DVault save service.
/// </summary>
public static class DataVaultSaveServiceTypedExtensions {
  /// <summary>
  /// Maps one source value to a registry-backed hub save request and persists it through the explicit save pipeline.
  /// </summary>
  /// <typeparam name="TSource">The source DTO or domain type mapped by <paramref name="mapper" />.</typeparam>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="source">The source value to map.</param>
  /// <param name="mapper">The typed hub row mapper.</param>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when mapper invocation or helper request assembly fails. The exception message identifies the logical hub target
  /// and stable source context while preserving the underlying validation reason in the inner exception.
  /// </exception>
  public static Task<DataVaultSaveResult> SaveHubAsync<TSource>(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      TSource source,
      IDataVaultHubMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      CancellationToken cancellationToken = default)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);

    var request = CreateHubRegistrySaveRequest(source, mapper, loadTimestamp, recordSource);
    return saveService.SaveAsync(dbContext, request, cancellationToken);
  }

  /// <summary>
  /// Maps one source value to a registry-backed link save request and persists it through the explicit save pipeline.
  /// </summary>
  /// <typeparam name="TSource">The source DTO or domain type mapped by <paramref name="mapper" />.</typeparam>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="source">The source value to map.</param>
  /// <param name="mapper">The typed link row mapper.</param>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when mapper invocation or helper request assembly fails. The exception message identifies the logical link target
  /// and stable source context while preserving the underlying validation reason in the inner exception.
  /// </exception>
  public static Task<DataVaultSaveResult> SaveLinkAsync<TSource>(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      TSource source,
      IDataVaultLinkMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      CancellationToken cancellationToken = default)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);

    var request = CreateLinkRegistrySaveRequest(source, mapper, loadTimestamp, recordSource);
    return saveService.SaveAsync(dbContext, request, cancellationToken);
  }

  /// <summary>
  /// Maps one source value to a registry-backed ordinary hub-parent satellite save request and persists it through the explicit save pipeline.
  /// </summary>
  /// <typeparam name="TSource">The source DTO or domain type mapped by <paramref name="mapper" />.</typeparam>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="source">The source value to map.</param>
  /// <param name="mapper">The typed satellite row mapper. The mapped operation must target an ordinary hub-parent satellite.</param>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when mapper invocation or helper request assembly fails. The exception message identifies the logical satellite target
  /// and stable source context while preserving the underlying validation reason in the inner exception.
  /// </exception>
  public static Task<DataVaultSaveResult> SaveOrdinaryHubSatelliteAsync<TSource>(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      TSource source,
      IDataVaultSatelliteMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      CancellationToken cancellationToken = default)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);

    var request = CreateOrdinaryHubSatelliteRegistrySaveRequest(source, mapper, loadTimestamp, recordSource);
    return saveService.SaveAsync(dbContext, request, cancellationToken);
  }

  /// <summary>
  /// Maps source values to caller-ordered registry-backed hub save requests and persists them as one explicit bulk batch.
  /// </summary>
  /// <typeparam name="TSource">The source DTO or domain type mapped by <paramref name="mapper" />.</typeparam>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="sources">The source values to map in caller-supplied order.</param>
  /// <param name="mapper">The typed hub row mapper.</param>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata for every mapped request.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata for every mapped request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when mapper invocation or helper request assembly fails. The exception message identifies the logical hub target,
  /// source CLR type, and zero-based batch index while preserving the underlying validation reason in the inner exception.
  /// </exception>
  public static Task<DataVaultSaveResult> SaveHubsAsync<TSource>(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      IEnumerable<TSource> sources,
      IDataVaultHubMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      CancellationToken cancellationToken = default)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(sources);
    ArgumentNullException.ThrowIfNull(mapper);

    var request = CreateHubRegistryBulkSaveRequest(sources, mapper, loadTimestamp, recordSource);
    return saveService.SaveAsync(dbContext, request, cancellationToken);
  }

  /// <summary>
  /// Maps source values to caller-ordered registry-backed link save requests and persists them as one explicit bulk batch.
  /// </summary>
  /// <typeparam name="TSource">The source DTO or domain type mapped by <paramref name="mapper" />.</typeparam>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="sources">The source values to map in caller-supplied order.</param>
  /// <param name="mapper">The typed link row mapper.</param>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata for every mapped request.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata for every mapped request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when mapper invocation or helper request assembly fails. The exception message identifies the logical link target,
  /// source CLR type, and zero-based batch index while preserving the underlying validation reason in the inner exception.
  /// </exception>
  public static Task<DataVaultSaveResult> SaveLinksAsync<TSource>(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      IEnumerable<TSource> sources,
      IDataVaultLinkMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      CancellationToken cancellationToken = default)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(sources);
    ArgumentNullException.ThrowIfNull(mapper);

    var request = CreateLinkRegistryBulkSaveRequest(sources, mapper, loadTimestamp, recordSource);
    return saveService.SaveAsync(dbContext, request, cancellationToken);
  }

  /// <summary>
  /// Maps source values to caller-ordered registry-backed ordinary hub-parent satellite save requests and persists them as one explicit bulk batch.
  /// </summary>
  /// <typeparam name="TSource">The source DTO or domain type mapped by <paramref name="mapper" />.</typeparam>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="sources">The source values to map in caller-supplied order.</param>
  /// <param name="mapper">The typed satellite row mapper. Each mapped operation must target an ordinary hub-parent satellite.</param>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata for every mapped request.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata for every mapped request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when mapper invocation or helper request assembly fails. The exception message identifies the logical satellite target,
  /// source CLR type, and zero-based batch index while preserving the underlying validation reason in the inner exception.
  /// </exception>
  public static Task<DataVaultSaveResult> SaveOrdinaryHubSatellitesAsync<TSource>(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      IEnumerable<TSource> sources,
      IDataVaultSatelliteMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      CancellationToken cancellationToken = default)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(sources);
    ArgumentNullException.ThrowIfNull(mapper);

    var request = CreateOrdinaryHubSatelliteRegistryBulkSaveRequest(sources, mapper, loadTimestamp, recordSource);
    return saveService.SaveAsync(dbContext, request, cancellationToken);
  }

  internal static DataVaultRegistrySaveRequest CreateHubRegistrySaveRequest<TSource>(
      TSource source,
      IDataVaultHubMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      int? batchIndex = null)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(mapper);

    var target = "hub";
    try {
      var operation = mapper.Map(source);
      if (operation is null) {
        throw new InvalidOperationException("The typed hub mapper returned null.");
      }

      target = "hub '" + operation.HubName + "'";
      return new DataVaultRegistrySaveRequest(loadTimestamp, recordSource, [operation], []);
    }
    catch (Exception exception) when (ShouldWrapAssemblyException(exception)) {
      throw CreateAssemblyException(target, source, batchIndex, exception);
    }
  }

  internal static DataVaultRegistrySaveRequest CreateLinkRegistrySaveRequest<TSource>(
      TSource source,
      IDataVaultLinkMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      int? batchIndex = null)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(mapper);

    var target = "link";
    try {
      var operation = mapper.Map(source);
      if (operation is null) {
        throw new InvalidOperationException("The typed link mapper returned null.");
      }

      target = "link '" + operation.LinkName + "'";
      return new DataVaultRegistrySaveRequest(loadTimestamp, recordSource, [], [operation]);
    }
    catch (Exception exception) when (ShouldWrapAssemblyException(exception)) {
      throw CreateAssemblyException(target, source, batchIndex, exception);
    }
  }

  internal static DataVaultRegistrySaveRequest CreateOrdinaryHubSatelliteRegistrySaveRequest<TSource>(
      TSource source,
      IDataVaultSatelliteMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource,
      int? batchIndex = null)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(mapper);

    var target = "hub-parent satellite";
    try {
      var operation = mapper.Map(source);
      if (operation is null) {
        throw new InvalidOperationException("The typed satellite mapper returned null.");
      }

      target = "satellite '" + operation.Parent.Name + "." + operation.SatelliteName + "'";
      RequireOrdinaryHubParentSatellite(operation);
      return new DataVaultRegistrySaveRequest(loadTimestamp, recordSource, [], [], [operation]);
    }
    catch (Exception exception) when (ShouldWrapAssemblyException(exception)) {
      throw CreateAssemblyException(target, source, batchIndex, exception);
    }
  }

  internal static DataVaultRegistryBulkSaveRequest CreateHubRegistryBulkSaveRequest<TSource>(
      IEnumerable<TSource> sources,
      IDataVaultHubMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(sources);
    ArgumentNullException.ThrowIfNull(mapper);

    return CreateBulkSaveRequest(
        sources,
        (source, batchIndex) => CreateHubRegistrySaveRequest(source, mapper, loadTimestamp, recordSource, batchIndex));
  }

  internal static DataVaultRegistryBulkSaveRequest CreateLinkRegistryBulkSaveRequest<TSource>(
      IEnumerable<TSource> sources,
      IDataVaultLinkMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(sources);
    ArgumentNullException.ThrowIfNull(mapper);

    return CreateBulkSaveRequest(
        sources,
        (source, batchIndex) => CreateLinkRegistrySaveRequest(source, mapper, loadTimestamp, recordSource, batchIndex));
  }

  internal static DataVaultRegistryBulkSaveRequest CreateOrdinaryHubSatelliteRegistryBulkSaveRequest<TSource>(
      IEnumerable<TSource> sources,
      IDataVaultSatelliteMapper<TSource> mapper,
      DateTimeOffset loadTimestamp,
      string recordSource)
      where TSource : notnull {
    ArgumentNullException.ThrowIfNull(sources);
    ArgumentNullException.ThrowIfNull(mapper);

    return CreateBulkSaveRequest(
        sources,
        (source, batchIndex) => CreateOrdinaryHubSatelliteRegistrySaveRequest(source, mapper, loadTimestamp, recordSource, batchIndex));
  }

  private static DataVaultRegistryBulkSaveRequest CreateBulkSaveRequest<TSource>(
      IEnumerable<TSource> sources,
      Func<TSource, int, DataVaultRegistrySaveRequest> requestFactory)
      where TSource : notnull {
    var requests = new List<DataVaultRegistrySaveRequest>();
    var batchIndex = 0;

    foreach (var source in sources) {
      requests.Add(requestFactory(source, batchIndex));
      batchIndex++;
    }

    return new DataVaultRegistryBulkSaveRequest(requests);
  }

  private static void RequireOrdinaryHubParentSatellite(DataVaultRegistrySatelliteSaveOperation operation) {
    if (operation.Parent.Kind != DataVaultMetadataReferenceKind.Hub) {
      throw new ArgumentException(
          "Typed satellite save helpers support only ordinary hub-parent satellite operations. " +
          "The mapped satellite targets " +
          operation.Parent.Kind.ToString().ToLowerInvariant() +
          " parent '" +
          operation.Parent.Name +
          "'.",
          nameof(operation));
    }

    if (operation.DrivingKeyValues.Count > 0) {
      throw new ArgumentException(
          "Typed satellite save helpers support only ordinary hub-parent satellite operations. " +
          "The mapped satellite contains driving-key values.",
          nameof(operation));
    }
  }

  private static InvalidOperationException CreateAssemblyException<TSource>(
      string target,
      TSource source,
      int? batchIndex,
      Exception innerException)
      where TSource : notnull {
    return new InvalidOperationException(
        "Failed to assemble typed Data Vault save request for " +
        target +
        " from source type '" +
        GetStableSourceTypeName(source) +
        "'" +
        FormatBatchIndex(batchIndex) +
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

  private static string FormatBatchIndex(int? batchIndex) {
    return batchIndex.HasValue
        ? " at batch index " + batchIndex.Value.ToString(CultureInfo.InvariantCulture)
        : string.Empty;
  }

  private static bool ShouldWrapAssemblyException(Exception exception) {
    return exception is not OperationCanceledException;
  }
}
