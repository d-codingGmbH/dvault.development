namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one produced Data Vault column.
/// </summary>
public sealed record DataVaultColumn(string Name, DataVaultColumnKind Kind);
