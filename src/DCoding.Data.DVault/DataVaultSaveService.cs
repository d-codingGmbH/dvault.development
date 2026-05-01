using System.Collections.ObjectModel;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines the explicit DVault v1 write boundary used by callers instead of SaveChanges interception.
/// </summary>
public interface IDataVaultSaveService {
  /// <summary>
  /// Persists the requested Data Vault hub and link rows through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The explicit save request containing write metadata and row operations.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including generated hash keys.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
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
  /// <param name="hubOperations">The hub rows to persist before link rows.</param>
  /// <param name="linkOperations">The link rows to persist after hub rows.</param>
  public DataVaultSaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultHubSaveOperation> hubOperations,
      IEnumerable<DataVaultLinkSaveOperation> linkOperations) {
    ArgumentException.ThrowIfNullOrWhiteSpace(recordSource);
    ArgumentNullException.ThrowIfNull(hubOperations);
    ArgumentNullException.ThrowIfNull(linkOperations);

    LoadTimestamp = loadTimestamp.ToUniversalTime();
    RecordSource = recordSource;
    HubOperations = RequireOperations(hubOperations, nameof(hubOperations));
    LinkOperations = RequireOperations(linkOperations, nameof(linkOperations));
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
  /// Gets the hub rows to persist before link rows.
  /// </summary>
  public IReadOnlyList<DataVaultHubSaveOperation> HubOperations { get; }

  /// <summary>
  /// Gets the link rows to persist after hub rows.
  /// </summary>
  public IReadOnlyList<DataVaultLinkSaveOperation> LinkOperations { get; }

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
/// Summarizes rows persisted by an explicit DVault save request.
/// </summary>
public sealed class DataVaultSaveResult {
  /// <summary>
  /// Initializes a new save result.
  /// </summary>
  /// <param name="rowsWritten">The row count inserted by the explicit service invocation.</param>
  /// <param name="savedRecords">The generated hub and link hash-key summaries.</param>
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
  /// Gets the generated hub and link hash-key summaries.
  /// </summary>
  public IReadOnlyList<DataVaultSavedRecord> SavedRecords { get; }
}

/// <summary>
/// Summarizes one hub or link row persisted by an explicit DVault save request.
/// </summary>
public sealed class DataVaultSavedRecord {
  /// <summary>
  /// Initializes a new saved row summary.
  /// </summary>
  /// <param name="kind">Whether the saved row is a hub or link.</param>
  /// <param name="metadataName">The metadata declaration name that produced the row.</param>
  /// <param name="tableName">The produced table name that received the row.</param>
  /// <param name="hashKey">The generated Data Vault hash key persisted for the row.</param>
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
  /// Gets whether the saved row is a hub or link.
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
  /// Gets the generated Data Vault hash key persisted for the row.
  /// </summary>
  public string HashKey { get; }
}

internal sealed class DefaultDataVaultSaveService : IDataVaultSaveService {
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

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

    var savedRecords = new List<DataVaultSavedRecord>();
    var rowsWritten = 0;

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

    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    return new DataVaultSaveResult(rowsWritten, savedRecords);
  }

  private async Task<SaveOperationResult> AddHubAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      DataVaultHubSaveOperation operation,
      CancellationToken cancellationToken) {
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

    var rowWritten = await AddRowIfMissingAsync(
        dbContext,
        tableName,
        hashKeyColumnName,
        hashKey,
        row,
        cancellationToken).ConfigureAwait(false);

    return new SaveOperationResult(
        new DataVaultSavedRecord(DataVaultTableKind.Hub, hub.Name, tableName, hashKey),
        rowWritten);
  }

  private async Task<SaveOperationResult> AddLinkAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      DataVaultLinkSaveOperation operation,
      CancellationToken cancellationToken) {
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

    var rowWritten = await AddRowIfMissingAsync(
        dbContext,
        tableName,
        linkHashKeyColumnName,
        linkHashKey,
        row,
        cancellationToken).ConfigureAwait(false);

    return new SaveOperationResult(
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, tableName, linkHashKey),
        rowWritten);
  }

  private static async Task<bool> AddRowIfMissingAsync(
      DbContext dbContext,
      string tableName,
      string hashKeyColumnName,
      string hashKey,
      Dictionary<string, object> row,
      CancellationToken cancellationToken) {
    var rows = dbContext.Set<Dictionary<string, object>>(tableName);

    if (rows.Local.Any(existingRow => HasHashKey(existingRow, hashKeyColumnName, hashKey))) {
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

  private static bool HasHashKey(Dictionary<string, object> row, string hashKeyColumnName, string hashKey) {
    return row.TryGetValue(hashKeyColumnName, out var value) &&
        string.Equals(value as string, hashKey, StringComparison.Ordinal);
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
}
