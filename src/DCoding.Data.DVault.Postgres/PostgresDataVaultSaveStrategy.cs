using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class PostgresDataVaultSaveStrategy : IDataVaultProviderSaveStrategy, IDataVaultProviderStagedBulkSaveDiagnostics {
  private const int PostgresMaxCommandParameterCount = 30000;
  private const int PostgresUnnestInsertMinimumRowCount = 32;
  internal const int MinimumStagedBulkOperationCount = 60;
  private const string OrdinalColumnName = "__dvault_ordinal";
  private const string RowNumberColumnName = "__dvault_row_number";
  private const string StagingTablePrefix = "__dvault_stage_";
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  internal const string NpgsqlProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return DataVaultProviderSaveStrategyGateEvaluator.EvaluatePostgres(dbContext, requests).CanSave;
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);
    cancellationToken.ThrowIfCancellationRequested();

    var uniquePlans = CreateUniqueRowSavePlans(context);
    var satellitePlans = CreateSatelliteSavePlans(context);

    if (uniquePlans.Count == 0 && satellitePlans.Count == 0) {
      return new DataVaultSaveResult(0, []);
    }

    return await ExecutePostgresSaveAsync(
        context.DbContext,
        uniquePlans,
        satellitePlans,
        IsStagedBatchShape(context.Requests),
        cancellationToken).ConfigureAwait(false);
  }

  public DataVaultStagedProviderBulkDiagnostics? EvaluateStagedProviderBulkSave(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    if (!IsSupportedProviderName(dbContext.Database.ProviderName)) {
      return null;
    }

    var counts = CountSaveOperations(requests);
    if (counts.OperationCount == 0) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.NotEvaluated,
          DataVaultStagedProviderBulkProviderCaveatKind.None,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          []);
    }

    if (DataVaultProviderSaveStrategyGateEvaluator.HasPendingTrackedChanges(dbContext)) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.Declined,
          DataVaultStagedProviderBulkProviderCaveatKind.DirtyContext,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkDirtyDbContext]);
    }

    if (ContainsMultiActiveSatelliteOperations(requests)) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.Declined,
          DataVaultStagedProviderBulkProviderCaveatKind.UnsupportedShape,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape]);
    }

    if (!IsStagedBatchShape(requests)) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.Declined,
          DataVaultStagedProviderBulkProviderCaveatKind.UnsupportedShape,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape]);
    }

    return new DataVaultStagedProviderBulkDiagnostics(
        DataVaultStagedProviderBulkLifecyclePhase.NativeBulkApplication,
        DataVaultStagedProviderBulkProviderCaveatKind.None,
        counts.RequestCount,
        counts.HubOperationCount,
        counts.LinkOperationCount,
        counts.SatelliteOperationCount,
        []);
  }

  private static async Task<DataVaultSaveResult> ExecutePostgresSaveAsync(
      DbContext dbContext,
      IReadOnlyList<UniqueRowSavePlan> uniquePlans,
      IReadOnlyList<SatelliteSavePlan> satellitePlans,
      bool useStagedBulk,
      CancellationToken cancellationToken) {
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
      var filteredSatellitePlans = await FilterSatellitePlansAsync(
          connection,
          transaction,
          dbContext,
          satellitePlans,
          cancellationToken).ConfigureAwait(false);
      var savedRecords = uniquePlans
          .Select(plan => plan.SavedRecord)
          .Concat(filteredSatellitePlans.Results.Select(result => result.SavedRecord))
          .ToArray();
      int rowsWritten;
      if (useStagedBulk && SupportsPostgresTextCopy(connection)) {
        rowsWritten = await ExecutePostgresStagedInsertRowsAsync(
            connection,
            transaction,
            dbContext,
            uniquePlans.Select(plan => new PostgresStagedInsertRow(
                plan.Ordinal,
                plan.Table.TableName,
                plan.Row,
                plan.Table.HashKeyColumnName)),
            cancellationToken).ConfigureAwait(false);

        rowsWritten += await ExecutePostgresStagedInsertRowsAsync(
            connection,
            transaction,
            dbContext,
            filteredSatellitePlans.RowsToWrite.Select(plan => new PostgresStagedInsertRow(
                plan.Ordinal,
                plan.Table.TableName,
                plan.Row,
                ConflictTargetColumnName: null)),
            cancellationToken).ConfigureAwait(false);
      }
      else {
        rowsWritten = await ExecutePostgresInsertRowsAsync(
            connection,
            transaction,
            dbContext,
            uniquePlans.Select(plan => new PostgresInsertRow(plan.Table.TableName, plan.Row, plan.Table.HashKeyColumnName)),
            cancellationToken).ConfigureAwait(false);

        rowsWritten += await ExecutePostgresInsertRowsAsync(
            connection,
            transaction,
            dbContext,
            filteredSatellitePlans.RowsToWrite.Select(plan => new PostgresInsertRow(plan.Table.TableName, plan.Row, ConflictTargetColumnName: null)),
            cancellationToken).ConfigureAwait(false);
      }

      if (localTransaction is not null) {
        await localTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
      }

      return new DataVaultSaveResult(rowsWritten, savedRecords);
    }
    catch {
      if (localTransaction is not null) {
        await localTransaction.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
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

    return plans
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  internal static bool IsSupportedProviderName(string? providerName) {
    return string.Equals(providerName, NpgsqlProviderName, StringComparison.Ordinal);
  }

  internal static bool IsStagedBatchShape(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    return CountSaveOperations(requests).OperationCount >= MinimumStagedBulkOperationCount;
  }

  private static bool ContainsMultiActiveSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    return requests.Any(request => request.SatelliteOperations.Any(operation => operation.Metadata.DrivingKeyNames.Count > 0));
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
      [projection.HashKeyColumnName] = ToProviderHashKeyValue(
          context.DbContext,
          projection.TableName,
          projection.HashKeyColumnName,
          hashKey),
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
        new DataVaultSavedRecord(DataVaultTableKind.Hub, hub.Name, projection.TableName, hashKey),
        Ordinal: -1);
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
    var dependentChildKeyFields = link.DependentChildKeys
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.DependentChildKeyValues, column.ColumnName, nameof(operation.DependentChildKeyValues))))
        .ToArray();
    var linkHashKey = ComputeHash(context, participantHashKeyFields.Concat(dependentChildKeyFields));
    var row = new Dictionary<string, object> {
      [projection.LinkHashKeyColumnName] = ToProviderHashKeyValue(
          context.DbContext,
          projection.TableName,
          projection.LinkHashKeyColumnName,
          linkHashKey),
      [projection.LoadTimestampColumnName] = DataVaultLoadTimestampValueConverter.ToProviderValue(
          context.DbContext,
          projection.TableName,
          projection.LoadTimestampColumnName,
          request.LoadTimestamp),
      [projection.RecordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < participantHashKeyFields.Length; index++) {
      row.Add(
          projection.ParticipantHashKeyColumnNames[index],
          ToProviderHashKeyValue(
              context.DbContext,
              projection.TableName,
              projection.ParticipantHashKeyColumnNames[index],
              participantHashKeyFields[index].Value));
    }

    for (var index = 0; index < dependentChildKeyFields.Length; index++) {
      row.Add(projection.DependentChildKeyColumnNames[index], dependentChildKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(projection.TableName, projection.LinkHashKeyColumnName),
        linkHashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, projection.TableName, linkHashKey, [], dependentChildKeyFields),
        Ordinal: -1);
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
      [projection.ParentHashKeyColumnName] = ToProviderHashKeyValue(
          dbContext,
          projection.TableName,
          projection.ParentHashKeyColumnName,
          operation.ParentHashKey),
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
    var dependentChildKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        link.DependentChildKeys.Select(column => column.ColumnName),
        [linkHashKeyColumnName, loadTimestampColumnName, recordSourceColumnName, .. participantHashKeyColumnNames]);

    return new LinkProjection(
        tableName,
        linkHashKeyColumnName,
        loadTimestampColumnName,
        recordSourceColumnName,
        participantHashKeyColumnNames,
        dependentChildKeyColumnNames);
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
      DbContext dbContext,
      IReadOnlyList<SatelliteSavePlan> plans,
      CancellationToken cancellationToken) {
    var results = new SaveOperationResult[plans.Count];
    var rowsToWrite = new List<SatelliteSavePlan>();

    foreach (var group in plans.GroupBy(plan => plan.Table)) {
      var latestHashDiffs = await LoadLatestSatelliteHashDiffsAsync(
          connection,
          transaction,
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
      DbConnection connection,
      DbTransaction transaction,
      DbContext dbContext,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var latestRows = new List<LatestSatelliteHashDiff>();

    foreach (var parentHashKeyBatch in parentHashKeys.Distinct(StringComparer.Ordinal).Chunk(500)) {
      await using var command = connection.CreateCommand();
      command.Transaction = transaction;

      var parameterNames = AddHashKeyCommandParameters(command, dbContext, table, parentHashKeyBatch);
      command.CommandText = CreateLatestSatelliteHashDiffsCommandText(dbContext, table, parameterNames);

      await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        latestRows.Add(new LatestSatelliteHashDiff(
            ReadHashKeyProviderValue(dbContext, table.TableName, table.ParentHashKeyColumnName, reader.GetValue(0)),
            reader.GetString(1),
            ReadDateTimeOffset(reader, ordinal: 2)));
      }
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

  private static async Task<int> ExecutePostgresInsertRowsAsync(
      DbConnection connection,
      DbTransaction transaction,
      DbContext dbContext,
      IEnumerable<PostgresInsertRow> rows,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

    var rowsWritten = 0;

    foreach (var group in rowArray.GroupBy(row => new PostgresInsertRowShape(
        row.TableName,
        CreateColumnSignature(row.Values.Keys),
        row.ConflictTargetColumnName))) {
      var columns = group.First().Values.Keys.ToArray();
      var chunkSize = Math.Max(1, PostgresMaxCommandParameterCount / columns.Length);

      foreach (var chunk in group.Chunk(chunkSize)) {
        rowsWritten += await ExecutePostgresInsertChunkAsync(
            connection,
            transaction,
            dbContext,
            group.Key.TableName,
            columns,
            chunk.Select(row => row.Values).ToArray(),
            group.Key.ConflictTargetColumnName,
            cancellationToken).ConfigureAwait(false);
      }
    }

    return rowsWritten;
  }

  private static async Task<int> ExecutePostgresStagedInsertRowsAsync(
      DbConnection connection,
      DbTransaction transaction,
      DbContext dbContext,
      IEnumerable<PostgresStagedInsertRow> rows,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

    var rowsWritten = 0;

    foreach (var group in rowArray.GroupBy(row => new PostgresInsertRowShape(
        row.TableName,
        CreateColumnSignature(row.Values.Keys),
        row.ConflictTargetColumnName))) {
      var columns = group.First().Values.Keys.ToArray();
      rowsWritten += await ExecutePostgresStagedInsertGroupAsync(
          connection,
          transaction,
          dbContext,
          group.Key.TableName,
          columns,
          group.Select(row => row).ToArray(),
          group.Key.ConflictTargetColumnName,
          cancellationToken).ConfigureAwait(false);
    }

    return rowsWritten;
  }

  private static async Task<int> ExecutePostgresStagedInsertGroupAsync(
      DbConnection connection,
      DbTransaction transaction,
      DbContext dbContext,
      string tableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<PostgresStagedInsertRow> rows,
      string? conflictTargetColumnName,
      CancellationToken cancellationToken) {
    var stagingTableName = CreatePostgresStagingTableName();
    var targetTableSql = QuotePostgresTableIdentifier(dbContext, tableName);

    try {
      await ExecutePostgresNonQueryAsync(
          connection,
          transaction,
          CreatePostgresCreateStagingTableCommandText(stagingTableName, targetTableSql),
          cancellationToken).ConfigureAwait(false);

      await WriteStagingRowsWithPostgresCopyAsync(
          connection,
          stagingTableName,
          new[] { OrdinalColumnName }.Concat(columns).ToArray(),
          CreatePostgresStagingRows(rows, columns),
          cancellationToken).ConfigureAwait(false);

      var commandText = conflictTargetColumnName is null
          ? CreatePostgresStagedInsertCommandText(targetTableSql, stagingTableName, columns)
          : CreatePostgresStagedUniqueInsertCommandText(targetTableSql, stagingTableName, columns, conflictTargetColumnName);

      return await ExecutePostgresNonQueryAsync(
          connection,
          transaction,
          commandText,
          cancellationToken).ConfigureAwait(false);
    }
    finally {
      await DropPostgresStagingTableAsync(connection, transaction, stagingTableName).ConfigureAwait(false);
    }
  }

  private static async Task<int> ExecutePostgresInsertChunkAsync(
      DbConnection connection,
      DbTransaction transaction,
      DbContext dbContext,
      string tableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      string? conflictTargetColumnName,
      CancellationToken cancellationToken) {
    if (rows.Count >= PostgresUnnestInsertMinimumRowCount) {
      return await ExecutePostgresInsertUnnestChunkAsync(
          connection,
          transaction,
          dbContext,
          tableName,
          columns,
          rows,
          conflictTargetColumnName,
          cancellationToken).ConfigureAwait(false);
    }

    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreatePostgresInsertCommandText(
        dbContext,
        tableName,
        columns,
        rows.Count,
        conflictTargetColumnName);

    var parameterIndex = 0;
    foreach (var row in rows) {
      foreach (var column in columns) {
        var parameter = command.CreateParameter();
        parameter.ParameterName = CreatePostgresParameterName(parameterIndex);
        parameter.Value = row[column];
        command.Parameters.Add(parameter);
        parameterIndex++;
      }
    }

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task<int> ExecutePostgresInsertUnnestChunkAsync(
      DbConnection connection,
      DbTransaction transaction,
      DbContext dbContext,
      string tableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      string? conflictTargetColumnName,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreatePostgresUnnestInsertCommandText(
        dbContext,
        tableName,
        columns,
        conflictTargetColumnName,
        command.Parameters.Count);

    foreach (var column in columns) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = CreatePostgresParameterName(command.Parameters.Count);
      parameter.Value = CreatePostgresArrayParameterValue(rows, column);
      command.Parameters.Add(parameter);
    }

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static string CreatePostgresInsertCommandText(
      DbContext dbContext,
      string tableName,
      IReadOnlyList<string> columns,
      int rowCount,
      string? conflictTargetColumnName) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuotePostgresTableIdentifier(dbContext, tableName))
        .Append(" (");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuotePostgresIdentifier(columns[columnIndex]));
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

        builder.Append(CreatePostgresParameterName(parameterIndex));
        parameterIndex++;
      }

      builder.Append(')');
    }

    if (conflictTargetColumnName is not null) {
      builder.Append(" ON CONFLICT (")
          .Append(QuotePostgresIdentifier(conflictTargetColumnName))
          .Append(") DO NOTHING");
    }

    return builder.ToString();
  }

  private static string CreatePostgresUnnestInsertCommandText(
      DbContext dbContext,
      string tableName,
      IReadOnlyList<string> columns,
      string? conflictTargetColumnName,
      int firstParameterIndex) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuotePostgresTableIdentifier(dbContext, tableName))
        .Append(" (");

    AppendQuotedColumnList(builder, columns);

    builder.Append(") SELECT * FROM unnest(");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder
          .Append(CreatePostgresParameterName(firstParameterIndex + columnIndex))
          .Append("::")
          .Append(GetPostgresArrayCastType(dbContext, tableName, columns[columnIndex]))
          .Append("[]");
    }

    builder.Append(")");

    if (conflictTargetColumnName is not null) {
      builder.Append(" ON CONFLICT (")
          .Append(QuotePostgresIdentifier(conflictTargetColumnName))
          .Append(") DO NOTHING");
    }

    return builder.ToString();
  }

  internal static string CreatePostgresCreateStagingTableCommandText(
      string stagingTableName,
      string targetTableSql) {
    ArgumentException.ThrowIfNullOrWhiteSpace(stagingTableName);
    ArgumentException.ThrowIfNullOrWhiteSpace(targetTableSql);

    return "CREATE TEMPORARY TABLE " +
        QuotePostgresIdentifier(stagingTableName) +
        " (" +
        QuotePostgresIdentifier(OrdinalColumnName) +
        " integer NOT NULL, LIKE " +
        targetTableSql +
        " INCLUDING DEFAULTS) ON COMMIT DROP";
  }

  internal static string CreatePostgresCopyCommandText(
      string stagingTableName,
      IReadOnlyList<string> columns) {
    ArgumentException.ThrowIfNullOrWhiteSpace(stagingTableName);
    ArgumentNullException.ThrowIfNull(columns);
    if (columns.Count == 0) {
      throw new ArgumentException("A PostgreSQL COPY command must project at least one staging column.", nameof(columns));
    }

    var builder = new StringBuilder();
    builder.Append("COPY ")
        .Append(QuotePostgresIdentifier(stagingTableName))
        .Append(" (");
    AppendQuotedColumnList(builder, columns);
    builder.Append(") FROM STDIN (FORMAT CSV, NULL '\\N')");

    return builder.ToString();
  }

  internal static string CreatePostgresStagedUniqueInsertCommandText(
      string targetTableSql,
      string stagingTableName,
      IReadOnlyList<string> columns,
      string conflictTargetColumnName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(targetTableSql);
    ArgumentException.ThrowIfNullOrWhiteSpace(stagingTableName);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentException.ThrowIfNullOrWhiteSpace(conflictTargetColumnName);
    if (columns.Count == 0) {
      throw new ArgumentException("A PostgreSQL staged unique insert must project at least one target column.", nameof(columns));
    }

    var builder = new StringBuilder();
    builder.Append("WITH ")
        .Append(QuotePostgresIdentifier("deduplicated"))
        .Append(" AS (SELECT ");
    AppendQualifiedQuotedColumnList(builder, "stage", columns);
    builder.Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuotePostgresIdentifier("stage"))
        .Append('.')
        .Append(QuotePostgresIdentifier(conflictTargetColumnName))
        .Append(" ORDER BY ")
        .Append(QuotePostgresIdentifier("stage"))
        .Append('.')
        .Append(QuotePostgresIdentifier(OrdinalColumnName))
        .Append(") AS ")
        .Append(QuotePostgresIdentifier(RowNumberColumnName))
        .Append(" FROM ")
        .Append(QuotePostgresIdentifier(stagingTableName))
        .Append(" AS ")
        .Append(QuotePostgresIdentifier("stage"))
        .Append(") INSERT INTO ")
        .Append(targetTableSql)
        .Append(" (");
    AppendQuotedColumnList(builder, columns);
    builder.Append(") SELECT ");
    AppendQualifiedQuotedColumnList(builder, "deduplicated", columns);
    builder.Append(" FROM ")
        .Append(QuotePostgresIdentifier("deduplicated"))
        .Append(" WHERE ")
        .Append(QuotePostgresIdentifier("deduplicated"))
        .Append('.')
        .Append(QuotePostgresIdentifier(RowNumberColumnName))
        .Append(" = 1 ON CONFLICT (")
        .Append(QuotePostgresIdentifier(conflictTargetColumnName))
        .Append(") DO NOTHING");

    return builder.ToString();
  }

  internal static string CreatePostgresStagedInsertCommandText(
      string targetTableSql,
      string stagingTableName,
      IReadOnlyList<string> columns) {
    ArgumentException.ThrowIfNullOrWhiteSpace(targetTableSql);
    ArgumentException.ThrowIfNullOrWhiteSpace(stagingTableName);
    ArgumentNullException.ThrowIfNull(columns);
    if (columns.Count == 0) {
      throw new ArgumentException("A PostgreSQL staged insert must project at least one target column.", nameof(columns));
    }

    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(targetTableSql)
        .Append(" (");
    AppendQuotedColumnList(builder, columns);
    builder.Append(") SELECT ");
    AppendQualifiedQuotedColumnList(builder, "stage", columns);
    builder.Append(" FROM ")
        .Append(QuotePostgresIdentifier(stagingTableName))
        .Append(" AS ")
        .Append(QuotePostgresIdentifier("stage"))
        .Append(" ORDER BY ")
        .Append(QuotePostgresIdentifier("stage"))
        .Append('.')
        .Append(QuotePostgresIdentifier(OrdinalColumnName));

    return builder.ToString();
  }

  internal static string CreatePostgresDropStagingTableCommandText(string stagingTableName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(stagingTableName);

    return "DROP TABLE IF EXISTS " + QuotePostgresIdentifier(stagingTableName);
  }

  private static void AppendQuotedColumnList(StringBuilder builder, IReadOnlyList<string> columns) {
    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuotePostgresIdentifier(columns[columnIndex]));
    }
  }

  private static void AppendQualifiedQuotedColumnList(
      StringBuilder builder,
      string tableAlias,
      IReadOnlyList<string> columns) {
    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuotePostgresIdentifier(tableAlias))
          .Append('.')
          .Append(QuotePostgresIdentifier(columns[columnIndex]));
    }
  }

  private static string CreateLatestSatelliteHashDiffsCommandText(
      DbContext dbContext,
      SatelliteTableProjection table,
      IReadOnlyList<string> parentHashKeyParameterNames) {
    var builder = new StringBuilder();
    builder.Append("SELECT DISTINCT ON (")
        .Append(QuotePostgresIdentifier(table.ParentHashKeyColumnName))
        .Append(") ")
        .Append(QuotePostgresIdentifier(table.ParentHashKeyColumnName))
        .Append(", ")
        .Append(QuotePostgresIdentifier(table.HashDiffColumnName))
        .Append(", ")
        .Append(QuotePostgresIdentifier(table.LoadTimestampColumnName))
        .Append(" FROM ")
        .Append(QuotePostgresTableIdentifier(dbContext, table.TableName))
        .Append(" WHERE ")
        .Append(QuotePostgresIdentifier(table.ParentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyParameterNames.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(parentHashKeyParameterNames[index]);
    }

    builder.Append(") ORDER BY ")
        .Append(QuotePostgresIdentifier(table.ParentHashKeyColumnName))
        .Append(", ")
        .Append(QuotePostgresIdentifier(table.LoadTimestampColumnName))
        .Append(" DESC");

    return builder.ToString();
  }

  private static IReadOnlyList<string> AddCommandParameters(
      DbCommand command,
      IEnumerable<string> values) {
    var parameterNames = new List<string>();

    foreach (var value in values) {
      var parameterName = CreatePostgresParameterName(command.Parameters.Count);
      var parameter = command.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.Value = value;
      command.Parameters.Add(parameter);
      parameterNames.Add(parameterName);
    }

    return parameterNames;
  }

  private static IReadOnlyList<string> AddHashKeyCommandParameters(
      DbCommand command,
      DbContext dbContext,
      SatelliteTableProjection table,
      IEnumerable<string> values) {
    var parameterNames = new List<string>();

    foreach (var value in values) {
      var parameterName = CreatePostgresParameterName(command.Parameters.Count);
      var parameter = command.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.Value = ToProviderHashKeyValue(
          dbContext,
          table.TableName,
          table.ParentHashKeyColumnName,
          value);
      command.Parameters.Add(parameter);
      parameterNames.Add(parameterName);
    }

    return parameterNames;
  }

  private static async Task<int> ExecutePostgresNonQueryAsync(
      DbConnection connection,
      DbTransaction transaction,
      string commandText,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = commandText;

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task DropPostgresStagingTableAsync(
      DbConnection connection,
      DbTransaction transaction,
      string stagingTableName) {
    if (connection.State != ConnectionState.Open) {
      return;
    }

    try {
      await using var command = connection.CreateCommand();
      command.Transaction = transaction;
      command.CommandText = CreatePostgresDropStagingTableCommandText(stagingTableName);

      await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
    }
    catch (DbException) {
    }
    catch (InvalidOperationException) {
    }
  }

  private static async Task WriteStagingRowsWithPostgresCopyAsync(
      DbConnection connection,
      string stagingTableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      CancellationToken cancellationToken) {
    cancellationToken.ThrowIfCancellationRequested();

    await using var writer = BeginPostgresTextImport(
        connection,
        CreatePostgresCopyCommandText(stagingTableName, columns));

    foreach (var row in rows) {
      cancellationToken.ThrowIfCancellationRequested();
      await writer.WriteLineAsync(CreatePostgresCopyCsvRow(columns, row).AsMemory(), cancellationToken).ConfigureAwait(false);
    }
  }

  private static TextWriter BeginPostgresTextImport(
      DbConnection connection,
      string copyCommandText) {
    var method = connection.GetType().GetMethod(
        "BeginTextImport",
        BindingFlags.Instance | BindingFlags.Public,
        binder: null,
        types: [typeof(string)],
        modifiers: null) ??
        throw new InvalidOperationException(
            "PostgreSQL staged Data Vault save requires an Npgsql connection that exposes BeginTextImport.");
    var writer = method.Invoke(connection, [copyCommandText]) as TextWriter;

    return writer ??
        throw new InvalidOperationException("PostgreSQL staged Data Vault save could not start text COPY.");
  }

  private static bool SupportsPostgresTextCopy(DbConnection connection) {
    return connection.GetType().GetMethod(
        "BeginTextImport",
        BindingFlags.Instance | BindingFlags.Public,
        binder: null,
        types: [typeof(string)],
        modifiers: null) is not null;
  }

  private static IReadOnlyList<Dictionary<string, object>> CreatePostgresStagingRows(
      IReadOnlyList<PostgresStagedInsertRow> rows,
      IReadOnlyList<string> columns) {
    return rows
        .Select(row => {
          var values = new Dictionary<string, object>(StringComparer.Ordinal) {
            [OrdinalColumnName] = row.Ordinal,
          };

          foreach (var column in columns) {
            values[column] = row.Values[column];
          }

          return values;
        })
        .ToArray();
  }

  private static string CreatePostgresCopyCsvRow(
      IReadOnlyList<string> columns,
      IReadOnlyDictionary<string, object> row) {
    var builder = new StringBuilder();

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(',');
      }

      AppendPostgresCopyCsvValue(builder, row[columns[columnIndex]]);
    }

    return builder.ToString();
  }

  private static void AppendPostgresCopyCsvValue(StringBuilder builder, object? value) {
    if (value is null or DBNull) {
      builder.Append(@"\N");
      return;
    }

    var formattedValue = value switch {
      byte[] bytes => "\\x" + Convert.ToHexString(bytes).ToLowerInvariant(),
      DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("O", CultureInfo.InvariantCulture),
      DateTime dateTime => dateTime.ToString("O", CultureInfo.InvariantCulture),
      IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
      _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty,
    };

    builder.Append('"');
    foreach (var character in formattedValue) {
      if (character == '"') {
        builder.Append("\"\"");
      }
      else {
        builder.Append(character);
      }
    }

    builder.Append('"');
  }

  private static string QuotePostgresTableIdentifier(DbContext dbContext, string producedTableName) {
    var entityType = FindEntityType(dbContext, producedTableName);
    var tableName = entityType?.GetTableName() ?? producedTableName;
    var schemaName = entityType?.GetSchema();

    if (string.IsNullOrWhiteSpace(schemaName)) {
      return QuotePostgresIdentifier(tableName);
    }

    return QuotePostgresIdentifier(schemaName) + "." + QuotePostgresIdentifier(tableName);
  }

  private static IEntityType? FindEntityType(DbContext dbContext, string producedTableName) {
    foreach (var entityType in dbContext.Model.GetEntityTypes()) {
      var producedName = entityType.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string;
      if (string.Equals(producedName, producedTableName, StringComparison.Ordinal)) {
        return entityType;
      }
    }

    foreach (var entityType in dbContext.Model.GetEntityTypes()) {
      if (string.Equals(entityType.GetTableName(), producedTableName, StringComparison.Ordinal) ||
          string.Equals(entityType.Name, producedTableName, StringComparison.Ordinal)) {
        return entityType;
      }
    }

    return null;
  }

  private static object CreatePostgresArrayParameterValue(
      IReadOnlyList<Dictionary<string, object>> rows,
      string columnName) {
    var firstValue = rows
        .Select(row => row[columnName])
        .FirstOrDefault(value => value is not null and not DBNull);

    return firstValue switch {
      string => rows.Select(row => (string)row[columnName]).ToArray(),
      byte[] => rows.Select(row => (byte[])row[columnName]).ToArray(),
      DateTimeOffset => rows.Select(row => (DateTimeOffset)row[columnName]).ToArray(),
      long => rows.Select(row => (long)row[columnName]).ToArray(),
      int => rows.Select(row => (int)row[columnName]).ToArray(),
      _ => rows.Select(row => row[columnName]).ToArray(),
    };
  }

  private static string GetPostgresArrayCastType(
      DbContext dbContext,
      string tableName,
      string columnName) {
    var property = FindEntityType(dbContext, tableName)
        ?.GetProperties()
        .FirstOrDefault(candidate => string.Equals(candidate.GetColumnName(), columnName, StringComparison.Ordinal));
    var valueFormat = property?.FindAnnotation(DataVaultAnnotationNames.ProviderValueFormat)?.Value;

    if (valueFormat is DataVaultProviderValueFormat.LowercaseHexBinary) {
      return "bytea";
    }

    if (valueFormat is DataVaultProviderValueFormat.UtcTicks ||
        property?.ClrType == typeof(long)) {
      return "bigint";
    }

    if (valueFormat is DataVaultProviderValueFormat.NativeInteger ||
        property?.ClrType == typeof(int)) {
      return "integer";
    }

    if (valueFormat is DataVaultProviderValueFormat.NativeDateTimeOffset ||
        property?.ClrType == typeof(DateTimeOffset)) {
      return "timestamptz";
    }

    return "text";
  }

  private static DateTimeOffset ReadDateTimeOffset(DbDataReader reader, int ordinal) {
    return DataVaultLoadTimestampValueConverter.ReadProviderValue(reader.GetValue(ordinal));
  }

  private static object ToProviderHashKeyValue(
      DbContext dbContext,
      string tableName,
      string columnName,
      string value) {
    return DataVaultHashKeyProviderValueConverter.ToProviderParameterValue(dbContext, tableName, columnName, value);
  }

  private static string ReadHashKeyProviderValue(
      DbContext dbContext,
      string tableName,
      string columnName,
      object value) {
    return (string)DataVaultHashKeyProviderValueConverter.ReadProviderValue(dbContext, tableName, columnName, value);
  }

  private static string CreatePostgresParameterName(int index) {
    return "@p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreatePostgresStagingTableName() {
    return StagingTablePrefix + Guid.NewGuid().ToString("N");
  }

  private static string QuotePostgresIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string CreateColumnSignature(IEnumerable<string> columns) {
    return string.Join('\u001f', columns);
  }

  private static PostgresSaveOperationCounts CountSaveOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var hubOperationCount = 0;
    var linkOperationCount = 0;
    var satelliteOperationCount = 0;
    foreach (var request in requests) {
      hubOperationCount += request.HubOperations.Count;
      linkOperationCount += request.LinkOperations.Count;
      satelliteOperationCount += request.SatelliteOperations.Count;
    }

    return new PostgresSaveOperationCounts(
        requests.Count,
        hubOperationCount,
        linkOperationCount,
        satelliteOperationCount);
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
      IReadOnlyList<string> ParticipantHashKeyColumnNames,
      IReadOnlyList<string> DependentChildKeyColumnNames);

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
      DataVaultSavedRecord SavedRecord,
      int Ordinal);

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

  private sealed record PostgresInsertRow(
      string TableName,
      Dictionary<string, object> Values,
      string? ConflictTargetColumnName);

  private sealed record PostgresStagedInsertRow(
      int Ordinal,
      string TableName,
      Dictionary<string, object> Values,
      string? ConflictTargetColumnName);

  private sealed record PostgresInsertRowShape(
      string TableName,
      string ColumnSignature,
      string? ConflictTargetColumnName);

  private readonly record struct PostgresSaveOperationCounts(
      int RequestCount,
      int HubOperationCount,
      int LinkOperationCount,
      int SatelliteOperationCount) {
    public int OperationCount => HubOperationCount + LinkOperationCount + SatelliteOperationCount;
  }
}
