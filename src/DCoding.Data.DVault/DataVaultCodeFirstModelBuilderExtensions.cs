using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides Entity Framework Core model configuration extensions for fluent DVault Code-First declarations.
/// </summary>
public static class DataVaultCodeFirstModelBuilderExtensions {
  /// <summary>
  /// Builds provider-neutral Data Vault metadata from fluent CLR entity declarations and translates it into Entity Framework metadata.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="configureModel">The fluent Code-First Data Vault metadata declarations to project.</param>
  /// <param name="providerCapabilities">The optional provider capability profile used to project storage metadata.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile? providerCapabilities = null) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(configureModel);

    providerCapabilities ??= DataVaultProviderCapabilityProfileSelection.Select(modelBuilder);

    return modelBuilder.ApplyDataVaultMetadata(
        BuildMetadataModel(configureModel),
        providerCapabilities);
  }

  /// <summary>
  /// Builds fluent Code-First Data Vault metadata, records the binary-first conventions profile, and translates it into Entity Framework metadata.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="configureModel">The fluent Code-First Data Vault metadata declarations to project.</param>
  /// <param name="providerCapabilities">The optional provider capability profile used to project storage metadata.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadataWithBinaryFirstProfile(
      this ModelBuilder modelBuilder,
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile? providerCapabilities = null) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(configureModel);

    providerCapabilities ??= DataVaultProviderCapabilityProfileSelection.Select(modelBuilder);
    var metadataModel = BuildMetadataModel(configureModel);

    modelBuilder.UseDataVaultBinaryFirstProfile(providerCapabilities);

    return modelBuilder.ApplyDataVaultMetadata(metadataModel, providerCapabilities);
  }

  /// <summary>
  /// Builds provider-neutral Data Vault metadata from fluent CLR entity declarations and translates it for one provider profile and timestamp storage shape.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="configureModel">The fluent Code-First Data Vault metadata declarations to project.</param>
  /// <param name="providerCapabilities">The provider capability profile used to project storage metadata.</param>
  /// <param name="loadTimestampStorage">The physical load-timestamp storage shape to project.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return modelBuilder.ApplyDataVaultMetadata(
        configureModel,
        providerCapabilities.WithLoadTimestampStorage(loadTimestampStorage));
  }

  private static DataVaultMetadataModel BuildMetadataModel(
      Action<DataVaultCodeFirstModelBuilder> configureModel) {
    var codeFirstModelBuilder = new DataVaultCodeFirstModelBuilder();
    configureModel(codeFirstModelBuilder);

    return codeFirstModelBuilder.BuildMetadataModel();
  }
}
