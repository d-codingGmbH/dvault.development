namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies point-in-time table column families.
/// </summary>
public enum DataVaultPointInTimeColumnKind {
  /// <summary>
  /// Hash-key reference to the PIT table's hub.
  /// </summary>
  HubHashKeyReference,

  /// <summary>
  /// PIT load timestamp used with the hub hash-key reference as the table key.
  /// </summary>
  LoadTimestamp,

  /// <summary>
  /// Snapshot load-timestamp reference for one participating satellite.
  /// </summary>
  SatelliteSnapshotLoadTimestampReference,
}
