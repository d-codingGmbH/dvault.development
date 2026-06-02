using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DCoding.Data.DVault;

/// <summary>
/// Composes existing Data Vault diagnostics, drift, guardrail, and request-bound diagnostics into one preflight report.
/// </summary>
public static class DataVaultPreflight {
  private const string ArtifactDriftNotProvided = "No reviewed model artifact import result was provided.";
  private const string IdempotencyLiveSchemaNotProvided = "No idempotency live schema read result was provided.";
  private const string MigrationOperationsNotProvided = "No migration operations were provided.";
  private const string RequestDiagnosticsNotProvided = "No representative request diagnostics were provided.";
  private const string SnapshotModelNotProvided = "No snapshot model was provided.";

  /// <summary>
  /// Runs one aggregate preflight evaluation against a configured DbContext and explicit caller-owned inputs.
  /// </summary>
  /// <param name="diagnostics">The diagnostics service used for validation and migration guardrail baselines.</param>
  /// <param name="request">The explicit caller-owned preflight request.</param>
  /// <returns>A structured aggregate preflight report with deterministic per-lane status.</returns>
  public static DataVaultPreflightReport Run(
      IDataVaultDiagnosticsService diagnostics,
      DataVaultPreflightRequest request) {
    ArgumentNullException.ThrowIfNull(diagnostics);
    ArgumentNullException.ThrowIfNull(request);

    var validationResult = diagnostics.Analyze(request.DbContext);
    var validationProvider = CreateDiagnosticsSection("validation-provider", validationResult);
    var artifactDrift = CreateArtifactDriftSection(request);
    var snapshotDrift = CreateSnapshotDriftSection(request);
    var idempotencySchema = CreateIdempotencySchemaSection(request);
    var migrationGuardrail = CreateMigrationGuardrailSection(validationResult, request.MigrationOperations);
    var requestDiagnostics = CreateRequestDiagnosticsSection(request);

    return new DataVaultPreflightReport(
        validationProvider,
        artifactDrift,
        snapshotDrift,
        idempotencySchema,
        migrationGuardrail,
        requestDiagnostics);
  }

  private static DataVaultPreflightSection<DataVaultDiagnosticsResult> CreateDiagnosticsSection(
      string name,
      DataVaultDiagnosticsResult diagnostics) {
    return new DataVaultPreflightSection<DataVaultDiagnosticsResult>(
        name,
        diagnostics.Validation.IsValid
            ? DataVaultPreflightSectionStatus.Passed
            : DataVaultPreflightSectionStatus.Blocked,
        diagnostics);
  }

  private static DataVaultPreflightSection<DataVaultModelDriftReport> CreateArtifactDriftSection(
      DataVaultPreflightRequest request) {
    var artifactImport = request.ReviewedArtifactImport ?? request.ExpectedImport;
    if (artifactImport is null) {
      return new DataVaultPreflightSection<DataVaultModelDriftReport>(
          "artifact-drift",
          DataVaultPreflightSectionStatus.Skipped,
          report: null,
          ArtifactDriftNotProvided);
    }

    artifactImport.ThrowIfInvalid();
    var report = DataVaultModelDriftReporter.Compare(artifactImport, request.DbContext);
    return new DataVaultPreflightSection<DataVaultModelDriftReport>(
        "artifact-drift",
        report.HasBlockingDifferences
            ? DataVaultPreflightSectionStatus.Blocked
            : DataVaultPreflightSectionStatus.Passed,
        report);
  }

  private static DataVaultPreflightSection<DataVaultIdempotencyPreflightReport> CreateIdempotencySchemaSection(
      DataVaultPreflightRequest request) {
    if (request.IdempotencyLiveSchemaReadResult is null) {
      return new DataVaultPreflightSection<DataVaultIdempotencyPreflightReport>(
          "idempotency-schema",
          DataVaultPreflightSectionStatus.Skipped,
          report: null,
          IdempotencyLiveSchemaNotProvided);
    }

    var providerCapabilities = DataVaultProviderCapabilityProfileSelection.Select(TryGetProviderName(request.DbContext));
    var report = request.ExpectedImport is null
        ? DataVaultIdempotencyPreflight.Compare(
            request.ExpectedMetadataModel!,
            request.IdempotencyLiveSchemaReadResult,
            providerCapabilities)
        : DataVaultIdempotencyPreflight.Compare(
            request.ExpectedImport,
            request.IdempotencyLiveSchemaReadResult,
            providerCapabilities);

    return new DataVaultPreflightSection<DataVaultIdempotencyPreflightReport>(
        "idempotency-schema",
        report.IsBlocked
            ? DataVaultPreflightSectionStatus.Blocked
            : DataVaultPreflightSectionStatus.Passed,
        report);
  }

  private static DataVaultPreflightSection<DataVaultModelDriftPreflightReport> CreateSnapshotDriftSection(
      DataVaultPreflightRequest request) {
    if (request.SnapshotModel is null) {
      return new DataVaultPreflightSection<DataVaultModelDriftPreflightReport>(
          "snapshot-drift",
          DataVaultPreflightSectionStatus.Skipped,
          report: null,
          SnapshotModelNotProvided);
    }

    var report = request.ExpectedImport is null
        ? DataVaultModelDriftPreflightReporter.Compare(
            request.ExpectedMetadataModel!,
            request.DbContext,
            request.SnapshotModel)
        : DataVaultModelDriftPreflightReporter.Compare(
            request.ExpectedImport,
            request.DbContext,
            request.SnapshotModel);

    return new DataVaultPreflightSection<DataVaultModelDriftPreflightReport>(
        "snapshot-drift",
        report.HasBlockingDifferences
            ? DataVaultPreflightSectionStatus.Blocked
            : DataVaultPreflightSectionStatus.Passed,
        report);
  }

  private static DataVaultPreflightSection<DataVaultMigrationGuardrailReport> CreateMigrationGuardrailSection(
      DataVaultDiagnosticsResult validationResult,
      IReadOnlyList<MigrationOperation>? operations) {
    if (operations is null) {
      return new DataVaultPreflightSection<DataVaultMigrationGuardrailReport>(
          "migration-guardrail",
          DataVaultPreflightSectionStatus.Skipped,
          report: null,
          MigrationOperationsNotProvided);
    }

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(validationResult, operations);
    return new DataVaultPreflightSection<DataVaultMigrationGuardrailReport>(
        "migration-guardrail",
        report.IsValid
            ? DataVaultPreflightSectionStatus.Passed
            : DataVaultPreflightSectionStatus.Blocked,
        report);
  }

  private static DataVaultPreflightSection<DataVaultPreflightRequestDiagnosticsReport> CreateRequestDiagnosticsSection(
      DataVaultPreflightRequest request) {
    var results = new List<DataVaultPreflightRepresentativeDiagnostics>();

    AddPrecomputedRequestDiagnostics(results, request.RepresentativeDiagnostics);
    AddFactoryRequestDiagnostics(results, request);

    if (results.Count == 0) {
      return new DataVaultPreflightSection<DataVaultPreflightRequestDiagnosticsReport>(
          "request-diagnostics",
          DataVaultPreflightSectionStatus.Skipped,
          report: null,
          RequestDiagnosticsNotProvided);
    }

    var report = new DataVaultPreflightRequestDiagnosticsReport(results);
    return new DataVaultPreflightSection<DataVaultPreflightRequestDiagnosticsReport>(
        "request-diagnostics",
        report.HasBlockingDiagnostics
            ? DataVaultPreflightSectionStatus.Blocked
            : DataVaultPreflightSectionStatus.Passed,
        report);
  }

  private static void AddPrecomputedRequestDiagnostics(
      ICollection<DataVaultPreflightRepresentativeDiagnostics> results,
      IEnumerable<DataVaultPreflightRepresentativeDiagnostics>? diagnostics) {
    if (diagnostics is null) {
      return;
    }

    foreach (var result in diagnostics) {
      ArgumentNullException.ThrowIfNull(result);
      results.Add(result);
    }
  }

  private static void AddFactoryRequestDiagnostics(
      ICollection<DataVaultPreflightRepresentativeDiagnostics> results,
      DataVaultPreflightRequest request) {
    if (request.RepresentativeDiagnosticsRequests is null) {
      return;
    }

    foreach (var diagnosticsRequest in request.RepresentativeDiagnosticsRequests) {
      ArgumentNullException.ThrowIfNull(diagnosticsRequest);
      var diagnostics = diagnosticsRequest.CreateDiagnostics(request.DbContext);
      results.Add(new DataVaultPreflightRepresentativeDiagnostics(diagnosticsRequest.Name, diagnostics));
    }
  }

  private static string? TryGetProviderName(DbContext dbContext) {
    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
  }
}
