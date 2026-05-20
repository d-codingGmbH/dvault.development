namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the public read family described by one read telemetry summary.
/// </summary>
public enum DataVaultReadTelemetryFamily {
  /// <summary>
  /// Telemetry describes a latest, current, or as-of satellite read.
  /// </summary>
  LatestSatellite,

  /// <summary>
  /// Telemetry describes a PIT-backed as-of read.
  /// </summary>
  Pit,

  /// <summary>
  /// Telemetry describes a generated bridge read.
  /// </summary>
  Bridge,
}
