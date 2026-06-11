namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one produced Data Vault table.
/// </summary>
public sealed class DataVaultTable {
  /// <summary>
  /// Initializes a new produced table.
  /// </summary>
  public DataVaultTable(
      string name,
      DataVaultTableKind kind,
      IEnumerable<DataVaultColumn> columns,
      IEnumerable<DataVaultIndex> indexes,
      IEnumerable<DataVaultConstraint> constraints)
      : this(name, kind, columns, indexes, constraints, Array.Empty<DataVaultPointInTimeField>()) {
  }

  /// <summary>
  /// Initializes a new produced table with point-in-time field descriptors.
  /// </summary>
  public DataVaultTable(
      string name,
      DataVaultTableKind kind,
      IEnumerable<DataVaultColumn> columns,
      IEnumerable<DataVaultIndex> indexes,
      IEnumerable<DataVaultConstraint> constraints,
      IEnumerable<DataVaultPointInTimeField> pointInTimeFields) {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentNullException.ThrowIfNull(indexes);
    ArgumentNullException.ThrowIfNull(constraints);
    ArgumentNullException.ThrowIfNull(pointInTimeFields);

    Name = name;
    Kind = kind;
    Columns = columns.ToArray();
    Indexes = indexes.ToArray();
    Constraints = constraints.ToArray();
    PointInTimeFields = pointInTimeFields.ToArray();
  }

  /// <summary>
  /// Gets the produced table name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the produced table kind.
  /// </summary>
  public DataVaultTableKind Kind { get; }

  /// <summary>
  /// Gets the produced table columns.
  /// </summary>
  public IReadOnlyList<DataVaultColumn> Columns { get; }

  /// <summary>
  /// Gets the produced table indexes.
  /// </summary>
  public IReadOnlyList<DataVaultIndex> Indexes { get; }

  /// <summary>
  /// Gets the produced table constraints.
  /// </summary>
  public IReadOnlyList<DataVaultConstraint> Constraints { get; }

  /// <summary>
  /// Gets provider-neutral point-in-time field descriptors for PIT tables, or an empty list for other table kinds.
  /// </summary>
  public IReadOnlyList<DataVaultPointInTimeField> PointInTimeFields { get; }
}
