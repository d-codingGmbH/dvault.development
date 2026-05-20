namespace DCoding.Data.DVault;

/// <summary>
/// Bounded summary emitted for one explicit DVault read attempt.
/// </summary>
public sealed class DataVaultReadTelemetrySummary {
  /// <summary>
  /// Initializes a new read telemetry summary.
  /// </summary>
  /// <param name="family">The public read family used by the attempt.</param>
  /// <param name="outcome">Whether the attempt returned successfully or failed.</param>
  /// <param name="requestedKeyCount">The number of requested parent or endpoint hash keys.</param>
  /// <param name="returnedRowCount">The number of rows returned by the completed read, or zero for failed attempts.</param>
  /// <param name="duration">The elapsed duration of the attempt.</param>
  /// <param name="strategyStatus">The request-bound provider-strategy selection status.</param>
  /// <param name="providerName">The Entity Framework provider name, when available.</param>
  /// <param name="selectedStrategyName">The selected provider strategy type name, when a provider strategy was selected.</param>
  /// <param name="fallbackCauseKinds">The distinct finite fallback-cause kinds observed when provider-neutral fallback was selected.</param>
  public DataVaultReadTelemetrySummary(
      DataVaultReadTelemetryFamily family,
      DataVaultTelemetryOutcome outcome,
      int requestedKeyCount,
      int returnedRowCount,
      TimeSpan duration,
      DataVaultReadStrategyDiagnosticsStatus strategyStatus,
      string? providerName,
      string? selectedStrategyName,
      IEnumerable<DataVaultReadStrategyFallbackCauseKind> fallbackCauseKinds) {
    ArgumentOutOfRangeException.ThrowIfNegative(requestedKeyCount);
    ArgumentOutOfRangeException.ThrowIfNegative(returnedRowCount);
    ArgumentNullException.ThrowIfNull(fallbackCauseKinds);
    if (duration < TimeSpan.Zero) {
      throw new ArgumentOutOfRangeException(nameof(duration), "Telemetry durations must not be negative.");
    }

    Family = family;
    Outcome = outcome;
    RequestedKeyCount = requestedKeyCount;
    ReturnedRowCount = returnedRowCount;
    Duration = duration;
    StrategyStatus = strategyStatus;
    ProviderName = providerName;
    SelectedStrategyName = selectedStrategyName;
    FallbackCauseKinds = fallbackCauseKinds.Distinct().ToArray();
  }

  /// <summary>
  /// Gets the public read family used by the attempt.
  /// </summary>
  public DataVaultReadTelemetryFamily Family { get; }

  /// <summary>
  /// Gets whether the attempt returned successfully or failed.
  /// </summary>
  public DataVaultTelemetryOutcome Outcome { get; }

  /// <summary>
  /// Gets the number of requested parent or endpoint hash keys.
  /// </summary>
  public int RequestedKeyCount { get; }

  /// <summary>
  /// Gets the number of rows returned by the completed read, or zero for failed attempts.
  /// </summary>
  public int ReturnedRowCount { get; }

  /// <summary>
  /// Gets the elapsed duration of the attempt.
  /// </summary>
  public TimeSpan Duration { get; }

  /// <summary>
  /// Gets the request-bound provider-strategy selection status.
  /// </summary>
  public DataVaultReadStrategyDiagnosticsStatus StrategyStatus { get; }

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
  public IReadOnlyList<DataVaultReadStrategyFallbackCauseKind> FallbackCauseKinds { get; }
}
