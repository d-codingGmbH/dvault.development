using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class SqlServerDataVaultLiveSchemaReader : CatalogDataVaultLiveSchemaReader {
  protected override IReadOnlyCollection<string> ProviderNames { get; } = [DataVaultLiveSchemaReader.SqlServerProviderName];

  protected override async Task<LiveSchemaTableIdentifier> ResolveTableIdentifierAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    if (!string.IsNullOrWhiteSpace(tableIdentifier.SchemaName)) {
      return tableIdentifier;
    }

    var schemaName = await ReadScalarStringAsync(
        connection,
        "SELECT SCHEMA_NAME();",
        cancellationToken).ConfigureAwait(false);
    return tableIdentifier with { SchemaName = schemaName };
  }

  protected override async Task<bool> TableExistsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    var value = await ReadScalarStringAsync(
        connection,
        "SELECT t.name " +
        "FROM sys.tables t " +
        "INNER JOIN sys.schemas s ON s.schema_id = t.schema_id " +
        "WHERE s.name = @schema AND t.name = @table;",
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
        "SELECT c.name, ROW_NUMBER() OVER (ORDER BY c.column_id) - 1, " +
        "CASE " +
        "WHEN ty.name IN (N'nvarchar', N'nchar') THEN ty.name + N'(' + " +
        "CASE WHEN c.max_length = -1 THEN N'max' ELSE CONVERT(nvarchar(10), c.max_length / 2) END + N')' " +
        "WHEN ty.name IN (N'varchar', N'char', N'varbinary', N'binary') THEN ty.name + N'(' + " +
        "CASE WHEN c.max_length = -1 THEN N'max' ELSE CONVERT(nvarchar(10), c.max_length) END + N')' " +
        "WHEN ty.name IN (N'decimal', N'numeric') THEN ty.name + N'(' + " +
        "CONVERT(nvarchar(10), c.precision) + N',' + CONVERT(nvarchar(10), c.scale) + N')' " +
        "ELSE ty.name END " +
        "FROM sys.columns c " +
        "INNER JOIN sys.tables t ON t.object_id = c.object_id " +
        "INNER JOIN sys.schemas s ON s.schema_id = t.schema_id " +
        "INNER JOIN sys.types ty ON ty.user_type_id = c.user_type_id " +
        "WHERE s.name = @schema AND t.name = @table " +
        "ORDER BY c.column_id;",
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
        "SELECT kc.name, c.name " +
        "FROM sys.key_constraints kc " +
        "INNER JOIN sys.tables t ON t.object_id = kc.parent_object_id " +
        "INNER JOIN sys.schemas s ON s.schema_id = t.schema_id " +
        "INNER JOIN sys.index_columns ic ON ic.object_id = t.object_id AND ic.index_id = kc.unique_index_id " +
        "INNER JOIN sys.columns c ON c.object_id = t.object_id AND c.column_id = ic.column_id " +
        "WHERE s.name = @schema AND t.name = @table AND kc.type = 'PK' " +
        "ORDER BY ic.key_ordinal;",
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
        "SELECT i.name, i.is_unique " +
        "FROM sys.indexes i " +
        "INNER JOIN sys.tables t ON t.object_id = i.object_id " +
        "INNER JOIN sys.schemas s ON s.schema_id = t.schema_id " +
        "WHERE s.name = @schema AND t.name = @table AND i.name IS NOT NULL " +
        "AND i.type > 0 AND i.is_primary_key = 0 AND i.is_unique_constraint = 0 " +
        "ORDER BY i.name;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName));

    var indexHeaders = new List<IndexHeader>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      indexHeaders.Add(new IndexHeader(reader.GetString(0), ConvertToBoolean(reader.GetValue(1))));
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
        "SELECT c.name, ic.is_descending_key " +
        "FROM sys.indexes i " +
        "INNER JOIN sys.tables t ON t.object_id = i.object_id " +
        "INNER JOIN sys.schemas s ON s.schema_id = t.schema_id " +
        "INNER JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id " +
        "INNER JOIN sys.columns c ON c.object_id = t.object_id AND c.column_id = ic.column_id " +
        "WHERE s.name = @schema AND t.name = @table AND i.name = @index " +
        "AND ic.is_included_column = 0 AND ic.key_ordinal > 0 " +
        "ORDER BY ic.key_ordinal;",
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName),
        ("@index", indexName));

    var columnNames = new List<string>();
    var descendingColumnNames = new List<string>();
    await using (var reader = await keyCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false)) {
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        var columnName = reader.GetString(0);
        columnNames.Add(columnName);
        if (ConvertToBoolean(reader.GetValue(1))) {
          descendingColumnNames.Add(columnName);
        }
      }
    }

    var includedColumnNames = await ReadStringListAsync(
        connection,
        "SELECT c.name " +
        "FROM sys.indexes i " +
        "INNER JOIN sys.tables t ON t.object_id = i.object_id " +
        "INNER JOIN sys.schemas s ON s.schema_id = t.schema_id " +
        "INNER JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id " +
        "INNER JOIN sys.columns c ON c.object_id = t.object_id AND c.column_id = ic.column_id " +
        "WHERE s.name = @schema AND t.name = @table AND i.name = @index " +
        "AND ic.is_included_column = 1 " +
        "ORDER BY ic.index_column_id;",
        cancellationToken,
        ("@schema", tableIdentifier.SchemaName),
        ("@table", tableIdentifier.TableName),
        ("@index", indexName)).ConfigureAwait(false);

    return new IndexColumnSet(columnNames, descendingColumnNames, includedColumnNames);
  }
}
