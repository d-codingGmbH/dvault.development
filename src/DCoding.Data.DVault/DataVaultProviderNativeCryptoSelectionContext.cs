namespace DCoding.Data.DVault;

/// <summary>
/// Supplies active provider facts for validating explicit provider-native crypto selection requests.
/// </summary>
/// <param name="ProviderName">The active EF Core provider name, when diagnostics know one.</param>
/// <param name="CapabilityProfileName">The active DVault provider capability profile name.</param>
/// <param name="CapabilityProfileDefaulted">A value indicating whether the capability profile was defaulted because the provider was unknown.</param>
/// <param name="ReviewedCapabilities">The reviewed static crypto capability facts for the active provider profile.</param>
public sealed record DataVaultProviderNativeCryptoSelectionContext(
    string? ProviderName,
    string CapabilityProfileName,
    bool CapabilityProfileDefaulted,
    IReadOnlyList<DataVaultProviderCryptoCapabilityFact> ReviewedCapabilities);
