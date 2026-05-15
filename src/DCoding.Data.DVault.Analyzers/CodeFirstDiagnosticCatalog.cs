using Microsoft.CodeAnalysis;

namespace DCoding.Data.DVault.Analyzers;

internal static class CodeFirstDiagnosticCatalog {
  public const string Category = "CodeFirst";

  public static readonly CodeFirstAnalyzerDiagnosticMetadata UnsupportedSelectorMetadata = new(
      "DMV1901",
      Category,
      "Unsupported Code-First selector shape",
      "{0} selector must target one direct readable scalar member on the configured entity type.",
      "Raised when BusinessKey(...), Payload(...), or DrivingKey(...) uses an unsupported selector shape.",
      "Use repeated direct readable scalar member selectors such as BusinessKey(x => x.Member), Payload(x => x.Member), or DrivingKey(x => x.Member).");

  public static readonly CodeFirstAnalyzerDiagnosticMetadata DuplicateMemberMetadata = new(
      "DMV1902",
      Category,
      "Duplicate Code-First member declaration",
      "{0} member '{1}' is declared more than once in the same Code-First scope.",
      "Raised when one BusinessKey(...), Payload(...), or DrivingKey(...) fluent scope repeats a logical member name.",
      "Declare each logical member name at most once per relevant BusinessKey(...), Payload(...), or DrivingKey(...) scope.");

  public static readonly DiagnosticDescriptor UnsupportedSelector = UnsupportedSelectorMetadata.CreateDescriptor();

  public static readonly DiagnosticDescriptor DuplicateMember = DuplicateMemberMetadata.CreateDescriptor();
}
