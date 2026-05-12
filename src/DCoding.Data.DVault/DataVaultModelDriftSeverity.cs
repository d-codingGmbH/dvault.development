namespace DCoding.Data.DVault;

/// <summary>
/// Classifies one model drift difference as informational or blocking.
/// </summary>
public enum DataVaultModelDriftSeverity {
  /// <summary>
  /// The difference is informational and does not make the generated metadata incompatible.
  /// </summary>
  Informational,

  /// <summary>
  /// The difference is blocking because generated metadata is incompatible with the expected model.
  /// </summary>
  Blocking,
}
