namespace DCoding.Data.DVault;

/// <summary>
/// Status assigned to one named Data Vault preflight section.
/// </summary>
public enum DataVaultPreflightSectionStatus {
  /// <summary>
  /// The section was evaluated and reported no blocking condition.
  /// </summary>
  Passed,

  /// <summary>
  /// The section was evaluated and reported a blocking condition.
  /// </summary>
  Blocked,

  /// <summary>
  /// The section was not evaluated because its explicit optional input was not supplied.
  /// </summary>
  Skipped,
}
