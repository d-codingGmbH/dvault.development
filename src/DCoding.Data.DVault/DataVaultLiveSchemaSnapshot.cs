namespace DCoding.Data.DVault;

/// <summary>
/// Provider-neutral live database schema snapshot for Data Vault-owned tables.
/// </summary>
public sealed class DataVaultLiveSchemaSnapshot {
  /// <summary>
  /// Initializes a new live schema snapshot with deterministic table ordering.
  /// </summary>
  /// <param name="tables">The Data Vault-owned live tables in the snapshot.</param>
  public DataVaultLiveSchemaSnapshot(IEnumerable<DataVaultLiveSchemaTable> tables) {
    ArgumentNullException.ThrowIfNull(tables);

    Tables = tables
        .Select(table => table ?? throw new ArgumentException("Live schema tables must not contain null values.", nameof(tables)))
        .OrderBy(table => table.TableName, StringComparer.Ordinal)
        .ToArray();
  }

  /// <summary>
  /// Gets the Data Vault-owned live tables in deterministic physical-name order.
  /// </summary>
  public IReadOnlyList<DataVaultLiveSchemaTable> Tables { get; }
}
