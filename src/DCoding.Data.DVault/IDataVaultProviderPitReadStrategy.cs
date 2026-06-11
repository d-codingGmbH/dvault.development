using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal interface IDataVaultProviderPitReadStrategy {
  int Priority { get; }

  bool CanReadPitRows(DbContext dbContext, DataVaultPitAsOfReadRequest request);

  Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
      DataVaultProviderPitReadStrategyContext context,
      CancellationToken cancellationToken = default);
}
