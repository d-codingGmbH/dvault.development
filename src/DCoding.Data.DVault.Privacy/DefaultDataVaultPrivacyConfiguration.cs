namespace DCoding.Data.DVault.Privacy;

internal sealed class DefaultDataVaultPrivacyConfiguration(
    IReadOnlyList<string> encryptedPayloadAliases,
    IDataVaultPrivacyKeyProvider? keyProvider)
    : IDataVaultPrivacyConfiguration {
  public IReadOnlyList<string> EncryptedPayloadAliases { get; } = encryptedPayloadAliases;

  public IDataVaultPrivacyKeyProvider? KeyProvider { get; } = keyProvider;
}
