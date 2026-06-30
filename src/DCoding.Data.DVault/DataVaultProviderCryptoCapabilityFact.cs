namespace DCoding.Data.DVault;

/// <summary>
/// Describes one reviewed provider-native crypto capability without probing a database.
/// </summary>
public sealed record DataVaultProviderCryptoCapabilityFact(
    string? ProviderName,
    string CapabilityProfileName,
    string CapabilityFamily,
    string CapabilityLabel,
    string CapabilityKind,
    string Status,
    string Guidance);
