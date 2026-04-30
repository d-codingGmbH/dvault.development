using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides Entity Framework Core model configuration extensions for DVault conventions.
/// </summary>
public static class DataVaultModelBuilderExtensions {
  /// <summary>
  /// Records the provider-neutral DVault default conventions on the Entity Framework model.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder UseDataVault(this ModelBuilder modelBuilder) {
    ArgumentNullException.ThrowIfNull(modelBuilder);

    modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.Conventions, DataVaultConventions.Default);

    return modelBuilder;
  }

  /// <summary>
  /// Translates provider-neutral Data Vault metadata declarations into provider-neutral Entity Framework model metadata.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="metadataModel">The provider-neutral Data Vault metadata declarations to project.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder ApplyDataVaultMetadata(
      this ModelBuilder modelBuilder,
      DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(metadataModel);

    modelBuilder.UseDataVault();
    DataVaultEfMetadataTranslator.Apply(modelBuilder, metadataModel);

    return modelBuilder;
  }
}
