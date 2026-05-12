using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines an optional provider-specific read strategy that can override the provider-neutral DVault latest/as-of satellite reader.
/// The core dispatcher evaluates registered strategies by descending priority, preserves registration order for equal
/// priorities, and selects the first strategy whose compatibility check accepts the current read request.
/// </summary>
public interface IDataVaultProviderReadStrategy {
  /// <summary>
  /// Gets the strategy priority used when multiple provider read strategies are registered.
  /// Higher values are evaluated first; equal values use dependency-injection registration order as the tie-break.
  /// </summary>
  int Priority { get; }

  /// <summary>
  /// Determines whether this strategy can read the supplied latest/as-of satellite request for the current Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose provider and model should be inspected.</param>
  /// <param name="request">The latest/as-of satellite read request to evaluate.</param>
  /// <returns><see langword="true" /> when this strategy can handle the read operation.</returns>
  bool CanReadLatestSatelliteRows(DbContext dbContext, DataVaultLatestSatelliteReadRequest request);

  /// <summary>
  /// Reads materialized latest/as-of satellite records using the provider-specific strategy.
  /// </summary>
  /// <param name="context">The strategy execution context.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The selected satellite rows.</returns>
  Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      DataVaultProviderReadStrategyContext context,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Reads latest/as-of satellite projection rows using the provider-specific strategy.
  /// </summary>
  /// <param name="context">The strategy execution context.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The selected satellite projection rows.</returns>
  Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
      DataVaultProviderReadStrategyContext context,
      CancellationToken cancellationToken = default);
}
