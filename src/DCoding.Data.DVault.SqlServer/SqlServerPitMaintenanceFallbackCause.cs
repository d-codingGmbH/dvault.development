namespace DCoding.Data.DVault;

internal sealed record SqlServerPitMaintenanceFallbackCause(
    SqlServerPitMaintenanceFallbackCauseKind Kind,
    string Detail);
