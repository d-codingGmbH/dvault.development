namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes a point-in-time table column name request.
/// </summary>
public sealed record DataVaultPointInTimeColumnNameContext(
    DataVaultPointInTimeColumnKind Kind,
    string PointInTimeName,
    string HubName,
    string? SatelliteName,
    string OwnerTableName);
