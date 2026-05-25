namespace DCoding.Data.DVault;

/// <summary>
/// Identifies unsupported or memory-sensitive chunked-save shapes using a finite redacted vocabulary.
/// </summary>
public enum DataVaultChunkedSaveUnsupportedShapeKind {
  /// <summary>
  /// The attempt touched more satellite continuity series than the v1 retained-state limit keeps in memory.
  /// </summary>
  RetainedSatelliteSeriesLimitExceeded,
}
