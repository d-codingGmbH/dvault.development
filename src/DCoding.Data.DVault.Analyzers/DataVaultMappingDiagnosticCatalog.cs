using Microsoft.CodeAnalysis;

namespace DCoding.Data.DVault.Analyzers;

internal static class DataVaultMappingDiagnosticCatalog {
  public const string Category = "SourceGeneration";

  public static readonly DiagnosticDescriptor AmbiguousMappingDeclaration = new(
      "DMV1950",
      "Ambiguous generated mapping declaration",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "A source type that participates in DVault generated mappings must declare exactly one supported mapping target.");

  public static readonly DiagnosticDescriptor MissingRequiredBinding = new(
      "DMV1951",
      "Missing generated mapping binding",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Generated DVault mappings require every runtime value needed by the registry-backed save operation to be explicitly bound.");

  public static readonly DiagnosticDescriptor InvalidBinding = new(
      "DMV1952",
      "Invalid generated mapping binding",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Generated DVault mappings can bind only non-static accessible string properties or fields on the mapped source type.");

  public static readonly DiagnosticDescriptor DuplicateBindingOrder = new(
      "DMV1953",
      "Duplicate generated mapping binding order",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Generated DVault mapping binding order values must be unique inside one binding family.");

  public static readonly DiagnosticDescriptor DuplicateBindingName = new(
      "DMV1954",
      "Duplicate generated mapping binding name",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Generated DVault mapping logical names must be unique inside one binding family.");

  public static readonly DiagnosticDescriptor RepeatedLinkParticipant = new(
      "DMV1955",
      "Duplicate generated link participant name",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Generated link mappings require produced participant names that are unique by StringComparer.Ordinal.");
}
