namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes an index name request.
/// </summary>
public sealed record DataVaultIndexNameContext(
    DataVaultIndexKind Kind,
    string TableName,
    IReadOnlyList<string> ColumnNames,
    bool IsUnique);
