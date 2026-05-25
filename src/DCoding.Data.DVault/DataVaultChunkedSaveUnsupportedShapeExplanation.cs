namespace DCoding.Data.DVault;

/// <summary>
/// Bounded explanation and remediation text for one chunked-save unsupported or memory-sensitive shape classification.
/// </summary>
public sealed record DataVaultChunkedSaveUnsupportedShapeExplanation(
    DataVaultChunkedSaveUnsupportedShapeKind Kind,
    string Explanation,
    string Remediation);
