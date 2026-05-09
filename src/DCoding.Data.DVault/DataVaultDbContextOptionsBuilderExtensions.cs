using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides Entity Framework Core DbContext options integration for configured DVault metadata registries.
/// </summary>
public static class DataVaultDbContextOptionsBuilderExtensions {
  /// <summary>
  /// Opts a DbContext into projecting the app-level default Data Vault metadata registry registered through AddDVault options.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultMetadata(this DbContextOptionsBuilder optionsBuilder) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);

    AddOrUpdateExtension(
        optionsBuilder,
        new DataVaultDbContextOptionsExtension(
            DataVaultMetadataSourceKinds.AppDefaultRegistry,
            metadataRegistry: null));

    return optionsBuilder;
  }

  /// <summary>
  /// Opts a DbContext into projecting an explicit Data Vault metadata model.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <param name="metadataModel">The provider-neutral metadata model to convert once into the context-scoped registry.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultMetadata(
      this DbContextOptionsBuilder optionsBuilder,
      DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(metadataModel);

    return optionsBuilder.UseDataVaultMetadata(DataVaultMetadataRegistry.Create(metadataModel));
  }

  /// <summary>
  /// Opts a DbContext into projecting an explicit context-scoped Data Vault metadata registry.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <param name="metadataRegistry">The immutable metadata registry to project for this DbContext configuration.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultMetadata(
      this DbContextOptionsBuilder optionsBuilder,
      DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    AddOrUpdateExtension(
        optionsBuilder,
        new DataVaultDbContextOptionsExtension(
            DataVaultMetadataSourceKinds.DbContextRegistry,
            metadataRegistry));

    return optionsBuilder;
  }

  private static void AddOrUpdateExtension(
      DbContextOptionsBuilder optionsBuilder,
      DataVaultDbContextOptionsExtension extension) {
    ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
  }
}
