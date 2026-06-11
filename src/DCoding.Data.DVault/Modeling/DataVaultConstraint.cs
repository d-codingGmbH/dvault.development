namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one produced Data Vault constraint.
/// </summary>
public sealed record DataVaultConstraint(
    string Name,
    DataVaultConstraintKind Kind,
    IReadOnlyList<string> ColumnNames);
