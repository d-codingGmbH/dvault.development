namespace DCoding.Data.DVault;

/// <summary>
/// Bounded diagnostics for one staged-provider bulk save evaluation.
/// </summary>
public sealed class DataVaultStagedProviderBulkDiagnostics {
  /// <summary>
  /// Initializes a new staged-provider bulk diagnostics payload.
  /// </summary>
  /// <param name="lifecyclePhase">The staged lifecycle phase reached by the provider strategy.</param>
  /// <param name="providerCaveatKind">The finite provider caveat classification, if any.</param>
  /// <param name="requestCount">The number of explicit save requests evaluated for staged bulk execution.</param>
  /// <param name="hubOperationCount">The number of hub operations evaluated for staged bulk execution.</param>
  /// <param name="linkOperationCount">The number of link operations evaluated for staged bulk execution.</param>
  /// <param name="satelliteOperationCount">The number of satellite operations evaluated for staged bulk execution.</param>
  /// <param name="fallbackCauseKinds">The finite staged-provider fallback or decline cause kinds.</param>
  public DataVaultStagedProviderBulkDiagnostics(
      DataVaultStagedProviderBulkLifecyclePhase lifecyclePhase,
      DataVaultStagedProviderBulkProviderCaveatKind providerCaveatKind,
      int requestCount,
      int hubOperationCount,
      int linkOperationCount,
      int satelliteOperationCount,
      IEnumerable<DataVaultSaveStrategyFallbackCauseKind> fallbackCauseKinds) {
    ArgumentOutOfRangeException.ThrowIfNegative(requestCount);
    ArgumentOutOfRangeException.ThrowIfNegative(hubOperationCount);
    ArgumentOutOfRangeException.ThrowIfNegative(linkOperationCount);
    ArgumentOutOfRangeException.ThrowIfNegative(satelliteOperationCount);
    ArgumentNullException.ThrowIfNull(fallbackCauseKinds);

    var causeKindArray = fallbackCauseKinds.Distinct().ToArray();
    foreach (var causeKind in causeKindArray) {
      if (!DataVaultStagedProviderBulkDiagnosticsSupport.IsStagedProviderBulkFallbackCause(causeKind)) {
        throw new ArgumentException(
            "Staged-provider bulk diagnostics can only carry staged-provider bulk fallback cause kinds.",
            nameof(fallbackCauseKinds));
      }
    }

    LifecyclePhase = lifecyclePhase;
    ProviderCaveatKind = providerCaveatKind;
    RequestCount = requestCount;
    HubOperationCount = hubOperationCount;
    LinkOperationCount = linkOperationCount;
    SatelliteOperationCount = satelliteOperationCount;
    FallbackCauseKinds = causeKindArray;
  }

  /// <summary>
  /// Gets the staged lifecycle phase reached by the provider strategy.
  /// </summary>
  public DataVaultStagedProviderBulkLifecyclePhase LifecyclePhase { get; }

  /// <summary>
  /// Gets the finite provider caveat classification, if any.
  /// </summary>
  public DataVaultStagedProviderBulkProviderCaveatKind ProviderCaveatKind { get; }

  /// <summary>
  /// Gets the number of explicit save requests evaluated for staged bulk execution.
  /// </summary>
  public int RequestCount { get; }

  /// <summary>
  /// Gets the number of hub operations evaluated for staged bulk execution.
  /// </summary>
  public int HubOperationCount { get; }

  /// <summary>
  /// Gets the number of link operations evaluated for staged bulk execution.
  /// </summary>
  public int LinkOperationCount { get; }

  /// <summary>
  /// Gets the number of satellite operations evaluated for staged bulk execution.
  /// </summary>
  public int SatelliteOperationCount { get; }

  /// <summary>
  /// Gets the total hub, link, and satellite operation count evaluated for staged bulk execution.
  /// </summary>
  public int OperationCount => HubOperationCount + LinkOperationCount + SatelliteOperationCount;

  /// <summary>
  /// Gets the finite staged-provider fallback or decline cause kinds.
  /// </summary>
  public IReadOnlyList<DataVaultSaveStrategyFallbackCauseKind> FallbackCauseKinds { get; }
}
