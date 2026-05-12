namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable description of one deterministic difference between expected and current Data Vault EF metadata.
/// </summary>
public sealed record DataVaultModelDriftDifference(
    DataVaultModelDriftSeverity Severity,
    string Code,
    DataVaultModelDriftElementKind ElementKind,
    string LogicalName,
    string? ProducedName,
    string PropertyPath,
    string? ExpectedValue,
    string? ActualValue,
    string Message);
