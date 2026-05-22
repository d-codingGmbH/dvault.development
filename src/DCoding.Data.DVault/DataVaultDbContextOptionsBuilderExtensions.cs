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
  /// Opts a DbContext into projecting a successful model-first import result.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <param name="importResult">The successful model-first import result to project for this DbContext configuration.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultMetadata(
      this DbContextOptionsBuilder optionsBuilder,
      DataVaultModelImportResult importResult) {
    ArgumentNullException.ThrowIfNull(importResult);

    return optionsBuilder.UseDataVaultMetadata(importResult.RequireMetadataRegistry());
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

  /// <summary>
  /// Opts a DbContext into the optional Data Vault SaveChanges metadata interceptor.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <param name="configure">The interceptor options callback that supplies metadata values.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultSaveChangesMetadataInterceptor(
      this DbContextOptionsBuilder optionsBuilder,
      Action<DataVaultSaveChangesMetadataInterceptorOptions> configure) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentNullException.ThrowIfNull(configure);

    var options = new DataVaultSaveChangesMetadataInterceptorOptions();
    configure(options);

    return optionsBuilder.UseDataVaultSaveChangesMetadataInterceptor(options);
  }

  /// <summary>
  /// Opts a DbContext into the optional Data Vault SaveChanges metadata interceptor.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <param name="options">The interceptor options that supply metadata values.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultSaveChangesMetadataInterceptor(
      this DbContextOptionsBuilder optionsBuilder,
      DataVaultSaveChangesMetadataInterceptorOptions options) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentNullException.ThrowIfNull(options);

    optionsBuilder.AddInterceptors(new DataVaultSaveChangesMetadataInterceptor(options));

    return optionsBuilder;
  }

  /// <summary>
  /// Opts a DbContext into the optional Data Vault SaveChanges runtime guard interceptor.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <param name="configure">The guard options callback that selects blocking or warning behavior.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultSaveChangesGuardInterceptor(
      this DbContextOptionsBuilder optionsBuilder,
      Action<DataVaultSaveChangesGuardOptions> configure) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentNullException.ThrowIfNull(configure);

    var options = new DataVaultSaveChangesGuardOptions();
    configure(options);

    return optionsBuilder.UseDataVaultSaveChangesGuardInterceptor(options);
  }

  /// <summary>
  /// Opts a DbContext into the optional Data Vault SaveChanges runtime guard interceptor.
  /// </summary>
  /// <param name="optionsBuilder">The Entity Framework DbContext options builder.</param>
  /// <param name="options">The guard options that select blocking or warning behavior.</param>
  /// <returns>The same options builder so DbContext configuration can continue fluently.</returns>
  public static DbContextOptionsBuilder UseDataVaultSaveChangesGuardInterceptor(
      this DbContextOptionsBuilder optionsBuilder,
      DataVaultSaveChangesGuardOptions options) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentNullException.ThrowIfNull(options);

    optionsBuilder.AddInterceptors(new DataVaultSaveChangesGuardInterceptor(options));

    return optionsBuilder;
  }

  private static void AddOrUpdateExtension(
      DbContextOptionsBuilder optionsBuilder,
      DataVaultDbContextOptionsExtension extension) {
    ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
  }
}
