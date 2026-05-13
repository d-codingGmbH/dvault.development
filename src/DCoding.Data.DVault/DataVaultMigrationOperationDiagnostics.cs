using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DCoding.Data.DVault;

/// <summary>
/// Analyzes generated EF Core migration operations against a Data Vault diagnostics explain baseline.
/// </summary>
public static class DataVaultMigrationOperationDiagnostics {
  /// <summary>
  /// Analyzes EF Core migration operations and returns the diagnostics result with migration guardrail findings appended.
  /// </summary>
  /// <param name="baseline">The Data Vault diagnostics baseline produced from metadata, registry, code-first declarations, or a DbContext.</param>
  /// <param name="operations">The generated EF Core migration operations to inspect before applying a migration.</param>
  /// <returns>The diagnostics result containing baseline and migration-operation issues.</returns>
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

  /// <summary>
  /// Analyzes EF Core migration operations and returns an automation-friendly guardrail report with remediation guidance.
  /// </summary>
  /// <param name="baseline">The Data Vault diagnostics baseline produced from metadata, registry, code-first declarations, or a DbContext.</param>
  /// <param name="operations">The generated EF Core migration operations to inspect before applying a migration.</param>
  /// <returns>A structured guardrail report for local scripts, tests, or build steps.</returns>
  public static DataVaultMigrationGuardrailReport AnalyzeReport(
      DataVaultDiagnosticsResult baseline,
      IEnumerable<MigrationOperation> operations) {
    return DataVaultMigrationGuardrailReport.Create(Analyze(baseline, operations));
  }

  /// <summary>
  /// Builds a metadata-model baseline, analyzes EF Core migration operations, and returns a guardrail report.
  /// </summary>
  /// <param name="diagnostics">The diagnostics service used to build the Data Vault explain baseline.</param>
  /// <param name="metadataModel">The provider-neutral Data Vault metadata model to use as the schema baseline.</param>
  /// <param name="operations">The generated EF Core migration operations to inspect before applying a migration.</param>
  /// <returns>A structured guardrail report for local scripts, tests, or build steps.</returns>
  public static DataVaultMigrationGuardrailReport AnalyzeReport(
      IDataVaultDiagnosticsService diagnostics,
      DataVaultMetadataModel metadataModel,
      IEnumerable<MigrationOperation> operations) {
    ArgumentNullException.ThrowIfNull(diagnostics);

    return AnalyzeReport(diagnostics.Analyze(metadataModel), operations);
  }

  /// <summary>
  /// Builds a metadata-registry baseline, analyzes EF Core migration operations, and returns a guardrail report.
  /// </summary>
  /// <param name="diagnostics">The diagnostics service used to build the Data Vault explain baseline.</param>
  /// <param name="metadataRegistry">The Data Vault metadata registry to use as the schema baseline.</param>
  /// <param name="operations">The generated EF Core migration operations to inspect before applying a migration.</param>
  /// <returns>A structured guardrail report for local scripts, tests, or build steps.</returns>
  public static DataVaultMigrationGuardrailReport AnalyzeReport(
      IDataVaultDiagnosticsService diagnostics,
      DataVaultMetadataRegistry metadataRegistry,
      IEnumerable<MigrationOperation> operations) {
    ArgumentNullException.ThrowIfNull(diagnostics);

    return AnalyzeReport(diagnostics.Analyze(metadataRegistry), operations);
  }

  /// <summary>
  /// Builds a code-first metadata baseline, analyzes EF Core migration operations, and returns a guardrail report.
  /// </summary>
  /// <param name="diagnostics">The diagnostics service used to build the Data Vault explain baseline.</param>
  /// <param name="configureModel">The code-first Data Vault metadata declaration callback to use as the schema baseline.</param>
  /// <param name="operations">The generated EF Core migration operations to inspect before applying a migration.</param>
  /// <returns>A structured guardrail report for local scripts, tests, or build steps.</returns>
  public static DataVaultMigrationGuardrailReport AnalyzeReport(
      IDataVaultDiagnosticsService diagnostics,
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      IEnumerable<MigrationOperation> operations) {
    ArgumentNullException.ThrowIfNull(diagnostics);

    return AnalyzeReport(diagnostics.Analyze(configureModel), operations);
  }

  /// <summary>
  /// Builds a configured DbContext baseline, analyzes EF Core migration operations, and returns a guardrail report.
  /// </summary>
  /// <param name="diagnostics">The diagnostics service used to read the Data Vault explain baseline from the DbContext model.</param>
  /// <param name="dbContext">The configured DbContext whose design-time model carries the Data Vault schema baseline.</param>
  /// <param name="operations">The generated EF Core migration operations to inspect before applying a migration.</param>
  /// <returns>A structured guardrail report for local scripts, tests, or build steps.</returns>
  public static DataVaultMigrationGuardrailReport AnalyzeReport(
      IDataVaultDiagnosticsService diagnostics,
      DbContext dbContext,
      IEnumerable<MigrationOperation> operations) {
    ArgumentNullException.ThrowIfNull(diagnostics);

    return AnalyzeReport(diagnostics.Analyze(dbContext), operations);
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
      case DropIndexOperation dropIndex:
        return AnalyzeDropIndex(schema, dropIndex);
      case RenameIndexOperation renameIndex:
        return AnalyzeRenameIndex(schema, renameIndex);
      case AddPrimaryKeyOperation addPrimaryKey:
        return AnalyzeAddPrimaryKey(schema, addPrimaryKey);
      case DropPrimaryKeyOperation dropPrimaryKey:
        return AnalyzeDropPrimaryKey(schema, dropPrimaryKey);
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
        entity.Columns.ContainsKey(operation.Name)) {
      return [];
    }

    if (entity.Kind is DataVaultTableKind.Hub or DataVaultTableKind.Link) {
      return [CreateIssue(
          "DVM2001",
          "MI-1 violation: migration adds payload column '" + operation.Name +
          "' to Data Vault " + FormatTableKind(entity.Kind) + " table '" + entity.TableName + "'.",
          CreatePath("AddColumn", entity.TableName, operation.Name))];
    }

    if (entity.Kind is not (DataVaultTableKind.Pit or DataVaultTableKind.Bridge)) {
      return [];
    }

    return [CreateIssue(
        "DVM2003",
        "MI-3 violation: migration adds unsupported structural column '" +
        operation.Name + "' to Data Vault " + FormatTableKind(entity.Kind) +
        " table '" + entity.TableName + "'.",
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
    var shape = code == "DVM2002"
        ? "required technical column"
        : "stable key, parent, participant, driving-key, snapshot-reference, or bridge-depth column";

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
        DataVaultPropertyRole.DrivingKey or
        DataVaultPropertyRole.SnapshotReference or
        DataVaultPropertyRole.BridgeDepth
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

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropIndex(
      DataVaultMigrationSchemaBaseline schema,
      DropIndexOperation operation) {
    if (string.IsNullOrWhiteSpace(operation.Table) ||
        !schema.TryGetEntity(operation.Table, out var entity) ||
        !entity.Indexes.TryGetValue(operation.Name, out var index)) {
      return [];
    }

    return [CreateIssue(
        "DVM2004",
        "MI-4 violation: migration drops Data Vault default index '" + index.Name +
        "' from table '" + entity.TableName + "'.",
        CreatePath("DropIndex", entity.TableName, index.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeRenameIndex(
      DataVaultMigrationSchemaBaseline schema,
      RenameIndexOperation operation) {
    if (string.IsNullOrWhiteSpace(operation.Table) ||
        !schema.TryGetEntity(operation.Table, out var entity) ||
        !entity.Indexes.TryGetValue(operation.Name, out var index)) {
      return [];
    }

    return [CreateIssue(
        "DVM2004",
        "MI-4 violation: migration renames Data Vault default index '" + index.Name +
        "' on table '" + entity.TableName + "' away from the produced name.",
        CreatePath("RenameIndex", entity.TableName, index.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeAddPrimaryKey(
      DataVaultMigrationSchemaBaseline schema,
      AddPrimaryKeyOperation operation) {
    if (!schema.TryGetEntity(operation.Table, out var entity)) {
      return [];
    }

    var operationColumns = operation.Columns ?? Array.Empty<string>();
    if (string.Equals(operation.Name, entity.PrimaryKey.Name, StringComparison.Ordinal) &&
        entity.PrimaryKey.PropertyNames.SequenceEqual(operationColumns, StringComparer.Ordinal)) {
      return [];
    }

    var operationName = string.IsNullOrWhiteSpace(operation.Name)
        ? "<unnamed>"
        : operation.Name;

    return [CreateIssue(
        "DVM2004",
        "MI-4 violation: migration creates Data Vault primary key '" + operationName +
        "' on table '" + entity.TableName + "' with wrong name or columns; expected '" +
        entity.PrimaryKey.Name + "' on columns [" +
        string.Join(", ", entity.PrimaryKey.PropertyNames) + "].",
        CreatePath("AddPrimaryKey", entity.TableName, operationName))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropPrimaryKey(
      DataVaultMigrationSchemaBaseline schema,
      DropPrimaryKeyOperation operation) {
    if (!schema.TryGetEntity(operation.Table, out var entity) ||
        !string.Equals(operation.Name, entity.PrimaryKey.Name, StringComparison.Ordinal)) {
      return [];
    }

    return [CreateIssue(
        "DVM2004",
        "MI-4 violation: migration drops Data Vault primary key '" + entity.PrimaryKey.Name +
        "' from table '" + entity.TableName + "'.",
        CreatePath("DropPrimaryKey", entity.TableName, entity.PrimaryKey.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropTable(
      DataVaultMigrationSchemaBaseline schema,
      DropTableOperation operation) {
    if (!schema.TryGetEntity(operation.Name, out var entity)) {
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
          .Where(entity => entity.TableKind is
              DataVaultTableKind.Hub or
              DataVaultTableKind.Link or
              DataVaultTableKind.Satellite or
              DataVaultTableKind.Pit or
              DataVaultTableKind.Bridge)
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
        DataVaultMigrationKeyBaseline primaryKey,
        IReadOnlyDictionary<string, DataVaultMigrationIndexBaseline> indexes) {
      TableName = tableName;
      Kind = kind;
      Columns = columns;
      PrimaryKey = primaryKey;
      Indexes = indexes;
    }

    public string TableName { get; }

    public DataVaultTableKind Kind { get; }

    public IReadOnlyDictionary<string, DataVaultMigrationColumnBaseline> Columns { get; }

    public DataVaultMigrationKeyBaseline PrimaryKey { get; }

    public IReadOnlyDictionary<string, DataVaultMigrationIndexBaseline> Indexes { get; }

    public static DataVaultMigrationEntityBaseline Create(DataVaultEntityExplain entity) {
      var columns = entity.Properties
          .Select(DataVaultMigrationColumnBaseline.Create)
          .ToDictionary(column => column.Name, StringComparer.Ordinal);
      var primaryKey = DataVaultMigrationKeyBaseline.Create(entity.PrimaryKey);
      var indexes = entity.Indexes
          .Select(DataVaultMigrationIndexBaseline.Create)
          .ToDictionary(index => index.Name, StringComparer.Ordinal);

      return new DataVaultMigrationEntityBaseline(entity.TableName, entity.TableKind, columns, primaryKey, indexes);
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

  private sealed record DataVaultMigrationKeyBaseline(
      string Name,
      IReadOnlyList<string> PropertyNames) {
    public static DataVaultMigrationKeyBaseline Create(DataVaultKeyExplain key) {
      return new DataVaultMigrationKeyBaseline(key.Name, key.PropertyNames);
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
