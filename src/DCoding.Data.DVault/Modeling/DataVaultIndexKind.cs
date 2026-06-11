namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies index name families produced by the modeling flow.
/// </summary>
public enum DataVaultIndexKind {
  /// <summary>
  /// Index over business-key columns.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Index over relationship columns.
  /// </summary>
  Relationship,

  /// <summary>
  /// Index over satellite parent columns.
  /// </summary>
  SatelliteParent,

  /// <summary>
  /// Index over bridge traversal columns.
  /// </summary>
  BridgeTraversal,

  /// <summary>
  /// Index over PIT row traversal columns.
  /// </summary>
  PitTraversal,
}
