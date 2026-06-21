using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal static class DataVaultHashKeyStorageMigrationManifestExporter {
  public const string CurrentSchemaVersion = "dvault.hash-key-storage-migration.v1";

  private const string ExpectedDigestEncoding = "lowercase-hex-no-prefix";
  private const string HexStringConversionBehavior = "none-string-model";
  private const string BinaryConversionBehavior = "lowercase-hex-string-to-bytes";

  private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

  public static DataVaultExplainDiagnostics ImportSourceSupportBundleExplainJson(
      string json,
      string? logicalSourcePath = null) {
    ArgumentNullException.ThrowIfNull(json);

    try {
      using var document = JsonDocument.Parse(json);
      var root = document.RootElement;
      if (!root.TryGetProperty("schemaVersion", out var schemaVersion) ||
          schemaVersion.ValueKind != JsonValueKind.String ||
          !string.Equals(
              schemaVersion.GetString(),
              DataVaultSupportBundle.CurrentSchemaVersion,
              StringComparison.Ordinal)) {
        throw new InvalidOperationException(
            "The hash-key storage migration dry-run requires a source support bundle with schemaVersion '" +
            DataVaultSupportBundle.CurrentSchemaVersion +
            "'.");
      }

      if (!root.TryGetProperty("diagnostics", out var diagnosticsElement)) {
        throw new InvalidOperationException(
            "The hash-key storage migration dry-run requires a source support bundle with a diagnostics section.");
      }

      var diagnostics = diagnosticsElement.Deserialize<DataVaultDiagnosticsResult>(SerializerOptions);
      if (diagnostics is null) {
        throw new InvalidOperationException(
            "The hash-key storage migration dry-run could not read diagnostics from the source support bundle.");
      }

      return diagnostics.Explain;
    }
    catch (JsonException exception) {
      throw new InvalidOperationException(
          "The hash-key storage migration dry-run could not parse source support bundle '" +
          (string.IsNullOrWhiteSpace(logicalSourcePath) ? "<memory>" : logicalSourcePath) +
          "': " +
          exception.Message,
          exception);
    }
  }

  public static string ExportDryRunJson(
      DataVaultExplainDiagnostics source,
      DataVaultExplainDiagnostics target,
      string targetDiagnosticsSourceKind) {
    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(target);
    ArgumentException.ThrowIfNullOrWhiteSpace(targetDiagnosticsSourceKind);

    var sourceColumns = CreateComparableColumns(source, "source").ToArray();
    var targetColumns = CreateComparableColumns(target, "target").ToArray();
    var pairedColumns = ValidateAndPairColumns(source, target, sourceColumns, targetColumns);
    var entries = pairedColumns
        .Select((pair, ordinal) => CreateManifestEntry(ordinal, pair.Source, pair.Target))
        .ToArray();
    var hashKeyColumnCount = entries.Count(entry =>
        entry.LogicalPropertyKind == DataVaultLogicalPropertyKind.HashKey);
    var participantReferenceColumnCount = entries.Count(entry =>
        entry.LogicalPropertyKind == DataVaultLogicalPropertyKind.ParticipantReference);

    return JsonSerializer.Serialize(
        new DataVaultHashKeyStorageMigrationManifest(
            CurrentSchemaVersion,
            new DataVaultHashKeyStorageMigrationDryRun(
                Enabled: true,
                Status: "compatible-review-only",
                DatabaseMutation: "none",
                MigrationApplication: "not-run",
                PublicHashKeyBoundary: ExpectedDigestEncoding,
                TargetDiagnosticsSourceKind: targetDiagnosticsSourceKind),
            CreateEndpoint(source),
            CreateEndpoint(target),
            new DataVaultHashKeyStorageMigrationComparison(
                IntendedChange: "HexString-to-Binary",
                CompatibilityStatus: "compatible-storage-profile-flip",
                EntryCount: entries.Length,
                HashKeyColumnCount: hashKeyColumnCount,
                ParticipantReferenceColumnCount: participantReferenceColumnCount,
                Ordering: "ordinal by tableName then propertyName"),
            entries),
        SerializerOptions);
  }

  private static DataVaultHashKeyStorageMigrationEndpoint CreateEndpoint(DataVaultExplainDiagnostics explain) {
    return new DataVaultHashKeyStorageMigrationEndpoint(
        explain.MetadataSourceKind,
        explain.MetadataSourceFingerprint,
        explain.ProviderName,
        explain.CapabilityProfileName,
        explain.CapabilityProfileDefaulted);
  }

  private static IEnumerable<ComparableColumn> CreateComparableColumns(
      DataVaultExplainDiagnostics explain,
      string baselineName) {
    return explain.Entities
        .SelectMany(entity => entity.Properties
            .Where(IsHashKeyStorageColumn)
            .Select(property => CreateComparableColumn(entity, property, baselineName)))
        .OrderBy(column => column.TableName, StringComparer.Ordinal)
        .ThenBy(column => column.PropertyName, StringComparer.Ordinal);
  }

  private static ComparableColumn CreateComparableColumn(
      DataVaultEntityExplain entity,
      DataVaultPropertyExplain property,
      string baselineName) {
    return new ComparableColumn(
        entity.TableName,
        entity.TableKind,
        entity.MetadataName,
        property.Name,
        property.Role,
        property.TechnicalRole,
        property.LogicalPropertyKind,
        property.MetadataName,
        property.ProviderProfileName,
        CreateFacts(entity, property, baselineName));
  }

  private static bool IsHashKeyStorageColumn(DataVaultPropertyExplain property) {
    return property.LogicalPropertyKind is
        DataVaultLogicalPropertyKind.HashKey or
        DataVaultLogicalPropertyKind.ParticipantReference;
  }

  private static DataVaultHashKeyStorageMigrationColumnFacts CreateFacts(
      DataVaultEntityExplain entity,
      DataVaultPropertyExplain property,
      string baselineName) {
    var columnIdentity = entity.TableName + "." + property.Name;
    if (property.HashKeyStorageProfile is null) {
      throw MissingFact(baselineName, columnIdentity, "storage profile");
    }

    if (string.IsNullOrWhiteSpace(property.StoreType)) {
      throw MissingFact(baselineName, columnIdentity, "provider store type");
    }

    if (string.IsNullOrWhiteSpace(property.ClrTypeName)) {
      throw MissingFact(baselineName, columnIdentity, "EF CLR model type");
    }

    if (string.IsNullOrWhiteSpace(property.ConversionBehavior)) {
      throw MissingFact(baselineName, columnIdentity, "conversion behavior");
    }

    if (string.IsNullOrWhiteSpace(property.StableHashAlgorithmId)) {
      throw MissingFact(baselineName, columnIdentity, "stable-hash algorithm id");
    }

    if (property.DigestByteLength is not > 0) {
      throw MissingFact(baselineName, columnIdentity, "stable-hash digest byte length");
    }

    if (string.IsNullOrWhiteSpace(property.DigestEncoding)) {
      throw MissingFact(baselineName, columnIdentity, "digest encoding");
    }

    return new DataVaultHashKeyStorageMigrationColumnFacts(
        property.HashKeyStorageProfile.Value,
        property.StoreType,
        property.ValueFormat,
        property.ClrTypeName,
        property.ConversionBehavior,
        property.StableHashAlgorithmId,
        property.DigestByteLength.Value,
        property.DigestEncoding);
  }

  private static InvalidOperationException MissingFact(
      string baselineName,
      string columnIdentity,
      string factName) {
    return new InvalidOperationException(
        "The hash-key storage migration dry-run requires " +
        baselineName +
        " column '" +
        columnIdentity +
        "' to declare " +
        factName +
        ".");
  }

  private static IReadOnlyList<ComparableColumnPair> ValidateAndPairColumns(
      DataVaultExplainDiagnostics source,
      DataVaultExplainDiagnostics target,
      IReadOnlyList<ComparableColumn> sourceColumns,
      IReadOnlyList<ComparableColumn> targetColumns) {
    var issues = new List<string>();

    if (sourceColumns.Count == 0) {
      issues.Add("source baseline has no DVault HashKey or ParticipantReference columns");
    }

    if (targetColumns.Count == 0) {
      issues.Add("target baseline has no DVault HashKey or ParticipantReference columns");
    }

    if (!string.Equals(source.CapabilityProfileName, target.CapabilityProfileName, StringComparison.Ordinal)) {
      issues.Add(
          "capability profile changed from '" +
          source.CapabilityProfileName +
          "' to '" +
          target.CapabilityProfileName +
          "'");
    }

    if (!string.IsNullOrWhiteSpace(source.ProviderName) &&
        !string.IsNullOrWhiteSpace(target.ProviderName) &&
        !string.Equals(source.ProviderName, target.ProviderName, StringComparison.Ordinal)) {
      issues.Add(
          "provider changed from '" +
          source.ProviderName +
          "' to '" +
          target.ProviderName +
          "'");
    }

    if (!string.IsNullOrWhiteSpace(source.MetadataSourceFingerprint) &&
        !string.IsNullOrWhiteSpace(target.MetadataSourceFingerprint) &&
        !string.Equals(source.MetadataSourceFingerprint, target.MetadataSourceFingerprint, StringComparison.Ordinal)) {
      issues.Add("metadata source fingerprint changed outside the storage-profile migration boundary");
    }

    var sourceByIdentity = CreateColumnsByIdentity(sourceColumns, "source", issues);
    var targetByIdentity = CreateColumnsByIdentity(targetColumns, "target", issues);
    var pairs = new List<ComparableColumnPair>();

    foreach (var sourceColumn in sourceColumns) {
      if (!targetByIdentity.TryGetValue(sourceColumn.Identity, out var targetColumn)) {
        issues.Add("target baseline is missing column '" + sourceColumn.Identity + "'");
        continue;
      }

      ValidateColumnPair(sourceColumn, targetColumn, issues);
      pairs.Add(new ComparableColumnPair(sourceColumn, targetColumn));
    }

    foreach (var targetColumn in targetColumns) {
      if (!sourceByIdentity.ContainsKey(targetColumn.Identity)) {
        issues.Add("target baseline adds column '" + targetColumn.Identity + "'");
      }
    }

    if (issues.Count > 0) {
      throw new InvalidOperationException(
          "The hash-key storage migration dry-run detected compatibility drift: " +
          string.Join("; ", issues) +
          ".");
    }

    return pairs;
  }

  private static Dictionary<string, ComparableColumn> CreateColumnsByIdentity(
      IReadOnlyList<ComparableColumn> columns,
      string baselineName,
      List<string> issues) {
    var result = new Dictionary<string, ComparableColumn>(StringComparer.Ordinal);
    foreach (var column in columns) {
      if (!result.TryAdd(column.Identity, column)) {
        issues.Add(baselineName + " baseline has duplicate column identity '" + column.Identity + "'");
      }
    }

    return result;
  }

  private static void ValidateColumnPair(
      ComparableColumn source,
      ComparableColumn target,
      List<string> issues) {
    if (source.TableKind != target.TableKind) {
      issues.Add(source.Identity + " changed table kind from '" + source.TableKind + "' to '" + target.TableKind + "'");
    }

    if (source.PropertyRole != target.PropertyRole) {
      issues.Add(source.Identity + " changed property role from '" + source.PropertyRole + "' to '" + target.PropertyRole + "'");
    }

    if (source.TechnicalRole != target.TechnicalRole) {
      issues.Add(source.Identity + " changed technical role from '" + source.TechnicalRole + "' to '" + target.TechnicalRole + "'");
    }

    if (source.LogicalPropertyKind != target.LogicalPropertyKind) {
      issues.Add(
          source.Identity +
          " changed logical property kind from '" +
          source.LogicalPropertyKind +
          "' to '" +
          target.LogicalPropertyKind +
          "'");
    }

    if (!string.Equals(source.MetadataName, target.MetadataName, StringComparison.Ordinal)) {
      issues.Add(source.Identity + " changed metadata name from '" + source.MetadataName + "' to '" + target.MetadataName + "'");
    }

    if (!string.Equals(source.ProviderProfileName, target.ProviderProfileName, StringComparison.Ordinal)) {
      issues.Add(
          source.Identity +
          " changed provider profile from '" +
          source.ProviderProfileName +
          "' to '" +
          target.ProviderProfileName +
          "'");
    }

    ValidateSourceFacts(source, issues);
    ValidateTargetFacts(target, issues);
    ValidatePairFacts(source, target, issues);
  }

  private static void ValidateSourceFacts(ComparableColumn source, List<string> issues) {
    if (source.Facts.StorageProfile != DataVaultHashKeyStorageProfile.HexString) {
      issues.Add(source.Identity + " source storage profile is '" + source.Facts.StorageProfile + "' instead of 'HexString'");
    }

    if (source.Facts.ProviderValueFormat != DataVaultProviderValueFormat.LowercaseHexText) {
      issues.Add(
          source.Identity +
          " source provider value format is '" +
          source.Facts.ProviderValueFormat +
          "' instead of 'LowercaseHexText'");
    }

    if (!string.Equals(source.Facts.EfClrModelType, typeof(string).FullName, StringComparison.Ordinal)) {
      issues.Add(source.Identity + " source EF CLR model type is '" + source.Facts.EfClrModelType + "' instead of 'System.String'");
    }

    if (!string.Equals(source.Facts.ConversionBehavior, HexStringConversionBehavior, StringComparison.Ordinal)) {
      issues.Add(
          source.Identity +
          " source conversion behavior is '" +
          source.Facts.ConversionBehavior +
          "' instead of '" +
          HexStringConversionBehavior +
          "'");
    }
  }

  private static void ValidateTargetFacts(ComparableColumn target, List<string> issues) {
    if (target.Facts.StorageProfile != DataVaultHashKeyStorageProfile.Binary) {
      issues.Add(target.Identity + " target storage profile is '" + target.Facts.StorageProfile + "' instead of 'Binary'");
    }

    if (target.Facts.ProviderValueFormat != DataVaultProviderValueFormat.LowercaseHexBinary) {
      issues.Add(
          target.Identity +
          " target provider value format is '" +
          target.Facts.ProviderValueFormat +
          "' instead of 'LowercaseHexBinary'");
    }

    if (!string.Equals(target.Facts.EfClrModelType, typeof(string).FullName, StringComparison.Ordinal)) {
      issues.Add(target.Identity + " target EF CLR model type is '" + target.Facts.EfClrModelType + "' instead of 'System.String'");
    }

    if (!string.Equals(target.Facts.ConversionBehavior, BinaryConversionBehavior, StringComparison.Ordinal)) {
      issues.Add(
          target.Identity +
          " target conversion behavior is '" +
          target.Facts.ConversionBehavior +
          "' instead of '" +
          BinaryConversionBehavior +
          "'");
    }
  }

  private static void ValidatePairFacts(
      ComparableColumn source,
      ComparableColumn target,
      List<string> issues) {
    if (!string.Equals(source.Facts.AlgorithmId, target.Facts.AlgorithmId, StringComparison.Ordinal)) {
      issues.Add(
          source.Identity +
          " changed algorithmId from '" +
          source.Facts.AlgorithmId +
          "' to '" +
          target.Facts.AlgorithmId +
          "'");
    }

    if (source.Facts.DigestByteLength != target.Facts.DigestByteLength) {
      issues.Add(
          source.Identity +
          " changed digestByteLength from '" +
          source.Facts.DigestByteLength +
          "' to '" +
          target.Facts.DigestByteLength +
          "'");
    }

    if (!string.Equals(source.Facts.DigestEncoding, target.Facts.DigestEncoding, StringComparison.Ordinal) ||
        !string.Equals(source.Facts.DigestEncoding, ExpectedDigestEncoding, StringComparison.Ordinal)) {
      issues.Add(
          source.Identity +
          " changed digest encoding from '" +
          source.Facts.DigestEncoding +
          "' to '" +
          target.Facts.DigestEncoding +
          "'");
    }

    if (!string.Equals(source.Facts.EfClrModelType, target.Facts.EfClrModelType, StringComparison.Ordinal)) {
      issues.Add(
          source.Identity +
          " changed EF CLR model type from '" +
          source.Facts.EfClrModelType +
          "' to '" +
          target.Facts.EfClrModelType +
          "'");
    }

    if (string.Equals(source.Facts.ProviderStoreType, target.Facts.ProviderStoreType, StringComparison.Ordinal)) {
      issues.Add(
          source.Identity +
          " did not change provider store type for the HexString-to-Binary storage-profile flip");
    }
  }

  private static DataVaultHashKeyStorageMigrationEntry CreateManifestEntry(
      int ordinal,
      ComparableColumn source,
      ComparableColumn target) {
    return new DataVaultHashKeyStorageMigrationEntry(
        ordinal,
        target.TableName,
        target.TableKind,
        target.EntityMetadataName,
        target.PropertyName,
        target.PropertyRole,
        target.TechnicalRole,
        target.LogicalPropertyKind,
        target.MetadataName,
        source.Facts,
        target.Facts);
  }

  private static JsonSerializerOptions CreateSerializerOptions() {
    var options = new JsonSerializerOptions {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
    };
    options.Converters.Add(new JsonStringEnumConverter());

    return options;
  }

  private sealed record DataVaultHashKeyStorageMigrationManifest(
      [property: JsonPropertyOrder(0)] string SchemaVersion,
      [property: JsonPropertyOrder(1)] DataVaultHashKeyStorageMigrationDryRun DryRun,
      [property: JsonPropertyOrder(2)] DataVaultHashKeyStorageMigrationEndpoint Source,
      [property: JsonPropertyOrder(3)] DataVaultHashKeyStorageMigrationEndpoint Target,
      [property: JsonPropertyOrder(4)] DataVaultHashKeyStorageMigrationComparison Comparison,
      [property: JsonPropertyOrder(5)] IReadOnlyList<DataVaultHashKeyStorageMigrationEntry> Entries);

  private sealed record DataVaultHashKeyStorageMigrationDryRun(
      bool Enabled,
      string Status,
      string DatabaseMutation,
      string MigrationApplication,
      string PublicHashKeyBoundary,
      string TargetDiagnosticsSourceKind);

  private sealed record DataVaultHashKeyStorageMigrationEndpoint(
      string MetadataSourceKind,
      string? MetadataSourceFingerprint,
      string? ProviderName,
      string CapabilityProfile,
      bool CapabilityProfileDefaulted);

  private sealed record DataVaultHashKeyStorageMigrationComparison(
      string IntendedChange,
      string CompatibilityStatus,
      int EntryCount,
      int HashKeyColumnCount,
      int ParticipantReferenceColumnCount,
      string Ordering);

  private sealed record DataVaultHashKeyStorageMigrationEntry(
      int Ordinal,
      string TableName,
      DataVaultTableKind TableKind,
      string EntityMetadataName,
      string PropertyName,
      DataVaultPropertyRole PropertyRole,
      TechnicalMetadataColumnRole? TechnicalRole,
      DataVaultLogicalPropertyKind LogicalPropertyKind,
      string PropertyMetadataName,
      DataVaultHashKeyStorageMigrationColumnFacts Source,
      DataVaultHashKeyStorageMigrationColumnFacts Target);

  private sealed record DataVaultHashKeyStorageMigrationColumnFacts(
      DataVaultHashKeyStorageProfile StorageProfile,
      string ProviderStoreType,
      DataVaultProviderValueFormat ProviderValueFormat,
      string EfClrModelType,
      string ConversionBehavior,
      string AlgorithmId,
      int DigestByteLength,
      string DigestEncoding);

  private sealed record ComparableColumn(
      string TableName,
      DataVaultTableKind TableKind,
      string EntityMetadataName,
      string PropertyName,
      DataVaultPropertyRole PropertyRole,
      TechnicalMetadataColumnRole? TechnicalRole,
      DataVaultLogicalPropertyKind LogicalPropertyKind,
      string MetadataName,
      string ProviderProfileName,
      DataVaultHashKeyStorageMigrationColumnFacts Facts) {
    public string Identity => TableName + "/" + PropertyName;
  }

  private sealed record ComparableColumnPair(ComparableColumn Source, ComparableColumn Target);
}
