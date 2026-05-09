using System.Reflection;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

internal sealed class DataVaultDbContextOptionsExtension : IDbContextOptionsExtension {
  private DbContextOptionsExtensionInfo? _info;

  public DataVaultDbContextOptionsExtension(
      string sourceKind,
      DataVaultMetadataRegistry? metadataRegistry) {
    ArgumentException.ThrowIfNullOrWhiteSpace(sourceKind);

    SourceKind = sourceKind;
    MetadataRegistry = metadataRegistry;
  }

  public string SourceKind { get; }

  public DataVaultMetadataRegistry? MetadataRegistry { get; }

  public DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

  public void ApplyServices(IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    services.AddSingleton<IModelCacheKeyFactory, DataVaultModelCacheKeyFactory>();
    services.AddSingleton<IModelCustomizer, DataVaultModelCustomizer>();
  }

  public void Validate(IDbContextOptions options) {
  }

  private sealed class ExtensionInfo : DbContextOptionsExtensionInfo {
    private readonly DataVaultDbContextOptionsExtension _extension;

    public ExtensionInfo(DataVaultDbContextOptionsExtension extension)
        : base(extension) {
      _extension = extension;
    }

    public override bool IsDatabaseProvider => false;

    public override string LogFragment => "using DVault metadata source " + _extension.SourceKind + " ";

    public override int GetServiceProviderHashCode() {
      return 0;
    }

    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) {
      return other is ExtensionInfo;
    }

    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) {
      ArgumentNullException.ThrowIfNull(debugInfo);

      debugInfo["DCoding.Data.DVault:MetadataSourceKind"] = _extension.SourceKind;
    }
  }
}

internal sealed class DataVaultModelCustomizer : ModelCustomizer {
  public DataVaultModelCustomizer(ModelCustomizerDependencies dependencies)
      : base(dependencies) {
  }

  public override void Customize(ModelBuilder modelBuilder, DbContext context) {
    base.Customize(modelBuilder, context);

    var extension = DataVaultDbContextMetadataSource.FindExtension(context);
    if (extension is null) {
      return;
    }

    var source = DataVaultDbContextMetadataSource.Resolve(context, extension);
    DataVaultModelBuilderExtensions.ApplyDataVaultMetadataRegistry(
        modelBuilder,
        source.MetadataRegistry,
        source.SourceKind);
  }
}

internal sealed class DataVaultModelCacheKeyFactory : IModelCacheKeyFactory {
  public object Create(DbContext context) {
    return Create(context, designTime: false);
  }

  public object Create(DbContext context, bool designTime) {
    ArgumentNullException.ThrowIfNull(context);

    var extension = DataVaultDbContextMetadataSource.FindExtension(context);
    var sourceKey = extension is null
        ? DataVaultDbContextMetadataSourceKey.None
        : DataVaultDbContextMetadataSource.CreateCacheKey(context, extension);

    return new DataVaultModelCacheKey(context.GetType(), designTime, sourceKey.SourceKind, sourceKey.Fingerprint);
  }

  private readonly record struct DataVaultModelCacheKey(
      Type ContextType,
      bool DesignTime,
      string SourceKind,
      string Fingerprint);
}

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

internal sealed record DataVaultResolvedDbContextMetadataSource(
    string SourceKind,
    DataVaultMetadataRegistry MetadataRegistry);

internal readonly record struct DataVaultDbContextMetadataSourceKey(
    string SourceKind,
    string Fingerprint) {
  public static DataVaultDbContextMetadataSourceKey None { get; } = new("<none>", "<none>");
}
