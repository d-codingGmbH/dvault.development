namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation of one ordered Data Vault link participant.
/// </summary>
public sealed record DataVaultLinkParticipantExplain(
    string Hub,
    string Name,
    string? Role,
    string ProducedName,
    string ColumnName,
    int Ordinal);
