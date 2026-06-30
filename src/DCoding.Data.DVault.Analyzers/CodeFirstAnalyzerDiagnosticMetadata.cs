using Microsoft.CodeAnalysis;

namespace DCoding.Data.DVault.Analyzers;

internal sealed class CodeFirstAnalyzerDiagnosticMetadata {
  public CodeFirstAnalyzerDiagnosticMetadata(
      string id,
      string category,
      string title,
      string message,
      string explanation,
      string remediation) {
    Id = RequireNonWhiteSpace(id, nameof(id));
    Category = RequireNonWhiteSpace(category, nameof(category));
    Title = RequireNonWhiteSpace(title, nameof(title));
    Message = RequireNonWhiteSpace(message, nameof(message));
    Explanation = RequireNonWhiteSpace(explanation, nameof(explanation));
    Remediation = RequireNonWhiteSpace(remediation, nameof(remediation));
  }

  public string Id { get; }

  public string Category { get; }

  public string Title { get; }

  public string Message { get; }

  public string Explanation { get; }

  public string Remediation { get; }

  public DiagnosticDescriptor CreateDescriptor() {
    return new DiagnosticDescriptor(
        Id,
        Title,
        Message,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Explanation + " Remediation: " + Remediation);
  }

  private static string RequireNonWhiteSpace(string value, string parameterName) {
    if (string.IsNullOrWhiteSpace(value)) {
      throw new ArgumentException("Value cannot be null or whitespace.", parameterName);
    }

    return value;
  }
}
