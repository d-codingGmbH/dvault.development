using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies a material reason provider-specific Data Vault read dispatch was not selected.
/// </summary>
public enum DataVaultReadStrategyFallbackCauseKind {
  /// <summary>
  /// The context provider name did not match a provider-specific strategy.
  /// </summary>
  ProviderNameMismatch,

  /// <summary>
  /// The provider name is unknown or has no registered provider capability mapping.
  /// </summary>
  UnknownOrUnregisteredProviderName,

  /// <summary>
  /// No provider-specific read strategy was registered.
  /// </summary>
  NoProviderSpecificStrategyRegistered,

  /// <summary>
  /// The read request targets a satellite parent shape not supported by the provider strategy.
  /// </summary>
  UnsupportedSatelliteParent,

  /// <summary>
  /// The read request targets a multi-active satellite shape not supported by the provider strategy.
  /// </summary>
  MultiActiveSatelliteUnsupported,

  /// <summary>
  /// A custom or unclassified strategy declined the read request.
  /// </summary>
  StrategyDeclined,

  /// <summary>
  /// The read request targets a PIT shape not supported by the provider strategy.
  /// </summary>
  UnsupportedPitShape,

  /// <summary>
  /// The read request targets a bridge shape not supported by the provider strategy.
  /// </summary>
  UnsupportedBridgeShape,

  /// <summary>
  /// The read request is missing complete generated read-model projection evidence required by the provider strategy.
  /// </summary>
  IncompleteReadShapeEvidence,

  /// <summary>
  /// The context carries an observable signal that maintained PIT or bridge rows may be stale for provider strategy dispatch.
  /// </summary>
  StaleReadModelMaintenance,
}
