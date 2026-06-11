namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the provider-neutral Data Vault role carried by an Entity Framework property.
/// </summary>
public enum DataVaultPropertyRole {
  /// <summary>
  /// Property carries a Data Vault technical metadata value.
  /// </summary>
  Technical,

  /// <summary>
  /// Property carries a hub business-key value.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Property carries a link participant hash-key reference.
  /// </summary>
  ParticipantReference,

  /// <summary>
  /// Property carries a satellite descriptive payload value.
  /// </summary>
  Payload,

  /// <summary>
  /// Property carries a PIT satellite snapshot load-timestamp reference.
  /// </summary>
  SnapshotReference,

  /// <summary>
  /// Property carries a bridge hierarchy traversal depth value.
  /// </summary>
  BridgeDepth,

  /// <summary>
  /// Property carries a multi-active satellite driving-key value.
  /// </summary>
  DrivingKey,
}
