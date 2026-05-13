namespace DCoding.Data.DVault;

internal sealed class DataVaultDiagnosticDefinition {
  internal DataVaultDiagnosticDefinition(
      string code,
      string severity,
      string category,
      string summary,
      string explanation,
      string remediation) {
    ArgumentException.ThrowIfNullOrWhiteSpace(code);
    ArgumentException.ThrowIfNullOrWhiteSpace(severity);
    ArgumentException.ThrowIfNullOrWhiteSpace(category);
    ArgumentException.ThrowIfNullOrWhiteSpace(summary);
    ArgumentException.ThrowIfNullOrWhiteSpace(explanation);
    ArgumentException.ThrowIfNullOrWhiteSpace(remediation);

    Code = code;
    Severity = severity;
    Category = category;
    Summary = summary;
    Explanation = explanation;
    Remediation = remediation;
  }

  internal string Code { get; }

  internal string Severity { get; }

  internal string Category { get; }

  internal string Summary { get; }

  internal string Explanation { get; }

  internal string Remediation { get; }
}
