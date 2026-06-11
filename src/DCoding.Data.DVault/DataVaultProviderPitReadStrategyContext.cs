using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DataVaultProviderPitReadStrategyContext {
  public DataVaultProviderPitReadStrategyContext(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    DbContext = dbContext;
    Request = request;
  }

  public DbContext DbContext { get; }

  public DataVaultPitAsOfReadRequest Request { get; }
}
