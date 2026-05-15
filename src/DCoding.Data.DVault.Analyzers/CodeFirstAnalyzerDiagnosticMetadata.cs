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
    ArgumentException.ThrowIfNullOrWhiteSpace(id);
    ArgumentException.ThrowIfNullOrWhiteSpace(category);
    ArgumentException.ThrowIfNullOrWhiteSpace(title);
    ArgumentException.ThrowIfNullOrWhiteSpace(message);
    ArgumentException.ThrowIfNullOrWhiteSpace(explanation);
    ArgumentException.ThrowIfNullOrWhiteSpace(remediation);

    Id = id;
    Category = category;
    Title = title;
    Message = message;
    Explanation = explanation;
    Remediation = remediation;
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
}
