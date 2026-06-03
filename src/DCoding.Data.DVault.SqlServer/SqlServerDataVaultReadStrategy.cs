using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class SqlServerDataVaultReadStrategy : DataVaultRelationalPitBridgeReadStrategy {
  private const int SqlServerMaxCommandParameterCount = 2100;

  protected override int MaxCommandParameterCount => SqlServerMaxCommandParameterCount;

  public override bool CanReadPitRows(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(dbContext, request).CanRead;
  }

  public override bool CanReadBridgeRows(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(dbContext, request).CanRead;
  }

  protected override string CreateParameterName(int index) {
    return CreateAtParameterName(index);
  }

  protected override string QuoteIdentifier(string identifier) {
    return "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }
}
