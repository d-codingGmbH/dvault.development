using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Structured aggregate Data Vault preflight report with deterministic section status and preserved lane reports.
/// </summary>
public sealed class DataVaultPreflightReport {
  private const string HashKeyStorageMigrationManifestNotProvided =
      "No hash-key storage migration manifest JSON was provided.";
  private const string IdempotencyLiveSchemaNotProvided = "No idempotency live schema read result was provided.";

  /// <summary>
  /// Initializes a new aggregate preflight report.
  /// </summary>
  /// <param name="validationProvider">The validation and provider-explain diagnostics section.</param>
  /// <param name="artifactDrift">The artifact-versus-design-time drift section.</param>
  /// <param name="snapshotDrift">The snapshot-model preflight drift section.</param>
  /// <param name="migrationGuardrail">The migration operation guardrail section.</param>
  /// <param name="requestDiagnostics">The representative request-bound diagnostics section.</param>
  public DataVaultPreflightReport(
      DataVaultPreflightSection<DataVaultDiagnosticsResult> validationProvider,
      DataVaultPreflightSection<DataVaultModelDriftReport> artifactDrift,
      DataVaultPreflightSection<DataVaultModelDriftPreflightReport> snapshotDrift,
      DataVaultPreflightSection<DataVaultMigrationGuardrailReport> migrationGuardrail,
      DataVaultPreflightSection<DataVaultPreflightRequestDiagnosticsReport> requestDiagnostics)
      : this(
          validationProvider,
          artifactDrift,
          snapshotDrift,
          new DataVaultPreflightSection<DataVaultIdempotencyPreflightReport>(
              "idempotency-schema",
              DataVaultPreflightSectionStatus.Skipped,
              report: null,
              IdempotencyLiveSchemaNotProvided),
          new DataVaultPreflightSection<DataVaultHashKeyStorageMigrationValidationResult>(
              "hash-key-storage-migration-manifest",
              DataVaultPreflightSectionStatus.Skipped,
              report: null,
              HashKeyStorageMigrationManifestNotProvided),
          migrationGuardrail,
          requestDiagnostics) {
  }

  /// <summary>
  /// Initializes a new aggregate preflight report with an explicit idempotency schema section.
  /// </summary>
  /// <param name="validationProvider">The validation and provider-explain diagnostics section.</param>
  /// <param name="artifactDrift">The artifact-versus-design-time drift section.</param>
  /// <param name="snapshotDrift">The snapshot-model preflight drift section.</param>
  /// <param name="idempotencySchema">The explicit idempotency constraint and index schema section.</param>
  /// <param name="migrationGuardrail">The migration operation guardrail section.</param>
  /// <param name="requestDiagnostics">The representative request-bound diagnostics section.</param>
  public DataVaultPreflightReport(
      DataVaultPreflightSection<DataVaultDiagnosticsResult> validationProvider,
      DataVaultPreflightSection<DataVaultModelDriftReport> artifactDrift,
      DataVaultPreflightSection<DataVaultModelDriftPreflightReport> snapshotDrift,
      DataVaultPreflightSection<DataVaultIdempotencyPreflightReport> idempotencySchema,
      DataVaultPreflightSection<DataVaultMigrationGuardrailReport> migrationGuardrail,
      DataVaultPreflightSection<DataVaultPreflightRequestDiagnosticsReport> requestDiagnostics)
      : this(
          validationProvider,
          artifactDrift,
          snapshotDrift,
          idempotencySchema,
          new DataVaultPreflightSection<DataVaultHashKeyStorageMigrationValidationResult>(
              "hash-key-storage-migration-manifest",
              DataVaultPreflightSectionStatus.Skipped,
              report: null,
              HashKeyStorageMigrationManifestNotProvided),
          migrationGuardrail,
          requestDiagnostics) {
  }

  /// <summary>
  /// Initializes a new aggregate preflight report with explicit idempotency and hash-key storage migration manifest sections.
  /// </summary>
  /// <param name="validationProvider">The validation and provider-explain diagnostics section.</param>
  /// <param name="artifactDrift">The artifact-versus-design-time drift section.</param>
  /// <param name="snapshotDrift">The snapshot-model preflight drift section.</param>
  /// <param name="idempotencySchema">The explicit idempotency constraint and index schema section.</param>
  /// <param name="hashKeyStorageMigrationManifest">The explicit hash-key storage migration manifest validation section.</param>
  /// <param name="migrationGuardrail">The migration operation guardrail section.</param>
  /// <param name="requestDiagnostics">The representative request-bound diagnostics section.</param>
  public DataVaultPreflightReport(
      DataVaultPreflightSection<DataVaultDiagnosticsResult> validationProvider,
      DataVaultPreflightSection<DataVaultModelDriftReport> artifactDrift,
      DataVaultPreflightSection<DataVaultModelDriftPreflightReport> snapshotDrift,
      DataVaultPreflightSection<DataVaultIdempotencyPreflightReport> idempotencySchema,
      DataVaultPreflightSection<DataVaultHashKeyStorageMigrationValidationResult> hashKeyStorageMigrationManifest,
      DataVaultPreflightSection<DataVaultMigrationGuardrailReport> migrationGuardrail,
      DataVaultPreflightSection<DataVaultPreflightRequestDiagnosticsReport> requestDiagnostics) {
    ArgumentNullException.ThrowIfNull(validationProvider);
    ArgumentNullException.ThrowIfNull(artifactDrift);
    ArgumentNullException.ThrowIfNull(snapshotDrift);
    ArgumentNullException.ThrowIfNull(idempotencySchema);
    ArgumentNullException.ThrowIfNull(hashKeyStorageMigrationManifest);
    ArgumentNullException.ThrowIfNull(migrationGuardrail);
    ArgumentNullException.ThrowIfNull(requestDiagnostics);

    ValidationProvider = validationProvider;
    ArtifactDrift = artifactDrift;
    SnapshotDrift = snapshotDrift;
    IdempotencySchema = idempotencySchema;
    HashKeyStorageMigrationManifest = hashKeyStorageMigrationManifest;
    MigrationGuardrail = migrationGuardrail;
    RequestDiagnostics = requestDiagnostics;
  }

  /// <summary>
  /// Gets the validation and provider-explain diagnostics section.
  /// </summary>
  public DataVaultPreflightSection<DataVaultDiagnosticsResult> ValidationProvider { get; }

  /// <summary>
  /// Gets the artifact-versus-design-time drift section.
  /// </summary>
  public DataVaultPreflightSection<DataVaultModelDriftReport> ArtifactDrift { get; }

  /// <summary>
  /// Gets the snapshot-model preflight drift section.
  /// </summary>
  public DataVaultPreflightSection<DataVaultModelDriftPreflightReport> SnapshotDrift { get; }

  /// <summary>
  /// Gets the explicit idempotency constraint and index schema section.
  /// </summary>
  public DataVaultPreflightSection<DataVaultIdempotencyPreflightReport> IdempotencySchema { get; }

  /// <summary>
  /// Gets the explicit hash-key storage migration manifest validation section.
  /// </summary>
  public DataVaultPreflightSection<DataVaultHashKeyStorageMigrationValidationResult> HashKeyStorageMigrationManifest { get; }

  /// <summary>
  /// Gets the migration operation guardrail section.
  /// </summary>
  public DataVaultPreflightSection<DataVaultMigrationGuardrailReport> MigrationGuardrail { get; }

  /// <summary>
  /// Gets the representative request-bound diagnostics section.
  /// </summary>
  public DataVaultPreflightSection<DataVaultPreflightRequestDiagnosticsReport> RequestDiagnostics { get; }

  /// <summary>
  /// Gets the deterministic overall preflight status.
  /// </summary>
  public DataVaultPreflightStatus Status =>
      Sections.Any(section => section.Status == DataVaultPreflightSectionStatus.Blocked)
          ? DataVaultPreflightStatus.Blocked
          : DataVaultPreflightStatus.Passed;

  /// <summary>
  /// Gets a value indicating whether any evaluated section reported a blocking condition.
  /// </summary>
  public bool IsBlocked => Status == DataVaultPreflightStatus.Blocked;

  /// <summary>
  /// Produces deterministic human-readable aggregate preflight output for console, test, or build logs.
  /// </summary>
  /// <returns>A concise deterministic display string with all lane summaries and preserved report renderings.</returns>
  public string ToDisplayString() {
    var sections = Sections;
    var builder = new StringBuilder();
    builder.Append("DVault preflight: ");
    builder.Append(FormatStatus(Status));
    builder.Append(", passed ");
    builder.Append(CountSections(sections, DataVaultPreflightSectionStatus.Passed).ToString(CultureInfo.InvariantCulture));
    builder.Append(", blocked ");
    builder.Append(CountSections(sections, DataVaultPreflightSectionStatus.Blocked).ToString(CultureInfo.InvariantCulture));
    builder.Append(", skipped ");
    builder.Append(CountSections(sections, DataVaultPreflightSectionStatus.Skipped).ToString(CultureInfo.InvariantCulture));
    builder.Append('.');

    AppendSection(builder, ValidationProvider, report => report.ToDisplayString());
    AppendSection(builder, ArtifactDrift, report => report.ToDisplayString());
    AppendSection(builder, SnapshotDrift, report => report.ToDisplayString());
    AppendSection(builder, IdempotencySchema, report => report.ToDisplayString());
    AppendSection(builder, HashKeyStorageMigrationManifest, report => report.ToDisplayString());
    AppendSection(builder, MigrationGuardrail, report => report.ToDisplayString());
    AppendSection(builder, RequestDiagnostics, report => report.ToDisplayString());

    return builder.ToString();
  }

  private IReadOnlyList<SectionSummary> Sections => [
    new(ValidationProvider.Name, ValidationProvider.Status),
    new(ArtifactDrift.Name, ArtifactDrift.Status),
    new(SnapshotDrift.Name, SnapshotDrift.Status),
    new(IdempotencySchema.Name, IdempotencySchema.Status),
    new(HashKeyStorageMigrationManifest.Name, HashKeyStorageMigrationManifest.Status),
    new(MigrationGuardrail.Name, MigrationGuardrail.Status),
    new(RequestDiagnostics.Name, RequestDiagnostics.Status),
  ];

  private static int CountSections(
      IEnumerable<SectionSummary> sections,
      DataVaultPreflightSectionStatus status) {
    return sections.Count(section => section.Status == status);
  }

  private static void AppendSection<TReport>(
      StringBuilder builder,
      DataVaultPreflightSection<TReport> section,
      Func<TReport, string> renderReport) where TReport : class {
    builder.AppendLine();
    builder.Append(section.Name);
    builder.Append(": ");
    builder.Append(FormatStatus(section.Status));

    if (section.Status == DataVaultPreflightSectionStatus.Skipped) {
      builder.Append(" (");
      builder.Append(section.SkipReason);
      builder.Append(')');
      return;
    }

    builder.AppendLine();
    builder.Append(renderReport(section.Report!));
  }

  private static string FormatStatus(DataVaultPreflightStatus status) {
    return status.ToString().ToLowerInvariant();
  }

  private static string FormatStatus(DataVaultPreflightSectionStatus status) {
    return status.ToString().ToLowerInvariant();
  }

  private readonly record struct SectionSummary(string Name, DataVaultPreflightSectionStatus Status);
}
