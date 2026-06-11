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
