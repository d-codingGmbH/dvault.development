using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides registry-backed maintenance adapters over the explicit DVault bridge maintenance service.
/// </summary>
public static class DataVaultBridgeMaintenanceServiceRegistryExtensions {
  /// <summary>
  /// Resolves bridge metadata from the authoritative DbContext registry and rebuilds the generated bridge table.
  /// </summary>
  /// <param name="maintenanceService">The explicit bridge maintenance service.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed bridge maintenance request containing the logical bridge name.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while rebuilding bridge rows.</param>
  /// <returns>The rebuild summary for the generated bridge table.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before maintenance starts when the DbContext has no authoritative registry source or the required bridge
  /// metadata declaration is missing from that source.
  /// </exception>
  public static Task<DataVaultBridgeMaintenanceResult> RebuildBridgeAsync(
      this IDataVaultBridgeMaintenanceService maintenanceService,
      DbContext dbContext,
      DataVaultRegistryBridgeMaintenanceRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(maintenanceService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var bridge = DataVaultRegistryMetadataResolver.GetRequiredBridge(registry, request.BridgeName);

    return maintenanceService.RebuildBridgeAsync(
        dbContext,
        new DataVaultBridgeMaintenanceRequest(bridge),
        cancellationToken);
  }

  /// <summary>
  /// Resolves bridge metadata from the authoritative DbContext registry and incrementally maintains the generated bridge table.
  /// </summary>
  /// <param name="maintenanceService">The explicit bridge maintenance service.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed bridge maintenance request containing the logical bridge name.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while maintaining bridge rows.</param>
  /// <returns>The incremental maintenance summary for the generated bridge table.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before maintenance starts when the DbContext has no authoritative registry source or the required bridge
  /// metadata declaration is missing from that source.
  /// </exception>
  public static Task<DataVaultBridgeMaintenanceResult> MaintainBridgeAsync(
      this IDataVaultBridgeMaintenanceService maintenanceService,
      DbContext dbContext,
      DataVaultRegistryBridgeMaintenanceRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(maintenanceService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var bridge = DataVaultRegistryMetadataResolver.GetRequiredBridge(registry, request.BridgeName);

    return maintenanceService.MaintainBridgeAsync(
        dbContext,
        new DataVaultBridgeMaintenanceRequest(bridge),
        cancellationToken);
  }
}
