using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultRegistryMetadataResolver {
  public static DataVaultMetadataRegistry ResolveRequiredRegistry(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    DataVaultDbContextOptionsExtension? extension;
    try {
      extension = DataVaultDbContextMetadataSource.FindExtension(dbContext);
    }
    catch (NotSupportedException exception) {
      throw MetadataSourceResolutionFailure(exception);
    }

    if (extension is null) {
      throw new InvalidOperationException(
          "Registry-backed DVault operations require an authoritative DataVaultMetadataRegistry selected through " +
          "DbContext options. Configure UseDataVaultMetadata() with an app-level AddDVault metadata registry, or " +
          "configure an explicit context-scoped registry with UseDataVaultMetadata(...).");
    }

    try {
      return DataVaultDbContextMetadataSource.Resolve(dbContext, extension).MetadataRegistry;
    }
    catch (NotSupportedException exception) {
      throw MetadataSourceResolutionFailure(exception);
    }
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

  public static DataVaultBridgeMetadata GetRequiredBridge(
      DataVaultMetadataRegistry registry,
      string bridgeName) {
    ArgumentNullException.ThrowIfNull(registry);

    if (registry.TryGetBridge(bridgeName, out var bridge) && bridge is not null) {
      return bridge;
    }

    throw new InvalidOperationException(
        "The authoritative Data Vault metadata registry does not contain bridge metadata '" + bridgeName + "'.");
  }

  public static DataVaultPitMetadata GetRequiredPit(
      DataVaultMetadataRegistry registry,
      string pitName) {
    ArgumentNullException.ThrowIfNull(registry);

    if (registry.TryGetPit(pitName, out var pit) && pit is not null) {
      return pit;
    }

    throw new InvalidOperationException(
        "The authoritative Data Vault metadata registry does not contain PIT metadata '" + pitName + "'.");
  }

  public static DataVaultPitMetadata GetRequiredPit(
      DataVaultMetadataRegistry registry,
      Type pitClrType) {
    ArgumentNullException.ThrowIfNull(registry);
    ArgumentNullException.ThrowIfNull(pitClrType);

    if (registry.TryGetPit(pitClrType, out var pit) && pit is not null) {
      return pit;
    }

    throw new InvalidOperationException(
        "The authoritative Data Vault metadata registry does not contain PIT metadata mapped to CLR type '" +
        FormatClrType(pitClrType) +
        "'.");
  }

  private static string FormatParent(DataVaultMetadataReference parent) {
    return parent.Kind.ToString().ToLowerInvariant() + " '" + parent.Name + "'";
  }

  private static string FormatClrType(Type clrType) {
    return clrType.FullName ?? clrType.Name;
  }

  private static InvalidOperationException MetadataSourceResolutionFailure(NotSupportedException exception) {
    return new InvalidOperationException(
        "Registry-backed DVault operations could not resolve the authoritative metadata registry because the " +
        "configured metadata source is outside the supported registry-backed operation baseline: " +
        exception.Message,
        exception);
  }
}
