namespace DCoding.Data.DVault;

/// <summary>
/// Reports whether an opt-in privacy proof can cover one marked personal-data payload alias.
/// </summary>
public sealed class DataVaultPersonalDataCoverageEvaluation {
  /// <summary>
  /// Initializes a new personal-data coverage evaluation.
  /// </summary>
  /// <param name="isPrivacyProofConfigured">A value indicating whether an opt-in privacy proof is configured.</param>
  /// <param name="isUsableCoverageAvailable">A value indicating whether the marked alias has usable privacy coverage.</param>
  /// <param name="message">Provider-neutral coverage guidance suitable for diagnostics.</param>
  public DataVaultPersonalDataCoverageEvaluation(
      bool isPrivacyProofConfigured,
      bool isUsableCoverageAvailable,
      string message)
      : this(
          isPrivacyProofConfigured,
          isUsableCoverageAvailable,
          message,
          coverageStatus: null) {
  }

  /// <summary>
  /// Initializes a new personal-data coverage evaluation.
  /// </summary>
  /// <param name="isPrivacyProofConfigured">A value indicating whether an opt-in privacy proof is configured.</param>
  /// <param name="isUsableCoverageAvailable">A value indicating whether the marked alias has usable privacy coverage.</param>
  /// <param name="message">Provider-neutral coverage guidance suitable for diagnostics.</param>
  /// <param name="coverageStatus">Optional structured coverage status supplied by the proof.</param>
  public DataVaultPersonalDataCoverageEvaluation(
      bool isPrivacyProofConfigured,
      bool isUsableCoverageAvailable,
      string message,
      string? coverageStatus) {
    IsPrivacyProofConfigured = isPrivacyProofConfigured;
    IsUsableCoverageAvailable = isUsableCoverageAvailable;
    Message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
    CoverageStatus = string.IsNullOrWhiteSpace(coverageStatus) ? null : coverageStatus;
  }

  /// <summary>
  /// Gets a value indicating whether an opt-in privacy proof is configured.
  /// </summary>
  public bool IsPrivacyProofConfigured { get; }

  /// <summary>
  /// Gets a value indicating whether the marked alias has usable privacy coverage.
  /// </summary>
  public bool IsUsableCoverageAvailable { get; }

  /// <summary>
  /// Gets provider-neutral coverage guidance suitable for diagnostics.
  /// </summary>
  public string Message { get; }

  /// <summary>
  /// Gets the optional structured coverage status supplied by the proof.
  /// </summary>
  public string? CoverageStatus { get; }
}
