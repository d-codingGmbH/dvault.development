namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Exposes the opt-in privacy extension proof configuration registered by the application.
/// </summary>
public interface IDataVaultPrivacyConfiguration {
  /// <summary>
  /// Gets the provider-neutral encrypted-payload aliases registered for explicit privacy flows.
  /// </summary>
  IReadOnlyList<string> EncryptedPayloadAliases { get; }

  /// <summary>
  /// Gets the caller-owned key provider, when one was registered.
  /// </summary>
  IDataVaultPrivacyKeyProvider? KeyProvider { get; }
}
