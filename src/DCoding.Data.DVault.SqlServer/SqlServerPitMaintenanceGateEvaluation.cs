namespace DCoding.Data.DVault;

internal sealed record SqlServerPitMaintenanceGateEvaluation(
    bool CanRebuild,
    IReadOnlyList<SqlServerPitMaintenanceFallbackCause> FallbackCauses);
