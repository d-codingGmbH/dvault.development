using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies a material reason provider-specific Data Vault save dispatch was not selected.
/// </summary>
public enum DataVaultSaveStrategyFallbackCauseKind {
  /// <summary>
  /// The context provider name did not match a provider-specific strategy.
  /// </summary>
  ProviderNameMismatch,

  /// <summary>
  /// The provider name is unknown or has no registered provider capability mapping.
  /// </summary>
  UnknownOrUnregisteredProviderName,

  /// <summary>
  /// No provider-specific save strategy was registered.
  /// </summary>
  NoProviderSpecificStrategyRegistered,

  /// <summary>
  /// The Entity Framework change tracker contains pending added, modified, or deleted state.
  /// </summary>
  DirtyDbContext,

  /// <summary>
  /// The save batch contains a multi-active satellite operation.
  /// </summary>
  MultiActiveSatelliteOperations,

  /// <summary>
  /// SQL Server optimized dispatch requires the current minimum total operation count for the request shape.
  /// </summary>
  SqlServerMinimumOperationThreshold,

  /// <summary>
  /// SQL Server optimized dispatch accepts at most 500 satellite operations.
  /// </summary>
  SqlServerMaximumSatelliteOperationThreshold,

  /// <summary>
  /// MySQL optimized dispatch requires the candidate's minimum total operation count.
  /// </summary>
  MySqlMinimumOperationThreshold,

  /// <summary>
  /// Oracle optimized dispatch requires at least 50 total operations.
  /// </summary>
  OracleMinimumOperationThreshold,

  /// <summary>
  /// A custom or unclassified strategy declined the request batch.
  /// </summary>
  StrategyDeclined,

  /// <summary>
  /// Oracle optimized dispatch accepts at most 10000 satellite operations.
  /// </summary>
  OracleMaximumSatelliteOperationThreshold,

  /// <summary>
  /// Staged-provider bulk execution declined because the context contains pending tracked changes.
  /// </summary>
  StagedProviderBulkDirtyDbContext,

  /// <summary>
  /// Staged-provider bulk execution declined because the request batch shape is unsupported.
  /// </summary>
  StagedProviderBulkUnsupportedShape,

  /// <summary>
  /// Staged-provider bulk execution declined because the provider path cannot participate in the caller-owned transaction.
  /// </summary>
  StagedProviderBulkTransactionParticipationUnsupported,

  /// <summary>
  /// Staged-provider bulk execution fell back because transient staging cleanup did not complete safely.
  /// </summary>
  StagedProviderBulkCleanupFailed,

  /// <summary>
  /// Staged-provider bulk execution declined because of a bounded provider limitation.
  /// </summary>
  StagedProviderBulkProviderLimitation,

  /// <summary>
  /// MySQL provider-native dispatch deliberately uses provider-neutral fallback for tiny satellite history batches.
  /// </summary>
  MySqlTinySatelliteHistoryProviderNeutralFallback,

  /// <summary>
  /// MySQL provider-native dispatch deliberately uses provider-neutral fallback for large mixed hub/link batches.
  /// </summary>
  MySqlLargeMixedProviderNeutralFallback,
}
