namespace DCoding.Data.DVault;

/// <summary>
/// Bounded explanation and remediation text for one provider-specific save-strategy fallback cause.
/// </summary>
public sealed record DataVaultSaveStrategyFallbackExplanation(
    DataVaultSaveStrategyFallbackCauseKind Kind,
    string Explanation,
    string Remediation);
