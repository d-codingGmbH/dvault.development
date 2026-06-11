using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies the supported provider-neutral bridge traversal shapes.
/// </summary>
public enum DataVaultBridgeKind {
  /// <summary>
  /// Represents a bridge that traverses one link between a source hub and a target hub.
  /// </summary>
  ManyToMany,

  /// <summary>
  /// Represents a bridge that traverses one recursive link as a directional hierarchy edge.
  /// </summary>
  Hierarchy,
}
