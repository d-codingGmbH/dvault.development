namespace DCoding.Data.DVault;

/// <summary>
/// Classifies the provider caveat associated with staged-provider bulk save diagnostics.
/// </summary>
public enum DataVaultStagedProviderBulkProviderCaveatKind {
  /// <summary>
  /// No staged-provider caveat was reported.
  /// </summary>
  None,

  /// <summary>
  /// The DbContext state prevented staged-provider bulk execution from isolating the explicit DVault batch.
  /// </summary>
  DirtyContext,

  /// <summary>
  /// The request shape was not eligible for the staged-provider bulk path.
  /// </summary>
  UnsupportedShape,

  /// <summary>
  /// The staged-provider path could not participate in the caller-owned transaction contract.
  /// </summary>
  TransactionParticipation,

  /// <summary>
  /// Staging cleanup prevented the provider strategy from completing the staged path safely.
  /// </summary>
  Cleanup,

  /// <summary>
  /// A bounded provider limitation prevented staged-provider bulk execution.
  /// </summary>
  ProviderLimitation,
}
