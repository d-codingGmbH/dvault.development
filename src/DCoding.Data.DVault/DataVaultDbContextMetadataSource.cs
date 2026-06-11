using System.Reflection;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

internal static class DataVaultDbContextMetadataSource {
  public static DataVaultDbContextOptionsExtension? FindExtension(DbContext context) {
    ArgumentNullException.ThrowIfNull(context);

    var serviceProvider = ((IInfrastructure<IServiceProvider>)context).Instance;
    return serviceProvider
        .GetService<IDbContextOptions>()
        ?.FindExtension<DataVaultDbContextOptionsExtension>();
  }

  public static DataVaultResolvedDbContextMetadataSource Resolve(
      DbContext context,
      DataVaultDbContextOptionsExtension extension) {
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(extension);

    if (extension.MetadataRegistry is not null) {
      return new DataVaultResolvedDbContextMetadataSource(
          extension.SourceKind,
          extension.MetadataRegistry);
    }

    var metadataRegistry = TryResolveAppDefaultRegistry(context);
    if (metadataRegistry is null) {
      throw new InvalidOperationException(
          "DVault metadata was opted in through DbContext options, but no app-level DataVaultMetadataRegistry is registered. " +
          "Register one with services.AddDVault(options => options.UseMetadataModel(...)) or " +
          "services.AddDVault(options => options.UseMetadataRegistry(...)), or configure an explicit context-scoped registry with UseDataVaultMetadata(...).");
    }

    return new DataVaultResolvedDbContextMetadataSource(
        extension.SourceKind,
        metadataRegistry);
  }

  public static DataVaultDbContextMetadataSourceKey CreateCacheKey(
      DbContext context,
      DataVaultDbContextOptionsExtension extension) {
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(extension);

    var metadataRegistry = extension.MetadataRegistry ?? TryResolveAppDefaultRegistry(context);
    return metadataRegistry is null
        ? new DataVaultDbContextMetadataSourceKey(extension.SourceKind, "<missing>")
        : new DataVaultDbContextMetadataSourceKey(
            extension.SourceKind,
            DataVaultMetadataSourceAnnotations.CreateFingerprint(metadataRegistry));
  }

  private static DataVaultMetadataRegistry? TryResolveAppDefaultRegistry(DbContext context) {
    var serviceProvider = ((IInfrastructure<IServiceProvider>)context).Instance;
    var metadataRegistry = serviceProvider.GetService<DataVaultMetadataRegistry>();
    if (metadataRegistry is not null) {
      return metadataRegistry;
    }

    var options = serviceProvider.GetService<IDbContextOptions>();
    if (options is null) {
      return null;
    }

    foreach (var extension in options.Extensions) {
      var applicationServiceProvider = TryGetApplicationServiceProvider(extension);
      metadataRegistry = applicationServiceProvider?.GetService<DataVaultMetadataRegistry>();
      if (metadataRegistry is not null) {
        return metadataRegistry;
      }
    }

    return null;
  }

  private static IServiceProvider? TryGetApplicationServiceProvider(IDbContextOptionsExtension extension) {
    return extension
        .GetType()
        .GetProperty(
            "ApplicationServiceProvider",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        ?.GetValue(extension) as IServiceProvider;
  }
}
