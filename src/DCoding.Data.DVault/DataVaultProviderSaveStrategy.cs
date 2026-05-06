using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines an optional provider-specific save strategy that can override the provider-neutral DVault fallback writer.
/// The core dispatcher evaluates registered strategies by descending priority, preserves registration order for equal
/// priorities, and selects the first strategy whose compatibility check accepts the current save request batch.
/// </summary>
public interface IDataVaultProviderSaveStrategy {
  /// <summary>
  /// Gets the strategy priority used when multiple provider strategies are registered.
  /// Higher values are evaluated first; equal values use dependency-injection registration order as the tie-break.
  /// </summary>
  int Priority { get; }

  /// <summary>
  /// Determines whether this strategy can persist the supplied requests for the current Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose provider and state should be inspected.</param>
  /// <param name="requests">The ordered save requests to persist.</param>
  /// <returns><see langword="true" /> when this strategy can handle the save operation.</returns>
  bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests);

  /// <summary>
  /// Persists the ordered save requests using the provider-specific strategy.
  /// </summary>
  /// <param name="context">The strategy execution context.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default);
}

/// <summary>
/// Carries the shared dependencies and ordered resolved requests used by provider-specific DVault save strategies.
/// </summary>
public sealed class DataVaultProviderSaveStrategyContext {
  /// <summary>
  /// Initializes a new provider save strategy context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="requests">The ordered save requests to persist.</param>
  /// <param name="stableHashService">The stable hash service used to compute hub and link hash keys.</param>
  /// <param name="stableHashNormalizer">The normalizer used before stable hash computation.</param>
  public DataVaultProviderSaveStrategyContext(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      IStableHashService stableHashService,
      IStableHashNormalizer stableHashNormalizer)
      : this(
          dbContext,
          requests,
          CreateDefaultResolvedRequests(requests),
          stableHashService,
          stableHashNormalizer) {
  }

  /// <summary>
  /// Initializes a new provider save strategy context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="requests">The ordered original save requests to persist.</param>
  /// <param name="resolvedRequests">The ordered save requests after timestamp and record-source resolution.</param>
  /// <param name="stableHashService">The stable hash service used to compute hub and link hash keys.</param>
  /// <param name="stableHashNormalizer">The normalizer used before stable hash computation.</param>
  public DataVaultProviderSaveStrategyContext(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      IReadOnlyList<DataVaultResolvedSaveRequest> resolvedRequests,
      IStableHashService stableHashService,
      IStableHashNormalizer stableHashNormalizer) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);
    ArgumentNullException.ThrowIfNull(stableHashService);
    ArgumentNullException.ThrowIfNull(stableHashNormalizer);

    DbContext = dbContext;
    Requests = requests.ToArray();
    ResolvedRequests = RequireResolvedRequests(Requests, resolvedRequests, nameof(resolvedRequests));
    StableHashService = stableHashService;
    StableHashNormalizer = stableHashNormalizer;
  }

  /// <summary>
  /// Gets the context whose model has been configured with Data Vault metadata.
  /// </summary>
  public DbContext DbContext { get; }

  /// <summary>
  /// Gets the ordered save requests to persist.
  /// </summary>
  public IReadOnlyList<DataVaultSaveRequest> Requests { get; }

  /// <summary>
  /// Gets the ordered save requests after timestamp and record-source resolution.
  /// </summary>
  public IReadOnlyList<DataVaultResolvedSaveRequest> ResolvedRequests { get; }

  /// <summary>
  /// Gets the stable hash service used to compute hub and link hash keys.
  /// </summary>
  public IStableHashService StableHashService { get; }

  /// <summary>
  /// Gets the normalizer used before stable hash computation.
  /// </summary>
  public IStableHashNormalizer StableHashNormalizer { get; }

  private static IReadOnlyList<DataVaultResolvedSaveRequest> CreateDefaultResolvedRequests(
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    return requests
        .Select(request => new DataVaultResolvedSaveRequest(request, request.LoadTimestamp, request.RecordSource))
        .ToArray();
  }

  private static IReadOnlyList<DataVaultResolvedSaveRequest> RequireResolvedRequests(
      IReadOnlyList<DataVaultSaveRequest> requests,
      IReadOnlyList<DataVaultResolvedSaveRequest> resolvedRequests,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(resolvedRequests);

    if (requests.Count != resolvedRequests.Count) {
      throw new ArgumentException("Data Vault resolved save requests must match the original request count.", parameterName);
    }

    var resolvedRequestArray = resolvedRequests.ToArray();
    for (var index = 0; index < requests.Count; index++) {
      if (resolvedRequestArray[index] is null) {
        throw new ArgumentException("Data Vault resolved save request collections must not contain null values.", parameterName);
      }

      if (!ReferenceEquals(resolvedRequestArray[index].Request, requests[index])) {
        throw new ArgumentException("Data Vault resolved save requests must reference the original request at the same ordinal.", parameterName);
      }
    }

    return resolvedRequestArray;
  }
}
