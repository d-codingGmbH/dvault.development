namespace DCoding.Data.DVault;

/// <summary>
/// Overall status assigned to a composed Data Vault preflight report.
/// </summary>
public enum DataVaultPreflightStatus {
  /// <summary>
  /// No evaluated preflight section reported a blocking condition.
  /// </summary>
  Passed,

  /// <summary>
  /// At least one evaluated preflight section reported a blocking condition.
  /// </summary>
  Blocked,
}
