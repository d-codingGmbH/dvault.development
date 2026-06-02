using Microsoft.CodeAnalysis;

namespace DCoding.Data.DVault.Analyzers;

internal static class DataVaultTypedReadModelDiagnosticCatalog {
  public const string Category = "SourceGeneration";

  public static readonly DiagnosticDescriptor MetadataSourceUnavailable = new(
      "DMV1960",
      "Typed read-model metadata source unavailable",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault read-model generation requires exactly one deterministic authoritative metadata source.");

  public static readonly DiagnosticDescriptor MetadataSourceFingerprintDrift = new(
      "DMV1961",
      "Typed read-model metadata source fingerprint drift",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault read-model generation stops when the configured metadata source fingerprint differs from the resolved source fingerprint.");

  public static readonly DiagnosticDescriptor UnsupportedSatelliteShape = new(
      "DMV1962",
      "Unsupported typed satellite read-model shape",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault satellite read-model generation supports only deterministic hub-parent or link-parent satellite shapes with string driving keys and payload values.");

  public static readonly DiagnosticDescriptor UnsupportedPitShape = new(
      "DMV1963",
      "Unsupported typed PIT read-model shape",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault PIT read-model generation supports only the bounded v1 PIT baseline.");

  public static readonly DiagnosticDescriptor UnsupportedBridgeShape = new(
      "DMV1964",
      "Unsupported typed bridge read-model shape",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault bridge read-model generation supports only the bounded v1 bridge baseline.");

  public static readonly DiagnosticDescriptor NameCollision = new(
      "DMV1965",
      "Typed read-model generated name collision",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault read-model generation stops when deterministic generated type, method, or property names collide.");

  public static readonly DiagnosticDescriptor PayloadNullabilityFallback = new(
      "DMV1966",
      "Typed satellite payload nullability fallback",
      "{0}",
      Category,
      DiagnosticSeverity.Warning,
      isEnabledByDefault: true,
      description: "Typed DVault satellite read-model generation emits nullable payload properties when authoritative CLR or EF nullability cannot be proven.");

  public static readonly DiagnosticDescriptor DynamicQueryShapeRequired = new(
      "DMV1967",
      "Typed read-model shape requires dynamic query behavior",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault read-model generation does not emit helpers for shapes that require dynamic runtime query construction, provider SQL, runtime projection selection, or unbounded traversal.");

  public static readonly DiagnosticDescriptor UnsupportedModelFirstShape = new(
      "DMV1968",
      "Unsupported model-first typed read-model shape",
      "{0}",
      Category,
      DiagnosticSeverity.Error,
      isEnabledByDefault: true,
      description: "Typed DVault read-model generation rejects raw or residual model-first inputs outside the projected support-bundle helper contract.");

  public static readonly DiagnosticDescriptor HelperSkipped = new(
      "DMV1969",
      "Typed read-model helper skipped",
      "{0}",
      Category,
      DiagnosticSeverity.Info,
      isEnabledByDefault: true,
      description: "Typed DVault read-model generation may skip valid runtime metadata shapes that are outside the v1 generated helper contract.");
}
