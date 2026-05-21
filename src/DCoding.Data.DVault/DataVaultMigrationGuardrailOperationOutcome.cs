namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable guardrail outcome for one inspected EF Core migration operation.
/// </summary>
public enum DataVaultMigrationGuardrailOperationOutcome {
  /// <summary>
  /// The operation produced no DVM migration guardrail findings.
  /// </summary>
  Safe,

  /// <summary>
  /// The operation produced warning-severity DVM migration guardrail findings only.
  /// </summary>
  Risky,

  /// <summary>
  /// The operation produced one or more error-severity DVM migration guardrail findings.
  /// </summary>
  Incompatible,
}
