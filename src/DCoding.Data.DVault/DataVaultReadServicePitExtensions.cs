using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides typed projection helpers over the provider-neutral Data Vault PIT-backed as-of read path.
/// </summary>
public static class DataVaultReadServicePitExtensions {
  /// <summary>
  /// Reads PIT-backed as-of rows and maps each selected row through a caller-supplied typed projection delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral PIT read helper surface.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The PIT-backed as-of read request.</param>
  /// <param name="projector">The delegate that maps one selected PIT row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows for parent hash keys with a visible PIT row.</returns>
  public static async Task<IReadOnlyList<TProjection>> ReadPitAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      Func<DataVaultPitProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    ArgumentNullException.ThrowIfNull(projector);

    var rows = readService is DefaultDataVaultReadService
        ? await readService.ReadPitRowsAsync(
            dbContext,
            request,
            cancellationToken).ConfigureAwait(false)
        : await DataVaultActivityTracing.TraceReadAsync(
            dbContext,
            DataVaultReadTelemetryFamily.Pit,
            DataVaultActivityTracing.ReadModeAsOf,
            request.ParentHashKeys.Count,
            () => readService.ReadPitRowsAsync(dbContext, request, cancellationToken)).ConfigureAwait(false);
    var projections = new TProjection[rows.Count];

    for (var index = 0; index < rows.Count; index++) {
      projections[index] = projector(DataVaultPitProjectionRow.FromReadRecord(request.Pit.Name, rows[index]));
    }

    return projections;
  }
}
