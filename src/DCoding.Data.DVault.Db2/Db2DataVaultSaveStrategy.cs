using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class Db2DataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
  internal const string Db2ProviderName = "IBM.EntityFrameworkCore";

  private const int Db2MaxCommandParameterCount = 30000;
  private const int Db2MaxRowsPerCommand = 1000;
  private const int Db2LatestHashDiffBatchSize = 500;
  private const string Db2DefaultStringParameterCastType = "VARCHAR(32672)";
  private const string Db2OrdinalParameterCastType = "INTEGER";
  private const string OrdinalColumnName = "__dvault_ordinal";
  private const string RowNumberColumnName = "__dvault_row_number";
  private const string SourceTableAlias = "source";
  private const string DeduplicatedTableAlias = "dedup";
  private const string TargetTableAlias = "target";
  private const string LatestRowsTableAlias = "latest";
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return DataVaultProviderSaveStrategyGateEvaluator.EvaluateDb2(dbContext, requests).CanSave;
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
          context.DbContext,
          connection,
          transaction,
          satellitePlans,
          cancellationToken).ConfigureAwait(false);
      var savedRecords = uniquePlans
          .Select(plan => plan.SavedRecord)
          .Concat(filteredSatellitePlans.Results.Select(result => result.SavedRecord))
          .ToArray();
      var rowsWritten = await ExecuteDb2InsertRowsAsync(
          context.DbContext,
          connection,
          transaction,
          uniquePlans.Select(plan => new Db2InsertRow(
              plan.Table.TableName,
              plan.Table.HashKeyColumnName,
              plan.Row,
              Db2InsertConflictBehavior.Ignore,
              plan.Ordinal)),
          cancellationToken).ConfigureAwait(false);

      rowsWritten += await ExecuteDb2InsertRowsAsync(
          context.DbContext,
          connection,
          transaction,
          filteredSatellitePlans.RowsToWrite.Select(plan => new Db2InsertRow(
              plan.Table.TableName,
              HashKeyColumnName: null,
              plan.Row,
              Db2InsertConflictBehavior.Fail,
              plan.Ordinal)),
          cancellationToken).ConfigureAwait(false);

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

  internal static bool IsSupportedProviderName(string? providerName) {
    return string.Equals(providerName, Db2ProviderName, StringComparison.Ordinal);
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
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, projection.TableName, linkHashKey),
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
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      IReadOnlyList<SatelliteSavePlan> plans,
      CancellationToken cancellationToken) {
    var results = new SaveOperationResult[plans.Count];
    var rowsToWrite = new List<SatelliteSavePlan>();

    foreach (var group in plans.GroupBy(plan => plan.Table)) {
      var latestHashDiffs = await LoadLatestSatelliteHashDiffsAsync(
          dbContext,
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
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var latestRows = new Dictionary<string, LatestSatelliteHashDiff>(StringComparer.Ordinal);

    foreach (var parentHashKeyBatch in parentHashKeys
        .Distinct(StringComparer.Ordinal)
        .Chunk(Db2LatestHashDiffBatchSize)) {
      await using var command = connection.CreateCommand();
      command.Transaction = transaction;

      var parameterNames = AddCommandParameters(command, parentHashKeyBatch);
      command.CommandText = CreateLatestSatelliteHashDiffsCommandText(dbContext, table, parameterNames);

      await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        var parentHashKey = GetRequiredString(reader, ordinal: 0);
        latestRows[parentHashKey] = new LatestSatelliteHashDiff(
            parentHashKey,
            GetRequiredString(reader, ordinal: 1),
            DataVaultLoadTimestampValueConverter.ReadProviderValue(reader.GetValue(2)));
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

  private static async Task<int> ExecuteDb2InsertRowsAsync(
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      IEnumerable<Db2InsertRow> rows,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

    var rowsWritten = 0;
    foreach (var group in rowArray.GroupBy(row => new Db2InsertRowShape(
        row.TableName,
        row.HashKeyColumnName,
        CreateColumnSignature(row.Values.Keys),
        row.ConflictBehavior))) {
      var tableIdentifier = ResolvePhysicalTableIdentifier(dbContext, group.Key.TableName);
      var columns = group.First().Values.Keys.ToArray();
      var columnCastTypes = ResolveColumnCastTypes(dbContext, group.Key.TableName, columns);
      var parameterCountPerRow = columns.Length +
          (group.Key.ConflictBehavior == Db2InsertConflictBehavior.Ignore ? 1 : 0);
      var chunkSize = Math.Min(
          Db2MaxRowsPerCommand,
          Math.Max(1, Db2MaxCommandParameterCount / parameterCountPerRow));

      foreach (var chunk in group.Chunk(chunkSize)) {
        rowsWritten += await ExecuteDb2InsertChunkAsync(
            connection,
            transaction,
            tableIdentifier,
            columns,
            columnCastTypes,
            group.Key.HashKeyColumnName,
            chunk,
            group.Key.ConflictBehavior,
            cancellationToken).ConfigureAwait(false);
      }
    }

    return rowsWritten;
  }

  private static async Task<int> ExecuteDb2InsertChunkAsync(
      DbConnection connection,
      DbTransaction transaction,
      Db2TableIdentifier tableIdentifier,
      IReadOnlyList<string> columns,
      IReadOnlyList<string> columnCastTypes,
      string? hashKeyColumnName,
      IReadOnlyList<Db2InsertRow> rows,
      Db2InsertConflictBehavior conflictBehavior,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;

    if (conflictBehavior == Db2InsertConflictBehavior.Ignore) {
      if (hashKeyColumnName is null) {
        throw new InvalidOperationException("DB2 unique insert requires a hash key column name.");
      }

      command.CommandText = CreateDb2UniqueInsertCommandTextFromSql(
          QuoteDb2TableIdentifier(tableIdentifier),
          columns,
          columnCastTypes,
          hashKeyColumnName,
          rows.Count);
      foreach (var row in rows) {
        AddParameter(command, row.Ordinal);
        foreach (var column in columns) {
          AddParameter(command, row.Values[column]);
        }
      }
    }
    else {
      command.CommandText = CreateDb2InsertCommandTextFromSql(
          QuoteDb2TableIdentifier(tableIdentifier),
          columns,
          columnCastTypes,
          rows.Count);
      foreach (var row in rows) {
        foreach (var column in columns) {
          AddParameter(command, row.Values[column]);
        }
      }
    }

    var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

    return affectedRows >= 0 ? affectedRows : rows.Count;
  }

  internal static string CreateDb2InsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      int rowCount) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);
    if (rowCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(rowCount));
    }

    return CreateDb2InsertCommandTextFromSql(
        QuoteDb2Identifier(tableName),
        columns,
        CreateDefaultColumnCastTypes(columns),
        rowCount);
  }

  private static string CreateDb2InsertCommandTextFromSql(
      string tableSql,
      IReadOnlyList<string> columns,
      IReadOnlyList<string> columnCastTypes,
      int rowCount) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(tableSql)
        .Append(" (");
    AppendIdentifierList(builder, columns);
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

        AppendDb2ParameterCast(builder, parameterIndex, columnCastTypes[columnIndex]);
        parameterIndex++;
      }

      builder.Append(')');
    }

    return builder.ToString();
  }

  internal static string CreateDb2UniqueInsertCommandText(
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

    return CreateDb2UniqueInsertCommandTextFromSql(
        QuoteDb2Identifier(tableName),
        columns,
        CreateDefaultColumnCastTypes(columns),
        hashKeyColumnName,
        rowCount);
  }

  private static string CreateDb2UniqueInsertCommandTextFromSql(
      string tableSql,
      IReadOnlyList<string> columns,
      IReadOnlyList<string> columnCastTypes,
      string hashKeyColumnName,
      int rowCount) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(tableSql)
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") SELECT ");
    AppendQualifiedIdentifierList(builder, DeduplicatedTableAlias, columns);
    builder.Append(" FROM (SELECT ");
    AppendQualifiedIdentifierList(builder, SourceTableAlias, columns);
    builder.Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteDb2Identifier(SourceTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(hashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteDb2Identifier(SourceTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(OrdinalColumnName))
        .Append(") AS ")
        .Append(QuoteDb2Identifier(RowNumberColumnName))
        .Append(" FROM (VALUES ");
    AppendDb2ValueRows(builder, columns, columnCastTypes, rowCount, includeOrdinal: true);
    builder.Append(") AS ")
        .Append(QuoteDb2Identifier(SourceTableAlias))
        .Append(" (")
        .Append(QuoteDb2Identifier(OrdinalColumnName));

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      builder.Append(", ")
          .Append(QuoteDb2Identifier(columns[columnIndex]));
    }

    builder.Append(")) AS ")
        .Append(QuoteDb2Identifier(DeduplicatedTableAlias))
        .Append(" WHERE ")
        .Append(QuoteDb2Identifier(DeduplicatedTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(RowNumberColumnName))
        .Append(" = 1 AND NOT EXISTS (SELECT 1 FROM ")
        .Append(tableSql)
        .Append(" AS ")
        .Append(QuoteDb2Identifier(TargetTableAlias))
        .Append(" WHERE ")
        .Append(QuoteDb2Identifier(TargetTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(hashKeyColumnName))
        .Append(" = ")
        .Append(QuoteDb2Identifier(DeduplicatedTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(hashKeyColumnName))
        .Append(')');

    return builder.ToString();
  }

  internal static string CreateLatestSatelliteHashDiffsCommandText(
      string tableName,
      string parentHashKeyColumnName,
      string hashDiffColumnName,
      string loadTimestampColumnName,
      IReadOnlyList<string> parentHashKeyParameterNames) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentException.ThrowIfNullOrWhiteSpace(parentHashKeyColumnName);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashDiffColumnName);
    ArgumentException.ThrowIfNullOrWhiteSpace(loadTimestampColumnName);
    ArgumentNullException.ThrowIfNull(parentHashKeyParameterNames);

    return CreateLatestSatelliteHashDiffsCommandTextFromSql(
        QuoteDb2Identifier(tableName),
        parentHashKeyColumnName,
        hashDiffColumnName,
        loadTimestampColumnName,
        parentHashKeyParameterNames);
  }

  private static string CreateLatestSatelliteHashDiffsCommandText(
      DbContext dbContext,
      SatelliteTableProjection table,
      IReadOnlyList<string> parentHashKeyParameterNames) {
    return CreateLatestSatelliteHashDiffsCommandTextFromSql(
        QuoteDb2TableIdentifier(ResolvePhysicalTableIdentifier(dbContext, table.TableName)),
        table.ParentHashKeyColumnName,
        table.HashDiffColumnName,
        table.LoadTimestampColumnName,
        parentHashKeyParameterNames);
  }

  private static string CreateLatestSatelliteHashDiffsCommandTextFromSql(
      string tableSql,
      string parentHashKeyColumnName,
      string hashDiffColumnName,
      string loadTimestampColumnName,
      IReadOnlyList<string> parentHashKeyParameterNames) {
    var builder = new StringBuilder();
    builder.Append("SELECT ")
        .Append(QuoteDb2Identifier(LatestRowsTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(parentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteDb2Identifier(LatestRowsTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(hashDiffColumnName))
        .Append(", ")
        .Append(QuoteDb2Identifier(LatestRowsTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(loadTimestampColumnName))
        .Append(" FROM (SELECT ")
        .Append(QuoteDb2Identifier(parentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteDb2Identifier(hashDiffColumnName))
        .Append(", ")
        .Append(QuoteDb2Identifier(loadTimestampColumnName))
        .Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteDb2Identifier(parentHashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteDb2Identifier(loadTimestampColumnName))
        .Append(" DESC) AS ")
        .Append(QuoteDb2Identifier(RowNumberColumnName))
        .Append(" FROM ")
        .Append(tableSql)
        .Append(" WHERE ")
        .Append(QuoteDb2Identifier(parentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyParameterNames.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(parentHashKeyParameterNames[index]);
    }

    builder.Append(")) AS ")
        .Append(QuoteDb2Identifier(LatestRowsTableAlias))
        .Append(" WHERE ")
        .Append(QuoteDb2Identifier(LatestRowsTableAlias))
        .Append('.')
        .Append(QuoteDb2Identifier(RowNumberColumnName))
        .Append(" = 1");

    return builder.ToString();
  }

  private static IReadOnlyList<string> AddCommandParameters(
      DbCommand command,
      IEnumerable<string> values) {
    var parameterNames = new List<string>();

    foreach (var value in values) {
      var parameterName = CreateDb2ParameterName(command.Parameters.Count);
      AddParameter(command, value);
      parameterNames.Add(parameterName);
    }

    return parameterNames;
  }

  private static void AddParameter(DbCommand command, object value) {
    var parameter = command.CreateParameter();
    parameter.ParameterName = CreateDb2ParameterName(command.Parameters.Count);
    parameter.Value = value;
    parameter.DbType = value switch {
      int => DbType.Int32,
      long => DbType.Int64,
      DateTime => DbType.DateTime,
      DateTimeOffset => DbType.DateTimeOffset,
      _ => DbType.String,
    };
    command.Parameters.Add(parameter);
  }

  private static void AppendDb2ValueRows(
      StringBuilder builder,
      IReadOnlyList<string> columns,
      IReadOnlyList<string> columnCastTypes,
      int rowCount,
      bool includeOrdinal) {
    var parameterIndex = 0;
    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
      if (rowIndex > 0) {
        builder.Append(", ");
      }

      builder.Append('(');
      if (includeOrdinal) {
        AppendDb2ParameterCast(builder, parameterIndex, Db2OrdinalParameterCastType);
        parameterIndex++;
      }

      for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
        if (columnIndex > 0 || includeOrdinal) {
          builder.Append(", ");
        }

        AppendDb2ParameterCast(builder, parameterIndex, columnCastTypes[columnIndex]);
        parameterIndex++;
      }

      builder.Append(')');
    }
  }

  private static IReadOnlyList<string> CreateDefaultColumnCastTypes(IReadOnlyList<string> columns) {
    return columns.Select(_ => Db2DefaultStringParameterCastType).ToArray();
  }

  private static IReadOnlyList<string> ResolveColumnCastTypes(
      DbContext dbContext,
      string producedTableName,
      IReadOnlyList<string> columns) {
    var entityType = FindEntityType(dbContext, producedTableName);
    if (entityType is null) {
      return CreateDefaultColumnCastTypes(columns);
    }

    return columns
        .Select(column => ResolveColumnCastType(entityType, column))
        .ToArray();
  }

  private static string ResolveColumnCastType(IEntityType entityType, string columnName) {
    var property = entityType
        .GetProperties()
        .FirstOrDefault(candidate =>
            string.Equals(candidate.GetColumnName(), columnName, StringComparison.Ordinal) ||
            string.Equals(candidate.Name, columnName, StringComparison.Ordinal));

    return string.IsNullOrWhiteSpace(property?.GetColumnType())
        ? Db2DefaultStringParameterCastType
        : property.GetColumnType()!;
  }

  private static void AppendDb2ParameterCast(
      StringBuilder builder,
      int parameterIndex,
      string storeType) {
    builder.Append("CAST(")
        .Append(CreateDb2ParameterName(parameterIndex))
        .Append(" AS ")
        .Append(string.IsNullOrWhiteSpace(storeType) ? Db2DefaultStringParameterCastType : storeType)
        .Append(')');
  }

  private static Db2TableIdentifier ResolvePhysicalTableIdentifier(DbContext dbContext, string producedTableName) {
    var entityType = FindEntityType(dbContext, producedTableName);

    return new Db2TableIdentifier(
        entityType?.GetTableName() ?? producedTableName,
        entityType?.GetSchema());
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

  private static string QuoteDb2TableIdentifier(Db2TableIdentifier identifier) {
    return string.IsNullOrWhiteSpace(identifier.SchemaName)
        ? QuoteDb2Identifier(identifier.TableName)
        : QuoteDb2Identifier(identifier.SchemaName!) + "." + QuoteDb2Identifier(identifier.TableName);
  }

  private static string GetRequiredString(DbDataReader reader, int ordinal) {
    return reader.GetValue(ordinal) as string ??
        Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("DB2 Data Vault latest satellite lookup returned a null value.");
  }

  private static string CreateDb2ParameterName(int index) {
    return "@p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string QuoteDb2Identifier(string identifier) {
    var normalizedIdentifier = identifier.ToUpperInvariant();

    return "\"" + normalizedIdentifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static void AppendIdentifierList(
      StringBuilder builder,
      IReadOnlyList<string> identifiers) {
    for (var index = 0; index < identifiers.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteDb2Identifier(identifiers[index]));
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

      builder.Append(QuoteDb2Identifier(qualifier))
          .Append('.')
          .Append(QuoteDb2Identifier(identifiers[index]));
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

  private sealed record Db2InsertRow(
      string TableName,
      string? HashKeyColumnName,
      Dictionary<string, object> Values,
      Db2InsertConflictBehavior ConflictBehavior,
      int Ordinal);

  private sealed record Db2InsertRowShape(
      string TableName,
      string? HashKeyColumnName,
      string ColumnSignature,
      Db2InsertConflictBehavior ConflictBehavior);

  private readonly record struct Db2TableIdentifier(string TableName, string? SchemaName);

  private enum Db2InsertConflictBehavior {
    Fail,
    Ignore,
  }
}
