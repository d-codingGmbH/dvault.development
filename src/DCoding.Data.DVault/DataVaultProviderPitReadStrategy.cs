using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal interface IDataVaultProviderPitReadStrategy {
  int Priority { get; }

  bool CanReadPitRows(DbContext dbContext, DataVaultPitAsOfReadRequest request);

  Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
      DataVaultProviderPitReadStrategyContext context,
      CancellationToken cancellationToken = default);
}

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
