using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class MySqlDataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
  internal const string PomeloProviderName = "Pomelo.EntityFrameworkCore.MySql";
  internal const string OracleProviderName = "MySql.EntityFrameworkCore";

  private const int MySqlMaxCommandParameterCount = 60000;
  private const int MySqlMaxRowsPerCommand = 1000;
  private const int MySqlLatestHashDiffBatchSize = 1000;
  private const int MinimumOptimizedBatchOperationCount = 50;
  private const string LatestRowsTableAlias = "__dvault_latest";
  private const string RowNumberColumnName = "__dvault_row_number";
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySql(dbContext, requests).CanSave;
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var uniquePlans = CreateUniqueRowSavePlans(context);
    var satellitePlans = CreateSatelliteSavePlans(context);
    var connection = context.DbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    DbTransaction? localTransaction = null;
    var transaction = context.DbContext.Database.CurrentTransaction?.GetDbTransaction();
    if (transaction is null) {
      localTransaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
      transaction = localTransaction;
    }

    try {
      var filteredSatellitePlans = await FilterSatellitePlansAsync(
          connection,
          transaction,
          satellitePlans,
          cancellationToken).ConfigureAwait(false);
      var savedRecords = uniquePlans
          .Select(plan => plan.SavedRecord)
          .Concat(filteredSatellitePlans.Results.Select(result => result.SavedRecord))
          .ToArray();
      var rowsWritten = await ExecuteMySqlInsertRowsAsync(
          connection,
          transaction,
          uniquePlans.Select(plan => new MySqlInsertRow(plan.Table.TableName, plan.Row)),
          MySqlInsertConflictBehavior.Ignore,
          cancellationToken).ConfigureAwait(false);

      rowsWritten += await ExecuteMySqlInsertRowsAsync(
          connection,
          transaction,
          filteredSatellitePlans.RowsToWrite.Select(plan => new MySqlInsertRow(plan.Table.TableName, plan.Row)),
          MySqlInsertConflictBehavior.Fail,
          cancellationToken).ConfigureAwait(false);

      if (localTransaction is not null) {
        await localTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
      }

      return new DataVaultSaveResult(rowsWritten, savedRecords);
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

  internal static bool IsSupportedProviderName(string? providerName) {
    return string.Equals(providerName, PomeloProviderName, StringComparison.Ordinal) ||
        string.Equals(providerName, OracleProviderName, StringComparison.Ordinal);
  }

  internal static bool IsOptimizedBatchShape(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var operationCount = 0;
    foreach (var request in requests) {
      operationCount += request.HubOperations.Count + request.LinkOperations.Count + request.SatelliteOperations.Count;
    }

    return operationCount >= MinimumOptimizedBatchOperationCount;
  }

  private static bool ContainsMultiActiveSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    return requests.Any(request => request.SatelliteOperations.Any(operation => operation.Metadata.DrivingKeyNames.Count > 0));
  }

  internal static string CreateMySqlInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      int rowCount,
      MySqlInsertConflictBehavior conflictBehavior) {
    var builder = new StringBuilder();
    builder.Append("INSERT ");
    if (conflictBehavior == MySqlInsertConflictBehavior.Ignore) {
      builder.Append("IGNORE ");
    }

    builder.Append("INTO ")
        .Append(QuoteMySqlIdentifier(tableName))
        .Append(" (");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteMySqlIdentifier(columns[columnIndex]));
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

        builder.Append(CreateMySqlParameterName(parameterIndex));
        parameterIndex++;
      }

      builder.Append(')');
    }

    return builder.ToString();
  }

  private static IReadOnlyList<UniqueRowSavePlan> CreateUniqueRowSavePlans(DataVaultProviderSaveStrategyContext context) {
    var plans = new List<UniqueRowSavePlan>();
    var hubProjections = new Dictionary<DataVaultHubMetadata, HubProjection>();
    var linkProjections = new Dictionary<DataVaultLinkMetadata, LinkProjection>();

    foreach (var request in context.ResolvedRequests) {
      plans.AddRange(request.Request.HubOperations.Select(operation => CreateHubSavePlan(
          context,
          request,
          operation,
          GetHubProjection(hubProjections, operation.Metadata))));
      plans.AddRange(request.Request.LinkOperations.Select(operation => CreateLinkSavePlan(
          context,
          request,
          operation,
          GetLinkProjection(linkProjections, operation.Metadata))));
    }

    return plans.ToArray();
  }

  private static UniqueRowSavePlan CreateHubSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultResolvedSaveRequest request,
      DataVaultHubSaveOperation operation,
      HubProjection projection) {
    var hub = operation.Metadata;
    var businessKeyFields = hub.BusinessKeyColumns
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.BusinessKeyValues, column.ColumnName, nameof(operation.BusinessKeyValues))))
        .ToArray();
    var hashKey = ComputeHash(context, businessKeyFields);
    var row = new Dictionary<string, object> {
      [projection.HashKeyColumnName] = hashKey,
      [projection.LoadTimestampColumnName] = DataVaultLoadTimestampValueConverter.ToProviderValue(
          context.DbContext,
          projection.TableName,
          projection.LoadTimestampColumnName,
          request.LoadTimestamp),
      [projection.RecordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < businessKeyFields.Length; index++) {
      row.Add(projection.BusinessKeyColumnNames[index], businessKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(projection.TableName, projection.HashKeyColumnName),
        hashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Hub, hub.Name, projection.TableName, hashKey));
  }

  private static UniqueRowSavePlan CreateLinkSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultResolvedSaveRequest request,
      DataVaultLinkSaveOperation operation,
      LinkProjection projection) {
    var link = operation.Metadata;
    var participantNames = link.Participants
        .Select(participant => participant.SourceEndpointName)
        .ToArray();
    var participantHashKeyFields = participantNames
        .Select(participantName => new KeyValuePair<string, string>(
            participantName,
            GetRequiredValue(operation.ParticipantHashKeyValues, participantName, nameof(operation.ParticipantHashKeyValues))))
        .ToArray();
    var linkHashKey = ComputeHash(context, participantHashKeyFields);
    var row = new Dictionary<string, object> {
      [projection.LinkHashKeyColumnName] = linkHashKey,
      [projection.LoadTimestampColumnName] = DataVaultLoadTimestampValueConverter.ToProviderValue(
          context.DbContext,
          projection.TableName,
          projection.LoadTimestampColumnName,
          request.LoadTimestamp),
      [projection.RecordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < participantHashKeyFields.Length; index++) {
      row.Add(projection.ParticipantHashKeyColumnNames[index], participantHashKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(projection.TableName, projection.LinkHashKeyColumnName),
        linkHashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, projection.TableName, linkHashKey));
  }

  private static IReadOnlyList<SatelliteSavePlan> CreateSatelliteSavePlans(DataVaultProviderSaveStrategyContext context) {
    var satelliteProjections = new Dictionary<DataVaultSatelliteMetadata, SatelliteProjection>();

    return context.ResolvedRequests
        .SelectMany(request => request.Request.SatelliteOperations
            .Select(operation => CreateSatelliteSavePlan(
                context.DbContext,
                request,
                operation,
                GetSatelliteProjection(satelliteProjections, operation.Metadata))))
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  private static SatelliteSavePlan CreateSatelliteSavePlan(
      DbContext dbContext,
      DataVaultResolvedSaveRequest request,
      DataVaultSatelliteSaveOperation operation,
      SatelliteProjection projection) {
    var satellite = operation.Metadata;
    var payloadFields = satellite.PayloadColumns
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.PayloadValues, column.ColumnName, nameof(operation.PayloadValues))))
        .ToArray();
    var row = new Dictionary<string, object> {
      [projection.ParentHashKeyColumnName] = operation.ParentHashKey,
      [projection.HashDiffColumnName] = operation.HashDiff,
      [projection.LoadTimestampColumnName] = DataVaultLoadTimestampValueConverter.ToProviderValue(
          dbContext,
          projection.TableName,
          projection.LoadTimestampColumnName,
          request.LoadTimestamp),
      [projection.RecordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < payloadFields.Length; index++) {
      row.Add(projection.PayloadColumnNames[index], payloadFields[index].Value);
    }

    var table = new SatelliteTableProjection(
        projection.TableName,
        projection.ParentHashKeyColumnName,
        projection.HashDiffColumnName,
        projection.LoadTimestampColumnName);
    var savedRecord = new DataVaultSavedRecord(
        DataVaultTableKind.Satellite,
        satellite.Name,
        projection.TableName,
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

  private static HubProjection GetHubProjection(
      Dictionary<DataVaultHubMetadata, HubProjection> projections,
      DataVaultHubMetadata hub) {
    if (!projections.TryGetValue(hub, out var projection)) {
      projection = CreateHubProjection(hub);
      projections.Add(hub, projection);
    }

    return projection;
  }

  private static LinkProjection GetLinkProjection(
      Dictionary<DataVaultLinkMetadata, LinkProjection> projections,
      DataVaultLinkMetadata link) {
    if (!projections.TryGetValue(link, out var projection)) {
      projection = CreateLinkProjection(link);
      projections.Add(link, projection);
    }

    return projection;
  }

  private static SatelliteProjection GetSatelliteProjection(
      Dictionary<DataVaultSatelliteMetadata, SatelliteProjection> projections,
      DataVaultSatelliteMetadata satellite) {
    if (!projections.TryGetValue(satellite, out var projection)) {
      projection = CreateSatelliteProjection(satellite);
      projections.Add(satellite, projection);
    }

    return projection;
  }

  private static HubProjection CreateHubProjection(DataVaultHubMetadata hub) {
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

    return new HubProjection(
        tableName,
        hashKeyColumnName,
        loadTimestampColumnName,
        recordSourceColumnName,
        businessKeyColumnNames);
  }

  private static LinkProjection CreateLinkProjection(DataVaultLinkMetadata link) {
    var participantNames = link.Participants
        .Select(participant => participant.SourceEndpointName)
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

    return new LinkProjection(
        tableName,
        linkHashKeyColumnName,
        loadTimestampColumnName,
        recordSourceColumnName,
        participantHashKeyColumnNames);
  }

  private static SatelliteProjection CreateSatelliteProjection(DataVaultSatelliteMetadata satellite) {
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

    return new SatelliteProjection(
        tableName,
        parentHashKeyColumnName,
        hashDiffColumnName,
        loadTimestampColumnName,
        recordSourceColumnName,
        payloadColumnNames);
  }

  private static async Task<FilteredSatelliteSavePlans> FilterSatellitePlansAsync(
      DbConnection connection,
      DbTransaction transaction,
      IReadOnlyList<SatelliteSavePlan> plans,
      CancellationToken cancellationToken) {
    var results = new SaveOperationResult[plans.Count];
    var rowsToWrite = new List<SatelliteSavePlan>();

    foreach (var group in plans.GroupBy(plan => plan.Table)) {
      var latestHashDiffs = await LoadLatestSatelliteHashDiffsAsync(
          connection,
          transaction,
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
      DbConnection connection,
      DbTransaction transaction,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var latestRows = new Dictionary<string, LatestSatelliteHashDiff>(StringComparer.Ordinal);

    foreach (var parentHashKeyBatch in parentHashKeys
        .Distinct(StringComparer.Ordinal)
        .Chunk(MySqlLatestHashDiffBatchSize)) {
      await using var command = connection.CreateCommand();
      command.Transaction = transaction;

      var parameterNames = AddCommandParameters(command, parentHashKeyBatch);
      command.CommandText = CreateLatestSatelliteHashDiffsCommandText(table, parameterNames);

      await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        var parentHashKey = GetRequiredString(reader, ordinal: 0);
        latestRows[parentHashKey] = new LatestSatelliteHashDiff(
            parentHashKey,
            GetRequiredString(reader, ordinal: 1),
            GetRequiredDateTimeOffset(reader, ordinal: 2));
      }
    }

    return latestRows;
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

  private static async Task<int> ExecuteMySqlInsertRowsAsync(
      DbConnection connection,
      DbTransaction transaction,
      IEnumerable<MySqlInsertRow> rows,
      MySqlInsertConflictBehavior conflictBehavior,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

    var rowsWritten = 0;

    foreach (var group in rowArray.GroupBy(row => new MySqlInsertRowShape(
        row.TableName,
        CreateColumnSignature(row.Values.Keys)))) {
      var columns = group.First().Values.Keys.ToArray();
      var chunkSize = Math.Min(MySqlMaxRowsPerCommand, Math.Max(1, MySqlMaxCommandParameterCount / columns.Length));

      foreach (var chunk in group.Chunk(chunkSize)) {
        rowsWritten += await ExecuteMySqlInsertChunkAsync(
            connection,
            transaction,
            group.Key.TableName,
            columns,
            chunk.Select(row => row.Values).ToArray(),
            conflictBehavior,
            cancellationToken).ConfigureAwait(false);
      }
    }

    return rowsWritten;
  }

  private static async Task<int> ExecuteMySqlInsertChunkAsync(
      DbConnection connection,
      DbTransaction transaction,
      string tableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      MySqlInsertConflictBehavior conflictBehavior,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreateMySqlInsertCommandText(tableName, columns, rows.Count, conflictBehavior);

    var parameterIndex = 0;
    foreach (var row in rows) {
      foreach (var column in columns) {
        var parameter = command.CreateParameter();
        parameter.ParameterName = CreateMySqlParameterName(parameterIndex);
        parameter.Value = row[column];
        command.Parameters.Add(parameter);
        parameterIndex++;
      }
    }

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static string CreateMySqlParameterName(int index) {
    return "@p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreateLatestSatelliteHashDiffsCommandText(
      SatelliteTableProjection table,
      IReadOnlyList<string> parentHashKeyParameterNames) {
    return CreateLatestSatelliteHashDiffsCommandText(
        table.TableName,
        table.ParentHashKeyColumnName,
        table.HashDiffColumnName,
        table.LoadTimestampColumnName,
        parentHashKeyParameterNames);
  }

  internal static string CreateLatestSatelliteHashDiffsCommandText(
      string tableName,
      string parentHashKeyColumnName,
      string hashDiffColumnName,
      string loadTimestampColumnName,
      IReadOnlyList<string> parentHashKeyParameterNames) {
    var builder = new StringBuilder();
    builder.Append("SELECT ")
        .Append(QuoteMySqlIdentifier(parentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteMySqlIdentifier(hashDiffColumnName))
        .Append(", ")
        .Append(QuoteMySqlIdentifier(loadTimestampColumnName))
        .Append(" FROM (SELECT ")
        .Append(QuoteMySqlIdentifier(parentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteMySqlIdentifier(hashDiffColumnName))
        .Append(", ")
        .Append(QuoteMySqlIdentifier(loadTimestampColumnName))
        .Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteMySqlIdentifier(parentHashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteMySqlIdentifier(loadTimestampColumnName))
        .Append(" DESC) AS ")
        .Append(QuoteMySqlIdentifier(RowNumberColumnName))
        .Append(" FROM ")
        .Append(QuoteMySqlIdentifier(tableName))
        .Append(" WHERE ")
        .Append(QuoteMySqlIdentifier(parentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyParameterNames.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(parentHashKeyParameterNames[index]);
    }

    builder.Append(")) AS ")
        .Append(QuoteMySqlIdentifier(LatestRowsTableAlias))
        .Append(" WHERE ")
        .Append(QuoteMySqlIdentifier(RowNumberColumnName))
        .Append(" = 1");

    return builder.ToString();
  }

  private static IReadOnlyList<string> AddCommandParameters(
      DbCommand command,
      IEnumerable<string> values) {
    var parameterNames = new List<string>();

    foreach (var value in values) {
      var parameterName = CreateMySqlParameterName(command.Parameters.Count);
      var parameter = command.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.Value = value;
      command.Parameters.Add(parameter);
      parameterNames.Add(parameterName);
    }

    return parameterNames;
  }

  private static string GetRequiredString(DbDataReader reader, int ordinal) {
    return reader.GetValue(ordinal) as string ??
        Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("MySQL Data Vault latest satellite lookup returned a null value.");
  }

  private static DateTimeOffset GetRequiredDateTimeOffset(DbDataReader reader, int ordinal) {
    return DataVaultLoadTimestampValueConverter.ReadProviderValue(reader.GetValue(ordinal));
  }

  private static string QuoteMySqlIdentifier(string identifier) {
    return "`" + identifier.Replace("`", "``", StringComparison.Ordinal) + "`";
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

  private sealed record HubProjection(
      string TableName,
      string HashKeyColumnName,
      string LoadTimestampColumnName,
      string RecordSourceColumnName,
      IReadOnlyList<string> BusinessKeyColumnNames);

  private sealed record LinkProjection(
      string TableName,
      string LinkHashKeyColumnName,
      string LoadTimestampColumnName,
      string RecordSourceColumnName,
      IReadOnlyList<string> ParticipantHashKeyColumnNames);

  private sealed record SatelliteProjection(
      string TableName,
      string ParentHashKeyColumnName,
      string HashDiffColumnName,
      string LoadTimestampColumnName,
      string RecordSourceColumnName,
      IReadOnlyList<string> PayloadColumnNames);

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

  private sealed record MySqlInsertRow(string TableName, Dictionary<string, object> Values);

  private sealed record MySqlInsertRowShape(string TableName, string ColumnSignature);
}

internal enum MySqlInsertConflictBehavior {
  Fail,
  Ignore,
}
