namespace DCoding.Data.DVault;

internal enum SqlServerPitMaintenanceFallbackCauseKind {
  ProviderNameMismatch,
  DirtyDbContext,
  CurrentTransactionSavepointUnavailable,
  UnsupportedPitParent,
  MultiActivePitUnsupported,
  MaintainParentsUnsupported,
}
