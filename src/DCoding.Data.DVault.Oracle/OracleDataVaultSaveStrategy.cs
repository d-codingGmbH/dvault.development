using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class OracleDataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
  private const int MinimumOptimizedBatchOperationCount = 50;
  private const int MaximumOptimizedSatelliteOperationCount = 200;
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  internal const string OracleProviderName = "Oracle.EntityFrameworkCore";

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return string.Equals(dbContext.Database.ProviderName, OracleProviderName, StringComparison.Ordinal) &&
        IsCleanContext(dbContext) &&
        IsOptimizedBatchShape(requests);
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
    var rowsWritten = await ExecuteOracleInsertRowsAsync(
        context.DbContext,
        uniquePlans.Select(plan => new OracleInsertRow(
            plan.Table.TableName,
            plan.Table.HashKeyColumnName,
            plan.HashKey,
            plan.Row,
            OracleInsertConflictBehavior.Ignore)),
        cancellationToken).ConfigureAwait(false);

    rowsWritten += await ExecuteOracleInsertRowsAsync(
        context.DbContext,
        filteredSatellitePlans.RowsToWrite.Select(plan => new OracleInsertRow(
            plan.Table.TableName,
            HashKeyColumnName: null,
            HashKey: null,
            plan.Row,
            OracleInsertConflictBehavior.Fail)),
        cancellationToken).ConfigureAwait(false);

    return new DataVaultSaveResult(rowsWritten, savedRecords);
  }

  private static bool IsCleanContext(DbContext dbContext) {
    return !dbContext.ChangeTracker
        .Entries()
        .Any(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
  }

  internal static bool IsOptimizedBatchShape(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var operationCount = 0;
    var satelliteOperationCount = 0;
    foreach (var request in requests) {
      operationCount += request.HubOperations.Count + request.LinkOperations.Count + request.SatelliteOperations.Count;
      satelliteOperationCount += request.SatelliteOperations.Count;
    }

    return operationCount >= MinimumOptimizedBatchOperationCount &&
        satelliteOperationCount <= MaximumOptimizedSatelliteOperationCount;
  }

  private static IReadOnlyList<UniqueRowSavePlan> CreateUniqueRowSavePlans(DataVaultProviderSaveStrategyContext context) {
    var plans = new List<UniqueRowSavePlan>();

    foreach (var request in context.ResolvedRequests) {
      plans.AddRange(request.Request.HubOperations.Select(operation => CreateHubSavePlan(context, request, operation)));
      plans.AddRange(request.Request.LinkOperations.Select(operation => CreateLinkSavePlan(context, request, operation)));
    }

    return plans.ToArray();
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
      [loadTimestampColumnName] = FormatLoadTimestamp(request.LoadTimestamp),
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
      [loadTimestampColumnName] = FormatLoadTimestamp(request.LoadTimestamp),
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
      [loadTimestampColumnName] = FormatLoadTimestamp(request.LoadTimestamp),
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
    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    var latestRows = new List<LatestSatelliteHashDiff>();

    try {
      foreach (var parentHashKeyBatch in parentHashKeys.Distinct(StringComparer.Ordinal).Chunk(500)) {
        await using var command = connection.CreateCommand();
        command.Transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();

        var parameterNames = AddCommandParameters(command, parentHashKeyBatch);
        command.CommandText = CreateLatestSatelliteHashDiffsCommandText(table, parameterNames);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
          latestRows.Add(new LatestSatelliteHashDiff(
              GetRequiredString(reader, ordinal: 0),
              GetRequiredString(reader, ordinal: 1),
              ParseLoadTimestamp(GetRequiredString(reader, ordinal: 2))));
        }
      }
    }
    finally {
      if (shouldCloseConnection) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }

    return latestRows
        .GroupBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .Select(group => group.OrderByDescending(row => row.LoadTimestamp).First())
        .ToDictionary(row => row.ParentHashKey, StringComparer.Ordinal);
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

  private static async Task<int> ExecuteOracleInsertRowsAsync(
      DbContext dbContext,
      IEnumerable<OracleInsertRow> rows,
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
      foreach (var row in rowArray) {
        rowsWritten += await ExecuteOracleInsertRowAsync(
            connection,
            transaction,
            row,
            cancellationToken).ConfigureAwait(false);
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

  private static async Task<int> ExecuteOracleInsertRowAsync(
      DbConnection connection,
      DbTransaction transaction,
      OracleInsertRow row,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;

    var columns = row.Values.Keys.ToArray();
    command.CommandText = CreateOracleInsertCommandText(row, columns);

    for (var index = 0; index < columns.Length; index++) {
      AddParameter(command, row.Values[columns[index]]);
    }

    if (row.ConflictBehavior == OracleInsertConflictBehavior.Ignore) {
      AddParameter(command, row.HashKey!);
    }

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static string CreateOracleInsertCommandText(
      OracleInsertRow row,
      IReadOnlyList<string> columns) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuoteOracleIdentifier(row.TableName))
        .Append(" (");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteOracleIdentifier(columns[columnIndex]));
    }

    builder.Append(") SELECT ");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateOracleParameterPlaceholder(columnIndex));
    }

    if (row.ConflictBehavior == OracleInsertConflictBehavior.Ignore) {
      builder.Append(" FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM ")
          .Append(QuoteOracleIdentifier(row.TableName))
          .Append(" WHERE ")
          .Append(QuoteOracleIdentifier(row.HashKeyColumnName!))
          .Append(" = ")
          .Append(CreateOracleParameterPlaceholder(columns.Count))
          .Append(')');
    }
    else {
      builder.Append(" FROM DUAL");
    }

    return builder.ToString();
  }

  private static string CreateLatestSatelliteHashDiffsCommandText(
      SatelliteTableProjection table,
      IReadOnlyList<string> parentHashKeyParameterNames) {
    var builder = new StringBuilder();
    builder.Append("SELECT ")
        .Append(QuoteOracleIdentifier(table.ParentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteOracleIdentifier(table.HashDiffColumnName))
        .Append(", ")
        .Append(QuoteOracleIdentifier(table.LoadTimestampColumnName))
        .Append(" FROM (SELECT ")
        .Append(QuoteOracleIdentifier(table.ParentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteOracleIdentifier(table.HashDiffColumnName))
        .Append(", ")
        .Append(QuoteOracleIdentifier(table.LoadTimestampColumnName))
        .Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteOracleIdentifier(table.ParentHashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteOracleIdentifier(table.LoadTimestampColumnName))
        .Append(" DESC) AS ")
        .Append(QuoteOracleIdentifier("rn"))
        .Append(" FROM ")
        .Append(QuoteOracleIdentifier(table.TableName))
        .Append(" WHERE ")
        .Append(QuoteOracleIdentifier(table.ParentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyParameterNames.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(parentHashKeyParameterNames[index]);
    }

    builder.Append(")) WHERE ")
        .Append(QuoteOracleIdentifier("rn"))
        .Append(" = 1");

    return builder.ToString();
  }

  private static IReadOnlyList<string> AddCommandParameters(
      DbCommand command,
      IEnumerable<string> values) {
    var parameterNames = new List<string>();

    foreach (var value in values) {
      var parameterName = CreateOracleParameterPlaceholder(command.Parameters.Count);
      AddParameter(command, value);
      parameterNames.Add(parameterName);
    }

    return parameterNames;
  }

  private static void AddParameter(DbCommand command, object value) {
    var parameter = command.CreateParameter();
    parameter.ParameterName = CreateOracleParameterName(command.Parameters.Count);
    parameter.Value = value;
    command.Parameters.Add(parameter);
  }

  private static string GetRequiredString(DbDataReader reader, int ordinal) {
    return reader.GetValue(ordinal) as string ??
        Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("Oracle Data Vault latest satellite lookup returned a null value.");
  }

  private static string CreateOracleParameterName(int index) {
    return "p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreateOracleParameterPlaceholder(int index) {
    return ":" + CreateOracleParameterName(index);
  }

  private static string QuoteOracleIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string FormatLoadTimestamp(DateTimeOffset timestamp) {
    return timestamp.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
  }

  private static DateTimeOffset ParseLoadTimestamp(string value) {
    return DateTimeOffset.Parse(
        value,
        CultureInfo.InvariantCulture,
        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
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

  private sealed record OracleInsertRow(
      string TableName,
      string? HashKeyColumnName,
      string? HashKey,
      Dictionary<string, object> Values,
      OracleInsertConflictBehavior ConflictBehavior);

  private enum OracleInsertConflictBehavior {
    Fail,
    Ignore,
  }
}
