using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Summarizes rows changed by one explicit PIT maintenance invocation.
/// </summary>
public sealed class DataVaultPitMaintenanceResult {
  /// <summary>
  /// Initializes a new PIT maintenance result.
  /// </summary>
  /// <param name="pit">The PIT metadata declaration maintained by the operation.</param>
  /// <param name="tableName">The generated PIT table name.</param>
  /// <param name="parentHashKeyCount">The number of parent hash keys considered by the operation.</param>
  /// <param name="rowsDeleted">The number of PIT rows deleted by the operation.</param>
  /// <param name="rowsWritten">The number of PIT rows inserted by the operation.</param>
  public DataVaultPitMaintenanceResult(
      DataVaultPitMetadata pit,
      string tableName,
      int parentHashKeyCount,
      int rowsDeleted,
      int rowsWritten) {
    ArgumentNullException.ThrowIfNull(pit);
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentOutOfRangeException.ThrowIfNegative(parentHashKeyCount);
    ArgumentOutOfRangeException.ThrowIfNegative(rowsDeleted);
    ArgumentOutOfRangeException.ThrowIfNegative(rowsWritten);

    Pit = pit;
    TableName = tableName;
    ParentHashKeyCount = parentHashKeyCount;
    RowsDeleted = rowsDeleted;
    RowsWritten = rowsWritten;
  }

  /// <summary>
  /// Gets the PIT metadata declaration maintained by the operation.
  /// </summary>
  public DataVaultPitMetadata Pit { get; }

  /// <summary>
  /// Gets the generated PIT table name.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets the number of parent hash keys considered by the operation.
  /// </summary>
  public int ParentHashKeyCount { get; }

  /// <summary>
  /// Gets the number of PIT rows deleted by the operation.
  /// </summary>
  public int RowsDeleted { get; }

  /// <summary>
  /// Gets the number of PIT rows inserted by the operation.
  /// </summary>
  public int RowsWritten { get; }

  /// <summary>
  /// Gets whether the invocation performed no database writes.
  /// </summary>
  public bool IsNoOp => RowsDeleted == 0 && RowsWritten == 0;
}
