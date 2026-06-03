using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class OracleDataVaultReadStrategy : DataVaultRelationalPitBridgeReadStrategy {
  private const int OracleMaxCommandParameterCount = 60000;

  protected override int MaxCommandParameterCount => OracleMaxCommandParameterCount;

  public override bool CanReadPitRows(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(dbContext, request).CanRead;
  }

  public override bool CanReadBridgeRows(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(dbContext, request).CanRead;
  }

  protected override string CreateParameterName(int index) {
    return CreateBareParameterName(index);
  }

  protected override string CreateParameterPlaceholder(int index) {
    return ":" + CreateParameterName(index);
  }

  protected override string QuoteIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}
