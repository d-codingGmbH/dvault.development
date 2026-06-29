namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics fact for one registered encrypted-payload alias.
/// </summary>
public sealed record DataVaultPrivacyAliasCoverageFact(
    string EncryptedPayloadAlias,
    string CoverageStatus,
    IReadOnlyList<DataVaultPrivacyCoveredPropertyFact> CoveredProperties);
