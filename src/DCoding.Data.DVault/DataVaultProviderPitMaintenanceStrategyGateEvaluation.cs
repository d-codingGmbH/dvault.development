namespace DCoding.Data.DVault;

internal sealed record DataVaultProviderPitMaintenanceStrategyGateEvaluation(
    bool CanRebuild,
    IReadOnlyList<DataVaultPitMaintenanceStrategyFallbackCause> FallbackCauses);
