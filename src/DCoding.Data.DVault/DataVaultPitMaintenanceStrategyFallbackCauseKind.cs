namespace DCoding.Data.DVault;

internal enum DataVaultPitMaintenanceStrategyFallbackCauseKind {
  ProviderNameMismatch,
  UnknownOrUnregisteredProviderName,
  NoProviderSpecificStrategyRegistered,
  DirtyDbContext,
  UnsupportedPitShape,
  IncompleteMaintenanceShapeEvidence,
  StrategyDeclined,
}
