using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines the explicit DVault v1 write boundary used by callers instead of SaveChanges interception.
/// </summary>
public interface IDataVaultSaveService {
  /// <summary>
  /// Persists the requested Data Vault hub, link, and satellite rows through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The explicit save request containing write metadata and row operations.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists multiple explicit Data Vault save requests as one ordered batch through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The explicit bulk save request containing the ordered write requests.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultBulkSaveRequest request,
      CancellationToken cancellationToken = default);
}

/// <summary>
/// Groups explicit DVault save operations that share one load timestamp and record source.
/// </summary>
public sealed class DataVaultSaveRequest {
  /// <summary>
  /// Initializes a new explicit save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows to persist before link and satellite rows.</param>
  /// <param name="linkOperations">The link rows to persist after hub rows and before satellite rows.</param>
  public DataVaultSaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultHubSaveOperation> hubOperations,
      IEnumerable<DataVaultLinkSaveOperation> linkOperations)
      : this(loadTimestamp, recordSource, hubOperations, linkOperations, []) {
  }

  /// <summary>
  /// Initializes a new explicit save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows to persist before link and satellite rows.</param>
  /// <param name="linkOperations">The link rows to persist after hub rows and before satellite rows.</param>
  /// <param name="satelliteOperations">The satellite rows to persist after hub and link rows.</param>
  public DataVaultSaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultHubSaveOperation> hubOperations,
      IEnumerable<DataVaultLinkSaveOperation> linkOperations,
      IEnumerable<DataVaultSatelliteSaveOperation> satelliteOperations) {
    ArgumentException.ThrowIfNullOrWhiteSpace(recordSource);
    ArgumentNullException.ThrowIfNull(hubOperations);
    ArgumentNullException.ThrowIfNull(linkOperations);
    ArgumentNullException.ThrowIfNull(satelliteOperations);

    LoadTimestamp = loadTimestamp.ToUniversalTime();
    RecordSource = recordSource;
    HubOperations = RequireOperations(hubOperations, nameof(hubOperations));
    LinkOperations = RequireOperations(linkOperations, nameof(linkOperations));
    SatelliteOperations = RequireOperations(satelliteOperations, nameof(satelliteOperations));
  }

  /// <summary>
  /// Gets the caller-supplied load timestamp normalized to a UTC instant.
  /// </summary>
  public DateTimeOffset LoadTimestamp { get; }

  /// <summary>
  /// Gets the caller-supplied record source used for every operation in the request.
  /// </summary>
  public string RecordSource { get; }

  /// <summary>
  /// Gets the hub rows to persist before link and satellite rows.
  /// </summary>
  public IReadOnlyList<DataVaultHubSaveOperation> HubOperations { get; }

  /// <summary>
  /// Gets the link rows to persist after hub rows and before satellite rows.
  /// </summary>
  public IReadOnlyList<DataVaultLinkSaveOperation> LinkOperations { get; }

  /// <summary>
  /// Gets the satellite rows to persist after hub and link rows.
  /// </summary>
  public IReadOnlyList<DataVaultSatelliteSaveOperation> SatelliteOperations { get; }

  private static IReadOnlyList<T> RequireOperations<T>(IEnumerable<T> operations, string parameterName)
      where T : class {
    var values = operations.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault save operation collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}

/// <summary>
/// Groups multiple explicit DVault save requests that should be processed as one ordered batch.
/// </summary>
public sealed class DataVaultBulkSaveRequest {
  /// <summary>
  /// Initializes a new explicit bulk save request.
  /// </summary>
  /// <param name="requests">The save requests to process in caller-supplied order.</param>
  public DataVaultBulkSaveRequest(IEnumerable<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    Requests = RequireRequests(requests, nameof(requests));
  }

  /// <summary>
  /// Gets the save requests processed in caller-supplied order.
  /// </summary>
  public IReadOnlyList<DataVaultSaveRequest> Requests { get; }

  private static IReadOnlyList<DataVaultSaveRequest> RequireRequests(
      IEnumerable<DataVaultSaveRequest> requests,
      string parameterName) {
    var values = requests.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault bulk save request collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}

/// <summary>
/// Describes one hub row to persist through the explicit DVault save service.
/// </summary>
public sealed class DataVaultHubSaveOperation {
  /// <summary>
  /// Initializes a new hub save operation.
  /// </summary>
  /// <param name="metadata">The hub metadata declaration that owns the target table and business-key shape.</param>
  /// <param name="businessKeyValues">Business-key values keyed by the hub metadata business-key names.</param>
  public DataVaultHubSaveOperation(
      DataVaultHubMetadata metadata,
      IEnumerable<KeyValuePair<string, string>> businessKeyValues) {
    ArgumentNullException.ThrowIfNull(metadata);

    Metadata = metadata;
    BusinessKeyValues = RequireValues(businessKeyValues, nameof(businessKeyValues));
  }

  /// <summary>
  /// Gets the hub metadata declaration that owns the target table and business-key shape.
  /// </summary>
  public DataVaultHubMetadata Metadata { get; }

  /// <summary>
  /// Gets business-key values keyed by the hub metadata business-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> BusinessKeyValues { get; }

  internal static IReadOnlyDictionary<string, string> RequireValues(
      IEnumerable<KeyValuePair<string, string>> values,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(values);

    var valueMap = new Dictionary<string, string>(StringComparer.Ordinal);
    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value.Key, parameterName);
      if (value.Value is null) {
        throw new ArgumentException("Data Vault save values must not contain null values.", parameterName);
      }

      if (!valueMap.TryAdd(value.Key, value.Value)) {
        throw new ArgumentException("Data Vault save values must not contain duplicate names.", parameterName);
      }
    }

    return new ReadOnlyDictionary<string, string>(valueMap);
  }
}

/// <summary>
/// Describes one link row to persist through the explicit DVault save service.
/// </summary>
public sealed class DataVaultLinkSaveOperation {
  /// <summary>
  /// Initializes a new link save operation.
  /// </summary>
  /// <param name="metadata">The link metadata declaration that owns the target table and participant shape.</param>
  /// <param name="participantHashKeyValues">Participant hash keys keyed by the participant hub metadata names.</param>
  public DataVaultLinkSaveOperation(
      DataVaultLinkMetadata metadata,
      IEnumerable<KeyValuePair<string, string>> participantHashKeyValues) {
    ArgumentNullException.ThrowIfNull(metadata);

    Metadata = metadata;
    ParticipantHashKeyValues = DataVaultHubSaveOperation.RequireValues(
        participantHashKeyValues,
        nameof(participantHashKeyValues));
  }

  /// <summary>
  /// Gets the link metadata declaration that owns the target table and participant shape.
  /// </summary>
  public DataVaultLinkMetadata Metadata { get; }

  /// <summary>
  /// Gets participant hash keys keyed by the participant hub metadata names.
  /// </summary>
  public IReadOnlyDictionary<string, string> ParticipantHashKeyValues { get; }
}

/// <summary>
/// Describes one satellite row to persist through the explicit DVault save service.
/// </summary>
public sealed class DataVaultSatelliteSaveOperation {
  /// <summary>
  /// Initializes a new satellite save operation.
  /// </summary>
  /// <param name="metadata">The satellite metadata declaration that owns the target table and payload shape.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="payloadValues">Payload values keyed by the satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultSatelliteSaveOperation(
      DataVaultSatelliteMetadata metadata,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff) {
    ArgumentNullException.ThrowIfNull(metadata);
    ArgumentException.ThrowIfNullOrWhiteSpace(parentHashKey);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashDiff);

    Metadata = metadata;
    ParentHashKey = parentHashKey;
    PayloadValues = DataVaultHubSaveOperation.RequireValues(payloadValues, nameof(payloadValues));
    HashDiff = hashDiff;
  }

  /// <summary>
  /// Gets the satellite metadata declaration that owns the target table and payload shape.
  /// </summary>
  public DataVaultSatelliteMetadata Metadata { get; }

  /// <summary>
  /// Gets the explicit parent hub or link hash key associated with this satellite row.
  /// </summary>
  public string ParentHashKey { get; }

  /// <summary>
  /// Gets payload values keyed by the satellite metadata payload names.
  /// </summary>
  public IReadOnlyDictionary<string, string> PayloadValues { get; }

  /// <summary>
  /// Gets the caller-supplied deterministic hash diff for this payload state.
  /// </summary>
  public string HashDiff { get; }
}

/// <summary>
/// Summarizes rows persisted by an explicit DVault save request.
/// </summary>
public sealed class DataVaultSaveResult {
  /// <summary>
  /// Initializes a new save result.
  /// </summary>
  /// <param name="rowsWritten">The row count inserted by the explicit service invocation.</param>
  /// <param name="savedRecords">The generated hub, link, and satellite hash-key summaries.</param>
  public DataVaultSaveResult(int rowsWritten, IEnumerable<DataVaultSavedRecord> savedRecords) {
    ArgumentNullException.ThrowIfNull(savedRecords);

    RowsWritten = rowsWritten;
    SavedRecords = savedRecords.ToArray();
  }

  /// <summary>
  /// Gets the row count inserted by the explicit service invocation.
  /// </summary>
  public int RowsWritten { get; }

  /// <summary>
  /// Gets the generated hub, link, and satellite hash-key summaries.
  /// </summary>
  public IReadOnlyList<DataVaultSavedRecord> SavedRecords { get; }
}

/// <summary>
/// Summarizes one hub, link, or satellite row persisted by an explicit DVault save request.
/// </summary>
public sealed class DataVaultSavedRecord {
  /// <summary>
  /// Initializes a new saved row summary.
  /// </summary>
  /// <param name="kind">Whether the saved row is a hub, link, or satellite.</param>
  /// <param name="metadataName">The metadata declaration name that produced the row.</param>
  /// <param name="tableName">The produced table name that received the row.</param>
  /// <param name="hashKey">The generated Data Vault hash key persisted for the row, or parent hash key for satellites.</param>
  public DataVaultSavedRecord(DataVaultTableKind kind, string metadataName, string tableName, string hashKey) {
    ArgumentException.ThrowIfNullOrWhiteSpace(metadataName);
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashKey);

    Kind = kind;
    MetadataName = metadataName;
    TableName = tableName;
    HashKey = hashKey;
  }

  /// <summary>
  /// Gets whether the saved row is a hub, link, or satellite.
  /// </summary>
  public DataVaultTableKind Kind { get; }

  /// <summary>
  /// Gets the metadata declaration name that produced the row.
  /// </summary>
  public string MetadataName { get; }

  /// <summary>
  /// Gets the produced table name that received the row.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets the generated Data Vault hash key persisted for the row, or parent hash key for satellites.
  /// </summary>
  public string HashKey { get; }
}

internal sealed class DefaultDataVaultSaveService : IDataVaultSaveService {
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;
  private const int SqliteMaxCommandParameterCount = 900;

  private readonly IStableHashService _stableHashService;
  private readonly IStableHashNormalizer _stableHashNormalizer;

  public DefaultDataVaultSaveService(IStableHashService stableHashService, IStableHashNormalizer stableHashNormalizer) {
    ArgumentNullException.ThrowIfNull(stableHashService);
    ArgumentNullException.ThrowIfNull(stableHashNormalizer);

    _stableHashService = stableHashService;
    _stableHashNormalizer = stableHashNormalizer;
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return await SaveRequestsAsync(dbContext, [request], cancellationToken).ConfigureAwait(false);
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultBulkSaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return await SaveRequestsAsync(dbContext, request.Requests, cancellationToken).ConfigureAwait(false);
  }

  private async Task<DataVaultSaveResult> SaveRequestsAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      CancellationToken cancellationToken) {
    if (CanUseSqliteSetBasedSave(dbContext)) {
      return await SaveRequestsWithSqliteSetBasedAsync(dbContext, requests, cancellationToken)
          .ConfigureAwait(false);
    }

    var savedRecords = new List<DataVaultSavedRecord>();
    var rowsWritten = 0;

    foreach (var request in requests) {
      foreach (var operation in request.HubOperations) {
        var result = await AddHubAsync(dbContext, request, operation, cancellationToken).ConfigureAwait(false);
        savedRecords.Add(result.SavedRecord);
        if (result.RowWritten) {
          rowsWritten++;
        }
      }

      foreach (var operation in request.LinkOperations) {
        var result = await AddLinkAsync(dbContext, request, operation, cancellationToken).ConfigureAwait(false);
        savedRecords.Add(result.SavedRecord);
        if (result.RowWritten) {
          rowsWritten++;
        }
      }
    }

    var satelliteResults = await AddSatellitesAsync(dbContext, requests, cancellationToken).ConfigureAwait(false);
    foreach (var result in satelliteResults) {
      savedRecords.Add(result.SavedRecord);
      if (result.RowWritten) {
        rowsWritten++;
      }
    }

    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    return new DataVaultSaveResult(rowsWritten, savedRecords);
  }

  private async Task<DataVaultSaveResult> SaveRequestsWithSqliteSetBasedAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      CancellationToken cancellationToken) {
    var uniquePlans = CreateUniqueRowSavePlans(requests);
    var satellitePlans = CreateSatelliteSavePlans(requests);
    var filteredSatellitePlans = await FilterSatellitePlansAsync(dbContext, satellitePlans, cancellationToken)
        .ConfigureAwait(false);
    var savedRecords = uniquePlans
        .Select(plan => plan.SavedRecord)
        .Concat(filteredSatellitePlans.Results.Select(result => result.SavedRecord))
        .ToArray();
    var rowsWritten = await ExecuteSqliteInsertRowsAsync(
        dbContext,
        uniquePlans.Select(plan => new SqliteInsertRow(plan.Table.TableName, plan.Row)),
        SqliteInsertConflictBehavior.Ignore,
        cancellationToken).ConfigureAwait(false);

    rowsWritten += await ExecuteSqliteInsertRowsAsync(
        dbContext,
        filteredSatellitePlans.RowsToWrite.Select(plan => new SqliteInsertRow(plan.Table.TableName, plan.Row)),
        SqliteInsertConflictBehavior.Fail,
        cancellationToken).ConfigureAwait(false);

    return new DataVaultSaveResult(rowsWritten, savedRecords);
  }

  private IReadOnlyList<UniqueRowSavePlan> CreateUniqueRowSavePlans(IReadOnlyList<DataVaultSaveRequest> requests) {
    var plans = new List<UniqueRowSavePlan>();

    foreach (var request in requests) {
      plans.AddRange(request.HubOperations.Select(operation => CreateHubSavePlan(request, operation)));
      plans.AddRange(request.LinkOperations.Select(operation => CreateLinkSavePlan(request, operation)));
    }

    return plans
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  private static bool CanUseSqliteSetBasedSave(DbContext dbContext) {
    return string.Equals(dbContext.Database.ProviderName, "Microsoft.EntityFrameworkCore.Sqlite", StringComparison.Ordinal) &&
        !dbContext.ChangeTracker
            .Entries()
            .Any(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
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

  private async Task<SaveOperationResult> AddHubAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      DataVaultHubSaveOperation operation,
      CancellationToken cancellationToken) {
    var plan = CreateHubSavePlan(request, operation);
    var rowWritten = await AddRowIfMissingAsync(
        dbContext,
        plan.Table.TableName,
        plan.Table.HashKeyColumnName,
        plan.HashKey,
        plan.Row,
        cancellationToken).ConfigureAwait(false);

    return new SaveOperationResult(plan.SavedRecord, rowWritten);
  }

  private UniqueRowSavePlan CreateHubSavePlan(
      DataVaultSaveRequest request,
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
    var hashKey = ComputeHash(businessKeyFields);
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
        new DataVaultSavedRecord(DataVaultTableKind.Hub, hub.Name, tableName, hashKey),
        Ordinal: -1);
  }

  private async Task<SaveOperationResult> AddLinkAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      DataVaultLinkSaveOperation operation,
      CancellationToken cancellationToken) {
    var plan = CreateLinkSavePlan(request, operation);
    var rowWritten = await AddRowIfMissingAsync(
        dbContext,
        plan.Table.TableName,
        plan.Table.HashKeyColumnName,
        plan.HashKey,
        plan.Row,
        cancellationToken).ConfigureAwait(false);

    return new SaveOperationResult(plan.SavedRecord, rowWritten);
  }

  private UniqueRowSavePlan CreateLinkSavePlan(
      DataVaultSaveRequest request,
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
    var linkHashKey = ComputeHash(participantHashKeyFields);
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
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, tableName, linkHashKey),
        Ordinal: -1);
  }

  private static async Task<bool> AddRowIfMissingAsync(
      DbContext dbContext,
      string tableName,
      string hashKeyColumnName,
      string hashKey,
      Dictionary<string, object> row,
      CancellationToken cancellationToken) {
    var rows = dbContext.Set<Dictionary<string, object>>(tableName);

    if (GetTrackedRows(dbContext, tableName)
        .Any(existingRow => HasHashKey(existingRow, hashKeyColumnName, hashKey))) {
      return false;
    }

    var exists = await rows
        .AsNoTracking()
        .AnyAsync(existingRow => EF.Property<string>(existingRow, hashKeyColumnName) == hashKey, cancellationToken)
        .ConfigureAwait(false);

    if (exists) {
      return false;
    }

    rows.Add(row);

    return true;
  }

  private async Task<IReadOnlyList<SaveOperationResult>> AddSatellitesAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      CancellationToken cancellationToken) {
    var plans = CreateSatelliteSavePlans(requests);
    var filteredPlans = await FilterSatellitePlansAsync(dbContext, plans, cancellationToken).ConfigureAwait(false);

    foreach (var group in filteredPlans.RowsToWrite.GroupBy(plan => plan.Table)) {
      var rows = dbContext.Set<Dictionary<string, object>>(group.Key.TableName);
      foreach (var plan in group) {
        rows.Add(plan.Row);
      }
    }

    return filteredPlans.Results;
  }

  private static IReadOnlyList<SatelliteSavePlan> CreateSatelliteSavePlans(IReadOnlyList<DataVaultSaveRequest> requests) {
    return requests
        .SelectMany(request => request.SatelliteOperations
            .Select(operation => CreateSatelliteSavePlan(request, operation)))
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  private static async Task<FilteredSatelliteSavePlans> FilterSatellitePlansAsync(
      DbContext dbContext,
      IReadOnlyList<SatelliteSavePlan> plans,
      CancellationToken cancellationToken) {
    var results = new SaveOperationResult[plans.Length];
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

  private static SatelliteSavePlan CreateSatelliteSavePlan(
      DataVaultSaveRequest request,
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

  private static async Task<Dictionary<string, LatestSatelliteHashDiff>> LoadLatestSatelliteHashDiffsAsync(
      DbContext dbContext,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var parentHashKeyArray = parentHashKeys.Distinct(StringComparer.Ordinal).ToArray();
    var latestByParent = GetLatestTrackedSatelliteHashDiffs(dbContext, table, parentHashKeyArray);
    var persistedHashDiffs = await LoadLatestPersistedSatelliteHashDiffsAsync(
        dbContext,
        table,
        parentHashKeyArray,
        cancellationToken).ConfigureAwait(false);

    foreach (var persistedHashDiff in persistedHashDiffs) {
      if (!latestByParent.TryGetValue(persistedHashDiff.ParentHashKey, out var current) ||
          persistedHashDiff.LoadTimestamp > current.LoadTimestamp) {
        latestByParent[persistedHashDiff.ParentHashKey] = persistedHashDiff;
      }
    }

    return latestByParent;
  }

  private static Dictionary<string, LatestSatelliteHashDiff> GetLatestTrackedSatelliteHashDiffs(
      DbContext dbContext,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys) {
    var parentKeySet = parentHashKeys.ToHashSet(StringComparer.Ordinal);
    var latestByParent = new Dictionary<string, LatestSatelliteHashDiff>(StringComparer.Ordinal);

    foreach (var trackedRow in GetTrackedRows(dbContext, table.TableName)) {
      if (!TryCreateLatestSatelliteHashDiff(trackedRow, table, out var current) ||
          !parentKeySet.Contains(current.ParentHashKey)) {
        continue;
      }

      if (!latestByParent.TryGetValue(current.ParentHashKey, out var previous) ||
          current.LoadTimestamp > previous.LoadTimestamp) {
        latestByParent[current.ParentHashKey] = current;
      }
    }

    return latestByParent;
  }

  private static async Task<IReadOnlyList<LatestSatelliteHashDiff>> LoadLatestPersistedSatelliteHashDiffsAsync(
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

      var batchLatestRows = batchRows
          .GroupBy(row => row.ParentHashKey, StringComparer.Ordinal)
          .Select(group => group.OrderByDescending(row => row.LoadTimestamp).First());

      latestRows.AddRange(batchLatestRows);
    }

    return latestRows;
  }

  private static bool AddSatelliteRowIfChanged(
      DbSet<Dictionary<string, object>> rows,
      Dictionary<string, LatestSatelliteHashDiff> latestHashDiffs,
      SatelliteSavePlan plan) {
    if (latestHashDiffs.TryGetValue(plan.ParentHashKey, out var latestHashDiff) &&
        string.Equals(latestHashDiff.HashDiff, plan.HashDiff, StringComparison.Ordinal)) {
      return false;
    }

    rows.Add(plan.Row);
    if (!latestHashDiffs.TryGetValue(plan.ParentHashKey, out latestHashDiff) ||
        plan.LoadTimestamp >= latestHashDiff.LoadTimestamp) {
      latestHashDiffs[plan.ParentHashKey] = new LatestSatelliteHashDiff(
          plan.ParentHashKey,
          plan.HashDiff,
          plan.LoadTimestamp);
    }

    return true;
  }

  private static bool TryCreateLatestSatelliteHashDiff(
      Dictionary<string, object> row,
      SatelliteTableProjection table,
      out LatestSatelliteHashDiff latestHashDiff) {
    if (row.TryGetValue(table.ParentHashKeyColumnName, out var parentHashKeyValue) &&
        row.TryGetValue(table.HashDiffColumnName, out var hashDiffValue) &&
        row.TryGetValue(table.LoadTimestampColumnName, out var loadTimestampValue) &&
        parentHashKeyValue is string parentHashKey &&
        hashDiffValue is string hashDiff &&
        loadTimestampValue is DateTimeOffset loadTimestamp) {
      latestHashDiff = new LatestSatelliteHashDiff(parentHashKey, hashDiff, loadTimestamp);
      return true;
    }

    latestHashDiff = new LatestSatelliteHashDiff(string.Empty, string.Empty, DateTimeOffset.MinValue);
    return false;
  }

  private static IEnumerable<Dictionary<string, object>> GetTrackedRows(DbContext dbContext, string tableName) {
    foreach (var entry in dbContext.ChangeTracker.Entries<Dictionary<string, object>>()) {
      if (entry.State == EntityState.Deleted) {
        continue;
      }

      var producedName = entry.Metadata.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string;
      if (string.Equals(producedName ?? entry.Metadata.Name, tableName, StringComparison.Ordinal)) {
        yield return entry.Entity;
      }
    }
  }

  private static bool HasHashKey(Dictionary<string, object> row, string hashKeyColumnName, string hashKey) {
    return HasColumnValue(row, hashKeyColumnName, hashKey);
  }

  private static bool HasColumnValue(Dictionary<string, object> row, string columnName, string value) {
    return row.TryGetValue(columnName, out var currentValue) &&
        string.Equals(currentValue as string, value, StringComparison.Ordinal);
  }

  private string ComputeHash(IEnumerable<KeyValuePair<string, string>> fields) {
    var normalizedFields = _stableHashNormalizer.NormalizeFields(
        fields.Select(field => new KeyValuePair<string, object?>(field.Key, field.Value)));

    return _stableHashService.ComputeHash(normalizedFields).Value;
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

  private sealed record LatestSatelliteHashDiff(string ParentHashKey, string HashDiff, DateTimeOffset LoadTimestamp);
}
