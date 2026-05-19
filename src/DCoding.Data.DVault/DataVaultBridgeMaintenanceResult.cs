namespace DCoding.Data.DVault;

/// <summary>
/// Summarizes rows affected by one explicit bridge maintenance operation.
/// </summary>
public sealed class DataVaultBridgeMaintenanceResult {
  /// <summary>
  /// Initializes a new bridge maintenance result.
  /// </summary>
  /// <param name="metadataName">The logical bridge metadata name that was maintained.</param>
  /// <param name="tableName">The generated bridge table/entity name that received maintenance changes.</param>
  /// <param name="rowsInserted">The count of bridge rows inserted by the operation.</param>
  /// <param name="rowsUpdated">The count of existing bridge rows updated by the operation.</param>
  /// <param name="rowsDeleted">The count of existing bridge rows deleted by the operation.</param>
  /// <param name="rowsUnchanged">The count of desired bridge rows already present with the expected values.</param>
  public DataVaultBridgeMaintenanceResult(
      string metadataName,
      string tableName,
      int rowsInserted,
      int rowsUpdated,
      int rowsDeleted,
      int rowsUnchanged) {
    ArgumentException.ThrowIfNullOrWhiteSpace(metadataName);
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentOutOfRangeException.ThrowIfNegative(rowsInserted);
    ArgumentOutOfRangeException.ThrowIfNegative(rowsUpdated);
    ArgumentOutOfRangeException.ThrowIfNegative(rowsDeleted);
    ArgumentOutOfRangeException.ThrowIfNegative(rowsUnchanged);

    MetadataName = metadataName;
    TableName = tableName;
    RowsInserted = rowsInserted;
    RowsUpdated = rowsUpdated;
    RowsDeleted = rowsDeleted;
    RowsUnchanged = rowsUnchanged;
  }

  /// <summary>
  /// Gets the logical bridge metadata name that was maintained.
  /// </summary>
  public string MetadataName { get; }

  /// <summary>
  /// Gets the generated bridge table/entity name that received maintenance changes.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets the count of bridge rows inserted by the operation.
  /// </summary>
  public int RowsInserted { get; }

  /// <summary>
  /// Gets the count of existing bridge rows updated by the operation.
  /// </summary>
  public int RowsUpdated { get; }

  /// <summary>
  /// Gets the count of existing bridge rows deleted by the operation.
  /// </summary>
  public int RowsDeleted { get; }

  /// <summary>
  /// Gets the count of desired bridge rows already present with the expected values.
  /// </summary>
  public int RowsUnchanged { get; }
}
