using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DataVaultProviderPitMaintenanceStrategyContext {
  public DataVaultProviderPitMaintenanceStrategyContext(
      DbContext dbContext,
      DataVaultPitRebuildRequest request) {
    DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    Request = request ?? throw new ArgumentNullException(nameof(request));
  }

  public DbContext DbContext { get; }

  public DataVaultPitRebuildRequest Request { get; }
}
