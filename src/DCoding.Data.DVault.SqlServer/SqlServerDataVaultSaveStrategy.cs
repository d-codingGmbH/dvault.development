using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.Json;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class SqlServerDataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
  private const int SqlServerMaxCommandParameterCount = 2000;
  private const int MinimumOptimizedBatchOperationCount = 50;
  private const int MaximumOptimizedSatelliteOperationCount = 500;
  private const int SqlServerOpenJsonInsertMinimumRowCount = 32;
  private const string OrdinalColumnName = "__dvault_ordinal";
  private const string RowNumberColumnName = "__dvault_row_number";
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  internal const string SqlServerProviderName = "Microsoft.EntityFrameworkCore.SqlServer";

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return DataVaultProviderSaveStrategyGateEvaluator.EvaluateSqlServer(dbContext, requests).CanSave;
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);
    cancellationToken.ThrowIfCancellationRequested();

    var uniquePlans = CreateUniqueRowSavePlans(context);
    var satellitePlans = CreateSatelliteSavePlansForContext(context);
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
      var rowsWritten = await ExecuteSqlServerUniqueInsertRowsAsync(
          context.DbContext,
          connection,
          transaction,
          uniquePlans,
          cancellationToken).ConfigureAwait(false);

      rowsWritten += await ExecuteSqlServerInsertRowsAsync(
          context.DbContext,
          connection,
          transaction,
          filteredSatellitePlans.RowsToWrite.Select(plan => new SqlServerInsertRow(plan.Table.TableName, plan.Row)),
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

  internal static string CreateSqlServerUniqueInsertCommandText(
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

    return CreateSqlServerUniqueInsertCommandText(
        new SqlServerTableIdentifier(tableName, null),
        columns,
        hashKeyColumnName,
        rowCount);
  }

  internal static string CreateSqlServerLatestSatelliteHashDiffCommandText(
      string tableName,
      string parentHashKeyColumnName,
      string hashDiffColumnName,
      string loadTimestampColumnName,
      int rowCount) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentException.ThrowIfNullOrWhiteSpace(parentHashKeyColumnName);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashDiffColumnName);
    ArgumentException.ThrowIfNullOrWhiteSpace(loadTimestampColumnName);

    if (rowCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(rowCount));
    }

    return CreateSqlServerLatestSatelliteHashDiffCommandText(
        new SqlServerTableIdentifier(tableName, null),
        parentHashKeyColumnName,
        hashDiffColumnName,
        loadTimestampColumnName,
        rowCount);
  }

  internal static string CreateSqlServerInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      int rowCount) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);

    if (rowCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(rowCount));
    }

    return CreateSqlServerInsertCommandText(
        new SqlServerTableIdentifier(tableName, null),
        columns,
        rowCount);
  }

  internal static string CreateSqlServerJsonUniqueInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      IReadOnlyDictionary<string, string> columnTypes) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashKeyColumnName);
    ArgumentNullException.ThrowIfNull(columnTypes);

    return CreateSqlServerJsonUniqueInsertCommandText(
        new SqlServerTableIdentifier(tableName, null),
        columns,
        hashKeyColumnName,
        columnTypes);
  }

  internal static string CreateSqlServerJsonInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      IReadOnlyDictionary<string, string> columnTypes) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentNullException.ThrowIfNull(columnTypes);

    return CreateSqlServerJsonInsertCommandText(
        new SqlServerTableIdentifier(tableName, null),
        columns,
        columnTypes);
  }

  internal static bool CanSaveProvider(string? providerName, bool hasPendingTrackedChanges) {
    return string.Equals(providerName, SqlServerProviderName, StringComparison.Ordinal) &&
        !hasPendingTrackedChanges;
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

  private static bool ContainsMultiActiveSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    return requests.Any(request => request.SatelliteOperations.Any(operation => operation.Metadata.DrivingKeyNames.Count > 0));
  }

  internal static bool ShouldWriteSatelliteHashDiff(string? latestHashDiff, string candidateHashDiff) {
    ArgumentException.ThrowIfNullOrWhiteSpace(candidateHashDiff);

    return latestHashDiff is null ||
        !string.Equals(latestHashDiff, candidateHashDiff, StringComparison.Ordinal);
  }

  internal static bool ShouldAdvanceLatestSatelliteHashDiff(
      DateTimeOffset latestLoadTimestamp,
      DateTimeOffset candidateLoadTimestamp) {
    return candidateLoadTimestamp >= latestLoadTimestamp;
  }

  private static string CreateSqlServerUniqueInsertCommandText(
      SqlServerTableIdentifier table,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      int rowCount) {
    var sourceColumns = new[] { OrdinalColumnName }.Concat(columns).ToArray();
    var builder = new StringBuilder();

    builder.Append("WITH ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append(" (");
    AppendIdentifierList(builder, sourceColumns);
    builder.Append(") AS (SELECT ");
    AppendQualifiedIdentifierList(builder, "values", sourceColumns);
    builder.Append(" FROM (VALUES ");
    AppendParameterRows(builder, rowCount, sourceColumns.Length);
    builder.Append(") AS ")
        .Append(QuoteSqlServerIdentifier("values"))
        .Append(" (");
    AppendIdentifierList(builder, sourceColumns);
    builder.Append(")), ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append(" AS (SELECT ");
    AppendQualifiedIdentifierList(builder, "source", columns);
    builder.Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(hashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(OrdinalColumnName))
        .Append(") AS ")
        .Append(QuoteSqlServerIdentifier(RowNumberColumnName))
        .Append(" FROM ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append(") INSERT INTO ")
        .Append(QuoteSqlServerTable(table))
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") SELECT ");
    AppendQualifiedIdentifierList(builder, "deduplicated", columns);
    builder.Append(" FROM ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append(" WHERE ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(RowNumberColumnName))
        .Append(" = 1 AND NOT EXISTS (SELECT 1 FROM ")
        .Append(QuoteSqlServerTable(table))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append(" WITH (UPDLOCK, HOLDLOCK) WHERE ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(hashKeyColumnName))
        .Append(" = ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(hashKeyColumnName))
        .Append(')');

    return builder.ToString();
  }

  private static string CreateSqlServerLatestSatelliteHashDiffCommandText(
      SqlServerTableIdentifier table,
      string parentHashKeyColumnName,
      string hashDiffColumnName,
      string loadTimestampColumnName,
      int rowCount) {
    var builder = new StringBuilder();

    builder.Append("WITH ")
        .Append(QuoteSqlServerIdentifier("requested"))
        .Append(" (")
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(") AS (SELECT ")
        .Append(QuoteSqlServerIdentifier("values"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(" FROM (VALUES ");
    AppendParameterRows(builder, rowCount, columnCount: 1);
    builder.Append(") AS ")
        .Append(QuoteSqlServerIdentifier("values"))
        .Append(" (")
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(")), ")
        .Append(QuoteSqlServerIdentifier("ranked"))
        .Append(" AS (SELECT ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(hashDiffColumnName))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier(hashDiffColumnName))
        .Append(", ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(loadTimestampColumnName))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier(loadTimestampColumnName))
        .Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(loadTimestampColumnName))
        .Append(" DESC) AS ")
        .Append(QuoteSqlServerIdentifier(RowNumberColumnName))
        .Append(" FROM ")
        .Append(QuoteSqlServerTable(table))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append(" INNER JOIN ")
        .Append(QuoteSqlServerIdentifier("requested"))
        .Append(" ON ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(" = ")
        .Append(QuoteSqlServerIdentifier("requested"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(") SELECT ")
        .Append(QuoteSqlServerIdentifier(parentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteSqlServerIdentifier(hashDiffColumnName))
        .Append(", ")
        .Append(QuoteSqlServerIdentifier(loadTimestampColumnName))
        .Append(" FROM ")
        .Append(QuoteSqlServerIdentifier("ranked"))
        .Append(" WHERE ")
        .Append(QuoteSqlServerIdentifier(RowNumberColumnName))
        .Append(" = 1");

    return builder.ToString();
  }

  private static string CreateSqlServerInsertCommandText(
      SqlServerTableIdentifier table,
      IReadOnlyList<string> columns,
      int rowCount) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuoteSqlServerTable(table))
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") VALUES ");
    AppendParameterRows(builder, rowCount, columns.Count);

    return builder.ToString();
  }

  private static string CreateSqlServerJsonUniqueInsertCommandText(
      SqlServerTableIdentifier table,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      IReadOnlyDictionary<string, string> columnTypes) {
    var sourceColumns = new[] { OrdinalColumnName }.Concat(columns).ToArray();
    var builder = new StringBuilder();

    builder.Append("WITH ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append(" AS (SELECT ");
    AppendQualifiedIdentifierList(builder, "payload", sourceColumns);
    builder.Append(" FROM OPENJSON(@p0) WITH (");
    AppendOpenJsonColumnList(builder, sourceColumns, columnTypes);
    builder.Append(") AS ")
        .Append(QuoteSqlServerIdentifier("payload"))
        .Append("), ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append(" AS (SELECT ");
    AppendQualifiedIdentifierList(builder, "source", columns);
    builder.Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(hashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(OrdinalColumnName))
        .Append(") AS ")
        .Append(QuoteSqlServerIdentifier(RowNumberColumnName))
        .Append(" FROM ")
        .Append(QuoteSqlServerIdentifier("source"))
        .Append(") INSERT INTO ")
        .Append(QuoteSqlServerTable(table))
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") SELECT ");
    AppendQualifiedIdentifierList(builder, "deduplicated", columns);
    builder.Append(" FROM ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append(" WHERE ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(RowNumberColumnName))
        .Append(" = 1 AND NOT EXISTS (SELECT 1 FROM ")
        .Append(QuoteSqlServerTable(table))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append(" WITH (UPDLOCK, HOLDLOCK) WHERE ")
        .Append(QuoteSqlServerIdentifier("target"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(hashKeyColumnName))
        .Append(" = ")
        .Append(QuoteSqlServerIdentifier("deduplicated"))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(hashKeyColumnName))
        .Append(')');

    return builder.ToString();
  }

  private static string CreateSqlServerJsonInsertCommandText(
      SqlServerTableIdentifier table,
      IReadOnlyList<string> columns,
      IReadOnlyDictionary<string, string> columnTypes) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuoteSqlServerTable(table))
        .Append(" (");
    AppendIdentifierList(builder, columns);
    builder.Append(") SELECT ");
    AppendQualifiedIdentifierList(builder, "payload", columns);
    builder.Append(" FROM OPENJSON(@p0) WITH (");
    AppendOpenJsonColumnList(builder, columns, columnTypes);
    builder.Append(") AS ")
        .Append(QuoteSqlServerIdentifier("payload"));

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

    return plans
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  private static UniqueRowSavePlan CreateHubSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultResolvedSaveRequest request,
      DataVaultHubSaveOperation operation,
      HubProjection projection) {
    var businessKeyFields = operation.Metadata.BusinessKeyColumns
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
        new DataVaultSavedRecord(DataVaultTableKind.Hub, operation.Metadata.Name, projection.TableName, hashKey),
        Ordinal: -1);
  }

  private static UniqueRowSavePlan CreateLinkSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultResolvedSaveRequest request,
      DataVaultLinkSaveOperation operation,
      LinkProjection projection) {
    var participantNames = operation.Metadata.Participants
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
        new DataVaultSavedRecord(DataVaultTableKind.Link, operation.Metadata.Name, projection.TableName, linkHashKey),
        Ordinal: -1);
  }

  private static IReadOnlyList<SatelliteSavePlan> CreateSatelliteSavePlansForContext(DataVaultProviderSaveStrategyContext context) {
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

  private static IReadOnlyList<SatelliteSavePlan> CreateSatelliteSavePlans(IReadOnlyList<DataVaultResolvedSaveRequest> requests) {
    var satelliteProjections = new Dictionary<DataVaultSatelliteMetadata, SatelliteProjection>();

    return requests
        .SelectMany(request => request.Request.SatelliteOperations
            .Select(operation => CreateSatelliteSavePlan(
                null,
                request,
                operation,
                GetSatelliteProjection(satelliteProjections, operation.Metadata))))
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  private static SatelliteSavePlan CreateSatelliteSavePlan(
      DbContext? dbContext,
      DataVaultResolvedSaveRequest request,
      DataVaultSatelliteSaveOperation operation,
      SatelliteProjection projection) {
    var payloadFields = operation.Metadata.PayloadColumns
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.PayloadValues, column.ColumnName, nameof(operation.PayloadValues))))
        .ToArray();
    var row = new Dictionary<string, object> {
      [projection.ParentHashKeyColumnName] = operation.ParentHashKey,
      [projection.HashDiffColumnName] = operation.HashDiff,
      [projection.LoadTimestampColumnName] = dbContext is null
          ? request.LoadTimestamp.ToUniversalTime()
          : DataVaultLoadTimestampValueConverter.ToProviderValue(
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
        operation.Metadata.Name,
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
    var latestRows = new List<LatestSatelliteHashDiff>();
    var resolvedTable = ResolveTable(dbContext, table.TableName);

    foreach (var parentHashKeyBatch in parentHashKeys.Distinct(StringComparer.Ordinal).Chunk(SqlServerMaxCommandParameterCount)) {
      await using var command = connection.CreateCommand();
      command.Transaction = transaction;
      command.CommandText = CreateSqlServerLatestSatelliteHashDiffCommandText(
          resolvedTable,
          table.ParentHashKeyColumnName,
          table.HashDiffColumnName,
          table.LoadTimestampColumnName,
          parentHashKeyBatch.Length);

      for (var index = 0; index < parentHashKeyBatch.Length; index++) {
        AddParameter(command, parentHashKeyBatch[index]);
      }

      await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
      while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
        latestRows.Add(new LatestSatelliteHashDiff(
            GetRequiredString(reader, ordinal: 0),
            GetRequiredString(reader, ordinal: 1),
            GetRequiredDateTimeOffset(reader, ordinal: 2)));
      }
    }

    return latestRows.ToDictionary(row => row.ParentHashKey, StringComparer.Ordinal);
  }

  private static bool ShouldWriteSatelliteRow(
      Dictionary<string, LatestSatelliteHashDiff> latestHashDiffs,
      SatelliteSavePlan plan) {
    return !latestHashDiffs.TryGetValue(plan.ParentHashKey, out var latestHashDiff) ||
        ShouldWriteSatelliteHashDiff(latestHashDiff.HashDiff, plan.HashDiff);
  }

  private static void TrackLatestSatelliteHashDiff(
      Dictionary<string, LatestSatelliteHashDiff> latestHashDiffs,
      SatelliteSavePlan plan) {
    if (!latestHashDiffs.TryGetValue(plan.ParentHashKey, out var latestHashDiff) ||
        ShouldAdvanceLatestSatelliteHashDiff(latestHashDiff.LoadTimestamp, plan.LoadTimestamp)) {
      latestHashDiffs[plan.ParentHashKey] = new LatestSatelliteHashDiff(
          plan.ParentHashKey,
          plan.HashDiff,
          plan.LoadTimestamp);
    }
  }

  private static async Task<int> ExecuteSqlServerUniqueInsertRowsAsync(
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      IEnumerable<UniqueRowSavePlan> rows,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

    var rowsWritten = 0;
    foreach (var group in rowArray.GroupBy(row => new SqlServerUniqueInsertRowShape(
        row.Table.TableName,
        row.Table.HashKeyColumnName,
        CreateColumnSignature(row.Row.Keys)))) {
      var columns = group.First().Row.Keys.ToArray();
      var chunkSize = Math.Max(1, SqlServerMaxCommandParameterCount / (columns.Length + 1));
      var resolvedTable = ResolveTable(dbContext, group.Key.TableName);

      foreach (var chunk in group.Chunk(chunkSize)) {
        rowsWritten += await ExecuteSqlServerUniqueInsertChunkAsync(
            dbContext,
            connection,
            transaction,
            resolvedTable,
            group.Key.TableName,
            columns,
            group.Key.HashKeyColumnName,
            chunk,
            cancellationToken).ConfigureAwait(false);
      }
    }

    return rowsWritten;
  }

  private static async Task<int> ExecuteSqlServerUniqueInsertChunkAsync(
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      SqlServerTableIdentifier table,
      string producedTableName,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      IReadOnlyList<UniqueRowSavePlan> rows,
      CancellationToken cancellationToken) {
    if (rows.Count >= SqlServerOpenJsonInsertMinimumRowCount) {
      return await ExecuteSqlServerUniqueInsertOpenJsonChunkAsync(
          dbContext,
          connection,
          transaction,
          table,
          producedTableName,
          columns,
          hashKeyColumnName,
          rows,
          cancellationToken).ConfigureAwait(false);
    }

    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreateSqlServerUniqueInsertCommandText(table, columns, hashKeyColumnName, rows.Count);

    foreach (var row in rows) {
      AddParameter(command, row.Ordinal);
      foreach (var column in columns) {
        AddParameter(command, row.Row[column]);
      }
    }

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task<int> ExecuteSqlServerUniqueInsertOpenJsonChunkAsync(
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      SqlServerTableIdentifier table,
      string producedTableName,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      IReadOnlyList<UniqueRowSavePlan> rows,
      CancellationToken cancellationToken) {
    var columnTypes = CreateSqlServerOpenJsonColumnTypes(
        dbContext,
        producedTableName,
        new[] { OrdinalColumnName }.Concat(columns),
        rows.Select(row => row.Row),
        extraColumnTypes: new Dictionary<string, string>(StringComparer.Ordinal) {
          [OrdinalColumnName] = "int",
        });

    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreateSqlServerJsonUniqueInsertCommandText(table, columns, hashKeyColumnName, columnTypes);
    AddParameter(command, SerializeUniqueRowsAsJson(rows, columns), DbType.String);

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task<int> ExecuteSqlServerInsertRowsAsync(
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      IEnumerable<SqlServerInsertRow> rows,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

    var rowsWritten = 0;
    foreach (var group in rowArray.GroupBy(row => new SqlServerInsertRowShape(
        row.TableName,
        CreateColumnSignature(row.Values.Keys)))) {
      var columns = group.First().Values.Keys.ToArray();
      var chunkSize = Math.Max(1, SqlServerMaxCommandParameterCount / columns.Length);
      var resolvedTable = ResolveTable(dbContext, group.Key.TableName);

      foreach (var chunk in group.Chunk(chunkSize)) {
        rowsWritten += await ExecuteSqlServerInsertChunkAsync(
            dbContext,
            connection,
            transaction,
            resolvedTable,
            group.Key.TableName,
            columns,
            chunk.Select(row => row.Values).ToArray(),
            cancellationToken).ConfigureAwait(false);
      }
    }

    return rowsWritten;
  }

  private static async Task<int> ExecuteSqlServerInsertChunkAsync(
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      SqlServerTableIdentifier table,
      string producedTableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      CancellationToken cancellationToken) {
    if (rows.Count >= SqlServerOpenJsonInsertMinimumRowCount) {
      return await ExecuteSqlServerInsertOpenJsonChunkAsync(
          dbContext,
          connection,
          transaction,
          table,
          producedTableName,
          columns,
          rows,
          cancellationToken).ConfigureAwait(false);
    }

    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreateSqlServerInsertCommandText(table, columns, rows.Count);

    foreach (var row in rows) {
      foreach (var column in columns) {
        AddParameter(command, row[column]);
      }
    }

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task<int> ExecuteSqlServerInsertOpenJsonChunkAsync(
      DbContext dbContext,
      DbConnection connection,
      DbTransaction transaction,
      SqlServerTableIdentifier table,
      string producedTableName,
      IReadOnlyList<string> columns,
      IReadOnlyList<Dictionary<string, object>> rows,
      CancellationToken cancellationToken) {
    var columnTypes = CreateSqlServerOpenJsonColumnTypes(
        dbContext,
        producedTableName,
        columns,
        rows,
        extraColumnTypes: null);

    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = CreateSqlServerJsonInsertCommandText(table, columns, columnTypes);
    AddParameter(command, SerializeRowsAsJson(rows, columns), DbType.String);

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
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

  private static DbParameter AddParameter(DbCommand command, object value, DbType? dbType = null) {
    var parameter = command.CreateParameter();
    parameter.ParameterName = CreateSqlServerParameterName(command.Parameters.Count);
    parameter.Value = value;
    if (dbType.HasValue) {
      parameter.DbType = dbType.Value;
    }

    command.Parameters.Add(parameter);

    return parameter;
  }

  private static string GetRequiredString(DbDataReader reader, int ordinal) {
    return reader.GetValue(ordinal) as string ??
        Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("SQL Server Data Vault latest satellite lookup returned a null value.");
  }

  private static DateTimeOffset GetRequiredDateTimeOffset(DbDataReader reader, int ordinal) {
    return DataVaultLoadTimestampValueConverter.ReadProviderValue(reader.GetValue(ordinal));
  }

  private static SqlServerTableIdentifier ResolveTable(DbContext dbContext, string producedName) {
    var entityType = dbContext.Model
        .GetEntityTypes()
        .SingleOrDefault(entity =>
            string.Equals(entity.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string, producedName, StringComparison.Ordinal) ||
            string.Equals(entity.GetTableName(), producedName, StringComparison.Ordinal));

    if (entityType is null) {
      return new SqlServerTableIdentifier(producedName, null);
    }

    return new SqlServerTableIdentifier(entityType.GetTableName() ?? producedName, entityType.GetSchema());
  }

  private static IProperty? FindProperty(DbContext dbContext, string producedTableName, string columnName) {
    var entityType = dbContext.Model
        .GetEntityTypes()
        .SingleOrDefault(entity =>
            string.Equals(entity.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string, producedTableName, StringComparison.Ordinal) ||
            string.Equals(entity.GetTableName(), producedTableName, StringComparison.Ordinal) ||
            string.Equals(entity.Name, producedTableName, StringComparison.Ordinal));

    return entityType?
        .GetProperties()
        .FirstOrDefault(property => string.Equals(property.GetColumnName(), columnName, StringComparison.Ordinal));
  }

  private static IReadOnlyDictionary<string, string> CreateSqlServerOpenJsonColumnTypes(
      DbContext dbContext,
      string producedTableName,
      IEnumerable<string> columns,
      IEnumerable<Dictionary<string, object>> rows,
      IReadOnlyDictionary<string, string>? extraColumnTypes) {
    var rowArray = rows.ToArray();
    var columnTypes = new Dictionary<string, string>(StringComparer.Ordinal);

    foreach (var column in columns) {
      if (extraColumnTypes is not null && extraColumnTypes.TryGetValue(column, out var extraColumnType)) {
        columnTypes[column] = extraColumnType;
        continue;
      }

      columnTypes[column] = GetSqlServerOpenJsonColumnType(dbContext, producedTableName, column, rowArray);
    }

    return columnTypes;
  }

  private static string GetSqlServerOpenJsonColumnType(
      DbContext dbContext,
      string producedTableName,
      string columnName,
      IReadOnlyList<Dictionary<string, object>> rows) {
    var property = FindProperty(dbContext, producedTableName, columnName);
    var valueFormat = property?.FindAnnotation(DataVaultAnnotationNames.ProviderValueFormat)?.Value;
    if (valueFormat is DataVaultProviderValueFormat.UtcTicks ||
        property?.ClrType == typeof(long)) {
      return "bigint";
    }

    if (valueFormat is DataVaultProviderValueFormat.NativeInteger ||
        property?.ClrType == typeof(int)) {
      return "int";
    }

    if (valueFormat is DataVaultProviderValueFormat.NativeDateTimeOffset ||
        property?.ClrType == typeof(DateTimeOffset)) {
      return "datetimeoffset";
    }

    var storeType = property?.GetColumnType();
    if (!string.IsNullOrWhiteSpace(storeType)) {
      return storeType;
    }

    var sampleValue = rows
        .Select(row => row.TryGetValue(columnName, out var value) ? value : null)
        .FirstOrDefault(value => value is not null and not DBNull);

    return sampleValue switch {
      long => "bigint",
      int => "int",
      DateTimeOffset => "datetimeoffset",
      _ => "nvarchar(max)",
    };
  }

  private static string QuoteSqlServerTable(SqlServerTableIdentifier table) {
    if (table.SchemaName is null) {
      return QuoteSqlServerIdentifier(table.TableName);
    }

    return QuoteSqlServerIdentifier(table.SchemaName) + "." + QuoteSqlServerIdentifier(table.TableName);
  }

  private static void AppendIdentifierList(
      StringBuilder builder,
      IReadOnlyList<string> identifiers) {
    for (var index = 0; index < identifiers.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteSqlServerIdentifier(identifiers[index]));
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

      builder.Append(QuoteSqlServerIdentifier(qualifier))
          .Append('.')
          .Append(QuoteSqlServerIdentifier(identifiers[index]));
    }
  }

  private static void AppendOpenJsonColumnList(
      StringBuilder builder,
      IReadOnlyList<string> columns,
      IReadOnlyDictionary<string, string> columnTypes) {
    for (var index = 0; index < columns.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      var column = columns[index];
      builder.Append(QuoteSqlServerIdentifier(column))
          .Append(' ')
          .Append(columnTypes[column])
          .Append(' ');
      AppendSqlServerStringLiteral(builder, CreateJsonPropertyPath(column));
    }
  }

  private static void AppendParameterRows(
      StringBuilder builder,
      int rowCount,
      int columnCount) {
    var parameterIndex = 0;
    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
      if (rowIndex > 0) {
        builder.Append(", ");
      }

      builder.Append('(');
      for (var columnIndex = 0; columnIndex < columnCount; columnIndex++) {
        if (columnIndex > 0) {
          builder.Append(", ");
        }

        builder.Append(CreateSqlServerParameterName(parameterIndex));
        parameterIndex++;
      }

      builder.Append(')');
    }
  }

  private static string CreateSqlServerParameterName(int index) {
    return "@p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string QuoteSqlServerIdentifier(string identifier) {
    return "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }

  private static void AppendSqlServerStringLiteral(StringBuilder builder, string value) {
    builder.Append('\'')
        .Append(value.Replace("'", "''", StringComparison.Ordinal))
        .Append('\'');
  }

  private static string CreateJsonPropertyPath(string propertyName) {
    return "$.\"" +
        propertyName
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("\"", "\\\"", StringComparison.Ordinal) +
        "\"";
  }

  private static string SerializeUniqueRowsAsJson(
      IReadOnlyList<UniqueRowSavePlan> rows,
      IReadOnlyList<string> columns) {
    var payload = new Dictionary<string, object?>[rows.Count];

    for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++) {
      var row = rows[rowIndex];
      var item = new Dictionary<string, object?>(StringComparer.Ordinal) {
        [OrdinalColumnName] = row.Ordinal,
      };

      foreach (var column in columns) {
        item[column] = row.Row[column];
      }

      payload[rowIndex] = item;
    }

    return JsonSerializer.Serialize(payload);
  }

  private static string SerializeRowsAsJson(
      IReadOnlyList<Dictionary<string, object>> rows,
      IReadOnlyList<string> columns) {
    var payload = new Dictionary<string, object?>[rows.Count];

    for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++) {
      var row = rows[rowIndex];
      var item = new Dictionary<string, object?>(StringComparer.Ordinal);

      foreach (var column in columns) {
        item[column] = row[column];
      }

      payload[rowIndex] = item;
    }

    return JsonSerializer.Serialize(payload);
  }

  private static string CreateColumnSignature(IEnumerable<string> columns) {
    return string.Join('\u001f', columns);
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

  internal sealed record SatelliteTableProjection(
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

  private sealed record SqlServerInsertRow(string TableName, Dictionary<string, object> Values);

  private sealed record SqlServerInsertRowShape(string TableName, string ColumnSignature);

  private sealed record SqlServerUniqueInsertRowShape(
      string TableName,
      string HashKeyColumnName,
      string ColumnSignature);

  private sealed record SqlServerTableIdentifier(string TableName, string? SchemaName);
}
