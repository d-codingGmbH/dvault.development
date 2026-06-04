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
    return AnalyzeCore(baseline, operations).Diagnostics;
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
    var analysis = AnalyzeCore(baseline, operations);
    return DataVaultMigrationGuardrailReport.Create(
        analysis.Diagnostics,
        analysis.OperationSummaries);
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

  private static DataVaultMigrationOperationAnalysis AnalyzeCore(
      DataVaultDiagnosticsResult baseline,
      IEnumerable<MigrationOperation> operations) {
    ArgumentNullException.ThrowIfNull(baseline);
    ArgumentNullException.ThrowIfNull(operations);

    var schema = DataVaultMigrationSchemaBaseline.Create(baseline.Explain);
    var operationSet = operations.ToArray();
    var operationContext = DataVaultMigrationOperationContext.Create(schema, operationSet);
    var operationIssues = new List<DataVaultDiagnosticsIssue>();
    var operationSummaries = new List<DataVaultMigrationGuardrailOperationSummary>();
    var ordinal = 0;

    foreach (var operation in operationSet) {
      var currentIssues = AnalyzeOperation(schema, operationContext, operation).ToArray();
      operationIssues.AddRange(currentIssues);
      operationSummaries.Add(CreateOperationSummary(ordinal, operation, currentIssues));
      ordinal++;
    }

    var issues = baseline.Issues
        .Concat(operationIssues)
        .ToArray();
    var validationIssues = issues
        .Where(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)
        .ToArray();
    var diagnostics = new DataVaultDiagnosticsResult(
        new DataVaultValidationDiagnostics(validationIssues.Length == 0, validationIssues),
        baseline.Explain,
        baseline.SaveStrategy,
        issues) {
      ReadStrategy = baseline.ReadStrategy,
    };

    return new DataVaultMigrationOperationAnalysis(diagnostics, operationSummaries);
  }

  private static DataVaultMigrationGuardrailOperationSummary CreateOperationSummary(
      int ordinal,
      MigrationOperation operation,
      IReadOnlyList<DataVaultDiagnosticsIssue> issues) {
    var descriptor = DescribeOperation(operation);
    var guardrailIssues = issues
        .Select(DataVaultMigrationGuardrailIssue.Create)
        .ToArray();

    return new DataVaultMigrationGuardrailOperationSummary(
        ordinal,
        descriptor.OperationName,
        descriptor.TargetName,
        descriptor.MemberName,
        descriptor.Path,
        GetOperationOutcome(guardrailIssues),
        guardrailIssues);
  }

  private static DataVaultMigrationGuardrailOperationOutcome GetOperationOutcome(
      IReadOnlyList<DataVaultMigrationGuardrailIssue> issues) {
    if (issues.Any(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)) {
      return DataVaultMigrationGuardrailOperationOutcome.Incompatible;
    }

    return issues.Any(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Warning)
        ? DataVaultMigrationGuardrailOperationOutcome.Risky
        : DataVaultMigrationGuardrailOperationOutcome.Safe;
  }

  private static MigrationOperationDescriptor DescribeOperation(MigrationOperation operation) {
    ArgumentNullException.ThrowIfNull(operation);

    return operation switch {
      CreateTableOperation createTable => CreateOperationDescriptor("CreateTable", createTable.Name, memberName: null),
      AddColumnOperation addColumn => CreateOperationDescriptor("AddColumn", addColumn.Table, addColumn.Name),
      DropColumnOperation dropColumn => CreateOperationDescriptor("DropColumn", dropColumn.Table, dropColumn.Name),
      AlterColumnOperation alterColumn => CreateOperationDescriptor("AlterColumn", alterColumn.Table, alterColumn.Name),
      RenameColumnOperation renameColumn => CreateOperationDescriptor("RenameColumn", renameColumn.Table, renameColumn.Name),
      CreateIndexOperation createIndex => CreateOperationDescriptor("CreateIndex", createIndex.Table, createIndex.Name),
      DropIndexOperation dropIndex => CreateOperationDescriptor("DropIndex", dropIndex.Table, dropIndex.Name),
      RenameIndexOperation renameIndex => CreateOperationDescriptor("RenameIndex", renameIndex.Table, renameIndex.Name),
      AddPrimaryKeyOperation addPrimaryKey => CreateOperationDescriptor(
          "AddPrimaryKey",
          addPrimaryKey.Table,
          NormalizeName(addPrimaryKey.Name) ?? "<unnamed>"),
      DropPrimaryKeyOperation dropPrimaryKey => CreateOperationDescriptor("DropPrimaryKey", dropPrimaryKey.Table, dropPrimaryKey.Name),
      DropTableOperation dropTable => CreateOperationDescriptor("DropTable", dropTable.Name, memberName: null),
      RenameTableOperation renameTable => CreateOperationDescriptor("RenameTable", renameTable.Name, renameTable.NewName),
      _ => CreateOperationDescriptor(GetOperationName(operation), targetName: null, memberName: null),
    };
  }

  private static MigrationOperationDescriptor CreateOperationDescriptor(
      string operationName,
      string? targetName,
      string? memberName) {
    var normalizedTargetName = NormalizeName(targetName);
    var normalizedMemberName = NormalizeName(memberName);

    return new MigrationOperationDescriptor(
        operationName,
        normalizedTargetName,
        normalizedMemberName,
        CreateOperationPath(operationName, normalizedTargetName, normalizedMemberName));
  }

  private static string CreateOperationPath(
      string operationName,
      string? targetName,
      string? memberName) {
    if (string.IsNullOrWhiteSpace(targetName)) {
      return string.IsNullOrWhiteSpace(memberName)
          ? "migration/" + operationName
          : "migration/" + operationName + "/" + memberName;
    }

    return string.IsNullOrWhiteSpace(memberName)
        ? "migration/" + operationName + "/" + targetName
        : "migration/" + operationName + "/" + targetName + "/" + memberName;
  }

  private static string? NormalizeName(string? value) {
    return string.IsNullOrWhiteSpace(value) ? null : value;
  }

  private static string GetOperationName(MigrationOperation operation) {
    const string operationSuffix = "Operation";
    var name = operation.GetType().Name;
    return name.EndsWith(operationSuffix, StringComparison.Ordinal)
        ? name[..^operationSuffix.Length]
        : name;
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeOperation(
      DataVaultMigrationSchemaBaseline schema,
      DataVaultMigrationOperationContext operationContext,
      MigrationOperation operation) {
    ArgumentNullException.ThrowIfNull(operation);

    switch (operation) {
      case CreateTableOperation createTable:
        return AnalyzeCreateTable(schema, createTable);
      case AddColumnOperation addColumn:
        return AnalyzeAddColumn(schema, addColumn);
      case DropColumnOperation dropColumn:
        return AnalyzeDropColumn(
            schema,
            operationContext,
            dropColumn);
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
        return AnalyzeDropIndex(schema, operationContext, dropIndex);
      case RenameIndexOperation renameIndex:
        return AnalyzeRenameIndex(schema, renameIndex);
      case AddPrimaryKeyOperation addPrimaryKey:
        return AnalyzeAddPrimaryKey(schema, addPrimaryKey);
      case DropPrimaryKeyOperation dropPrimaryKey:
        return AnalyzeDropPrimaryKey(schema, operationContext, dropPrimaryKey);
      case DropTableOperation dropTable:
        return AnalyzeDropTable(schema, operationContext, dropTable);
      case RenameTableOperation renameTable:
        return AnalyzeRenameTable(schema, renameTable);
      default:
        return [];
    }
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeCreateTable(
      DataVaultMigrationSchemaBaseline schema,
      CreateTableOperation operation) {
    if (!schema.TryGetEntity(operation.Name, out var entity)) {
      return [];
    }

    var issues = new List<DataVaultDiagnosticsIssue>();
    var operationColumnNames = operation.Columns
        .Select(column => column.Name)
        .Where(columnName => !string.IsNullOrWhiteSpace(columnName))
        .ToHashSet(StringComparer.Ordinal);

    foreach (var column in entity.ColumnsInOrder) {
      if (operationColumnNames.Contains(column.Name)) {
        continue;
      }

      var code = GetDropColumnCode(column);
      if (code is null) {
        continue;
      }

      var shape = code == "DVM2002"
          ? "required technical column"
          : "generated structural or payload column";
      var invariant = code == "DVM2002" ? "MI-2" : "MI-3";
      issues.Add(CreateIssue(
          code,
          invariant + " violation: migration creates " + FormatEntityContext(entity) +
          " without " + shape + " " + FormatColumnContext(column) + ".",
          CreatePath("CreateTable", entity.TableName, column.Name)));
    }

    foreach (var operationColumn in operation.Columns) {
      if (string.IsNullOrWhiteSpace(operationColumn.Name) ||
          entity.Columns.ContainsKey(operationColumn.Name)) {
        continue;
      }

      var unexpectedColumnIssue = CreateUnexpectedCreateTableColumnIssue(entity, operationColumn.Name);
      if (unexpectedColumnIssue is not null) {
        issues.Add(unexpectedColumnIssue);
      }
    }

    if (operation.PrimaryKey is not null) {
      issues.AddRange(AnalyzeCreateTablePrimaryKey(entity, operation.PrimaryKey));
    }

    return issues;
  }

  private static DataVaultDiagnosticsIssue? CreateUnexpectedCreateTableColumnIssue(
      DataVaultMigrationEntityBaseline entity,
      string columnName) {
    if (entity.Kind is DataVaultTableKind.Hub or DataVaultTableKind.Link) {
      return CreateIssue(
          "DVM2001",
          "MI-1 violation: migration creates " + FormatEntityContext(entity) +
          " with unexpected payload column '" + columnName + "'.",
          CreatePath("CreateTable", entity.TableName, columnName));
    }

    if (entity.Kind is not (DataVaultTableKind.Pit or DataVaultTableKind.Bridge)) {
      return null;
    }

    return CreateIssue(
        "DVM2003",
        "MI-3 violation: migration creates " + FormatEntityContext(entity) +
        " with unsupported structural column '" + columnName + "'.",
        CreatePath("CreateTable", entity.TableName, columnName));
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeCreateTablePrimaryKey(
      DataVaultMigrationEntityBaseline entity,
      AddPrimaryKeyOperation primaryKey) {
    var operationColumns = primaryKey.Columns ?? Array.Empty<string>();
    if (string.Equals(primaryKey.Name, entity.PrimaryKey.Name, StringComparison.Ordinal) &&
        entity.PrimaryKey.PropertyNames.SequenceEqual(operationColumns, StringComparer.Ordinal)) {
      return [];
    }

    var operationName = string.IsNullOrWhiteSpace(primaryKey.Name)
        ? "<unnamed>"
        : primaryKey.Name;

    return [CreateIssue(
        "DVM2004",
        "MI-4 violation: migration creates " + FormatEntityContext(entity) +
        " with inline primary key '" + operationName +
        "' with wrong name or columns; expected " + FormatPrimaryKeyContext(entity.PrimaryKey) + ".",
        CreatePath("CreateTable", entity.TableName, operationName))];
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
          "' to " + FormatEntityContext(entity) + ".",
          CreatePath("AddColumn", entity.TableName, operation.Name))];
    }

    if (entity.Kind is not (DataVaultTableKind.Pit or DataVaultTableKind.Bridge)) {
      return [];
    }

    return [CreateIssue(
        "DVM2003",
        "MI-3 violation: migration adds unsupported structural column '" +
        operation.Name + "' to " + FormatEntityContext(entity) + ".",
        CreatePath("AddColumn", entity.TableName, operation.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropColumn(
      DataVaultMigrationSchemaBaseline schema,
      DataVaultMigrationOperationContext operationContext,
      DropColumnOperation operation) {
    if (!schema.TryGetEntity(operation.Table, out var entity)) {
      return [];
    }

    if (entity.Columns.TryGetValue(operation.Name, out var column)) {
      return AnalyzeGeneratedColumnChange(
          entity,
          column,
          "DropColumn",
          action: "drops",
          code: GetDropColumnCode(column));
    }

    if (operationContext.TryFindSuspiciousColumnReplacement(operation, out var replacement)) {
      return [CreateIssue(
          "DVM2008",
          "MI-5 suspicious drift: migration drops column '" + operation.Name +
          "' from " + FormatEntityContext(entity) +
          " and adds generated " + FormatColumnContext(replacement.Column) +
          " in the same migration instead of preserving an explicit EF rename or metadata-evolution operation.",
          CreatePath("DropColumn", entity.TableName, operation.Name))];
    }

    return [];
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

    return AnalyzeGeneratedColumnChange(
        entity,
        column,
        operationName,
        action,
        GetAlterColumnCode(column));
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeGeneratedColumnChange(
      DataVaultMigrationEntityBaseline entity,
      DataVaultMigrationColumnBaseline column,
      string operationName,
      string action,
      string? code) {
    if (code is null) {
      return [];
    }

    var invariant = code == "DVM2002" ? "MI-2" : "MI-3";
    var shape = code == "DVM2002"
        ? "required technical column"
        : "generated structural or payload column";

    return [CreateIssue(
        code,
        invariant + " violation: migration " + action + " " + shape + " " +
        FormatColumnContext(column) + " on " + FormatEntityContext(entity) + ".",
        CreatePath(operationName, entity.TableName, column.Name))];
  }

  private static string? GetDropColumnCode(DataVaultMigrationColumnBaseline column) {
    if (column.Role == DataVaultPropertyRole.Technical) {
      return column.TechnicalRole is TechnicalMetadataColumnRole.LoadTimestamp or
          TechnicalMetadataColumnRole.RecordSource or
          TechnicalMetadataColumnRole.HashDiff
          ? "DVM2002"
          : "DVM2003";
    }

    return column.Role is DataVaultPropertyRole.BusinessKey or
        DataVaultPropertyRole.ParticipantReference or
        DataVaultPropertyRole.Payload or
        DataVaultPropertyRole.DrivingKey or
        DataVaultPropertyRole.SnapshotReference or
        DataVaultPropertyRole.BridgeDepth
        ? "DVM2003"
        : null;
  }

  private static string? GetAlterColumnCode(DataVaultMigrationColumnBaseline column) {
    if (column.Role == DataVaultPropertyRole.Payload) {
      return null;
    }

    return GetDropColumnCode(column);
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
        "MI-5 explicit rename: migration renames Data Vault-owned " + FormatColumnContext(column) +
        " on " + FormatEntityContext(entity) + " away from the produced name.",
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
        "MI-4 violation: migration creates generated index '" + operation.Name +
        "' on " + FormatEntityContext(entity) +
        " with wrong uniqueness or columns; expected " + FormatIndexContext(index) + ".",
        CreatePath("CreateIndex", entity.TableName, index.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropIndex(
      DataVaultMigrationSchemaBaseline schema,
      DataVaultMigrationOperationContext operationContext,
      DropIndexOperation operation) {
    if (string.IsNullOrWhiteSpace(operation.Table) ||
        !schema.TryGetEntity(operation.Table, out var entity)) {
      return [];
    }

    if (!entity.Indexes.TryGetValue(operation.Name, out var index)) {
      if (operationContext.TryFindSuspiciousIndexReplacement(operation, out var replacement)) {
        return [CreateIssue(
            "DVM2008",
            "MI-5 suspicious drift: migration drops index '" + operation.Name +
            "' from " + FormatEntityContext(entity) +
            " and creates generated " + FormatIndexContext(replacement.Index) +
            " in the same migration instead of preserving an explicit EF rename or metadata-evolution operation.",
            CreatePath("DropIndex", entity.TableName, operation.Name))];
      }

      return [];
    }

    return [CreateIssue(
        "DVM2007",
        "MI-4 destructive change: migration drops generated secondary index " +
        FormatIndexContext(index) + " from " + FormatEntityContext(entity) + ".",
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
        "MI-4 explicit rename: migration renames generated secondary index " +
        FormatIndexContext(index) + " on " + FormatEntityContext(entity) + " away from the produced name.",
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
        "' on " + FormatEntityContext(entity) + " with wrong name or columns; expected " +
        FormatPrimaryKeyContext(entity.PrimaryKey) + ".",
        CreatePath("AddPrimaryKey", entity.TableName, operationName))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropPrimaryKey(
      DataVaultMigrationSchemaBaseline schema,
      DataVaultMigrationOperationContext operationContext,
      DropPrimaryKeyOperation operation) {
    if (!schema.TryGetEntity(operation.Table, out var entity)) {
      return [];
    }

    if (!string.Equals(operation.Name, entity.PrimaryKey.Name, StringComparison.Ordinal)) {
      if (operationContext.TryFindSuspiciousPrimaryKeyReplacement(operation, out var replacement)) {
        return [CreateIssue(
            "DVM2008",
            "MI-5 suspicious drift: migration drops primary key '" + operation.Name +
            "' from " + FormatEntityContext(entity) +
            " and creates generated " + FormatPrimaryKeyContext(replacement.PrimaryKey) +
            " in the same migration instead of preserving an explicit EF rename or metadata-evolution operation.",
            CreatePath("DropPrimaryKey", entity.TableName, operation.Name))];
      }

      return [];
    }

    return [CreateIssue(
        "DVM2007",
        "MI-4 destructive change: migration drops generated primary-key constraint " +
        FormatPrimaryKeyContext(entity.PrimaryKey) + " from " + FormatEntityContext(entity) + ".",
        CreatePath("DropPrimaryKey", entity.TableName, entity.PrimaryKey.Name))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeDropTable(
      DataVaultMigrationSchemaBaseline schema,
      DataVaultMigrationOperationContext operationContext,
      DropTableOperation operation) {
    if (!schema.TryGetEntity(operation.Name, out var entity)) {
      if (operationContext.TryFindSuspiciousTableReplacement(operation, out var replacement)) {
        return [CreateIssue(
            "DVM2008",
            "MI-5 suspicious drift: migration drops table '" + operation.Name +
            "' and creates " + FormatEntityContext(replacement.Entity) +
            " in the same migration instead of preserving an explicit EF rename or metadata-evolution operation.",
            CreatePath("DropTable", operation.Name))];
      }

      return [];
    }

    return [CreateIssue(
        "DVM2006",
        "MI-5 destructive change: migration drops " + FormatEntityContext(entity) + ".",
        CreatePath("DropTable", entity.TableName))];
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> AnalyzeRenameTable(
      DataVaultMigrationSchemaBaseline schema,
      RenameTableOperation operation) {
    if (!schema.TryGetEntity(operation.Name, out var entity)) {
      return [];
    }

    var newName = NormalizeName(operation.NewName) ?? "<unchanged>";
    return [CreateIssue(
        "DVM2005",
        "MI-5 explicit rename: migration renames " + FormatEntityContext(entity) +
        " away from the produced table name to '" + newName + "'.",
        CreatePath("RenameTable", entity.TableName, newName))];
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

  private static string FormatEntityContext(DataVaultMigrationEntityBaseline entity) {
    var parentContext = entity.ParentReference is null
        ? string.Empty
        : ", parent " + entity.ParentReference.Kind.ToString().ToLowerInvariant() +
            " '" + entity.ParentReference.Name + "'";

    return "Data Vault " + FormatStructureKind(entity.Kind) + " table '" + entity.TableName +
        "' (metadata name '" + entity.MetadataName + "', produced name '" + entity.ProducedName + "'" +
        parentContext + ")";
  }

  private static string FormatColumnContext(DataVaultMigrationColumnBaseline column) {
    return "column '" + column.Name + "' (metadata name '" + column.MetadataName +
        "', produced name '" + column.ProducedName + "', role " + FormatColumnRole(column) + ")";
  }

  private static string FormatPrimaryKeyContext(DataVaultMigrationKeyBaseline primaryKey) {
    return "primary-key constraint '" + primaryKey.Name + "' (produced name '" +
        primaryKey.ProducedName + "', columns [" + string.Join(", ", primaryKey.PropertyNames) + "])";
  }

  private static string FormatIndexContext(DataVaultMigrationIndexBaseline index) {
    return "secondary index '" + index.Name + "' (produced name '" + index.ProducedName +
        "', columns [" + string.Join(", ", index.PropertyNames) + "], unique " +
        index.IsUnique.ToString().ToLowerInvariant() + ")";
  }

  private static string FormatStructureKind(DataVaultTableKind kind) {
    return kind.ToString().ToLowerInvariant();
  }

  private static string FormatColumnRole(DataVaultMigrationColumnBaseline column) {
    if (column.Role == DataVaultPropertyRole.Technical && column.TechnicalRole is not null) {
      return "technical " + column.TechnicalRole.Value.ToString().ToLowerInvariant();
    }

    return column.Role.ToString().ToLowerInvariant();
  }

  private sealed record DataVaultMigrationOperationAnalysis(
      DataVaultDiagnosticsResult Diagnostics,
      IReadOnlyList<DataVaultMigrationGuardrailOperationSummary> OperationSummaries);

  private sealed record MigrationOperationDescriptor(
      string OperationName,
      string? TargetName,
      string? MemberName,
      string Path);

  private sealed record CreatedTableCandidate(
      CreateTableOperation Operation,
      DataVaultMigrationEntityBaseline Entity);

  private sealed class DataVaultMigrationOperationContext {
    private DataVaultMigrationOperationContext(
        IReadOnlyList<TableReplacementCandidate> tableReplacements,
        IReadOnlyList<ColumnReplacementCandidate> columnReplacements,
        IReadOnlyList<IndexReplacementCandidate> indexReplacements,
        IReadOnlyList<PrimaryKeyReplacementCandidate> primaryKeyReplacements) {
      TableReplacements = tableReplacements;
      ColumnReplacements = columnReplacements;
      IndexReplacements = indexReplacements;
      PrimaryKeyReplacements = primaryKeyReplacements;
    }

    private IReadOnlyList<TableReplacementCandidate> TableReplacements { get; }

    private IReadOnlyList<ColumnReplacementCandidate> ColumnReplacements { get; }

    private IReadOnlyList<IndexReplacementCandidate> IndexReplacements { get; }

    private IReadOnlyList<PrimaryKeyReplacementCandidate> PrimaryKeyReplacements { get; }

    public static DataVaultMigrationOperationContext Create(
        DataVaultMigrationSchemaBaseline schema,
        IReadOnlyList<MigrationOperation> operations) {
      var tableReplacements = CreateTableReplacementCandidates(schema, operations);
      var columnReplacements = CreateColumnReplacementCandidates(schema, operations);
      var indexReplacements = CreateIndexReplacementCandidates(schema, operations);
      var primaryKeyReplacements = CreatePrimaryKeyReplacementCandidates(schema, operations);

      return new DataVaultMigrationOperationContext(
          tableReplacements,
          columnReplacements,
          indexReplacements,
          primaryKeyReplacements);
    }

    public bool TryFindSuspiciousTableReplacement(
        DropTableOperation operation,
        out TableReplacementCandidate replacement) {
      replacement = TableReplacements.FirstOrDefault(candidate =>
          string.Equals(candidate.DroppedName, operation.Name, StringComparison.Ordinal))!;

      return replacement is not null;
    }

    public bool TryFindSuspiciousColumnReplacement(
        DropColumnOperation operation,
        out ColumnReplacementCandidate replacement) {
      replacement = ColumnReplacements.FirstOrDefault(candidate =>
          string.Equals(candidate.TableName, operation.Table, StringComparison.Ordinal) &&
          string.Equals(candidate.DroppedName, operation.Name, StringComparison.Ordinal))!;

      return replacement is not null;
    }

    public bool TryFindSuspiciousIndexReplacement(
        DropIndexOperation operation,
        out IndexReplacementCandidate replacement) {
      replacement = IndexReplacements.FirstOrDefault(candidate =>
          string.Equals(candidate.TableName, operation.Table, StringComparison.Ordinal) &&
          string.Equals(candidate.DroppedName, operation.Name, StringComparison.Ordinal))!;

      return replacement is not null;
    }

    public bool TryFindSuspiciousPrimaryKeyReplacement(
        DropPrimaryKeyOperation operation,
        out PrimaryKeyReplacementCandidate replacement) {
      replacement = PrimaryKeyReplacements.FirstOrDefault(candidate =>
          string.Equals(candidate.TableName, operation.Table, StringComparison.Ordinal) &&
          string.Equals(candidate.DroppedName, operation.Name, StringComparison.Ordinal))!;

      return replacement is not null;
    }

    private static IReadOnlyList<TableReplacementCandidate> CreateTableReplacementCandidates(
        DataVaultMigrationSchemaBaseline schema,
        IReadOnlyList<MigrationOperation> operations) {
      var createdTables = new List<CreatedTableCandidate>();
      foreach (var createTable in operations.OfType<CreateTableOperation>()) {
        if (!schema.TryGetEntity(createTable.Name, out var entity)) {
          continue;
        }

        createdTables.Add(new CreatedTableCandidate(createTable, entity));
      }

      if (createdTables.Count == 0) {
        return Array.Empty<TableReplacementCandidate>();
      }

      var candidates = new List<TableReplacementCandidate>();
      foreach (var dropTable in operations.OfType<DropTableOperation>()) {
        if (schema.TryGetEntity(dropTable.Name, out _)) {
          continue;
        }

        var replacement = createdTables
            .FirstOrDefault(candidate => IsSuspiciousTableReplacement(dropTable, candidate.Entity));
        if (replacement is not null) {
          candidates.Add(new TableReplacementCandidate(dropTable.Name, replacement.Entity));
        }
      }

      return candidates;
    }

    private static IReadOnlyList<ColumnReplacementCandidate> CreateColumnReplacementCandidates(
        DataVaultMigrationSchemaBaseline schema,
        IReadOnlyList<MigrationOperation> operations) {
      var addedColumns = new List<AddedColumnCandidate>();
      foreach (var addColumn in operations.OfType<AddColumnOperation>()) {
        if (!schema.TryGetEntity(addColumn.Table, out var entity) ||
            !entity.Columns.TryGetValue(addColumn.Name, out var column)) {
          continue;
        }

        addedColumns.Add(new AddedColumnCandidate(addColumn, column));
      }

      if (addedColumns.Count == 0) {
        return Array.Empty<ColumnReplacementCandidate>();
      }

      var candidates = new List<ColumnReplacementCandidate>();
      foreach (var dropColumn in operations.OfType<DropColumnOperation>()) {
        if (!schema.TryGetEntity(dropColumn.Table, out var entity) ||
            entity.Columns.ContainsKey(dropColumn.Name)) {
          continue;
        }

        var replacement = addedColumns.FirstOrDefault(candidate =>
            string.Equals(candidate.Operation.Table, dropColumn.Table, StringComparison.Ordinal) &&
            IsSuspiciousColumnReplacement(dropColumn, candidate.Column));
        if (replacement is not null) {
          candidates.Add(new ColumnReplacementCandidate(dropColumn.Table, dropColumn.Name, replacement.Column));
        }
      }

      return candidates;
    }

    private static IReadOnlyList<IndexReplacementCandidate> CreateIndexReplacementCandidates(
        DataVaultMigrationSchemaBaseline schema,
        IReadOnlyList<MigrationOperation> operations) {
      var createdIndexes = new List<CreatedIndexCandidate>();
      foreach (var createIndex in operations.OfType<CreateIndexOperation>()) {
        if (!schema.TryGetEntity(createIndex.Table, out var entity) ||
            !entity.Indexes.TryGetValue(createIndex.Name, out var index)) {
          continue;
        }

        createdIndexes.Add(new CreatedIndexCandidate(createIndex, index));
      }

      if (createdIndexes.Count == 0) {
        return Array.Empty<IndexReplacementCandidate>();
      }

      var candidates = new List<IndexReplacementCandidate>();
      foreach (var dropIndex in operations.OfType<DropIndexOperation>()) {
        if (string.IsNullOrWhiteSpace(dropIndex.Table) ||
            !schema.TryGetEntity(dropIndex.Table, out var entity) ||
            entity.Indexes.ContainsKey(dropIndex.Name)) {
          continue;
        }

        var replacement = createdIndexes.FirstOrDefault(candidate =>
            string.Equals(candidate.Operation.Table, dropIndex.Table, StringComparison.Ordinal) &&
            IsSuspiciousIndexReplacement(dropIndex, candidate.Index));
        if (replacement is not null) {
          candidates.Add(new IndexReplacementCandidate(dropIndex.Table, dropIndex.Name, replacement.Index));
        }
      }

      return candidates;
    }

    private static IReadOnlyList<PrimaryKeyReplacementCandidate> CreatePrimaryKeyReplacementCandidates(
        DataVaultMigrationSchemaBaseline schema,
        IReadOnlyList<MigrationOperation> operations) {
      var addedPrimaryKeys = new List<AddedPrimaryKeyCandidate>();
      foreach (var addPrimaryKey in operations.OfType<AddPrimaryKeyOperation>()) {
        if (!schema.TryGetEntity(addPrimaryKey.Table, out _)) {
          continue;
        }

        addedPrimaryKeys.Add(new AddedPrimaryKeyCandidate(addPrimaryKey));
      }

      if (addedPrimaryKeys.Count == 0) {
        return Array.Empty<PrimaryKeyReplacementCandidate>();
      }

      var candidates = new List<PrimaryKeyReplacementCandidate>();
      foreach (var dropPrimaryKey in operations.OfType<DropPrimaryKeyOperation>()) {
        if (!schema.TryGetEntity(dropPrimaryKey.Table, out var entity) ||
            string.Equals(dropPrimaryKey.Name, entity.PrimaryKey.Name, StringComparison.Ordinal)) {
          continue;
        }

        var replacement = addedPrimaryKeys.FirstOrDefault(candidate =>
            string.Equals(candidate.Operation.Table, dropPrimaryKey.Table, StringComparison.Ordinal) &&
            IsSuspiciousPrimaryKeyReplacement(dropPrimaryKey, entity.PrimaryKey, candidate.Operation));
        if (replacement is not null) {
          candidates.Add(new PrimaryKeyReplacementCandidate(dropPrimaryKey.Table, dropPrimaryKey.Name, entity.PrimaryKey));
        }
      }

      return candidates;
    }

    private static bool IsSuspiciousTableReplacement(
        DropTableOperation operation,
        DataVaultMigrationEntityBaseline entity) {
      return AnnotationMatchesEntity(operation, entity) ||
          IdentifierLooksRelated(operation.Name, entity.TableName) ||
          IdentifierLooksRelated(operation.Name, entity.MetadataName);
    }

    private static bool IsSuspiciousColumnReplacement(
        DropColumnOperation operation,
        DataVaultMigrationColumnBaseline column) {
      return AnnotationMatchesColumn(operation, column) ||
          IdentifierLooksRelated(operation.Name, column.Name) ||
          IdentifierLooksRelated(operation.Name, column.MetadataName) ||
          IdentifierCarriesRole(operation.Name, column);
    }

    private static bool IsSuspiciousIndexReplacement(
        DropIndexOperation operation,
        DataVaultMigrationIndexBaseline index) {
      return IdentifierLooksRelated(operation.Name, index.Name) ||
          index.PropertyNames.Any(propertyName => IdentifierLooksRelated(operation.Name, propertyName));
    }

    private static bool IsSuspiciousPrimaryKeyReplacement(
        DropPrimaryKeyOperation dropOperation,
        DataVaultMigrationKeyBaseline primaryKey,
        AddPrimaryKeyOperation addOperation) {
      var addedColumns = addOperation.Columns ?? Array.Empty<string>();
      return primaryKey.PropertyNames.SequenceEqual(addedColumns, StringComparer.Ordinal) &&
          (IdentifierLooksRelated(dropOperation.Name, primaryKey.Name) ||
              primaryKey.PropertyNames.Any(propertyName => IdentifierLooksRelated(dropOperation.Name, propertyName)));
    }

    private static bool AnnotationMatchesEntity(
        MigrationOperation operation,
        DataVaultMigrationEntityBaseline entity) {
      if (string.Equals(GetStringAnnotation(operation, DataVaultAnnotationNames.ProducedName), entity.TableName, StringComparison.Ordinal) ||
          string.Equals(GetStringAnnotation(operation, DataVaultAnnotationNames.MetadataName), entity.MetadataName, StringComparison.Ordinal)) {
        return true;
      }

      return GetAnnotationValue<DataVaultTableKind>(operation, DataVaultAnnotationNames.EntityKind) == entity.Kind &&
          (IdentifierLooksRelated(GetStringAnnotation(operation, DataVaultAnnotationNames.ProducedName), entity.TableName) ||
              IdentifierLooksRelated(GetStringAnnotation(operation, DataVaultAnnotationNames.MetadataName), entity.MetadataName));
    }

    private static bool AnnotationMatchesColumn(
        MigrationOperation operation,
        DataVaultMigrationColumnBaseline column) {
      if (string.Equals(GetStringAnnotation(operation, DataVaultAnnotationNames.ProducedName), column.Name, StringComparison.Ordinal) ||
          string.Equals(GetStringAnnotation(operation, DataVaultAnnotationNames.MetadataName), column.MetadataName, StringComparison.Ordinal)) {
        return true;
      }

      return GetAnnotationValue<DataVaultPropertyRole>(operation, DataVaultAnnotationNames.PropertyRole) == column.Role &&
          operation is DropColumnOperation dropColumn &&
          (IdentifierLooksRelated(dropColumn.Name, column.Name) ||
              IdentifierLooksRelated(dropColumn.Name, column.MetadataName) ||
              IdentifierCarriesRole(dropColumn.Name, column));
    }

    private static bool IdentifierCarriesRole(
        string identifier,
        DataVaultMigrationColumnBaseline column) {
      var normalizedIdentifier = NormalizeIdentifier(identifier);
      if (normalizedIdentifier.Length == 0) {
        return false;
      }

      if (column.TechnicalRole is not null &&
          normalizedIdentifier.Contains(NormalizeIdentifier(column.TechnicalRole.Value.ToString()), StringComparison.Ordinal)) {
        return true;
      }

      return column.Role switch {
        DataVaultPropertyRole.BusinessKey => normalizedIdentifier.Contains("businesskey", StringComparison.Ordinal),
        DataVaultPropertyRole.ParticipantReference => normalizedIdentifier.Contains("hashkey", StringComparison.Ordinal),
        DataVaultPropertyRole.DrivingKey => normalizedIdentifier.Contains("drivingkey", StringComparison.Ordinal),
        DataVaultPropertyRole.SnapshotReference => normalizedIdentifier.Contains("loadtimestamp", StringComparison.Ordinal),
        DataVaultPropertyRole.BridgeDepth => normalizedIdentifier.Contains("traversaldepth", StringComparison.Ordinal),
        _ => false,
      };
    }

    private static bool IdentifierLooksRelated(string? left, string? right) {
      var normalizedLeft = NormalizeIdentifier(left);
      var normalizedRight = NormalizeIdentifier(right);

      return normalizedLeft.Length > 0 &&
          normalizedRight.Length > 0 &&
          (normalizedLeft.Contains(normalizedRight, StringComparison.Ordinal) ||
              normalizedRight.Contains(normalizedLeft, StringComparison.Ordinal));
    }

    private static string NormalizeIdentifier(string? value) {
      if (string.IsNullOrWhiteSpace(value)) {
        return string.Empty;
      }

      return new string(value
          .Where(char.IsLetterOrDigit)
          .Select(char.ToLowerInvariant)
          .ToArray());
    }

    private static string? GetStringAnnotation(MigrationOperation operation, string annotationName) {
      return operation.FindAnnotation(annotationName)?.Value as string;
    }

    private static T? GetAnnotationValue<T>(MigrationOperation operation, string annotationName)
        where T : struct {
      return operation.FindAnnotation(annotationName)?.Value is T value
          ? value
          : null;
    }
  }

  private sealed record TableReplacementCandidate(
      string DroppedName,
      DataVaultMigrationEntityBaseline Entity);

  private sealed record AddedColumnCandidate(
      AddColumnOperation Operation,
      DataVaultMigrationColumnBaseline Column);

  private sealed record ColumnReplacementCandidate(
      string TableName,
      string DroppedName,
      DataVaultMigrationColumnBaseline Column);

  private sealed record CreatedIndexCandidate(
      CreateIndexOperation Operation,
      DataVaultMigrationIndexBaseline Index);

  private sealed record IndexReplacementCandidate(
      string TableName,
      string DroppedName,
      DataVaultMigrationIndexBaseline Index);

  private sealed record PrimaryKeyReplacementCandidate(
      string TableName,
      string DroppedName,
      DataVaultMigrationKeyBaseline PrimaryKey);

  private sealed record AddedPrimaryKeyCandidate(AddPrimaryKeyOperation Operation);

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
        string producedName,
        DataVaultTableKind kind,
        string metadataName,
        DataVaultParentReferenceExplain? parentReference,
        IReadOnlyList<DataVaultMigrationColumnBaseline> columnsInOrder,
        IReadOnlyDictionary<string, DataVaultMigrationColumnBaseline> columns,
        DataVaultMigrationKeyBaseline primaryKey,
        IReadOnlyDictionary<string, DataVaultMigrationIndexBaseline> indexes) {
      TableName = tableName;
      ProducedName = producedName;
      Kind = kind;
      MetadataName = metadataName;
      ParentReference = parentReference;
      ColumnsInOrder = columnsInOrder;
      Columns = columns;
      PrimaryKey = primaryKey;
      Indexes = indexes;
    }

    public string TableName { get; }

    public string ProducedName { get; }

    public DataVaultTableKind Kind { get; }

    public string MetadataName { get; }

    public DataVaultParentReferenceExplain? ParentReference { get; }

    public IReadOnlyList<DataVaultMigrationColumnBaseline> ColumnsInOrder { get; }

    public IReadOnlyDictionary<string, DataVaultMigrationColumnBaseline> Columns { get; }

    public DataVaultMigrationKeyBaseline PrimaryKey { get; }

    public IReadOnlyDictionary<string, DataVaultMigrationIndexBaseline> Indexes { get; }

    public static DataVaultMigrationEntityBaseline Create(DataVaultEntityExplain entity) {
      var columnsInOrder = entity.Properties
          .Select(DataVaultMigrationColumnBaseline.Create)
          .ToArray();
      var columns = columnsInOrder
          .ToDictionary(column => column.Name, StringComparer.Ordinal);
      var primaryKey = DataVaultMigrationKeyBaseline.Create(entity.PrimaryKey);
      var indexes = entity.Indexes
          .Select(DataVaultMigrationIndexBaseline.Create)
          .ToDictionary(index => index.Name, StringComparer.Ordinal);

      return new DataVaultMigrationEntityBaseline(
          entity.TableName,
          string.IsNullOrWhiteSpace(entity.ProducedName) ? entity.TableName : entity.ProducedName,
          entity.TableKind,
          entity.MetadataName,
          entity.ParentReference,
          columnsInOrder,
          columns,
          primaryKey,
          indexes);
    }
  }

  private sealed record DataVaultMigrationColumnBaseline(
      string Name,
      string ProducedName,
      DataVaultPropertyRole Role,
      TechnicalMetadataColumnRole? TechnicalRole,
      string MetadataName) {
    public static DataVaultMigrationColumnBaseline Create(DataVaultPropertyExplain property) {
      return new DataVaultMigrationColumnBaseline(
          property.Name,
          string.IsNullOrWhiteSpace(property.ProducedName) ? property.Name : property.ProducedName,
          property.Role,
          property.TechnicalRole,
          property.MetadataName);
    }
  }

  private sealed record DataVaultMigrationKeyBaseline(
      string Name,
      string ProducedName,
      IReadOnlyList<string> PropertyNames) {
    public static DataVaultMigrationKeyBaseline Create(DataVaultKeyExplain key) {
      return new DataVaultMigrationKeyBaseline(
          key.Name,
          string.IsNullOrWhiteSpace(key.ProducedName) ? key.Name : key.ProducedName,
          key.PropertyNames);
    }
  }

  private sealed record DataVaultMigrationIndexBaseline(
      string Name,
      string ProducedName,
      IReadOnlyList<string> PropertyNames,
      bool IsUnique) {
    public static DataVaultMigrationIndexBaseline Create(DataVaultIndexExplain index) {
      return new DataVaultMigrationIndexBaseline(
          index.Name,
          string.IsNullOrWhiteSpace(index.ProducedName) ? index.Name : index.ProducedName,
          index.PropertyNames,
          index.IsUnique);
    }
  }
}
