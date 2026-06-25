namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Classifies the configured caller-owned privacy key-provider posture without probing key material.
/// </summary>
public enum DataVaultPrivacyKeyProviderPosture {
  /// <summary>
  /// No caller-owned privacy key provider is configured.
  /// </summary>
  None = 0,

  /// <summary>
  /// A marker-only privacy key provider is configured but it cannot convert encrypted payloads.
  /// </summary>
  MarkerOnly = 1,

  /// <summary>
  /// A caller-owned encrypted-payload key provider is configured.
  /// </summary>
  EncryptedPayloadCapable = 2,
}
