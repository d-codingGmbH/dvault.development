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

    modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.Conventions, DataVaultConventions.Default);
    modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, providerCapabilities.ProfileName);

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

    modelBuilder.UseDataVault(providerCapabilities);
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
}
