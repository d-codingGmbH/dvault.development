using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines the explicit provider-neutral maintenance surface for generated PIT tables.
/// </summary>
public interface IDataVaultPitMaintenanceService {
  /// <summary>
  /// Deletes and rebuilds every row for one generated PIT table.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The PIT rebuild request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while maintaining rows.</param>
  /// <returns>The number of PIT rows deleted and inserted by the rebuild.</returns>
  Task<DataVaultPitMaintenanceResult> RebuildAsync(
      DbContext dbContext,
      DataVaultPitRebuildRequest request,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Recomputes complete PIT history for explicit parent hash keys.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The bounded parent maintenance request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while maintaining rows.</param>
  /// <returns>The number of PIT rows deleted and inserted by the bounded maintenance operation.</returns>
  Task<DataVaultPitMaintenanceResult> MaintainParentsAsync(
      DbContext dbContext,
      DataVaultPitParentMaintenanceRequest request,
      CancellationToken cancellationToken = default);
}
