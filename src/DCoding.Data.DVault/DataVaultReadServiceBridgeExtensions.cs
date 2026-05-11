using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides provider-neutral bridge read helpers over the explicit DVault read service.
/// </summary>
public static class DataVaultReadServiceBridgeExtensions {
  /// <summary>
  /// Reads generated bridge rows that match the requested endpoint hash keys.
  /// </summary>
  /// <param name="readService">The explicit read service that anchors the provider-neutral bridge helper surface.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The bridge read request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The bridge rows in deterministic generated endpoint column order.</returns>
  public static Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultBridgeReadPipeline.ReadBridgeReadRecordsAsync(
        dbContext,
        request,
        cancellationToken);
  }

  /// <summary>
  /// Reads generated bridge rows and maps each selected row through a caller-supplied typed projection delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral bridge helper surface.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The bridge read request.</param>
  /// <param name="projector">The delegate that maps one bridge row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows in deterministic generated endpoint column order.</returns>
  public static async Task<IReadOnlyList<TProjection>> ReadBridgeAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      Func<DataVaultBridgeProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    ArgumentNullException.ThrowIfNull(projector);

    var rows = await DataVaultBridgeReadPipeline.ReadBridgeProjectionRowsAsync(
        dbContext,
        request,
        cancellationToken).ConfigureAwait(false);
    var projections = new TProjection[rows.Count];

    for (var index = 0; index < rows.Count; index++) {
      projections[index] = projector(rows[index]);
    }

    return projections;
  }
}
