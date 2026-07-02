using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class MySqlDataVaultReadStrategy : DataVaultRelationalPitBridgeReadStrategy {
  private const int MySqlMaxCommandParameterCount = 60000;

  protected override int MaxCommandParameterCount => MySqlMaxCommandParameterCount;

  public override bool CanReadLatestSatelliteRows(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(dbContext, request).CanRead;
  }

  public override bool CanReadPitRows(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(dbContext, request).CanRead;
  }

  public override bool CanReadBridgeRows(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(dbContext, request).CanRead;
  }

  protected override string CreateParameterName(int index) {
    return CreateAtParameterName(index);
  }

  protected override string QuoteIdentifier(string identifier) {
    return "`" + identifier.Replace("`", "``", StringComparison.Ordinal) + "`";
  }

  internal override string CreateLatestRowsCommandText(
      DbContext dbContext,
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      int parentHashKeyCount,
      bool hasAsOf) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(projection);
    ArgumentNullException.ThrowIfNull(selectedColumns);
    if (parentHashKeyCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(parentHashKeyCount));
    }

    const string SourceAlias = "__dvault_source";
    const string LatestAlias = "__dvault_latest";
    const string LatestLoadTimestampColumnName = "__dvault_latest_load_timestamp";

    var builder = new StringBuilder();
    builder.Append("SELECT ");
    AppendQualifiedColumnList(builder, SourceAlias, selectedColumns);
    builder.Append(" FROM ")
        .Append(QuoteTableIdentifier(dbContext, projection.TableName))
        .Append(" AS ")
        .Append(QuoteIdentifier(SourceAlias))
        .Append(" INNER JOIN (SELECT ")
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(", MAX(")
        .Append(QuoteIdentifier(projection.LoadTimestampColumnName))
        .Append(") AS ")
        .Append(QuoteIdentifier(LatestLoadTimestampColumnName))
        .Append(" FROM ")
        .Append(QuoteTableIdentifier(dbContext, projection.TableName))
        .Append(" WHERE ")
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyCount; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateParameterPlaceholder(index));
    }

    builder.Append(')');
    if (hasAsOf) {
      builder.Append(" AND ")
          .Append(QuoteIdentifier(projection.LoadTimestampColumnName))
          .Append(" <= ")
          .Append(CreateParameterPlaceholder(parentHashKeyCount));
    }

    builder.Append(" GROUP BY ")
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(") AS ")
        .Append(QuoteIdentifier(LatestAlias))
        .Append(" ON ")
        .Append(QuoteIdentifier(SourceAlias))
        .Append('.')
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" = ")
        .Append(QuoteIdentifier(LatestAlias))
        .Append('.')
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" AND ")
        .Append(QuoteIdentifier(SourceAlias))
        .Append('.')
        .Append(QuoteIdentifier(projection.LoadTimestampColumnName))
        .Append(" = ")
        .Append(QuoteIdentifier(LatestAlias))
        .Append('.')
        .Append(QuoteIdentifier(LatestLoadTimestampColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteIdentifier(SourceAlias))
        .Append('.')
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName));

    return builder.ToString();
  }

  private void AppendQualifiedColumnList(
      StringBuilder builder,
      string qualifier,
      IReadOnlyList<string> columns) {
    for (var index = 0; index < columns.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteIdentifier(qualifier))
          .Append('.')
          .Append(QuoteIdentifier(columns[index]));
    }
  }
}
