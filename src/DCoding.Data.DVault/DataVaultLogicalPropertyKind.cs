using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the provider-aware logical property kinds used by the v1 Data Vault EF translator.
/// </summary>
public enum DataVaultLogicalPropertyKind {
  /// <summary>
  /// Data Vault hash key technical value.
  /// </summary>
  HashKey,

  /// <summary>
  /// Data Vault hash diff technical value.
  /// </summary>
  HashDiff,

  /// <summary>
  /// Data Vault load timestamp technical value.
  /// </summary>
  LoadTimestamp,

  /// <summary>
  /// Data Vault record source technical value.
  /// </summary>
  RecordSource,

  /// <summary>
  /// Link participant hash-key reference value.
  /// </summary>
  ParticipantReference,

  /// <summary>
  /// Hub business-key value.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Satellite text payload value.
  /// </summary>
  PayloadText,

  /// <summary>
  /// PIT satellite snapshot load-timestamp reference value.
  /// </summary>
  SatelliteSnapshotReference,

  /// <summary>
  /// Integer hierarchy depth value produced for bridge traversal metadata.
  /// </summary>
  BridgeDepth,

  /// <summary>
  /// Multi-active satellite driving-key value.
  /// </summary>
  DrivingKey,

  /// <summary>
  /// Link dependent child key value.
  /// </summary>
  DependentChildKey,
}
