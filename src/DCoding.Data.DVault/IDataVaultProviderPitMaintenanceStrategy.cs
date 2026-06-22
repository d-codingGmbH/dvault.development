using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal interface IDataVaultProviderPitMaintenanceStrategy {
  int Priority { get; }

  bool CanRebuild(
      DbContext dbContext,
      DataVaultPitRebuildRequest request);

  Task<DataVaultPitMaintenanceResult> RebuildAsync(
      DataVaultProviderPitMaintenanceStrategyContext context,
      CancellationToken cancellationToken = default);
}
