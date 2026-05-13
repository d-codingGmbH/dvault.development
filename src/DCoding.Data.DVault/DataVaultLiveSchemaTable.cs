namespace DCoding.Data.DVault;

/// <summary>
/// Describes one live Data Vault-owned table and its bounded drift-comparison surface.
/// </summary>
public sealed class DataVaultLiveSchemaTable {
  /// <summary>
  /// Initializes a new live schema table.
  /// </summary>
  /// <param name="tableName">The physical table name.</param>
  /// <param name="columns">The ordered live columns.</param>
  /// <param name="primaryKey">The named primary-key constraint.</param>
  /// <param name="indexes">The live secondary indexes.</param>
  public DataVaultLiveSchemaTable(
      string tableName,
      IEnumerable<DataVaultLiveSchemaColumn> columns,
      DataVaultLiveSchemaPrimaryKey primaryKey,
      IEnumerable<DataVaultLiveSchemaIndex> indexes) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentNullException.ThrowIfNull(primaryKey);
    ArgumentNullException.ThrowIfNull(indexes);

    TableName = tableName;
    Columns = columns
        .Select(column => column ?? throw new ArgumentException("Live schema columns must not contain null values.", nameof(columns)))
        .OrderBy(column => column.Ordinal)
        .ThenBy(column => column.ColumnName, StringComparer.Ordinal)
        .ToArray();
    PrimaryKey = primaryKey;
    Indexes = indexes
        .Select(index => index ?? throw new ArgumentException("Live schema indexes must not contain null values.", nameof(indexes)))
        .OrderBy(index => index.IndexName, StringComparer.Ordinal)
        .ToArray();
  }

  /// <summary>
  /// Gets the physical table name.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets the live columns in deterministic ordinal order.
  /// </summary>
  public IReadOnlyList<DataVaultLiveSchemaColumn> Columns { get; }

  /// <summary>
  /// Gets the named primary-key constraint.
  /// </summary>
  public DataVaultLiveSchemaPrimaryKey PrimaryKey { get; }

  /// <summary>
  /// Gets the live secondary indexes in deterministic physical-name order.
  /// </summary>
  public IReadOnlyList<DataVaultLiveSchemaIndex> Indexes { get; }
}
