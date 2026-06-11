using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides Entity Framework Core model configuration extensions for DVault conventions.
/// </summary>
public static class DataVaultModelBuilderExtensions {
  private static readonly DataVaultProviderCapabilityProfile DefaultProviderCapabilities = DataVaultProviderCapabilityProfiles.Sqlite;

  /// <summary>
  /// Records the provider-neutral DVault default conventions on the Entity Framework model.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder UseDataVault(this ModelBuilder modelBuilder) {
    return modelBuilder.UseDataVault(DefaultProviderCapabilities);
  }

  /// <summary>
  /// Records the provider-neutral DVault default conventions and selected provider capability profile on the Entity Framework model.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="providerCapabilities">The provider capability profile used when projecting Data Vault metadata.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder UseDataVault(
      this ModelBuilder modelBuilder,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    UseDataVaultCore(modelBuilder, providerCapabilities);

    return modelBuilder;
  }

  /// <summary>
  /// Records the provider-neutral DVault default conventions, selected provider profile, and load-timestamp storage shape.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="providerCapabilities">The provider capability profile used when projecting Data Vault metadata.</param>
  /// <param name="loadTimestampStorage">The physical load-timestamp storage shape to project.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder UseDataVault(
      this ModelBuilder modelBuilder,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return modelBuilder.UseDataVault(providerCapabilities.WithLoadTimestampStorage(loadTimestampStorage));
  }

  /// <summary>
  /// Translates provider-neutral Data Vault metadata declarations into provider-aware Entity Framework model metadata.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="metadataModel">The provider-neutral Data Vault metadata declarations to project.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(modelBuilder);

    return modelBuilder.ApplyDataVaultMetadata(
        metadataModel,
        DataVaultProviderCapabilityProfileSelection.Select(modelBuilder));
  }

  /// <summary>
  /// Translates an immutable Data Vault metadata registry into provider-aware Entity Framework model metadata.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="metadataRegistry">The authoritative metadata registry to project.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    return ApplyDataVaultMetadataRegistry(
        modelBuilder,
        metadataRegistry,
        DataVaultMetadataSourceKinds.ModelRegistry);
  }

  /// <summary>
  /// Translates a successful model-first import result into provider-aware Entity Framework model metadata.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="importResult">The successful model-first import result to project.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      DataVaultModelImportResult importResult) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(importResult);

    var projectionResult = importResult.ApplyTo(modelBuilder);
    projectionResult.ThrowIfInvalid();

    return modelBuilder;
  }

  /// <summary>
  /// Translates a successful model-first import result into Entity Framework metadata for one provider profile.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="importResult">The successful model-first import result to project.</param>
  /// <param name="providerCapabilities">The provider capability profile used to project storage metadata.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      DataVaultModelImportResult importResult,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(importResult);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var projectionResult = importResult.ApplyTo(modelBuilder, providerCapabilities);
    projectionResult.ThrowIfInvalid();

    return modelBuilder;
  }

  /// <summary>
  /// Builds provider-neutral Data Vault metadata from fluent code-first declarations and translates it into Entity Framework metadata.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="configureModel">The code-first Data Vault model configuration callback.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      Action<DataVaultCodeFirstModelBuilder> configureModel) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(configureModel);

    var codeFirstModelBuilder = new DataVaultCodeFirstModelBuilder();
    configureModel(codeFirstModelBuilder);

    return modelBuilder.ApplyDataVaultMetadata(codeFirstModelBuilder.BuildMetadataModel());
  }

  /// <summary>
  /// Translates provider-neutral Data Vault metadata declarations into Entity Framework metadata for one provider profile.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="metadataModel">The provider-neutral Data Vault metadata declarations to project.</param>
  /// <param name="providerCapabilities">The provider capability profile used to project storage metadata.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(metadataModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var shouldProject = DataVaultMetadataSourceAnnotations.TryRecordSource(
        modelBuilder,
        DataVaultMetadataSourceKinds.ModelMetadata,
        DataVaultMetadataSourceAnnotations.CreateFingerprint(metadataModel));
    if (!shouldProject) {
      return modelBuilder;
    }

    providerCapabilities = UseDataVaultCore(modelBuilder, providerCapabilities);
    DataVaultEfMetadataTranslator.Apply(modelBuilder, metadataModel, providerCapabilities);

    return modelBuilder;
  }

  /// <summary>
  /// Translates provider-neutral Data Vault metadata into Entity Framework metadata for one provider profile and timestamp storage shape.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="metadataModel">The provider-neutral Data Vault metadata declarations to project.</param>
  /// <param name="providerCapabilities">The provider capability profile used to project storage metadata.</param>
  /// <param name="loadTimestampStorage">The physical load-timestamp storage shape to project.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return modelBuilder.ApplyDataVaultMetadata(
        metadataModel,
        providerCapabilities.WithLoadTimestampStorage(loadTimestampStorage));
  }

  internal static ModelBuilder ApplyDataVaultMetadataRegistry(
      ModelBuilder modelBuilder,
      DataVaultMetadataRegistry metadataRegistry,
      string sourceKind,
      DataVaultProviderCapabilityProfile? providerCapabilities = null,
      DataVaultConventions? conventions = null) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(metadataRegistry);
    ArgumentException.ThrowIfNullOrWhiteSpace(sourceKind);

    var shouldProject = DataVaultMetadataSourceAnnotations.TryRecordSource(
        modelBuilder,
        sourceKind,
        DataVaultMetadataSourceAnnotations.CreateFingerprint(metadataRegistry));
    if (!shouldProject) {
      return modelBuilder;
    }

    providerCapabilities = providerCapabilities is null
        ? DataVaultMetadataSourceAnnotations.SelectProviderCapabilities(modelBuilder, metadataRegistry)
        : DataVaultMetadataSourceAnnotations.SelectProviderCapabilities(providerCapabilities, metadataRegistry);
    var metadataModel = DataVaultMetadataSourceAnnotations.CreateMetadataModel(metadataRegistry);

    providerCapabilities = UseDataVaultCore(modelBuilder, providerCapabilities, conventions);
    DataVaultEfMetadataTranslator.Apply(modelBuilder, metadataModel, providerCapabilities);

    return modelBuilder;
  }

  internal static DataVaultProviderCapabilityProfile UseDataVaultCore(
      ModelBuilder modelBuilder,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultConventions? conventions = null) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var currentHashKeyMapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.HashKey);
    var annotatedConventions = modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.Conventions)?.Value as DataVaultConventions;
    var providerHasNonDefaultHashShape = HasNonDefaultHashKeyShape(currentHashKeyMapping);
    var modelConventions = providerHasNonDefaultHashShape ? null : annotatedConventions;
    var hashKeyStorageProfile = conventions?.HashKeyStorageProfile ??
        modelConventions?.HashKeyStorageProfile ??
        currentHashKeyMapping.HashKeyStorageProfile ??
        DataVaultConventions.Default.HashKeyStorageProfile;
    var stableHashAlgorithmId = conventions?.StableHashAlgorithmId ??
        modelConventions?.StableHashAlgorithmId ??
        currentHashKeyMapping.StableHashAlgorithmId ??
        DataVaultConventions.Default.StableHashAlgorithmId;
    var stableHashDigestByteLength = conventions?.StableHashDigestByteLength ??
        modelConventions?.StableHashDigestByteLength ??
        currentHashKeyMapping.DigestByteLength ??
        DataVaultConventions.Default.StableHashDigestByteLength;
    conventions = DataVaultConventions.CreateWithStableHashAlgorithm(
        stableHashAlgorithmId,
        stableHashDigestByteLength,
        hashKeyStorageProfile);
    var projectedProviderCapabilities = providerCapabilities.WithHashKeyStorageProfile(
        hashKeyStorageProfile,
        stableHashAlgorithmId,
        stableHashDigestByteLength);

    modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.Conventions, conventions);
    modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, projectedProviderCapabilities.ProfileName);

    return projectedProviderCapabilities;
  }

  private static bool HasNonDefaultHashKeyShape(DataVaultProviderTypeMapping mapping) {
    return mapping.HashKeyStorageProfile is not null &&
        (mapping.HashKeyStorageProfile != DataVaultConventions.Default.HashKeyStorageProfile ||
            !string.Equals(mapping.StableHashAlgorithmId, DataVaultConventions.Default.StableHashAlgorithmId, StringComparison.Ordinal) ||
            mapping.DigestByteLength != DataVaultConventions.Default.StableHashDigestByteLength);
  }
}
