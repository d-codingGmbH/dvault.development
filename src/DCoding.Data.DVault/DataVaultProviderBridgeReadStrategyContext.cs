using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

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
