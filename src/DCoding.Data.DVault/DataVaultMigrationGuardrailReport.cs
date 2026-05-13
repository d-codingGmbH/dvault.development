using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Structured Data Vault migration guardrail report suitable for local scripts, tests, and build steps.
/// </summary>
public sealed record DataVaultMigrationGuardrailReport {
  internal DataVaultMigrationGuardrailReport(
      DataVaultDiagnosticsResult diagnostics,
      IReadOnlyList<DataVaultMigrationGuardrailIssue> issues) {
    ArgumentNullException.ThrowIfNull(diagnostics);
    ArgumentNullException.ThrowIfNull(issues);

    Diagnostics = diagnostics;
    Issues = issues.ToArray();
  }

  /// <summary>
  /// Gets the underlying Data Vault diagnostics result produced after migration operation analysis.
  /// </summary>
  public DataVaultDiagnosticsResult Diagnostics { get; }

  /// <summary>
  /// Gets the migration-operation findings with remediation guidance from the central DVM catalog.
  /// </summary>
  public IReadOnlyList<DataVaultMigrationGuardrailIssue> Issues { get; }

  /// <summary>
  /// Gets a value indicating whether the underlying diagnostics result contains no error-severity validation issues.
  /// </summary>
  public bool IsValid => Diagnostics.Validation.IsValid;

  /// <summary>
  /// Gets a value indicating whether any migration-operation guardrail findings were reported.
  /// </summary>
  public bool HasFindings => Issues.Count > 0;

  /// <summary>
  /// Produces deterministic human-readable migration guardrail output for console, test, or build logs.
  /// </summary>
  /// <returns>A concise deterministic display string.</returns>
  public string ToDisplayString() {
    var builder = new StringBuilder();
    builder.Append("DVault migration guardrails: ");
    builder.Append(IsValid ? "valid" : "invalid");
    builder.Append(", findings ");
    builder.Append(Issues.Count.ToString(CultureInfo.InvariantCulture));

    foreach (var issue in Issues) {
      builder.AppendLine();
      builder.Append("- ");
      builder.Append(issue.Severity);
      builder.Append(' ');
      builder.Append(issue.Code);
      builder.Append(' ');
      builder.Append(issue.Path);
      builder.Append(": ");
      builder.Append(issue.Message);
      builder.Append(" Remediation: ");
      builder.Append(issue.Remediation);
    }

    return builder.ToString();
  }

  internal static DataVaultMigrationGuardrailReport Create(DataVaultDiagnosticsResult diagnostics) {
    ArgumentNullException.ThrowIfNull(diagnostics);

    var issues = diagnostics.Issues
        .Where(issue => issue.Code.StartsWith("DVM", StringComparison.Ordinal))
        .Select(CreateIssue)
        .ToArray();

    return new DataVaultMigrationGuardrailReport(diagnostics, issues);
  }

  private static DataVaultMigrationGuardrailIssue CreateIssue(DataVaultDiagnosticsIssue issue) {
    var definition = DataVaultDiagnosticCatalog.GetMigrationOperationDefinition(issue.Code);
    return new DataVaultMigrationGuardrailIssue(
        issue.Severity,
        issue.Code,
        issue.Path ?? string.Empty,
        issue.Message,
        definition.Remediation);
  }
}
