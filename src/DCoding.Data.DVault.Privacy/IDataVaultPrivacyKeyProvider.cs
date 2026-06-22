namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Marks a caller-owned key-provider dependency for explicit privacy flows.
/// </summary>
/// <remarks>
/// The marker does not give DVault ownership of key material, cryptographic policy, approval decisions,
/// key storage, key rotation, or compliance behavior.
/// </remarks>
public interface IDataVaultPrivacyKeyProvider {
}
