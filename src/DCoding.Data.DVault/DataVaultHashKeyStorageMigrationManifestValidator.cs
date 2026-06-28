using System.Globalization;
using System.Text.Json;

namespace DCoding.Data.DVault;

/// <summary>
/// Parses and validates the current hash-key storage migration dry-run manifest shape.
/// </summary>
public static class DataVaultHashKeyStorageMigrationManifestValidator {
  private const string ExpectedPublicHashKeyBoundary = "lowercase-hex-no-prefix";
  private const string ExpectedIntendedChange = "HexString-to-Binary";
  private const string ExpectedCompatibilityStatus = "compatible-storage-profile-flip";
  private const string ExpectedOrdering = "ordinal by tableName then propertyName";
  private const string ExpectedSourceStorageProfile = "HexString";
  private const string ExpectedTargetStorageProfile = "Binary";
  private const string ExpectedSourceProviderValueFormat = "LowercaseHexText";
  private const string ExpectedTargetProviderValueFormat = "LowercaseHexBinary";
  private const string ExpectedEfClrModelType = "System.String";
  private const string ExpectedSourceConversionBehavior = "none-string-model";
  private const string ExpectedTargetConversionBehavior = "lowercase-hex-string-to-bytes";

  private static readonly IReadOnlyDictionary<string, string> ProviderProfilesByProviderName =
      new Dictionary<string, string>(StringComparer.Ordinal) {
        ["Microsoft.EntityFrameworkCore.Sqlite"] = "sqlite-v1",
        ["Oracle.EntityFrameworkCore"] = "oracle-v1",
        ["Npgsql.EntityFrameworkCore.PostgreSQL"] = "postgres-v1",
        ["Microsoft.EntityFrameworkCore.SqlServer"] = "sqlserver-v1",
        ["IBM.EntityFrameworkCore"] = "db2-v1",
        ["MySql.EntityFrameworkCore"] = "mysql-pomelo-v1",
        ["Pomelo.EntityFrameworkCore.MySql"] = "mysql-pomelo-v1",
      };

  private static readonly IReadOnlySet<string> BuiltInCapabilityProfiles =
      new HashSet<string>(ProviderProfilesByProviderName.Values, StringComparer.Ordinal);

  private static readonly IReadOnlyDictionary<string, int> BuiltInStableHashDigestLengths =
      new Dictionary<string, int>(StringComparer.Ordinal) {
        ["sha256-v1"] = 32,
        ["sha1-v1"] = 20,
        ["sha256-128-v1"] = 16,
        ["sha256-160-v1"] = 20,
      };

  /// <summary>
  /// Parses and validates a serialized <c>dvault.hash-key-storage-migration.v1</c> dry-run manifest.
  /// </summary>
  /// <param name="json">The manifest JSON to validate.</param>
  /// <returns>A deterministic validation result with redacted findings.</returns>
  public static DataVaultHashKeyStorageMigrationValidationResult ValidateJson(string json) {
    ArgumentNullException.ThrowIfNull(json);

    var findings = new List<DataVaultHashKeyStorageMigrationValidationFinding>();
    try {
      using var document = JsonDocument.Parse(json);
      if (document.RootElement.ValueKind != JsonValueKind.Object) {
        AddError(
            findings,
            "hash-key-migration-root-object-required",
            "$",
            "object",
            FormatJsonValue(document.RootElement),
            "The hash-key storage migration manifest root must be a JSON object.");
        return CreateResult(findings);
      }

      ValidateRoot(document.RootElement, findings);
    }
    catch (JsonException exception) {
      AddError(
          findings,
          "hash-key-migration-json-malformed",
          "$",
          "valid JSON",
          "<malformed>",
          "The hash-key storage migration manifest JSON could not be parsed: " + exception.Message);
    }

    return CreateResult(findings);
  }

  private static void ValidateRoot(
      JsonElement root,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var schemaVersion = ReadRequiredString(
        root,
        "schemaVersion",
        "$.schemaVersion",
        findings);
    if (schemaVersion is not null &&
        !string.Equals(
            schemaVersion,
            DataVaultHashKeyStorageMigrationManifestExporter.CurrentSchemaVersion,
            StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-schema-version-unsupported",
          "$.schemaVersion",
          DataVaultHashKeyStorageMigrationManifestExporter.CurrentSchemaVersion,
          schemaVersion,
          "The manifest schemaVersion is not supported by the v1 validator.");
    }

    if (TryReadRequiredObject(root, "dryRun", "$.dryRun", findings, out var dryRun)) {
      ValidateDryRun(dryRun, findings);
    }

    EndpointFacts? source = null;
    if (TryReadRequiredObject(root, "source", "$.source", findings, out var sourceElement)) {
      source = ValidateEndpoint("source", sourceElement, "$.source", findings);
    }

    EndpointFacts? target = null;
    if (TryReadRequiredObject(root, "target", "$.target", findings, out var targetElement)) {
      target = ValidateEndpoint("target", targetElement, "$.target", findings);
    }

    ValidateEndpointPair(source, target, findings);

    ComparisonFacts? comparison = null;
    if (TryReadRequiredObject(root, "comparison", "$.comparison", findings, out var comparisonElement)) {
      comparison = ValidateComparison(comparisonElement, findings);
    }

    IReadOnlyList<EntryFacts> entries = Array.Empty<EntryFacts>();
    if (TryReadRequiredArray(root, "entries", "$.entries", findings, out var entriesElement)) {
      entries = ValidateEntries(entriesElement, findings);
    }

    ValidateCoverage(comparison, entries, findings);
  }

  private static void ValidateDryRun(
      JsonElement dryRun,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var enabled = ReadRequiredBool(dryRun, "enabled", "$.dryRun.enabled", findings);
    if (enabled is not null && enabled.Value != true) {
      AddError(
          findings,
          "hash-key-migration-dry-run-disabled",
          "$.dryRun.enabled",
          "true",
          enabled.Value.ToString().ToLowerInvariant(),
          "The manifest must describe a dry-run validation artifact.");
    }

    ValidateRequiredStringValue(
        dryRun,
        "status",
        "$.dryRun.status",
        "hash-key-migration-dry-run-status-unsupported",
        "compatible-review-only",
        findings);
    ValidateRequiredStringValue(
        dryRun,
        "databaseMutation",
        "$.dryRun.databaseMutation",
        "hash-key-migration-database-mutation-unsupported",
        "none",
        findings);
    ValidateRequiredStringValue(
        dryRun,
        "migrationApplication",
        "$.dryRun.migrationApplication",
        "hash-key-migration-application-unsupported",
        "not-run",
        findings);
    ValidateRequiredStringValue(
        dryRun,
        "publicHashKeyBoundary",
        "$.dryRun.publicHashKeyBoundary",
        "hash-key-migration-public-boundary-unsupported",
        ExpectedPublicHashKeyBoundary,
        findings);
    _ = ReadRequiredString(
        dryRun,
        "targetDiagnosticsSourceKind",
        "$.dryRun.targetDiagnosticsSourceKind",
        findings);
  }

  private static EndpointFacts ValidateEndpoint(
      string endpointName,
      JsonElement endpoint,
      string path,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var metadataSourceKind = ReadRequiredString(endpoint, "metadataSourceKind", path + ".metadataSourceKind", findings);
    var metadataSourceFingerprint = ReadRequiredString(
        endpoint,
        "metadataSourceFingerprint",
        path + ".metadataSourceFingerprint",
        findings);
    var providerName = ReadOptionalString(endpoint, "providerName", path + ".providerName", findings);
    var capabilityProfile = ReadRequiredString(endpoint, "capabilityProfile", path + ".capabilityProfile", findings);
    var capabilityProfileDefaulted = ReadRequiredBool(
        endpoint,
        "capabilityProfileDefaulted",
        path + ".capabilityProfileDefaulted",
        findings);

    if (capabilityProfile is not null && !BuiltInCapabilityProfiles.Contains(capabilityProfile)) {
      AddError(
          findings,
          "hash-key-migration-capability-profile-unsupported",
          path + ".capabilityProfile",
          FormatAllowedValues(BuiltInCapabilityProfiles),
          capabilityProfile,
          "The endpoint capability profile is outside the visible built-in hash-key storage migration baseline.");
    }

    if (!string.IsNullOrWhiteSpace(providerName)) {
      if (!ProviderProfilesByProviderName.TryGetValue(providerName, out var expectedCapabilityProfile)) {
        AddError(
            findings,
            "hash-key-migration-provider-unsupported",
            path + ".providerName",
            FormatAllowedValues(ProviderProfilesByProviderName.Keys),
            providerName,
            "The endpoint provider is outside the visible built-in hash-key storage migration baseline.");
      }
      else if (capabilityProfile is not null &&
          !string.Equals(expectedCapabilityProfile, capabilityProfile, StringComparison.Ordinal)) {
        AddError(
            findings,
            "hash-key-migration-provider-capability-mismatch",
            path + ".capabilityProfile",
            expectedCapabilityProfile,
            capabilityProfile,
            "The endpoint provider and capability profile do not describe the same built-in provider baseline.");
      }
    }

    if (capabilityProfileDefaulted == true) {
      AddWarning(
          findings,
          "hash-key-migration-capability-profile-defaulted",
          path + ".capabilityProfileDefaulted",
          "false",
          "true",
          "The endpoint capability profile was defaulted; validate the provider provenance before applying a cutover plan.");
    }

    return new EndpointFacts(
        endpointName,
        metadataSourceKind,
        metadataSourceFingerprint,
        providerName,
        capabilityProfile,
        capabilityProfileDefaulted);
  }

  private static void ValidateEndpointPair(
      EndpointFacts? source,
      EndpointFacts? target,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    if (source is null || target is null) {
      return;
    }

    if (!string.IsNullOrWhiteSpace(source.CapabilityProfile) &&
        !string.IsNullOrWhiteSpace(target.CapabilityProfile) &&
        !string.Equals(source.CapabilityProfile, target.CapabilityProfile, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-mixed-capability-profile",
          "$.target.capabilityProfile",
          source.CapabilityProfile,
          target.CapabilityProfile,
          "The source and target endpoints must use one provider capability profile for a storage-only migration.");
    }

    if (!string.IsNullOrWhiteSpace(source.ProviderName) &&
        !string.IsNullOrWhiteSpace(target.ProviderName) &&
        !string.Equals(source.ProviderName, target.ProviderName, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-mixed-provider",
          "$.target.providerName",
          source.ProviderName,
          target.ProviderName,
          "The source and target endpoints must use one provider for a storage-only migration.");
    }

    if (!string.IsNullOrWhiteSpace(source.MetadataSourceFingerprint) &&
        !string.IsNullOrWhiteSpace(target.MetadataSourceFingerprint) &&
        !string.Equals(source.MetadataSourceFingerprint, target.MetadataSourceFingerprint, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-metadata-source-fingerprint-drift",
          "$.target.metadataSourceFingerprint",
          source.MetadataSourceFingerprint,
          target.MetadataSourceFingerprint,
          "The source and target endpoints must be derived from the same reviewed metadata source.");
    }
  }

  private static ComparisonFacts ValidateComparison(
      JsonElement comparison,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    ValidateRequiredStringValue(
        comparison,
        "intendedChange",
        "$.comparison.intendedChange",
        "hash-key-migration-intended-change-unsupported",
        ExpectedIntendedChange,
        findings);
    ValidateRequiredStringValue(
        comparison,
        "compatibilityStatus",
        "$.comparison.compatibilityStatus",
        "hash-key-migration-compatibility-status-unsupported",
        ExpectedCompatibilityStatus,
        findings);
    ValidateRequiredStringValue(
        comparison,
        "ordering",
        "$.comparison.ordering",
        "hash-key-migration-ordering-unsupported",
        ExpectedOrdering,
        findings);

    return new ComparisonFacts(
        ReadRequiredInt(comparison, "entryCount", "$.comparison.entryCount", findings),
        ReadRequiredInt(comparison, "hashKeyColumnCount", "$.comparison.hashKeyColumnCount", findings),
        ReadRequiredInt(
            comparison,
            "participantReferenceColumnCount",
            "$.comparison.participantReferenceColumnCount",
            findings));
  }

  private static IReadOnlyList<EntryFacts> ValidateEntries(
      JsonElement entriesElement,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var entries = new List<EntryFacts>();
    var index = 0;
    foreach (var entryElement in entriesElement.EnumerateArray()) {
      var entryPath = "$.entries[" + index.ToString(CultureInfo.InvariantCulture) + "]";
      if (entryElement.ValueKind != JsonValueKind.Object) {
        AddError(
            findings,
            "hash-key-migration-entry-object-required",
            entryPath,
            "object",
            FormatJsonValue(entryElement),
            "Every manifest entry must be a JSON object.");
        index++;
        continue;
      }

      entries.Add(ValidateEntry(index, entryElement, entryPath, findings));
      index++;
    }

    if (entries.Count == 0) {
      AddError(
          findings,
          "hash-key-migration-coverage-empty",
          "$.entries",
          "at least one HashKey or ParticipantReference entry",
          "0 entries",
          "The manifest must cover every DVault-owned HashKey and ParticipantReference column.");
    }

    return entries;
  }

  private static EntryFacts ValidateEntry(
      int index,
      JsonElement entry,
      string entryPath,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var ordinal = ReadRequiredInt(entry, "ordinal", entryPath + ".ordinal", findings);
    var tableName = ReadRequiredString(entry, "tableName", entryPath + ".tableName", findings);
    _ = ReadRequiredString(entry, "tableKind", entryPath + ".tableKind", findings);
    _ = ReadRequiredString(entry, "entityMetadataName", entryPath + ".entityMetadataName", findings);
    var propertyName = ReadRequiredString(entry, "propertyName", entryPath + ".propertyName", findings);
    _ = ReadRequiredString(entry, "propertyRole", entryPath + ".propertyRole", findings);
    var logicalPropertyKind = ReadRequiredString(
        entry,
        "logicalPropertyKind",
        entryPath + ".logicalPropertyKind",
        findings);
    _ = ReadRequiredString(entry, "propertyMetadataName", entryPath + ".propertyMetadataName", findings);

    if (ordinal is not null && ordinal.Value != index) {
      AddError(
          findings,
          "hash-key-migration-entry-ordinal-mismatch",
          tableName,
          propertyName,
          entryPath + ".ordinal",
          index.ToString(CultureInfo.InvariantCulture),
          ordinal.Value.ToString(CultureInfo.InvariantCulture),
          "Manifest entry ordinals must be contiguous and match array order.");
    }

    if (logicalPropertyKind is not null &&
        !string.Equals(logicalPropertyKind, "HashKey", StringComparison.Ordinal) &&
        !string.Equals(logicalPropertyKind, "ParticipantReference", StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-logical-property-kind-unsupported",
          tableName,
          propertyName,
          entryPath + ".logicalPropertyKind",
          "HashKey or ParticipantReference",
          logicalPropertyKind,
          "Manifest coverage is limited to DVault-owned HashKey and ParticipantReference columns.");
    }

    ColumnFacts? source = null;
    if (TryReadRequiredObject(entry, "source", entryPath + ".source", findings, out var sourceElement)) {
      source = ValidateColumnFacts(
          tableName,
          propertyName,
          "source",
          sourceElement,
          entryPath + ".source",
          ExpectedSourceStorageProfile,
          ExpectedSourceProviderValueFormat,
          ExpectedSourceConversionBehavior,
          findings);
    }

    ColumnFacts? target = null;
    if (TryReadRequiredObject(entry, "target", entryPath + ".target", findings, out var targetElement)) {
      target = ValidateColumnFacts(
          tableName,
          propertyName,
          "target",
          targetElement,
          entryPath + ".target",
          ExpectedTargetStorageProfile,
          ExpectedTargetProviderValueFormat,
          ExpectedTargetConversionBehavior,
          findings);
    }

    ValidateColumnPair(tableName, propertyName, source, target, entryPath, findings);

    return new EntryFacts(
        index,
        ordinal,
        tableName,
        propertyName,
        logicalPropertyKind,
        source,
        target);
  }

  private static ColumnFacts ValidateColumnFacts(
      string? tableName,
      string? propertyName,
      string baselineName,
      JsonElement facts,
      string path,
      string expectedStorageProfile,
      string expectedProviderValueFormat,
      string expectedConversionBehavior,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var storageProfile = ReadRequiredString(facts, "storageProfile", path + ".storageProfile", findings);
    var providerStoreType = ReadRequiredString(facts, "providerStoreType", path + ".providerStoreType", findings);
    var providerValueFormat = ReadRequiredString(facts, "providerValueFormat", path + ".providerValueFormat", findings);
    var efClrModelType = ReadRequiredString(facts, "efClrModelType", path + ".efClrModelType", findings);
    var conversionBehavior = ReadRequiredString(facts, "conversionBehavior", path + ".conversionBehavior", findings);
    var algorithmId = ReadRequiredString(facts, "algorithmId", path + ".algorithmId", findings);
    var digestByteLength = ReadRequiredInt(facts, "digestByteLength", path + ".digestByteLength", findings);
    var digestEncoding = ReadRequiredString(facts, "digestEncoding", path + ".digestEncoding", findings);

    if (storageProfile is not null) {
      if (!string.Equals(storageProfile, ExpectedSourceStorageProfile, StringComparison.Ordinal) &&
          !string.Equals(storageProfile, ExpectedTargetStorageProfile, StringComparison.Ordinal)) {
        AddError(
            findings,
            "hash-key-migration-storage-profile-unsupported",
            tableName,
            propertyName,
            path + ".storageProfile",
            ExpectedSourceStorageProfile + " or " + ExpectedTargetStorageProfile,
            storageProfile,
            "The manifest contains a storage profile outside the v1 hash-key migration vocabulary.");
      }
      else if (!string.Equals(storageProfile, expectedStorageProfile, StringComparison.Ordinal)) {
        AddError(
            findings,
            "hash-key-migration-storage-profile-mismatch",
            tableName,
            propertyName,
            path + ".storageProfile",
            expectedStorageProfile,
            storageProfile,
            "The " + baselineName + " entry does not match the expected HexString-to-Binary storage boundary.");
      }
    }

    if (providerValueFormat is not null) {
      if (!string.Equals(providerValueFormat, ExpectedSourceProviderValueFormat, StringComparison.Ordinal) &&
          !string.Equals(providerValueFormat, ExpectedTargetProviderValueFormat, StringComparison.Ordinal)) {
        AddError(
            findings,
            "hash-key-migration-provider-value-format-unsupported",
            tableName,
            propertyName,
            path + ".providerValueFormat",
            ExpectedSourceProviderValueFormat + " or " + ExpectedTargetProviderValueFormat,
            providerValueFormat,
            "The manifest contains a provider value format outside the v1 hash-key migration vocabulary.");
      }
      else if (!string.Equals(providerValueFormat, expectedProviderValueFormat, StringComparison.Ordinal)) {
        AddError(
            findings,
            "hash-key-migration-provider-value-format-mismatch",
            tableName,
            propertyName,
            path + ".providerValueFormat",
            expectedProviderValueFormat,
            providerValueFormat,
            "The " + baselineName + " entry does not match the expected provider value format.");
      }
    }

    if (efClrModelType is not null &&
        !string.Equals(efClrModelType, ExpectedEfClrModelType, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-ef-clr-model-type-unsupported",
          tableName,
          propertyName,
          path + ".efClrModelType",
          ExpectedEfClrModelType,
          efClrModelType,
          "The EF model boundary must continue to expose hash keys as strings.");
    }

    if (conversionBehavior is not null &&
        !string.Equals(conversionBehavior, expectedConversionBehavior, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-conversion-behavior-unsupported",
          tableName,
          propertyName,
          path + ".conversionBehavior",
          expectedConversionBehavior,
          conversionBehavior,
          "The conversion behavior does not match the expected storage-profile baseline.");
    }

    if (algorithmId is not null) {
      if (!BuiltInStableHashDigestLengths.TryGetValue(algorithmId, out var expectedDigestByteLength)) {
        AddError(
            findings,
            "hash-key-migration-stable-hash-unsupported",
            tableName,
            propertyName,
            path + ".algorithmId",
            FormatAllowedValues(BuiltInStableHashDigestLengths.Keys),
            algorithmId,
            "The stable-hash algorithm id is outside the visible built-in baseline.");
      }
      else if (digestByteLength is not null && digestByteLength.Value != expectedDigestByteLength) {
        AddError(
            findings,
            "hash-key-migration-digest-length-unsupported",
            tableName,
            propertyName,
            path + ".digestByteLength",
            expectedDigestByteLength.ToString(CultureInfo.InvariantCulture),
            digestByteLength.Value.ToString(CultureInfo.InvariantCulture),
            "The digest byte length does not match the declared stable-hash algorithm.");
      }
    }

    if (digestEncoding is not null &&
        !string.Equals(digestEncoding, ExpectedPublicHashKeyBoundary, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-digest-encoding-unsupported",
          tableName,
          propertyName,
          path + ".digestEncoding",
          ExpectedPublicHashKeyBoundary,
          digestEncoding,
          "The digest encoding is outside the v1 public hash-key boundary.");
    }

    return new ColumnFacts(
        storageProfile,
        providerStoreType,
        providerValueFormat,
        efClrModelType,
        conversionBehavior,
        algorithmId,
        digestByteLength,
        digestEncoding);
  }

  private static void ValidateColumnPair(
      string? tableName,
      string? propertyName,
      ColumnFacts? source,
      ColumnFacts? target,
      string entryPath,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    if (source is null || target is null) {
      return;
    }

    if (!string.IsNullOrWhiteSpace(source.AlgorithmId) &&
        !string.IsNullOrWhiteSpace(target.AlgorithmId) &&
        !string.Equals(source.AlgorithmId, target.AlgorithmId, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-algorithm-drift",
          tableName,
          propertyName,
          entryPath + ".target.algorithmId",
          source.AlgorithmId,
          target.AlgorithmId,
          "The source and target stable-hash algorithm id must remain unchanged.");
    }

    if (source.DigestByteLength is not null &&
        target.DigestByteLength is not null &&
        source.DigestByteLength.Value != target.DigestByteLength.Value) {
      AddError(
          findings,
          "hash-key-migration-digest-length-drift",
          tableName,
          propertyName,
          entryPath + ".target.digestByteLength",
          source.DigestByteLength.Value.ToString(CultureInfo.InvariantCulture),
          target.DigestByteLength.Value.ToString(CultureInfo.InvariantCulture),
          "The source and target stable-hash digest byte length must remain unchanged.");
    }

    if (!string.IsNullOrWhiteSpace(source.DigestEncoding) &&
        !string.IsNullOrWhiteSpace(target.DigestEncoding) &&
        !string.Equals(source.DigestEncoding, target.DigestEncoding, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-digest-encoding-drift",
          tableName,
          propertyName,
          entryPath + ".target.digestEncoding",
          source.DigestEncoding,
          target.DigestEncoding,
          "The source and target digest encoding must remain unchanged.");
    }

    if (!string.IsNullOrWhiteSpace(source.ProviderStoreType) &&
        !string.IsNullOrWhiteSpace(target.ProviderStoreType) &&
        string.Equals(source.ProviderStoreType, target.ProviderStoreType, StringComparison.Ordinal)) {
      AddError(
          findings,
          "hash-key-migration-provider-store-type-unchanged",
          tableName,
          propertyName,
          entryPath + ".target.providerStoreType",
          "different from " + source.ProviderStoreType,
          target.ProviderStoreType,
          "The provider store type must change for the HexString-to-Binary storage-profile flip.");
    }
  }

  private static void ValidateCoverage(
      ComparisonFacts? comparison,
      IReadOnlyList<EntryFacts> entries,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var identities = new HashSet<string>(StringComparer.Ordinal);
    var sourceStorageProfiles = new HashSet<string>(StringComparer.Ordinal);
    var targetStorageProfiles = new HashSet<string>(StringComparer.Ordinal);

    var hashKeyColumnCount = 0;
    var participantReferenceColumnCount = 0;

    foreach (var entry in entries) {
      if (string.IsNullOrWhiteSpace(entry.TableName) || string.IsNullOrWhiteSpace(entry.PropertyName)) {
        AddError(
            findings,
            "hash-key-migration-coverage-identity-missing",
            entry.TableName,
            entry.PropertyName,
            "$.entries[" + entry.Index.ToString(CultureInfo.InvariantCulture) + "]",
            "tableName and propertyName",
            "<missing>",
            "Every covered column must have a deterministic table and property identity.");
      }
      else if (!identities.Add(entry.TableName + "/" + entry.PropertyName)) {
        AddError(
            findings,
            "hash-key-migration-coverage-identity-duplicate",
            entry.TableName,
            entry.PropertyName,
            "$.entries[" + entry.Index.ToString(CultureInfo.InvariantCulture) + "]",
            "unique tableName/propertyName",
            entry.TableName + "/" + entry.PropertyName,
            "The manifest contains duplicate column coverage identity.");
      }

      if (string.Equals(entry.LogicalPropertyKind, "HashKey", StringComparison.Ordinal)) {
        hashKeyColumnCount++;
      }
      else if (string.Equals(entry.LogicalPropertyKind, "ParticipantReference", StringComparison.Ordinal)) {
        participantReferenceColumnCount++;
      }

      if (!string.IsNullOrWhiteSpace(entry.Source?.StorageProfile)) {
        sourceStorageProfiles.Add(entry.Source.StorageProfile);
      }

      if (!string.IsNullOrWhiteSpace(entry.Target?.StorageProfile)) {
        targetStorageProfiles.Add(entry.Target.StorageProfile);
      }
    }

    if (sourceStorageProfiles.Count > 1) {
      AddError(
          findings,
          "hash-key-migration-mixed-source-storage-profile",
          "$.entries",
          ExpectedSourceStorageProfile,
          FormatAllowedValues(sourceStorageProfiles),
          "The source coverage contains mixed storage profiles.");
    }

    if (targetStorageProfiles.Count > 1) {
      AddError(
          findings,
          "hash-key-migration-mixed-target-storage-profile",
          "$.entries",
          ExpectedTargetStorageProfile,
          FormatAllowedValues(targetStorageProfiles),
          "The target coverage contains mixed storage profiles.");
    }

    ValidateEntryOrdering(entries, findings);

    if (comparison is null) {
      return;
    }

    ValidateComparisonCount(
        comparison.EntryCount,
        entries.Count,
        "$.comparison.entryCount",
        "hash-key-migration-coverage-entry-count-mismatch",
        "The comparison entryCount must match the number of manifest entries.",
        findings);
    ValidateComparisonCount(
        comparison.HashKeyColumnCount,
        hashKeyColumnCount,
        "$.comparison.hashKeyColumnCount",
        "hash-key-migration-coverage-hash-key-count-mismatch",
        "The comparison hashKeyColumnCount must match covered HashKey entries.",
        findings);
    ValidateComparisonCount(
        comparison.ParticipantReferenceColumnCount,
        participantReferenceColumnCount,
        "$.comparison.participantReferenceColumnCount",
        "hash-key-migration-coverage-participant-reference-count-mismatch",
        "The comparison participantReferenceColumnCount must match covered ParticipantReference entries.",
        findings);
  }

  private static void ValidateEntryOrdering(
      IReadOnlyList<EntryFacts> entries,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var sortableEntries = entries
        .Where(entry => !string.IsNullOrWhiteSpace(entry.TableName) && !string.IsNullOrWhiteSpace(entry.PropertyName))
        .ToArray();
    var sortedEntries = sortableEntries
        .OrderBy(entry => entry.TableName, StringComparer.Ordinal)
        .ThenBy(entry => entry.PropertyName, StringComparer.Ordinal)
        .ToArray();

    for (var index = 0; index < sortableEntries.Length; index++) {
      if (ReferenceEquals(sortableEntries[index], sortedEntries[index])) {
        continue;
      }

      AddError(
          findings,
          "hash-key-migration-entry-ordering-drift",
          sortableEntries[index].TableName,
          sortableEntries[index].PropertyName,
          "$.entries[" + sortableEntries[index].Index.ToString(CultureInfo.InvariantCulture) + "]",
          ExpectedOrdering,
          sortableEntries[index].TableName + "/" + sortableEntries[index].PropertyName,
          "Manifest entries must remain ordered by tableName then propertyName.");
      return;
    }
  }

  private static void ValidateComparisonCount(
      int? actualCount,
      int expectedCount,
      string path,
      string code,
      string message,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    if (actualCount is null || actualCount.Value == expectedCount) {
      return;
    }

    AddError(
        findings,
        code,
        path,
        expectedCount.ToString(CultureInfo.InvariantCulture),
        actualCount.Value.ToString(CultureInfo.InvariantCulture),
        message);
  }

  private static void ValidateRequiredStringValue(
      JsonElement parent,
      string propertyName,
      string path,
      string code,
      string expectedValue,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var actualValue = ReadRequiredString(parent, propertyName, path, findings);
    if (actualValue is null || string.Equals(actualValue, expectedValue, StringComparison.Ordinal)) {
      return;
    }

    AddError(
        findings,
        code,
        path,
        expectedValue,
        actualValue,
        "The manifest field does not match the v1 hash-key storage migration contract.");
  }

  private static bool TryReadRequiredObject(
      JsonElement parent,
      string propertyName,
      string path,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings,
      out JsonElement value) {
    if (!parent.TryGetProperty(propertyName, out value)) {
      AddError(
          findings,
          "hash-key-migration-required-section-missing",
          path,
          "object",
          "<missing>",
          "The manifest is missing a required section.");
      return false;
    }

    if (value.ValueKind != JsonValueKind.Object) {
      AddError(
          findings,
          "hash-key-migration-required-section-invalid",
          path,
          "object",
          FormatJsonValue(value),
          "The manifest required section must be a JSON object.");
      return false;
    }

    return true;
  }

  private static bool TryReadRequiredArray(
      JsonElement parent,
      string propertyName,
      string path,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings,
      out JsonElement value) {
    if (!parent.TryGetProperty(propertyName, out value)) {
      AddError(
          findings,
          "hash-key-migration-required-section-missing",
          path,
          "array",
          "<missing>",
          "The manifest is missing a required section.");
      return false;
    }

    if (value.ValueKind != JsonValueKind.Array) {
      AddError(
          findings,
          "hash-key-migration-required-section-invalid",
          path,
          "array",
          FormatJsonValue(value),
          "The manifest required section must be a JSON array.");
      return false;
    }

    return true;
  }

  private static string? ReadRequiredString(
      JsonElement parent,
      string propertyName,
      string path,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    if (!parent.TryGetProperty(propertyName, out var value)) {
      AddError(
          findings,
          "hash-key-migration-required-field-missing",
          path,
          "string",
          "<missing>",
          "The manifest is missing a required string field.");
      return null;
    }

    if (value.ValueKind != JsonValueKind.String) {
      AddError(
          findings,
          "hash-key-migration-required-field-invalid",
          path,
          "string",
          FormatJsonValue(value),
          "The manifest field must be a JSON string.");
      return null;
    }

    var text = value.GetString();
    if (string.IsNullOrWhiteSpace(text)) {
      AddError(
          findings,
          "hash-key-migration-required-field-empty",
          path,
          "non-empty string",
          text is null ? "<null>" : "<empty>",
          "The manifest string field must not be empty.");
      return null;
    }

    return text;
  }

  private static string? ReadOptionalString(
      JsonElement parent,
      string propertyName,
      string path,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    if (!parent.TryGetProperty(propertyName, out var value) || value.ValueKind == JsonValueKind.Null) {
      return null;
    }

    if (value.ValueKind != JsonValueKind.String) {
      AddError(
          findings,
          "hash-key-migration-optional-field-invalid",
          path,
          "string or null",
          FormatJsonValue(value),
          "The manifest optional field must be a JSON string or null.");
      return null;
    }

    return value.GetString();
  }

  private static bool? ReadRequiredBool(
      JsonElement parent,
      string propertyName,
      string path,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    if (!parent.TryGetProperty(propertyName, out var value)) {
      AddError(
          findings,
          "hash-key-migration-required-field-missing",
          path,
          "boolean",
          "<missing>",
          "The manifest is missing a required boolean field.");
      return null;
    }

    if (value.ValueKind is not JsonValueKind.True and not JsonValueKind.False) {
      AddError(
          findings,
          "hash-key-migration-required-field-invalid",
          path,
          "boolean",
          FormatJsonValue(value),
          "The manifest field must be a JSON boolean.");
      return null;
    }

    return value.GetBoolean();
  }

  private static int? ReadRequiredInt(
      JsonElement parent,
      string propertyName,
      string path,
      List<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    if (!parent.TryGetProperty(propertyName, out var value)) {
      AddError(
          findings,
          "hash-key-migration-required-field-missing",
          path,
          "integer",
          "<missing>",
          "The manifest is missing a required integer field.");
      return null;
    }

    if (value.ValueKind != JsonValueKind.Number || !value.TryGetInt32(out var result)) {
      AddError(
          findings,
          "hash-key-migration-required-field-invalid",
          path,
          "integer",
          FormatJsonValue(value),
          "The manifest field must be a JSON integer.");
      return null;
    }

    return result;
  }

  private static DataVaultHashKeyStorageMigrationValidationResult CreateResult(
      IReadOnlyCollection<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    var orderedFindings = SortFindings(findings).ToList();
    if (orderedFindings.All(finding => finding.Severity != DataVaultDiagnosticsIssueSeverity.Error)) {
      orderedFindings.Add(new DataVaultHashKeyStorageMigrationValidationFinding(
          DataVaultDiagnosticsIssueSeverity.Info,
          "hash-key-migration-manifest-compatible",
          null,
          null,
          "$",
          DataVaultHashKeyStorageMigrationManifestExporter.CurrentSchemaVersion,
          DataVaultHashKeyStorageMigrationManifestExporter.CurrentSchemaVersion,
          "The manifest matches the v1 HexString-to-Binary storage-only validation contract."));
      orderedFindings = SortFindings(orderedFindings).ToList();
    }

    return new DataVaultHashKeyStorageMigrationValidationResult(orderedFindings);
  }

  private static IEnumerable<DataVaultHashKeyStorageMigrationValidationFinding> SortFindings(
      IEnumerable<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    return findings
        .OrderBy(finding => GetSeveritySortKey(finding.Severity))
        .ThenBy(finding => finding.Code, StringComparer.Ordinal)
        .ThenBy(finding => finding.TableName ?? string.Empty, StringComparer.Ordinal)
        .ThenBy(finding => finding.ColumnName ?? string.Empty, StringComparer.Ordinal)
        .ThenBy(finding => finding.Path, StringComparer.Ordinal);
  }

  private static int GetSeveritySortKey(DataVaultDiagnosticsIssueSeverity severity) {
    return severity switch {
      DataVaultDiagnosticsIssueSeverity.Error => 0,
      DataVaultDiagnosticsIssueSeverity.Warning => 1,
      DataVaultDiagnosticsIssueSeverity.Info => 2,
      _ => 3,
    };
  }

  private static string FormatAllowedValues(IEnumerable<string> values) {
    return string.Join(", ", values.Order(StringComparer.Ordinal));
  }

  private static string FormatJsonValue(JsonElement value) {
    return value.ValueKind switch {
      JsonValueKind.String => value.GetString() ?? "<null>",
      JsonValueKind.Number => value.GetRawText(),
      JsonValueKind.True => "true",
      JsonValueKind.False => "false",
      JsonValueKind.Null => "<null>",
      JsonValueKind.Object => "<object>",
      JsonValueKind.Array => "<array>",
      JsonValueKind.Undefined => "<undefined>",
      _ => "<unknown>",
    };
  }

  private static void AddError(
      ICollection<DataVaultHashKeyStorageMigrationValidationFinding> findings,
      string code,
      string path,
      string? expectedValue,
      string? actualValue,
      string message) {
    AddError(findings, code, null, null, path, expectedValue, actualValue, message);
  }

  private static void AddError(
      ICollection<DataVaultHashKeyStorageMigrationValidationFinding> findings,
      string code,
      string? tableName,
      string? columnName,
      string path,
      string? expectedValue,
      string? actualValue,
      string message) {
    findings.Add(new DataVaultHashKeyStorageMigrationValidationFinding(
        DataVaultDiagnosticsIssueSeverity.Error,
        code,
        tableName,
        columnName,
        path,
        expectedValue,
        actualValue,
        message));
  }

  private static void AddWarning(
      ICollection<DataVaultHashKeyStorageMigrationValidationFinding> findings,
      string code,
      string path,
      string? expectedValue,
      string? actualValue,
      string message) {
    findings.Add(new DataVaultHashKeyStorageMigrationValidationFinding(
        DataVaultDiagnosticsIssueSeverity.Warning,
        code,
        null,
        null,
        path,
        expectedValue,
        actualValue,
        message));
  }

  private sealed record EndpointFacts(
      string Name,
      string? MetadataSourceKind,
      string? MetadataSourceFingerprint,
      string? ProviderName,
      string? CapabilityProfile,
      bool? CapabilityProfileDefaulted);

  private sealed record ComparisonFacts(
      int? EntryCount,
      int? HashKeyColumnCount,
      int? ParticipantReferenceColumnCount);

  private sealed record EntryFacts(
      int Index,
      int? Ordinal,
      string? TableName,
      string? PropertyName,
      string? LogicalPropertyKind,
      ColumnFacts? Source,
      ColumnFacts? Target);

  private sealed record ColumnFacts(
      string? StorageProfile,
      string? ProviderStoreType,
      string? ProviderValueFormat,
      string? EfClrModelType,
      string? ConversionBehavior,
      string? AlgorithmId,
      int? DigestByteLength,
      string? DigestEncoding);
}
