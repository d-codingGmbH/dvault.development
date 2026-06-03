using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Severity assigned to one Data Vault diagnostics issue.
/// </summary>
public enum DataVaultDiagnosticsIssueSeverity {
  /// <summary>
  /// Informational diagnostic.
  /// </summary>
  Info,

  /// <summary>
  /// Risky but non-blocking diagnostic.
  /// </summary>
  Warning,

  /// <summary>
  /// Blocking validation diagnostic.
  /// </summary>
  Error,
}

/// <summary>
/// Status of request-bound Data Vault save-strategy evaluation.
/// </summary>
public enum DataVaultSaveStrategyDiagnosticsStatus {
  /// <summary>
  /// Strategy evaluation was not requested because no explicit save request batch was supplied.
  /// </summary>
  NotEvaluated,

  /// <summary>
  /// A provider-specific save strategy accepted the supplied context and ordered request batch.
  /// </summary>
  ProviderStrategySelected,

  /// <summary>
  /// No provider-specific save strategy accepted the supplied context and ordered request batch.
  /// </summary>
  ProviderNeutralFallback,
}

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
  /// SQL Server optimized dispatch requires at least 50 total operations.
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
}

/// <summary>
/// Status of request-bound Data Vault read-strategy evaluation.
/// </summary>
public enum DataVaultReadStrategyDiagnosticsStatus {
  /// <summary>
  /// Strategy evaluation was not requested because no read request was supplied.
  /// </summary>
  NotEvaluated,

  /// <summary>
  /// A provider-specific read strategy accepted the supplied context and read request.
  /// </summary>
  ProviderStrategySelected,

  /// <summary>
  /// No provider-specific read strategy accepted the supplied context and read request.
  /// </summary>
  ProviderNeutralFallback,
}

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

/// <summary>
/// Machine-readable issue emitted by Data Vault diagnostics.
/// </summary>
public sealed record DataVaultDiagnosticsIssue(
    DataVaultDiagnosticsIssueSeverity Severity,
    string Code,
    string Message,
    string? Path = null);

/// <summary>
/// Validation section of a Data Vault diagnostics result.
/// </summary>
public sealed record DataVaultValidationDiagnostics(
    bool IsValid,
    IReadOnlyList<DataVaultDiagnosticsIssue> Issues);

/// <summary>
/// Machine-readable explanation of one Data Vault parent metadata reference.
/// </summary>
public sealed record DataVaultParentReferenceExplain(
    DataVaultMetadataReferenceKind Kind,
    string Name);

/// <summary>
/// Machine-readable explanation of one translated Data Vault property.
/// </summary>
public sealed record DataVaultPropertyExplain(
    string Name,
    DataVaultPropertyRole Role,
    TechnicalMetadataColumnRole? TechnicalRole,
    string MetadataName,
    int Ordinal,
    DataVaultLogicalPropertyKind LogicalPropertyKind,
    string ProviderProfileName,
    string StoreType,
    DataVaultProviderValueFormat ValueFormat) {
  /// <summary>
  /// Gets the EF model CLR type name for this translated property.
  /// </summary>
  public string ClrTypeName { get; init; } = string.Empty;

  /// <summary>
  /// Gets a value indicating whether EF marks this translated property nullable.
  /// </summary>
  public bool IsNullable { get; init; }
}

/// <summary>
/// Machine-readable explanation of one provider capability type mapping.
/// </summary>
public sealed record DataVaultProviderTypeMappingExplain(
    DataVaultLogicalPropertyKind LogicalPropertyKind,
    string ModelClrTypeName,
    string StoreType,
    DataVaultProviderValueFormat ValueFormat);

/// <summary>
/// Machine-readable explanation of one translated Data Vault key.
/// </summary>
public sealed record DataVaultKeyExplain(
    string Name,
    IReadOnlyList<string> PropertyNames);

/// <summary>
/// Machine-readable explanation of one translated Data Vault index.
/// </summary>
public sealed record DataVaultIndexExplain(
    string Name,
    IReadOnlyList<string> PropertyNames,
    bool IsUnique,
    IReadOnlyList<string> DescendingPropertyNames,
    IReadOnlyList<string> IncludedPropertyNames);

/// <summary>
/// Machine-readable explanation of one translated Data Vault constraint.
/// </summary>
public sealed record DataVaultConstraintExplain(
    string Name,
    DataVaultConstraintKind Kind,
    IReadOnlyList<string> PropertyNames);

/// <summary>
/// Machine-readable explanation of one translated Data Vault entity/table.
/// </summary>
public sealed record DataVaultEntityExplain(
    string TableName,
    DataVaultTableKind TableKind,
    string MetadataName,
    DataVaultParentReferenceExplain? ParentReference,
    IReadOnlyList<DataVaultPropertyExplain> Properties,
    DataVaultKeyExplain PrimaryKey,
    IReadOnlyList<DataVaultIndexExplain> Indexes,
    IReadOnlyList<DataVaultConstraintExplain> Constraints);

/// <summary>
/// Machine-readable explanation section of a Data Vault diagnostics result.
/// </summary>
public sealed record DataVaultExplainDiagnostics(
    string MetadataSourceKind,
    string? MetadataSourceFingerprint,
    string? ProviderName,
    string CapabilityProfileName,
    bool CapabilityProfileDefaulted,
    DataVaultProviderValueFormat LoadTimestampValueFormat,
    string LoadTimestampStoreType,
    string ProviderBehaviorProfileName,
    bool ProviderBehaviorDefaulted,
    IReadOnlyList<DataVaultEntityExplain> Entities) {
  /// <summary>
  /// Gets the value format used when PIT rows persist satellite snapshot load-timestamp references.
  /// </summary>
  public DataVaultProviderValueFormat SatelliteSnapshotReferenceValueFormat { get; init; } =
      DataVaultProviderValueFormat.Text;

  /// <summary>
  /// Gets the provider store type used when PIT rows persist satellite snapshot load-timestamp references.
  /// </summary>
  public string SatelliteSnapshotReferenceStoreType { get; init; } = string.Empty;

  /// <summary>
  /// Gets the deterministic provider type-mapping facts declared by the selected capability profile.
  /// </summary>
  public IReadOnlyList<DataVaultProviderTypeMappingExplain> TypeMappings { get; init; } =
      Array.Empty<DataVaultProviderTypeMappingExplain>();

  /// <summary>
  /// Gets the provider-specific maximum physical identifier length, if the selected capability profile declares one.
  /// </summary>
  public int? MaximumIdentifierLength { get; init; }

  /// <summary>
  /// Gets a value indicating whether the selected capability profile accepts secondary indexes covered by primary keys.
  /// </summary>
  public bool AllowsIndexesCoveredByPrimaryKey { get; init; } = true;

  /// <summary>
  /// Gets how the selected capability profile projects index include columns without native include-column support.
  /// </summary>
  public DataVaultUnsupportedIncludedIndexColumnMode UnsupportedIncludedIndexColumnMode { get; init; } =
      DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey;

  /// <summary>
  /// Gets the SQL-function posture declared by the selected capability profile.
  /// </summary>
  public DataVaultProviderSqlFunctionSupport SqlFunctionSupport { get; init; } =
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported;

  /// <summary>
  /// Gets the concurrency posture declared by the selected capability profile.
  /// </summary>
  public DataVaultProviderConcurrencySupport ConcurrencySupport { get; init; } =
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported;
}

/// <summary>
/// Machine-readable cause explaining provider-specific save-strategy fallback.
/// </summary>
public sealed record DataVaultSaveStrategyFallbackCause(
    DataVaultSaveStrategyFallbackCauseKind Kind,
    string Message);

/// <summary>
/// Machine-readable declared gate for provider-specific save-strategy eligibility.
/// </summary>
public sealed record DataVaultSaveStrategyGateRequirement(
    DataVaultSaveStrategyFallbackCauseKind Kind,
    int? MinimumTotalOperationCount = null,
    int? MaximumSatelliteOperationCount = null);

/// <summary>
/// Machine-readable diagnostics for one provider-specific save-strategy candidate.
/// </summary>
public sealed record DataVaultSaveStrategyCandidateDiagnostics(
    int Ordinal,
    string StrategyName,
    int Priority,
    bool CanSave,
    IReadOnlyList<DataVaultSaveStrategyFallbackCause> FallbackCauses) {
  /// <summary>
  /// Gets the provider names this candidate declares as eligible, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<string> SupportedProviderNames { get; init; } = Array.Empty<string>();

  /// <summary>
  /// Gets the bounded eligibility gates this candidate declares, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<DataVaultSaveStrategyGateRequirement> GateRequirements { get; init; } =
      Array.Empty<DataVaultSaveStrategyGateRequirement>();

  /// <summary>
  /// Gets bounded staged-provider bulk diagnostics reported by this candidate, when applicable.
  /// </summary>
  public DataVaultStagedProviderBulkDiagnostics? StagedProviderBulk { get; init; }
}

/// <summary>
/// Machine-readable diagnostics for request-bound provider-specific save-strategy dispatch.
/// </summary>
public sealed record DataVaultSaveStrategyDiagnostics(
    DataVaultSaveStrategyDiagnosticsStatus Status,
    string? ProviderName,
    string? SelectedStrategyName,
    int? SelectedStrategyPriority,
    IReadOnlyList<DataVaultSaveStrategyCandidateDiagnostics> Candidates,
    IReadOnlyList<DataVaultSaveStrategyFallbackCause> FallbackCauses) {
  /// <summary>
  /// Gets representative bounded staged-provider bulk diagnostics, when staged evaluation participated in strategy dispatch.
  /// </summary>
  public DataVaultStagedProviderBulkDiagnostics? StagedProviderBulk { get; init; }
}

/// <summary>
/// Machine-readable cause explaining provider-specific read-strategy fallback.
/// </summary>
public sealed record DataVaultReadStrategyFallbackCause(
    DataVaultReadStrategyFallbackCauseKind Kind,
    string Message);

/// <summary>
/// Machine-readable declared gate for provider-specific read-strategy eligibility.
/// </summary>
public sealed record DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind Kind);

/// <summary>
/// Machine-readable diagnostics for one provider-specific read-strategy candidate.
/// </summary>
public sealed record DataVaultReadStrategyCandidateDiagnostics(
    int Ordinal,
    string StrategyName,
    int Priority,
    bool CanRead,
    IReadOnlyList<DataVaultReadStrategyFallbackCause> FallbackCauses) {
  /// <summary>
  /// Gets the provider names this candidate declares as eligible, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<string> SupportedProviderNames { get; init; } = Array.Empty<string>();

  /// <summary>
  /// Gets the bounded eligibility gates this candidate declares, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<DataVaultReadStrategyGateRequirement> GateRequirements { get; init; } =
      Array.Empty<DataVaultReadStrategyGateRequirement>();
}

/// <summary>
/// Machine-readable diagnostics for request-bound provider-specific read-strategy dispatch.
/// </summary>
public sealed record DataVaultReadStrategyDiagnostics(
    DataVaultReadStrategyDiagnosticsStatus Status,
    string? ProviderName,
    string? SelectedStrategyName,
    int? SelectedStrategyPriority,
    IReadOnlyList<DataVaultReadStrategyCandidateDiagnostics> Candidates,
    IReadOnlyList<DataVaultReadStrategyFallbackCause> FallbackCauses);

/// <summary>
/// Identifies the request-bound Data Vault read shape analyzed by diagnostics.
/// </summary>
public enum DataVaultReadShapeKind {
  /// <summary>
  /// Latest or as-of satellite read over one satellite table.
  /// </summary>
  LatestSatellite,

  /// <summary>
  /// PIT-backed as-of read over one maintained PIT table.
  /// </summary>
  PitAsOf,

  /// <summary>
  /// Bridge read over one maintained bridge table.
  /// </summary>
  Bridge,
}

/// <summary>
/// Identifies whether a latest-satellite request is current or as-of bounded.
/// </summary>
public enum DataVaultSatelliteReadSemantics {
  /// <summary>
  /// The request selects current/latest persisted rows without an as-of cutoff.
  /// </summary>
  Current,

  /// <summary>
  /// The request selects rows visible at the supplied as-of cutoff.
  /// </summary>
  AsOf,
}

/// <summary>
/// Machine-readable translated table identity for one read-shape diagnostics target.
/// </summary>
public sealed record DataVaultReadShapeEntity(
    string MetadataName,
    DataVaultTableKind TableKind,
    string TableName);

/// <summary>
/// Machine-readable named set of translated columns used by a read shape.
/// </summary>
public sealed record DataVaultReadShapeColumnSet(
    string Role,
    IReadOnlyList<string> ColumnNames);

/// <summary>
/// Machine-readable expected translated index or key baseline for a read shape.
/// </summary>
public sealed record DataVaultReadShapeIndexBaseline(
    string Name,
    string Kind,
    IReadOnlyList<string> ColumnNames,
    bool IsUnique,
    IReadOnlyList<string> DescendingColumnNames,
    IReadOnlyList<string> IncludedColumnNames);

/// <summary>
/// Provider caveat and fallback facts attached to request-bound read-shape diagnostics.
/// </summary>
public sealed record DataVaultReadShapeProviderDiagnostics(
    string? ProviderName,
    string CapabilityProfileName,
    bool CapabilityProfileDefaulted,
    string ProviderBehaviorProfileName,
    bool ProviderBehaviorDefaulted,
    DataVaultReadStrategyDiagnosticsStatus ReadStrategyStatus,
    IReadOnlyList<DataVaultReadStrategyFallbackCause> ReadStrategyFallbackCauses) {
  /// <summary>
  /// Gets the selected provider-specific read strategy name when a provider strategy accepted the request.
  /// </summary>
  public string? SelectedStrategyName { get; init; }

  /// <summary>
  /// Gets bounded performance-profile recommendation context for this provider/read-shape evaluation.
  /// </summary>
  public DataVaultProviderTuningRecommendation? Recommendation { get; init; }
}

/// <summary>
/// Machine-readable diagnostics for latest/current/as-of satellite read shape.
/// </summary>
public sealed record DataVaultSatelliteReadShapeDiagnostics(
    DataVaultSatelliteReadSemantics Semantics,
    DataVaultReadShapeEntity Satellite,
    DataVaultParentReferenceExplain ParentReference,
    IReadOnlyList<DataVaultReadShapeColumnSet> FilterColumns,
    string SeriesSelectionRule,
    string CutoffRule,
    IReadOnlyList<DataVaultReadShapeColumnSet> DeterministicOrdering,
    IReadOnlyList<DataVaultReadShapeIndexBaseline> ExpectedIndexBaseline) {
  /// <summary>
  /// Gets deterministic projected-column groups for the translated satellite read.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> ProjectedColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();
}

/// <summary>
/// Machine-readable PIT satellite reference facts used by PIT read-shape diagnostics.
/// </summary>
public sealed record DataVaultPitReferencedSatelliteReadShapeDiagnostics(
    string MetadataName,
    string TableName,
    string SnapshotReferenceColumnName,
    string ParentHashKeyColumnName,
    string LoadTimestampColumnName,
    IReadOnlyList<string> DrivingKeyColumnNames);

/// <summary>
/// Machine-readable diagnostics for PIT-backed as-of read shape.
/// </summary>
public sealed record DataVaultPitReadShapeDiagnostics(
    DataVaultReadShapeEntity Pit,
    DataVaultParentReferenceExplain ParentReference,
    IReadOnlyList<DataVaultPitReferencedSatelliteReadShapeDiagnostics> ReferencedSatellites,
    IReadOnlyList<DataVaultReadShapeColumnSet> FilterColumns,
    string PitRowSelectionRule,
    string SnapshotLookupBehavior,
    string NoLatestFallbackBehavior,
    string MaintainedPitPrerequisite,
    IReadOnlyList<DataVaultReadShapeIndexBaseline> ExpectedIndexBaseline) {
  /// <summary>
  /// Gets deterministic projected-column groups for the translated PIT read.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> ProjectedColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();

  /// <summary>
  /// Gets the PIT row identity column groups used for row selection and result disambiguation.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> RowIdentityColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();

  /// <summary>
  /// Gets the number of referenced satellite snapshot lookups required by the PIT read.
  /// </summary>
  public int ReferencedSatelliteLookupCount { get; init; }
}

/// <summary>
/// Machine-readable endpoint facts used by bridge read-shape diagnostics.
/// </summary>
public sealed record DataVaultBridgeEndpointReadShapeDiagnostics(
    DataVaultBridgeTraversalEndpoint Endpoint,
    string EndpointName,
    string ColumnName);

/// <summary>
/// Machine-readable diagnostics for bridge read shape.
/// </summary>
public sealed record DataVaultBridgeReadShapeDiagnostics(
    DataVaultBridgeKind BridgeKind,
    DataVaultReadShapeEntity Bridge,
    IReadOnlyList<DataVaultBridgeEndpointReadShapeDiagnostics> Endpoints,
    DataVaultBridgeTraversalEndpoint FilterEndpoint,
    DataVaultReadShapeColumnSet EndpointFilter,
    DataVaultReadShapeColumnSet? DepthPredicate,
    IReadOnlyList<DataVaultReadShapeColumnSet> DeterministicOrdering,
    IReadOnlyList<string> SupportedEndpointRules,
    IReadOnlyList<DataVaultReadShapeIndexBaseline> ExpectedTraversalIndexBaseline) {
  /// <summary>
  /// Gets deterministic projected-column groups for the translated bridge read.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> ProjectedColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();
}

/// <summary>
/// Machine-readable request-bound Data Vault read/query-shape diagnostics.
/// </summary>
public sealed record DataVaultReadShapeDiagnostics(
    DataVaultReadShapeKind Kind,
    DataVaultReadShapeProviderDiagnostics Provider,
    DataVaultSatelliteReadShapeDiagnostics? Satellite = null,
    DataVaultPitReadShapeDiagnostics? Pit = null,
    DataVaultBridgeReadShapeDiagnostics? Bridge = null);

/// <summary>
/// Closed repository-backed performance-profile category used by provider tuning diagnostics.
/// </summary>
public enum DataVaultPerformanceProfileCategory {
  /// <summary>
  /// The checked-in "Small app-local vault" performance profile.
  /// </summary>
  SmallAppLocalVault,

  /// <summary>
  /// The checked-in "Medium chunked ingestion" performance profile.
  /// </summary>
  MediumChunkedIngestion,

  /// <summary>
  /// The checked-in "Staged provider ingestion" performance profile.
  /// </summary>
  StagedProviderIngestion,

  /// <summary>
  /// The checked-in "Read-model heavy" performance profile.
  /// </summary>
  ReadModelHeavy,
}

/// <summary>
/// Identifies the bounded threshold fact carried by provider tuning diagnostics.
/// </summary>
public enum DataVaultProviderThresholdFactKind {
  /// <summary>
  /// A provider strategy requires at least the specified total operation count.
  /// </summary>
  MinimumTotalOperationCount,

  /// <summary>
  /// A provider strategy accepts at most the specified satellite operation count.
  /// </summary>
  MaximumSatelliteOperationCount,
}

/// <summary>
/// Bounded performance-profile recommendation derived from request-bound provider diagnostics.
/// </summary>
public sealed record DataVaultProviderTuningRecommendation(
    DataVaultPerformanceProfileCategory Category,
    string ProfileName,
    string Message);

/// <summary>
/// Bounded provider threshold fact derived from known provider save-strategy gates.
/// </summary>
public sealed record DataVaultProviderThresholdFact(
    DataVaultProviderThresholdFactKind Kind,
    DataVaultSaveStrategyFallbackCauseKind GateKind,
    string ProviderName,
    string Message) {
  /// <summary>
  /// Gets the minimum total operation count when the threshold is a minimum-operation gate.
  /// </summary>
  public int? MinimumTotalOperationCount { get; init; }

  /// <summary>
  /// Gets the maximum satellite operation count when the threshold is a maximum-satellite gate.
  /// </summary>
  public int? MaximumSatelliteOperationCount { get; init; }
}

/// <summary>
/// Request-bound provider tuning diagnostics for save dispatch.
/// </summary>
public sealed record DataVaultSaveProviderTuningDiagnostics(
    DataVaultProviderTuningRecommendation? Recommendation = null,
    IReadOnlyList<DataVaultProviderThresholdFact>? ThresholdFacts = null);

/// <summary>
/// Request-bound provider tuning diagnostics for read dispatch.
/// </summary>
public sealed record DataVaultReadProviderTuningDiagnostics(
    DataVaultProviderTuningRecommendation? Recommendation = null);

/// <summary>
/// Request-bound provider tuning diagnostics derived from save/read strategy diagnostics.
/// </summary>
public sealed record DataVaultProviderTuningDiagnostics(
    DataVaultSaveProviderTuningDiagnostics? Save = null,
    DataVaultReadProviderTuningDiagnostics? Read = null);

/// <summary>
/// Stable structured Data Vault diagnostics payload.
/// </summary>
public sealed record DataVaultDiagnosticsResult(
    DataVaultValidationDiagnostics Validation,
    DataVaultExplainDiagnostics Explain,
    DataVaultSaveStrategyDiagnostics SaveStrategy,
    IReadOnlyList<DataVaultDiagnosticsIssue> Issues) {
  /// <summary>
  /// Gets request-bound provider-specific read-strategy diagnostics.
  /// </summary>
  public DataVaultReadStrategyDiagnostics ReadStrategy { get; init; } = new(
      DataVaultReadStrategyDiagnosticsStatus.NotEvaluated,
      ProviderName: null,
      SelectedStrategyName: null,
      SelectedStrategyPriority: null,
      Candidates: Array.Empty<DataVaultReadStrategyCandidateDiagnostics>(),
      FallbackCauses: Array.Empty<DataVaultReadStrategyFallbackCause>());

  /// <summary>
  /// Gets request-bound read/query-shape diagnostics for supported Data Vault read requests.
  /// </summary>
  public DataVaultReadShapeDiagnostics? ReadShape { get; init; }

  /// <summary>
  /// Gets request-bound provider tuning diagnostics derived from save/read strategy and read-shape facts.
  /// </summary>
  public DataVaultProviderTuningDiagnostics? ProviderTuning { get; init; }

  /// <summary>
  /// Produces a concise human-readable rendering of the structured diagnostics payload.
  /// </summary>
  public string ToDisplayString() {
    var builder = new StringBuilder();
    builder.Append("DVault diagnostics: ");
    builder.Append(Validation.IsValid ? "valid" : "invalid");
    builder.Append(", capability ");
    builder.Append(Explain.CapabilityProfileName);
    if (Explain.CapabilityProfileDefaulted) {
      builder.Append(" (defaulted)");
    }

    builder.Append(", provider ");
    builder.Append(string.IsNullOrWhiteSpace(Explain.ProviderName) ? "<none>" : Explain.ProviderName);
    builder.Append(", load timestamp ");
    builder.Append(Explain.LoadTimestampValueFormat);
    builder.Append('/');
    builder.Append(Explain.LoadTimestampStoreType);
    builder.Append(", snapshot reference ");
    builder.Append(Explain.SatelliteSnapshotReferenceValueFormat);
    builder.Append('/');
    builder.Append(Explain.SatelliteSnapshotReferenceStoreType);
    builder.Append(", identifier max ");
    builder.Append(Explain.MaximumIdentifierLength.HasValue
        ? Explain.MaximumIdentifierLength.Value.ToString(CultureInfo.InvariantCulture)
        : "<provider-default>");
    builder.Append(", included indexes ");
    builder.Append(Explain.UnsupportedIncludedIndexColumnMode);
    builder.Append(", SQL functions ");
    builder.Append(Explain.SqlFunctionSupport);
    builder.Append(", concurrency ");
    builder.Append(Explain.ConcurrencySupport);
    builder.Append(", provider behavior ");
    builder.Append(Explain.ProviderBehaviorProfileName);
    if (Explain.ProviderBehaviorDefaulted) {
      builder.Append(" (defaulted)");
    }

    builder.Append(", entities ");
    builder.Append(Explain.Entities.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(", save strategy ");
    builder.Append(SaveStrategy.Status.ToString());
    if (!string.IsNullOrWhiteSpace(SaveStrategy.SelectedStrategyName)) {
      builder.Append(" (");
      builder.Append(SaveStrategy.SelectedStrategyName);
      builder.Append(')');
    }

    AppendSaveStrategyDisplayDetails(builder, SaveStrategy);
    builder.Append(", read strategy ");
    builder.Append(ReadStrategy.Status.ToString());
    if (!string.IsNullOrWhiteSpace(ReadStrategy.SelectedStrategyName)) {
      builder.Append(" (");
      builder.Append(ReadStrategy.SelectedStrategyName);
      builder.Append(')');
    }

    AppendReadStrategyDisplayDetails(builder, ReadStrategy);
    if (ReadShape is not null) {
      builder.Append(", read shape ");
      builder.Append(ReadShape.Kind);
      AppendReadShapeDisplayDetails(builder, ReadShape);
    }

    AppendProviderTuningDisplayDetails(builder, ProviderTuning);

    if (Issues.Count > 0) {
      builder.AppendLine();
      foreach (var issue in Issues) {
        builder.Append("- ");
        builder.Append(issue.Severity);
        builder.Append(' ');
        builder.Append(issue.Code);
        builder.Append(": ");
        builder.Append(issue.Message);
        builder.AppendLine();
      }
    }

    return builder.ToString().TrimEnd();
  }

  private static void AppendSaveStrategyDisplayDetails(
      StringBuilder builder,
      DataVaultSaveStrategyDiagnostics strategy) {
    if (strategy.SelectedStrategyPriority.HasValue) {
      builder.Append(", save priority ");
      builder.Append(strategy.SelectedStrategyPriority.Value.ToString(CultureInfo.InvariantCulture));
    }

    builder.Append(", save candidates ");
    builder.Append(strategy.Candidates.Count.ToString(CultureInfo.InvariantCulture));
    if (strategy.FallbackCauses.Count > 0) {
      builder.Append(", save fallback causes ");
      builder.Append(string.Join(", ", strategy.FallbackCauses.Select(cause => cause.Kind.ToString())));
    }

    if (strategy.StagedProviderBulk is not null) {
      builder.Append(", staged provider bulk ");
      builder.Append(strategy.StagedProviderBulk.LifecyclePhase);
      builder.Append(", staged provider caveat ");
      builder.Append(strategy.StagedProviderBulk.ProviderCaveatKind);
      builder.Append(", staged operations ");
      builder.Append(strategy.StagedProviderBulk.OperationCount.ToString(CultureInfo.InvariantCulture));
    }
  }

  private static void AppendReadStrategyDisplayDetails(
      StringBuilder builder,
      DataVaultReadStrategyDiagnostics strategy) {
    if (strategy.SelectedStrategyPriority.HasValue) {
      builder.Append(", read priority ");
      builder.Append(strategy.SelectedStrategyPriority.Value.ToString(CultureInfo.InvariantCulture));
    }

    builder.Append(", read candidates ");
    builder.Append(strategy.Candidates.Count.ToString(CultureInfo.InvariantCulture));
    if (strategy.FallbackCauses.Count > 0) {
      builder.Append(", read fallback causes ");
      builder.Append(string.Join(", ", strategy.FallbackCauses.Select(cause => cause.Kind.ToString())));
    }
  }

  private static void AppendReadShapeDisplayDetails(
      StringBuilder builder,
      DataVaultReadShapeDiagnostics readShape) {
    switch (readShape.Kind) {
      case DataVaultReadShapeKind.LatestSatellite when readShape.Satellite is not null:
        builder.Append(" (");
        builder.Append(readShape.Satellite.Satellite.TableName);
        builder.Append(", ");
        builder.Append(readShape.Satellite.Semantics);
        builder.Append(')');
        return;

      case DataVaultReadShapeKind.PitAsOf when readShape.Pit is not null:
        builder.Append(" (");
        builder.Append(readShape.Pit.Pit.TableName);
        builder.Append(')');
        return;

      case DataVaultReadShapeKind.Bridge when readShape.Bridge is not null:
        builder.Append(" (");
        builder.Append(readShape.Bridge.Bridge.TableName);
        builder.Append(", ");
        builder.Append(readShape.Bridge.FilterEndpoint);
        builder.Append(')');
        return;
    }
  }

  private static void AppendProviderTuningDisplayDetails(
      StringBuilder builder,
      DataVaultProviderTuningDiagnostics? providerTuning) {
    if (providerTuning?.Save?.Recommendation is not null) {
      builder.Append(", save recommendation ");
      builder.Append(providerTuning.Save.Recommendation.Category);
    }

    if (providerTuning?.Read?.Recommendation is not null) {
      builder.Append(", read recommendation ");
      builder.Append(providerTuning.Read.Recommendation.Category);
    }
  }
}

/// <summary>
/// Produces stable Data Vault validation, explain, and request-bound save-strategy diagnostics.
/// </summary>
public interface IDataVaultDiagnosticsService {
  /// <summary>
  /// Analyzes a provider-neutral metadata model using the default SQLite capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(DataVaultMetadataModel metadataModel);

  /// <summary>
  /// Analyzes a provider-neutral metadata model using an explicit provider capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities);

  /// <summary>
  /// Analyzes an immutable metadata registry using the default selected capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(DataVaultMetadataRegistry metadataRegistry);

  /// <summary>
  /// Analyzes an immutable metadata registry using an explicit provider capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataRegistry metadataRegistry,
      DataVaultProviderCapabilityProfile providerCapabilities);

  /// <summary>
  /// Builds and analyzes fluent code-first Data Vault metadata declarations.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(Action<DataVaultCodeFirstModelBuilder> configureModel);

  /// <summary>
  /// Builds and analyzes fluent code-first Data Vault metadata declarations using an explicit provider capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile providerCapabilities);

  /// <summary>
  /// Analyzes the Data Vault metadata already projected on a DbContext without evaluating request-bound strategy dispatch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(DbContext dbContext);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific save-strategy dispatch for one explicit save request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultSaveRequest request);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific save-strategy dispatch for one ordered explicit bulk save request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBulkSaveRequest request);

  /// <summary>
  /// Resolves one registry-backed save request and evaluates provider-specific save-strategy dispatch for the resolved request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistrySaveRequest request);

  /// <summary>
  /// Resolves one registry-backed bulk save request and evaluates provider-specific save-strategy dispatch for the resolved batch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBulkSaveRequest request);
}

/// <summary>
/// Produces request-bound Data Vault read-strategy diagnostics.
/// </summary>
public interface IDataVaultReadDiagnosticsService {
  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific read-strategy dispatch for one latest/as-of satellite request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request);

  /// <summary>
  /// Resolves one registry-backed latest/as-of satellite read request and evaluates provider-specific read-strategy dispatch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryLatestSatelliteReadRequest request);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific read-strategy dispatch for one PIT-backed as-of request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific read-strategy dispatch for one bridge read request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBridgeReadRequest request);

  /// <summary>
  /// Resolves one registry-backed bridge read request and evaluates provider-specific read-strategy dispatch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBridgeReadRequest request);
}

internal sealed class DefaultDataVaultDiagnosticsService : IDataVaultDiagnosticsService, IDataVaultReadDiagnosticsService {
  private static readonly DataVaultSaveStrategyDiagnostics NotEvaluatedStrategy = new(
      DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated,
      ProviderName: null,
      SelectedStrategyName: null,
      SelectedStrategyPriority: null,
      Candidates: Array.Empty<DataVaultSaveStrategyCandidateDiagnostics>(),
      FallbackCauses: Array.Empty<DataVaultSaveStrategyFallbackCause>());
  private static readonly DataVaultReadStrategyDiagnostics NotEvaluatedReadStrategy = new(
      DataVaultReadStrategyDiagnosticsStatus.NotEvaluated,
      ProviderName: null,
      SelectedStrategyName: null,
      SelectedStrategyPriority: null,
      Candidates: Array.Empty<DataVaultReadStrategyCandidateDiagnostics>(),
      FallbackCauses: Array.Empty<DataVaultReadStrategyFallbackCause>());

  private readonly IDataVaultProviderBehaviorSelector _providerBehaviorSelector;
  private readonly IReadOnlyList<IDataVaultProviderBridgeReadStrategy> _providerBridgeReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderPitReadStrategy> _providerPitReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderReadStrategy> _providerReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderSaveStrategy> _providerSaveStrategies;

  public DefaultDataVaultDiagnosticsService(
      IEnumerable<IDataVaultProviderSaveStrategy> providerSaveStrategies,
      IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies,
      IEnumerable<IDataVaultProviderPitReadStrategy> providerPitReadStrategies,
      IEnumerable<IDataVaultProviderBridgeReadStrategy> providerBridgeReadStrategies,
      IDataVaultProviderBehaviorSelector providerBehaviorSelector) {
    ArgumentNullException.ThrowIfNull(providerSaveStrategies);
    ArgumentNullException.ThrowIfNull(providerReadStrategies);
    ArgumentNullException.ThrowIfNull(providerPitReadStrategies);
    ArgumentNullException.ThrowIfNull(providerBridgeReadStrategies);
    ArgumentNullException.ThrowIfNull(providerBehaviorSelector);

    _providerSaveStrategies = providerSaveStrategies.ToArray();
    _providerReadStrategies = providerReadStrategies.ToArray();
    _providerPitReadStrategies = providerPitReadStrategies.ToArray();
    _providerBridgeReadStrategies = providerBridgeReadStrategies.ToArray();
    _providerBehaviorSelector = providerBehaviorSelector;
  }

  public DataVaultDiagnosticsResult Analyze(DataVaultMetadataModel metadataModel) {
    return Analyze(metadataModel, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  public DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(metadataModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return AnalyzeMetadataModel(
        metadataModel,
        providerCapabilities,
        DataVaultMetadataSourceKinds.ModelMetadata,
        DataVaultMetadataSourceAnnotations.CreateFingerprint(metadataModel),
        providerName: null,
        providerBehaviorProfile: DataVaultProviderBehaviorProfiles.ProviderNeutral,
        capabilityProfileDefaulted: false,
        providerBehaviorDefaulted: false);
  }

  public DataVaultDiagnosticsResult Analyze(DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    var providerCapabilities = metadataRegistry.TryGetProviderCapabilityProfile(
        DataVaultProviderCapabilityProfiles.Sqlite.ProfileName,
        out var registryProfile) && registryProfile is not null
        ? registryProfile
        : DataVaultProviderCapabilityProfiles.Sqlite;

    return Analyze(metadataRegistry, providerCapabilities);
  }

  public DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataRegistry metadataRegistry,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return AnalyzeMetadataModel(
        DataVaultMetadataSourceAnnotations.CreateMetadataModel(metadataRegistry),
        providerCapabilities,
        DataVaultMetadataSourceKinds.ModelRegistry,
        DataVaultMetadataSourceAnnotations.CreateFingerprint(metadataRegistry),
        providerName: null,
        providerBehaviorProfile: DataVaultProviderBehaviorProfiles.ProviderNeutral,
        capabilityProfileDefaulted: false,
        providerBehaviorDefaulted: false);
  }

  public DataVaultDiagnosticsResult Analyze(Action<DataVaultCodeFirstModelBuilder> configureModel) {
    return Analyze(configureModel, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  public DataVaultDiagnosticsResult Analyze(
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(configureModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    try {
      var builder = new DataVaultCodeFirstModelBuilder();
      configureModel(builder);
      return AnalyzeMetadataModel(
          builder.BuildMetadataModel(),
          providerCapabilities,
          "code-first",
          sourceFingerprint: null,
          providerName: null,
          providerBehaviorProfile: DataVaultProviderBehaviorProfiles.ProviderNeutral,
          capabilityProfileDefaulted: false,
          providerBehaviorDefaulted: false);
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      return CreateFailureResult(
          "code-first",
          providerCapabilities,
          new DataVaultDiagnosticsIssue(
              DataVaultDiagnosticsIssueSeverity.Error,
              "code-first-validation-failed",
              exception.Message,
              "code-first"));
    }
  }

  public DataVaultDiagnosticsResult Analyze(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: request);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var satellite = DataVaultRegistryMetadataResolver.GetRequiredSatellite(
        registry,
        request.Parent,
        request.SatelliteName);

    return AnalyzeDbContext(
        dbContext,
        requests: null,
        readRequest: new DataVaultLatestSatelliteReadRequest(satellite, request.ParentHashKeys, request.AsOf));
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: request);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: request);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var bridge = DataVaultRegistryMetadataResolver.GetRequiredBridge(registry, request.BridgeName);

    return AnalyzeDbContext(
        dbContext,
        requests: null,
        readRequest: new DataVaultBridgeReadRequest(bridge, request.Endpoint, request.EndpointHashKeys, request.MaximumDepth));
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultSaveRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, [request], readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBulkSaveRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, request.Requests, readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistrySaveRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    return AnalyzeDbContext(
        dbContext,
        [DataVaultSaveServiceRegistryExtensions.ResolveRequest(registry, request)],
        readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBulkSaveRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var requests = request.Requests
        .Select(current => DataVaultSaveServiceRegistryExtensions.ResolveRequest(registry, current))
        .ToArray();

    return AnalyzeDbContext(dbContext, requests, readRequest: null);
  }

  private DataVaultDiagnosticsResult AnalyzeMetadataModel(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities,
      string sourceKind,
      string? sourceFingerprint,
      string? providerName,
      DataVaultProviderBehaviorProfile providerBehaviorProfile,
      bool capabilityProfileDefaulted,
      bool providerBehaviorDefaulted) {
    var validationIssues = ValidateMetadataModel(metadataModel)
        .Concat(ValidateProviderMappings(metadataModel, providerCapabilities))
        .ToArray();
    var issues = validationIssues.ToList();

    ModelBuilder? modelBuilder = null;
    if (!validationIssues.Any(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)) {
      try {
        modelBuilder = new ModelBuilder(new ConventionSet());
        modelBuilder.UseDataVault(providerCapabilities);
        DataVaultEfMetadataTranslator.Apply(modelBuilder, metadataModel, providerCapabilities);
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "metadata-translation-failed",
            exception.Message,
            "explain"));
      }
    }

    var explain = modelBuilder is null
        ? CreateEmptyExplain(
            sourceKind,
            sourceFingerprint,
            providerName,
            providerCapabilities,
            providerBehaviorProfile,
            capabilityProfileDefaulted,
            providerBehaviorDefaulted)
        : CreateExplain(
            modelBuilder.Model,
            sourceKind,
            sourceFingerprint,
            providerName,
            providerCapabilities,
            providerBehaviorProfile,
            capabilityProfileDefaulted,
            providerBehaviorDefaulted);

    return CreateResult(explain, NotEvaluatedStrategy, NotEvaluatedReadStrategy, issues);
  }

  private DataVaultDiagnosticsResult AnalyzeDbContext(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest>? requests,
      object? readRequest) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var providerName = dbContext.Database.ProviderName;
    var capabilityProfile = DataVaultProviderCapabilityProfileSelection.Select(providerName);
    var capabilityProfileDefaulted =
        !string.IsNullOrWhiteSpace(providerName) &&
        !DataVaultProviderCapabilityProfileSelection.TrySelectRegistered(providerName, out _);
    var providerBehavior = _providerBehaviorSelector.SelectBehavior(dbContext);
    var providerBehaviorDefaulted =
        !string.IsNullOrWhiteSpace(providerName) &&
        string.Equals(
            providerBehavior.ProfileName,
            DataVaultProviderBehaviorProfiles.ProviderNeutral.ProfileName,
            StringComparison.Ordinal);
    var issues = new List<DataVaultDiagnosticsIssue>();

    if (capabilityProfileDefaulted) {
      issues.Add(new DataVaultDiagnosticsIssue(
          DataVaultDiagnosticsIssueSeverity.Warning,
          "capability-profile-defaulted",
          "Provider name '" + providerName + "' did not resolve to a registered Data Vault provider capability profile; diagnostics used '" +
          capabilityProfile.ProfileName +
          "'.",
          "capability-profile"));
    }

    if (providerBehaviorDefaulted) {
      issues.Add(new DataVaultDiagnosticsIssue(
          DataVaultDiagnosticsIssueSeverity.Warning,
          "provider-behavior-defaulted",
          "Provider name '" + providerName + "' did not resolve to a provider-specific Data Vault behavior profile; diagnostics used '" +
          providerBehavior.ProfileName +
          "'.",
          "provider-behavior"));
    }

    var extension = DataVaultDbContextMetadataSource.FindExtension(dbContext);
    if (extension is not null) {
      try {
        var source = DataVaultDbContextMetadataSource.Resolve(dbContext, extension);
        issues.AddRange(ValidateMetadataModel(DataVaultMetadataSourceAnnotations.CreateMetadataModel(source.MetadataRegistry)));
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "metadata-source-resolution-failed",
            exception.Message,
            "metadata-source"));
      }
    }

    var explainModel = dbContext.GetService<IDesignTimeModel>().Model;
    var explain = CreateExplain(
        explainModel,
        GetStringAnnotation(explainModel, DataVaultAnnotationNames.MetadataSourceKind) ?? "<model>",
        GetStringAnnotation(explainModel, DataVaultAnnotationNames.MetadataSourceFingerprint),
        providerName,
        capabilityProfile,
        providerBehavior,
        capabilityProfileDefaulted,
        providerBehaviorDefaulted);
    var strategy = requests is null
        ? NotEvaluatedStrategy with { ProviderName = providerName }
        : EvaluateSaveStrategy(dbContext, requests, capabilityProfileDefaulted);
    var readStrategy = readRequest switch {
      null => NotEvaluatedReadStrategy with { ProviderName = providerName },
      DataVaultLatestSatelliteReadRequest latestRequest => EvaluateReadStrategy(dbContext, latestRequest, capabilityProfileDefaulted),
      DataVaultPitAsOfReadRequest pitRequest => EvaluatePitReadStrategy(dbContext, pitRequest, capabilityProfileDefaulted),
      DataVaultBridgeReadRequest bridgeRequest => EvaluateBridgeReadStrategy(dbContext, bridgeRequest, capabilityProfileDefaulted),
      _ => NotEvaluatedReadStrategy with { ProviderName = providerName },
    };
    var readShape = CreateReadShapeDiagnostics(explain, readStrategy, readRequest);
    var providerTuning = CreateProviderTuningDiagnostics(strategy, readStrategy, readShape);

    return CreateResult(explain, strategy, readStrategy, issues, readShape, providerTuning);
  }

  private DataVaultSaveStrategyDiagnostics EvaluateSaveStrategy(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerSaveStrategies
        .Select((strategy, registrationOrdinal) => new SaveStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultSaveStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      var stagedProviderBulk = DataVaultStagedProviderBulkDiagnosticsSupport.TryEvaluate(strategy, dbContext, requests);
      bool canSave;
      IReadOnlyList<DataVaultSaveStrategyFallbackCause> fallbackCauses;
      try {
        canSave = strategy.CanSave(dbContext, requests);
        if (canSave) {
          fallbackCauses = Array.Empty<DataVaultSaveStrategyFallbackCause>();
        }
        else if (DataVaultProviderSaveStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                requests,
                out var evaluation)) {
          fallbackCauses = evaluation.FallbackCauses;
        }
        else {
          var stagedFallbackCauses = DataVaultStagedProviderBulkDiagnosticsSupport.CreateFallbackCauses(stagedProviderBulk);
          fallbackCauses = stagedFallbackCauses.Count > 0
              ? stagedFallbackCauses
              : new[]
              {
                  new DataVaultSaveStrategyFallbackCause(
                      DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
                      "Provider save strategy '" + strategy.GetType().Name + "' declined the request batch."),
              };
        }
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canSave = false;
        var stagedFallbackCauses = DataVaultStagedProviderBulkDiagnosticsSupport.CreateFallbackCauses(stagedProviderBulk);
        fallbackCauses = stagedFallbackCauses.Count > 0
            ? stagedFallbackCauses
            : new[]
            {
                new DataVaultSaveStrategyFallbackCause(
                    DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider save strategy '" + strategy.GetType().Name + "' failed compatibility evaluation."),
            };
      }

      var candidate = new DataVaultSaveStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canSave,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategyGateRequirements(strategy),
        StagedProviderBulk = stagedProviderBulk,
      };
      candidates.Add(candidate);

      if (canSave) {
        var representativeStagedProviderBulk = candidate.StagedProviderBulk ??
            DataVaultStagedProviderBulkDiagnosticsSupport.SelectRepresentative(candidates);
        return new DataVaultSaveStrategyDiagnostics(
            DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultSaveStrategyFallbackCause>()) {
          StagedProviderBulk = representativeStagedProviderBulk,
        };
      }
    }

    var fallbackCauseList = candidates
        .SelectMany(candidate => candidate.FallbackCauses)
        .ToList();

    if (orderedStrategies.Length == 0) {
      fallbackCauseList.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          "No provider-specific Data Vault save strategy is registered."));
    }

    if (capabilityProfileDefaulted &&
        !fallbackCauseList.Any(cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauseList.Insert(0, new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauseList.Count == 0) {
      fallbackCauseList.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
          "Every registered provider-specific Data Vault save strategy declined the request batch."));
    }

    return new DataVaultSaveStrategyDiagnostics(
        DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        candidates,
        DistinctFallbackCauses(fallbackCauseList)) {
      StagedProviderBulk = DataVaultStagedProviderBulkDiagnosticsSupport.SelectRepresentative(candidates),
    };
  }

  private DataVaultReadStrategyDiagnostics EvaluateReadStrategy(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerReadStrategies
        .Select((strategy, registrationOrdinal) => new ReadStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultReadStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      bool canRead;
      IReadOnlyList<DataVaultReadStrategyFallbackCause> fallbackCauses;
      try {
        canRead = strategy.CanReadLatestSatelliteRows(dbContext, request);
        fallbackCauses = canRead
            ? Array.Empty<DataVaultReadStrategyFallbackCause>()
            : DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                request,
                out var evaluation)
                ? evaluation.FallbackCauses
                : [new DataVaultReadStrategyFallbackCause(
                    DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider read strategy '" + strategy.GetType().Name + "' declined the latest/as-of satellite read request.")];
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canRead = false;
        fallbackCauses = [new DataVaultReadStrategyFallbackCause(
            DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
            "Provider read strategy '" + strategy.GetType().Name + "' failed compatibility evaluation.")];
      }

      var candidate = new DataVaultReadStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canRead,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderReadStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderReadStrategyGateEvaluator.GetKnownLatestSatelliteGateRequirements(strategy),
      };
      candidates.Add(candidate);

      if (canRead) {
        return new DataVaultReadStrategyDiagnostics(
            DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultReadStrategyFallbackCause>());
      }
    }

    var fallbackCauseList = candidates
        .SelectMany(candidate => candidate.FallbackCauses)
        .ToList();

    if (orderedStrategies.Length == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          "No provider-specific Data Vault read strategy is registered."));
    }

    if (capabilityProfileDefaulted &&
        !fallbackCauseList.Any(cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauseList.Insert(0, new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauseList.Count == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
          "Every registered provider-specific Data Vault read strategy declined the latest/as-of satellite read request."));
    }

    return new DataVaultReadStrategyDiagnostics(
        DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        candidates,
        DistinctFallbackCauses(fallbackCauseList));
  }

  private DataVaultReadStrategyDiagnostics EvaluatePitReadStrategy(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerPitReadStrategies
        .Select((strategy, registrationOrdinal) => new PitReadStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultReadStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      bool canRead;
      IReadOnlyList<DataVaultReadStrategyFallbackCause> fallbackCauses;
      try {
        canRead = strategy.CanReadPitRows(dbContext, request);
        fallbackCauses = canRead
            ? Array.Empty<DataVaultReadStrategyFallbackCause>()
            : DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                request,
                out var evaluation)
                ? evaluation.FallbackCauses
                : [new DataVaultReadStrategyFallbackCause(
                    DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider read strategy '" + strategy.GetType().Name + "' declined the PIT read request.")];
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canRead = false;
        fallbackCauses = [new DataVaultReadStrategyFallbackCause(
            DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
            "Provider read strategy '" + strategy.GetType().Name + "' failed compatibility evaluation.")];
      }

      var candidate = new DataVaultReadStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canRead,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderReadStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderReadStrategyGateEvaluator.GetKnownPitGateRequirements(strategy),
      };
      candidates.Add(candidate);

      if (canRead) {
        return new DataVaultReadStrategyDiagnostics(
            DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultReadStrategyFallbackCause>());
      }
    }

    return CreateReadFallbackDiagnostics(
        providerName,
        capabilityProfileDefaulted,
        orderedStrategies.Length,
        candidates,
        "No provider-specific Data Vault PIT read strategy is registered.",
        "Every registered provider-specific Data Vault PIT read strategy declined the request.");
  }

  private DataVaultReadStrategyDiagnostics EvaluateBridgeReadStrategy(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerBridgeReadStrategies
        .Select((strategy, registrationOrdinal) => new BridgeReadStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultReadStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      bool canRead;
      IReadOnlyList<DataVaultReadStrategyFallbackCause> fallbackCauses;
      try {
        canRead = strategy.CanReadBridgeRows(dbContext, request);
        fallbackCauses = canRead
            ? Array.Empty<DataVaultReadStrategyFallbackCause>()
            : DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                request,
                out var evaluation)
                ? evaluation.FallbackCauses
                : [new DataVaultReadStrategyFallbackCause(
                    DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider read strategy '" + strategy.GetType().Name + "' declined the bridge read request.")];
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canRead = false;
        fallbackCauses = [new DataVaultReadStrategyFallbackCause(
            DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
            "Provider read strategy '" + strategy.GetType().Name + "' failed compatibility evaluation.")];
      }

      var candidate = new DataVaultReadStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canRead,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderReadStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderReadStrategyGateEvaluator.GetKnownBridgeGateRequirements(strategy),
      };
      candidates.Add(candidate);

      if (canRead) {
        return new DataVaultReadStrategyDiagnostics(
            DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultReadStrategyFallbackCause>());
      }
    }

    return CreateReadFallbackDiagnostics(
        providerName,
        capabilityProfileDefaulted,
        orderedStrategies.Length,
        candidates,
        "No provider-specific Data Vault bridge read strategy is registered.",
        "Every registered provider-specific Data Vault bridge read strategy declined the request.");
  }

  private static DataVaultReadStrategyDiagnostics CreateReadFallbackDiagnostics(
      string? providerName,
      bool capabilityProfileDefaulted,
      int strategyCount,
      IReadOnlyList<DataVaultReadStrategyCandidateDiagnostics> candidates,
      string noStrategyMessage,
      string allDeclinedMessage) {
    var fallbackCauseList = candidates
        .SelectMany(candidate => candidate.FallbackCauses)
        .ToList();

    if (strategyCount == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          noStrategyMessage));
    }

    if (capabilityProfileDefaulted &&
        !fallbackCauseList.Any(cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauseList.Insert(0, new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauseList.Count == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
          allDeclinedMessage));
    }

    return new DataVaultReadStrategyDiagnostics(
        DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        candidates,
        DistinctFallbackCauses(fallbackCauseList));
  }

  private static DataVaultReadShapeDiagnostics? CreateReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultReadStrategyDiagnostics readStrategy,
      object? readRequest) {
    return readRequest switch {
      DataVaultLatestSatelliteReadRequest latestRequest => new DataVaultReadShapeDiagnostics(
          DataVaultReadShapeKind.LatestSatellite,
          CreateReadShapeProviderDiagnostics(explain, readStrategy, DataVaultReadShapeKind.LatestSatellite),
          Satellite: CreateSatelliteReadShapeDiagnostics(explain, latestRequest)),
      DataVaultPitAsOfReadRequest pitRequest => new DataVaultReadShapeDiagnostics(
          DataVaultReadShapeKind.PitAsOf,
          CreateReadShapeProviderDiagnostics(explain, readStrategy, DataVaultReadShapeKind.PitAsOf),
          Pit: CreatePitReadShapeDiagnostics(explain, pitRequest)),
      DataVaultBridgeReadRequest bridgeRequest => new DataVaultReadShapeDiagnostics(
          DataVaultReadShapeKind.Bridge,
          CreateReadShapeProviderDiagnostics(explain, readStrategy, DataVaultReadShapeKind.Bridge),
          Bridge: CreateBridgeReadShapeDiagnostics(explain, bridgeRequest)),
      _ => null,
    };
  }

  private static DataVaultReadShapeProviderDiagnostics CreateReadShapeProviderDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultReadStrategyDiagnostics readStrategy,
      DataVaultReadShapeKind readShapeKind) {
    return new DataVaultReadShapeProviderDiagnostics(
        readStrategy.ProviderName ?? explain.ProviderName,
        explain.CapabilityProfileName,
        explain.CapabilityProfileDefaulted,
        explain.ProviderBehaviorProfileName,
        explain.ProviderBehaviorDefaulted,
        readStrategy.Status,
        readStrategy.FallbackCauses) {
      SelectedStrategyName = readStrategy.SelectedStrategyName,
      Recommendation = CreateReadProviderTuningRecommendation(readStrategy, readShapeKind),
    };
  }

  private static DataVaultProviderTuningDiagnostics? CreateProviderTuningDiagnostics(
      DataVaultSaveStrategyDiagnostics saveStrategy,
      DataVaultReadStrategyDiagnostics readStrategy,
      DataVaultReadShapeDiagnostics? readShape) {
    var save = CreateSaveProviderTuningDiagnostics(saveStrategy);
    var read = readShape is null ? null : CreateReadProviderTuningDiagnostics(readStrategy, readShape.Kind);

    return save is null && read is null
        ? null
        : new DataVaultProviderTuningDiagnostics(save, read);
  }

  private static DataVaultSaveProviderTuningDiagnostics? CreateSaveProviderTuningDiagnostics(
      DataVaultSaveStrategyDiagnostics strategy) {
    if (strategy.Status == DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated) {
      return null;
    }

    var thresholdFacts = CreateSaveProviderThresholdFacts(strategy);
    return new DataVaultSaveProviderTuningDiagnostics(
        CreateSaveProviderTuningRecommendation(strategy),
        thresholdFacts.Count == 0 ? null : thresholdFacts);
  }

  private static DataVaultReadProviderTuningDiagnostics? CreateReadProviderTuningDiagnostics(
      DataVaultReadStrategyDiagnostics strategy,
      DataVaultReadShapeKind readShapeKind) {
    if (strategy.Status == DataVaultReadStrategyDiagnosticsStatus.NotEvaluated) {
      return null;
    }

    return new DataVaultReadProviderTuningDiagnostics(
        CreateReadProviderTuningRecommendation(strategy, readShapeKind));
  }

  private static DataVaultProviderTuningRecommendation CreateSaveProviderTuningRecommendation(
      DataVaultSaveStrategyDiagnostics strategy) {
    if (strategy.Status == DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected &&
        !string.Equals(strategy.SelectedStrategyName, "SqliteDataVaultSaveStrategy", StringComparison.Ordinal)) {
      return new DataVaultProviderTuningRecommendation(
          DataVaultPerformanceProfileCategory.StagedProviderIngestion,
          "Staged provider ingestion",
          "Provider-specific save diagnostics selected an eligible ordered bulk path; keep the context clean and verify provider-local benchmark evidence before claiming provider-native ingestion behavior.");
    }

    if (strategy.Status == DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback &&
        strategy.Candidates.Any(candidate => candidate.SupportedProviderNames.Count > 0)) {
      return new DataVaultProviderTuningRecommendation(
          DataVaultPerformanceProfileCategory.StagedProviderIngestion,
          "Staged provider ingestion",
          "Provider-specific save diagnostics evaluated registered candidates but fell back; use the reported gates, fallback causes, and threshold facts before claiming provider-native ingestion behavior.");
    }

    return new DataVaultProviderTuningRecommendation(
        DataVaultPerformanceProfileCategory.SmallAppLocalVault,
        "Small app-local vault",
        "Save diagnostics are provider-neutral or SQLite-selected; use the small app-local vault profile until provider-specific eligibility and local evidence justify a wider ingestion profile.");
  }

  private static DataVaultProviderTuningRecommendation CreateReadProviderTuningRecommendation(
      DataVaultReadStrategyDiagnostics strategy,
      DataVaultReadShapeKind readShapeKind) {
    if (strategy.Status == DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected &&
        IsRepositoryProvenOptimizedReadStrategy(strategy.SelectedStrategyName, readShapeKind)) {
      return new DataVaultProviderTuningRecommendation(
          DataVaultPerformanceProfileCategory.ReadModelHeavy,
          "Read-model heavy",
          "Read diagnostics selected the repository-proven " +
          FormatOptimizedReadProviderName(strategy.SelectedStrategyName) +
          " optimized path for " +
          readShapeKind +
          "; keep PIT and bridge rows maintained when those shapes are used.");
    }

    return new DataVaultProviderTuningRecommendation(
        DataVaultPerformanceProfileCategory.ReadModelHeavy,
        "Read-model heavy",
        "Read diagnostics provide provider-neutral " +
        readShapeKind +
        " guidance; SQLite remains the repository-proven optimized latest-satellite provider, while SQLite, PostgreSQL, and SQL Server are repository-proven optimized PIT/bridge providers when diagnostics select their candidates. Unsupported providers, unsupported shapes, or incomplete read-shape evidence remain fallback guidance.");
  }

  private static bool IsRepositoryProvenOptimizedReadStrategy(
      string? selectedStrategyName,
      DataVaultReadShapeKind readShapeKind) {
    return selectedStrategyName switch {
      "SqliteDataVaultReadStrategy" => true,
      "PostgresDataVaultReadStrategy" => readShapeKind is DataVaultReadShapeKind.PitAsOf or DataVaultReadShapeKind.Bridge,
      "SqlServerDataVaultReadStrategy" => readShapeKind is DataVaultReadShapeKind.PitAsOf or DataVaultReadShapeKind.Bridge,
      _ => false,
    };
  }

  private static string FormatOptimizedReadProviderName(string? selectedStrategyName) {
    return selectedStrategyName switch {
      "SqliteDataVaultReadStrategy" => "SQLite",
      "PostgresDataVaultReadStrategy" => "PostgreSQL",
      "SqlServerDataVaultReadStrategy" => "SQL Server",
      _ => "provider-specific",
    };
  }

  private static IReadOnlyList<DataVaultProviderThresholdFact> CreateSaveProviderThresholdFacts(
      DataVaultSaveStrategyDiagnostics strategy) {
    var facts = new List<DataVaultProviderThresholdFact>();
    var keys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var candidate in strategy.Candidates.OrderBy(candidate => candidate.Ordinal)) {
      foreach (var requirement in candidate.GateRequirements) {
        if (!requirement.MinimumTotalOperationCount.HasValue && !requirement.MaximumSatelliteOperationCount.HasValue) {
          continue;
        }

        foreach (var providerName in candidate.SupportedProviderNames) {
          var fact = CreateSaveProviderThresholdFact(candidate.StrategyName, providerName, requirement);
          var key = fact.Kind +
              "\u001f" +
              fact.GateKind +
              "\u001f" +
              fact.ProviderName +
              "\u001f" +
              fact.MinimumTotalOperationCount?.ToString(CultureInfo.InvariantCulture) +
              "\u001f" +
              fact.MaximumSatelliteOperationCount?.ToString(CultureInfo.InvariantCulture);
          if (keys.Add(key)) {
            facts.Add(fact);
          }
        }
      }
    }

    return facts;
  }

  private static DataVaultProviderThresholdFact CreateSaveProviderThresholdFact(
      string strategyName,
      string providerName,
      DataVaultSaveStrategyGateRequirement requirement) {
    if (requirement.MinimumTotalOperationCount.HasValue) {
      var minimum = requirement.MinimumTotalOperationCount.Value;
      return new DataVaultProviderThresholdFact(
          DataVaultProviderThresholdFactKind.MinimumTotalOperationCount,
          requirement.Kind,
          providerName,
          FormatSaveStrategyDisplayName(strategyName) +
          " optimized dispatch requires at least " +
          minimum.ToString(CultureInfo.InvariantCulture) +
          " total operations.") {
        MinimumTotalOperationCount = minimum,
      };
    }

    var maximum = requirement.MaximumSatelliteOperationCount.GetValueOrDefault();
    return new DataVaultProviderThresholdFact(
        DataVaultProviderThresholdFactKind.MaximumSatelliteOperationCount,
        requirement.Kind,
        providerName,
        FormatSaveStrategyDisplayName(strategyName) +
        " optimized dispatch accepts at most " +
        maximum.ToString(CultureInfo.InvariantCulture) +
        " satellite operations.") {
      MaximumSatelliteOperationCount = maximum,
    };
  }

  private static string FormatSaveStrategyDisplayName(string strategyName) {
    return strategyName switch {
      "SqlServerDataVaultSaveStrategy" => "SQL Server",
      "MySqlStagedDataVaultSaveStrategy" => "MySQL staged bulk",
      "MySqlDataVaultSaveStrategy" => "MySQL",
      "OracleDataVaultSaveStrategy" => "Oracle",
      "PostgresDataVaultSaveStrategy" => "PostgreSQL",
      "SqliteDataVaultSaveStrategy" => "SQLite",
      _ => strategyName,
    };
  }

  private static DataVaultSatelliteReadShapeDiagnostics CreateSatelliteReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultLatestSatelliteReadRequest request) {
    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(request.Satellite);
    var entity = FindEntityExplain(
        explain,
        DataVaultTableKind.Satellite,
        request.Satellite.Name,
        projection.TableName);
    var filterColumns = new List<DataVaultReadShapeColumnSet>
    {
        new("parentHashKeyFilter", [projection.ParentHashKeyColumnName]),
    };
    if (request.AsOf.HasValue) {
      filterColumns.Add(new DataVaultReadShapeColumnSet("asOfCutoff", [projection.LoadTimestampColumnName]));
    }

    var orderingColumns = new[]
    {
        projection.ParentHashKeyColumnName,
    }
        .Concat(projection.DrivingKeyColumnNames)
        .ToArray();

    return new DataVaultSatelliteReadShapeDiagnostics(
        request.AsOf.HasValue
            ? DataVaultSatelliteReadSemantics.AsOf
            : DataVaultSatelliteReadSemantics.Current,
        new DataVaultReadShapeEntity(request.Satellite.Name, DataVaultTableKind.Satellite, projection.TableName),
        new DataVaultParentReferenceExplain(request.Satellite.Parent.Kind, request.Satellite.Parent.Name),
        filterColumns,
        "Select the latest load timestamp per parent hash key and driving-key series.",
        request.AsOf.HasValue
            ? "Apply " + projection.LoadTimestampColumnName + " <= supplied as-of cutoff; the cutoff value is not included in diagnostics."
            : "No as-of cutoff is applied; current reads consider all persisted satellite rows.",
        [new DataVaultReadShapeColumnSet("resultOrdering", orderingColumns)],
        CreateIndexBaseline(entity)) {
      ProjectedColumns = CreateSatelliteProjectedColumns(projection),
    };
  }

  private static DataVaultPitReadShapeDiagnostics CreatePitReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultPitAsOfReadRequest request) {
    var pit = request.Pit;
    var tableName = GetPitTableName(pit.Name);
    var parentHashKeyColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, pit.Parent.Name, tableName));
    var loadTimestampColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, pit.Name, tableName));
    var entity = FindEntityExplain(explain, DataVaultTableKind.Pit, pit.Name, tableName);
    var pitDrivingKeyColumnNames = FindPropertyColumnNames(entity, DataVaultPropertyRole.DrivingKey);
    var snapshotColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        pit.Satellites.Select(satellite => satellite.SatelliteName + " Load Timestamp"),
        [parentHashKeyColumnName, .. pitDrivingKeyColumnNames, loadTimestampColumnName]);
    var referencedSatellites = pit.Satellites
        .Select((satellite, index) => {
          var satelliteTableName = DefaultDataVaultNamingPolicy.Instance.GetSatelliteTableName(
              new DataVaultSatelliteNameContext(pit.Parent.Name, satellite.SatelliteName));
          var satelliteParentHashKeyColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
              new DataVaultTechnicalColumnNameContext(
                  DataVaultTechnicalColumnKind.HashKey,
                  pit.Parent.Name,
                  satelliteTableName));
          var satelliteLoadTimestampColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
              new DataVaultTechnicalColumnNameContext(
                  DataVaultTechnicalColumnKind.LoadTimestamp,
                  satellite.SatelliteName,
                  satelliteTableName));
          var satelliteEntity = FindEntityExplain(
              explain,
              DataVaultTableKind.Satellite,
              satellite.SatelliteName,
              satelliteTableName);

          return new DataVaultPitReferencedSatelliteReadShapeDiagnostics(
              satellite.SatelliteName,
              satelliteTableName,
              snapshotColumnNames[index],
              satelliteParentHashKeyColumnName,
              satelliteLoadTimestampColumnName,
              FindPropertyColumnNames(satelliteEntity, DataVaultPropertyRole.DrivingKey));
        })
        .ToArray();
    var rowIdentityColumns = new[]
    {
        parentHashKeyColumnName,
    }
        .Concat(pitDrivingKeyColumnNames)
        .Append(loadTimestampColumnName)
        .ToArray();

    return new DataVaultPitReadShapeDiagnostics(
        new DataVaultReadShapeEntity(pit.Name, DataVaultTableKind.Pit, tableName),
        new DataVaultParentReferenceExplain(pit.Parent.Kind, pit.Parent.Name),
        referencedSatellites,
        [
            new DataVaultReadShapeColumnSet("parentHashKeyFilter", [parentHashKeyColumnName]),
            new DataVaultReadShapeColumnSet("asOfCutoff", [loadTimestampColumnName]),
        ],
        pitDrivingKeyColumnNames.Count == 0
            ? "Select the latest PIT row per parent hash key with " + loadTimestampColumnName + " <= supplied as-of cutoff."
            : "Select the latest PIT row per parent hash key and driving-key tuple with " + loadTimestampColumnName + " <= supplied as-of cutoff.",
        pitDrivingKeyColumnNames.Count == 0
            ? "Resolve each satellite snapshot by parent hash key and the snapshot load-timestamp reference stored on the selected PIT row."
            : "Resolve ordinary satellite snapshots by parent hash key and multi-active satellite snapshots by parent hash key, driving-key tuple, and the snapshot load-timestamp reference stored on the selected PIT row.",
        "Missing PIT rows or null satellite snapshot references yield no latest-satellite fallback.",
        "PIT rows must already be maintained; diagnostics and reads do not rebuild or refresh PIT tables.",
        CreateIndexBaseline(entity)) {
      ProjectedColumns = CreatePitProjectedColumns(
          explain,
          pit,
          parentHashKeyColumnName,
          pitDrivingKeyColumnNames,
          loadTimestampColumnName,
          referencedSatellites),
      RowIdentityColumns = [new DataVaultReadShapeColumnSet("pitRowIdentity", rowIdentityColumns)],
      ReferencedSatelliteLookupCount = referencedSatellites.Length,
    };
  }

  private static DataVaultBridgeReadShapeDiagnostics CreateBridgeReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultBridgeReadRequest request) {
    var bridge = request.Bridge;
    var tableName = GetBridgeTableName(bridge);
    var endpoints = bridge.Endpoints
        .Select(endpoint => new DataVaultBridgeEndpointReadShapeDiagnostics(
            ToPublicEndpoint(endpoint.Role),
            endpoint.SourceEndpointName,
            GetBridgeEndpointHashKeyColumnName(endpoint)))
        .ToArray();
    var filterEndpoint = endpoints.Single(endpoint => endpoint.Endpoint == request.Endpoint);
    var entity = FindEntityExplain(explain, DataVaultTableKind.Bridge, bridge.Name, tableName);
    var orderingColumns = request.MaximumDepth.HasValue
        ? endpoints.Select(endpoint => endpoint.ColumnName).Append(DataVaultBridgeProjectionRow.TraversalDepthName).ToArray()
        : endpoints.Select(endpoint => endpoint.ColumnName).ToArray();

    return new DataVaultBridgeReadShapeDiagnostics(
        bridge.Kind,
        new DataVaultReadShapeEntity(bridge.Name, DataVaultTableKind.Bridge, tableName),
        endpoints,
        request.Endpoint,
        new DataVaultReadShapeColumnSet("endpointHashKeyFilter", [filterEndpoint.ColumnName]),
        request.MaximumDepth.HasValue
            ? new DataVaultReadShapeColumnSet("maximumDepthPredicate", [DataVaultBridgeProjectionRow.TraversalDepthName])
            : null,
        [new DataVaultReadShapeColumnSet("resultOrdering", orderingColumns)],
        GetSupportedBridgeEndpointRules(bridge.Kind),
        CreateIndexBaseline(entity)) {
      ProjectedColumns = CreateBridgeProjectedColumns(endpoints, request.MaximumDepth.HasValue),
    };
  }

  private static IReadOnlyList<DataVaultReadShapeColumnSet> CreateSatelliteProjectedColumns(
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection) {
    var columnSets = new List<DataVaultReadShapeColumnSet>
    {
        new(
            "technicalProjection",
            [
                projection.ParentHashKeyColumnName,
                projection.HashDiffColumnName,
                projection.LoadTimestampColumnName,
                projection.RecordSourceColumnName,
            ]),
        new("payloadProjection", projection.PayloadColumnNames),
    };

    if (projection.DrivingKeyColumnNames.Count > 0) {
      columnSets.Add(new DataVaultReadShapeColumnSet("drivingKeyProjection", projection.DrivingKeyColumnNames));
    }

    return columnSets;
  }

  private static IReadOnlyList<DataVaultReadShapeColumnSet> CreatePitProjectedColumns(
      DataVaultExplainDiagnostics explain,
      DataVaultPitMetadata pit,
      string parentHashKeyColumnName,
      IReadOnlyList<string> pitDrivingKeyColumnNames,
      string loadTimestampColumnName,
      IReadOnlyList<DataVaultPitReferencedSatelliteReadShapeDiagnostics> referencedSatellites) {
    var columnSets = new List<DataVaultReadShapeColumnSet>
    {
        new DataVaultReadShapeColumnSet(
            "pitTechnicalProjection",
            [
                parentHashKeyColumnName,
                loadTimestampColumnName,
            ]),
    };

    if (pitDrivingKeyColumnNames.Count > 0) {
      columnSets.Add(new DataVaultReadShapeColumnSet("pitDrivingKeyProjection", pitDrivingKeyColumnNames));
    }

    columnSets.AddRange(
    [
        new DataVaultReadShapeColumnSet(
            "snapshotReferenceProjection",
            referencedSatellites.Select(satellite => satellite.SnapshotReferenceColumnName).ToArray()),
        new DataVaultReadShapeColumnSet(
            "satellitePayloadProjection",
            CreatePitSatellitePayloadProjectionColumns(explain, pit)),
    ]);

    return columnSets;
  }

  private static IReadOnlyList<string> FindPropertyColumnNames(
      DataVaultEntityExplain? entity,
      DataVaultPropertyRole role) {
    return entity?.Properties
        .Where(property => property.Role == role)
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.Name, StringComparer.Ordinal)
        .Select(property => property.Name)
        .ToArray() ?? Array.Empty<string>();
  }

  private static IReadOnlyList<string> CreatePitSatellitePayloadProjectionColumns(
      DataVaultExplainDiagnostics explain,
      DataVaultPitMetadata pit) {
    return pit.Satellites
        .SelectMany(satellite => FindSatellitePayloadColumnNames(explain, pit.Parent, satellite.SatelliteName))
        .ToArray();
  }

  private static IReadOnlyList<string> FindSatellitePayloadColumnNames(
      DataVaultExplainDiagnostics explain,
      DataVaultMetadataReference parent,
      string satelliteName) {
    return explain.Entities
        .Where(entity =>
            entity.TableKind == DataVaultTableKind.Satellite &&
            string.Equals(entity.MetadataName, satelliteName, StringComparison.Ordinal) &&
            entity.ParentReference is not null &&
            entity.ParentReference.Kind == parent.Kind &&
            string.Equals(entity.ParentReference.Name, parent.Name, StringComparison.Ordinal))
        .SelectMany(entity => entity.Properties
            .Where(property => property.Role == DataVaultPropertyRole.Payload)
            .Select(property => property.Name))
        .ToArray();
  }

  private static IReadOnlyList<DataVaultReadShapeColumnSet> CreateBridgeProjectedColumns(
      IReadOnlyList<DataVaultBridgeEndpointReadShapeDiagnostics> endpoints,
      bool includeDepthProjection) {
    var columnSets = new List<DataVaultReadShapeColumnSet>
    {
        new("endpointProjection", endpoints.Select(endpoint => endpoint.ColumnName).ToArray()),
    };

    if (includeDepthProjection) {
      columnSets.Add(new DataVaultReadShapeColumnSet("depthProjection", [DataVaultBridgeProjectionRow.TraversalDepthName]));
    }

    return columnSets;
  }

  private static DataVaultEntityExplain? FindEntityExplain(
      DataVaultExplainDiagnostics explain,
      DataVaultTableKind tableKind,
      string metadataName,
      string tableName) {
    return explain.Entities.FirstOrDefault(entity =>
        entity.TableKind == tableKind &&
        string.Equals(entity.MetadataName, metadataName, StringComparison.Ordinal) &&
        string.Equals(entity.TableName, tableName, StringComparison.Ordinal));
  }

  private static IReadOnlyList<DataVaultReadShapeIndexBaseline> CreateIndexBaseline(
      DataVaultEntityExplain? entity) {
    if (entity is null) {
      return Array.Empty<DataVaultReadShapeIndexBaseline>();
    }

    var baselines = new List<DataVaultReadShapeIndexBaseline>();
    if (!string.Equals(entity.PrimaryKey.Name, "<none>", StringComparison.Ordinal)) {
      baselines.Add(new DataVaultReadShapeIndexBaseline(
          entity.PrimaryKey.Name,
          "primary-key",
          entity.PrimaryKey.PropertyNames,
          IsUnique: true,
          DescendingColumnNames: Array.Empty<string>(),
          IncludedColumnNames: Array.Empty<string>()));
    }

    baselines.AddRange(entity.Indexes.Select(index => new DataVaultReadShapeIndexBaseline(
        index.Name,
        "secondary-index",
        index.PropertyNames,
        index.IsUnique,
        index.DescendingPropertyNames,
        index.IncludedPropertyNames)));

    return baselines;
  }

  private static IReadOnlyList<string> GetSupportedBridgeEndpointRules(DataVaultBridgeKind bridgeKind) {
    return bridgeKind switch {
      DataVaultBridgeKind.ManyToMany => [
          "Many-to-many bridge reads support From and To endpoint filters.",
      ],
      DataVaultBridgeKind.Hierarchy => [
          "Hierarchy bridge reads support Ancestor and Descendant endpoint filters.",
          "Hierarchy bridge reads require a bounded maximumDepth predicate.",
      ],
      _ => Array.Empty<string>(),
    };
  }

  private static DataVaultBridgeTraversalEndpoint ToPublicEndpoint(DataVaultBridgeEndpointRole endpointRole) {
    return endpointRole switch {
      DataVaultBridgeEndpointRole.From => DataVaultBridgeTraversalEndpoint.From,
      DataVaultBridgeEndpointRole.To => DataVaultBridgeTraversalEndpoint.To,
      DataVaultBridgeEndpointRole.Ancestor => DataVaultBridgeTraversalEndpoint.Ancestor,
      DataVaultBridgeEndpointRole.Descendant => DataVaultBridgeTraversalEndpoint.Descendant,
      _ => throw new ArgumentOutOfRangeException(nameof(endpointRole), endpointRole, "Unsupported bridge endpoint role."),
    };
  }

  private static string GetPitTableName(string pitName) {
    return "Pit" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(pitName);
  }

  private static string GetBridgeTableName(DataVaultBridgeMetadata bridge) {
    return "Bridge" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(bridge.Name);
  }

  private static string GetBridgeEndpointHashKeyColumnName(DataVaultBridgeEndpointMetadata endpoint) {
    var baseName = endpoint.Role switch {
      DataVaultBridgeEndpointRole.Ancestor => "Ancestor" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      DataVaultBridgeEndpointRole.Descendant => "Descendant" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      _ => endpoint.HubReference.Name,
    };

    return DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(baseName) + "HashKey";
  }

  private static DataVaultDiagnosticsResult CreateResult(
      DataVaultExplainDiagnostics explain,
      DataVaultSaveStrategyDiagnostics strategy,
      DataVaultReadStrategyDiagnostics readStrategy,
      IReadOnlyList<DataVaultDiagnosticsIssue> issues,
      DataVaultReadShapeDiagnostics? readShape = null,
      DataVaultProviderTuningDiagnostics? providerTuning = null) {
    var issueArray = issues.ToArray();
    var validationIssues = issueArray
        .Where(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)
        .ToArray();
    var validation = new DataVaultValidationDiagnostics(validationIssues.Length == 0, validationIssues);

    return new DataVaultDiagnosticsResult(validation, explain, strategy, issueArray) {
      ReadStrategy = readStrategy,
      ReadShape = readShape,
      ProviderTuning = providerTuning,
    };
  }

  private static DataVaultDiagnosticsResult CreateFailureResult(
      string sourceKind,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultDiagnosticsIssue issue) {
    return CreateResult(
        CreateEmptyExplain(
            sourceKind,
            sourceFingerprint: null,
            providerName: null,
            providerCapabilities,
            DataVaultProviderBehaviorProfiles.ProviderNeutral,
            capabilityProfileDefaulted: false,
            providerBehaviorDefaulted: false),
        NotEvaluatedStrategy,
        NotEvaluatedReadStrategy,
        [issue]);
  }

  private static DataVaultExplainDiagnostics CreateExplain(
      IReadOnlyModel model,
      string sourceKind,
      string? sourceFingerprint,
      string? providerName,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultProviderBehaviorProfile providerBehaviorProfile,
      bool capabilityProfileDefaulted,
      bool providerBehaviorDefaulted) {
    var loadTimestampMapping = GetLoadTimestampMapping(providerCapabilities);
    var satelliteSnapshotReferenceMapping = GetSatelliteSnapshotReferenceMapping(providerCapabilities);
    var entities = model
        .GetEntityTypes()
        .Where(IsDataVaultEntity)
        .Select(CreateEntityExplain)
        .OrderBy(entity => GetEntityKindSortKey(entity.TableKind))
        .ThenBy(entity => entity.MetadataName, StringComparer.Ordinal)
        .ThenBy(entity => entity.TableName, StringComparer.Ordinal)
        .ToArray();

    return new DataVaultExplainDiagnostics(
        sourceKind,
        sourceFingerprint,
        providerName,
        providerCapabilities.ProfileName,
        capabilityProfileDefaulted,
        loadTimestampMapping.ValueFormat,
        loadTimestampMapping.NativeStoreType,
        providerBehaviorProfile.ProfileName,
        providerBehaviorDefaulted,
        entities) {
      SatelliteSnapshotReferenceValueFormat = satelliteSnapshotReferenceMapping.ValueFormat,
      SatelliteSnapshotReferenceStoreType = satelliteSnapshotReferenceMapping.NativeStoreType,
      TypeMappings = CreateTypeMappingExplain(providerCapabilities),
      MaximumIdentifierLength = providerCapabilities.MaximumIdentifierLength,
      AllowsIndexesCoveredByPrimaryKey = providerCapabilities.AllowsIndexesCoveredByPrimaryKey,
      UnsupportedIncludedIndexColumnMode = providerCapabilities.UnsupportedIncludedIndexColumnMode,
      SqlFunctionSupport = providerCapabilities.SqlFunctionSupport,
      ConcurrencySupport = providerCapabilities.ConcurrencySupport,
    };
  }

  private static DataVaultExplainDiagnostics CreateEmptyExplain(
      string sourceKind,
      string? sourceFingerprint,
      string? providerName,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultProviderBehaviorProfile providerBehaviorProfile,
      bool capabilityProfileDefaulted,
      bool providerBehaviorDefaulted) {
    var loadTimestampMapping = GetLoadTimestampMapping(providerCapabilities);
    var satelliteSnapshotReferenceMapping = GetSatelliteSnapshotReferenceMapping(providerCapabilities);
    return new DataVaultExplainDiagnostics(
        sourceKind,
        sourceFingerprint,
        providerName,
        providerCapabilities.ProfileName,
        capabilityProfileDefaulted,
        loadTimestampMapping.ValueFormat,
        loadTimestampMapping.NativeStoreType,
        providerBehaviorProfile.ProfileName,
        providerBehaviorDefaulted,
        Array.Empty<DataVaultEntityExplain>()) {
      SatelliteSnapshotReferenceValueFormat = satelliteSnapshotReferenceMapping.ValueFormat,
      SatelliteSnapshotReferenceStoreType = satelliteSnapshotReferenceMapping.NativeStoreType,
      TypeMappings = CreateTypeMappingExplain(providerCapabilities),
      MaximumIdentifierLength = providerCapabilities.MaximumIdentifierLength,
      AllowsIndexesCoveredByPrimaryKey = providerCapabilities.AllowsIndexesCoveredByPrimaryKey,
      UnsupportedIncludedIndexColumnMode = providerCapabilities.UnsupportedIncludedIndexColumnMode,
      SqlFunctionSupport = providerCapabilities.SqlFunctionSupport,
      ConcurrencySupport = providerCapabilities.ConcurrencySupport,
    };
  }

  private static DataVaultEntityExplain CreateEntityExplain(IReadOnlyEntityType entityType) {
    var tableName = GetStringAnnotation(entityType, DataVaultAnnotationNames.ProducedName) ??
        entityType.GetTableName() ??
        entityType.Name;
    var tableKind = GetAnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind);
    var metadataName = GetStringAnnotation(entityType, DataVaultAnnotationNames.MetadataName) ?? tableName;
    var parentKind = GetNullableAnnotationValue<DataVaultMetadataReferenceKind>(
        entityType,
        DataVaultAnnotationNames.ParentReferenceKind);
    var parentName = GetStringAnnotation(entityType, DataVaultAnnotationNames.ParentReferenceName);
    var parentReference = parentKind.HasValue && parentName is not null
        ? new DataVaultParentReferenceExplain(parentKind.Value, parentName)
        : null;
    var properties = entityType
        .GetProperties()
        .Select(CreatePropertyExplain)
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.Name, StringComparer.Ordinal)
        .ToArray();
    var primaryKey = entityType.FindPrimaryKey();
    var primaryKeyExplain = primaryKey is null
        ? new DataVaultKeyExplain("<none>", Array.Empty<string>())
        : new DataVaultKeyExplain(
            GetStringAnnotation(primaryKey, DataVaultAnnotationNames.ProducedName) ??
                primaryKey.GetName() ??
                "Pk" + tableName,
            primaryKey.Properties.Select(property => property.Name).ToArray());
    var indexes = entityType
        .GetIndexes()
        .Select(CreateIndexExplain)
        .OrderBy(index => index.Name, StringComparer.Ordinal)
        .ToArray();
    var constraints = primaryKey is null
        ? Array.Empty<DataVaultConstraintExplain>()
        : [new DataVaultConstraintExplain(
            primaryKeyExplain.Name,
            DataVaultConstraintKind.PrimaryKey,
            primaryKeyExplain.PropertyNames)];

    return new DataVaultEntityExplain(
        tableName,
        tableKind,
        metadataName,
        parentReference,
        properties,
        primaryKeyExplain,
        indexes,
        constraints);
  }

  private static DataVaultPropertyExplain CreatePropertyExplain(IReadOnlyProperty property) {
    return new DataVaultPropertyExplain(
        GetStringAnnotation(property, DataVaultAnnotationNames.ProducedName) ?? property.Name,
        GetAnnotationValue<DataVaultPropertyRole>(property, DataVaultAnnotationNames.PropertyRole),
        GetNullableAnnotationValue<TechnicalMetadataColumnRole>(property, DataVaultAnnotationNames.TechnicalColumnRole),
        GetStringAnnotation(property, DataVaultAnnotationNames.MetadataName) ?? property.Name,
        GetNullableAnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal) ?? property.GetColumnOrder() ?? 0,
        GetAnnotationValue<DataVaultLogicalPropertyKind>(property, DataVaultAnnotationNames.ProviderLogicalPropertyKind),
        GetStringAnnotation(property, DataVaultAnnotationNames.ProviderProfile) ?? string.Empty,
        GetStringAnnotation(property, DataVaultAnnotationNames.ProviderStorageType) ?? property.GetColumnType() ?? string.Empty,
        GetAnnotationValue<DataVaultProviderValueFormat>(property, DataVaultAnnotationNames.ProviderValueFormat)) {
      ClrTypeName = property.ClrType.FullName ?? property.ClrType.Name,
      IsNullable = property.IsNullable,
    };
  }

  private static DataVaultIndexExplain CreateIndexExplain(IReadOnlyIndex index) {
    var propertyNames = index.Properties.Select(property => property.Name).ToArray();
    var descendingPropertyNames = GetDescendingPropertyNames(index).ToArray();
    var includedPropertyNames = GetIncludedPropertyNames(index);

    return new DataVaultIndexExplain(
        GetStringAnnotation(index, DataVaultAnnotationNames.ProducedName) ??
            index.GetDatabaseName() ??
            string.Join("_", propertyNames),
        propertyNames,
        index.IsUnique,
        descendingPropertyNames,
        includedPropertyNames);
  }

  private static IEnumerable<string> GetDescendingPropertyNames(IReadOnlyIndex index) {
    if (index.IsDescending is null) {
      yield break;
    }

    for (var ordinal = 0; ordinal < index.Properties.Count && ordinal < index.IsDescending.Count; ordinal++) {
      if (index.IsDescending[ordinal]) {
        yield return index.Properties[ordinal].Name;
      }
    }
  }

  private static IReadOnlyList<string> GetIncludedPropertyNames(IReadOnlyIndex index) {
    foreach (var annotationName in new[] { "SqlServer:Include", "Npgsql:IndexInclude" }) {
      var value = index.FindAnnotation(annotationName)?.Value;
      if (value is string[] stringArray) {
        return stringArray;
      }

      if (value is IEnumerable<string> stringValues) {
        return stringValues.ToArray();
      }
    }

    return Array.Empty<string>();
  }

  private static bool IsDataVaultEntity(IReadOnlyEntityType entityType) {
    return entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value is DataVaultTableKind;
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> ValidateMetadataModel(DataVaultMetadataModel metadataModel) {
    var issues = new List<DataVaultDiagnosticsIssue>();
    AddDuplicateNameIssues(issues, "hub", metadataModel.Hubs.Select(hub => hub.Name), "metadata.hubs");
    AddDuplicateNameIssues(issues, "link", metadataModel.Links.Select(link => link.Name), "metadata.links");
    AddDuplicateNameIssues(
        issues,
        "point-in-time-table",
        metadataModel.PointInTimeTables.Select(pointInTime => pointInTime.Name),
        "metadata.pointInTimeTables");
    AddDuplicateNameIssues(issues, "bridge", metadataModel.Bridges.Select(bridge => bridge.Name), "metadata.bridges");
    AddDuplicateNameIssues(issues, "pit", metadataModel.Pits.Select(pit => pit.Name), "metadata.pits");

    var hubNames = metadataModel.Hubs.Select(hub => hub.Name).ToHashSet(StringComparer.Ordinal);
    var linkNames = metadataModel.Links.Select(link => link.Name).ToHashSet(StringComparer.Ordinal);
    var satelliteKeys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var satellite in metadataModel.Satellites) {
      var key = satellite.Parent.Kind + ":" + satellite.Parent.Name + ":" + satellite.Name;
      if (!satelliteKeys.Add(key)) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "duplicate-logical-name",
            "Duplicate satellite metadata logical name '" + satellite.Name + "' under " + FormatParent(satellite.Parent) + ".",
            "metadata.satellites"));
      }
    }

    foreach (var link in metadataModel.Links) {
      foreach (var participant in link.Participants) {
        if (!hubNames.Contains(participant.HubReference.Name)) {
          issues.Add(MissingReferenceIssue(
              "link",
              link.Name,
              "hub",
              participant.HubReference.Name,
              "metadata.links"));
        }
      }
    }

    foreach (var satellite in metadataModel.Satellites) {
      if (satellite.Parent.Kind == DataVaultMetadataReferenceKind.Hub && !hubNames.Contains(satellite.Parent.Name)) {
        issues.Add(MissingReferenceIssue(
            "satellite",
            satellite.Name,
            "hub",
            satellite.Parent.Name,
            "metadata.satellites"));
      }
      else if (satellite.Parent.Kind == DataVaultMetadataReferenceKind.Link && !linkNames.Contains(satellite.Parent.Name)) {
        issues.Add(MissingReferenceIssue(
            "satellite",
            satellite.Name,
            "link",
            satellite.Parent.Name,
            "metadata.satellites"));
      }
    }

    foreach (var bridge in metadataModel.Bridges) {
      if (!hubNames.Contains(bridge.SourceHubReference.Name)) {
        issues.Add(MissingReferenceIssue("bridge", bridge.Name, "hub", bridge.SourceHubReference.Name, "metadata.bridges"));
      }

      if (!hubNames.Contains(bridge.TargetHubReference.Name)) {
        issues.Add(MissingReferenceIssue("bridge", bridge.Name, "hub", bridge.TargetHubReference.Name, "metadata.bridges"));
      }

      if (!linkNames.Contains(bridge.LinkReference.Name)) {
        issues.Add(MissingReferenceIssue("bridge", bridge.Name, "link", bridge.LinkReference.Name, "metadata.bridges"));
      }
    }

    foreach (var pit in metadataModel.Pits) {
      if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Hub && !hubNames.Contains(pit.Parent.Name)) {
        issues.Add(MissingReferenceIssue("pit", pit.Name, "hub", pit.Parent.Name, "metadata.pits"));
      }
      else if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Link && !linkNames.Contains(pit.Parent.Name)) {
        issues.Add(MissingReferenceIssue("pit", pit.Name, "link", pit.Parent.Name, "metadata.pits"));
      }
    }

    return issues;
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> ValidateProviderMappings(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var issues = new List<DataVaultDiagnosticsIssue>();
    var requiredKinds = GetRequiredLogicalPropertyKinds(metadataModel);
    foreach (var requiredKind in requiredKinds.OrderBy(kind => kind)) {
      try {
        providerCapabilities.GetRequiredTypeMapping(requiredKind);
      }
      catch (NotSupportedException exception) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "missing-provider-type-mapping",
            exception.Message,
            "capability-profile." + providerCapabilities.ProfileName));
      }
    }

    return issues;
  }

  private static IReadOnlySet<DataVaultLogicalPropertyKind> GetRequiredLogicalPropertyKinds(
      DataVaultMetadataModel metadataModel) {
    var kinds = new HashSet<DataVaultLogicalPropertyKind>
    {
        DataVaultLogicalPropertyKind.HashKey,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        DataVaultLogicalPropertyKind.RecordSource,
    };

    if (metadataModel.Hubs.Any(hub => hub.BusinessKeyColumns.Count > 0)) {
      kinds.Add(DataVaultLogicalPropertyKind.BusinessKey);
    }

    if (metadataModel.Links.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.ParticipantReference);
    }

    if (metadataModel.Satellites.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.HashDiff);
      kinds.Add(DataVaultLogicalPropertyKind.PayloadText);
      if (metadataModel.Satellites.Any(satellite => satellite.DrivingKeyNames.Count > 0)) {
        kinds.Add(DataVaultLogicalPropertyKind.DrivingKey);
      }
    }

    if (metadataModel.Bridges.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.ParticipantReference);
      if (metadataModel.Bridges.Any(bridge => bridge.Kind == DataVaultBridgeKind.Hierarchy)) {
        kinds.Add(DataVaultLogicalPropertyKind.BridgeDepth);
      }
    }

    if (metadataModel.Pits.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.SatelliteSnapshotReference);
    }

    return kinds;
  }

  private static void AddDuplicateNameIssues(
      ICollection<DataVaultDiagnosticsIssue> issues,
      string kind,
      IEnumerable<string> names,
      string path) {
    foreach (var group in names.GroupBy(name => name, StringComparer.Ordinal).Where(group => group.Count() > 1)) {
      issues.Add(new DataVaultDiagnosticsIssue(
          DataVaultDiagnosticsIssueSeverity.Error,
          "duplicate-logical-name",
          "Duplicate " + kind + " metadata logical name '" + group.Key + "'.",
          path));
    }
  }

  private static DataVaultDiagnosticsIssue MissingReferenceIssue(
      string sourceKind,
      string sourceName,
      string targetKind,
      string targetName,
      string path) {
    return new DataVaultDiagnosticsIssue(
        DataVaultDiagnosticsIssueSeverity.Error,
        "missing-reference",
        sourceKind + " metadata '" + sourceName + "' references missing " + targetKind + " metadata '" + targetName + "'.",
        path);
  }

  private static IReadOnlyList<DataVaultSaveStrategyFallbackCause> DistinctFallbackCauses(
      IEnumerable<DataVaultSaveStrategyFallbackCause> causes) {
    var values = new List<DataVaultSaveStrategyFallbackCause>();
    var keys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var cause in causes) {
      var key = cause.Kind + "\u001f" + cause.Message;
      if (keys.Add(key)) {
        values.Add(cause);
      }
    }

    return values;
  }

  private static IReadOnlyList<DataVaultReadStrategyFallbackCause> DistinctFallbackCauses(
      IEnumerable<DataVaultReadStrategyFallbackCause> causes) {
    var values = new List<DataVaultReadStrategyFallbackCause>();
    var keys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var cause in causes) {
      var key = cause.Kind + "" + cause.Message;
      if (keys.Add(key)) {
        values.Add(cause);
      }
    }

    return values;
  }

  private static DataVaultProviderTypeMapping GetLoadTimestampMapping(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    return GetTypeMappingOrMissing(providerCapabilities, DataVaultLogicalPropertyKind.LoadTimestamp);
  }

  private static DataVaultProviderTypeMapping GetSatelliteSnapshotReferenceMapping(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    return GetTypeMappingOrMissing(providerCapabilities, DataVaultLogicalPropertyKind.SatelliteSnapshotReference);
  }

  private static DataVaultProviderTypeMapping GetTypeMappingOrMissing(
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultLogicalPropertyKind logicalPropertyKind) {
    try {
      return providerCapabilities.GetRequiredTypeMapping(logicalPropertyKind);
    }
    catch (NotSupportedException) {
      return new DataVaultProviderTypeMapping(
          logicalPropertyKind,
          typeof(DateTimeOffset),
          "<missing>",
          DataVaultProviderValueFormat.Text);
    }
  }

  private static IReadOnlyList<DataVaultProviderTypeMappingExplain> CreateTypeMappingExplain(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    return providerCapabilities.TypeMappings
        .OrderBy(mapping => mapping.LogicalPropertyKind)
        .Select(mapping => new DataVaultProviderTypeMappingExplain(
            mapping.LogicalPropertyKind,
            mapping.ModelClrType.FullName ?? mapping.ModelClrType.Name,
            mapping.NativeStoreType,
            mapping.ValueFormat))
        .ToArray();
  }

  private static string FormatParent(DataVaultMetadataReference parent) {
    return parent.Kind.ToString().ToLowerInvariant() + " '" + parent.Name + "'";
  }

  private static int GetEntityKindSortKey(DataVaultTableKind tableKind) {
    return tableKind switch {
      DataVaultTableKind.Hub => 0,
      DataVaultTableKind.Link => 1,
      DataVaultTableKind.Satellite => 2,
      DataVaultTableKind.Bridge => 3,
      DataVaultTableKind.Pit => 4,
      DataVaultTableKind.PointInTime => 5,
      _ => 99,
    };
  }

  private static string? GetStringAnnotation(IReadOnlyAnnotatable annotatable, string annotationName) {
    return annotatable.FindAnnotation(annotationName)?.Value as string;
  }

  private static T GetAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : default;
  }

  private static T? GetNullableAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : null;
  }

  private readonly record struct SaveStrategyRegistration(
      IDataVaultProviderSaveStrategy Strategy,
      int RegistrationOrdinal);

  private readonly record struct ReadStrategyRegistration(
      IDataVaultProviderReadStrategy Strategy,
      int RegistrationOrdinal);

  private readonly record struct PitReadStrategyRegistration(
      IDataVaultProviderPitReadStrategy Strategy,
      int RegistrationOrdinal);

  private readonly record struct BridgeReadStrategyRegistration(
      IDataVaultProviderBridgeReadStrategy Strategy,
      int RegistrationOrdinal);
}

internal enum DataVaultKnownProviderSaveStrategy {
  Sqlite,
  Postgres,
  SqlServer,
  MySql,
  MySqlStaged,
  Oracle,
}

internal sealed record DataVaultProviderSaveStrategyGateEvaluation(
    bool CanSave,
    IReadOnlyList<DataVaultSaveStrategyFallbackCause> FallbackCauses);

internal static class DataVaultProviderSaveStrategyGateEvaluator {
  private const int MinimumSqlServerOptimizedBatchOperationCount = 50;
  private const int MaximumSqlServerOptimizedSatelliteOperationCount = 500;
  private const int MinimumMySqlOptimizedBatchOperationCount = 50;
  private const int MinimumMySqlStagedBatchOperationCount = 60;
  private const int MinimumOracleOptimizedBatchOperationCount = 50;
  private const int MaximumOracleOptimizedSatelliteOperationCount = 10000;

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.Sqlite,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.Sqlite],
        minimumOperationCount: null,
        maximumSatelliteOperationCount: null);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluatePostgres(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.Postgres,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.Postgres],
        minimumOperationCount: null,
        maximumSatelliteOperationCount: null);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlServer(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlServer(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.SqlServer,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.SqlServer],
        minimumOperationCount: MinimumSqlServerOptimizedBatchOperationCount,
        maximumSatelliteOperationCount: MaximumSqlServerOptimizedSatelliteOperationCount);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySql(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.MySql,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
        minimumOperationCount: MinimumMySqlOptimizedBatchOperationCount,
        maximumSatelliteOperationCount: null);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySqlStaged(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySqlStaged(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySqlStaged(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.MySqlStaged,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
        minimumOperationCount: MinimumMySqlStagedBatchOperationCount,
        maximumSatelliteOperationCount: null);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateOracle(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateOracle(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.Oracle,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.Oracle],
        minimumOperationCount: MinimumOracleOptimizedBatchOperationCount,
        maximumSatelliteOperationCount: MaximumOracleOptimizedSatelliteOperationCount);
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderSaveStrategy strategy,
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      out DataVaultProviderSaveStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultSaveStrategy" => EvaluateSqlite(dbContext, requests),
      "PostgresDataVaultSaveStrategy" => EvaluatePostgres(dbContext, requests),
      "SqlServerDataVaultSaveStrategy" => EvaluateSqlServer(dbContext, requests),
      "MySqlStagedDataVaultSaveStrategy" => EvaluateMySqlStaged(dbContext, requests),
      "MySqlDataVaultSaveStrategy" => EvaluateMySql(dbContext, requests),
      "OracleDataVaultSaveStrategy" => EvaluateOracle(dbContext, requests),
      _ => new DataVaultProviderSaveStrategyGateEvaluation(false, Array.Empty<DataVaultSaveStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanSave;
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderSaveStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultSaveStrategy" => [KnownProviderNames.Sqlite],
      "PostgresDataVaultSaveStrategy" => [KnownProviderNames.Postgres],
      "SqlServerDataVaultSaveStrategy" => [KnownProviderNames.SqlServer],
      "MySqlStagedDataVaultSaveStrategy" => [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
      "MySqlDataVaultSaveStrategy" => [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
      "OracleDataVaultSaveStrategy" => [KnownProviderNames.Oracle],
      _ => Array.Empty<string>(),
    };
  }

  public static IReadOnlyList<DataVaultSaveStrategyGateRequirement> GetKnownStrategyGateRequirements(
      IDataVaultProviderSaveStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    var commonRequirements = new[]
    {
        new DataVaultSaveStrategyGateRequirement(DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch),
        new DataVaultSaveStrategyGateRequirement(DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext),
        new DataVaultSaveStrategyGateRequirement(DataVaultSaveStrategyFallbackCauseKind.MultiActiveSatelliteOperations),
    };

    return strategy.GetType().Name switch {
      "SqliteDataVaultSaveStrategy" => commonRequirements,
      "PostgresDataVaultSaveStrategy" => commonRequirements,
      "SqlServerDataVaultSaveStrategy" => commonRequirements
          .Concat([
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
                  MinimumTotalOperationCount: MinimumSqlServerOptimizedBatchOperationCount),
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold,
                  MaximumSatelliteOperationCount: MaximumSqlServerOptimizedSatelliteOperationCount),
          ])
          .ToArray(),
      "MySqlDataVaultSaveStrategy" => commonRequirements
          .Append(new DataVaultSaveStrategyGateRequirement(
              DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
              MinimumTotalOperationCount: MinimumMySqlOptimizedBatchOperationCount))
          .ToArray(),
      "MySqlStagedDataVaultSaveStrategy" => commonRequirements
          .Append(new DataVaultSaveStrategyGateRequirement(
              DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
              MinimumTotalOperationCount: MinimumMySqlStagedBatchOperationCount))
          .ToArray(),
      "OracleDataVaultSaveStrategy" => commonRequirements
          .Concat([
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold,
                  MinimumTotalOperationCount: MinimumOracleOptimizedBatchOperationCount),
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.OracleMaximumSatelliteOperationThreshold,
                  MaximumSatelliteOperationCount: MaximumOracleOptimizedSatelliteOperationCount),
          ])
          .ToArray(),
      _ => Array.Empty<DataVaultSaveStrategyGateRequirement>(),
    };
  }

  public static bool HasPendingTrackedChanges(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return dbContext.ChangeTracker
        .Entries()
        .Any(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
  }

  public static bool ContainsMultiActiveSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    return requests.Any(request => request.SatelliteOperations.Any(operation => operation.Metadata.DrivingKeyNames.Count > 0));
  }

  public static int CountOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var operationCount = 0;
    foreach (var request in requests) {
      operationCount += request.HubOperations.Count + request.LinkOperations.Count + request.SatelliteOperations.Count;
    }

    return operationCount;
  }

  public static int CountSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var operationCount = 0;
    foreach (var request in requests) {
      operationCount += request.SatelliteOperations.Count;
    }

    return operationCount;
  }

  private static DataVaultProviderSaveStrategyGateEvaluation Evaluate(
      DataVaultKnownProviderSaveStrategy strategy,
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests,
      IReadOnlyList<string> supportedProviderNames,
      int? minimumOperationCount,
      int? maximumSatelliteOperationCount) {
    ArgumentNullException.ThrowIfNull(requests);

    var causes = new List<DataVaultSaveStrategyFallbackCause>();
    if (!supportedProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      causes.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match " + FormatStrategyName(strategy) + "."));
    }

    if (hasPendingTrackedChanges) {
      causes.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext,
          "The DbContext change tracker contains pending added, modified, or deleted state."));
    }

    if (ContainsMultiActiveSatelliteOperations(requests)) {
      causes.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.MultiActiveSatelliteOperations,
          "The save batch contains one or more multi-active satellite operations."));
    }

    if (minimumOperationCount.HasValue) {
      var operationCount = CountOperations(requests);
      if (operationCount < minimumOperationCount.Value) {
        causes.Add(new DataVaultSaveStrategyFallbackCause(
            GetMinimumThresholdCauseKind(strategy),
            FormatStrategyName(strategy) +
            " optimized dispatch requires at least " +
            minimumOperationCount.Value.ToString(CultureInfo.InvariantCulture) +
            " total operations; the request batch contains " +
            operationCount.ToString(CultureInfo.InvariantCulture) +
            "."));
      }
    }

    if (maximumSatelliteOperationCount.HasValue) {
      var satelliteOperationCount = CountSatelliteOperations(requests);
      if (satelliteOperationCount > maximumSatelliteOperationCount.Value) {
        causes.Add(new DataVaultSaveStrategyFallbackCause(
            GetMaximumSatelliteThresholdCauseKind(strategy),
            FormatStrategyName(strategy) +
            " optimized dispatch accepts at most " +
            maximumSatelliteOperationCount.Value.ToString(CultureInfo.InvariantCulture) +
            " satellite operations; the request batch contains " +
            satelliteOperationCount.ToString(CultureInfo.InvariantCulture) +
            "."));
      }
    }

    return new DataVaultProviderSaveStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static DataVaultSaveStrategyFallbackCauseKind GetMinimumThresholdCauseKind(
      DataVaultKnownProviderSaveStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderSaveStrategy.SqlServer => DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
      DataVaultKnownProviderSaveStrategy.MySql => DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
      DataVaultKnownProviderSaveStrategy.MySqlStaged => DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
      DataVaultKnownProviderSaveStrategy.Oracle => DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold,
      _ => DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
    };
  }

  private static DataVaultSaveStrategyFallbackCauseKind GetMaximumSatelliteThresholdCauseKind(
      DataVaultKnownProviderSaveStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderSaveStrategy.SqlServer => DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold,
      DataVaultKnownProviderSaveStrategy.Oracle => DataVaultSaveStrategyFallbackCauseKind.OracleMaximumSatelliteOperationThreshold,
      _ => DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
    };
  }

  private static string FormatStrategyName(DataVaultKnownProviderSaveStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderSaveStrategy.Sqlite => "SQLite",
      DataVaultKnownProviderSaveStrategy.Postgres => "PostgreSQL",
      DataVaultKnownProviderSaveStrategy.SqlServer => "SQL Server",
      DataVaultKnownProviderSaveStrategy.MySql => "MySQL",
      DataVaultKnownProviderSaveStrategy.MySqlStaged => "MySQL staged bulk",
      DataVaultKnownProviderSaveStrategy.Oracle => "Oracle",
      _ => strategy.ToString(),
    };
  }
}

internal enum DataVaultKnownProviderReadStrategy {
  Sqlite,
  Postgres,
  SqlServer,
  MySql,
  Oracle,
}

internal sealed record DataVaultProviderReadStrategyGateEvaluation(
    bool CanRead,
    IReadOnlyList<DataVaultReadStrategyFallbackCause> FallbackCauses);

internal static class DataVaultProviderReadStrategyGateEvaluator {
  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(dbContext.Database.ProviderName, request);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluatePostgres(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluatePostgres(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlServer(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlServer(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySql(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySql(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateOracle(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateOracle(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultLatestSatelliteReadRequest request) {
    return EvaluateLatestSatellite(
        DataVaultKnownProviderReadStrategy.Sqlite,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Sqlite]);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateSqlite(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlite(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.Sqlite,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Sqlite],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateSqlite(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlite(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.Sqlite,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Sqlite],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluatePostgres(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluatePostgres(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.Postgres,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Postgres],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluatePostgres(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluatePostgres(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.Postgres,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Postgres],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateSqlServer(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlServer(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.SqlServer,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.SqlServer],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateSqlServer(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlServer(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.SqlServer,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.SqlServer],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateMySql(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateMySql(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.MySql,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateMySql(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateMySql(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.MySql,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateOracle(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateOracle(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.Oracle,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Oracle],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateOracle(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateOracle(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.Oracle,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Oracle],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderReadStrategy strategy,
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      out DataVaultProviderReadStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => EvaluateSqlite(dbContext, request),
      _ => new DataVaultProviderReadStrategyGateEvaluation(false, Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanRead;
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderPitReadStrategy strategy,
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      out DataVaultProviderReadStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => EvaluateSqlite(dbContext, request),
      "PostgresDataVaultReadStrategy" => EvaluatePostgres(dbContext, request),
      "SqlServerDataVaultReadStrategy" => EvaluateSqlServer(dbContext, request),
      "MySqlDataVaultReadStrategy" => EvaluateMySql(dbContext, request),
      "OracleDataVaultReadStrategy" => EvaluateOracle(dbContext, request),
      _ => new DataVaultProviderReadStrategyGateEvaluation(false, Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanRead;
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderBridgeReadStrategy strategy,
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      out DataVaultProviderReadStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => EvaluateSqlite(dbContext, request),
      "PostgresDataVaultReadStrategy" => EvaluatePostgres(dbContext, request),
      "SqlServerDataVaultReadStrategy" => EvaluateSqlServer(dbContext, request),
      "MySqlDataVaultReadStrategy" => EvaluateMySql(dbContext, request),
      "OracleDataVaultReadStrategy" => EvaluateOracle(dbContext, request),
      _ => new DataVaultProviderReadStrategyGateEvaluation(false, Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanRead;
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return GetKnownStrategySupportedProviderNames(strategy.GetType().Name);
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderPitReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return GetKnownStrategySupportedProviderNames(strategy.GetType().Name);
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderBridgeReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return GetKnownStrategySupportedProviderNames(strategy.GetType().Name);
  }

  public static IReadOnlyList<DataVaultReadStrategyGateRequirement> GetKnownLatestSatelliteGateRequirements(
      IDataVaultProviderReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported),
      ],
      _ => Array.Empty<DataVaultReadStrategyGateRequirement>(),
    };
  }

  public static IReadOnlyList<DataVaultReadStrategyGateRequirement> GetKnownPitGateRequirements(
      IDataVaultProviderPitReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "PostgresDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "SqlServerDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "MySqlDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "OracleDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      _ => Array.Empty<DataVaultReadStrategyGateRequirement>(),
    };
  }

  public static IReadOnlyList<DataVaultReadStrategyGateRequirement> GetKnownBridgeGateRequirements(
      IDataVaultProviderBridgeReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "PostgresDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "SqlServerDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "MySqlDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "OracleDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      _ => Array.Empty<DataVaultReadStrategyGateRequirement>(),
    };
  }

  private static DataVaultProviderReadStrategyGateEvaluation EvaluateLatestSatellite(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      DataVaultLatestSatelliteReadRequest request,
      IReadOnlyList<string> supportedProviderNames) {
    ArgumentNullException.ThrowIfNull(request);

    var causes = new List<DataVaultReadStrategyFallbackCause>();
    if (!supportedProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match " + FormatStrategyName(strategy) + "."));
    }

    if (request.Satellite.Parent.Kind != DataVaultMetadataReferenceKind.Hub) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent,
          FormatStrategyName(strategy) + " optimized latest/as-of satellite reads support hub-parent satellites only."));
    }

    if (request.Satellite.DrivingKeyNames.Count > 0) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported,
          FormatStrategyName(strategy) + " optimized latest/as-of satellite reads do not support multi-active driving keys."));
    }

    return new DataVaultProviderReadStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static DataVaultProviderReadStrategyGateEvaluation EvaluatePit(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      IReadOnlyList<string> supportedProviderNames,
      bool supportsLinkParent = false,
      bool supportsMultiActive = false,
      bool hasCompleteReadShapeEvidence = true,
      bool hasStaleReadModelMaintenanceSignal = false) {
    ArgumentNullException.ThrowIfNull(request);

    var causes = CreateProviderMismatchCauses(strategy, providerName, supportedProviderNames);
    AddStaleReadModelMaintenanceCause(causes, strategy, hasStaleReadModelMaintenanceSignal, "PIT");

    if (!hasCompleteReadShapeEvidence) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence,
          FormatStrategyName(strategy) + " optimized PIT reads require a complete generated PIT table/entity projection and referenced satellite projection evidence in the DbContext model."));
    }

    if (request.Pit.Parent.Kind != DataVaultMetadataReferenceKind.Hub &&
        (!supportsLinkParent || request.Pit.Parent.Kind != DataVaultMetadataReferenceKind.Link)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          supportsLinkParent
              ? FormatStrategyName(strategy) + " optimized PIT reads support hub- or link-parent PIT declarations only."
              : FormatStrategyName(strategy) + " optimized PIT reads support hub-parent PIT declarations only."));
    }

    if (request.Pit.Satellites.Count == 0) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized PIT reads require at least one satellite snapshot reference."));
    }

    if (!supportsMultiActive && request.Pit.Satellites.Any(satellite => satellite.IsMultiActive)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized PIT reads do not support multi-active satellite references."));
    }

    if (supportsLinkParent &&
        request.Pit.Parent.Kind == DataVaultMetadataReferenceKind.Link &&
        request.Pit.Satellites.Any(satellite => satellite.IsMultiActive)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized link-parent PIT reads require non-multi-active satellite references."));
    }

    var duplicateSatelliteName = request.Pit.Satellites
        .GroupBy(satellite => satellite.SatelliteName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();
    if (duplicateSatelliteName is not null) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized PIT reads require distinct satellite snapshot references."));
    }

    return new DataVaultProviderReadStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static DataVaultProviderReadStrategyGateEvaluation EvaluateBridge(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      DataVaultBridgeReadRequest request,
      IReadOnlyList<string> supportedProviderNames,
      bool hasCompleteReadShapeEvidence = true,
      bool hasStaleReadModelMaintenanceSignal = false) {
    ArgumentNullException.ThrowIfNull(request);

    var causes = CreateProviderMismatchCauses(strategy, providerName, supportedProviderNames);
    AddStaleReadModelMaintenanceCause(causes, strategy, hasStaleReadModelMaintenanceSignal, "bridge");

    if (!hasCompleteReadShapeEvidence) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence,
          FormatStrategyName(strategy) + " optimized bridge reads require a complete generated bridge table/entity projection in the DbContext model."));
    }

    if (request.Bridge.ProjectionFeatures != DataVaultBridgeProjectionFeatures.None) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape,
          FormatStrategyName(strategy) + " optimized bridge reads support endpoint hash keys and TraversalDepth only."));
    }

    if (request.Bridge.Kind is not DataVaultBridgeKind.ManyToMany and not DataVaultBridgeKind.Hierarchy) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape,
          FormatStrategyName(strategy) + " optimized bridge reads support many-to-many and hierarchy bridges only."));
    }

    return new DataVaultProviderReadStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static bool HasCompletePitReadShapeEvidence(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    try {
      _ = DataVaultPitReadPipeline.CreatePitProjection(dbContext, request);
      return true;
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      return false;
    }
  }

  private static bool HasCompleteBridgeReadShapeEvidence(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    try {
      _ = DataVaultBridgeReadPipeline.CreateBridgeProjection(dbContext, request);
      return true;
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      return false;
    }
  }

  private static bool HasStaleReadModelMaintenanceSignal(DbContext dbContext) {
    try {
      return dbContext.ChangeTracker.HasChanges();
    }
    catch (Exception exception) when (exception is InvalidOperationException or NotSupportedException) {
      return true;
    }
  }

  private static void AddStaleReadModelMaintenanceCause(
      ICollection<DataVaultReadStrategyFallbackCause> causes,
      DataVaultKnownProviderReadStrategy strategy,
      bool hasStaleReadModelMaintenanceSignal,
      string readModelKind) {
    if (!hasStaleReadModelMaintenanceSignal) {
      return;
    }

    causes.Add(new DataVaultReadStrategyFallbackCause(
        DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance,
        FormatStrategyName(strategy) + " optimized " + readModelKind + " reads require clean context evidence because pending tracked changes can make caller-maintained read-model rows stale."));
  }

  private static List<DataVaultReadStrategyFallbackCause> CreateProviderMismatchCauses(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      IReadOnlyList<string> supportedProviderNames) {
    var causes = new List<DataVaultReadStrategyFallbackCause>();
    if (!supportedProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match " + FormatStrategyName(strategy) + "."));
    }

    return causes;
  }

  private static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(string strategyName) {
    return strategyName switch {
      "SqliteDataVaultReadStrategy" => [KnownProviderNames.Sqlite],
      "PostgresDataVaultReadStrategy" => [KnownProviderNames.Postgres],
      "SqlServerDataVaultReadStrategy" => [KnownProviderNames.SqlServer],
      "MySqlDataVaultReadStrategy" => [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
      "OracleDataVaultReadStrategy" => [KnownProviderNames.Oracle],
      _ => Array.Empty<string>(),
    };
  }

  private static string FormatStrategyName(DataVaultKnownProviderReadStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderReadStrategy.Sqlite => "SQLite",
      DataVaultKnownProviderReadStrategy.Postgres => "PostgreSQL",
      DataVaultKnownProviderReadStrategy.SqlServer => "SQL Server",
      DataVaultKnownProviderReadStrategy.MySql => "MySQL",
      DataVaultKnownProviderReadStrategy.Oracle => "Oracle",
      _ => strategy.ToString(),
    };
  }
}

internal static class KnownProviderNames {
  public const string Sqlite = "Microsoft.EntityFrameworkCore.Sqlite";
  public const string Postgres = "Npgsql.EntityFrameworkCore.PostgreSQL";
  public const string SqlServer = "Microsoft.EntityFrameworkCore.SqlServer";
  public const string Oracle = "Oracle.EntityFrameworkCore";
  public const string MySqlPomelo = "Pomelo.EntityFrameworkCore.MySql";
  public const string MySqlOracle = "MySql.EntityFrameworkCore";
}
