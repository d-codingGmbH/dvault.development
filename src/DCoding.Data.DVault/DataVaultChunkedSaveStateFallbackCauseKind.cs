namespace DCoding.Data.DVault;

/// <summary>
/// Identifies finite bounded-state fallback causes observed during one chunked save attempt.
/// </summary>
public enum DataVaultChunkedSaveStateFallbackCauseKind {
  /// <summary>
  /// The retained satellite continuity-state series limit was reached and the attempt used persisted per-chunk lookup fallback.
  /// </summary>
  RetainedSatelliteSeriesLimitReached,
}
