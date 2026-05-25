namespace DCoding.Data.DVault;

/// <summary>
/// Bounded summary emitted for one explicit DVault save attempt.
/// </summary>
public sealed class DataVaultSaveTelemetrySummary {
  /// <summary>
  /// Initializes a new save telemetry summary.
  /// </summary>
  /// <param name="operationKind">Whether the attempt used the single-request or bulk save API.</param>
  /// <param name="outcome">Whether the attempt returned successfully or failed.</param>
  /// <param name="requestCount">The number of explicit save requests in the attempt.</param>
  /// <param name="hubOperationCount">The number of hub operations in the attempt.</param>
  /// <param name="linkOperationCount">The number of link operations in the attempt.</param>
  /// <param name="satelliteOperationCount">The number of satellite operations in the attempt.</param>
  /// <param name="rowsWritten">The number of rows reported by the completed save result, or zero for failed attempts.</param>
  /// <param name="savedRecordCount">The number of saved-record summaries reported by the completed save result, or zero for failed attempts.</param>
  /// <param name="duration">The elapsed duration of the attempt.</param>
  /// <param name="strategyStatus">The request-bound provider-strategy selection status.</param>
  /// <param name="providerName">The Entity Framework provider name, when available.</param>
  /// <param name="selectedStrategyName">The selected provider strategy type name, when a provider strategy was selected.</param>
  /// <param name="fallbackCauseKinds">The distinct finite fallback-cause kinds observed when provider-neutral fallback was selected.</param>
  public DataVaultSaveTelemetrySummary(
      DataVaultSaveTelemetryOperationKind operationKind,
      DataVaultTelemetryOutcome outcome,
      int requestCount,
      int hubOperationCount,
      int linkOperationCount,
      int satelliteOperationCount,
      int rowsWritten,
      int savedRecordCount,
      TimeSpan duration,
      DataVaultSaveStrategyDiagnosticsStatus strategyStatus,
      string? providerName,
      string? selectedStrategyName,
      IEnumerable<DataVaultSaveStrategyFallbackCauseKind> fallbackCauseKinds)
      : this(
          operationKind,
          outcome,
          requestCount,
          hubOperationCount,
          linkOperationCount,
          satelliteOperationCount,
          rowsWritten,
          savedRecordCount,
          duration,
          strategyStatus,
          providerName,
          selectedStrategyName,
          fallbackCauseKinds,
          chunkCount: 0,
          processedChunkCount: 0,
          retainedStateCurrentCount: 0,
          retainedStateHighWaterCount: 0,
          [],
          []) {
  }

  /// <summary>
  /// Initializes a new save telemetry summary with chunked-save retained-state diagnostics.
  /// </summary>
  /// <param name="operationKind">Whether the attempt used the single-request, bulk save, or chunked save API.</param>
  /// <param name="outcome">Whether the attempt returned successfully or failed.</param>
  /// <param name="requestCount">The number of explicit save requests in the attempt.</param>
  /// <param name="hubOperationCount">The number of hub operations in the attempt.</param>
  /// <param name="linkOperationCount">The number of link operations in the attempt.</param>
  /// <param name="satelliteOperationCount">The number of satellite operations in the attempt.</param>
  /// <param name="rowsWritten">The number of rows reported by the completed save result, or zero for failed attempts.</param>
  /// <param name="savedRecordCount">The number of saved-record summaries reported by the completed save result, or zero for failed attempts.</param>
  /// <param name="duration">The elapsed duration of the attempt.</param>
  /// <param name="strategyStatus">The request-bound provider-strategy selection status.</param>
  /// <param name="providerName">The Entity Framework provider name, when available.</param>
  /// <param name="selectedStrategyName">The selected provider strategy type name, when a provider strategy was selected.</param>
  /// <param name="fallbackCauseKinds">The distinct finite fallback-cause kinds observed when provider-neutral fallback was selected.</param>
  /// <param name="chunkCount">The number of chunks observed during the attempt.</param>
  /// <param name="processedChunkCount">The number of non-empty chunks processed during the attempt.</param>
  /// <param name="retainedStateCurrentCount">The retained satellite continuity-state count when telemetry was emitted.</param>
  /// <param name="retainedStateHighWaterCount">The highest retained satellite continuity-state count observed during the attempt.</param>
  /// <param name="chunkedStateFallbackCauseKinds">The distinct finite retained-state fallback causes observed during chunked execution.</param>
  /// <param name="unsupportedShapeKinds">The distinct finite unsupported or memory-sensitive shape classifications observed.</param>
  public DataVaultSaveTelemetrySummary(
      DataVaultSaveTelemetryOperationKind operationKind,
      DataVaultTelemetryOutcome outcome,
      int requestCount,
      int hubOperationCount,
      int linkOperationCount,
      int satelliteOperationCount,
      int rowsWritten,
      int savedRecordCount,
      TimeSpan duration,
      DataVaultSaveStrategyDiagnosticsStatus strategyStatus,
      string? providerName,
      string? selectedStrategyName,
      IEnumerable<DataVaultSaveStrategyFallbackCauseKind> fallbackCauseKinds,
      int chunkCount,
      int processedChunkCount,
      int retainedStateCurrentCount,
      int retainedStateHighWaterCount,
      IEnumerable<DataVaultChunkedSaveStateFallbackCauseKind> chunkedStateFallbackCauseKinds,
      IEnumerable<DataVaultChunkedSaveUnsupportedShapeKind> unsupportedShapeKinds) {
    ArgumentOutOfRangeException.ThrowIfNegative(requestCount);
    ArgumentOutOfRangeException.ThrowIfNegative(hubOperationCount);
    ArgumentOutOfRangeException.ThrowIfNegative(linkOperationCount);
    ArgumentOutOfRangeException.ThrowIfNegative(satelliteOperationCount);
    ArgumentOutOfRangeException.ThrowIfNegative(rowsWritten);
    ArgumentOutOfRangeException.ThrowIfNegative(savedRecordCount);
    ArgumentOutOfRangeException.ThrowIfNegative(chunkCount);
    ArgumentOutOfRangeException.ThrowIfNegative(processedChunkCount);
    ArgumentOutOfRangeException.ThrowIfNegative(retainedStateCurrentCount);
    ArgumentOutOfRangeException.ThrowIfNegative(retainedStateHighWaterCount);
    ArgumentNullException.ThrowIfNull(fallbackCauseKinds);
    ArgumentNullException.ThrowIfNull(chunkedStateFallbackCauseKinds);
    ArgumentNullException.ThrowIfNull(unsupportedShapeKinds);
    if (duration < TimeSpan.Zero) {
      throw new ArgumentOutOfRangeException(nameof(duration), "Telemetry durations must not be negative.");
    }

    OperationKind = operationKind;
    Outcome = outcome;
    RequestCount = requestCount;
    HubOperationCount = hubOperationCount;
    LinkOperationCount = linkOperationCount;
    SatelliteOperationCount = satelliteOperationCount;
    RowsWritten = rowsWritten;
    SavedRecordCount = savedRecordCount;
    Duration = duration;
    StrategyStatus = strategyStatus;
    ProviderName = providerName;
    SelectedStrategyName = selectedStrategyName;
    FallbackCauseKinds = fallbackCauseKinds.Distinct().ToArray();
    ChunkCount = chunkCount;
    ProcessedChunkCount = processedChunkCount;
    RetainedStateCurrentCount = retainedStateCurrentCount;
    RetainedStateHighWaterCount = retainedStateHighWaterCount;
    ChunkedStateFallbackCauseKinds = chunkedStateFallbackCauseKinds.Distinct().ToArray();
    UnsupportedShapeKinds = unsupportedShapeKinds.Distinct().ToArray();
    FallbackExplanations = DataVaultSaveTelemetryExplanationCatalog.ExplainSaveStrategyFallbacks(FallbackCauseKinds);
    ChunkedStateFallbackExplanations =
        DataVaultSaveTelemetryExplanationCatalog.ExplainChunkedStateFallbacks(ChunkedStateFallbackCauseKinds);
    UnsupportedShapeExplanations =
        DataVaultSaveTelemetryExplanationCatalog.ExplainUnsupportedShapes(UnsupportedShapeKinds);
    ChunkedTransactionExplanation =
        DataVaultSaveTelemetryExplanationCatalog.ExplainChunkedTransaction(OperationKind);
  }

  /// <summary>
  /// Gets whether the attempt used the single-request or bulk save API.
  /// </summary>
  public DataVaultSaveTelemetryOperationKind OperationKind { get; }

  /// <summary>
  /// Gets whether the attempt returned successfully or failed.
  /// </summary>
  public DataVaultTelemetryOutcome Outcome { get; }

  /// <summary>
  /// Gets the number of explicit save requests in the attempt.
  /// </summary>
  public int RequestCount { get; }

  /// <summary>
  /// Gets the number of hub operations in the attempt.
  /// </summary>
  public int HubOperationCount { get; }

  /// <summary>
  /// Gets the number of link operations in the attempt.
  /// </summary>
  public int LinkOperationCount { get; }

  /// <summary>
  /// Gets the number of satellite operations in the attempt.
  /// </summary>
  public int SatelliteOperationCount { get; }

  /// <summary>
  /// Gets the total hub, link, and satellite operation count in the attempt.
  /// </summary>
  public int OperationCount => HubOperationCount + LinkOperationCount + SatelliteOperationCount;

  /// <summary>
  /// Gets the number of rows reported by the completed save result, or zero for failed attempts.
  /// </summary>
  public int RowsWritten { get; }

  /// <summary>
  /// Gets the number of saved-record summaries reported by the completed save result, or zero for failed attempts.
  /// </summary>
  public int SavedRecordCount { get; }

  /// <summary>
  /// Gets the elapsed duration of the attempt.
  /// </summary>
  public TimeSpan Duration { get; }

  /// <summary>
  /// Gets the request-bound provider-strategy selection status.
  /// </summary>
  public DataVaultSaveStrategyDiagnosticsStatus StrategyStatus { get; }

  /// <summary>
  /// Gets the Entity Framework provider name, when available.
  /// </summary>
  public string? ProviderName { get; }

  /// <summary>
  /// Gets the selected provider strategy type name, when a provider strategy was selected.
  /// </summary>
  public string? SelectedStrategyName { get; }

  /// <summary>
  /// Gets distinct finite fallback-cause kinds observed when provider-neutral fallback was selected.
  /// </summary>
  public IReadOnlyList<DataVaultSaveStrategyFallbackCauseKind> FallbackCauseKinds { get; }

  /// <summary>
  /// Gets bounded explanation and remediation text for each provider-specific save-strategy fallback cause.
  /// </summary>
  public IReadOnlyList<DataVaultSaveStrategyFallbackExplanation> FallbackExplanations { get; }

  /// <summary>
  /// Gets the number of chunks observed during a chunked save attempt.
  /// </summary>
  public int ChunkCount { get; }

  /// <summary>
  /// Gets the number of non-empty chunks processed during a chunked save attempt.
  /// </summary>
  public int ProcessedChunkCount { get; }

  /// <summary>
  /// Gets the retained satellite continuity-state count when the summary was emitted.
  /// </summary>
  public int RetainedStateCurrentCount { get; }

  /// <summary>
  /// Gets the highest retained satellite continuity-state count observed during the attempt.
  /// </summary>
  public int RetainedStateHighWaterCount { get; }

  /// <summary>
  /// Gets distinct finite retained-state fallback causes observed during chunked execution.
  /// </summary>
  public IReadOnlyList<DataVaultChunkedSaveStateFallbackCauseKind> ChunkedStateFallbackCauseKinds { get; }

  /// <summary>
  /// Gets bounded explanation and remediation text for each chunked retained-state fallback cause.
  /// </summary>
  public IReadOnlyList<DataVaultChunkedSaveStateFallbackExplanation> ChunkedStateFallbackExplanations { get; }

  /// <summary>
  /// Gets distinct finite unsupported or memory-sensitive shape classifications observed during chunked execution.
  /// </summary>
  public IReadOnlyList<DataVaultChunkedSaveUnsupportedShapeKind> UnsupportedShapeKinds { get; }

  /// <summary>
  /// Gets bounded explanation and remediation text for each chunked unsupported or memory-sensitive shape classification.
  /// </summary>
  public IReadOnlyList<DataVaultChunkedSaveUnsupportedShapeExplanation> UnsupportedShapeExplanations { get; }

  /// <summary>
  /// Gets bounded transaction guidance for chunked save attempts.
  /// </summary>
  public DataVaultChunkedSaveTransactionExplanation? ChunkedTransactionExplanation { get; }
}
