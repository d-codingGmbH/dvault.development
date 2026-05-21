namespace DCoding.Data.DVault;

/// <summary>
/// Ordered machine-readable outcome for one inspected EF Core migration operation.
/// </summary>
/// <param name="Ordinal">The zero-based ordinal from the supplied migration operation sequence.</param>
/// <param name="OperationName">The deterministic EF Core migration operation name.</param>
/// <param name="TargetName">The primary table or target name when the operation exposes one.</param>
/// <param name="MemberName">The column, index, key, or member name when the operation exposes one.</param>
/// <param name="Path">The deterministic migration operation path.</param>
/// <param name="Outcome">The safe, risky, or incompatible outcome derived from the operation findings.</param>
/// <param name="Issues">The DVM migration guardrail findings produced by this operation.</param>
public sealed record DataVaultMigrationGuardrailOperationSummary(
    int Ordinal,
    string OperationName,
    string? TargetName,
    string? MemberName,
    string Path,
    DataVaultMigrationGuardrailOperationOutcome Outcome,
    IReadOnlyList<DataVaultMigrationGuardrailIssue> Issues);
