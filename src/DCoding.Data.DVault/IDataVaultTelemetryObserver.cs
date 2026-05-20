namespace DCoding.Data.DVault;

/// <summary>
/// Receives bounded telemetry summaries for explicit DVault save and read attempts.
/// </summary>
public interface IDataVaultTelemetryObserver {
  /// <summary>
  /// Records one explicit save attempt summary.
  /// </summary>
  /// <param name="summary">The bounded save telemetry summary.</param>
  void RecordSave(DataVaultSaveTelemetrySummary summary);

  /// <summary>
  /// Records one explicit read attempt summary.
  /// </summary>
  /// <param name="summary">The bounded read telemetry summary.</param>
  void RecordRead(DataVaultReadTelemetrySummary summary);
}
