using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Structured result for one hash-key storage migration manifest validation pass.
/// </summary>
public sealed class DataVaultHashKeyStorageMigrationValidationResult {
  /// <summary>
  /// Initializes a new hash-key storage migration manifest validation result.
  /// </summary>
  public DataVaultHashKeyStorageMigrationValidationResult(
      IEnumerable<DataVaultHashKeyStorageMigrationValidationFinding> findings) {
    ArgumentNullException.ThrowIfNull(findings);

    Findings = findings
        .Select(finding => finding ?? throw new ArgumentException("Validation findings must not contain null values.", nameof(findings)))
        .ToArray();
  }

  /// <summary>
  /// Gets deterministic redacted validation findings ordered by severity, code, table, column, and JSON path.
  /// </summary>
  public IReadOnlyList<DataVaultHashKeyStorageMigrationValidationFinding> Findings { get; }

  /// <summary>
  /// Gets a value indicating whether the manifest has no blocking error findings.
  /// </summary>
  public bool IsValid => Findings.All(finding => finding.Severity != DataVaultDiagnosticsIssueSeverity.Error);

  /// <summary>
  /// Produces deterministic human-readable validation output for console, test, or build logs.
  /// </summary>
  public string ToDisplayString() {
    var errorCount = Findings.Count(finding => finding.Severity == DataVaultDiagnosticsIssueSeverity.Error);
    var warningCount = Findings.Count(finding => finding.Severity == DataVaultDiagnosticsIssueSeverity.Warning);
    var infoCount = Findings.Count(finding => finding.Severity == DataVaultDiagnosticsIssueSeverity.Info);

    var builder = new StringBuilder();
    builder.Append("DVault hash-key storage migration manifest: ");
    builder.Append(IsValid ? "valid" : "invalid");
    builder.Append(", errors ");
    builder.Append(errorCount.ToString(CultureInfo.InvariantCulture));
    builder.Append(", warnings ");
    builder.Append(warningCount.ToString(CultureInfo.InvariantCulture));
    builder.Append(", info ");
    builder.Append(infoCount.ToString(CultureInfo.InvariantCulture));
    builder.Append('.');

    foreach (var finding in Findings) {
      builder.AppendLine();
      builder.Append("- ");
      builder.Append(finding.Severity);
      builder.Append(' ');
      builder.Append(finding.Code);
      builder.Append(' ');
      builder.Append(string.IsNullOrWhiteSpace(finding.TableName) ? "<manifest>" : finding.TableName);
      builder.Append('/');
      builder.Append(string.IsNullOrWhiteSpace(finding.ColumnName) ? "<none>" : finding.ColumnName);
      builder.Append(" [");
      builder.Append(finding.Path);
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
}
