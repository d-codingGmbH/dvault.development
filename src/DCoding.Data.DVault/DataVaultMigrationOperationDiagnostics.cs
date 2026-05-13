using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DCoding.Data.DVault;

internal static class DataVaultMigrationOperationDiagnostics {
  public static DataVaultDiagnosticsResult Analyze(
      DataVaultDiagnosticsResult baseline,
      IEnumerable<MigrationOperation> operations) {
    ArgumentNullException.ThrowIfNull(baseline);
    ArgumentNullException.ThrowIfNull(operations);

    var schema = DataVaultMigrationSchemaBaseline.Create(baseline.Explain);
    var issues = baseline.Issues
        .Concat(operations.SelectMany(operation => AnalyzeOperation(schema, operation)))
        .ToArray();
    var validationIssues = issues
        .Where(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)
        .ToArray();

    return new DataVaultDiagnosticsResult(
        new DataVaultValidationDiagnostics(validationIssues.Length == 0, validationIssues),
        baseline.Explain,
        baseline.SaveStrategy,
        issues) {
      ReadStrategy = baseline.ReadStrategy,
    };
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeOperation(
      DataVaultMigrationSchemaBaseline schema,
      MigrationOperation operation) {
    ArgumentNullException.ThrowIfNull(operation);

    switch (operation) {
      case AddColumnOperation addColumn:
        return AnalyzeAddColumn(schema, addColumn);
      case DropColumnOperation dropColumn:
        return AnalyzeDropOrAlterColumn(
            schema,
            "DropColumn",
            dropColumn.Table,
            dropColumn.Name,
            action: "drops");
      case AlterColumnOperation alterColumn:
        return AnalyzeDropOrAlterColumn(
            schema,
            "AlterColumn",
            alterColumn.Table,
            alterColumn.Name,
            action: "alters");
      case RenameColumnOperation renameColumn:
        return AnalyzeRenameColumn(schema, renameColumn);
      case CreateIndexOperation createIndex:
        return AnalyzeCreateIndex(schema, createIndex);
      case DropTableOperation dropTable:
        return AnalyzeDropTable(schema, dropTable);
      default:
        return [];
    }
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeAddColumn(
      DataVaultMigrationSchemaBaseline schema,
      AddColumnOperation operation) {
    if (!schema.TryGetEntity(operation.Table, out var entity) ||
        entity.Columns.ContainsKey(operation.Name) ||
        entity.Kind is not (DataVaultTableKind.Hub or DataVaultTableKind.Link)) {
      return [];
    }

    return [CreateIssue(
        "DVM2001",
        "MI-1 violation: migration adds payload column '" + operation.Name +
        "' to Data Vault " + FormatTableKind(entity.Kind) + " table '" + entity.TableName + "'.",
        CreatePath("AddColumn", entity.TableName, operation.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropOrAlterColumn(
      DataVaultMigrationSchemaBaseline schema,
      string operationName,
      string tableName,
      string columnName,
      string action) {
    if (!schema.TryGetEntity(tableName, out var entity) ||
        !entity.Columns.TryGetValue(columnName, out var column)) {
      return [];
    }

    var code = GetDropOrAlterColumnCode(column);
    if (code is null) {
      return [];
    }

    var invariant = code == "DVM2002" ? "MI-2" : "MI-3";
    var shape = code == "DVM2002" ? "required technical column" : "stable key, parent, participant, or driving-key column";

    return [CreateIssue(
        code,
        invariant + " violation: migration " + action + " " + shape + " '" +
        column.Name + "' on Data Vault " + FormatTableKind(entity.Kind) + " table '" + entity.TableName + "'.",
        CreatePath(operationName, entity.TableName, column.Name))];
  }

  private static string? GetDropOrAlterColumnCode(DataVaultMigrationColumnBaseline column) {
    if (column.Role == DataVaultPropertyRole.Technical) {
      return column.TechnicalRole is TechnicalMetadataColumnRole.LoadTimestamp or
          TechnicalMetadataColumnRole.RecordSource or
          TechnicalMetadataColumnRole.HashDiff
          ? "DVM2002"
          : "DVM2003";
    }

    return column.Role is DataVaultPropertyRole.BusinessKey or
        DataVaultPropertyRole.ParticipantReference or
        DataVaultPropertyRole.DrivingKey
        ? "DVM2003"
        : null;
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeRenameColumn(
      DataVaultMigrationSchemaBaseline schema,
      RenameColumnOperation operation) {
    if (!schema.TryGetEntity(operation.Table, out var entity) ||
        !entity.Columns.TryGetValue(operation.Name, out var column) ||
        column.Role == DataVaultPropertyRole.Payload) {
      return [];
    }

    return [CreateIssue(
        "DVM2005",
        "MI-5 violation: migration renames Data Vault-owned column '" + column.Name +
        "' on table '" + entity.TableName + "' away from the produced name.",
        CreatePath("RenameColumn", entity.TableName, column.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeCreateIndex(
      DataVaultMigrationSchemaBaseline schema,
      CreateIndexOperation operation) {
    if (!schema.TryGetEntity(operation.Table, out var entity) ||
        !entity.Indexes.TryGetValue(operation.Name, out var index) ||
        (index.IsUnique == operation.IsUnique &&
            index.PropertyNames.SequenceEqual(operation.Columns, StringComparer.Ordinal))) {
      return [];
    }

    return [CreateIssue(
        "DVM2004",
        "MI-4 violation: migration creates Data Vault default index '" + index.Name +
        "' on table '" + entity.TableName + "' with wrong uniqueness or columns.",
        CreatePath("CreateIndex", entity.TableName, index.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropTable(
      DataVaultMigrationSchemaBaseline schema,
      DropTableOperation operation) {
    if (!schema.TryGetEntity(operation.Name, out var entity) ||
        entity.Kind is not (DataVaultTableKind.Hub or DataVaultTableKind.Link or DataVaultTableKind.Satellite)) {
      return [];
    }

    return [CreateIssue(
        "DVM2006",
        "MI-5 violation: migration drops Data Vault-produced table '" + entity.TableName + "'.",
        CreatePath("DropTable", entity.TableName))];
  }

  private static DataVaultDiagnosticsIssue CreateIssue(
      string code,
      string message,
      string path) {
    var definition = DataVaultDiagnosticCatalog.GetMigrationOperationDefinition(code);
    return new DataVaultDiagnosticsIssue(ToIssueSeverity(definition.Severity), definition.Code, message, path);
  }

  private static DataVaultDiagnosticsIssueSeverity ToIssueSeverity(string severity) {
    return severity switch {
      "error" => DataVaultDiagnosticsIssueSeverity.Error,
      "warning" => DataVaultDiagnosticsIssueSeverity.Warning,
      _ => throw new InvalidOperationException("Unsupported migration diagnostic severity '" + severity + "'."),
    };
  }

  private static string CreatePath(string operationName, string targetName, string? memberName = null) {
    return string.IsNullOrWhiteSpace(memberName)
        ? "migration/" + operationName + "/" + targetName
        : "migration/" + operationName + "/" + targetName + "/" + memberName;
  }

  private static string FormatTableKind(DataVaultTableKind kind) {
    return kind.ToString().ToLowerInvariant();
  }

  private sealed class DataVaultMigrationSchemaBaseline {
    private DataVaultMigrationSchemaBaseline(IReadOnlyDictionary<string, DataVaultMigrationEntityBaseline> entities) {
      Entities = entities;
    }

    private IReadOnlyDictionary<string, DataVaultMigrationEntityBaseline> Entities { get; }

    public static DataVaultMigrationSchemaBaseline Create(DataVaultExplainDiagnostics explain) {
      ArgumentNullException.ThrowIfNull(explain);

      var entities = explain.Entities
          .Where(entity => entity.TableKind is DataVaultTableKind.Hub or DataVaultTableKind.Link or DataVaultTableKind.Satellite)
          .Select(DataVaultMigrationEntityBaseline.Create)
          .ToDictionary(entity => entity.TableName, StringComparer.Ordinal);

      return new DataVaultMigrationSchemaBaseline(entities);
    }

    public bool TryGetEntity(string tableName, out DataVaultMigrationEntityBaseline entity) {
      ArgumentException.ThrowIfNullOrWhiteSpace(tableName);

      return Entities.TryGetValue(tableName, out entity!);
    }
  }

  private sealed class DataVaultMigrationEntityBaseline {
    private DataVaultMigrationEntityBaseline(
        string tableName,
        DataVaultTableKind kind,
        IReadOnlyDictionary<string, DataVaultMigrationColumnBaseline> columns,
        IReadOnlyDictionary<string, DataVaultMigrationIndexBaseline> indexes) {
      TableName = tableName;
      Kind = kind;
      Columns = columns;
      Indexes = indexes;
    }

    public string TableName { get; }

    public DataVaultTableKind Kind { get; }

    public IReadOnlyDictionary<string, DataVaultMigrationColumnBaseline> Columns { get; }

    public IReadOnlyDictionary<string, DataVaultMigrationIndexBaseline> Indexes { get; }

    public static DataVaultMigrationEntityBaseline Create(DataVaultEntityExplain entity) {
      var columns = entity.Properties
          .Select(DataVaultMigrationColumnBaseline.Create)
          .ToDictionary(column => column.Name, StringComparer.Ordinal);
      var indexes = entity.Indexes
          .Select(DataVaultMigrationIndexBaseline.Create)
          .ToDictionary(index => index.Name, StringComparer.Ordinal);

      return new DataVaultMigrationEntityBaseline(entity.TableName, entity.TableKind, columns, indexes);
    }
  }

  private sealed record DataVaultMigrationColumnBaseline(
      string Name,
      DataVaultPropertyRole Role,
      TechnicalMetadataColumnRole? TechnicalRole) {
    public static DataVaultMigrationColumnBaseline Create(DataVaultPropertyExplain property) {
      return new DataVaultMigrationColumnBaseline(property.Name, property.Role, property.TechnicalRole);
    }
  }

  private sealed record DataVaultMigrationIndexBaseline(
      string Name,
      IReadOnlyList<string> PropertyNames,
      bool IsUnique) {
    public static DataVaultMigrationIndexBaseline Create(DataVaultIndexExplain index) {
      return new DataVaultMigrationIndexBaseline(index.Name, index.PropertyNames, index.IsUnique);
    }
  }
}
