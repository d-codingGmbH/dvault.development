using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Compares expected Data Vault metadata with bounded live database schema snapshots.
/// </summary>
public static class DataVaultLiveSchemaDriftReporter {
  /// <summary>
  /// Compares expected Data Vault metadata with a live schema read result using the default SQLite capability profile.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="liveSchemaReadResult">The classified live schema read result to compare.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaReadResult liveSchemaReadResult) {
    return Compare(expectedMetadataModel, liveSchemaReadResult, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  /// <summary>
  /// Compares expected Data Vault metadata with a live schema read result using an explicit provider capability profile.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="liveSchemaReadResult">The classified live schema read result to compare.</param>
  /// <param name="providerCapabilities">The provider capability profile expected for generated storage metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaReadResult liveSchemaReadResult,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(liveSchemaReadResult);

    if (!liveSchemaReadResult.IsSucceeded) {
      return CreateLiveSchemaStatusReport(liveSchemaReadResult);
    }

    return Compare(expectedMetadataModel, liveSchemaReadResult.Snapshot!, providerCapabilities);
  }

  /// <summary>
  /// Compares expected Data Vault metadata with a successful live schema snapshot using the default SQLite capability profile.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="liveSchemaSnapshot">The successful live database schema snapshot to compare.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaSnapshot liveSchemaSnapshot) {
    return Compare(expectedMetadataModel, liveSchemaSnapshot, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  /// <summary>
  /// Compares expected Data Vault metadata with a successful live schema snapshot using an explicit provider capability profile.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="liveSchemaSnapshot">The successful live database schema snapshot to compare.</param>
  /// <param name="providerCapabilities">The provider capability profile expected for generated storage metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaSnapshot liveSchemaSnapshot,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(expectedMetadataModel);
    ArgumentNullException.ThrowIfNull(liveSchemaSnapshot);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var expectedModel = BuildExpectedModel(expectedMetadataModel, providerCapabilities);
    return CompareSnapshots(CreateExpectedSnapshot(expectedModel), liveSchemaSnapshot);
  }

  /// <summary>
  /// Compares a successful model-first import result with a live schema read result using an explicit provider capability profile.
  /// </summary>
  /// <param name="expectedImport">The expected successful dvault.model.v1 import result.</param>
  /// <param name="liveSchemaReadResult">The classified live schema read result to compare.</param>
  /// <param name="providerCapabilities">The provider capability profile expected for generated storage metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultModelImportResult expectedImport,
      DataVaultLiveSchemaReadResult liveSchemaReadResult,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(expectedImport);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    expectedImport.ThrowIfInvalid();
    if (expectedImport.MetadataModel is null) {
      throw new InvalidOperationException("The Data Vault model import result does not contain an expected metadata model.");
    }

    return Compare(
        expectedImport.MetadataModel,
        liveSchemaReadResult,
        providerCapabilities.WithLoadTimestampStorage(expectedImport.LoadTimestampStorage));
  }

  /// <summary>
  /// Reads and compares a live database schema for the supplied context using the built-in provider dispatch.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="currentContext">The current DbContext whose live database schema should be compared.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading live schema metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static async Task<DataVaultModelDriftReport> CompareAsync(
      DataVaultMetadataModel expectedMetadataModel,
      DbContext currentContext,
      CancellationToken cancellationToken = default) {
    return await CompareAsync(
        expectedMetadataModel,
        currentContext,
        new BuiltInDataVaultLiveSchemaReader(),
        cancellationToken).ConfigureAwait(false);
  }

  /// <summary>
  /// Reads and compares a live database schema for the supplied context using an explicit live schema reader.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="currentContext">The current DbContext whose live database schema should be compared.</param>
  /// <param name="liveSchemaReader">The live schema reader implementation to use.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading live schema metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static async Task<DataVaultModelDriftReport> CompareAsync(
      DataVaultMetadataModel expectedMetadataModel,
      DbContext currentContext,
      IDataVaultLiveSchemaReader liveSchemaReader,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(currentContext);
    ArgumentNullException.ThrowIfNull(liveSchemaReader);

    var liveSchemaReadResult = await liveSchemaReader.ReadAsync(currentContext, cancellationToken).ConfigureAwait(false);
    return Compare(
        expectedMetadataModel,
        liveSchemaReadResult,
        DataVaultProviderCapabilityProfileSelection.Select(TryGetProviderName(currentContext)));
  }

  /// <summary>
  /// Reads and compares a live database schema for the supplied context and model-first import result.
  /// </summary>
  /// <param name="expectedImport">The expected successful dvault.model.v1 import result.</param>
  /// <param name="currentContext">The current DbContext whose live database schema should be compared.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading live schema metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static async Task<DataVaultModelDriftReport> CompareAsync(
      DataVaultModelImportResult expectedImport,
      DbContext currentContext,
      CancellationToken cancellationToken = default) {
    return await CompareAsync(
        expectedImport,
        currentContext,
        new BuiltInDataVaultLiveSchemaReader(),
        cancellationToken).ConfigureAwait(false);
  }

  /// <summary>
  /// Reads and compares a live database schema for the supplied context and model-first import result.
  /// </summary>
  /// <param name="expectedImport">The expected successful dvault.model.v1 import result.</param>
  /// <param name="currentContext">The current DbContext whose live database schema should be compared.</param>
  /// <param name="liveSchemaReader">The live schema reader implementation to use.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading live schema metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static async Task<DataVaultModelDriftReport> CompareAsync(
      DataVaultModelImportResult expectedImport,
      DbContext currentContext,
      IDataVaultLiveSchemaReader liveSchemaReader,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(expectedImport);
    ArgumentNullException.ThrowIfNull(currentContext);
    ArgumentNullException.ThrowIfNull(liveSchemaReader);

    expectedImport.ThrowIfInvalid();
    if (expectedImport.MetadataModel is null) {
      throw new InvalidOperationException("The Data Vault model import result does not contain an expected metadata model.");
    }

    var liveSchemaReadResult = await liveSchemaReader.ReadAsync(currentContext, cancellationToken).ConfigureAwait(false);
    return Compare(
        expectedImport,
        liveSchemaReadResult,
        DataVaultProviderCapabilityProfileSelection.Select(TryGetProviderName(currentContext)));
  }

  private static DataVaultModelDriftReport CompareSnapshots(
      ExpectedLiveSchemaSnapshot expected,
      DataVaultLiveSchemaSnapshot actual) {
    var differences = new List<DataVaultModelDriftDifference>();
    var matchedActualTables = new bool[actual.Tables.Count];

    foreach (var expectedTable in expected.Tables) {
      var actualTableIndex = FindTable(expectedTable, actual.Tables, matchedActualTables);
      if (actualTableIndex is null) {
        AddDifference(
            differences,
            DataVaultModelDriftSeverity.Blocking,
            "missing-live-table",
            DataVaultModelDriftElementKind.Entity,
            expectedTable.LogicalName,
            expectedTable.TableName,
            "tables." + expectedTable.TableName,
            expectedTable.TableKind.ToString(),
            "<missing>",
            "The expected Data Vault table is missing from the live database schema.");
        continue;
      }

      matchedActualTables[actualTableIndex.Value] = true;
      CompareTable(differences, expectedTable, actual.Tables[actualTableIndex.Value]);
    }

    for (var index = 0; index < actual.Tables.Count; index++) {
      if (matchedActualTables[index]) {
        continue;
      }

      var actualTable = actual.Tables[index];
      AddDifference(
          differences,
          DataVaultModelDriftSeverity.Informational,
          "unexpected-live-table",
          DataVaultModelDriftElementKind.Entity,
          actualTable.TableName,
          actualTable.TableName,
          "tables." + actualTable.TableName,
          "<missing>",
          "live-table",
          "The live database schema contains an additional Data Vault-like table that is not in the expected model.");
    }

    return new DataVaultModelDriftReport(SortDifferences(differences));
  }

  private static void CompareTable(
      ICollection<DataVaultModelDriftDifference> differences,
      ExpectedLiveSchemaTable expected,
      DataVaultLiveSchemaTable actual) {
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-table-name-mismatch",
        DataVaultModelDriftElementKind.Entity,
        expected.LogicalName,
        expected.TableName,
        "tables." + expected.TableName + ".name",
        expected.TableName,
        actual.TableName,
        "The live database table has a different physical name.");

    CompareColumns(differences, expected, actual);
    ComparePrimaryKey(differences, expected, actual);
    CompareIndexes(differences, expected, actual);
  }

  private static void CompareColumns(
      ICollection<DataVaultModelDriftDifference> differences,
      ExpectedLiveSchemaTable expectedTable,
      DataVaultLiveSchemaTable actualTable) {
    var matchedActualColumns = new bool[actualTable.Columns.Count];
    foreach (var expectedColumn in expectedTable.Columns) {
      var actualColumnIndex = FindColumn(expectedColumn, actualTable.Columns, matchedActualColumns);
      if (actualColumnIndex is null) {
        AddDifference(
            differences,
            DataVaultModelDriftSeverity.Blocking,
            "missing-live-column",
            DataVaultModelDriftElementKind.Property,
            expectedTable.LogicalName + "." + expectedColumn.MetadataName,
            expectedColumn.ColumnName,
            "tables." + expectedTable.TableName + ".columns." + expectedColumn.ColumnName,
            expectedColumn.ProviderStorageType,
            "<missing>",
            "The expected Data Vault column is missing from the live database table.");
        continue;
      }

      matchedActualColumns[actualColumnIndex.Value] = true;
      CompareColumn(differences, expectedTable, expectedColumn, actualTable.Columns[actualColumnIndex.Value]);
    }

    for (var index = 0; index < actualTable.Columns.Count; index++) {
      if (matchedActualColumns[index]) {
        continue;
      }

      var actualColumn = actualTable.Columns[index];
      AddDifference(
          differences,
          DataVaultModelDriftSeverity.Informational,
          "unexpected-live-column",
          DataVaultModelDriftElementKind.Property,
          expectedTable.LogicalName + "." + actualColumn.ColumnName,
          actualColumn.ColumnName,
          "tables." + actualTable.TableName + ".columns." + actualColumn.ColumnName,
          "<missing>",
          actualColumn.ProviderStorageType,
          "The live database table contains an additional column that is not in the expected model.");
    }
  }

  private static void CompareColumn(
      ICollection<DataVaultModelDriftDifference> differences,
      ExpectedLiveSchemaTable expectedTable,
      ExpectedLiveSchemaColumn expected,
      DataVaultLiveSchemaColumn actual) {
    var logicalName = expectedTable.LogicalName + "." + expected.MetadataName;
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-column-name-mismatch",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ColumnName,
        "tables." + expectedTable.TableName + ".columns." + expected.ColumnName + ".name",
        expected.ColumnName,
        actual.ColumnName,
        "The live database column has a different physical name.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-column-ordinal-mismatch",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ColumnName,
        "tables." + expectedTable.TableName + ".columns." + expected.ColumnName + ".ordinal",
        expected.Ordinal.ToString(CultureInfo.InvariantCulture),
        actual.Ordinal.ToString(CultureInfo.InvariantCulture),
        "The live database column has a different physical ordinal.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-column-storage-type-mismatch",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ColumnName,
        "tables." + expectedTable.TableName + ".columns." + expected.ColumnName + ".providerStorageType",
        expected.ProviderStorageType,
        actual.ProviderStorageType,
        "The live database column has an incompatible provider storage type.");
  }

  private static void ComparePrimaryKey(
      ICollection<DataVaultModelDriftDifference> differences,
      ExpectedLiveSchemaTable expectedTable,
      DataVaultLiveSchemaTable actualTable) {
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-primary-key-name-mismatch",
        DataVaultModelDriftElementKind.Key,
        expectedTable.LogicalName,
        expectedTable.PrimaryKey.ConstraintName,
        "tables." + expectedTable.TableName + ".primaryKey.name",
        expectedTable.PrimaryKey.ConstraintName,
        actualTable.PrimaryKey.ConstraintName,
        "The live database primary-key constraint has a different physical name.");
    AddListDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-primary-key-column-mismatch",
        DataVaultModelDriftElementKind.Key,
        expectedTable.LogicalName,
        expectedTable.PrimaryKey.ConstraintName,
        "tables." + expectedTable.TableName + ".primaryKey.columns",
        expectedTable.PrimaryKey.ColumnNames,
        actualTable.PrimaryKey.ColumnNames,
        "The live database primary key has an incompatible column set or order.");
  }

  private static void CompareIndexes(
      ICollection<DataVaultModelDriftDifference> differences,
      ExpectedLiveSchemaTable expectedTable,
      DataVaultLiveSchemaTable actualTable) {
    var matchedActualIndexes = new bool[actualTable.Indexes.Count];
    foreach (var expectedIndex in expectedTable.Indexes) {
      var actualIndexIndex = FindIndex(expectedIndex, actualTable.Indexes, matchedActualIndexes);
      if (actualIndexIndex is null) {
        AddDifference(
            differences,
            DataVaultModelDriftSeverity.Blocking,
            "missing-live-index",
            DataVaultModelDriftElementKind.Index,
            expectedTable.LogicalName,
            expectedIndex.IndexName,
            "tables." + expectedTable.TableName + ".indexes." + expectedIndex.IndexName,
            FormatIndexSignature(expectedIndex),
            "<missing>",
            "The expected Data Vault secondary index is missing from the live database table.");
        continue;
      }

      matchedActualIndexes[actualIndexIndex.Value] = true;
      CompareIndex(differences, expectedTable, expectedIndex, actualTable.Indexes[actualIndexIndex.Value]);
    }

    for (var index = 0; index < actualTable.Indexes.Count; index++) {
      if (matchedActualIndexes[index]) {
        continue;
      }

      var actualIndex = actualTable.Indexes[index];
      AddDifference(
          differences,
          DataVaultModelDriftSeverity.Informational,
          "unexpected-live-index",
          DataVaultModelDriftElementKind.Index,
          expectedTable.LogicalName,
          actualIndex.IndexName,
          "tables." + actualTable.TableName + ".indexes." + actualIndex.IndexName,
          "<missing>",
          FormatIndexSignature(actualIndex),
          "The live database table contains an additional secondary index that is not in the expected model.");
    }
  }

  private static void CompareIndex(
      ICollection<DataVaultModelDriftDifference> differences,
      ExpectedLiveSchemaTable expectedTable,
      ExpectedLiveSchemaIndex expected,
      DataVaultLiveSchemaIndex actual) {
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-index-name-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedTable.LogicalName,
        expected.IndexName,
        "tables." + expectedTable.TableName + ".indexes." + expected.IndexName + ".name",
        expected.IndexName,
        actual.IndexName,
        "The live database index has a different physical name.");
    AddListDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-index-column-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedTable.LogicalName,
        expected.IndexName,
        "tables." + expectedTable.TableName + ".indexes." + expected.IndexName + ".columns",
        expected.ColumnNames,
        actual.ColumnNames,
        "The live database index has an incompatible column set or order.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "live-index-uniqueness-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedTable.LogicalName,
        expected.IndexName,
        "tables." + expectedTable.TableName + ".indexes." + expected.IndexName + ".isUnique",
        expected.IsUnique.ToString(),
        actual.IsUnique.ToString(),
        "The live database index has an incompatible uniqueness flag.");
  }

  private static int? FindTable(
      ExpectedLiveSchemaTable expected,
      IReadOnlyList<DataVaultLiveSchemaTable> actualTables,
      IReadOnlyList<bool> matched) {
    var exact = FindSingleIndex(
        actualTables,
        matched,
        table => string.Equals(table.TableName, expected.TableName, StringComparison.Ordinal));
    if (exact is not null) {
      return exact;
    }

    return FindSingleIndex(actualTables, matched, table => HasSameTableShape(expected, table));
  }

  private static int? FindColumn(
      ExpectedLiveSchemaColumn expected,
      IReadOnlyList<DataVaultLiveSchemaColumn> actualColumns,
      IReadOnlyList<bool> matched) {
    var exact = FindSingleIndex(
        actualColumns,
        matched,
        column => string.Equals(column.ColumnName, expected.ColumnName, StringComparison.Ordinal));
    if (exact is not null) {
      return exact;
    }

    return FindSingleIndex(actualColumns, matched, column => column.Ordinal == expected.Ordinal);
  }

  private static int? FindIndex(
      ExpectedLiveSchemaIndex expected,
      IReadOnlyList<DataVaultLiveSchemaIndex> actualIndexes,
      IReadOnlyList<bool> matched) {
    var exact = FindSingleIndex(
        actualIndexes,
        matched,
        index => string.Equals(index.IndexName, expected.IndexName, StringComparison.Ordinal));
    if (exact is not null) {
      return exact;
    }

    return FindSingleIndex(actualIndexes, matched, index => HasSameIndexSignature(expected, index));
  }

  private static int? FindSingleIndex<T>(
      IReadOnlyList<T> values,
      IReadOnlyList<bool> matched,
      Func<T, bool> predicate) {
    int? foundIndex = null;
    for (var index = 0; index < values.Count; index++) {
      if (matched[index] || !predicate(values[index])) {
        continue;
      }

      if (foundIndex is not null) {
        return null;
      }

      foundIndex = index;
    }

    return foundIndex;
  }

  private static IReadOnlyModel BuildExpectedModel(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(metadataModel, providerCapabilities);

    return modelBuilder.Model;
  }

  private static ExpectedLiveSchemaSnapshot CreateExpectedSnapshot(IReadOnlyModel model) {
    var tables = model
        .GetEntityTypes()
        .Where(IsDataVaultEntity)
        .Select(CreateExpectedTableSnapshot)
        .OrderBy(table => GetTableKindSortKey(table.TableKind))
        .ThenBy(table => table.LogicalName, StringComparer.Ordinal)
        .ThenBy(table => table.TableName, StringComparer.Ordinal)
        .ToArray();

    return new ExpectedLiveSchemaSnapshot(tables);
  }

  private static ExpectedLiveSchemaTable CreateExpectedTableSnapshot(IReadOnlyEntityType entityType) {
    var producedTableName = GetStringAnnotation(entityType, DataVaultAnnotationNames.ProducedName) ??
        entityType.GetTableName() ??
        entityType.Name;
    var tableName = entityType.GetTableName() ?? producedTableName;
    var tableIdentifier = StoreObjectIdentifier.Table(tableName, entityType.GetSchema());
    var tableKind = GetAnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind);
    var metadataName = GetStringAnnotation(entityType, DataVaultAnnotationNames.MetadataName) ?? producedTableName;
    var columns = entityType
        .GetProperties()
        .Select(property => CreateExpectedColumnSnapshot(property, tableIdentifier))
        .OrderBy(column => column.Ordinal)
        .ThenBy(column => column.ColumnName, StringComparer.Ordinal)
        .ToArray();
    var primaryKey = entityType.FindPrimaryKey();
    var primaryKeySnapshot = primaryKey is null
        ? new ExpectedLiveSchemaPrimaryKey("<none>", Array.Empty<string>())
        : new ExpectedLiveSchemaPrimaryKey(
            primaryKey.GetName() ??
                GetStringAnnotation(primaryKey, DataVaultAnnotationNames.ProducedName) ??
                "Pk" + producedTableName,
            primaryKey.Properties.Select(property => CreateColumnReference(property, tableIdentifier)).ToArray());
    var indexes = entityType
        .GetIndexes()
        .Select(index => CreateExpectedIndexSnapshot(index, tableIdentifier))
        .OrderBy(index => index.IndexName, StringComparer.Ordinal)
        .ToArray();

    return new ExpectedLiveSchemaTable(
        tableKind,
        metadataName,
        tableName,
        columns,
        primaryKeySnapshot,
        indexes);
  }

  private static ExpectedLiveSchemaColumn CreateExpectedColumnSnapshot(
      IReadOnlyProperty property,
      StoreObjectIdentifier tableIdentifier) {
    return new ExpectedLiveSchemaColumn(
        GetStringAnnotation(property, DataVaultAnnotationNames.MetadataName) ?? property.Name,
        CreateColumnReference(property, tableIdentifier),
        GetNullableAnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal) ?? property.GetColumnOrder() ?? 0,
        GetStringAnnotation(property, DataVaultAnnotationNames.ProviderStorageType) ?? property.GetColumnType() ?? string.Empty);
  }

  private static ExpectedLiveSchemaIndex CreateExpectedIndexSnapshot(
      IReadOnlyIndex index,
      StoreObjectIdentifier tableIdentifier) {
    var columnNames = index.Properties.Select(property => CreateColumnReference(property, tableIdentifier)).ToArray();
    return new ExpectedLiveSchemaIndex(
        index.GetDatabaseName() ??
            GetStringAnnotation(index, DataVaultAnnotationNames.ProducedName) ??
            string.Join("_", columnNames),
        columnNames,
        index.IsUnique);
  }

  private static string CreateColumnReference(
      IReadOnlyProperty property,
      StoreObjectIdentifier tableIdentifier) {
    return property.GetColumnName(tableIdentifier) ??
        GetStringAnnotation(property, DataVaultAnnotationNames.ProducedName) ??
        property.Name;
  }

  private static DataVaultModelDriftReport CreateLiveSchemaStatusReport(
      DataVaultLiveSchemaReadResult liveSchemaReadResult) {
    var code = liveSchemaReadResult.Status switch {
      DataVaultLiveSchemaReadStatus.UnsupportedProvider => "live-schema-provider-unsupported",
      DataVaultLiveSchemaReadStatus.Unavailable => "live-schema-unavailable",
      _ => "live-schema-read-unsuccessful",
    };

    return new DataVaultModelDriftReport(
        [
            new DataVaultModelDriftDifference(
                DataVaultModelDriftSeverity.Blocking,
                code,
                DataVaultModelDriftElementKind.Model,
                "<live-schema>",
                liveSchemaReadResult.ProviderName,
                "liveSchema.status",
                DataVaultLiveSchemaReadStatus.Succeeded.ToString(),
                liveSchemaReadResult.Status.ToString(),
                liveSchemaReadResult.Message),
        ]);
  }

  private static string? TryGetProviderName(DbContext dbContext) {
    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
  }

  private static IReadOnlyList<DataVaultModelDriftDifference> SortDifferences(
      IEnumerable<DataVaultModelDriftDifference> differences) {
    return differences
        .OrderBy(difference => difference.ElementKind)
        .ThenBy(difference => difference.LogicalName, StringComparer.Ordinal)
        .ThenBy(difference => difference.ProducedName ?? string.Empty, StringComparer.Ordinal)
        .ThenBy(difference => difference.Code, StringComparer.Ordinal)
        .ThenBy(difference => difference.PropertyPath, StringComparer.Ordinal)
        .ToArray();
  }

  private static void AddScalarDifference(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string code,
      DataVaultModelDriftElementKind elementKind,
      string logicalName,
      string? producedName,
      string propertyPath,
      string? expectedValue,
      string? actualValue,
      string message) {
    if (string.Equals(expectedValue, actualValue, StringComparison.Ordinal)) {
      return;
    }

    AddDifference(
        differences,
        severity,
        code,
        elementKind,
        logicalName,
        producedName,
        propertyPath,
        expectedValue,
        actualValue,
        message);
  }

  private static void AddListDifference(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string code,
      DataVaultModelDriftElementKind elementKind,
      string logicalName,
      string? producedName,
      string propertyPath,
      IReadOnlyList<string> expectedValue,
      IReadOnlyList<string> actualValue,
      string message) {
    if (HasSameValues(expectedValue, actualValue)) {
      return;
    }

    AddDifference(
        differences,
        severity,
        code,
        elementKind,
        logicalName,
        producedName,
        propertyPath,
        FormatValues(expectedValue),
        FormatValues(actualValue),
        message);
  }

  private static void AddDifference(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string code,
      DataVaultModelDriftElementKind elementKind,
      string logicalName,
      string? producedName,
      string propertyPath,
      string? expectedValue,
      string? actualValue,
      string message) {
    differences.Add(new DataVaultModelDriftDifference(
        severity,
        code,
        elementKind,
        logicalName,
        producedName,
        propertyPath,
        expectedValue,
        actualValue,
        message));
  }

  private static bool IsDataVaultEntity(IReadOnlyEntityType entityType) {
    return entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value is DataVaultTableKind;
  }

  private static bool HasSameTableShape(ExpectedLiveSchemaTable expected, DataVaultLiveSchemaTable actual) {
    return HasSameColumnShape(expected.Columns, actual.Columns) &&
        HasSameValues(expected.PrimaryKey.ColumnNames, actual.PrimaryKey.ColumnNames) &&
        HasSameIndexShape(expected.Indexes, actual.Indexes);
  }

  private static bool HasSameColumnShape(
      IReadOnlyList<ExpectedLiveSchemaColumn> expected,
      IReadOnlyList<DataVaultLiveSchemaColumn> actual) {
    if (expected.Count != actual.Count) {
      return false;
    }

    for (var index = 0; index < expected.Count; index++) {
      if (!string.Equals(expected[index].ColumnName, actual[index].ColumnName, StringComparison.Ordinal) ||
          !string.Equals(expected[index].ProviderStorageType, actual[index].ProviderStorageType, StringComparison.Ordinal)) {
        return false;
      }
    }

    return true;
  }

  private static bool HasSameIndexShape(
      IReadOnlyList<ExpectedLiveSchemaIndex> expected,
      IReadOnlyList<DataVaultLiveSchemaIndex> actual) {
    if (expected.Count != actual.Count) {
      return false;
    }

    var matchedActualIndexes = new bool[actual.Count];
    foreach (var expectedIndex in expected) {
      var actualIndex = FindSingleIndex(actual, matchedActualIndexes, index => HasSameIndexSignature(expectedIndex, index));
      if (actualIndex is null) {
        return false;
      }

      matchedActualIndexes[actualIndex.Value] = true;
    }

    return true;
  }

  private static bool HasSameIndexSignature(ExpectedLiveSchemaIndex expected, DataVaultLiveSchemaIndex actual) {
    return expected.IsUnique == actual.IsUnique && HasSameValues(expected.ColumnNames, actual.ColumnNames);
  }

  private static bool HasSameValues(IReadOnlyList<string> expected, IReadOnlyList<string> actual) {
    if (expected.Count != actual.Count) {
      return false;
    }

    for (var index = 0; index < expected.Count; index++) {
      if (!string.Equals(expected[index], actual[index], StringComparison.Ordinal)) {
        return false;
      }
    }

    return true;
  }

  private static string FormatIndexSignature(ExpectedLiveSchemaIndex index) {
    return "columns=" + FormatValues(index.ColumnNames) + "; unique=" + index.IsUnique.ToString();
  }

  private static string FormatIndexSignature(DataVaultLiveSchemaIndex index) {
    return "columns=" + FormatValues(index.ColumnNames) + "; unique=" + index.IsUnique.ToString();
  }

  private static string FormatValues(IReadOnlyList<string> values) {
    return values.Count == 0 ? "<none>" : string.Join("|", values);
  }

  private static int GetTableKindSortKey(DataVaultTableKind tableKind) {
    return tableKind switch {
      DataVaultTableKind.Hub => 0,
      DataVaultTableKind.Link => 1,
      DataVaultTableKind.Satellite => 2,
      DataVaultTableKind.Bridge => 3,
      DataVaultTableKind.Pit => 4,
      DataVaultTableKind.PointInTime => 5,
      _ => 99,
    };
  }

  private static string? GetStringAnnotation(IReadOnlyAnnotatable annotatable, string annotationName) {
    return annotatable.FindAnnotation(annotationName)?.Value as string;
  }

  private static T GetAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : default;
  }

  private static T? GetNullableAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : null;
  }

  private sealed class BuiltInDataVaultLiveSchemaReader : IDataVaultLiveSchemaReader {
    public Task<DataVaultLiveSchemaReadResult> ReadAsync(
        DbContext dbContext,
        CancellationToken cancellationToken = default) {
      return DataVaultLiveSchemaReader.ReadAsync(dbContext, cancellationToken);
    }
  }

  private sealed record ExpectedLiveSchemaSnapshot(IReadOnlyList<ExpectedLiveSchemaTable> Tables);

  private sealed record ExpectedLiveSchemaTable(
      DataVaultTableKind TableKind,
      string MetadataName,
      string TableName,
      IReadOnlyList<ExpectedLiveSchemaColumn> Columns,
      ExpectedLiveSchemaPrimaryKey PrimaryKey,
      IReadOnlyList<ExpectedLiveSchemaIndex> Indexes) {
    public string LogicalName => TableKind + ":" + MetadataName;
  }

  private sealed record ExpectedLiveSchemaColumn(
      string MetadataName,
      string ColumnName,
      int Ordinal,
      string ProviderStorageType);

  private sealed record ExpectedLiveSchemaPrimaryKey(string ConstraintName, IReadOnlyList<string> ColumnNames);

  private sealed record ExpectedLiveSchemaIndex(string IndexName, IReadOnlyList<string> ColumnNames, bool IsUnique);
}
