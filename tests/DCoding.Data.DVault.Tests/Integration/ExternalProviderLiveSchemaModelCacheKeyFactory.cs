using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class ExternalProviderLiveSchemaModelCacheKeyFactory : IModelCacheKeyFactory {
  public object Create(DbContext context, bool designTime) {
    if (context is ExternalProviderLiveSchemaContext liveSchemaContext) {
      var options = liveSchemaContext.ModelOptions;
      return (
          context.GetType(),
          options.ProviderCapabilities.ProfileName,
          options.DefaultSchema ?? string.Empty,
          options.TableNamePrefix,
          string.Join("|", options.TableNameOverrides.OrderBy(item => item.Key, StringComparer.Ordinal)),
          string.Join("|", options.IdentifierNameOverrides.OrderBy(item => item.Key, StringComparer.Ordinal)),
          designTime);
    }

    return (context.GetType(), designTime);
  }
}
