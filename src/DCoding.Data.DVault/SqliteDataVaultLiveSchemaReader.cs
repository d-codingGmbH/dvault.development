using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class SqliteDataVaultLiveSchemaReader : IDataVaultLiveSchemaReader {
  private const string ProviderName = "Microsoft.EntityFrameworkCore.Sqlite";
  private const string MissingPrimaryKeyName = "<missing-primary-key>";
  private const string UnnamedPrimaryKeyName = "<unnamed-primary-key>";

  public async Task<DataVaultLiveSchemaReadResult> ReadAsync(
      DbContext dbContext,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);

    if (!string.Equals(dbContext.Database.ProviderName, ProviderName, StringComparison.Ordinal)) {
      return DataVaultLiveSchemaReadResult.UnsupportedProvider(dbContext.Database.ProviderName);
    }

    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State == ConnectionState.Closed;

    try {
      if (shouldCloseConnection) {
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
      }

      var tableNames = await GetTableNamesAsync(dbContext, connection, cancellationToken).ConfigureAwait(false);
      var tables = new List<DataVaultLiveSchemaTable>();
      foreach (var tableName in tableNames) {
        var table = await ReadTableAsync(connection, tableName, cancellationToken).ConfigureAwait(false);
        if (table is not null) {
          tables.Add(table);
        }
      }

      return DataVaultLiveSchemaReadResult.Success(
          dbContext.Database.ProviderName,
          new DataVaultLiveSchemaSnapshot(tables));
    }
    catch (Exception exception) when (IsSchemaUnavailableException(exception)) {
      return DataVaultLiveSchemaReadResult.Unavailable(dbContext.Database.ProviderName, exception.Message);
    }
    finally {
      if (shouldCloseConnection && connection.State != ConnectionState.Closed) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static async Task<IReadOnlyList<string>> GetTableNamesAsync(
      DbContext dbContext,
      DbConnection connection,
      CancellationToken cancellationToken) {
    var expectedTableNames = dbContext.Model
        .GetEntityTypes()
        .Where(IsDataVaultEntity)
        .Select(entityType => entityType.GetTableName() ?? entityType.Name)
        .Where(tableName => !string.IsNullOrWhiteSpace(tableName))
        .ToArray();
    var candidateLiveTableNames = await ReadStringListAsync(
        connection,
        "SELECT name FROM sqlite_master " +
        "WHERE type = 'table' AND name NOT LIKE 'sqlite_%' AND " +
        "(name LIKE 'Hub%' OR name LIKE 'Link%' OR name LIKE 'Sat%' OR name LIKE 'Bridge%' OR name LIKE 'Pit%') " +
        "ORDER BY name;",
        cancellationToken).ConfigureAwait(false);

    return expectedTableNames
        .Concat(candidateLiveTableNames)
        .Distinct(StringComparer.Ordinal)
        .OrderBy(tableName => tableName, StringComparer.Ordinal)
        .ToArray();
  }

  private static async Task<DataVaultLiveSchemaTable?> ReadTableAsync(
      DbConnection connection,
      string tableName,
      CancellationToken cancellationToken) {
    if (!await TableExistsAsync(connection, tableName, cancellationToken).ConfigureAwait(false)) {
      return null;
    }

    var columns = await ReadColumnsAsync(connection, tableName, cancellationToken).ConfigureAwait(false);
    var primaryKeyColumnNames = await ReadPrimaryKeyColumnNamesAsync(connection, tableName, cancellationToken).ConfigureAwait(false);
    var primaryKeyName = primaryKeyColumnNames.Count == 0
        ? MissingPrimaryKeyName
        : await ReadPrimaryKeyNameAsync(connection, tableName, cancellationToken).ConfigureAwait(false);
    var indexes = await ReadIndexesAsync(connection, tableName, cancellationToken).ConfigureAwait(false);

    return new DataVaultLiveSchemaTable(
        tableName,
        columns,
        new DataVaultLiveSchemaPrimaryKey(primaryKeyName, primaryKeyColumnNames),
        indexes);
  }

  private static async Task<bool> TableExistsAsync(
      DbConnection connection,
      string tableName,
      CancellationToken cancellationToken) {
    var value = await ReadScalarStringAsync(
        connection,
        "SELECT name FROM sqlite_master WHERE type = 'table' AND name = " + SqlLiteral(tableName) + ";",
        cancellationToken).ConfigureAwait(false);

    return string.Equals(value, tableName, StringComparison.Ordinal);
  }

  private static async Task<IReadOnlyList<DataVaultLiveSchemaColumn>> ReadColumnsAsync(
      DbConnection connection,
      string tableName,
      CancellationToken cancellationToken) {
    using var command = connection.CreateCommand();
    command.CommandText =
        "SELECT name, cid, type FROM pragma_table_info(" + SqlLiteral(tableName) + ") ORDER BY cid;";

    var columns = new List<DataVaultLiveSchemaColumn>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      columns.Add(new DataVaultLiveSchemaColumn(
          reader.GetString(0),
          Convert.ToInt32(reader.GetValue(1), CultureInfo.InvariantCulture),
          reader.IsDBNull(2) ? string.Empty : reader.GetString(2)));
    }

    return columns;
  }

  private static Task<IReadOnlyList<string>> ReadPrimaryKeyColumnNamesAsync(
      DbConnection connection,
      string tableName,
      CancellationToken cancellationToken) {
    return ReadStringListAsync(
        connection,
        "SELECT name FROM pragma_table_info(" + SqlLiteral(tableName) + ") WHERE pk > 0 ORDER BY pk;",
        cancellationToken);
  }

  private static async Task<string> ReadPrimaryKeyNameAsync(
      DbConnection connection,
      string tableName,
      CancellationToken cancellationToken) {
    var createSql = await ReadScalarStringAsync(
        connection,
        "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = " + SqlLiteral(tableName) + ";",
        cancellationToken).ConfigureAwait(false);

    if (createSql is null) {
      return MissingPrimaryKeyName;
    }

    const string constraintPrefix = "CONSTRAINT \"";
    const string primaryKeySuffix = "\" PRIMARY KEY";

    var constraintNameStart = createSql.IndexOf(constraintPrefix, StringComparison.Ordinal);
    if (constraintNameStart < 0) {
      return UnnamedPrimaryKeyName;
    }

    var constraintNameEnd = createSql.IndexOf(
        primaryKeySuffix,
        constraintNameStart + constraintPrefix.Length,
        StringComparison.Ordinal);

    if (constraintNameEnd < 0) {
      return UnnamedPrimaryKeyName;
    }

    return createSql.Substring(
        constraintNameStart + constraintPrefix.Length,
        constraintNameEnd - constraintNameStart - constraintPrefix.Length);
  }

  private static async Task<IReadOnlyList<DataVaultLiveSchemaIndex>> ReadIndexesAsync(
      DbConnection connection,
      string tableName,
      CancellationToken cancellationToken) {
    using var command = connection.CreateCommand();
    command.CommandText =
        "SELECT name, \"unique\" FROM pragma_index_list(" + SqlLiteral(tableName) + ") " +
        "WHERE origin <> 'pk' ORDER BY name;";

    var indexes = new List<DataVaultLiveSchemaIndex>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    var indexHeaders = new List<IndexHeader>();
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      indexHeaders.Add(new IndexHeader(
          reader.GetString(0),
          Convert.ToInt32(reader.GetValue(1), CultureInfo.InvariantCulture) != 0));
    }

    foreach (var indexHeader in indexHeaders) {
      var columns = await ReadIndexColumnsAsync(connection, indexHeader.IndexName, cancellationToken).ConfigureAwait(false);
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
      string indexName,
      CancellationToken cancellationToken) {
    using var command = connection.CreateCommand();
    command.CommandText =
        "SELECT name, \"desc\" FROM pragma_index_xinfo(" + SqlLiteral(indexName) + ") " +
        "WHERE key = 1 AND name IS NOT NULL ORDER BY seqno;";

    var columnNames = new List<string>();
    var descendingColumnNames = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var columnName = reader.GetString(0);
      columnNames.Add(columnName);
      if (Convert.ToInt32(reader.GetValue(1), CultureInfo.InvariantCulture) != 0) {
        descendingColumnNames.Add(columnName);
      }
    }

    return new IndexColumnSet(columnNames, descendingColumnNames, IncludedColumnNames: Array.Empty<string>());
  }

  private static async Task<IReadOnlyList<string>> ReadStringListAsync(
      DbConnection connection,
      string commandText,
      CancellationToken cancellationToken) {
    using var command = connection.CreateCommand();
    command.CommandText = commandText;

    var values = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      if (!reader.IsDBNull(0)) {
        values.Add(reader.GetString(0));
      }
    }

    return values;
  }

  private static async Task<string?> ReadScalarStringAsync(
      DbConnection connection,
      string commandText,
      CancellationToken cancellationToken) {
    using var command = connection.CreateCommand();
    command.CommandText = commandText;

    var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
    return value?.ToString();
  }

  private static bool IsDataVaultEntity(IReadOnlyEntityType entityType) {
    return entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value is DataVaultTableKind;
  }

  private static bool IsSchemaUnavailableException(Exception exception) {
    return exception is DbException or InvalidOperationException;
  }

  private static string SqlLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private sealed record IndexHeader(string IndexName, bool IsUnique);
}
