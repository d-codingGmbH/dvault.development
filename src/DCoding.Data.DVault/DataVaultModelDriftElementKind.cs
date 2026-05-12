namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the kind of model element affected by a drift difference.
/// </summary>
public enum DataVaultModelDriftElementKind {
  /// <summary>
  /// The difference affects the whole EF model metadata.
  /// </summary>
  Model,

  /// <summary>
  /// The difference affects one generated Data Vault entity or table.
  /// </summary>
  Entity,

  /// <summary>
  /// The difference affects one generated Data Vault property or column.
  /// </summary>
  Property,

  /// <summary>
  /// The difference affects one generated Data Vault key.
  /// </summary>
  Key,

  /// <summary>
  /// The difference affects one generated Data Vault index.
  /// </summary>
  Index,
}
