namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies Data Vault technical column families.
/// </summary>
public enum DataVaultTechnicalColumnKind {
  /// <summary>
  /// Hash key column.
  /// </summary>
  HashKey,

  /// <summary>
  /// Hash diff column.
  /// </summary>
  HashDiff,

  /// <summary>
  /// Load timestamp column.
  /// </summary>
  LoadTimestamp,

  /// <summary>
  /// Record source column.
  /// </summary>
  RecordSource,
}
