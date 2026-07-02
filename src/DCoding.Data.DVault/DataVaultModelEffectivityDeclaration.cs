namespace DCoding.Data.DVault;

internal sealed record DataVaultModelEffectivityDeclaration(
    string EffectiveFrom,
    string? EffectiveTo,
    string? CurrentFlag,
    string Path);
