namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Groups provider-neutral Data Vault metadata declarations for Entity Framework translation.
/// </summary>
public sealed class DataVaultMetadataModel {
  /// <summary>
  /// Initializes a new aggregate metadata model.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  public DataVaultMetadataModel(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites) {
    Hubs = RequireItems(hubs, nameof(hubs));
    Links = RequireItems(links, nameof(links));
    Satellites = RequireItems(satellites, nameof(satellites));
  }

  /// <summary>
  /// Gets the hub metadata declarations to translate.
  /// </summary>
  public IReadOnlyList<DataVaultHubMetadata> Hubs { get; }

  /// <summary>
  /// Gets the link metadata declarations to translate.
  /// </summary>
  public IReadOnlyList<DataVaultLinkMetadata> Links { get; }

  /// <summary>
  /// Gets the satellite metadata declarations to translate.
  /// </summary>
  public IReadOnlyList<DataVaultSatelliteMetadata> Satellites { get; }

  /// <summary>
  /// Creates a new aggregate metadata model from provider-neutral Data Vault declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <returns>The aggregate metadata model.</returns>
  public static DataVaultMetadataModel Create(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites) {
    return new DataVaultMetadataModel(hubs, links, satellites);
  }

  private static IReadOnlyList<T> RequireItems<T>(IEnumerable<T> items, string parameterName)
      where T : class {
    ArgumentNullException.ThrowIfNull(items, parameterName);

    var values = items.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Metadata declaration collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}
