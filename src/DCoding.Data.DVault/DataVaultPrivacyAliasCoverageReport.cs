namespace DCoding.Data.DVault;

/// <summary>
/// Provider-neutral alias coverage report supplied by optional privacy extensions.
/// </summary>
public sealed record DataVaultPrivacyAliasCoverageReport(
    string KeyProviderPosture,
    IReadOnlyList<DataVaultPrivacyAliasCoverageFact> AliasCoverages);
