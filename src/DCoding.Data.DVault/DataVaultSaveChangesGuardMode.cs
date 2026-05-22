namespace DCoding.Data.DVault;

/// <summary>
/// Selects how the optional Data Vault SaveChanges guard handles unsafe generated-row changes.
/// </summary>
public enum DataVaultSaveChangesGuardMode {
  /// <summary>
  /// Throws a DataVaultSaveChangesGuardException before SaveChanges persists unsafe generated-row changes.
  /// </summary>
  Blocking,

  /// <summary>
  /// Emits a deterministic warning report to the configured caller callback and allows SaveChanges to continue.
  /// </summary>
  Warning,
}
