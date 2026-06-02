using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Validates provider-shaped Data Vault primary keys and idempotency/read-path indexes against explicit live-schema input.
/// </summary>
public static class DataVaultIdempotencyPreflight {
  private const string BridgeOperationFamily = "bridge-traversal-maintenance";
  private const string HubOperationFamily = "hub-save-idempotency";
  private const string LinkOperationFamily = "link-save-idempotency";
  private const string PitOperationFamily = "pit-as-of-read";
  private const string PrimaryKeyKind = "primary-key";
  private const string SatelliteOperationFamily = "satellite-latest-state";
  private const string SecondaryIndexKind = "secondary-index";

  /// <summary>
  /// Compares provider-neutral expected metadata with a classified live-schema read result using the default SQLite capability profile.
  /// </summary>
  public static DataVaultIdempotencyPreflightReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaReadResult liveSchemaReadResult) {
    return Compare(expectedMetadataModel, liveSchemaReadResult, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  /// <summary>
  /// Compares provider-neutral expected metadata with a classified live-schema read result using an explicit provider capability profile.
  /// </summary>
  public static DataVaultIdempotencyPreflightReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaReadResult liveSchemaReadResult,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(expectedMetadataModel);
    ArgumentNullException.ThrowIfNull(liveSchemaReadResult);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var expectedStructures = CreateExpectedStructures(expectedMetadataModel, providerCapabilities);
    if (!liveSchemaReadResult.IsSucceeded) {
      return CreateLiveSchemaStatusReport(liveSchemaReadResult, providerCapabilities, expectedStructures);
    }

    return CompareStructures(
        expectedStructures,
        liveSchemaReadResult.Snapshot!,
        liveSchemaReadResult.ProviderName,
        providerCapabilities);
  }

  /// <summary>
  /// Compares provider-neutral expected metadata with a successful live-schema snapshot using the default SQLite capability profile.
  /// </summary>
  public static DataVaultIdempotencyPreflightReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaSnapshot liveSchemaSnapshot) {
    return Compare(expectedMetadataModel, liveSchemaSnapshot, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  /// <summary>
  /// Compares provider-neutral expected metadata with a successful live-schema snapshot using an explicit provider capability profile.
  /// </summary>
  public static DataVaultIdempotencyPreflightReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultLiveSchemaSnapshot liveSchemaSnapshot,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(expectedMetadataModel);
    ArgumentNullException.ThrowIfNull(liveSchemaSnapshot);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return CompareStructures(
        CreateExpectedStructures(expectedMetadataModel, providerCapabilities),
        liveSchemaSnapshot,
        providerName: null,
        providerCapabilities);
  }

  /// <summary>
  /// Compares a successful model-first import result with a classified live-schema read result using an explicit provider capability profile.
  /// </summary>
  public static DataVaultIdempotencyPreflightReport Compare(
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
  /// Reads and compares live idempotency schema structures for the supplied context using built-in live-schema dispatch.
  /// </summary>
  public static async Task<DataVaultIdempotencyPreflightReport> CompareAsync(
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
  /// Reads and compares live idempotency schema structures for the supplied context using an explicit live-schema reader.
  /// </summary>
  public static async Task<DataVaultIdempotencyPreflightReport> CompareAsync(
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
  /// Reads and compares live idempotency schema structures for a model-first import result using built-in live-schema dispatch.
  /// </summary>
  public static async Task<DataVaultIdempotencyPreflightReport> CompareAsync(
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
  /// Reads and compares live idempotency schema structures for a model-first import result using an explicit live-schema reader.
  /// </summary>
  public static async Task<DataVaultIdempotencyPreflightReport> CompareAsync(
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

    var providerCapabilities = DataVaultProviderCapabilityProfileSelection
        .Select(TryGetProviderName(currentContext))
        .WithLoadTimestampStorage(expectedImport.LoadTimestampStorage);
    var liveSchemaReadResult = await liveSchemaReader.ReadAsync(currentContext, cancellationToken).ConfigureAwait(false);
    return Compare(expectedImport.MetadataModel, liveSchemaReadResult, providerCapabilities);
  }

  internal static IReadOnlyList<DataVaultIdempotencyPreflightStructure> CreateExpectedStructures(
      DataVaultMetadataModel expectedMetadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(expectedMetadataModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(expectedMetadataModel, providerCapabilities);

    return modelBuilder.Model
        .GetEntityTypes()
        .Where(IsIdempotencyTable)
        .SelectMany(CreateExpectedStructures)
        .OrderBy(structure => GetTableKindSortKey(GetTableKind(structure.OperationFamily)))
        .ThenBy(structure => structure.TableName, StringComparer.Ordinal)
        .ThenBy(structure => structure.OperationFamily, StringComparer.Ordinal)
        .ThenBy(structure => structure.Kind, StringComparer.Ordinal)
        .ThenBy(structure => structure.Name, StringComparer.Ordinal)
        .ToArray();
  }

  private static DataVaultIdempotencyPreflightReport CompareStructures(
      IReadOnlyList<DataVaultIdempotencyPreflightStructure> expectedStructures,
      DataVaultLiveSchemaSnapshot liveSchemaSnapshot,
      string? providerName,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var findings = new List<DataVaultIdempotencyPreflightFinding>();
    var expectedByTable = expectedStructures
        .GroupBy(structure => structure.TableName, StringComparer.Ordinal)
        .OrderBy(group => group.Key, StringComparer.Ordinal);

    foreach (var expectedTable in expectedByTable) {
      var actualTable = liveSchemaSnapshot.Tables.SingleOrDefault(
          table => string.Equals(table.TableName, expectedTable.Key, StringComparison.Ordinal));
      if (actualTable is null) {
        AddMissingTableFinding(findings, expectedTable.Key, expectedTable);
        continue;
      }

      CompareTableStructures(findings, expectedTable, actualTable);
    }

    var orderedFindings = SortFindings(findings);
    return new DataVaultIdempotencyPreflightReport(
        orderedFindings.Count == 0
            ? DataVaultIdempotencyPreflightStatus.Passed
            : DataVaultIdempotencyPreflightStatus.Blocked,
        providerName,
        providerCapabilities.ProfileName,
        expectedStructures,
        orderedFindings);
  }

  private static void CompareTableStructures(
      ICollection<DataVaultIdempotencyPreflightFinding> findings,
      IGrouping<string, DataVaultIdempotencyPreflightStructure> expectedTable,
      DataVaultLiveSchemaTable actualTable) {
    var expectedPrimaryKey = expectedTable.SingleOrDefault(structure => structure.Kind == PrimaryKeyKind);
    if (expectedPrimaryKey is not null) {
      ComparePrimaryKey(findings, expectedPrimaryKey, actualTable.PrimaryKey);
    }

    var expectedIndexes = expectedTable
        .Where(structure => structure.Kind == SecondaryIndexKind)
        .ToArray();
    var matchedActualIndexes = new bool[actualTable.Indexes.Count];
    foreach (var expectedIndex in expectedIndexes) {
      var actualIndexIndex = FindIndex(expectedIndex, actualTable.Indexes, matchedActualIndexes);
      if (actualIndexIndex is null) {
        AddFinding(
            findings,
            "missing-idempotency-index",
            expectedIndex,
            "tables." + expectedIndex.TableName + ".indexes." + expectedIndex.Name,
            FormatIndexSignature(expectedIndex),
            "<missing>",
            "The expected Data Vault idempotency/access-path index is missing from the live schema.");
        continue;
      }

      matchedActualIndexes[actualIndexIndex.Value] = true;
      CompareIndex(findings, expectedIndex, actualTable.Indexes[actualIndexIndex.Value]);
    }
  }

  private static void ComparePrimaryKey(
      ICollection<DataVaultIdempotencyPreflightFinding> findings,
      DataVaultIdempotencyPreflightStructure expected,
      DataVaultLiveSchemaPrimaryKey actual) {
    AddScalarFinding(
        findings,
        "idempotency-primary-key-name-mismatch",
        expected,
        "tables." + expected.TableName + ".primaryKey.name",
        expected.Name,
        actual.ConstraintName,
        "The live primary-key constraint name differs from the translated Data Vault baseline.");
    AddListFinding(
        findings,
        "idempotency-primary-key-column-mismatch",
        expected,
        "tables." + expected.TableName + ".primaryKey.columns",
        expected.ColumnNames,
        actual.ColumnNames,
        "The live primary-key column set or order differs from the translated Data Vault baseline.");
  }

  private static void CompareIndex(
      ICollection<DataVaultIdempotencyPreflightFinding> findings,
      DataVaultIdempotencyPreflightStructure expected,
      DataVaultLiveSchemaIndex actual) {
    AddScalarFinding(
        findings,
        "idempotency-index-name-mismatch",
        expected,
        "tables." + expected.TableName + ".indexes." + expected.Name + ".name",
        expected.Name,
        actual.IndexName,
        "The live idempotency/access-path index name differs from the translated Data Vault baseline.");
    AddListFinding(
        findings,
        "idempotency-index-column-mismatch",
        expected,
        "tables." + expected.TableName + ".indexes." + expected.Name + ".columns",
        expected.ColumnNames,
        actual.ColumnNames,
        "The live idempotency/access-path index column set or order differs from the translated Data Vault baseline.");
    AddScalarFinding(
        findings,
        "idempotency-index-uniqueness-mismatch",
        expected,
        "tables." + expected.TableName + ".indexes." + expected.Name + ".isUnique",
        expected.IsUnique.ToString(),
        actual.IsUnique.ToString(),
        "The live idempotency/access-path index uniqueness flag differs from the translated Data Vault baseline.");
    AddListFinding(
        findings,
        "idempotency-index-descending-column-mismatch",
        expected,
        "tables." + expected.TableName + ".indexes." + expected.Name + ".descendingColumns",
        expected.DescendingColumnNames,
        actual.DescendingColumnNames,
        "The live idempotency/access-path index descending column set differs from the translated Data Vault baseline.");
    AddListFinding(
        findings,
        "idempotency-index-included-column-mismatch",
        expected,
        "tables." + expected.TableName + ".indexes." + expected.Name + ".includedColumns",
        expected.IncludedColumnNames,
        actual.IncludedColumnNames,
        "The live idempotency/access-path index included column set differs from the translated Data Vault baseline.");
  }

  private static int? FindIndex(
      DataVaultIdempotencyPreflightStructure expected,
      IReadOnlyList<DataVaultLiveSchemaIndex> actualIndexes,
      IReadOnlyList<bool> matched) {
    var exact = FindSingleIndex(
        actualIndexes,
        matched,
        index => string.Equals(index.IndexName, expected.Name, StringComparison.Ordinal));
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

  private static DataVaultIdempotencyPreflightReport CreateLiveSchemaStatusReport(
      DataVaultLiveSchemaReadResult liveSchemaReadResult,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IReadOnlyList<DataVaultIdempotencyPreflightStructure> expectedStructures) {
    var (status, code, message) = liveSchemaReadResult.Status switch {
      DataVaultLiveSchemaReadStatus.UnsupportedProvider => (
          DataVaultIdempotencyPreflightStatus.UnsupportedProvider,
          "idempotency-live-schema-provider-unsupported",
          "Live schema reading is not supported for provider '" + FormatProviderName(liveSchemaReadResult.ProviderName) + "'."),
      DataVaultLiveSchemaReadStatus.Unavailable => (
          DataVaultIdempotencyPreflightStatus.UnavailableLiveSchema,
          "idempotency-live-schema-unavailable",
          "Live schema was unavailable for provider '" +
          FormatProviderName(liveSchemaReadResult.ProviderName) +
          "'; verify the caller-owned database connection and schema access."),
      _ => (
          DataVaultIdempotencyPreflightStatus.Blocked,
          "idempotency-live-schema-read-unsuccessful",
          "Live schema reading did not return a successful snapshot for provider '" +
          FormatProviderName(liveSchemaReadResult.ProviderName) +
          "'."),
    };

    var finding = new DataVaultIdempotencyPreflightFinding(
        DataVaultModelDriftSeverity.Blocking,
        code,
        "<live-schema>",
        "live-schema-read",
        "live-schema",
        "<live-schema>",
        "liveSchema.status",
        DataVaultLiveSchemaReadStatus.Succeeded.ToString(),
        liveSchemaReadResult.Status.ToString(),
        message);

    return new DataVaultIdempotencyPreflightReport(
        status,
        liveSchemaReadResult.ProviderName,
        providerCapabilities.ProfileName,
        expectedStructures,
        [finding],
        message);
  }

  private static IEnumerable<DataVaultIdempotencyPreflightStructure> CreateExpectedStructures(
      IReadOnlyEntityType entityType) {
    var tableName = entityType.GetTableName() ?? entityType.Name;
    var tableKind = GetAnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind);
    var operationFamily = GetOperationFamily(tableKind);
    var primaryKey = entityType.FindPrimaryKey();
    if (primaryKey is not null) {
      yield return new DataVaultIdempotencyPreflightStructure(
          tableName,
          operationFamily,
          PrimaryKeyKind,
          primaryKey.GetName() ??
              GetStringAnnotation(primaryKey, DataVaultAnnotationNames.ProducedName) ??
              "Pk" + tableName,
          primaryKey.Properties.Select(CreateColumnReference).ToArray(),
          IsUnique: true,
          DescendingColumnNames: Array.Empty<string>(),
          IncludedColumnNames: Array.Empty<string>());
    }

    foreach (var index in entityType.GetIndexes().OrderBy(index => index.GetDatabaseName(), StringComparer.Ordinal)) {
      yield return new DataVaultIdempotencyPreflightStructure(
          tableName,
          operationFamily,
          SecondaryIndexKind,
          index.GetDatabaseName() ??
              GetStringAnnotation(index, DataVaultAnnotationNames.ProducedName) ??
              string.Join("_", index.Properties.Select(CreateColumnReference)),
          index.Properties.Select(CreateColumnReference).ToArray(),
          index.IsUnique,
          GetDescendingColumnNames(index).ToArray(),
          GetIncludedColumnNames(index));
    }
  }

  private static void AddMissingTableFinding(
      ICollection<DataVaultIdempotencyPreflightFinding> findings,
      string tableName,
      IEnumerable<DataVaultIdempotencyPreflightStructure> expectedStructures) {
    var representative = expectedStructures.First();
    findings.Add(new DataVaultIdempotencyPreflightFinding(
        DataVaultModelDriftSeverity.Blocking,
        "missing-idempotency-table",
        tableName,
        representative.OperationFamily,
        "table",
        tableName,
        "tables." + tableName,
        "Data Vault table with " + expectedStructures.Count().ToString() + " idempotency-critical structure(s)",
        "<missing>",
        "The expected Data Vault table is missing from the live schema; apply the reviewed schema migration before evaluating idempotent operations."));
  }

  private static void AddScalarFinding(
      ICollection<DataVaultIdempotencyPreflightFinding> findings,
      string code,
      DataVaultIdempotencyPreflightStructure expected,
      string propertyPath,
      string? expectedValue,
      string? actualValue,
      string message) {
    if (string.Equals(expectedValue, actualValue, StringComparison.Ordinal)) {
      return;
    }

    AddFinding(findings, code, expected, propertyPath, expectedValue, actualValue, message);
  }

  private static void AddListFinding(
      ICollection<DataVaultIdempotencyPreflightFinding> findings,
      string code,
      DataVaultIdempotencyPreflightStructure expected,
      string propertyPath,
      IReadOnlyList<string> expectedValue,
      IReadOnlyList<string> actualValue,
      string message) {
    if (HasSameValues(expectedValue, actualValue)) {
      return;
    }

    AddFinding(
        findings,
        code,
        expected,
        propertyPath,
        FormatValues(expectedValue),
        FormatValues(actualValue),
        message);
  }

  private static void AddFinding(
      ICollection<DataVaultIdempotencyPreflightFinding> findings,
      string code,
      DataVaultIdempotencyPreflightStructure expected,
      string propertyPath,
      string? expectedValue,
      string? actualValue,
      string message) {
    findings.Add(new DataVaultIdempotencyPreflightFinding(
        DataVaultModelDriftSeverity.Blocking,
        code,
        expected.TableName,
        expected.OperationFamily,
        expected.Kind,
        expected.Name,
        propertyPath,
        expectedValue,
        actualValue,
        message));
  }

  private static IReadOnlyList<DataVaultIdempotencyPreflightFinding> SortFindings(
      IEnumerable<DataVaultIdempotencyPreflightFinding> findings) {
    return findings
        .OrderBy(finding => finding.TableName, StringComparer.Ordinal)
        .ThenBy(finding => finding.OperationFamily, StringComparer.Ordinal)
        .ThenBy(finding => finding.StructureKind, StringComparer.Ordinal)
        .ThenBy(finding => finding.StructureName, StringComparer.Ordinal)
        .ThenBy(finding => finding.Code, StringComparer.Ordinal)
        .ThenBy(finding => finding.PropertyPath, StringComparer.Ordinal)
        .ToArray();
  }

  private static bool IsIdempotencyTable(IReadOnlyEntityType entityType) {
    return GetAnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind) is
        DataVaultTableKind.Hub or
        DataVaultTableKind.Link or
        DataVaultTableKind.Satellite or
        DataVaultTableKind.Pit or
        DataVaultTableKind.Bridge;
  }

  private static string GetOperationFamily(DataVaultTableKind tableKind) {
    return tableKind switch {
      DataVaultTableKind.Hub => HubOperationFamily,
      DataVaultTableKind.Link => LinkOperationFamily,
      DataVaultTableKind.Satellite => SatelliteOperationFamily,
      DataVaultTableKind.Pit => PitOperationFamily,
      DataVaultTableKind.Bridge => BridgeOperationFamily,
      _ => "idempotency-unsupported-table-kind",
    };
  }

  private static DataVaultTableKind GetTableKind(string operationFamily) {
    return operationFamily switch {
      HubOperationFamily => DataVaultTableKind.Hub,
      LinkOperationFamily => DataVaultTableKind.Link,
      SatelliteOperationFamily => DataVaultTableKind.Satellite,
      BridgeOperationFamily => DataVaultTableKind.Bridge,
      PitOperationFamily => DataVaultTableKind.Pit,
      _ => DataVaultTableKind.PointInTime,
    };
  }

  private static string CreateColumnReference(IReadOnlyProperty property) {
    return GetStringAnnotation(property, DataVaultAnnotationNames.ProducedName) ?? property.Name;
  }

  private static IEnumerable<string> GetDescendingColumnNames(IReadOnlyIndex index) {
    if (index.IsDescending is null) {
      yield break;
    }

    for (var ordinal = 0; ordinal < index.Properties.Count && ordinal < index.IsDescending.Count; ordinal++) {
      if (index.IsDescending[ordinal]) {
        yield return CreateColumnReference(index.Properties[ordinal]);
      }
    }
  }

  private static IReadOnlyList<string> GetIncludedColumnNames(IReadOnlyIndex index) {
    foreach (var annotationName in new[] { "SqlServer:Include", "Npgsql:IndexInclude" }) {
      var value = index.FindAnnotation(annotationName)?.Value;
      if (value is string[] stringArray) {
        return stringArray;
      }

      if (value is IEnumerable<string> stringValues) {
        return stringValues.ToArray();
      }
    }

    return Array.Empty<string>();
  }

  private static bool HasSameIndexSignature(
      DataVaultIdempotencyPreflightStructure expected,
      DataVaultLiveSchemaIndex actual) {
    return expected.IsUnique == actual.IsUnique &&
        HasSameValues(expected.ColumnNames, actual.ColumnNames) &&
        HasSameValues(expected.DescendingColumnNames, actual.DescendingColumnNames) &&
        HasSameValues(expected.IncludedColumnNames, actual.IncludedColumnNames);
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

  private static string FormatIndexSignature(DataVaultIdempotencyPreflightStructure index) {
    return "columns=" +
        FormatValues(index.ColumnNames) +
        "; unique=" +
        index.IsUnique.ToString() +
        "; descending=" +
        FormatValues(index.DescendingColumnNames) +
        "; included=" +
        FormatValues(index.IncludedColumnNames);
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

  private static string FormatProviderName(string? providerName) {
    return string.IsNullOrWhiteSpace(providerName) ? "<unknown>" : providerName;
  }

  private static string? TryGetProviderName(DbContext dbContext) {
    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
  }

  private static string? GetStringAnnotation(IReadOnlyAnnotatable annotatable, string annotationName) {
    return annotatable.FindAnnotation(annotationName)?.Value as string;
  }

  private static T GetAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : default;
  }

  private sealed class BuiltInDataVaultLiveSchemaReader : IDataVaultLiveSchemaReader {
    public Task<DataVaultLiveSchemaReadResult> ReadAsync(
        DbContext dbContext,
        CancellationToken cancellationToken = default) {
      return DataVaultLiveSchemaReader.ReadAsync(dbContext, cancellationToken);
    }
  }
}
