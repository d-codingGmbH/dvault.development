namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Identifies the explicit encrypted-payload conversion direction requested from a caller-owned key provider.
/// </summary>
public enum DataVaultEncryptedPayloadConversionDirection {
  /// <summary>
  /// Convert an application payload value to the encrypted provider value that will be persisted.
  /// </summary>
  Encrypt = 0,

  /// <summary>
  /// Convert a persisted encrypted provider value back to the application payload value.
  /// </summary>
  Decrypt = 1,
}
