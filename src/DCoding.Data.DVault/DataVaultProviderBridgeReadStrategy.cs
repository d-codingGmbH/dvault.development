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

internal sealed class DataVaultProviderBridgeReadStrategyContext {
  public DataVaultProviderBridgeReadStrategyContext(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    DbContext = dbContext;
    Request = request;
  }

  public DbContext DbContext { get; }

  public DataVaultBridgeReadRequest Request { get; }
}
