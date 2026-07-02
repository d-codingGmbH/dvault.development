namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the effectivity role carried by a satellite payload field.
/// </summary>
public enum DataVaultEffectivityRole {
  /// <summary>
  /// Field stores the lower bound of the effectivity window.
  /// </summary>
  EffectiveFrom,

  /// <summary>
  /// Field stores the optional upper bound of the effectivity window.
  /// </summary>
  EffectiveTo,

  /// <summary>
  /// Field stores an optional current-row marker or status value.
  /// </summary>
  CurrentFlag,
}
