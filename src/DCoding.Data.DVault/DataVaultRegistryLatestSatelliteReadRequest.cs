using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes a registry-backed request for latest satellite rows for explicit parent hash keys.
/// </summary>
public sealed class DataVaultRegistryLatestSatelliteReadRequest {
  /// <summary>
  /// Initializes a new registry-backed latest satellite read request.
  /// </summary>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  public DataVaultRegistryLatestSatelliteReadRequest(
      DataVaultMetadataReference parent,
      string satelliteName,
      IEnumerable<string> parentHashKeys)
      : this(parent, satelliteName, parentHashKeys, asOf: null) {
  }

  /// <summary>
  /// Initializes a new registry-backed latest satellite read request with an optional as-of timestamp.
  /// </summary>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="asOf">The inclusive UTC cutoff for as-of reads, or null for the latest persisted row.</param>
  public DataVaultRegistryLatestSatelliteReadRequest(
      DataVaultMetadataReference parent,
      string satelliteName,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset? asOf) {
    ArgumentNullException.ThrowIfNull(parent);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    Parent = parent;
    SatelliteName = DataVaultMetadataValidation.RequireName(satelliteName, nameof(satelliteName));
    ParentHashKeys = DataVaultLatestSatelliteReadRequest.RequireParentHashKeys(parentHashKeys);
    AsOf = asOf?.ToUniversalTime();
  }

  /// <summary>
  /// Gets the exact parent hub or link metadata reference used to resolve the satellite.
  /// </summary>
  public DataVaultMetadataReference Parent { get; }

  /// <summary>
  /// Gets the exact logical satellite metadata name to resolve from the authoritative registry.
  /// </summary>
  public string SatelliteName { get; }

  /// <summary>
  /// Gets the parent hub or link hash keys to read.
  /// </summary>
  public IReadOnlyList<string> ParentHashKeys { get; }

  /// <summary>
  /// Gets the inclusive UTC cutoff for as-of reads, or null for the latest persisted row.
  /// </summary>
  public DateTimeOffset? AsOf { get; }
}
