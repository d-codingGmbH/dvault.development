using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class OracleDataVaultLiveSchemaReader : CatalogDataVaultLiveSchemaReader {
  protected override IReadOnlyCollection<string> ProviderNames { get; } = [DataVaultLiveSchemaReader.OracleProviderName];

  protected override async Task<LiveSchemaTableIdentifier> ResolveTableIdentifierAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    if (!string.IsNullOrWhiteSpace(tableIdentifier.SchemaName)) {
      return tableIdentifier;
    }

    var schemaName = await ReadScalarStringAsync(
        connection,
        "SELECT USER FROM DUAL",
        cancellationToken).ConfigureAwait(false);
    return tableIdentifier with { SchemaName = schemaName };
  }

  protected override async Task<bool> TableExistsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    var value = await ReadScalarStringAsync(
        connection,
        "SELECT table_name FROM all_tables WHERE owner = :schema AND table_name = :table",
        cancellationToken,
        ("schema", tableIdentifier.SchemaName),
        ("table", tableIdentifier.TableName)).ConfigureAwait(false);

    return string.Equals(value, tableIdentifier.TableName, StringComparison.Ordinal);
  }

  protected override async Task<IReadOnlyList<DataVaultLiveSchemaColumn>> ReadColumnsAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken) {
    var tableIdentifier = expectedTable.Identifier;
    using var command = CreateCommand(
        connection,
        "SELECT column_name, column_id - 1, " +
        "CASE " +
        "WHEN data_type IN ('VARCHAR2', 'CHAR', 'NVARCHAR2', 'NCHAR') THEN data_type || '(' || char_length || " +
        "CASE char_used WHEN 'C' THEN ' CHAR' WHEN 'B' THEN ' BYTE' ELSE '' END || ')' " +
        "WHEN data_type = 'NUMBER' AND data_precision IS NOT NULL AND data_scale = 0 THEN " +
        "data_type || '(' || data_precision || ')' " +
        "WHEN data_type = 'NUMBER' AND data_precision IS NOT NULL AND data_scale IS NOT NULL THEN " +
        "data_type || '(' || data_precision || ',' || data_scale || ')' " +
        "ELSE data_type END " +
        "FROM all_tab_columns " +
        "WHERE owner = :schema AND table_name = :table " +
        "ORDER BY column_id",
        ("schema", tableIdentifier.SchemaName),
        ("table", tableIdentifier.TableName));

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
        "SELECT c.constraint_name, cc.column_name " +
        "FROM all_constraints c " +
        "INNER JOIN all_cons_columns cc ON cc.owner = c.owner AND cc.constraint_name = c.constraint_name " +
        "WHERE c.owner = :schema AND c.table_name = :table AND c.constraint_type = 'P' " +
        "ORDER BY cc.position",
        ("schema", tableIdentifier.SchemaName),
        ("table", tableIdentifier.TableName));

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
        "SELECT i.index_name, CASE i.uniqueness WHEN 'UNIQUE' THEN 1 ELSE 0 END " +
        "FROM all_indexes i " +
        "WHERE i.owner = :schema AND i.table_name = :table " +
        "AND NOT EXISTS (" +
        "SELECT 1 FROM all_constraints c " +
        "WHERE c.owner = i.owner AND c.table_name = i.table_name AND c.constraint_type = 'P' " +
        "AND c.index_name = i.index_name) " +
        "ORDER BY i.index_name",
        ("schema", tableIdentifier.SchemaName),
        ("table", tableIdentifier.TableName));

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
        "SELECT column_name, descend " +
        "FROM all_ind_columns " +
        "WHERE index_owner = :schema AND table_name = :table AND index_name = :index " +
        "ORDER BY column_position",
        ("schema", tableIdentifier.SchemaName),
        ("table", tableIdentifier.TableName),
        ("index", indexName));

    var columnNames = new List<string>();
    var descendingColumnNames = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var columnName = reader.GetString(0);
      columnNames.Add(columnName);
      if (!reader.IsDBNull(1) && string.Equals(reader.GetString(1), "DESC", StringComparison.OrdinalIgnoreCase)) {
        descendingColumnNames.Add(columnName);
      }
    }

    return new IndexColumnSet(columnNames, descendingColumnNames, IncludedColumnNames: Array.Empty<string>());
  }
}
