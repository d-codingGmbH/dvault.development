using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class Db2DataVaultLiveSchemaReader : CatalogDataVaultLiveSchemaReader {
  private const string RedactedUnavailableMessage =
      "DB2 catalog access was unavailable. Verify the caller-owned DB2 connection and catalog privileges.";

  protected override IReadOnlyCollection<string> ProviderNames { get; } = [DataVaultLiveSchemaReader.Db2ProviderName];

  protected override async Task<LiveSchemaTableIdentifier> ResolveTableIdentifierAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    if (!string.IsNullOrWhiteSpace(tableIdentifier.SchemaName)) {
      return tableIdentifier;
    }

    var schemaName = await ReadScalarStringAsync(
        connection,
        "SELECT CURRENT SCHEMA FROM SYSIBM.SYSDUMMY1",
        cancellationToken).ConfigureAwait(false);
    return tableIdentifier with { SchemaName = schemaName?.Trim() };
  }

  protected override async Task<bool> TableExistsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    var value = await ReadScalarStringAsync(
        connection,
        "SELECT TABNAME " +
        "FROM SYSCAT.TABLES " +
        "WHERE UPPER(TABSCHEMA) = @schema AND UPPER(TABNAME) = @table AND TYPE = 'T'",
        cancellationToken,
        ("@schema", NormalizeCatalogIdentifierValue(tableIdentifier.SchemaName)),
        ("@table", NormalizeCatalogIdentifierValue(tableIdentifier.TableName))).ConfigureAwait(false);

    return string.Equals(
        NormalizeCatalogIdentifierValue(value),
        NormalizeCatalogIdentifierValue(tableIdentifier.TableName),
        StringComparison.Ordinal);
  }

  protected override async Task<IReadOnlyList<DataVaultLiveSchemaColumn>> ReadColumnsAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken) {
    var tableIdentifier = expectedTable.Identifier;
    var expectedColumnNames = CreateCatalogNameMap(expectedTable.ColumnNames);
    using var command = CreateCommand(
        connection,
        "SELECT COLNAME, COLNO, " +
        "CASE " +
        "WHEN TYPENAME IN ('VARCHAR', 'CHAR', 'VARBINARY', 'BINARY') THEN " +
        "TYPENAME || '(' || RTRIM(CHAR(LENGTH)) || ')' " +
        "WHEN TYPENAME IN ('DECIMAL', 'NUMERIC') THEN " +
        "TYPENAME || '(' || RTRIM(CHAR(LENGTH)) || ',' || RTRIM(CHAR(SCALE)) || ')' " +
        "ELSE TYPENAME END " +
        "FROM SYSCAT.COLUMNS " +
        "WHERE UPPER(TABSCHEMA) = @schema AND UPPER(TABNAME) = @table " +
        "ORDER BY COLNO",
        ("@schema", NormalizeCatalogIdentifierValue(tableIdentifier.SchemaName)),
        ("@table", NormalizeCatalogIdentifierValue(tableIdentifier.TableName)));

    var columns = new List<DataVaultLiveSchemaColumn>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var columnName = NormalizeExpectedCatalogName(reader.GetString(0), expectedColumnNames);
      columns.Add(new DataVaultLiveSchemaColumn(
          columnName,
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
    var expectedColumnNames = CreateCatalogNameMap(expectedTable.ColumnNames);
    using var command = CreateCommand(
        connection,
        "SELECT tc.CONSTNAME, kc.COLNAME " +
        "FROM SYSCAT.TABCONST tc " +
        "INNER JOIN SYSCAT.KEYCOLUSE kc " +
        "ON kc.TABSCHEMA = tc.TABSCHEMA AND kc.TABNAME = tc.TABNAME AND kc.CONSTNAME = tc.CONSTNAME " +
        "WHERE UPPER(tc.TABSCHEMA) = @schema AND UPPER(tc.TABNAME) = @table AND tc.TYPE = 'P' " +
        "ORDER BY kc.COLSEQ",
        ("@schema", NormalizeCatalogIdentifierValue(tableIdentifier.SchemaName)),
        ("@table", NormalizeCatalogIdentifierValue(tableIdentifier.TableName)));

    string? primaryKeyName = null;
    var columnNames = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      primaryKeyName ??= NormalizeExpectedCatalogName(
          reader.GetString(0),
          CreateCatalogNameMap([expectedTable.PrimaryKeyName]));
      columnNames.Add(NormalizeExpectedCatalogName(reader.GetString(1), expectedColumnNames));
    }

    return CreatePrimaryKey(primaryKeyName, columnNames);
  }

  protected override async Task<IReadOnlyList<DataVaultLiveSchemaIndex>> ReadIndexesAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken) {
    var tableIdentifier = expectedTable.Identifier;
    var expectedIndexNames = CreateCatalogNameMap(expectedTable.IndexNames);
    using var command = CreateCommand(
        connection,
        "SELECT INDSCHEMA, INDNAME, UNIQUERULE " +
        "FROM SYSCAT.INDEXES " +
        "WHERE UPPER(TABSCHEMA) = @schema AND UPPER(TABNAME) = @table AND UNIQUERULE <> 'P' " +
        "ORDER BY INDNAME",
        ("@schema", NormalizeCatalogIdentifierValue(tableIdentifier.SchemaName)),
        ("@table", NormalizeCatalogIdentifierValue(tableIdentifier.TableName)));

    var indexHeaders = new List<Db2IndexHeader>();
    await using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false)) {
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        var indexName = NormalizeExpectedCatalogName(reader.GetString(1), expectedIndexNames);
        indexHeaders.Add(new Db2IndexHeader(
            reader.GetString(0),
            reader.GetString(1),
            indexName,
            !string.Equals(reader.GetString(2), "D", StringComparison.Ordinal)));
      }
    }

    var indexes = new List<DataVaultLiveSchemaIndex>();
    foreach (var indexHeader in indexHeaders) {
      var columns = await ReadIndexColumnsAsync(
          connection,
          expectedTable,
          indexHeader,
          cancellationToken).ConfigureAwait(false);
      indexes.Add(new DataVaultLiveSchemaIndex(
          indexHeader.NormalizedIndexName,
          columns.ColumnNames,
          indexHeader.IsUnique,
          columns.DescendingColumnNames,
          columns.IncludedColumnNames));
    }

    return indexes;
  }

  protected override DataVaultLiveSchemaReadResult CreateUnavailableResult(string? providerName, Exception exception) {
    return DataVaultLiveSchemaReadResult.Unavailable(providerName, RedactedUnavailableMessage);
  }

  private static async Task<IndexColumnSet> ReadIndexColumnsAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      Db2IndexHeader indexHeader,
      CancellationToken cancellationToken) {
    var expectedColumnNames = CreateCatalogNameMap(expectedTable.ColumnNames);
    using var command = CreateCommand(
        connection,
        "SELECT COLNAME, COLORDER " +
        "FROM SYSCAT.INDEXCOLUSE " +
        "WHERE UPPER(INDSCHEMA) = @indexSchema AND UPPER(INDNAME) = @index " +
        "ORDER BY COLSEQ",
        ("@indexSchema", NormalizeCatalogIdentifierValue(indexHeader.IndexSchemaName)),
        ("@index", NormalizeCatalogIdentifierValue(indexHeader.CatalogIndexName)));

    var columnNames = new List<string>();
    var descendingColumnNames = new List<string>();
    var includedColumnNames = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var columnName = NormalizeExpectedCatalogName(reader.GetString(0), expectedColumnNames);
      var columnOrder = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
      if (string.Equals(columnOrder, "I", StringComparison.Ordinal)) {
        includedColumnNames.Add(columnName);
        continue;
      }

      columnNames.Add(columnName);
      if (string.Equals(columnOrder, "D", StringComparison.Ordinal)) {
        descendingColumnNames.Add(columnName);
      }
    }

    return new IndexColumnSet(columnNames, descendingColumnNames, includedColumnNames);
  }

  private static IReadOnlyDictionary<string, string> CreateCatalogNameMap(IEnumerable<string?> expectedNames) {
    var names = new Dictionary<string, string>(StringComparer.Ordinal);
    foreach (var expectedName in expectedNames) {
      if (string.IsNullOrWhiteSpace(expectedName)) {
        continue;
      }

      names.TryAdd(NormalizeCatalogIdentifierValue(expectedName), expectedName);
    }

    return names;
  }

  private static string NormalizeExpectedCatalogName(
      string catalogName,
      IReadOnlyDictionary<string, string> expectedNamesByCatalogName) {
    var trimmedCatalogName = catalogName.Trim();
    return expectedNamesByCatalogName.TryGetValue(NormalizeCatalogIdentifierValue(trimmedCatalogName), out var expectedName)
        ? expectedName
        : trimmedCatalogName;
  }

  private static string NormalizeCatalogIdentifierValue(string? value) {
    return (value ?? string.Empty).Trim().ToUpperInvariant();
  }

  private sealed record Db2IndexHeader(
      string IndexSchemaName,
      string CatalogIndexName,
      string NormalizedIndexName,
      bool IsUnique);
}
