namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Exposes the opt-in privacy extension skeleton configuration registered by the application.
/// </summary>
public interface IDataVaultPrivacyConfiguration {
  /// <summary>
  /// Gets the provider-neutral encrypted-payload aliases registered for future explicit privacy flows.
  /// </summary>
  IReadOnlyList<string> EncryptedPayloadAliases { get; }

  /// <summary>
  /// Gets the caller-owned key-provider placeholder, when one was registered.
  /// </summary>
  IDataVaultPrivacyKeyProvider? KeyProvider { get; }
}
