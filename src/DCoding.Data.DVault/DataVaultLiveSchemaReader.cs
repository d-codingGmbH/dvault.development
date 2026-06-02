using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides the built-in live database schema reader dispatch for the bounded Data Vault schema surface.
/// </summary>
public static class DataVaultLiveSchemaReader {
  internal const string SqliteProviderName = "Microsoft.EntityFrameworkCore.Sqlite";
  internal const string PostgresProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";
  internal const string SqlServerProviderName = "Microsoft.EntityFrameworkCore.SqlServer";
  internal const string OracleProviderName = "Oracle.EntityFrameworkCore";
  internal const string MySqlProviderName = "MySql.EntityFrameworkCore";
  internal const string PomeloMySqlProviderName = "Pomelo.EntityFrameworkCore.MySql";
  private static readonly IDataVaultLiveSchemaReader SqliteReader = new SqliteDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader PostgresReader = new PostgresDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader SqlServerReader = new SqlServerDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader OracleReader = new OracleDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader MySqlReader = new MySqlDataVaultLiveSchemaReader();
  private static readonly IReadOnlyDictionary<string, IDataVaultLiveSchemaReader> BuiltInReadersByProviderName =
      new Dictionary<string, IDataVaultLiveSchemaReader>(StringComparer.Ordinal) {
        [SqliteProviderName] = SqliteReader,
        [PostgresProviderName] = PostgresReader,
        [SqlServerProviderName] = SqlServerReader,
        [OracleProviderName] = OracleReader,
        [MySqlProviderName] = MySqlReader,
        [PomeloMySqlProviderName] = MySqlReader,
      };

  /// <summary>
  /// Reads a live database schema snapshot for the supplied context using the built-in reader for the current provider.
  /// </summary>
  /// <param name="dbContext">The context whose provider, model, and connection identify the live schema to read.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading schema metadata.</param>
  /// <returns>
  /// A classified live-schema read result. Recognized built-in providers return a snapshot when their database catalog is
  /// reachable; providers without a built-in reader return an unsupported-provider result instead of silently passing or
  /// throwing an unclassified failure.
  /// </returns>
  public static Task<DataVaultLiveSchemaReadResult> ReadAsync(
      DbContext dbContext,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var providerName = TryGetProviderName(dbContext);
    return providerName is not null && BuiltInReadersByProviderName.TryGetValue(providerName, out var reader)
        ? reader.ReadAsync(dbContext, cancellationToken)
        : Task.FromResult(DataVaultLiveSchemaReadResult.UnsupportedProvider(providerName));
  }

  private static string? TryGetProviderName(DbContext dbContext) {
    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
  }
}

internal sealed class UnsupportedDataVaultLiveSchemaReader : IDataVaultLiveSchemaReader {
  public Task<DataVaultLiveSchemaReadResult> ReadAsync(
      DbContext dbContext,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);

    try {
      return Task.FromResult(DataVaultLiveSchemaReadResult.UnsupportedProvider(dbContext.Database.ProviderName));
    }
    catch (InvalidOperationException) {
      return Task.FromResult(DataVaultLiveSchemaReadResult.UnsupportedProvider(providerName: null));
    }
  }
}

internal abstract class CatalogDataVaultLiveSchemaReader : IDataVaultLiveSchemaReader {
  protected const string MissingPrimaryKeyName = "<missing-primary-key>";
  protected const string UnnamedPrimaryKeyName = "<unnamed-primary-key>";

  protected abstract IReadOnlyCollection<string> ProviderNames { get; }

  public async Task<DataVaultLiveSchemaReadResult> ReadAsync(
      DbContext dbContext,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var providerName = TryGetProviderName(dbContext);
    if (providerName is null || !ProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      return DataVaultLiveSchemaReadResult.UnsupportedProvider(providerName);
    }

    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State == ConnectionState.Closed;

    try {
      if (shouldCloseConnection) {
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
      }

      var expectedTables = GetExpectedTables(dbContext);
      var tables = new List<DataVaultLiveSchemaTable>();
      foreach (var expectedTable in expectedTables) {
        var tableIdentifier = await ResolveTableIdentifierAsync(
            connection,
            expectedTable.Identifier,
            cancellationToken).ConfigureAwait(false);
        if (!await TableExistsAsync(connection, tableIdentifier, cancellationToken).ConfigureAwait(false)) {
          continue;
        }

        tables.Add(new DataVaultLiveSchemaTable(
            tableIdentifier.TableName,
            await ReadColumnsAsync(connection, tableIdentifier, cancellationToken).ConfigureAwait(false),
            await ReadPrimaryKeyAsync(
                connection,
                tableIdentifier,
                expectedTable.PrimaryKeyName,
                cancellationToken).ConfigureAwait(false),
            await ReadIndexesAsync(connection, tableIdentifier, cancellationToken).ConfigureAwait(false)));
      }

      return DataVaultLiveSchemaReadResult.Success(providerName, new DataVaultLiveSchemaSnapshot(tables));
    }
    catch (Exception exception) when (IsSchemaUnavailableException(exception)) {
      return DataVaultLiveSchemaReadResult.Unavailable(providerName, exception.Message);
    }
    finally {
      if (shouldCloseConnection && connection.State != ConnectionState.Closed) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  protected virtual Task<LiveSchemaTableIdentifier> ResolveTableIdentifierAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
    return Task.FromResult(tableIdentifier);
  }

  protected abstract Task<bool> TableExistsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken);

  protected abstract Task<IReadOnlyList<DataVaultLiveSchemaColumn>> ReadColumnsAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken);

  protected abstract Task<DataVaultLiveSchemaPrimaryKey> ReadPrimaryKeyAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      string? expectedPrimaryKeyName,
      CancellationToken cancellationToken);

  protected abstract Task<IReadOnlyList<DataVaultLiveSchemaIndex>> ReadIndexesAsync(
      DbConnection connection,
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken);

  protected static async Task<string?> ReadScalarStringAsync(
      DbConnection connection,
      string commandText,
      CancellationToken cancellationToken,
      params (string Name, object? Value)[] parameters) {
    using var command = CreateCommand(connection, commandText, parameters);

    var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
    return value?.ToString();
  }

  protected static async Task<IReadOnlyList<string>> ReadStringListAsync(
      DbConnection connection,
      string commandText,
      CancellationToken cancellationToken,
      params (string Name, object? Value)[] parameters) {
    using var command = CreateCommand(connection, commandText, parameters);

    var values = new List<string>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      if (!reader.IsDBNull(0)) {
        values.Add(reader.GetString(0));
      }
    }

    return values;
  }

  protected static DbCommand CreateCommand(
      DbConnection connection,
      string commandText,
      params (string Name, object? Value)[] parameters) {
    var command = connection.CreateCommand();
    command.CommandText = commandText;

    foreach (var parameterValue in parameters) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = parameterValue.Name;
      parameter.Value = parameterValue.Value ?? DBNull.Value;
      command.Parameters.Add(parameter);
    }

    return command;
  }

  protected static int ConvertToInt32(object value) {
    return Convert.ToInt32(value, CultureInfo.InvariantCulture);
  }

  protected static bool ConvertToBoolean(object value) {
    if (value is bool boolValue) {
      return boolValue;
    }

    return Convert.ToInt32(value, CultureInfo.InvariantCulture) != 0;
  }

  protected static DataVaultLiveSchemaPrimaryKey CreatePrimaryKey(
      string? primaryKeyName,
      IReadOnlyList<string> columnNames) {
    return columnNames.Count == 0
        ? new DataVaultLiveSchemaPrimaryKey(MissingPrimaryKeyName, columnNames)
        : new DataVaultLiveSchemaPrimaryKey(primaryKeyName ?? UnnamedPrimaryKeyName, columnNames);
  }

  private static IReadOnlyList<LiveSchemaExpectedTable> GetExpectedTables(DbContext dbContext) {
    var expectedTablesByIdentifier = new Dictionary<LiveSchemaTableIdentifier, LiveSchemaExpectedTable>();
    foreach (var entityType in dbContext.Model.GetEntityTypes().Where(IsDataVaultEntity)) {
      var tableName = entityType.GetTableName() ?? entityType.Name;
      if (string.IsNullOrWhiteSpace(tableName)) {
        continue;
      }

      var identifier = new LiveSchemaTableIdentifier(
          tableName,
          entityType.GetSchema() ?? dbContext.Model.GetDefaultSchema());
      expectedTablesByIdentifier.TryAdd(
          identifier,
          new LiveSchemaExpectedTable(identifier, entityType.FindPrimaryKey()?.GetName()));
    }

    return expectedTablesByIdentifier.Values
        .OrderBy(table => table.Identifier.SchemaName ?? string.Empty, StringComparer.Ordinal)
        .ThenBy(table => table.Identifier.TableName, StringComparer.Ordinal)
        .ToArray();
  }

  private static bool IsDataVaultEntity(IReadOnlyEntityType entityType) {
    return entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value is DataVaultTableKind;
  }

  private static bool IsSchemaUnavailableException(Exception exception) {
    return exception is DbException or InvalidOperationException;
  }

  private static string? TryGetProviderName(DbContext dbContext) {
    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
  }
}

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
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
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
      LiveSchemaTableIdentifier tableIdentifier,
      string? expectedPrimaryKeyName,
      CancellationToken cancellationToken) {
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
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
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
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      indexHeaders.Add(new IndexHeader(reader.GetString(0), reader.GetBoolean(1)));
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
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
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
      LiveSchemaTableIdentifier tableIdentifier,
      string? expectedPrimaryKeyName,
      CancellationToken cancellationToken) {
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
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
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
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
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
      LiveSchemaTableIdentifier tableIdentifier,
      string? expectedPrimaryKeyName,
      CancellationToken cancellationToken) {
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
      LiveSchemaTableIdentifier tableIdentifier,
      CancellationToken cancellationToken) {
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

internal sealed record LiveSchemaTableIdentifier(string TableName, string? SchemaName);

internal sealed record LiveSchemaExpectedTable(LiveSchemaTableIdentifier Identifier, string? PrimaryKeyName);

internal sealed record IndexHeader(string IndexName, bool IsUnique);

internal sealed record IndexColumnSet(
    IReadOnlyList<string> ColumnNames,
    IReadOnlyList<string> DescendingColumnNames,
    IReadOnlyList<string> IncludedColumnNames);
