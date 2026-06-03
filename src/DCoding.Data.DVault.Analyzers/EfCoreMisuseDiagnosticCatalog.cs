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

  public static readonly CodeFirstAnalyzerDiagnosticMetadata MissingModelCacheDiscriminatorMetadata = new(
      "DMV1912",
      Category,
      "Missing DVault model-cache discriminator",
      "DbContext '{0}' varies its DVault EF model shape from '{1}' without a visible model-cache key discriminator.",
      "Raised when source-visible DVault model-shape variation depends on DbContext instance state and the visible EF model-cache-key path does not include that varying state.",
      "Replace IModelCacheKeyFactory for the context and include every caller-owned tenant, schema, naming, provider, or profile discriminator that can change the realized DVault EF model shape.");

  public static readonly CodeFirstAnalyzerDiagnosticMetadata UnsafeCompiledModelSelectionMetadata = new(
      "DMV1913",
      Category,
      "Unsafe DVault compiled-model selection",
      "UseModel(...) applies a compiled EF model to DVault context '{0}' whose visible model shape varies from '{1}'.",
      "Raised when source-visible UseModel(...) selects a compiled or runtime EF model for a DVault context whose realized model shape is visibly variable.",
      "Use compiled models only for one fixed realized DVault model shape or for the documented design-model-to-runtime-model lane where the selected metadata and model shape are fixed.");

  public static readonly CodeFirstAnalyzerDiagnosticMetadata UnsafeDbContextPoolingMetadata = new(
      "DMV1914",
      Category,
      "Unsafe DVault DbContext pooling",
      "AddDbContextPool<{0}>(...) pools a DVault context whose visible model shape varies from '{1}'.",
      "Raised when source-visible AddDbContextPool<TContext>(...) targets a DVault context whose realized model shape visibly varies beyond one fixed options-only shape.",
      "Use DbContext pooling only for options-only DVault contexts with one fixed metadata source, provider configuration, naming, schema, and profile.");

  public static readonly DiagnosticDescriptor GeneratedDbSetExposure = GeneratedDbSetExposureMetadata.CreateDescriptor();

  public static readonly DiagnosticDescriptor DirectGeneratedTableWrite = DirectGeneratedTableWriteMetadata.CreateDescriptor();

  public static readonly DiagnosticDescriptor MissingModelCacheDiscriminator = MissingModelCacheDiscriminatorMetadata.CreateDescriptor();

  public static readonly DiagnosticDescriptor UnsafeCompiledModelSelection = UnsafeCompiledModelSelectionMetadata.CreateDescriptor();

  public static readonly DiagnosticDescriptor UnsafeDbContextPooling = UnsafeDbContextPoolingMetadata.CreateDescriptor();
}
