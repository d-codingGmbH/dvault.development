using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Structured Data Vault migration guardrail report suitable for local scripts, tests, and build steps.
/// </summary>
public sealed record DataVaultMigrationGuardrailReport {
  internal DataVaultMigrationGuardrailReport(
      DataVaultDiagnosticsResult diagnostics,
      IReadOnlyList<DataVaultMigrationGuardrailIssue> issues,
      IReadOnlyList<DataVaultMigrationGuardrailOperationSummary> operationSummaries) {
    ArgumentNullException.ThrowIfNull(diagnostics);
    ArgumentNullException.ThrowIfNull(issues);
    ArgumentNullException.ThrowIfNull(operationSummaries);

    Diagnostics = diagnostics;
    Issues = issues.ToArray();
    OperationSummaries = operationSummaries.ToArray();
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
  /// Gets the ordered safe, risky, or incompatible outcome for each inspected migration operation.
  /// </summary>
  public IReadOnlyList<DataVaultMigrationGuardrailOperationSummary> OperationSummaries { get; }

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
    builder.Append(", operations ");
    builder.Append(OperationSummaries.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(", provider ");
    builder.Append(FormatProviderName(Diagnostics.Explain.ProviderName));
    builder.Append(", capability ");
    builder.Append(Diagnostics.Explain.CapabilityProfileName);
    if (Diagnostics.Explain.CapabilityProfileDefaulted) {
      builder.Append(" (defaulted)");
    }

    builder.Append(", provider behavior ");
    builder.Append(Diagnostics.Explain.ProviderBehaviorProfileName);
    if (Diagnostics.Explain.ProviderBehaviorDefaulted) {
      builder.Append(" (defaulted)");
    }

    foreach (var summary in OperationSummaries) {
      builder.AppendLine();
      builder.Append("- ");
      builder.Append(FormatOutcome(summary.Outcome));
      builder.Append(' ');
      builder.Append(summary.Path);
      builder.Append(": ");
      if (summary.Issues.Count == 0) {
        builder.Append("no DVM findings");
        continue;
      }

      builder.Append("findings ");
      builder.Append(summary.Issues.Count.ToString(CultureInfo.InvariantCulture));

      foreach (var issue in summary.Issues) {
        builder.AppendLine();
        builder.Append("  - ");
        AppendIssue(builder, issue);
      }
    }

    if (OperationSummaries.Count == 0) {
      foreach (var issue in Issues) {
        builder.AppendLine();
        builder.Append("- ");
        AppendIssue(builder, issue);
      }
    }

    return builder.ToString();
  }

  internal static DataVaultMigrationGuardrailReport Create(
      DataVaultDiagnosticsResult diagnostics,
      IReadOnlyList<DataVaultMigrationGuardrailOperationSummary>? operationSummaries = null) {
    ArgumentNullException.ThrowIfNull(diagnostics);

    var issues = diagnostics.Issues
        .Where(issue => issue.Code.StartsWith("DVM", StringComparison.Ordinal))
        .Select(DataVaultMigrationGuardrailIssue.Create)
        .ToArray();

    return new DataVaultMigrationGuardrailReport(
        diagnostics,
        issues,
        operationSummaries ?? Array.Empty<DataVaultMigrationGuardrailOperationSummary>());
  }

  private static void AppendIssue(
      StringBuilder builder,
      DataVaultMigrationGuardrailIssue issue) {
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

  private static string FormatProviderName(string? providerName) {
    return string.IsNullOrWhiteSpace(providerName)
        ? "<none>"
        : providerName;
  }

  private static string FormatOutcome(DataVaultMigrationGuardrailOperationOutcome outcome) {
    return outcome.ToString().ToLowerInvariant();
  }
}
