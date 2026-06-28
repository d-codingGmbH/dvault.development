namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable finding emitted by the hash-key storage migration manifest validator.
/// </summary>
public sealed record DataVaultHashKeyStorageMigrationValidationFinding(
    DataVaultDiagnosticsIssueSeverity Severity,
    string Code,
    string? TableName,
    string? ColumnName,
    string Path,
    string? ExpectedValue,
    string? ActualValue,
    string Message);
