using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Privacy;

internal sealed class DataVaultPrivacyPersonalDataCoverageProof(
    IDataVaultPrivacyConfiguration configuration) : IDataVaultPersonalDataCoverageProof {
  public DataVaultPersonalDataCoverageEvaluation EvaluateEncryptedPayloadAlias(string encryptedPayloadAlias) {
    if (string.IsNullOrWhiteSpace(encryptedPayloadAlias)) {
      return new DataVaultPersonalDataCoverageEvaluation(
          isPrivacyProofConfigured: true,
          isUsableCoverageAvailable: false,
          "The encrypted-payload alias is blank.",
          "proof-evaluation-unavailable");
    }

    if (!configuration.EncryptedPayloadAliases.Contains(encryptedPayloadAlias, StringComparer.Ordinal)) {
      return new DataVaultPersonalDataCoverageEvaluation(
          isPrivacyProofConfigured: true,
          isUsableCoverageAvailable: false,
          "the active DVault privacy proof has not registered encrypted payload alias '" +
          encryptedPayloadAlias +
          "'.",
          "alias-unregistered");
    }

    if (configuration.KeyProvider is null) {
      return new DataVaultPersonalDataCoverageEvaluation(
          isPrivacyProofConfigured: true,
          isUsableCoverageAvailable: false,
          "encrypted payload alias '" +
          encryptedPayloadAlias +
          "' requires a caller-owned DVault privacy key provider.",
          "unusable-key-provider-posture");
    }

    if (configuration.KeyProvider is not IDataVaultEncryptedPayloadKeyProvider) {
      return new DataVaultPersonalDataCoverageEvaluation(
          isPrivacyProofConfigured: true,
          isUsableCoverageAvailable: false,
          "encrypted payload alias '" +
          encryptedPayloadAlias +
          "' requires a caller-owned DVault encrypted payload key provider.",
          "unusable-key-provider-posture");
    }

    return new DataVaultPersonalDataCoverageEvaluation(
        isPrivacyProofConfigured: true,
        isUsableCoverageAvailable: true,
        "encrypted payload alias '" + encryptedPayloadAlias + "' has usable caller-owned converter coverage.",
        "covered");
  }
}
