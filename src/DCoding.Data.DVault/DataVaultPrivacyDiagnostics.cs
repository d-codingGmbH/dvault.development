namespace DCoding.Data.DVault;

/// <summary>
/// Structured redaction-safe privacy adoption facts emitted by diagnostics and support bundles.
/// </summary>
public sealed record DataVaultPrivacyDiagnostics(
    DataVaultProviderNativeEncryptionBoundaryFact ProviderNativeEncryption,
    string KeyProviderPosture,
    IReadOnlyList<DataVaultPrivacyAliasCoverageFact> AliasCoverages,
    IReadOnlyList<DataVaultPrivacyPersonalDataCoverageFact> PersonalDataCoverages) {
  /// <summary>
  /// Gets reviewed static provider-native crypto capability facts for the selected provider or provider profile.
  /// </summary>
  public IReadOnlyList<DataVaultProviderCryptoCapabilityFact> ProviderCryptoCapabilities { get; init; } =
      Array.Empty<DataVaultProviderCryptoCapabilityFact>();

  /// <summary>
  /// Gets explicit provider-native crypto selection facts after fail-closed validation.
  /// </summary>
  public IReadOnlyList<DataVaultProviderNativeCryptoSelectionFact> ProviderNativeCryptoSelections { get; init; } =
      Array.Empty<DataVaultProviderNativeCryptoSelectionFact>();

  /// <summary>
  /// Creates an empty provider-neutral privacy diagnostics payload.
  /// </summary>
  public static DataVaultPrivacyDiagnostics Empty { get; } = new(
      new DataVaultProviderNativeEncryptionBoundaryFact(
          ProviderName: null,
          CapabilityProfileName: "unknown",
          BoundaryStatus: "unmanaged",
          GuidanceStatus: "guidance-only",
          ManagedByDVault: false,
          UsesDatabaseCapabilityProbing: false,
          Source: "docs/architecture/dvault-v1-optional-privacy-extension-boundary.md",
          Message: "Provider-native encryption remains unmanaged and guidance-only for DVault; diagnostics do not probe database encryption settings or route runtime behavior based on native encryption availability."),
      "none",
      Array.Empty<DataVaultPrivacyAliasCoverageFact>(),
      Array.Empty<DataVaultPrivacyPersonalDataCoverageFact>()) {
    ProviderCryptoCapabilities = Array.Empty<DataVaultProviderCryptoCapabilityFact>(),
    ProviderNativeCryptoSelections = Array.Empty<DataVaultProviderNativeCryptoSelectionFact>(),
  };
}
