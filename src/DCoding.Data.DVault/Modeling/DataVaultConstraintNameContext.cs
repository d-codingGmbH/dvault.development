namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes a constraint name request.
/// </summary>
public sealed record DataVaultConstraintNameContext(
    DataVaultConstraintKind Kind,
    string TableName,
    IReadOnlyList<string> ColumnNames);
