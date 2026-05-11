using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines explicit provider-neutral Data Vault read helpers for common latest-row access patterns.
/// </summary>
public interface IDataVaultReadService {
  /// <summary>
  /// Reads the latest satellite rows for the requested parent hash keys, optionally as of one UTC timestamp.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The latest satellite read request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The latest satellite rows grouped by parent hash key and driving-key identity.</returns>
  Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Reads PIT-backed as-of rows for requested parent hub hash keys.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The PIT-backed as-of read request.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The PIT rows that have a visible matched PIT row at or before the requested as-of timestamp.</returns>
  Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      CancellationToken cancellationToken = default);
}
