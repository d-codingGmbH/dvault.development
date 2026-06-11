namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one produced point-in-time table field and its key participation.
/// </summary>
public sealed record DataVaultPointInTimeField(
    string Name,
    DataVaultPointInTimeColumnKind Kind,
    string? SatelliteName,
    int? KeyOrdinal);
