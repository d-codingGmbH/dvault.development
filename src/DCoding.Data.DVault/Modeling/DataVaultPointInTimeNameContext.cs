namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes a point-in-time table name request.
/// </summary>
public sealed record DataVaultPointInTimeNameContext(
    string PointInTimeName,
    string HubName,
    IReadOnlyList<string> SatelliteNames);
