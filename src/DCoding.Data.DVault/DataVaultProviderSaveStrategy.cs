using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines an optional provider-specific save strategy that can override the provider-neutral DVault fallback writer.
/// </summary>
public interface IDataVaultProviderSaveStrategy {
  /// <summary>
  /// Gets the strategy priority used when multiple provider strategies are registered.
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
/// Carries the shared dependencies and ordered requests used by provider-specific DVault save strategies.
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
      IStableHashNormalizer stableHashNormalizer) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);
    ArgumentNullException.ThrowIfNull(stableHashService);
    ArgumentNullException.ThrowIfNull(stableHashNormalizer);

    DbContext = dbContext;
    Requests = requests;
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
  /// Gets the stable hash service used to compute hub and link hash keys.
  /// </summary>
  public IStableHashService StableHashService { get; }

  /// <summary>
  /// Gets the normalizer used before stable hash computation.
  /// </summary>
  public IStableHashNormalizer StableHashNormalizer { get; }
}
