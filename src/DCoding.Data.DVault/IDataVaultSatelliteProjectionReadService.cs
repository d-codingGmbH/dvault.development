using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal interface IDataVaultSatelliteProjectionReadService {
  Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken);
}
