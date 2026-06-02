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
  /// <param name="descendingColumnNames">The ordered physical key columns that are stored descending, when the provider exposes direction.</param>
  /// <param name="includedColumnNames">The ordered physical included columns, when the provider exposes native included columns.</param>
  public DataVaultLiveSchemaIndex(
      string indexName,
      IEnumerable<string> columnNames,
      bool isUnique,
      IEnumerable<string>? descendingColumnNames = null,
      IEnumerable<string>? includedColumnNames = null) {
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
    DescendingColumnNames = (descendingColumnNames ?? Array.Empty<string>())
        .Select(columnName => {
          ArgumentException.ThrowIfNullOrWhiteSpace(columnName);
          return columnName;
        })
        .ToArray();
    IncludedColumnNames = (includedColumnNames ?? Array.Empty<string>())
        .Select(columnName => {
          ArgumentException.ThrowIfNullOrWhiteSpace(columnName);
          return columnName;
        })
        .ToArray();
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

  /// <summary>
  /// Gets the ordered physical key columns that are stored descending, when the provider exposes direction.
  /// </summary>
  public IReadOnlyList<string> DescendingColumnNames { get; }

  /// <summary>
  /// Gets the ordered physical included columns, when the provider exposes native included columns.
  /// </summary>
  public IReadOnlyList<string> IncludedColumnNames { get; }
}
