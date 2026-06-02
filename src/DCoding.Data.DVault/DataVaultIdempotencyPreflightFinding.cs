namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable idempotency preflight finding scoped to one Data Vault table and operation family.
/// </summary>
public sealed record DataVaultIdempotencyPreflightFinding(
    DataVaultModelDriftSeverity Severity,
    string Code,
    string TableName,
    string OperationFamily,
    string StructureKind,
    string StructureName,
    string PropertyPath,
    string? ExpectedValue,
    string? ActualValue,
    string Message);
