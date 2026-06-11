namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the physical storage profile used for Data Vault hash-key values.
/// </summary>
public enum DataVaultHashKeyStorageProfile {
  /// <summary>
  /// Hash keys are persisted as canonical lowercase hexadecimal text.
  /// </summary>
  HexString,

  /// <summary>
  /// Hash keys are persisted as digest bytes while the logical model boundary remains lowercase hexadecimal text.
  /// </summary>
  Binary,
}
