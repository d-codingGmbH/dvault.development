namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies produced Data Vault table kinds.
/// </summary>
public enum DataVaultTableKind {
  /// <summary>
  /// Hub table.
  /// </summary>
  Hub,

  /// <summary>
  /// Link table.
  /// </summary>
  Link,

  /// <summary>
  /// Satellite table.
  /// </summary>
  Satellite,

  /// <summary>
  /// Point-in-time table.
  /// </summary>
  PointInTime,

  /// <summary>
  /// PIT table translated from metadata declarations.
  /// </summary>
  Pit,

  /// <summary>
  /// Bridge table.
  /// </summary>
  Bridge,
}
