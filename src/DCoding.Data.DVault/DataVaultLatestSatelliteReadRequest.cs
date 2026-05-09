using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes a request for latest satellite rows for explicit parent hash keys.
/// </summary>
public sealed class DataVaultLatestSatelliteReadRequest {
  /// <summary>
  /// Initializes a new latest satellite read request.
  /// </summary>
  /// <param name="satellite">The satellite metadata declaration to read.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  public DataVaultLatestSatelliteReadRequest(
      DataVaultSatelliteMetadata satellite,
      IEnumerable<string> parentHashKeys)
      : this(satellite, parentHashKeys, asOf: null) {
  }

  /// <summary>
  /// Initializes a new latest satellite read request with an optional as-of timestamp.
  /// </summary>
  /// <param name="satellite">The satellite metadata declaration to read.</param>
  /// <param name="parentHashKeys">The parent hub or link hash keys to read.</param>
  /// <param name="asOf">The inclusive UTC cutoff for as-of reads, or null for the latest persisted row.</param>
  public DataVaultLatestSatelliteReadRequest(
      DataVaultSatelliteMetadata satellite,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset? asOf) {
    ArgumentNullException.ThrowIfNull(satellite);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    Satellite = satellite;
    ParentHashKeys = RequireParentHashKeys(parentHashKeys);
    AsOf = asOf?.ToUniversalTime();
  }

  /// <summary>
  /// Gets the satellite metadata declaration to read.
  /// </summary>
  public DataVaultSatelliteMetadata Satellite { get; }

  /// <summary>
  /// Gets the parent hub or link hash keys to read.
  /// </summary>
  public IReadOnlyList<string> ParentHashKeys { get; }

  /// <summary>
  /// Gets the inclusive UTC cutoff for as-of reads, or null for the latest persisted row.
  /// </summary>
  public DateTimeOffset? AsOf { get; }

  internal static IReadOnlyList<string> RequireParentHashKeys(IEnumerable<string> parentHashKeys) {
    var values = parentHashKeys
        .Distinct(StringComparer.Ordinal)
        .ToArray();

    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(parentHashKeys));
    }

    return values;
  }
}
