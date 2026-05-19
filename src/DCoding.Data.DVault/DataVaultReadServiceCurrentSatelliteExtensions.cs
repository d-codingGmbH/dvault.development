using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides current and as-of convenience helpers over the existing latest-satellite read pipeline.
/// </summary>
public static class DataVaultReadServiceCurrentSatelliteExtensions {
  /// <summary>
  /// Reads the current satellite rows for explicit metadata and parent hash keys.
  /// </summary>
  /// <param name="readService">The explicit read service that performs the provider-neutral latest-row query.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="satellite">The satellite metadata declaration to read.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The current satellite rows grouped by parent hash key and driving-key identity.</returns>
  public static Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadCurrentSatelliteRowsAsync(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultSatelliteMetadata satellite,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(satellite);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    return readService.ReadLatestSatelliteRowsAsync(
        dbContext,
        new DataVaultLatestSatelliteReadRequest(satellite, parentHashKeys),
        cancellationToken);
  }

  /// <summary>
  /// Reads satellite rows visible at one as-of timestamp for explicit metadata and parent hash keys.
  /// </summary>
  /// <param name="readService">The explicit read service that performs the provider-neutral latest-row query.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="satellite">The satellite metadata declaration to read.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="asOf">The inclusive cutoff for as-of reads.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The satellite rows visible at the requested as-of timestamp.</returns>
  public static Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadAsOfSatelliteRowsAsync(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultSatelliteMetadata satellite,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset asOf,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(satellite);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    return readService.ReadLatestSatelliteRowsAsync(
        dbContext,
        new DataVaultLatestSatelliteReadRequest(satellite, parentHashKeys, asOf),
        cancellationToken);
  }

  /// <summary>
  /// Reads the current satellite rows and maps each selected row through a caller-supplied typed projection delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral latest-row helper surface.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="satellite">The satellite metadata declaration to read.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="projector">The delegate that maps one selected satellite row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows grouped by parent hash key and driving-key identity.</returns>
  public static Task<IReadOnlyList<TProjection>> ReadCurrentSatelliteAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultSatelliteMetadata satellite,
      IEnumerable<string> parentHashKeys,
      Func<DataVaultSatelliteProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(satellite);
    ArgumentNullException.ThrowIfNull(parentHashKeys);
    ArgumentNullException.ThrowIfNull(projector);

    return readService.ReadLatestSatelliteAsync(
        dbContext,
        new DataVaultLatestSatelliteReadRequest(satellite, parentHashKeys),
        projector,
        cancellationToken);
  }

  /// <summary>
  /// Reads satellite rows visible at one as-of timestamp and maps each selected row through a typed projection delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral latest-row helper surface.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="satellite">The satellite metadata declaration to read.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="asOf">The inclusive cutoff for as-of reads.</param>
  /// <param name="projector">The delegate that maps one selected satellite row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows visible at the requested as-of timestamp.</returns>
  public static Task<IReadOnlyList<TProjection>> ReadAsOfSatelliteAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultSatelliteMetadata satellite,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset asOf,
      Func<DataVaultSatelliteProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(satellite);
    ArgumentNullException.ThrowIfNull(parentHashKeys);
    ArgumentNullException.ThrowIfNull(projector);

    return readService.ReadLatestSatelliteAsync(
        dbContext,
        new DataVaultLatestSatelliteReadRequest(satellite, parentHashKeys, asOf),
        projector,
        cancellationToken);
  }

  /// <summary>
  /// Resolves satellite metadata from the authoritative DbContext registry and reads the current matching satellite rows.
  /// </summary>
  /// <param name="readService">The explicit read service that performs the provider-neutral latest-row query.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The current satellite rows grouped by parent hash key and driving-key identity.</returns>
  public static Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadCurrentSatelliteRowsAsync(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultMetadataReference parent,
      string satelliteName,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(parent);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    return readService.ReadLatestSatelliteRowsAsync(
        dbContext,
        new DataVaultRegistryLatestSatelliteReadRequest(parent, satelliteName, parentHashKeys),
        cancellationToken);
  }

  /// <summary>
  /// Resolves satellite metadata from the authoritative DbContext registry and reads rows visible at one as-of timestamp.
  /// </summary>
  /// <param name="readService">The explicit read service that performs the provider-neutral latest-row query.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="asOf">The inclusive cutoff for as-of reads.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The satellite rows visible at the requested as-of timestamp.</returns>
  public static Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadAsOfSatelliteRowsAsync(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultMetadataReference parent,
      string satelliteName,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset asOf,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(parent);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    return readService.ReadLatestSatelliteRowsAsync(
        dbContext,
        new DataVaultRegistryLatestSatelliteReadRequest(parent, satelliteName, parentHashKeys, asOf),
        cancellationToken);
  }

  /// <summary>
  /// Resolves satellite metadata from the authoritative DbContext registry and maps current rows through a typed delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral latest-row helper surface.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="projector">The delegate that maps one selected satellite row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows grouped by parent hash key and driving-key identity.</returns>
  public static Task<IReadOnlyList<TProjection>> ReadCurrentSatelliteAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultMetadataReference parent,
      string satelliteName,
      IEnumerable<string> parentHashKeys,
      Func<DataVaultSatelliteProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(parent);
    ArgumentNullException.ThrowIfNull(parentHashKeys);
    ArgumentNullException.ThrowIfNull(projector);

    return readService.ReadLatestSatelliteAsync(
        dbContext,
        new DataVaultRegistryLatestSatelliteReadRequest(parent, satelliteName, parentHashKeys),
        projector,
        cancellationToken);
  }

  /// <summary>
  /// Resolves satellite metadata from the authoritative DbContext registry and maps as-of rows through a typed delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral latest-row helper surface.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="asOf">The inclusive cutoff for as-of reads.</param>
  /// <param name="projector">The delegate that maps one selected satellite row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows visible at the requested as-of timestamp.</returns>
  public static Task<IReadOnlyList<TProjection>> ReadAsOfSatelliteAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultMetadataReference parent,
      string satelliteName,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset asOf,
      Func<DataVaultSatelliteProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(parent);
    ArgumentNullException.ThrowIfNull(parentHashKeys);
    ArgumentNullException.ThrowIfNull(projector);

    return readService.ReadLatestSatelliteAsync(
        dbContext,
        new DataVaultRegistryLatestSatelliteReadRequest(parent, satelliteName, parentHashKeys, asOf),
        projector,
        cancellationToken);
  }
}
