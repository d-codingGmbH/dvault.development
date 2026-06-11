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
