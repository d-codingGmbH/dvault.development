namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the bridge endpoint used as the traversal filter for a bridge read request.
/// </summary>
public enum DataVaultBridgeTraversalEndpoint {
  /// <summary>
  /// Reads a many-to-many bridge from the declared source endpoint.
  /// </summary>
  From,

  /// <summary>
  /// Reads a many-to-many bridge from the declared target endpoint.
  /// </summary>
  To,

  /// <summary>
  /// Reads a hierarchy bridge from the declared ancestor endpoint.
  /// </summary>
  Ancestor,

  /// <summary>
  /// Reads a hierarchy bridge from the declared descendant endpoint.
  /// </summary>
  Descendant,
}
