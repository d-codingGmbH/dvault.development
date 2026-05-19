using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultReadService : IDataVaultReadService, IDataVaultSatelliteProjectionReadService {
  private readonly IReadOnlyList<IDataVaultProviderBridgeReadStrategy> _providerBridgeReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderPitReadStrategy> _providerPitReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderReadStrategy> _providerReadStrategies;

  public DefaultDataVaultReadService()
      : this([], [], []) {
  }

  public DefaultDataVaultReadService(IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies) {
    ArgumentNullException.ThrowIfNull(providerReadStrategies);

    _providerReadStrategies = OrderByPriority(providerReadStrategies);
    _providerPitReadStrategies = [];
    _providerBridgeReadStrategies = [];
  }

  public DefaultDataVaultReadService(
      IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies,
      IEnumerable<IDataVaultProviderPitReadStrategy> providerPitReadStrategies,
      IEnumerable<IDataVaultProviderBridgeReadStrategy> providerBridgeReadStrategies) {
    ArgumentNullException.ThrowIfNull(providerReadStrategies);
    ArgumentNullException.ThrowIfNull(providerPitReadStrategies);
    ArgumentNullException.ThrowIfNull(providerBridgeReadStrategies);

    _providerReadStrategies = OrderByPriority(providerReadStrategies);
    _providerPitReadStrategies = OrderByPriority(providerPitReadStrategies);
    _providerBridgeReadStrategies = OrderByPriority(providerBridgeReadStrategies);
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

    foreach (var strategy in _providerPitReadStrategies) {
      if (!strategy.CanReadPitRows(dbContext, request)) {
        continue;
      }

      return strategy.ReadPitRowsAsync(
          new DataVaultProviderPitReadStrategyContext(dbContext, request),
          cancellationToken);
    }

    return DataVaultPitReadPipeline.ReadPitReadRecordsAsync(
        dbContext,
        request,
        cancellationToken);
  }

  public Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    foreach (var strategy in _providerBridgeReadStrategies) {
      if (!strategy.CanReadBridgeRows(dbContext, request)) {
        continue;
      }

      return strategy.ReadBridgeRowsAsync(
          new DataVaultProviderBridgeReadStrategyContext(dbContext, request),
          cancellationToken);
    }

    return DataVaultBridgeReadPipeline.ReadBridgeReadRecordsAsync(
        dbContext,
        request,
        cancellationToken);
  }

  public Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    foreach (var strategy in _providerBridgeReadStrategies) {
      if (!strategy.CanReadBridgeRows(dbContext, request)) {
        continue;
      }

      return strategy.ReadBridgeProjectionRowsAsync(
          new DataVaultProviderBridgeReadStrategyContext(dbContext, request),
          cancellationToken);
    }

    return DataVaultBridgeReadPipeline.ReadBridgeProjectionRowsAsync(
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

  private static IReadOnlyList<IDataVaultProviderReadStrategy> OrderByPriority(
      IEnumerable<IDataVaultProviderReadStrategy> strategies) {
    return strategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
  }

  private static IReadOnlyList<IDataVaultProviderPitReadStrategy> OrderByPriority(
      IEnumerable<IDataVaultProviderPitReadStrategy> strategies) {
    return strategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
  }

  private static IReadOnlyList<IDataVaultProviderBridgeReadStrategy> OrderByPriority(
      IEnumerable<IDataVaultProviderBridgeReadStrategy> strategies) {
    return strategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
  }
}
