namespace DCoding.Data.DVault;

/// <summary>
/// Bounded explanation and remediation text for one chunked-save retained-state fallback cause.
/// </summary>
public sealed record DataVaultChunkedSaveStateFallbackExplanation(
    DataVaultChunkedSaveStateFallbackCauseKind Kind,
    string Explanation,
    string Remediation);
