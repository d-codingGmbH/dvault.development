using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultRegistryMetadataResolver {
  public static DataVaultMetadataRegistry ResolveRequiredRegistry(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var extension = DataVaultDbContextMetadataSource.FindExtension(dbContext);
    if (extension is null) {
      throw new InvalidOperationException(
          "Registry-backed DVault operations require an authoritative DataVaultMetadataRegistry selected through " +
          "DbContext options. Configure UseDataVaultMetadata() with an app-level AddDVault metadata registry, or " +
          "configure an explicit context-scoped registry with UseDataVaultMetadata(...).");
    }

    return DataVaultDbContextMetadataSource.Resolve(dbContext, extension).MetadataRegistry;
  }

  public static DataVaultHubMetadata GetRequiredHub(
      DataVaultMetadataRegistry registry,
      string hubName) {
    ArgumentNullException.ThrowIfNull(registry);

    if (registry.TryGetHub(hubName, out var hub) && hub is not null) {
      return hub;
    }

    throw new InvalidOperationException(
        "The authoritative Data Vault metadata registry does not contain hub metadata '" + hubName + "'.");
  }

  public static DataVaultLinkMetadata GetRequiredLink(
      DataVaultMetadataRegistry registry,
      string linkName) {
    ArgumentNullException.ThrowIfNull(registry);

    if (registry.TryGetLink(linkName, out var link) && link is not null) {
      return link;
    }

    throw new InvalidOperationException(
        "The authoritative Data Vault metadata registry does not contain link metadata '" + linkName + "'.");
  }

  public static DataVaultSatelliteMetadata GetRequiredSatellite(
      DataVaultMetadataRegistry registry,
      DataVaultMetadataReference parent,
      string satelliteName) {
    ArgumentNullException.ThrowIfNull(registry);
    ArgumentNullException.ThrowIfNull(parent);

    if (registry.TryGetSatellite(parent, satelliteName, out var satellite) && satellite is not null) {
      return satellite;
    }

    throw new InvalidOperationException(
        "The authoritative Data Vault metadata registry does not contain satellite metadata '" +
        satelliteName +
        "' under " +
        FormatParent(parent) +
        ".");
  }

  private static string FormatParent(DataVaultMetadataReference parent) {
    return parent.Kind.ToString().ToLowerInvariant() + " '" + parent.Name + "'";
  }
}
