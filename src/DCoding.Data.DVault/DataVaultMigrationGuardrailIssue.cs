namespace DCoding.Data.DVault;

/// <summary>
/// Automation-friendly migration guardrail issue with central remediation guidance.
/// </summary>
/// <param name="Severity">The deterministic guardrail severity.</param>
/// <param name="Code">The stable DVM diagnostic code.</param>
/// <param name="Path">The deterministic migration operation path.</param>
/// <param name="Message">The human-readable guardrail message.</param>
/// <param name="Remediation">The central remediation guidance for the diagnostic code.</param>
public sealed record DataVaultMigrationGuardrailIssue(
    DataVaultDiagnosticsIssueSeverity Severity,
    string Code,
    string Path,
    string Message,
    string Remediation);
