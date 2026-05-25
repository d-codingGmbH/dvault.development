namespace DCoding.Data.DVault;

/// <summary>
/// Bounded transaction guidance for one chunked save attempt.
/// </summary>
public sealed record DataVaultChunkedSaveTransactionExplanation(
    string Explanation,
    string Remediation);
