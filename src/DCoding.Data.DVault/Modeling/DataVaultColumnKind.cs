namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies produced Data Vault column kinds.
/// </summary>
public enum DataVaultColumnKind {
  /// <summary>
  /// Data Vault technical column.
  /// </summary>
  Technical,

  /// <summary>
  /// Business-key column.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Satellite payload column.
  /// </summary>
  Payload,

  /// <summary>
  /// Point-in-time table column.
  /// </summary>
  PointInTime,

  /// <summary>
  /// Satellite driving-key column.
  /// </summary>
  DrivingKey,
}
