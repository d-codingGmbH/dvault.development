using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for SQLite-specific DVault services.
/// </summary>
public static class DVaultSqliteServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus the SQLite optimized save strategy.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultSqlite(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        SqliteDataVaultSaveStrategy.ProviderName,
        DataVaultProviderCapabilityProfiles.Sqlite);
    services.AddDVault();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, SqliteDataVaultProviderBehavior>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, SqliteDataVaultSaveStrategy>());

    return services;
  }
}

internal sealed class SqliteDataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
  internal const string ProviderName = "Microsoft.EntityFrameworkCore.Sqlite";

  private const int SqliteMaxCommandParameterCount = 900;
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return string.Equals(dbContext.Database.ProviderName, ProviderName, StringComparison.Ordinal) &&
        !ContainsMultiActiveSatelliteOperations(requests) &&
        !dbContext.ChangeTracker
            .Entries()
            .Any(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var uniquePlans = CreateUniqueRowSavePlans(context);
    var satellitePlans = CreateSatelliteSavePlans(context.ResolvedRequests);
    var filteredSatellitePlans = await FilterSatellitePlansAsync(
        context.DbContext,
        satellitePlans,
        cancellationToken).ConfigureAwait(false);
    var savedRecords = uniquePlans
        .Select(plan => plan.SavedRecord)
        .Concat(filteredSatellitePlans.Results.Select(result => result.SavedRecord))
        .ToArray();
    var rowsWritten = await ExecuteSqliteInsertRowsAsync(
        context.DbContext,
        uniquePlans.Select(plan => new SqliteInsertRow(plan.Table.TableName, plan.Row)),
        SqliteInsertConflictBehavior.Ignore,
        cancellationToken).ConfigureAwait(false);

    rowsWritten += await ExecuteSqliteInsertRowsAsync(
        context.DbContext,
        filteredSatellitePlans.RowsToWrite.Select(plan => new SqliteInsertRow(plan.Table.TableName, plan.Row)),
        SqliteInsertConflictBehavior.Fail,
        cancellationToken).ConfigureAwait(false);

    return new DataVaultSaveResult(rowsWritten, savedRecords);
  }

  private static IReadOnlyList<UniqueRowSavePlan> CreateUniqueRowSavePlans(DataVaultProviderSaveStrategyContext context) {
    var plans = new List<UniqueRowSavePlan>();

    foreach (var request in context.ResolvedRequests) {
      plans.AddRange(request.Request.HubOperations.Select(operation => CreateHubSavePlan(context, request, operation)));
      plans.AddRange(request.Request.LinkOperations.Select(operation => CreateLinkSavePlan(context, request, operation)));
    }

    return plans.ToArray();
  }

  private static bool ContainsMultiActiveSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    return requests.Any(request => request.SatelliteOperations.Any(operation => operation.Metadata.DrivingKeyNames.Count > 0));
  }

  private static UniqueRowSavePlan CreateHubSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultResolvedSaveRequest request,
      DataVaultHubSaveOperation operation) {
    var hub = operation.Metadata;
    var tableName = NamingPolicy.GetHubTableName(new DataVaultHubNameContext(hub.Name));
    var hashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, hub.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, hub.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, hub.Name, tableName));
    var businessKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        hub.BusinessKeyColumns.Select(column => column.ColumnName),
        [hashKeyColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var businessKeyFields = hub.BusinessKeyColumns
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.BusinessKeyValues, column.ColumnName, nameof(operation.BusinessKeyValues))))
        .ToArray();
    var hashKey = ComputeHash(context, businessKeyFields);
    var row = new Dictionary<string, object> {
      [hashKeyColumnName] = hashKey,
      [loadTimestampColumnName] = request.LoadTimestamp,
      [recordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < businessKeyFields.Length; index++) {
      row.Add(businessKeyColumnNames[index], businessKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(tableName, hashKeyColumnName),
        hashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Hub, hub.Name, tableName, hashKey));
  }

  private static UniqueRowSavePlan CreateLinkSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultResolvedSaveRequest request,
      DataVaultLinkSaveOperation operation) {
    var link = operation.Metadata;
    var participantNames = link.Participants
        .Select(participant => participant.HubReference.Name)
        .ToArray();
    var tableName = NamingPolicy.GetLinkTableName(new DataVaultLinkNameContext(link.Name, participantNames));
    var linkHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, link.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, link.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, link.Name, tableName));
    var participantHashKeyColumnNames = participantNames
        .Select(participantName => NamingPolicy.GetTechnicalColumnName(
            new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, participantName, tableName)))
        .ToArray();
    var participantHashKeyFields = participantNames
        .Select(participantName => new KeyValuePair<string, string>(
            participantName,
            GetRequiredValue(operation.ParticipantHashKeyValues, participantName, nameof(operation.ParticipantHashKeyValues))))
        .ToArray();
    var linkHashKey = ComputeHash(context, participantHashKeyFields);
    var row = new Dictionary<string, object> {
      [linkHashKeyColumnName] = linkHashKey,
      [loadTimestampColumnName] = request.LoadTimestamp,
      [recordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < participantHashKeyFields.Length; index++) {
      row.Add(participantHashKeyColumnNames[index], participantHashKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(tableName, linkHashKeyColumnName),
        linkHashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, tableName, linkHashKey));
  }

  private static IReadOnlyList<SatelliteSavePlan> CreateSatelliteSavePlans(IReadOnlyList<DataVaultResolvedSaveRequest> requests) {
    return requests
        .SelectMany(request => request.Request.SatelliteOperations
            .Select(operation => CreateSatelliteSavePlan(request, operation)))
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  private static SatelliteSavePlan CreateSatelliteSavePlan(
      DataVaultResolvedSaveRequest request,
      DataVaultSatelliteSaveOperation operation) {
    var satellite = operation.Metadata;
    var tableName = NamingPolicy.GetSatelliteTableName(
        new DataVaultSatelliteNameContext(satellite.Parent.Name, satellite.Name));
    var parentHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, satellite.Parent.Name, tableName));
    var hashDiffColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashDiff, satellite.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, satellite.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, satellite.Name, tableName));
    var payloadColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.PayloadColumns.Select(column => column.ColumnName),
        [parentHashKeyColumnName, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var payloadFields = satellite.PayloadColumns
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.PayloadValues, column.ColumnName, nameof(operation.PayloadValues))))
        .ToArray();
    var row = new Dictionary<string, object> {
      [parentHashKeyColumnName] = operation.ParentHashKey,
      [hashDiffColumnName] = operation.HashDiff,
      [loadTimestampColumnName] = request.LoadTimestamp,
      [recordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < payloadFields.Length; index++) {
      row.Add(payloadColumnNames[index], payloadFields[index].Value);
    }

    var table = new SatelliteTableProjection(
        tableName,
        parentHashKeyColumnName,
        hashDiffColumnName,
        loadTimestampColumnName);
    var savedRecord = new DataVaultSavedRecord(
        DataVaultTableKind.Satellite,
        satellite.Name,
        tableName,
        operation.ParentHashKey);

    return new SatelliteSavePlan(
        -1,
        table,
        operation.ParentHashKey,
        operation.HashDiff,
        request.LoadTimestamp,
        row,
        savedRecord);
  }

  private static async Task<FilteredSatelliteSavePlans> FilterSatellitePlansAsync(
      DbContext dbContext,
      IReadOnlyList<SatelliteSavePlan> plans,
      CancellationToken cancellationToken) {
    var results = new SaveOperationResult[plans.Count];
    var rowsToWrite = new List<SatelliteSavePlan>();

    foreach (var group in plans.GroupBy(plan => plan.Table)) {
      var latestHashDiffs = await LoadLatestSatelliteHashDiffsAsync(
          dbContext,
          group.Key,
          group.Select(plan => plan.ParentHashKey),
          cancellationToken).ConfigureAwait(false);

      foreach (var plan in group) {
        var rowWritten = ShouldWriteSatelliteRow(latestHashDiffs, plan);
        if (rowWritten) {
          rowsToWrite.Add(plan);
          TrackLatestSatelliteHashDiff(latestHashDiffs, plan);
        }

        results[plan.Ordinal] = new SaveOperationResult(plan.SavedRecord, rowWritten);
      }
    }

    return new FilteredSatelliteSavePlans(rowsToWrite, results);
  }

  private static async Task<Dictionary<string, LatestSatelliteHashDiff>> LoadLatestSatelliteHashDiffsAsync(
      DbContext dbContext,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var rows = dbContext.Set<Dictionary<string, object>>(table.TableName);
    var latestRows = new List<LatestSatelliteHashDiff>();

    foreach (var parentHashKeyBatch in parentHashKeys.Distinct(StringComparer.Ordinal).Chunk(500)) {
      var batchRows = await rows
          .AsNoTracking()
          .Where(existingRow => parentHashKeyBatch.Contains(EF.Property<string>(existingRow, table.ParentHashKeyColumnName)))
          .Select(existingRow => new LatestSatelliteHashDiff(
              EF.Property<string>(existingRow, table.ParentHashKeyColumnName),
              EF.Property<string>(existingRow, table.HashDiffColumnName),
              EF.Property<DateTimeOffset>(existingRow, table.LoadTimestampColumnName)))
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);

      latestRows.AddRange(batchRows
          .GroupBy(row => row.ParentHashKey, StringComparer.Ordinal)
          .Select(group => group.OrderByDescending(row => row.LoadTimestamp).First()));
    }

    return latestRows.ToDictionary(row => row.ParentHashKey, StringComparer.Ordinal);
  }

  private static bool ShouldWriteSatelliteRow(
      Dictionary<string, LatestSatelliteHashDiff> latestHashDiffs,
      SatelliteSavePlan plan) {
    return !latestHashDiffs.TryGetValue(plan.ParentHashKey, out var latestHashDiff) ||
        !string.Equals(latestHashDiff.HashDiff, plan.HashDiff, StringComparison.Ordinal);
  }

  private static void TrackLatestSatelliteHashDiff(
      Dictionary<string, LatestSatelliteHashDiff> latestHashDiffs,
      SatelliteSavePlan plan) {
    if (!latestHashDiffs.TryGetValue(plan.ParentHashKey, out var latestHashDiff) ||
        plan.LoadTimestamp >= latestHashDiff.LoadTimestamp) {
      latestHashDiffs[plan.ParentHashKey] = new LatestSatelliteHashDiff(
          plan.ParentHashKey,
          plan.HashDiff,
          plan.LoadTimestamp);
    }
  }

  private static async Task<int> ExecuteSqliteInsertRowsAsync(
      DbContext dbContext,
      IEnumerable<SqliteInsertRow> rows,
      SqliteInsertConflictBehavior conflictBehavior,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    DbTransaction? localTransaction = null;
    var transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();
    if (transaction is null) {
      localTransaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
      transaction = localTransaction;
    }

    try {
      var rowsWritten = 0;

      foreach (var group in rowArray.GroupBy(row => new SqliteInsertRowShape(
          row.TableName,
          CreateColumnSignature(row.Values.Keys)))) {
        var columns = group.First().Values.Keys.ToArray();
        var chunkSize = Math.Max(1, SqliteMaxCommandParameterCount / columns.Length);

        foreach (var chunk in group.Chunk(chunkSize)) {
          rowsWritten += await ExecuteSqliteInsertChunkAsync(
              connection,
              transaction,
              group.Key.TableName,
              columns,
              chunk.Select(row => row.Values).ToArray(),
              conflictBehavior,
              cancellationToken).ConfigureAwait(false);
        }
      }

      if (localTransaction is not null) {
        await localTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
      }

      return rowsWritten;
    }
    catch {
      if (localTransaction is not null) {
        await localTransaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
      }

      throw;
    }
    finally {
      if (localTransaction is not null) {
        await localTransaction.DisposeAsync().ConfigureAwait(false);
      }

      if (shouldCloseConnection) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static async Task<int> ExecuteSqliteInsertChunkAsync(
      DbConnection connection,
      DbTransaction transaction,
      string tableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      SqliteInsertConflictBehavior conflictBehavior,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreateSqliteInsertCommandText(tableName, columns, rows.Count, conflictBehavior);

    var parameterIndex = 0;
    foreach (var row in rows) {
      foreach (var column in columns) {
        var parameter = command.CreateParameter();
        parameter.ParameterName = CreateSqliteParameterName(parameterIndex);
        parameter.Value = row[column];
        command.Parameters.Add(parameter);
        parameterIndex++;
      }
    }

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static string CreateSqliteInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      int rowCount,
      SqliteInsertConflictBehavior conflictBehavior) {
    var builder = new StringBuilder();
    builder.Append("INSERT ");
    if (conflictBehavior == SqliteInsertConflictBehavior.Ignore) {
      builder.Append("OR IGNORE ");
    }

    builder.Append("INTO ")
        .Append(QuoteSqliteIdentifier(tableName))
        .Append(" (");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteSqliteIdentifier(columns[columnIndex]));
    }

    builder.Append(") VALUES ");

    var parameterIndex = 0;
    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
      if (rowIndex > 0) {
        builder.Append(", ");
      }

      builder.Append('(');
      for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
        if (columnIndex > 0) {
          builder.Append(", ");
        }

        builder.Append(CreateSqliteParameterName(parameterIndex));
        parameterIndex++;
      }

      builder.Append(')');
    }

    return builder.ToString();
  }

  private static string CreateSqliteParameterName(int index) {
    return "@p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string QuoteSqliteIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string CreateColumnSignature(IEnumerable<string> columns) {
    return string.Join('\u001f', columns);
  }

  private static string ComputeHash(
      DataVaultProviderSaveStrategyContext context,
      IEnumerable<KeyValuePair<string, string>> fields) {
    var normalizedFields = context.StableHashNormalizer.NormalizeFields(
        fields.Select(field => new KeyValuePair<string, object?>(field.Key, field.Value)));

    return context.StableHashService.ComputeHash(normalizedFields).Value;
  }

  private static string GetRequiredValue(
      IReadOnlyDictionary<string, string> values,
      string name,
      string parameterName) {
    if (values.TryGetValue(name, out var value)) {
      return value;
    }

    throw new ArgumentException("The Data Vault save operation is missing required value '" + name + "'.", parameterName);
  }

  private sealed record SaveOperationResult(DataVaultSavedRecord SavedRecord, bool RowWritten);

  private sealed record UniqueTableProjection(string TableName, string HashKeyColumnName);

  private sealed record UniqueRowSavePlan(
      UniqueTableProjection Table,
      string HashKey,
      Dictionary<string, object> Row,
      DataVaultSavedRecord SavedRecord);

  private sealed record SatelliteTableProjection(
      string TableName,
      string ParentHashKeyColumnName,
      string HashDiffColumnName,
      string LoadTimestampColumnName);

  private sealed record SatelliteSavePlan(
      int Ordinal,
      SatelliteTableProjection Table,
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      Dictionary<string, object> Row,
      DataVaultSavedRecord SavedRecord);

  private sealed record FilteredSatelliteSavePlans(
      IReadOnlyList<SatelliteSavePlan> RowsToWrite,
      IReadOnlyList<SaveOperationResult> Results);

  private sealed record LatestSatelliteHashDiff(string ParentHashKey, string HashDiff, DateTimeOffset LoadTimestamp);

  private sealed record SqliteInsertRow(string TableName, Dictionary<string, object> Values);

  private sealed record SqliteInsertRowShape(string TableName, string ColumnSignature);

  private enum SqliteInsertConflictBehavior {
    Fail,
    Ignore,
  }
}
