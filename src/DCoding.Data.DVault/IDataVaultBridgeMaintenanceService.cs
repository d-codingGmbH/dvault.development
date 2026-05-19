using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines the explicit DVault v1 bridge maintenance boundary used by callers after source-link ingestion.
/// </summary>
public interface IDataVaultBridgeMaintenanceService {
  /// <summary>
  /// Rebuilds one generated bridge table from the currently persisted source-link rows.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The bridge maintenance request containing the bridge metadata to rebuild.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while rebuilding bridge rows.</param>
  /// <returns>The rebuild summary for the generated bridge table.</returns>
  Task<DataVaultBridgeMaintenanceResult> RebuildBridgeAsync(
      DbContext dbContext,
      DataVaultBridgeMaintenanceRequest request,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Maintains one generated bridge table from the currently persisted source-link rows without deleting obsolete rows.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The bridge maintenance request containing the bridge metadata to maintain.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while maintaining bridge rows.</param>
  /// <returns>The incremental maintenance summary for the generated bridge table.</returns>
  Task<DataVaultBridgeMaintenanceResult> MaintainBridgeAsync(
      DbContext dbContext,
      DataVaultBridgeMaintenanceRequest request,
      CancellationToken cancellationToken = default);
}
