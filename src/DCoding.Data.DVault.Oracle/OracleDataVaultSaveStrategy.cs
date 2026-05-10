using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class OracleDataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
  private const int OracleMaxCommandParameterCount = 60000;
  private const int OracleMaxRowsPerCommand = 250;
  private const int MinimumOptimizedBatchOperationCount = 50;
  private const string OrdinalColumnName = "__dvault_ordinal";
  private const string RowNumberColumnName = "__dvault_row_number";
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  internal const string OracleProviderName = "Oracle.EntityFrameworkCore";

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return DataVaultProviderSaveStrategyGateEvaluator.EvaluateOracle(dbContext, requests).CanSave;
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var uniquePlans = CreateUniqueRowSavePlans(context);
    var satellitePlans = CreateSatelliteSavePlans(context);
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
        uniquePlans
            .Select(plan => new OracleInsertRow(
                plan.Table.TableName,
                plan.Table.HashKeyColumnName,
                plan.Row,
                OracleInsertConflictBehavior.Ignore))
            .Concat(filteredSatellitePlans.RowsToWrite.Select(plan => new OracleInsertRow(
                plan.Table.TableName,
                HashKeyColumnName: null,
                plan.Row,
                OracleInsertConflictBehavior.Fail))),
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
    foreach (var request in requests) {
      operationCount += request.HubOperations.Count + request.LinkOperations.Count + request.SatelliteOperations.Count;
    }

    return operationCount >= MinimumOptimizedBatchOperationCount;
  }

  private static bool ContainsMultiActiveSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    return requests.Any(request => request.SatelliteOperations.Any(operation => operation.Metadata.DrivingKeyNames.Count > 0));
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
        .Select(participant => participant.HubReference.Name)
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
              DataVaultLoadTimestampValueConverter.ReadProviderValue(reader.GetValue(2))));
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
    var rowArray = rows
        .Select((row, index) => row with { Ordinal = index })
        .ToArray();
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
      foreach (var group in rowArray.GroupBy(row => new OracleInsertRowShape(
          row.TableName,
          row.HashKeyColumnName,
          CreateColumnSignature(row.Values.Keys),
          row.ConflictBehavior))) {
        var columns = group.First().Values.Keys.ToArray();
        var parameterCountPerRow = columns.Length +
            (group.Key.ConflictBehavior == OracleInsertConflictBehavior.Ignore ? 1 : 0);
        var chunkSize = Math.Min(
            OracleMaxRowsPerCommand,
            Math.Max(1, OracleMaxCommandParameterCount / parameterCountPerRow));

        foreach (var chunk in group.Chunk(chunkSize)) {
          rowsWritten += group.Key.ConflictBehavior == OracleInsertConflictBehavior.Ignore
              ? await ExecuteOracleUniqueInsertChunkAsync(
                  connection,
                  transaction,
                  group.Key.TableName,
                  columns,
                  group.Key.HashKeyColumnName
                      ?? throw new InvalidOperationException("Oracle unique insert rows require a hash key column."),
                  chunk,
                  cancellationToken).ConfigureAwait(false)
              : await ExecuteOracleInsertAllChunkAsync(
                  connection,
                  transaction,
                  group.Key.TableName,
                  columns,
                  chunk.Select(row => row.Values).ToArray(),
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

  private static async Task<int> ExecuteOracleUniqueInsertChunkAsync(
      DbConnection connection,
      DbTransaction transaction,
      string tableName,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      IReadOnlyList<OracleInsertRow> rows,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;

    if (HasDistinctHashKeys(rows, hashKeyColumnName) && TrySetOracleArrayBindCount(command, rows.Count)) {
      command.CommandText = CreateOracleArrayUniqueInsertCommandText(tableName, columns, hashKeyColumnName);
      foreach (var column in columns) {
        AddParameter(command, CreateOracleArrayParameterValue(rows.Select(row => row.Values[column])));
      }

      AddParameter(command, CreateOracleArrayParameterValue(rows.Select(row => row.Values[hashKeyColumnName])));

      var arrayAffectedRows = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

      return arrayAffectedRows >= 0 ? arrayAffectedRows : rows.Count;
    }

    command.CommandText = CreateOracleUniqueInsertCommandText(tableName, columns, hashKeyColumnName, rows.Count);

    foreach (var row in rows) {
      AddParameter(command, row.Ordinal);
      foreach (var column in columns) {
        AddParameter(command, row.Values[column]);
      }
    }

    var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

    return affectedRows >= 0 ? affectedRows : rows.Count;
  }

  private static async Task<int> ExecuteOracleInsertAllChunkAsync(
      DbConnection connection,
      DbTransaction transaction,
      string tableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;

    if (TrySetOracleArrayBindCount(command, rows.Count)) {
      command.CommandText = CreateOracleArrayInsertCommandText(tableName, columns);
      foreach (var column in columns) {
        AddParameter(command, CreateOracleArrayParameterValue(rows.Select(row => row[column])));
      }

      var arrayAffectedRows = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

      return arrayAffectedRows >= 0 ? arrayAffectedRows : rows.Count;
    }

    command.CommandText = CreateOracleInsertAllCommandText(tableName, columns, rows.Count);

    foreach (var row in rows) {
      foreach (var column in columns) {
        AddParameter(command, row[column]);
      }
    }

    var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

    return affectedRows >= 0 ? affectedRows : rows.Count;
  }

  internal static string CreateOracleArrayInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);

    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuoteOracleIdentifier(tableName))
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") VALUES (");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateOracleParameterPlaceholder(columnIndex));
    }

    builder.Append(')');

    return builder.ToString();
  }

  internal static string CreateOracleArrayUniqueInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      string hashKeyColumnName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashKeyColumnName);

    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuoteOracleIdentifier(tableName))
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") SELECT ");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateOracleParameterPlaceholder(columnIndex));
    }

    builder.Append(" FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM ")
        .Append(QuoteOracleIdentifier(tableName))
        .Append(" WHERE ")
        .Append(QuoteOracleIdentifier(hashKeyColumnName))
        .Append(" = ")
        .Append(CreateOracleParameterPlaceholder(columns.Count))
        .Append(')');

    return builder.ToString();
  }

  internal static string CreateOracleInsertAllCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      int rowCount) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);
    if (rowCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(rowCount));
    }

    var builder = new StringBuilder();
    builder.Append("INSERT ALL");

    var parameterIndex = 0;
    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
      builder.Append(" INTO ")
          .Append(QuoteOracleIdentifier(tableName))
          .Append(" (");
      AppendIdentifierList(builder, columns);
      builder.Append(") VALUES (");

      for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
        if (columnIndex > 0) {
          builder.Append(", ");
        }

        builder.Append(CreateOracleParameterPlaceholder(parameterIndex));
        parameterIndex++;
      }

      builder.Append(')');
    }

    builder.Append(" SELECT 1 FROM DUAL");

    return builder.ToString();
  }

  internal static string CreateOracleUniqueInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      int rowCount) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashKeyColumnName);
    if (rowCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(rowCount));
    }

    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuoteOracleIdentifier(tableName))
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") SELECT ");
    AppendQualifiedIdentifierList(builder, "source", columns);
    builder.Append(" FROM (SELECT ");
    AppendQualifiedIdentifierList(builder, "ranked", columns);
    builder.Append(" FROM (SELECT ");
    AppendQualifiedIdentifierList(builder, "source", columns);
    builder.Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteOracleIdentifier("source"))
        .Append('.')
        .Append(QuoteOracleIdentifier(hashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteOracleIdentifier("source"))
        .Append('.')
        .Append(QuoteOracleIdentifier(OrdinalColumnName))
        .Append(") ")
        .Append(QuoteOracleIdentifier(RowNumberColumnName))
        .Append(" FROM (");
    AppendOracleSourceRows(builder, columns, rowCount, includeOrdinal: true);
    builder.Append(") ")
        .Append(QuoteOracleIdentifier("source"))
        .Append(") ")
        .Append(QuoteOracleIdentifier("ranked"))
        .Append(" WHERE ")
        .Append(QuoteOracleIdentifier("ranked"))
        .Append('.')
        .Append(QuoteOracleIdentifier(RowNumberColumnName))
        .Append(" = 1) ")
        .Append(QuoteOracleIdentifier("source"))
        .Append(" WHERE NOT EXISTS (SELECT 1 FROM ")
        .Append(QuoteOracleIdentifier(tableName))
        .Append(" ")
        .Append(QuoteOracleIdentifier("target"))
        .Append(" WHERE ")
        .Append(QuoteOracleIdentifier("target"))
        .Append('.')
        .Append(QuoteOracleIdentifier(hashKeyColumnName))
        .Append(" = ")
        .Append(QuoteOracleIdentifier("source"))
        .Append('.')
        .Append(QuoteOracleIdentifier(hashKeyColumnName))
        .Append(')');

    return builder.ToString();
  }

  private static void AppendOracleSourceRows(
      StringBuilder builder,
      IReadOnlyList<string> columns,
      int rowCount,
      bool includeOrdinal) {
    var parameterIndex = 0;

    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
      if (rowIndex > 0) {
        builder.Append(" UNION ALL ");
      }

      builder.Append("SELECT ");
      if (includeOrdinal) {
        builder.Append(CreateOracleParameterPlaceholder(parameterIndex))
            .Append(' ')
            .Append(QuoteOracleIdentifier(OrdinalColumnName));
        parameterIndex++;
      }

      for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
        if (columnIndex > 0 || includeOrdinal) {
          builder.Append(", ");
        }

        builder.Append(CreateOracleParameterPlaceholder(parameterIndex))
            .Append(' ')
            .Append(QuoteOracleIdentifier(columns[columnIndex]));
        parameterIndex++;
      }

      builder.Append(" FROM DUAL");
    }
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

  private static bool HasDistinctHashKeys(IReadOnlyList<OracleInsertRow> rows, string hashKeyColumnName) {
    var hashKeys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var row in rows) {
      if (!row.Values.TryGetValue(hashKeyColumnName, out var hashKeyValue) ||
          hashKeyValue is not string hashKey ||
          !hashKeys.Add(hashKey)) {
        return false;
      }
    }

    return true;
  }

  private static object CreateOracleArrayParameterValue(IEnumerable<object> values) {
    var valueArray = values.ToArray();
    if (valueArray.All(value => value is string)) {
      return valueArray.Cast<string>().ToArray();
    }

    if (valueArray.All(value => value is int)) {
      return valueArray.Cast<int>().ToArray();
    }

    if (valueArray.All(value => value is long)) {
      return valueArray.Cast<long>().ToArray();
    }

    if (valueArray.All(value => value is DateTime)) {
      return valueArray.Cast<DateTime>().ToArray();
    }

    if (valueArray.All(value => value is DateTimeOffset)) {
      return valueArray.Cast<DateTimeOffset>().ToArray();
    }

    return valueArray;
  }

  private static bool TrySetOracleArrayBindCount(DbCommand command, int rowCount) {
    var arrayBindCountProperty = command.GetType().GetProperty("ArrayBindCount");
    if (arrayBindCountProperty is null ||
        arrayBindCountProperty.PropertyType != typeof(int) ||
        !arrayBindCountProperty.CanWrite) {
      return false;
    }

    arrayBindCountProperty.SetValue(command, rowCount);

    return true;
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

  private static void AppendIdentifierList(
      StringBuilder builder,
      IReadOnlyList<string> identifiers) {
    for (var index = 0; index < identifiers.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteOracleIdentifier(identifiers[index]));
    }
  }

  private static void AppendQualifiedIdentifierList(
      StringBuilder builder,
      string qualifier,
      IReadOnlyList<string> identifiers) {
    for (var index = 0; index < identifiers.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteOracleIdentifier(qualifier))
          .Append('.')
          .Append(QuoteOracleIdentifier(identifiers[index]));
    }
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

  private sealed record OracleInsertRow(
      string TableName,
      string? HashKeyColumnName,
      Dictionary<string, object> Values,
      OracleInsertConflictBehavior ConflictBehavior) {
    public int Ordinal { get; init; }
  }

  private sealed record OracleInsertRowShape(
      string TableName,
      string? HashKeyColumnName,
      string ColumnSignature,
      OracleInsertConflictBehavior ConflictBehavior);

  private enum OracleInsertConflictBehavior {
    Fail,
    Ignore,
  }
}
