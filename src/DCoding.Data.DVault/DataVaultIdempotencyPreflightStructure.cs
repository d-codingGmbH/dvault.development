namespace DCoding.Data.DVault;

/// <summary>
/// Expected provider-shaped schema structure used by Data Vault idempotency and bounded read operations.
/// </summary>
public sealed record DataVaultIdempotencyPreflightStructure(
    string TableName,
    string OperationFamily,
    string Kind,
    string Name,
    IReadOnlyList<string> ColumnNames,
    bool IsUnique,
    IReadOnlyList<string> DescendingColumnNames,
    IReadOnlyList<string> IncludedColumnNames);
