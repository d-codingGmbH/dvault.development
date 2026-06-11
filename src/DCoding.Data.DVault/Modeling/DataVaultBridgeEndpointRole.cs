using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies the role of one bridge endpoint binding.
/// </summary>
internal enum DataVaultBridgeEndpointRole {
  /// <summary>
  /// Many-to-many source endpoint.
  /// </summary>
  From,

  /// <summary>
  /// Many-to-many target endpoint.
  /// </summary>
  To,

  /// <summary>
  /// Hierarchy ancestor endpoint.
  /// </summary>
  Ancestor,

  /// <summary>
  /// Hierarchy descendant endpoint.
  /// </summary>
  Descendant,
}
