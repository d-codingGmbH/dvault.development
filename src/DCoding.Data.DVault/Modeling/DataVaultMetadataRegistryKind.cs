namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies the Data Vault metadata kind addressed by registry lookup and optional CLR mapping.
/// </summary>
public enum DataVaultMetadataRegistryKind {
  /// <summary>
  /// Hub metadata.
  /// </summary>
  Hub,

  /// <summary>
  /// Link metadata.
  /// </summary>
  Link,

  /// <summary>
  /// Satellite metadata.
  /// </summary>
  Satellite,

  /// <summary>
  /// Legacy point-in-time table metadata.
  /// </summary>
  PointInTimeTable,

  /// <summary>
  /// Bridge metadata.
  /// </summary>
  Bridge,

  /// <summary>
  /// PIT metadata.
  /// </summary>
  Pit,
}
