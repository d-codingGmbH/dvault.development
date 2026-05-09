using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides registry-backed read adapters over the explicit DVault read service.
/// </summary>
public static class DataVaultReadServiceRegistryExtensions {
  /// <summary>
  /// Resolves satellite metadata from the authoritative DbContext registry and reads the latest matching satellite rows.
  /// </summary>
  /// <param name="readService">The explicit read service that performs the provider-neutral latest-row query.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed latest satellite request containing the parent and logical satellite name.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The latest satellite rows grouped by parent hash key and driving-key identity.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before query orchestration starts when the DbContext has no authoritative registry source or the required satellite
  /// metadata declaration is missing from that source.
  /// </exception>
  /// <remarks>
  /// This adapter resolves metadata once and then delegates to the existing explicit read pipeline. Callers that invoke
  /// <see cref="IDataVaultReadService.ReadLatestSatelliteRowsAsync(DbContext, DataVaultLatestSatelliteReadRequest, CancellationToken)" />
  /// keep explicit caller-supplied metadata precedence and bypass registry resolution.
  /// </remarks>
  public static Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultRegistryLatestSatelliteReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var satellite = DataVaultRegistryMetadataResolver.GetRequiredSatellite(
        registry,
        request.Parent,
        request.SatelliteName);

    return readService.ReadLatestSatelliteRowsAsync(
        dbContext,
        new DataVaultLatestSatelliteReadRequest(satellite, request.ParentHashKeys, request.AsOf),
        cancellationToken);
  }
}
