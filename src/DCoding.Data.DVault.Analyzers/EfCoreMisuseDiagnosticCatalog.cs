using Microsoft.CodeAnalysis;

namespace DCoding.Data.DVault.Analyzers;

internal static class EfCoreMisuseDiagnosticCatalog {
  public const string Category = "EfCore";

  public static readonly CodeFirstAnalyzerDiagnosticMetadata GeneratedDbSetExposureMetadata = new(
      "DMV1910",
      Category,
      "Unsupported generated DVault DbSet exposure",
      "DbContext member '{0}' exposes DVault generated shared-type table '{1}' as DbSet<Dictionary<string, object>>.",
      "Raised when a DbContext exposes source-visible DVault generated shared-type tables through DbSet<Dictionary<string, object>> members.",
      "Keep generated DVault tables off the DbContext public surface and use context.Set<Dictionary<string, object>>(producedName) only for documented read-only query shapes.");

  public static readonly CodeFirstAnalyzerDiagnosticMetadata DirectGeneratedTableWriteMetadata = new(
      "DMV1911",
      Category,
      "Unsafe direct generated DVault table write",
      "{0}(...) writes directly to DVault generated shared-type table '{1}'.",
      "Raised when source directly mutates a DbSet<Dictionary<string, object>> created from a source-visible DVault produced table name instead of using the explicit DVault save boundary.",
      "Use IDataVaultSaveService for ordinary DVault hub, link, and satellite writes; reserve context.Set<Dictionary<string, object>>(producedName) for read-only queries such as AsNoTracking() or compiled-query projections unless the source visibly opts into UseDataVaultSaveChangesMetadataInterceptor(...).");

  public static readonly DiagnosticDescriptor GeneratedDbSetExposure = GeneratedDbSetExposureMetadata.CreateDescriptor();

  public static readonly DiagnosticDescriptor DirectGeneratedTableWrite = DirectGeneratedTableWriteMetadata.CreateDescriptor();
}
