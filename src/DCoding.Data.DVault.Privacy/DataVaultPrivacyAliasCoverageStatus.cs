namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Classifies whether one registered encrypted-payload alias is covered by mapped EF properties.
/// </summary>
public enum DataVaultPrivacyAliasCoverageStatus {
  /// <summary>
  /// At least one mapped EF property uses the encrypted-payload converter for the alias.
  /// </summary>
  Covered = 0,

  /// <summary>
  /// The alias is registered but no mapped EF property uses the encrypted-payload converter for it.
  /// </summary>
  RegisteredButUnmapped = 1,
}
