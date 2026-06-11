using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal interface IDataVaultProviderBridgeReadStrategy {
  int Priority { get; }

  bool CanReadBridgeRows(DbContext dbContext, DataVaultBridgeReadRequest request);

  Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      CancellationToken cancellationToken = default);

  Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      CancellationToken cancellationToken = default);
}
