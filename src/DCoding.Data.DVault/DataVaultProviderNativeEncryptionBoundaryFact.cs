namespace DCoding.Data.DVault;

/// <summary>
/// Describes the provider-native encryption boundary without probing provider encryption settings.
/// </summary>
public sealed record DataVaultProviderNativeEncryptionBoundaryFact(
    string? ProviderName,
    string CapabilityProfileName,
    string BoundaryStatus,
    string GuidanceStatus,
    bool ManagedByDVault,
    bool UsesDatabaseCapabilityProbing,
    string Source,
    string Message);
