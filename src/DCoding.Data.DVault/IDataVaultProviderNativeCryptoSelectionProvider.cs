namespace DCoding.Data.DVault;

/// <summary>
/// Supplies redaction-safe diagnostics for provider-owned explicit provider-native crypto selection requests.
/// </summary>
public interface IDataVaultProviderNativeCryptoSelectionProvider {
  /// <summary>
  /// Evaluates configured provider-native crypto selection requests against the active reviewed capability facts.
  /// </summary>
  /// <param name="context">The active provider and reviewed capability context.</param>
  /// <returns>Deterministic provider-native crypto selection facts.</returns>
  IReadOnlyList<DataVaultProviderNativeCryptoSelectionFact> Analyze(
      DataVaultProviderNativeCryptoSelectionContext context);
}
