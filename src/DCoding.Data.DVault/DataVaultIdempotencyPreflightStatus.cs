namespace DCoding.Data.DVault;

/// <summary>
/// Status assigned to one explicit Data Vault idempotency preflight evaluation.
/// </summary>
public enum DataVaultIdempotencyPreflightStatus {
  /// <summary>
  /// The check was evaluated and all idempotency-critical structures matched.
  /// </summary>
  Passed,

  /// <summary>
  /// The check was evaluated and at least one idempotency-critical structure was missing or mismatched.
  /// </summary>
  Blocked,

  /// <summary>
  /// The check was not evaluated because the caller did not supply explicit live-schema input.
  /// </summary>
  Skipped,

  /// <summary>
  /// The caller requested a live check for a provider without a supported live-schema reader.
  /// </summary>
  UnsupportedProvider,

  /// <summary>
  /// The caller requested a live check but the provider database or catalog was unavailable.
  /// </summary>
  UnavailableLiveSchema,
}
