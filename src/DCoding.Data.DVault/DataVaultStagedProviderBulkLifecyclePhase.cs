namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the bounded staged-provider bulk save lifecycle phase reported by diagnostics.
/// </summary>
public enum DataVaultStagedProviderBulkLifecyclePhase {
  /// <summary>
  /// Staged-provider bulk evaluation was not attempted.
  /// </summary>
  NotEvaluated,

  /// <summary>
  /// The provider strategy evaluated whether the request batch was eligible for staged bulk execution.
  /// </summary>
  EligibilityEvaluation,

  /// <summary>
  /// The provider strategy prepared transient staging infrastructure.
  /// </summary>
  StageSetup,

  /// <summary>
  /// The provider strategy staged bounded hub, link, or satellite operations.
  /// </summary>
  StagePopulation,

  /// <summary>
  /// The provider strategy attempted provider-native bulk application from staged rows.
  /// </summary>
  NativeBulkApplication,

  /// <summary>
  /// The provider strategy cleaned up transient staging infrastructure.
  /// </summary>
  Cleanup,

  /// <summary>
  /// The provider strategy declined staged execution before provider-neutral fallback.
  /// </summary>
  Declined,

  /// <summary>
  /// Staged execution started but resolved to provider-neutral fallback.
  /// </summary>
  Fallback,

  /// <summary>
  /// Staged execution completed without staged fallback causes.
  /// </summary>
  Completed,
}
