namespace DCoding.Data.DVault;

internal sealed record DataVaultPitMaintenanceStrategyFallbackCause(
    DataVaultPitMaintenanceStrategyFallbackCauseKind Kind,
    string Message);
