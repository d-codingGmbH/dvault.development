namespace DCoding.Data.DVault;

/// <summary>
/// Evaluates whether an opt-in privacy proof covers a marked personal-data encrypted-payload alias.
/// </summary>
public interface IDataVaultPersonalDataCoverageProof {
  /// <summary>
  /// Evaluates privacy coverage for one encrypted-payload alias.
  /// </summary>
  /// <param name="encryptedPayloadAlias">The stable provider-neutral encrypted-payload alias.</param>
  /// <returns>The provider-neutral coverage evaluation.</returns>
  DataVaultPersonalDataCoverageEvaluation EvaluateEncryptedPayloadAlias(string encryptedPayloadAlias);
}
