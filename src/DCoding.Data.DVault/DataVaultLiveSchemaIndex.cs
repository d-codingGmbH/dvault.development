namespace DCoding.Data.DVault;

/// <summary>
/// Describes one live Data Vault table secondary index.
/// </summary>
public sealed class DataVaultLiveSchemaIndex {
  /// <summary>
  /// Initializes a new live secondary index description.
  /// </summary>
  /// <param name="indexName">The physical index name.</param>
  /// <param name="columnNames">The ordered physical index columns.</param>
  /// <param name="isUnique">A value indicating whether the index is unique.</param>
  public DataVaultLiveSchemaIndex(string indexName, IEnumerable<string> columnNames, bool isUnique) {
    ArgumentException.ThrowIfNullOrWhiteSpace(indexName);
    ArgumentNullException.ThrowIfNull(columnNames);

    IndexName = indexName;
    ColumnNames = columnNames
        .Select(columnName => {
          ArgumentException.ThrowIfNullOrWhiteSpace(columnName);
          return columnName;
        })
        .ToArray();
    IsUnique = isUnique;
  }

  /// <summary>
  /// Gets the physical index name.
  /// </summary>
  public string IndexName { get; }

  /// <summary>
  /// Gets the ordered physical index columns.
  /// </summary>
  public IReadOnlyList<string> ColumnNames { get; }

  /// <summary>
  /// Gets a value indicating whether the index is unique.
  /// </summary>
  public bool IsUnique { get; }
}
