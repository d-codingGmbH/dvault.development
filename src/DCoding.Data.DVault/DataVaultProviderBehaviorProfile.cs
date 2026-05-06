namespace DCoding.Data.DVault;

/// <summary>
/// Describes the selected provider-behavior profile while keeping provider behavior separate from other hook categories.
/// </summary>
public sealed class DataVaultProviderBehaviorProfile {
  /// <summary>
  /// Initializes a provider-behavior profile.
  /// </summary>
  /// <param name="profileName">The deterministic provider-behavior profile name.</param>
  public DataVaultProviderBehaviorProfile(string profileName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(profileName);

    ProfileName = profileName;
  }

  /// <summary>
  /// Gets the deterministic provider-behavior profile name.
  /// </summary>
  public string ProfileName { get; }
}
