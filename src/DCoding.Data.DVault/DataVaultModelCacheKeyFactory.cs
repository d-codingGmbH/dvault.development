using System.Reflection;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

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
    var conventions = DataVaultDbContextMetadataSource.TryResolveAppDefaultConventions(context) ??
        DataVaultConventions.Default;

    return new DataVaultModelCacheKey(
        context.GetType(),
        designTime,
        sourceKey.SourceKind,
        sourceKey.Fingerprint,
        conventions.StableHashAlgorithmId,
        conventions.StableHashDigestByteLength,
        conventions.HashKeyStorageProfile,
        conventions.ProfileName);
  }

  private readonly record struct DataVaultModelCacheKey(
      Type ContextType,
      bool DesignTime,
      string SourceKind,
      string Fingerprint,
      string StableHashAlgorithmId,
      int StableHashDigestByteLength,
      DataVaultHashKeyStorageProfile HashKeyStorageProfile,
      string ProfileName);
}
