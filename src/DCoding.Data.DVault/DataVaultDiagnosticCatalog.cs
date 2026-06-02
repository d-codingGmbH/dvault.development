namespace DCoding.Data.DVault;

internal static class DataVaultDiagnosticCatalog {
  private const string ErrorSeverity = "error";
  private const string WarningSeverity = "warning";

  private static readonly IReadOnlyList<DataVaultDiagnosticDefinition> ModelArtifactSeedDefinitions =
  [
      new DataVaultDiagnosticDefinition(
          "DMV1001",
          ErrorSeverity,
          "schema-version",
          "Missing schema version",
          "Raised when a dvault.model.v1 artifact does not declare a non-blank string schemaVersion.",
          "Add schemaVersion with the supported value dvault.model.v1."),
      new DataVaultDiagnosticDefinition(
          "DMV1002",
          ErrorSeverity,
          "schema-version",
          "Unsupported schema version",
          "Raised when a dvault.model.v1 artifact declares a schemaVersion other than the current supported version.",
          "Change schemaVersion to dvault.model.v1 or process the artifact with a compatible importer."),
      new DataVaultDiagnosticDefinition(
          "DMV1101",
          ErrorSeverity,
          "shape",
          "Unknown artifact field",
          "Raised when an artifact object contains a field that is not part of the dvault.model.v1 contract.",
          "Remove the unknown field or move the information into a supported artifact field."),
      new DataVaultDiagnosticDefinition(
          "DMV1102",
          ErrorSeverity,
          "shape",
          "Invalid artifact shape",
          "Raised when JSON syntax, object structure, value kind, or required scalar shape does not match dvault.model.v1.",
          "Correct the JSON shape so required objects, arrays, and string values use the documented structure."),
      new DataVaultDiagnosticDefinition(
          "DMV1103",
          ErrorSeverity,
          "shape",
          "Empty required collection",
          "Raised when a required dvault.model.v1 collection is present but does not contain the minimum number of entries.",
          "Add the required entries to the collection before importing the artifact."),
      new DataVaultDiagnosticDefinition(
          "DMV1201",
          ErrorSeverity,
          "duplicate",
          "Duplicate declaration name",
          "Raised when two top-level model declarations of the same kind use the same logical name.",
          "Rename or remove the duplicate declaration so each logical name is unique within its declaration kind."),
      new DataVaultDiagnosticDefinition(
          "DMV1202",
          ErrorSeverity,
          "duplicate",
          "Duplicate member or participant name",
          "Raised when a declaration repeats a member name, participant role, business key, payload, or driving key that must be unique.",
          "Remove the repeated value or choose a distinct name for each member in the affected declaration."),
      new DataVaultDiagnosticDefinition(
          "DMV1203",
          ErrorSeverity,
          "duplicate",
          "Duplicate PIT or bridge binding",
          "Raised when a PIT repeats a satellite reference or a bridge binds both endpoints to the same source participant.",
          "Use each satellite reference once and bind bridge endpoints to distinct source-link participants."),
      new DataVaultDiagnosticDefinition(
          "DMV1301",
          ErrorSeverity,
          "reference",
          "Missing model reference",
          "Raised when a declaration references a hub, link, satellite, role, or endpoint that is not declared in the artifact.",
          "Declare the referenced model element or update the reference to an existing element of the required kind."),
      new DataVaultDiagnosticDefinition(
          "DMV1302",
          ErrorSeverity,
          "reference",
          "Wrong reference kind",
          "Raised when a declaration references an existing model element through the wrong kind of relationship.",
          "Point the reference at an element of the expected kind or correct the declaration's reference kind."),
      new DataVaultDiagnosticDefinition(
          "DMV1303",
          ErrorSeverity,
          "reference",
          "PIT satellite parent mismatch",
          "Raised when a PIT references a satellite that does not belong to the PIT hub.",
          "Reference only satellites whose parent hub matches the PIT hub."),
      new DataVaultDiagnosticDefinition(
          "DMV1401",
          ErrorSeverity,
          "naming",
          "Default naming collision",
          "Raised when default naming would produce the same logical persistence name for multiple declarations or columns.",
          "Rename one of the colliding declarations or roles so default naming produces unique names."),
      new DataVaultDiagnosticDefinition(
          "DMV1501",
          ErrorSeverity,
          "capability",
          "Unsupported metadata capability",
          "Raised when the artifact asks for a modeling capability that cannot be mapped to the current DVault metadata surface.",
          "Use only supported dvault.model.v1 capabilities or split the model into declarations the current runtime can map."),
      new DataVaultDiagnosticDefinition(
          "DMV1502",
          ErrorSeverity,
          "provider-choice",
          "Unsupported provider-specific choice",
          "Raised when the artifact includes a provider-specific field or option that dvault.model.v1 does not accept.",
          "Remove provider-specific fields or use one of the provider-neutral choices supported by the importer."),
      new DataVaultDiagnosticDefinition(
          "DMV1601",
          ErrorSeverity,
          "recursive-participant-binding",
          "Ambiguous recursive participant binding",
          "Raised when a hierarchy bridge or recursive participant binding cannot determine distinct source-link roles.",
          "Declare distinct participant roles and bind hierarchy endpoints to unambiguous role-specific participants."),
      new DataVaultDiagnosticDefinition(
          "DMV1602",
          ErrorSeverity,
          "recursive-participant-binding",
          "Recursive link role required",
          "Raised when a link repeats the same hub without roles on every repeated occurrence.",
          "Assign explicit roles to each repeated participant so the recursive link can be resolved deterministically."),
      new DataVaultDiagnosticDefinition(
          "DMV1701",
          ErrorSeverity,
          "shape",
          "Driving key overlaps payload",
          "Raised when a satellite declares the same name as both a driving key and a payload field.",
          "Remove the overlapping field from either drivingKeys or payload so each field has one satellite role."),
      new DataVaultDiagnosticDefinition(
          "DMV1801",
          ErrorSeverity,
          "projection",
          "Artifact projection failed",
          "Raised when a parsed artifact cannot be projected into Entity Framework metadata after import validation.",
          "Review the projection error, adjust the affected declaration, and retry the import before applying metadata."),
  ];

  private static readonly IReadOnlyDictionary<string, DataVaultDiagnosticDefinition> ModelArtifactSeedDefinitionsByCode =
      ModelArtifactSeedDefinitions.ToDictionary(definition => definition.Code, StringComparer.Ordinal);
  private static readonly IReadOnlyList<DataVaultDiagnosticDefinition> MigrationOperationSeedDefinitions =
  [
      new DataVaultDiagnosticDefinition(
          "DVM2001",
          ErrorSeverity,
          "migration-guardrail",
          "Hub or link payload column created",
          "Raised when a migration adds or creates a descriptive payload column on a Data Vault hub or link table.",
          "Move descriptive values to a satellite or exclude the payload column from the Data Vault-produced hub or link table."),
      new DataVaultDiagnosticDefinition(
          "DVM2002",
          ErrorSeverity,
          "migration-guardrail",
          "Required technical column changed",
          "Raised when a migration drops, alters, or omits LoadTimestamp, RecordSource, satellite HashDiff, or PIT load-timestamp columns.",
          "Keep required technical columns aligned with the Data Vault metadata translator output."),
      new DataVaultDiagnosticDefinition(
          "DVM2003",
          ErrorSeverity,
          "migration-guardrail",
          "Data Vault generated column or structural shape changed",
          "Raised when a migration adds, drops, alters, or omits a generated hash key, participant reference, parent hash key, satellite payload, satellite driving key, PIT snapshot reference, or bridge traversal-depth column.",
          "Preserve the translated generated columns or re-author the migration as an explicit Data Vault metadata evolution."),
      new DataVaultDiagnosticDefinition(
          "DVM2004",
          WarningSeverity,
          "migration-guardrail",
          "Default key or index coverage changed",
          "Raised when a migration drops, renames, creates, or recreates a Data Vault primary key or default index with different uniqueness, column order, or target columns.",
          "Keep primary-key and default index definitions aligned with the Data Vault naming policy and translated schema baseline."),
      new DataVaultDiagnosticDefinition(
          "DVM2005",
          WarningSeverity,
          "migration-guardrail",
          "Data Vault-owned generated structure renamed",
          "Raised when a migration explicitly renames a Data Vault-owned table, column, primary key, or default index away from its produced name.",
          "Keep Data Vault-owned generated names aligned with the current naming policy output or keep the explicit rename operation visible when the metadata evolution is intentional."),
      new DataVaultDiagnosticDefinition(
          "DVM2006",
          ErrorSeverity,
          "migration-guardrail",
          "Data Vault-owned table dropped",
          "Raised when a migration drops a hub, link, satellite, PIT, or bridge table produced by Data Vault metadata translation.",
          "Do not drop Data Vault-produced hub, link, satellite, PIT, or bridge tables unless the owning metadata declaration is intentionally removed."),
      new DataVaultDiagnosticDefinition(
          "DVM2007",
          ErrorSeverity,
          "migration-guardrail",
          "Data Vault generated index or constraint dropped",
          "Raised when a migration drops a Data Vault-produced secondary index or named generated primary-key constraint.",
          "Do not drop Data Vault-produced indexes or named generated constraints; restore the generated object or re-author the migration as an explicit rename or metadata evolution."),
      new DataVaultDiagnosticDefinition(
          "DVM2008",
          WarningSeverity,
          "migration-guardrail",
          "Suspicious Data Vault generated structure replacement",
          "Raised when a migration uses a drop-plus-add sequence that appears to replace a generated Data Vault table, column, index, or constraint without an explicit EF rename operation.",
          "Review the metadata name and produced-name continuity; use an explicit EF rename operation for intentional renames or restore the original generated structure before applying the migration."),
  ];

  private static readonly IReadOnlyDictionary<string, DataVaultDiagnosticDefinition> MigrationOperationSeedDefinitionsByCode =
      MigrationOperationSeedDefinitions.ToDictionary(definition => definition.Code, StringComparer.Ordinal);

  internal static IReadOnlyList<DataVaultDiagnosticDefinition> ModelArtifactDefinitions => ModelArtifactSeedDefinitions;

  internal static IReadOnlyList<DataVaultDiagnosticDefinition> MigrationOperationDefinitions => MigrationOperationSeedDefinitions;

  internal static DataVaultDiagnosticDefinition GetModelArtifactDefinition(string code) {
    ArgumentException.ThrowIfNullOrWhiteSpace(code);

    if (ModelArtifactSeedDefinitionsByCode.TryGetValue(code, out var definition)) {
      return definition;
    }

    throw new InvalidOperationException("Diagnostic code '" + code + "' is not part of the model-artifact diagnostic catalog.");
  }

  internal static DataVaultDiagnosticDefinition GetMigrationOperationDefinition(string code) {
    ArgumentException.ThrowIfNullOrWhiteSpace(code);

    if (MigrationOperationSeedDefinitionsByCode.TryGetValue(code, out var definition)) {
      return definition;
    }

    throw new InvalidOperationException("Diagnostic code '" + code + "' is not part of the migration-operation diagnostic catalog.");
  }
}
