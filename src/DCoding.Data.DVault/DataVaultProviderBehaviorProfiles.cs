namespace DCoding.Data.DVault;

/// <summary>
/// Provides built-in provider-behavior profiles.
/// </summary>
public static class DataVaultProviderBehaviorProfiles {
  /// <summary>
  /// Gets the provider-neutral baseline inherited when no provider-behavior override applies.
  /// </summary>
  public static DataVaultProviderBehaviorProfile ProviderNeutral { get; } = new("provider-neutral-v1");
}
