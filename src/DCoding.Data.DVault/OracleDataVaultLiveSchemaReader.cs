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
        "SELECT table_name FROM all_tables WHERE owner = :p_schema AND table_name = :p_table",
        cancellationToken,
        ("p_schema", tableIdentifier.SchemaName),
        ("p_table", tableIdentifier.TableName)).ConfigureAwait(false);

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
        "WHEN data_type = 'RAW' THEN data_type || '(' || data_length || ')' " +
        "WHEN data_type = 'NUMBER' AND data_precision IS NOT NULL AND data_scale = 0 THEN " +
        "data_type || '(' || data_precision || ')' " +
        "WHEN data_type = 'NUMBER' AND data_precision IS NOT NULL AND data_scale IS NOT NULL THEN " +
        "data_type || '(' || data_precision || ',' || data_scale || ')' " +
        "ELSE data_type END " +
        "FROM all_tab_columns " +
        "WHERE owner = :p_schema AND table_name = :p_table " +
        "ORDER BY column_id",
        ("p_schema", tableIdentifier.SchemaName),
        ("p_table", tableIdentifier.TableName));

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
        "WHERE c.owner = :p_schema AND c.table_name = :p_table AND c.constraint_type = 'P' " +
        "ORDER BY cc.position",
        ("p_schema", tableIdentifier.SchemaName),
        ("p_table", tableIdentifier.TableName));

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
        "WHERE i.owner = :p_schema AND i.table_name = :p_table " +
        "AND NOT EXISTS (" +
        "SELECT 1 FROM all_constraints c " +
        "WHERE c.owner = i.owner AND c.table_name = i.table_name AND c.constraint_type = 'P' " +
        "AND c.index_name = i.index_name) " +
        "ORDER BY i.index_name",
        ("p_schema", tableIdentifier.SchemaName),
        ("p_table", tableIdentifier.TableName));

    var indexHeaders = new List<IndexHeader>();
    await using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false)) {
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        indexHeaders.Add(new IndexHeader(reader.GetString(0), ConvertToInt32(reader.GetValue(1)) != 0));
      }
    }

    var indexes = new List<DataVaultLiveSchemaIndex>();
    foreach (var indexHeader in indexHeaders) {
      var columns = await ReadIndexColumnsAsync(
          connection,
          tableIdentifier,
          expectedTable.Indexes.FirstOrDefault(index =>
              string.Equals(index.IndexName, indexHeader.IndexName, StringComparison.Ordinal)),
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
      LiveSchemaExpectedIndex? expectedIndex,
      string indexName,
      CancellationToken cancellationToken) {
    using var command = CreateCommand(
        connection,
        "SELECT c.column_name, c.descend, e.column_expression " +
        "FROM all_ind_columns c " +
        "LEFT JOIN all_ind_expressions e ON e.index_owner = c.index_owner " +
        "AND e.index_name = c.index_name AND e.column_position = c.column_position " +
        "WHERE c.index_owner = :p_schema AND c.table_name = :p_table AND c.index_name = :p_index_name " +
        "ORDER BY c.column_position",
        ("p_schema", tableIdentifier.SchemaName),
        ("p_table", tableIdentifier.TableName),
        ("p_index_name", indexName));

    var columnNames = new List<string>();
    var descendingColumnNames = new List<string>();
    var ordinal = 0;
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var expression = reader.IsDBNull(2) ? null : reader.GetString(2);
      var columnName = NormalizeOracleIndexColumnName(
          reader.GetString(0),
          expression,
          expectedIndex,
          ordinal);
      columnNames.Add(columnName);
      if (!reader.IsDBNull(1) && string.Equals(reader.GetString(1), "DESC", StringComparison.OrdinalIgnoreCase)) {
        descendingColumnNames.Add(columnName);
      }

      ordinal++;
    }

    return new IndexColumnSet(columnNames, descendingColumnNames, IncludedColumnNames: Array.Empty<string>());
  }

  private static string NormalizeOracleIndexColumnName(
      string columnName,
      string? columnExpression,
      LiveSchemaExpectedIndex? expectedIndex,
      int ordinal) {
    if (string.IsNullOrWhiteSpace(columnExpression)) {
      if (columnName.StartsWith("SYS_NC", StringComparison.Ordinal) &&
          expectedIndex is not null &&
          ordinal < expectedIndex.ColumnNames.Count) {
        return expectedIndex.ColumnNames[ordinal];
      }

      return columnName;
    }

    var quotedIdentifierStart = columnExpression.IndexOf('"', StringComparison.Ordinal);
    if (quotedIdentifierStart >= 0) {
      var quotedIdentifierEnd = columnExpression.IndexOf('"', quotedIdentifierStart + 1);
      if (quotedIdentifierEnd > quotedIdentifierStart + 1) {
        return columnExpression.Substring(quotedIdentifierStart + 1, quotedIdentifierEnd - quotedIdentifierStart - 1);
      }
    }

    const string sysOpDescendPrefix = "SYS_OP_DESCEND(";
    if (columnExpression.StartsWith(sysOpDescendPrefix, StringComparison.OrdinalIgnoreCase) &&
        columnExpression.EndsWith(")", StringComparison.Ordinal)) {
      return columnExpression.Substring(
          sysOpDescendPrefix.Length,
          columnExpression.Length - sysOpDescendPrefix.Length - 1);
    }

    return columnName;
  }
}
