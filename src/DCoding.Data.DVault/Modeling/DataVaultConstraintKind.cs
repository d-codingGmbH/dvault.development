namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies constraint name families produced by the modeling flow.
/// </summary>
public enum DataVaultConstraintKind {
  /// <summary>
  /// Primary key constraint.
  /// </summary>
  PrimaryKey,

  /// <summary>
  /// Foreign key constraint.
  /// </summary>
  ForeignKey,
}
