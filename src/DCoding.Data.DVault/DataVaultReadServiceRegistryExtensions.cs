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

  /// <summary>
  /// Resolves bridge metadata from the authoritative DbContext registry and reads matching generated bridge rows.
  /// </summary>
  /// <param name="readService">The explicit read service that anchors the provider-neutral bridge helper surface.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed bridge read request containing the bridge name and endpoint keys.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The bridge rows in deterministic generated endpoint column order.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before query orchestration starts when the DbContext has no authoritative registry source or the required bridge
  /// metadata declaration is missing from that source.
  /// </exception>
  public static Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultRegistryBridgeReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var bridge = DataVaultRegistryMetadataResolver.GetRequiredBridge(registry, request.BridgeName);

    return readService.ReadBridgeRowsAsync(
        dbContext,
        new DataVaultBridgeReadRequest(bridge, request.Endpoint, request.EndpointHashKeys, request.MaximumDepth),
        cancellationToken);
  }

  /// <summary>
  /// Resolves satellite metadata from the authoritative DbContext registry and maps each selected latest row through a typed delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral latest-row helper surface.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed latest satellite request containing the parent and logical satellite name.</param>
  /// <param name="projector">The delegate that maps one selected satellite row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows grouped by parent hash key and driving-key identity.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before query orchestration starts when the DbContext has no authoritative registry source, the required satellite
  /// metadata declaration is missing from that source, or the typed projection contract rejects the row mapping.
  /// </exception>
  /// <remarks>
  /// This adapter resolves metadata once and then delegates to the explicit typed projection pipeline, so explicit and
  /// registry-backed typed reads share selection, row access, and diagnostic behavior.
  /// </remarks>
  /// <example>
  /// <code>
  /// <![CDATA[
  /// var rows = await readService.ReadLatestSatelliteAsync(
  ///     context,
  ///     new DataVaultRegistryLatestSatelliteReadRequest(
  ///         DataVaultMetadataReference.Hub("Customer"),
  ///         "Contact",
  ///         [customerHashKey],
  ///         cutoffUtc),
  ///     row => new CustomerContactRead(
  ///         row.RequiredString("ParentHashKey"),
  ///         row.RequiredString("HashDiff"),
  ///         row.RequiredDateTimeOffset("LoadTimestamp"),
  ///         row.RequiredString("RecordSource"),
  ///         row.RequiredString("ContactType"),
  ///         row.NullableString("EmailAddress")));
  /// ]]>
  /// </code>
  /// </example>
  public static Task<IReadOnlyList<TProjection>> ReadLatestSatelliteAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultRegistryLatestSatelliteReadRequest request,
      Func<DataVaultSatelliteProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    ArgumentNullException.ThrowIfNull(projector);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var satellite = DataVaultRegistryMetadataResolver.GetRequiredSatellite(
        registry,
        request.Parent,
        request.SatelliteName);

    return readService.ReadLatestSatelliteAsync(
        dbContext,
        new DataVaultLatestSatelliteReadRequest(satellite, request.ParentHashKeys, request.AsOf),
        projector,
        cancellationToken);
  }

  /// <summary>
  /// Resolves bridge metadata from the authoritative DbContext registry and maps each selected row through a typed delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral bridge helper surface.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed bridge read request containing the bridge name and endpoint keys.</param>
  /// <param name="projector">The delegate that maps one bridge row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows in deterministic generated endpoint column order.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before query orchestration starts when the DbContext has no authoritative registry source, the required bridge
  /// metadata declaration is missing from that source, or the typed projection contract rejects the row mapping.
  /// </exception>
  public static Task<IReadOnlyList<TProjection>> ReadBridgeAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultRegistryBridgeReadRequest request,
      Func<DataVaultBridgeProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    ArgumentNullException.ThrowIfNull(projector);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var bridge = DataVaultRegistryMetadataResolver.GetRequiredBridge(registry, request.BridgeName);

    return readService.ReadBridgeAsync(
        dbContext,
        new DataVaultBridgeReadRequest(bridge, request.Endpoint, request.EndpointHashKeys, request.MaximumDepth),
        projector,
        cancellationToken);
  }
}
