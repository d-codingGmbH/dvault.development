using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides registry-backed maintenance adapters over the explicit DVault PIT maintenance service.
/// </summary>
public static class DataVaultPitMaintenanceServiceRegistryExtensions {
  /// <summary>
  /// Resolves PIT metadata from the authoritative DbContext registry and rebuilds the generated PIT table.
  /// </summary>
  /// <param name="maintenanceService">The explicit PIT maintenance service.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed PIT rebuild request containing the exact PIT lookup target.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while rebuilding PIT rows.</param>
  /// <returns>The rebuild summary for the generated PIT table.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before maintenance starts when the DbContext has no authoritative registry source, the required PIT metadata
  /// declaration is missing from that source, or the resolved PIT declaration is outside the supported maintenance baseline.
  /// </exception>
  public static Task<DataVaultPitMaintenanceResult> RebuildAsync(
      this IDataVaultPitMaintenanceService maintenanceService,
      DbContext dbContext,
      DataVaultRegistryPitRebuildRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(maintenanceService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var pit = ResolveRequiredPit(registry, request);
    DataVaultPitMaintenanceShapeValidator.ValidateSupportedShape(pit);

    return maintenanceService.RebuildAsync(
        dbContext,
        new DataVaultPitRebuildRequest(pit),
        cancellationToken);
  }

  /// <summary>
  /// Resolves PIT metadata from the authoritative DbContext registry and recomputes PIT history for explicit parent hash keys.
  /// </summary>
  /// <param name="maintenanceService">The explicit PIT maintenance service.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed PIT parent-maintenance request containing the exact PIT lookup target.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while maintaining PIT rows.</param>
  /// <returns>The bounded maintenance summary for the generated PIT table.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before maintenance starts when the DbContext has no authoritative registry source, the required PIT metadata
  /// declaration is missing from that source, or the resolved PIT declaration is outside the supported maintenance baseline.
  /// </exception>
  public static Task<DataVaultPitMaintenanceResult> MaintainParentsAsync(
      this IDataVaultPitMaintenanceService maintenanceService,
      DbContext dbContext,
      DataVaultRegistryPitParentMaintenanceRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(maintenanceService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var pit = ResolveRequiredPit(registry, request);
    DataVaultPitMaintenanceShapeValidator.ValidateSupportedShape(pit);

    return maintenanceService.MaintainParentsAsync(
        dbContext,
        new DataVaultPitParentMaintenanceRequest(pit, request.ParentHashKeys),
        cancellationToken);
  }

  private static DataVaultPitMetadata ResolveRequiredPit(
      DataVaultMetadataRegistry registry,
      DataVaultRegistryPitRebuildRequest request) {
    return request.PitClrType is null
        ? DataVaultRegistryMetadataResolver.GetRequiredPit(registry, request.PitName!)
        : DataVaultRegistryMetadataResolver.GetRequiredPit(registry, request.PitClrType);
  }

  private static DataVaultPitMetadata ResolveRequiredPit(
      DataVaultMetadataRegistry registry,
      DataVaultRegistryPitParentMaintenanceRequest request) {
    return request.PitClrType is null
        ? DataVaultRegistryMetadataResolver.GetRequiredPit(registry, request.PitName!)
        : DataVaultRegistryMetadataResolver.GetRequiredPit(registry, request.PitClrType);
  }
}
