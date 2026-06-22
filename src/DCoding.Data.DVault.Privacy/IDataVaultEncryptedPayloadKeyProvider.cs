namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Resolves caller-owned encrypted payload conversion behavior for explicit privacy flows.
/// </summary>
/// <remarks>
/// DVault passes an encrypted-payload alias and payload value to this dependency, but the consuming
/// application owns key material, cryptographic policy, approval decisions, and key lifecycle behavior.
/// </remarks>
public interface IDataVaultEncryptedPayloadKeyProvider : IDataVaultPrivacyKeyProvider {
  /// <summary>
  /// Converts one payload value for the requested encrypted-payload alias.
  /// </summary>
  /// <param name="request">The provider-neutral conversion request.</param>
  /// <returns>The caller-approved converted value, or an explicit declined result.</returns>
  DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
      DataVaultEncryptedPayloadConversionRequest request);
}
