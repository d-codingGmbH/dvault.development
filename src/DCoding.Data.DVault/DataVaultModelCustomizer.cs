using System.Reflection;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

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
    var conventions = DataVaultDbContextMetadataSource.TryResolveAppDefaultConventions(context);
    DataVaultModelBuilderExtensions.ApplyDataVaultMetadataRegistry(
        modelBuilder,
        source.MetadataRegistry,
        source.SourceKind,
        conventions: conventions);
  }
}
