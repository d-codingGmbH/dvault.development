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
      IReadOnlyDictionary<string, string> drivingKeyValues,
      IReadOnlyList<DataVaultPitSatelliteSnapshot> satelliteSnapshots) {
    ParentHashKey = parentHashKey;
    LoadTimestamp = loadTimestamp.ToUniversalTime();
    DrivingKeyValues = new ReadOnlyDictionary<string, string>(
        drivingKeyValues as IDictionary<string, string> ??
        drivingKeyValues.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.Ordinal));
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
  /// Gets PIT row driving-key values keyed by canonical driving-key name for tuple-aware PIT rows.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

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
