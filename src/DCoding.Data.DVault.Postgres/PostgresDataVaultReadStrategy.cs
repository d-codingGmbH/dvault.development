using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class PostgresDataVaultReadStrategy : DataVaultRelationalPitBridgeReadStrategy {
  private const int PostgresMaxCommandParameterCount = 60000;

  protected override int MaxCommandParameterCount => PostgresMaxCommandParameterCount;

  public override bool CanReadPitRows(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(dbContext, request).CanRead;
  }

  public override bool CanReadBridgeRows(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(dbContext, request).CanRead;
  }

  protected override string CreateParameterName(int index) {
    return CreateAtParameterName(index);
  }

  protected override string QuoteIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}
