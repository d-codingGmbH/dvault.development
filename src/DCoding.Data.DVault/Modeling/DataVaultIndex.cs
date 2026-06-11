namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one produced Data Vault index.
/// </summary>
public sealed record DataVaultIndex(string Name, IReadOnlyList<string> ColumnNames, bool IsUnique);
