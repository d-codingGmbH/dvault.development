namespace DCoding.Data.DVault;

/// <summary>
/// Describes one live Data Vault table primary-key constraint.
/// </summary>
public sealed class DataVaultLiveSchemaPrimaryKey {
  /// <summary>
  /// Initializes a new live primary-key constraint description.
  /// </summary>
  /// <param name="constraintName">The physical primary-key constraint name.</param>
  /// <param name="columnNames">The ordered physical primary-key columns.</param>
  public DataVaultLiveSchemaPrimaryKey(string constraintName, IEnumerable<string> columnNames) {
    ArgumentException.ThrowIfNullOrWhiteSpace(constraintName);
    ArgumentNullException.ThrowIfNull(columnNames);

    ConstraintName = constraintName;
    ColumnNames = columnNames
        .Select(columnName => {
          ArgumentException.ThrowIfNullOrWhiteSpace(columnName);
          return columnName;
        })
        .ToArray();
  }

  /// <summary>
  /// Gets the physical primary-key constraint name.
  /// </summary>
  public string ConstraintName { get; }

  /// <summary>
  /// Gets the ordered physical primary-key columns.
  /// </summary>
  public IReadOnlyList<string> ColumnNames { get; }
}
