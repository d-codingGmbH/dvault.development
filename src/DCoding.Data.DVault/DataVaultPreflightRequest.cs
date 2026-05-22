using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DCoding.Data.DVault;

/// <summary>
/// Explicit caller-owned options for one Data Vault aggregate preflight evaluation.
/// </summary>
public sealed class DataVaultPreflightRequest {
  /// <summary>
  /// Initializes a new preflight request with a provider-neutral metadata model as the authoritative expectation.
  /// </summary>
  /// <param name="dbContext">The configured DbContext to validate and compare.</param>
  /// <param name="expectedMetadataModel">The authoritative provider-neutral metadata model.</param>
  public DataVaultPreflightRequest(
      DbContext dbContext,
      DataVaultMetadataModel expectedMetadataModel) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(expectedMetadataModel);

    DbContext = dbContext;
    ExpectedMetadataModel = expectedMetadataModel;
  }

  /// <summary>
  /// Initializes a new preflight request with a successful model artifact import result as the authoritative expectation.
  /// </summary>
  /// <param name="dbContext">The configured DbContext to validate and compare.</param>
  /// <param name="expectedImport">The authoritative successful dvault.model.v1 import result.</param>
  public DataVaultPreflightRequest(
      DbContext dbContext,
      DataVaultModelImportResult expectedImport) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(expectedImport);

    expectedImport.ThrowIfInvalid();

    DbContext = dbContext;
    ExpectedImport = expectedImport;
  }

  /// <summary>
  /// Gets the configured DbContext to validate and compare.
  /// </summary>
  public DbContext DbContext { get; }

  /// <summary>
  /// Gets the authoritative provider-neutral metadata model when this request was created from metadata.
  /// </summary>
  public DataVaultMetadataModel? ExpectedMetadataModel { get; }

  /// <summary>
  /// Gets the authoritative successful model artifact import result when this request was created from an import.
  /// </summary>
  public DataVaultModelImportResult? ExpectedImport { get; }

  /// <summary>
  /// Gets the optional successful reviewed model artifact import result used for artifact-versus-design-time drift.
  /// </summary>
  public DataVaultModelImportResult? ReviewedArtifactImport { get; init; }

  /// <summary>
  /// Gets the optional explicit consumer-materialized snapshot model used for snapshot preflight drift.
  /// </summary>
  public IReadOnlyModel? SnapshotModel { get; init; }

  /// <summary>
  /// Gets optional explicit migration operations used for migration guardrail diagnostics.
  /// A null value skips the migration lane; an empty list evaluates the lane with no operations.
  /// </summary>
  public IReadOnlyList<MigrationOperation>? MigrationOperations { get; init; }

  /// <summary>
  /// Gets optional precomputed representative diagnostics results to preserve in the aggregate report.
  /// </summary>
  public IReadOnlyList<DataVaultPreflightRepresentativeDiagnostics> RepresentativeDiagnostics { get; init; } =
      Array.Empty<DataVaultPreflightRepresentativeDiagnostics>();

  /// <summary>
  /// Gets optional caller-owned representative diagnostics requests to evaluate against the configured DbContext.
  /// </summary>
  public IReadOnlyList<DataVaultPreflightRepresentativeDiagnosticsRequest> RepresentativeDiagnosticsRequests { get; init; } =
      Array.Empty<DataVaultPreflightRepresentativeDiagnosticsRequest>();
}
