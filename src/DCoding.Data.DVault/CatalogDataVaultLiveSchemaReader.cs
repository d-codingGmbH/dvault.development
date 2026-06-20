using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

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

        var resolvedExpectedTable = expectedTable with { Identifier = tableIdentifier };
        tables.Add(new DataVaultLiveSchemaTable(
            tableIdentifier.TableName,
            await ReadColumnsAsync(connection, resolvedExpectedTable, cancellationToken).ConfigureAwait(false),
            await ReadPrimaryKeyAsync(
                connection,
                resolvedExpectedTable,
                cancellationToken).ConfigureAwait(false),
            await ReadIndexesAsync(connection, resolvedExpectedTable, cancellationToken).ConfigureAwait(false)));
      }

      return DataVaultLiveSchemaReadResult.Success(providerName, new DataVaultLiveSchemaSnapshot(tables));
    }
    catch (Exception exception) when (IsSchemaUnavailableException(exception)) {
      return CreateUnavailableResult(providerName, exception);
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
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken);

  protected abstract Task<DataVaultLiveSchemaPrimaryKey> ReadPrimaryKeyAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken);

  protected abstract Task<IReadOnlyList<DataVaultLiveSchemaIndex>> ReadIndexesAsync(
      DbConnection connection,
      LiveSchemaExpectedTable expectedTable,
      CancellationToken cancellationToken);

  protected virtual DataVaultLiveSchemaReadResult CreateUnavailableResult(string? providerName, Exception exception) {
    return DataVaultLiveSchemaReadResult.Unavailable(providerName, exception.Message);
  }

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

      var schemaName = entityType.GetSchema() ?? dbContext.Model.GetDefaultSchema();
      var storeObject = StoreObjectIdentifier.Table(tableName, schemaName);
      var identifier = new LiveSchemaTableIdentifier(
          tableName,
          schemaName);
      expectedTablesByIdentifier.TryAdd(
          identifier,
          new LiveSchemaExpectedTable(
              identifier,
              entityType.FindPrimaryKey()?.GetName(),
              entityType.GetProperties()
                  .Select(property => property.GetColumnName(storeObject) ?? property.Name)
                  .ToArray(),
              entityType.GetIndexes()
                  .Select(index => index.GetDatabaseName() ?? string.Join("_", index.Properties.Select(property =>
                      property.GetColumnName(storeObject) ?? property.Name)))
                  .ToArray()));
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
