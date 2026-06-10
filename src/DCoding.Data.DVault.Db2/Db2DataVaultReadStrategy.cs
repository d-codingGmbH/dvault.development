using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class Db2DataVaultReadStrategy : DataVaultRelationalPitBridgeReadStrategy {
  private const int Db2MaxCommandParameterCount = 30000;

  protected override int MaxCommandParameterCount => Db2MaxCommandParameterCount;

  public override bool CanReadPitRows(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(dbContext, request).CanRead;
  }

  public override bool CanReadBridgeRows(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(dbContext, request).CanRead;
  }

  protected override string CreateParameterName(int index) {
    return CreateAtParameterName(index);
  }

  protected override string QuoteIdentifier(string identifier) {
    var normalizedIdentifier = identifier.ToUpperInvariant();

    return "\"" + normalizedIdentifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  protected override string QuoteTableIdentifier(DbContext dbContext, string tableName) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var entityType = FindEntityType(dbContext, tableName);
    var physicalTableName = entityType?.GetTableName() ?? tableName;
    var schemaName = entityType?.GetSchema();

    return string.IsNullOrWhiteSpace(schemaName)
        ? QuoteIdentifier(physicalTableName)
        : QuoteIdentifier(schemaName!) + "." + QuoteIdentifier(physicalTableName);
  }

  private static IEntityType? FindEntityType(DbContext dbContext, string producedTableName) {
    foreach (var entityType in dbContext.Model.GetEntityTypes()) {
      var producedName = entityType.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string;
      if (string.Equals(producedName, producedTableName, StringComparison.Ordinal)) {
        return entityType;
      }
    }

    foreach (var entityType in dbContext.Model.GetEntityTypes()) {
      if (string.Equals(entityType.GetTableName(), producedTableName, StringComparison.Ordinal) ||
          string.Equals(entityType.Name, producedTableName, StringComparison.Ordinal)) {
        return entityType;
      }
    }

    return null;
  }
}
