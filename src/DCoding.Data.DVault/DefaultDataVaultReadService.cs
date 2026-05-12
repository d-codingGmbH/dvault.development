using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultReadService : IDataVaultReadService, IDataVaultSatelliteProjectionReadService {
  private readonly IReadOnlyList<IDataVaultProviderReadStrategy> _providerReadStrategies;

  public DefaultDataVaultReadService()
      : this([]) {
  }

  public DefaultDataVaultReadService(IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies) {
    ArgumentNullException.ThrowIfNull(providerReadStrategies);

    _providerReadStrategies = providerReadStrategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
  }

  public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    foreach (var strategy in _providerReadStrategies) {
      if (!strategy.CanReadLatestSatelliteRows(dbContext, request)) {
        continue;
      }

      return strategy.ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(dbContext, request),
          cancellationToken);
    }

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

    foreach (var strategy in _providerReadStrategies) {
      if (!strategy.CanReadLatestSatelliteRows(dbContext, request)) {
        continue;
      }

      return strategy.ReadLatestSatelliteProjectionRowsAsync(
          new DataVaultProviderReadStrategyContext(dbContext, request),
          cancellationToken);
    }

    return DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(
        dbContext,
        request,
        cancellationToken);
  }
}
