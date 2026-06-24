namespace DCoding.Data.DVault;

internal enum DataVaultPitMaintenanceStrategyFallbackCauseKind {
  ProviderNameMismatch,
  UnknownOrUnregisteredProviderName,
  NoProviderSpecificStrategyRegistered,
  DirtyDbContext,
  CurrentTransactionSavepointUnavailable,
  UnsupportedPitShape,
  IncompleteMaintenanceShapeEvidence,
  StrategyDeclined,
}
