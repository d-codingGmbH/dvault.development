namespace DCoding.Data.DVault;

/// <summary>
/// Describes one explicit provider-native crypto selection request after redaction-safe validation.
/// </summary>
/// <param name="ProviderName">The active EF Core provider name, when diagnostics know one.</param>
/// <param name="EncryptedPayloadAlias">The stable provider-neutral encrypted-payload alias selected by the caller.</param>
/// <param name="ProviderPackageName">The provider package or extension package that owns the native selection surface.</param>
/// <param name="CapabilityProfileName">The DVault provider capability profile requested by the provider-owned selection.</param>
/// <param name="CapabilityFamily">The exact reviewed provider-native capability family requested by the provider-owned selection.</param>
/// <param name="CapabilityLabel">The reviewed capability label when the request matched a known static capability fact.</param>
/// <param name="CapabilityKind">The reviewed capability kind when the request matched a known static capability fact.</param>
/// <param name="CapabilityStatus">The reviewed capability status when the request matched a known static capability fact.</param>
/// <param name="SelectionStatus">The fail-closed selection status for this request.</param>
/// <param name="Message">The redaction-safe diagnostic message for this request.</param>
public sealed record DataVaultProviderNativeCryptoSelectionFact(
    string? ProviderName,
    string EncryptedPayloadAlias,
    string ProviderPackageName,
    string CapabilityProfileName,
    string CapabilityFamily,
    string? CapabilityLabel,
    string? CapabilityKind,
    string? CapabilityStatus,
    string SelectionStatus,
    string Message);
