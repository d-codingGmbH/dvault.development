using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies optional bridge projection features outside the baseline provider-neutral v1 mapping.
/// </summary>
[Flags]
internal enum DataVaultBridgeProjectionFeatures {
  /// <summary>
  /// The bridge asks only for the baseline provider-neutral projection.
  /// </summary>
  None = 0,

  /// <summary>
  /// The bridge asks for effectivity-window columns.
  /// </summary>
  EffectivityWindow = 1,

  /// <summary>
  /// The bridge asks for additional path payload columns.
  /// </summary>
  PathPayload = 2,

  /// <summary>
  /// The bridge asks for closure maintenance state.
  /// </summary>
  ClosureMaintenance = 4,

  /// <summary>
  /// The bridge asks for generated Entity Framework relationship graph metadata.
  /// </summary>
  RelationshipGraph = 8,
}
