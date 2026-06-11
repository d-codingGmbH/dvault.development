using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class MySqlDataVaultLiveSchemaReader : CatalogDataVaultLiveSchemaReader {
  protected override IReadOnlyCollection<string> ProviderNames { get; } = [
      DataVaultLiveSchemaReader.MySqlProviderName,
      DataVaultLiveSchemaReader.PomeloMySqlProviderName,
  ];

  protected override async Task<LiveSchemaTableIdentifier> ResolveTableIdentifierAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    if (!string.IsNullOrWhiteSpace(tableIdentifier.SchemaName)) {
      return tableIdentifier;
    }

    var schemaName = await ReadScalarStringAsync(
        connection,
        "SELECT DATABASE();",
        cancellationToken).ConfigureAwait(false);
    return tableIdentifier with { SchemaName = schemaName };
  }

  protected override async Task<bool> TableExistsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    var value = await ReadScalarStringAsync(
        connection,
        "SELECT TABLE_NAME " +
        "FROM information_schema.TABLES " +
        "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table AND TABLE_TYPE = 'BASE TABLE';",
        cancellationToken,
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName)).ConfigureAwait(false);

    return string.Equals(value, tableIdentifier.TableName, StringComparison.Ordinal);
  }

  protected override async Task<IReadOnlyList<DataVaultLiveSchemaColumn>> ReadColumnsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    using var command = CreateCommand(
        connection,
        "SELECT COLUMN_NAME, ORDINAL_POSITION - 1, COLUMN_TYPE " +
        "FROM information_schema.COLUMNS " +
        "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table " +
        "ORDER BY ORDINAL_POSITION;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName));

    var columns = new List<DataVaultLiveSchemaColumn>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      columns.Add(new DataVaultLiveSchemaColumn(
          reader.GetString(0),
          ConvertToInt32(reader.GetValue(1)),
          reader.GetString(2)));
    }

    return columns;
  }

  protected override async Task<DataVaultLiveSchemaPrimaryKey> ReadPrimaryKeyAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      string? expectedPrimaryKeyName,
      CancellationToken cancellationToken) {
    using var command = CreateCommand(
        connection,
        "SELECT tc.CONSTRAINT_NAME, kcu.COLUMN_NAME " +
        "FROM information_schema.TABLE_CONSTRAINTS tc " +
        "INNER JOIN information_schema.KEY_COLUMN_USAGE kcu " +
        "ON kcu.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA " +
        "AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME " +
        "AND kcu.TABLE_SCHEMA = tc.TABLE_SCHEMA " +
        "AND kcu.TABLE_NAME = tc.TABLE_NAME " +
        "WHERE tc.TABLE_SCHEMA = @schema AND tc.TABLE_NAME = @table " +
        "AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY' " +
        "ORDER BY kcu.ORDINAL_POSITION;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName));

    string? primaryKeyName = null;
    var columnNames = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      primaryKeyName ??= reader.GetString(0);
      columnNames.Add(reader.GetString(1));
    }

    if (string.Equals(primaryKeyName, "PRIMARY", StringComparison.Ordinal) &&
        !string.IsNullOrWhiteSpace(expectedPrimaryKeyName)) {
      primaryKeyName = expectedPrimaryKeyName;
    }

    return CreatePrimaryKey(primaryKeyName, columnNames);
  }

  protected override async Task<IReadOnlyList<DataVaultLiveSchemaIndex>> ReadIndexesAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    using var command = CreateCommand(
        connection,
        "SELECT INDEX_NAME, MAX(CASE NON_UNIQUE WHEN 0 THEN 1 ELSE 0 END) " +
        "FROM information_schema.STATISTICS " +
        "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table AND INDEX_NAME <> 'PRIMARY' " +
        "GROUP BY INDEX_NAME " +
        "ORDER BY INDEX_NAME;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName));

    var indexHeaders = new List<IndexHeader>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      indexHeaders.Add(new IndexHeader(reader.GetString(0), ConvertToInt32(reader.GetValue(1)) != 0));
    }

    var indexes = new List<DataVaultLiveSchemaIndex>();
    foreach (var indexHeader in indexHeaders) {
      var columns = await ReadIndexColumnsAsync(
          connection,
          tableIdentifier,
          indexHeader.IndexName,
          cancellationToken).ConfigureAwait(false);
      indexes.Add(new DataVaultLiveSchemaIndex(
          indexHeader.IndexName,
          columns.ColumnNames,
          indexHeader.IsUnique,
          columns.DescendingColumnNames));
    }

    return indexes;
  }

  private static async Task<IndexColumnSet> ReadIndexColumnsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      string indexName,
      CancellationToken cancellationToken) {
    using var command = CreateCommand(
        connection,
        "SELECT COLUMN_NAME, COLLATION " +
        "FROM information_schema.STATISTICS " +
        "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table AND INDEX_NAME = @index " +
        "ORDER BY SEQ_IN_INDEX;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName),
        ("@index", indexName));

    var columnNames = new List<string>();
    var descendingColumnNames = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var columnName = reader.GetString(0);
      columnNames.Add(columnName);
      if (!reader.IsDBNull(1) && string.Equals(reader.GetString(1), "D", StringComparison.Ordinal)) {
        descendingColumnNames.Add(columnName);
      }
    }

    return new IndexColumnSet(columnNames, descendingColumnNames, IncludedColumnNames: Array.Empty<string>());
  }
}
