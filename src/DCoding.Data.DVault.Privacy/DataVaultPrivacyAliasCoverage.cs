namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Machine-readable coverage facts for one registered encrypted-payload alias.
/// </summary>
public sealed record DataVaultPrivacyAliasCoverage(
    string EncryptedPayloadAlias,
    DataVaultPrivacyAliasCoverageStatus Status,
    IReadOnlyList<DataVaultPrivacyCoveredProperty> CoveredProperties);
