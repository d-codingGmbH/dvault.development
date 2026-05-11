using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one materialized PIT-backed as-of row returned by a Data Vault read service.
/// </summary>
public sealed class DataVaultPitReadRecord {
  internal DataVaultPitReadRecord(
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      IReadOnlyList<DataVaultPitSatelliteSnapshot> satelliteSnapshots) {
    ParentHashKey = parentHashKey;
    LoadTimestamp = loadTimestamp.ToUniversalTime();
    SatelliteSnapshots = new ReadOnlyCollection<DataVaultPitSatelliteSnapshot>(satelliteSnapshots.ToArray());
    SatelliteSnapshotsByName = new ReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot>(
        satelliteSnapshots.ToDictionary(snapshot => snapshot.SatelliteName, snapshot => snapshot, StringComparer.Ordinal));
  }

  /// <summary>
  /// Gets the requested parent hub hash key that matched a PIT row.
  /// </summary>
  public string ParentHashKey { get; }

  /// <summary>
  /// Gets the matched PIT row load timestamp normalized to UTC.
  /// </summary>
  public DateTimeOffset LoadTimestamp { get; }

  /// <summary>
  /// Gets satellite snapshot segments in PIT declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultPitSatelliteSnapshot> SatelliteSnapshots { get; }

  /// <summary>
  /// Gets satellite snapshot segments keyed by declared satellite name using ordinal comparison.
  /// </summary>
  public IReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot> SatelliteSnapshotsByName { get; }
}
