using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one materialized PIT-backed as-of row returned by a Data Vault read service.
/// </summary>
public sealed class DataVaultPitReadRecord {
  private IReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot>? _satelliteSnapshotsByName;

  internal DataVaultPitReadRecord(
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      IReadOnlyList<DataVaultPitSatelliteSnapshot> satelliteSnapshots) {
    ParentHashKey = parentHashKey;
    LoadTimestamp = loadTimestamp.ToUniversalTime();
    var snapshotArray = satelliteSnapshots as DataVaultPitSatelliteSnapshot[] ?? satelliteSnapshots.ToArray();
    SatelliteSnapshots = new ReadOnlyCollection<DataVaultPitSatelliteSnapshot>(snapshotArray);
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
  public IReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot> SatelliteSnapshotsByName =>
      _satelliteSnapshotsByName ??= new ReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot>(
          CreateSnapshotMap(SatelliteSnapshots));

  private static Dictionary<string, DataVaultPitSatelliteSnapshot> CreateSnapshotMap(
      IReadOnlyList<DataVaultPitSatelliteSnapshot> satelliteSnapshots) {
    var snapshotsByName = new Dictionary<string, DataVaultPitSatelliteSnapshot>(
        satelliteSnapshots.Count,
        StringComparer.Ordinal);
    foreach (var snapshot in satelliteSnapshots) {
      snapshotsByName.Add(snapshot.SatelliteName, snapshot);
    }

    return snapshotsByName;
  }
}
