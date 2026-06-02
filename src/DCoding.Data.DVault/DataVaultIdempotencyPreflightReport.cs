using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Structured and displayable result for one explicit Data Vault idempotency schema preflight check.
/// </summary>
public sealed class DataVaultIdempotencyPreflightReport {
  /// <summary>
  /// Initializes a new idempotency preflight report.
  /// </summary>
  public DataVaultIdempotencyPreflightReport(
      DataVaultIdempotencyPreflightStatus status,
      string? providerName,
      string capabilityProfileName,
      IEnumerable<DataVaultIdempotencyPreflightStructure> expectedStructures,
      IEnumerable<DataVaultIdempotencyPreflightFinding> findings,
      string? message = null) {
    if (!Enum.IsDefined(status)) {
      throw new ArgumentOutOfRangeException(nameof(status));
    }

    ArgumentException.ThrowIfNullOrWhiteSpace(capabilityProfileName);
    ArgumentNullException.ThrowIfNull(expectedStructures);
    ArgumentNullException.ThrowIfNull(findings);

    Status = status;
    ProviderName = providerName;
    CapabilityProfileName = capabilityProfileName;
    ExpectedStructures = expectedStructures
        .Select(structure => structure ?? throw new ArgumentException("Expected structures must not contain null values.", nameof(expectedStructures)))
        .ToArray();
    Findings = findings
        .Select(finding => finding ?? throw new ArgumentException("Idempotency preflight findings must not contain null values.", nameof(findings)))
        .ToArray();
    Message = message;
  }

  /// <summary>
  /// Gets the classified idempotency preflight status.
  /// </summary>
  public DataVaultIdempotencyPreflightStatus Status { get; }

  /// <summary>
  /// Gets the Entity Framework provider name, when available.
  /// </summary>
  public string? ProviderName { get; }

  /// <summary>
  /// Gets the provider capability profile used to project expected idempotency structures.
  /// </summary>
  public string CapabilityProfileName { get; }

  /// <summary>
  /// Gets the provider-shaped idempotency-critical structure baseline that was evaluated.
  /// </summary>
  public IReadOnlyList<DataVaultIdempotencyPreflightStructure> ExpectedStructures { get; }

  /// <summary>
  /// Gets deterministic redacted findings for missing or mismatched idempotency-critical structures.
  /// </summary>
  public IReadOnlyList<DataVaultIdempotencyPreflightFinding> Findings { get; }

  /// <summary>
  /// Gets a bounded status message for skipped, unsupported-provider, or unavailable-live-schema outcomes.
  /// </summary>
  public string? Message { get; }

  /// <summary>
  /// Gets a value indicating whether the report contains a blocking outcome.
  /// </summary>
  public bool IsBlocked => Status is
      DataVaultIdempotencyPreflightStatus.Blocked or
      DataVaultIdempotencyPreflightStatus.UnsupportedProvider or
      DataVaultIdempotencyPreflightStatus.UnavailableLiveSchema;

  /// <summary>
  /// Creates a skipped idempotency preflight report for callers that model omitted live input as a report.
  /// </summary>
  public static DataVaultIdempotencyPreflightReport Skipped(string reason) {
    ArgumentException.ThrowIfNullOrWhiteSpace(reason);

    return new DataVaultIdempotencyPreflightReport(
        DataVaultIdempotencyPreflightStatus.Skipped,
        providerName: null,
        capabilityProfileName: "<not-evaluated>",
        expectedStructures: Array.Empty<DataVaultIdempotencyPreflightStructure>(),
        findings: Array.Empty<DataVaultIdempotencyPreflightFinding>(),
        reason);
  }

  /// <summary>
  /// Produces deterministic human-readable idempotency preflight output for console, test, or build logs.
  /// </summary>
  public string ToDisplayString() {
    var builder = new StringBuilder();
    builder.Append("DVault idempotency preflight: ");
    builder.Append(FormatStatus(Status));
    builder.Append(", structures ");
    builder.Append(ExpectedStructures.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(", findings ");
    builder.Append(Findings.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(", provider ");
    builder.Append(string.IsNullOrWhiteSpace(ProviderName) ? "<none>" : ProviderName);
    builder.Append('/');
    builder.Append(CapabilityProfileName);
    builder.Append('.');

    if (!string.IsNullOrWhiteSpace(Message)) {
      builder.Append(' ');
      builder.Append(Message);
    }

    foreach (var finding in Findings) {
      builder.AppendLine();
      builder.Append("- ");
      builder.Append(finding.Severity);
      builder.Append(' ');
      builder.Append(finding.Code);
      builder.Append(' ');
      builder.Append(finding.OperationFamily);
      builder.Append(' ');
      builder.Append(finding.TableName);
      builder.Append(" (");
      builder.Append(finding.StructureName);
      builder.Append(") [");
      builder.Append(finding.PropertyPath);
      builder.Append("]: ");
      builder.Append(finding.Message);
      if (finding.ExpectedValue is not null || finding.ActualValue is not null) {
        builder.Append(" Expected=");
        builder.Append(finding.ExpectedValue ?? "<null>");
        builder.Append("; Actual=");
        builder.Append(finding.ActualValue ?? "<null>");
        builder.Append('.');
      }
    }

    return builder.ToString();
  }

  private static string FormatStatus(DataVaultIdempotencyPreflightStatus status) {
    return status switch {
      DataVaultIdempotencyPreflightStatus.UnavailableLiveSchema => "unavailable-live-schema",
      DataVaultIdempotencyPreflightStatus.UnsupportedProvider => "unsupported-provider",
      _ => status.ToString().ToLowerInvariant(),
    };
  }
}
