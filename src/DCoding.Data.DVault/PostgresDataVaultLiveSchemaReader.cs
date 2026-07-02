using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class PostgresDataVaultLiveSchemaReader : CatalogDataVaultLiveSchemaReader {
  protected override IReadOnlyCollection<string> ProviderNames { get; } = [DataVaultLiveSchemaReader.PostgresProviderName];

  protected override async Task<LiveSchemaTableIdentifier> ResolveTableIdentifierAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    if (!string.IsNullOrWhiteSpace(tableIdentifier.SchemaName)) {
      return tableIdentifier;
    }

    var schemaName = await ReadScalarStringAsync(
        connection,
        "SELECT CURRENT_SCHEMA();",
        cancellationToken).ConfigureAwait(false);
    return tableIdentifier with { SchemaName = schemaName };
  }

  protected override async Task<bool> TableExistsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    var value = await ReadScalarStringAsync(
        connection,
        "SELECT c.relname " +
        "FROM pg_class c " +
        "INNER JOIN pg_namespace n ON n.oid = c.relnamespace " +
        "WHERE n.nspname = @schema AND c.relname = @table AND c.relkind IN ('r', 'p');",
        cancellationToken,
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName)).ConfigureAwait(false);

    return string.Equals(value, tableIdentifier.TableName, StringComparison.Ordinal);
  }

  protected override async Task<IReadOnlyList<DataVaultLiveSchemaColumn>> ReadColumnsAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken) {
    var tableIdentifier = expectedTable.Identifier;
    using var command = CreateCommand(
        connection,
        "SELECT a.attname, a.attnum - 1, " +
        "CASE " +
        "WHEN t.typname = 'varchar' AND a.atttypmod > 4 THEN 'varchar(' || (a.atttypmod - 4)::text || ')' " +
        "WHEN t.typname = 'varchar' THEN 'varchar' " +
        "WHEN t.typname = 'text' THEN 'text' " +
        "WHEN t.typname = 'timestamptz' THEN 'timestamp with time zone' " +
        "WHEN t.typname = 'int4' THEN 'integer' " +
        "ELSE format_type(a.atttypid, a.atttypmod) END " +
        "FROM pg_attribute a " +
        "INNER JOIN pg_class c ON c.oid = a.attrelid " +
        "INNER JOIN pg_namespace n ON n.oid = c.relnamespace " +
        "INNER JOIN pg_type t ON t.oid = a.atttypid " +
        "WHERE n.nspname = @schema AND c.relname = @table AND a.attnum > 0 AND NOT a.attisdropped " +
        "ORDER BY a.attnum;",
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
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken) {
    var tableIdentifier = expectedTable.Identifier;
    using var command = CreateCommand(
        connection,
        "SELECT con.conname, a.attname " +
        "FROM pg_constraint con " +
        "INNER JOIN pg_class c ON c.oid = con.conrelid " +
        "INNER JOIN pg_namespace n ON n.oid = c.relnamespace " +
        "INNER JOIN LATERAL unnest(con.conkey) WITH ORDINALITY AS key_columns(attnum, column_ordinal) ON TRUE " +
        "INNER JOIN pg_attribute a ON a.attrelid = c.oid AND a.attnum = key_columns.attnum " +
        "WHERE n.nspname = @schema AND c.relname = @table AND con.contype = 'p' " +
        "ORDER BY key_columns.column_ordinal;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName));

    string? primaryKeyName = null;
    var columnNames = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      primaryKeyName ??= reader.GetString(0);
      columnNames.Add(reader.GetString(1));
    }

    return CreatePrimaryKey(primaryKeyName, columnNames);
  }

  protected override async Task<IReadOnlyList<DataVaultLiveSchemaIndex>> ReadIndexesAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken) {
    var tableIdentifier = expectedTable.Identifier;
    using var command = CreateCommand(
        connection,
        "SELECT ix.relname, i.indisunique " +
        "FROM pg_index i " +
        "INNER JOIN pg_class ix ON ix.oid = i.indexrelid " +
        "INNER JOIN pg_class c ON c.oid = i.indrelid " +
        "INNER JOIN pg_namespace n ON n.oid = c.relnamespace " +
        "WHERE n.nspname = @schema AND c.relname = @table AND NOT i.indisprimary " +
        "ORDER BY ix.relname;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName));

    var indexHeaders = new List<IndexHeader>();
    await using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false)) {
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        indexHeaders.Add(new IndexHeader(reader.GetString(0), reader.GetBoolean(1)));
      }
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
          columns.DescendingColumnNames,
          columns.IncludedColumnNames));
    }

    return indexes;
  }

  private static async Task<IndexColumnSet> ReadIndexColumnsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      string indexName,
      CancellationToken cancellationToken) {
    using var keyCommand = CreateCommand(
        connection,
        "SELECT a.attname " +
        ", CASE WHEN (i.indoption[key_columns.column_ordinal - 1] & 1) = 1 THEN TRUE ELSE FALSE END " +
        "FROM pg_index i " +
        "INNER JOIN pg_class ix ON ix.oid = i.indexrelid " +
        "INNER JOIN pg_class c ON c.oid = i.indrelid " +
        "INNER JOIN pg_namespace n ON n.oid = c.relnamespace " +
        "INNER JOIN LATERAL unnest(i.indkey) WITH ORDINALITY AS key_columns(attnum, column_ordinal) ON TRUE " +
        "INNER JOIN pg_attribute a ON a.attrelid = c.oid AND a.attnum = key_columns.attnum " +
        "WHERE n.nspname = @schema AND c.relname = @table AND ix.relname = @index " +
        "AND key_columns.column_ordinal <= i.indnkeyatts " +
        "ORDER BY key_columns.column_ordinal;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName),
        ("@index", indexName));

    var columnNames = new List<string>();
    var descendingColumnNames = new List<string>();
    await using (var reader = await keyCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false)) {
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        var columnName = reader.GetString(0);
        columnNames.Add(columnName);
        if (reader.GetBoolean(1)) {
          descendingColumnNames.Add(columnName);
        }
      }
    }

    var includedColumnNames = await ReadStringListAsync(
        connection,
        "SELECT a.attname " +
        "FROM pg_index i " +
        "INNER JOIN pg_class ix ON ix.oid = i.indexrelid " +
        "INNER JOIN pg_class c ON c.oid = i.indrelid " +
        "INNER JOIN pg_namespace n ON n.oid = c.relnamespace " +
        "INNER JOIN LATERAL unnest(i.indkey) WITH ORDINALITY AS key_columns(attnum, column_ordinal) ON TRUE " +
        "INNER JOIN pg_attribute a ON a.attrelid = c.oid AND a.attnum = key_columns.attnum " +
        "WHERE n.nspname = @schema AND c.relname = @table AND ix.relname = @index " +
        "AND key_columns.column_ordinal > i.indnkeyatts " +
        "ORDER BY key_columns.column_ordinal;",
        cancellationToken,
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName),
        ("@index", indexName)).ConfigureAwait(false);

    return new IndexColumnSet(columnNames, descendingColumnNames, includedColumnNames);
  }
}
