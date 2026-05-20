namespace DCoding.Data.DVault;

/// <summary>
/// Outcome assigned to one explicit DVault telemetry attempt.
/// </summary>
public enum DataVaultTelemetryOutcome {
  /// <summary>
  /// The explicit save or read attempt completed successfully.
  /// </summary>
  Succeeded,

  /// <summary>
  /// The explicit save or read attempt failed before returning a caller-visible result.
  /// </summary>
  Failed,
}
