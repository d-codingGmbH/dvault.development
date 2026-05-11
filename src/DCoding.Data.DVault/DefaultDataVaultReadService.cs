using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultReadService : IDataVaultReadService, IDataVaultSatelliteProjectionReadService {
  public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultSatelliteReadPipeline.ReadLatestReadRecordsAsync(
        dbContext,
        request,
        cancellationToken);
  }

  public Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultPitReadPipeline.ReadPitReadRecordsAsync(
        dbContext,
        request,
        cancellationToken);
  }

  Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> IDataVaultSatelliteProjectionReadService.ReadLatestSatelliteProjectionRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(
        dbContext,
        request,
        cancellationToken);
  }
}
