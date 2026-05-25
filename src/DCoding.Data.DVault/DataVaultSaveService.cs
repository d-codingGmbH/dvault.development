using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

  /// <summary>
  /// Persists ordered chunks of explicit Data Vault save requests through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The explicit chunked save request containing ordered bounded chunks.</param>
  /// <param name="cancellationToken">A token used to observe cancellation before continuing to later chunks.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultChunkedSaveRequest request,
      CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides registry-backed save adapters over the explicit DVault save service.
/// </summary>
public static class DataVaultSaveServiceRegistryExtensions {
  /// <summary>
  /// Resolves hub, link, and satellite metadata from the authoritative DbContext registry and persists the resulting explicit request.
  /// </summary>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed save request containing logical metadata names and row operations.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before write orchestration starts when the DbContext has no authoritative registry source or a required metadata
  /// declaration is missing from that source.
  /// </exception>
  /// <remarks>
  /// This adapter resolves metadata once and then delegates to the existing explicit request pipeline. Callers that invoke
  /// <see cref="IDataVaultSaveService.SaveAsync(DbContext, DataVaultSaveRequest, CancellationToken)" /> or
  /// <see cref="IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest, CancellationToken)" /> keep explicit
  /// caller-supplied metadata precedence and bypass registry resolution.
  /// </remarks>
  public static Task<DataVaultSaveResult> SaveAsync(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      DataVaultRegistrySaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    return saveService.SaveAsync(
        dbContext,
        ResolveRequest(registry, request),
        cancellationToken);
  }

  /// <summary>
  /// Resolves all registry-backed save requests from the authoritative DbContext registry and persists them as one ordered batch.
  /// </summary>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed bulk save request containing ordered logical-name requests.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before write orchestration starts when the DbContext has no authoritative registry source or a required metadata
  /// declaration is missing from that source.
  /// </exception>
  /// <remarks>
  /// All metadata declarations are resolved before the underlying explicit save service is called, so missing registry entries
  /// fail deterministically without partial persistence. Explicit request overloads remain the advanced path when the caller
  /// wants supplied metadata to take precedence over the registry.
  /// </remarks>
  public static Task<DataVaultSaveResult> SaveAsync(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      DataVaultRegistryBulkSaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var resolvedRequests = request.Requests
        .Select(current => ResolveRequest(registry, current))
        .ToArray();

    return saveService.SaveAsync(
        dbContext,
        new DataVaultBulkSaveRequest(resolvedRequests),
        cancellationToken);
  }

  internal static DataVaultSaveRequest ResolveRequest(
      DataVaultMetadataRegistry registry,
      DataVaultRegistrySaveRequest request) {
    var hubOperations = request.HubOperations
        .Select(operation => new DataVaultHubSaveOperation(
            DataVaultRegistryMetadataResolver.GetRequiredHub(registry, operation.HubName),
            operation.BusinessKeyValues))
        .ToArray();
    var linkOperations = request.LinkOperations
        .Select(operation => new DataVaultLinkSaveOperation(
            DataVaultRegistryMetadataResolver.GetRequiredLink(registry, operation.LinkName),
            operation.ParticipantHashKeyValues))
        .ToArray();
    var satelliteOperations = request.SatelliteOperations
        .Select(operation => new DataVaultSatelliteSaveOperation(
            DataVaultRegistryMetadataResolver.GetRequiredSatellite(registry, operation.Parent, operation.SatelliteName),
            operation.ParentHashKey,
            operation.DrivingKeyValues,
            operation.PayloadValues,
            operation.HashDiff))
        .ToArray();

    return new DataVaultSaveRequest(
        request.LoadTimestamp,
        request.RecordSource,
        hubOperations,
        linkOperations,
        satelliteOperations);
  }
}

/// <summary>
/// Groups registry-backed DVault save operations that share one load timestamp and record source.
/// </summary>
public sealed class DataVaultRegistrySaveRequest {
  /// <summary>
  /// Initializes a new registry-backed save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows whose metadata should be resolved by logical hub name.</param>
  /// <param name="linkOperations">The link rows whose metadata should be resolved by logical link name.</param>
  public DataVaultRegistrySaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultRegistryHubSaveOperation> hubOperations,
      IEnumerable<DataVaultRegistryLinkSaveOperation> linkOperations)
      : this(loadTimestamp, recordSource, hubOperations, linkOperations, []) {
  }

  /// <summary>
  /// Initializes a new registry-backed save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows whose metadata should be resolved by logical hub name.</param>
  /// <param name="linkOperations">The link rows whose metadata should be resolved by logical link name.</param>
  /// <param name="satelliteOperations">The satellite rows whose metadata should be resolved by parent and logical satellite name.</param>
  public DataVaultRegistrySaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultRegistryHubSaveOperation> hubOperations,
      IEnumerable<DataVaultRegistryLinkSaveOperation> linkOperations,
      IEnumerable<DataVaultRegistrySatelliteSaveOperation> satelliteOperations) {
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
  /// Gets the hub rows whose metadata should be resolved by logical hub name before the explicit save pipeline runs.
  /// </summary>
  public IReadOnlyList<DataVaultRegistryHubSaveOperation> HubOperations { get; }

  /// <summary>
  /// Gets the link rows whose metadata should be resolved by logical link name before the explicit save pipeline runs.
  /// </summary>
  public IReadOnlyList<DataVaultRegistryLinkSaveOperation> LinkOperations { get; }

  /// <summary>
  /// Gets the satellite rows whose metadata should be resolved by parent and logical satellite name before the explicit save pipeline runs.
  /// </summary>
  public IReadOnlyList<DataVaultRegistrySatelliteSaveOperation> SatelliteOperations { get; }

  private static IReadOnlyList<T> RequireOperations<T>(IEnumerable<T> operations, string parameterName)
      where T : class {
    var values = operations.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault registry save operation collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}

/// <summary>
/// Groups multiple registry-backed DVault save requests that should be processed as one ordered batch.
/// </summary>
public sealed class DataVaultRegistryBulkSaveRequest {
  /// <summary>
  /// Initializes a new registry-backed bulk save request.
  /// </summary>
  /// <param name="requests">The registry-backed save requests to resolve and process in caller-supplied order.</param>
  public DataVaultRegistryBulkSaveRequest(IEnumerable<DataVaultRegistrySaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    Requests = RequireRequests(requests, nameof(requests));
  }

  /// <summary>
  /// Gets the registry-backed save requests processed in caller-supplied order.
  /// </summary>
  public IReadOnlyList<DataVaultRegistrySaveRequest> Requests { get; }

  private static IReadOnlyList<DataVaultRegistrySaveRequest> RequireRequests(
      IEnumerable<DataVaultRegistrySaveRequest> requests,
      string parameterName) {
    var values = requests.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault registry bulk save request collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}

/// <summary>
/// Describes one hub row whose metadata should be resolved from the authoritative registry by logical hub name.
/// </summary>
public sealed class DataVaultRegistryHubSaveOperation {
  /// <summary>
  /// Initializes a new registry-backed hub save operation.
  /// </summary>
  /// <param name="hubName">The exact logical hub metadata name to resolve from the authoritative registry.</param>
  /// <param name="businessKeyValues">Business-key values keyed by the resolved hub metadata business-key names.</param>
  public DataVaultRegistryHubSaveOperation(
      string hubName,
      IEnumerable<KeyValuePair<string, string>> businessKeyValues) {
    HubName = DataVaultMetadataValidation.RequireName(hubName, nameof(hubName));
    BusinessKeyValues = DataVaultHubSaveOperation.RequireValues(businessKeyValues, nameof(businessKeyValues));
  }

  /// <summary>
  /// Gets the exact logical hub metadata name to resolve from the authoritative registry.
  /// </summary>
  public string HubName { get; }

  /// <summary>
  /// Gets business-key values keyed by the resolved hub metadata business-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> BusinessKeyValues { get; }
}

/// <summary>
/// Describes one link row whose metadata should be resolved from the authoritative registry by logical link name.
/// </summary>
public sealed class DataVaultRegistryLinkSaveOperation {
  /// <summary>
  /// Initializes a new registry-backed link save operation.
  /// </summary>
  /// <param name="linkName">The exact logical link metadata name to resolve from the authoritative registry.</param>
  /// <param name="participantHashKeyValues">Participant hash keys keyed by the resolved link produced participant names.</param>
  public DataVaultRegistryLinkSaveOperation(
      string linkName,
      IEnumerable<KeyValuePair<string, string>> participantHashKeyValues) {
    LinkName = DataVaultMetadataValidation.RequireName(linkName, nameof(linkName));
    ParticipantHashKeyValues = DataVaultHubSaveOperation.RequireValues(
        participantHashKeyValues,
        nameof(participantHashKeyValues));
  }

  /// <summary>
  /// Gets the exact logical link metadata name to resolve from the authoritative registry.
  /// </summary>
  public string LinkName { get; }

  /// <summary>
  /// Gets participant hash keys keyed by the resolved link produced participant names.
  /// </summary>
  public IReadOnlyDictionary<string, string> ParticipantHashKeyValues { get; }
}

/// <summary>
/// Describes one satellite row whose metadata should be resolved from the authoritative registry by parent and logical satellite name.
/// </summary>
public sealed class DataVaultRegistrySatelliteSaveOperation {
  /// <summary>
  /// Initializes a new registry-backed satellite save operation.
  /// </summary>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="payloadValues">Payload values keyed by the resolved satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultRegistrySatelliteSaveOperation(
      DataVaultMetadataReference parent,
      string satelliteName,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff)
      : this(parent, satelliteName, parentHashKey, [], payloadValues, hashDiff) {
  }

  /// <summary>
  /// Initializes a new registry-backed multi-active satellite save operation.
  /// </summary>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="drivingKeyValues">Driving-key values keyed by the resolved satellite metadata driving-key names.</param>
  /// <param name="payloadValues">Payload values keyed by the resolved satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultRegistrySatelliteSaveOperation(
      DataVaultMetadataReference parent,
      string satelliteName,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff) {
    ArgumentNullException.ThrowIfNull(parent);
    ArgumentException.ThrowIfNullOrWhiteSpace(parentHashKey);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashDiff);

    Parent = parent;
    SatelliteName = DataVaultMetadataValidation.RequireName(satelliteName, nameof(satelliteName));
    ParentHashKey = parentHashKey;
    DrivingKeyValues = DataVaultHubSaveOperation.RequireValues(drivingKeyValues, nameof(drivingKeyValues));
    PayloadValues = DataVaultHubSaveOperation.RequireValues(payloadValues, nameof(payloadValues));
    HashDiff = hashDiff;
  }

  /// <summary>
  /// Gets the exact parent hub or link metadata reference used to resolve the satellite.
  /// </summary>
  public DataVaultMetadataReference Parent { get; }

  /// <summary>
  /// Gets the exact logical satellite metadata name to resolve from the authoritative registry.
  /// </summary>
  public string SatelliteName { get; }

  /// <summary>
  /// Gets the explicit parent hub or link hash key associated with this satellite row.
  /// </summary>
  public string ParentHashKey { get; }

  /// <summary>
  /// Gets driving-key values keyed by the resolved satellite metadata driving-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

  /// <summary>
  /// Gets payload values keyed by the resolved satellite metadata payload names.
  /// </summary>
  public IReadOnlyDictionary<string, string> PayloadValues { get; }

  /// <summary>
  /// Gets the caller-supplied deterministic hash diff for this payload state.
  /// </summary>
  public string HashDiff { get; }
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
/// Groups ordered bounded chunks of explicit DVault save requests for provider-neutral chunked execution.
/// </summary>
public sealed class DataVaultChunkedSaveRequest {
  /// <summary>
  /// Initializes a new explicit chunked save request.
  /// </summary>
  /// <param name="chunks">The chunks to process in caller-supplied order.</param>
  public DataVaultChunkedSaveRequest(IEnumerable<DataVaultSaveChunk> chunks) {
    ArgumentNullException.ThrowIfNull(chunks);

    Chunks = RequireChunks(chunks, nameof(chunks));
  }

  /// <summary>
  /// Gets the chunks processed in caller-supplied order.
  /// </summary>
  public IReadOnlyList<DataVaultSaveChunk> Chunks { get; }

  private static IReadOnlyList<DataVaultSaveChunk> RequireChunks(
      IEnumerable<DataVaultSaveChunk> chunks,
      string parameterName) {
    var values = chunks.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault chunked save request collections must not contain null chunks.", parameterName);
      }
    }

    return values;
  }
}

/// <summary>
/// Groups one bounded ordered chunk of explicit DVault save requests.
/// </summary>
public sealed class DataVaultSaveChunk {
  /// <summary>
  /// Initializes a new explicit save chunk.
  /// </summary>
  /// <param name="requests">The save requests to process in caller-supplied order within this chunk.</param>
  public DataVaultSaveChunk(IEnumerable<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    Requests = new DataVaultBulkSaveRequest(requests).Requests;
  }

  /// <summary>
  /// Gets the save requests processed in caller-supplied order within this chunk.
  /// </summary>
  public IReadOnlyList<DataVaultSaveRequest> Requests { get; }
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
  /// <param name="participantHashKeyValues">Participant hash keys keyed by the produced participant names.</param>
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
  /// Gets participant hash keys keyed by the produced participant names.
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
      string hashDiff)
      : this(metadata, parentHashKey, [], payloadValues, hashDiff) {
  }

  /// <summary>
  /// Initializes a new multi-active satellite save operation.
  /// </summary>
  /// <param name="metadata">The satellite metadata declaration that owns the target table and payload shape.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="drivingKeyValues">Driving-key values keyed by the satellite metadata driving-key names.</param>
  /// <param name="payloadValues">Payload values keyed by the satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultSatelliteSaveOperation(
      DataVaultSatelliteMetadata metadata,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff) {
    ArgumentNullException.ThrowIfNull(metadata);
    ArgumentException.ThrowIfNullOrWhiteSpace(parentHashKey);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashDiff);

    Metadata = metadata;
    ParentHashKey = parentHashKey;
    DrivingKeyValues = RequireDrivingKeyValues(metadata, drivingKeyValues);
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
  /// Gets driving-key values keyed by the satellite metadata driving-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

  /// <summary>
  /// Gets payload values keyed by the satellite metadata payload names.
  /// </summary>
  public IReadOnlyDictionary<string, string> PayloadValues { get; }

  /// <summary>
  /// Gets the caller-supplied deterministic hash diff for this payload state.
  /// </summary>
  public string HashDiff { get; }

  private static IReadOnlyDictionary<string, string> RequireDrivingKeyValues(
      DataVaultSatelliteMetadata metadata,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues) {
    var values = DataVaultHubSaveOperation.RequireValues(drivingKeyValues, nameof(drivingKeyValues));
    var declaredNames = metadata.DrivingKeyNames.ToHashSet(StringComparer.Ordinal);

    foreach (var drivingKeyName in metadata.DrivingKeyNames) {
      if (!values.ContainsKey(drivingKeyName)) {
        throw new ArgumentException(
            "The Data Vault satellite save operation is missing required driving-key value '" + drivingKeyName + "'.",
            nameof(drivingKeyValues));
      }
    }

    foreach (var suppliedName in values.Keys) {
      if (!declaredNames.Contains(suppliedName)) {
        throw new ArgumentException(
            "The Data Vault satellite save operation contains unexpected driving-key value '" + suppliedName + "'.",
            nameof(drivingKeyValues));
      }
    }

    return values;
  }
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
  public DataVaultSavedRecord(DataVaultTableKind kind, string metadataName, string tableName, string hashKey)
      : this(kind, metadataName, tableName, hashKey, []) {
  }

  /// <summary>
  /// Initializes a new saved row summary with multi-active driving-key identity values.
  /// </summary>
  /// <param name="kind">Whether the saved row is a hub, link, or satellite.</param>
  /// <param name="metadataName">The metadata declaration name that produced the row.</param>
  /// <param name="tableName">The produced table name that received the row.</param>
  /// <param name="hashKey">The generated Data Vault hash key persisted for the row, or parent hash key for satellites.</param>
  /// <param name="drivingKeyValues">Driving-key identity values keyed by canonical driving-key name.</param>
  public DataVaultSavedRecord(
      DataVaultTableKind kind,
      string metadataName,
      string tableName,
      string hashKey,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues) {
    ArgumentException.ThrowIfNullOrWhiteSpace(metadataName);
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashKey);

    Kind = kind;
    MetadataName = metadataName;
    TableName = tableName;
    HashKey = hashKey;
    DrivingKeyValues = DataVaultHubSaveOperation.RequireValues(drivingKeyValues, nameof(drivingKeyValues));
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

  /// <summary>
  /// Gets multi-active driving-key identity values keyed by canonical driving-key name.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }
}

internal sealed class DefaultDataVaultSaveService : IDataVaultSaveService {
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  private readonly IDataVaultLoadTimestampResolver _loadTimestampResolver;
  private readonly IReadOnlyList<IDataVaultProviderSaveStrategy> _providerSaveStrategies;
  private readonly IDataVaultRecordSourceResolver _recordSourceResolver;
  private readonly IStableHashService _stableHashService;
  private readonly IStableHashNormalizer _stableHashNormalizer;
  private readonly IReadOnlyList<IDataVaultTelemetryObserver> _telemetryObservers;

  public DefaultDataVaultSaveService(
      IStableHashService stableHashService,
      IStableHashNormalizer stableHashNormalizer)
      : this(
          stableHashService,
          stableHashNormalizer,
          [DefaultDataVaultLoadTimestampResolver.Instance],
          [DefaultDataVaultRecordSourceResolver.Instance],
          []) {
  }

  public DefaultDataVaultSaveService(
      IStableHashService stableHashService,
      IStableHashNormalizer stableHashNormalizer,
      IEnumerable<IDataVaultProviderSaveStrategy> providerSaveStrategies)
      : this(
          stableHashService,
          stableHashNormalizer,
          [DefaultDataVaultLoadTimestampResolver.Instance],
          [DefaultDataVaultRecordSourceResolver.Instance],
          providerSaveStrategies) {
  }

  public DefaultDataVaultSaveService(
      IStableHashService stableHashService,
      IStableHashNormalizer stableHashNormalizer,
      IEnumerable<IDataVaultLoadTimestampResolver> loadTimestampResolvers,
      IEnumerable<IDataVaultRecordSourceResolver> recordSourceResolvers,
      IEnumerable<IDataVaultProviderSaveStrategy> providerSaveStrategies)
      : this(
          stableHashService,
          stableHashNormalizer,
          loadTimestampResolvers,
          recordSourceResolvers,
          providerSaveStrategies,
          []) {
  }

  public DefaultDataVaultSaveService(
      IStableHashService stableHashService,
      IStableHashNormalizer stableHashNormalizer,
      IEnumerable<IDataVaultLoadTimestampResolver> loadTimestampResolvers,
      IEnumerable<IDataVaultRecordSourceResolver> recordSourceResolvers,
      IEnumerable<IDataVaultProviderSaveStrategy> providerSaveStrategies,
      IEnumerable<IDataVaultTelemetryObserver> telemetryObservers) {
    ArgumentNullException.ThrowIfNull(stableHashService);
    ArgumentNullException.ThrowIfNull(stableHashNormalizer);
    ArgumentNullException.ThrowIfNull(loadTimestampResolvers);
    ArgumentNullException.ThrowIfNull(recordSourceResolvers);
    ArgumentNullException.ThrowIfNull(providerSaveStrategies);
    ArgumentNullException.ThrowIfNull(telemetryObservers);

    _stableHashService = stableHashService;
    _stableHashNormalizer = stableHashNormalizer;
    _loadTimestampResolver = RequireSingleResolver(
        loadTimestampResolvers,
        DefaultDataVaultLoadTimestampResolver.Instance,
        "Data Vault load timestamp resolver configuration is ambiguous; register at most one load timestamp resolver.");
    _recordSourceResolver = RequireSingleResolver(
        recordSourceResolvers,
        DefaultDataVaultRecordSourceResolver.Instance,
        "Data Vault record-source resolver configuration is ambiguous; register at most one record-source resolver.");
    _providerSaveStrategies = providerSaveStrategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
    _telemetryObservers = DataVaultTelemetryDispatcher.CreateObservers(telemetryObservers);
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return await SaveRequestsAsync(
        dbContext,
        [request],
        DataVaultSaveTelemetryOperationKind.SingleRequest,
        cancellationToken).ConfigureAwait(false);
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultBulkSaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return await SaveRequestsAsync(
        dbContext,
        request.Requests,
        DataVaultSaveTelemetryOperationKind.BulkRequest,
        cancellationToken).ConfigureAwait(false);
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultChunkedSaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return await SaveChunksAsync(
        dbContext,
        request.Chunks,
        cancellationToken).ConfigureAwait(false);
  }

  private async Task<DataVaultSaveResult> SaveChunksAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveChunk> chunks,
      CancellationToken cancellationToken) {
    var rowsWritten = 0;
    var uniqueSavedRecords = new List<DataVaultSavedRecord>();
    var satelliteSavedRecords = new List<DataVaultSavedRecord>();

    foreach (var chunk in chunks) {
      cancellationToken.ThrowIfCancellationRequested();
      if (chunk.Requests.Count == 0) {
        continue;
      }

      var result = await SaveRequestsAsync(
          dbContext,
          chunk.Requests,
          DataVaultSaveTelemetryOperationKind.BulkRequest,
          cancellationToken).ConfigureAwait(false);

      rowsWritten += result.RowsWritten;
      foreach (var savedRecord in result.SavedRecords) {
        if (savedRecord.Kind == DataVaultTableKind.Satellite) {
          satelliteSavedRecords.Add(savedRecord);
        }
        else {
          uniqueSavedRecords.Add(savedRecord);
        }
      }
    }

    return new DataVaultSaveResult(rowsWritten, uniqueSavedRecords.Concat(satelliteSavedRecords));
  }

  private async Task<DataVaultSaveResult> SaveRequestsAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      DataVaultSaveTelemetryOperationKind operationKind,
      CancellationToken cancellationToken) {
    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultSaveTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      var resolvedRequests = ResolveRequests(requests);
      strategySelection = DataVaultTelemetryStrategySelector.SelectSaveStrategy(dbContext, _providerSaveStrategies, requests);

      DataVaultSaveResult result;
      if (strategySelection.Strategy is not null) {
        var context = new DataVaultProviderSaveStrategyContext(
            dbContext,
            requests,
            resolvedRequests,
            _stableHashService,
            _stableHashNormalizer);
        result = await strategySelection.Strategy.SaveAsync(context, cancellationToken).ConfigureAwait(false);
      }
      else {
        result = await SaveProviderNeutralAsync(dbContext, resolvedRequests, cancellationToken).ConfigureAwait(false);
      }

      DataVaultTelemetryDispatcher.RecordSave(
          _telemetryObservers,
          DataVaultTelemetrySummaryFactory.CreateSaveSummary(
              operationKind,
              DataVaultTelemetryOutcome.Succeeded,
              requests,
              result,
              DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
              strategySelection));

      return result;
    }
    catch {
      DataVaultTelemetryDispatcher.RecordSave(
          _telemetryObservers,
          DataVaultTelemetrySummaryFactory.CreateSaveSummary(
              operationKind,
              DataVaultTelemetryOutcome.Failed,
              requests,
              result: null,
              DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
              strategySelection));
      throw;
    }
  }

  private async Task<DataVaultSaveResult> SaveProviderNeutralAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultResolvedSaveRequest> resolvedRequests,
      CancellationToken cancellationToken) {
    var savedRecords = new List<DataVaultSavedRecord>();
    var rowsWritten = 0;
    var uniqueResults = await AddUniqueRowsAsync(
        dbContext,
        CreateUniqueRowSavePlans(resolvedRequests),
        cancellationToken).ConfigureAwait(false);

    foreach (var result in uniqueResults) {
      savedRecords.Add(result.SavedRecord);
      if (result.RowWritten) {
        rowsWritten++;
      }
    }

    var satelliteResults = await AddSatellitesAsync(dbContext, resolvedRequests, cancellationToken).ConfigureAwait(false);
    foreach (var result in satelliteResults) {
      savedRecords.Add(result.SavedRecord);
      if (result.RowWritten) {
        rowsWritten++;
      }
    }

    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    return new DataVaultSaveResult(rowsWritten, savedRecords);
  }

  private IReadOnlyList<DataVaultResolvedSaveRequest> ResolveRequests(IReadOnlyList<DataVaultSaveRequest> requests) {
    var resolvedRequests = new DataVaultResolvedSaveRequest[requests.Count];

    for (var index = 0; index < requests.Count; index++) {
      var request = requests[index];
      var loadTimestamp = _loadTimestampResolver.ResolveLoadTimestamp(new DataVaultLoadTimestampResolutionContext(request));
      if (loadTimestamp is null) {
        throw new InvalidOperationException("Data Vault load timestamp resolver returned null.");
      }

      if (loadTimestamp.Value.Offset != TimeSpan.Zero) {
        throw new InvalidOperationException("Data Vault load timestamp resolver must return a UTC DateTimeOffset with zero offset.");
      }

      var recordSource = _recordSourceResolver.ResolveRecordSource(
          new DataVaultRecordSourceResolutionContext(request, loadTimestamp.Value));
      if (string.IsNullOrWhiteSpace(recordSource)) {
        throw new InvalidOperationException("Data Vault record-source resolver must return a non-empty record source.");
      }

      resolvedRequests[index] = new DataVaultResolvedSaveRequest(request, loadTimestamp.Value, recordSource);
    }

    return resolvedRequests;
  }

  private async Task<IReadOnlyList<SaveOperationResult>> AddUniqueRowsAsync(
      DbContext dbContext,
      IReadOnlyList<UniqueRowSavePlan> plans,
      CancellationToken cancellationToken) {
    var results = new SaveOperationResult[plans.Count];

    foreach (var group in plans.GroupBy(plan => plan.Table)) {
      var trackedHashKeys = GetTrackedHashKeys(
          dbContext,
          group.Key.TableName,
          group.Key.HashKeyColumnName);
      var candidateHashKeys = group
          .Select(plan => plan.HashKey)
          .Where(hashKey => !trackedHashKeys.Contains(hashKey))
          .Distinct(StringComparer.Ordinal)
          .ToArray();
      var persistedHashKeys = await LoadPersistedUniqueHashKeysAsync(
          dbContext,
          group.Key,
          candidateHashKeys,
          cancellationToken).ConfigureAwait(false);
      var rows = dbContext.Set<Dictionary<string, object>>(group.Key.TableName);

      foreach (var plan in group) {
        var rowWritten = !trackedHashKeys.Contains(plan.HashKey) &&
            !persistedHashKeys.Contains(plan.HashKey);
        if (rowWritten) {
          ApplyModelValueFormats(dbContext, group.Key.TableName, plan.Row);
          rows.Add(plan.Row);
          trackedHashKeys.Add(plan.HashKey);
        }

        results[plan.Ordinal] = new SaveOperationResult(plan.SavedRecord, rowWritten);
      }
    }

    return results;
  }

  private IReadOnlyList<UniqueRowSavePlan> CreateUniqueRowSavePlans(
      IReadOnlyList<DataVaultResolvedSaveRequest> requests) {
    var plans = new List<UniqueRowSavePlan>();

    foreach (var request in requests) {
      foreach (var operation in request.Request.HubOperations) {
        plans.Add(CreateHubSavePlan(request, operation));
      }

      foreach (var operation in request.Request.LinkOperations) {
        plans.Add(CreateLinkSavePlan(request, operation));
      }
    }

    return plans
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
  }

  private static async Task<HashSet<string>> LoadPersistedUniqueHashKeysAsync(
      DbContext dbContext,
      UniqueTableProjection table,
      IReadOnlyCollection<string> hashKeys,
      CancellationToken cancellationToken) {
    var persistedHashKeys = new HashSet<string>(StringComparer.Ordinal);
    if (hashKeys.Count == 0) {
      return persistedHashKeys;
    }

    var rows = dbContext.Set<Dictionary<string, object>>(table.TableName);
    foreach (var hashKeyBatch in hashKeys.Chunk(500)) {
      var persistedRows = await rows
          .AsNoTracking()
          .WhereStringPropertyEqualsAny(table.HashKeyColumnName, hashKeyBatch)
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);

      foreach (var persistedRow in persistedRows) {
        if (persistedRow.TryGetValue(table.HashKeyColumnName, out var value) &&
            value is string hashKey) {
          persistedHashKeys.Add(hashKey);
        }
      }
    }

    return persistedHashKeys;
  }

  private static HashSet<string> GetTrackedHashKeys(
      DbContext dbContext,
      string tableName,
      string hashKeyColumnName) {
    var hashKeys = new HashSet<string>(StringComparer.Ordinal);

    foreach (var trackedRow in GetTrackedRows(dbContext, tableName)) {
      if (trackedRow.TryGetValue(hashKeyColumnName, out var value) &&
          value is string hashKey) {
        hashKeys.Add(hashKey);
      }
    }

    return hashKeys;
  }

  private UniqueRowSavePlan CreateHubSavePlan(
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

  private UniqueRowSavePlan CreateLinkSavePlan(
      DataVaultResolvedSaveRequest request,
      DataVaultLinkSaveOperation operation) {
    var link = operation.Metadata;
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

  private async Task<IReadOnlyList<SaveOperationResult>> AddSatellitesAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultResolvedSaveRequest> requests,
      CancellationToken cancellationToken) {
    var plans = CreateSatelliteSavePlans(requests);
    var filteredPlans = await FilterSatellitePlansAsync(dbContext, plans, cancellationToken).ConfigureAwait(false);

    foreach (var group in filteredPlans.RowsToWrite.GroupBy(plan => plan.Table)) {
      var rows = dbContext.Set<Dictionary<string, object>>(group.Key.TableName);
      foreach (var plan in group) {
        ApplyModelValueFormats(dbContext, group.Key.TableName, plan.Row);
        rows.Add(plan.Row);
      }
    }

    return filteredPlans.Results;
  }

  private static IReadOnlyList<SatelliteSavePlan> CreateSatelliteSavePlans(IReadOnlyList<DataVaultResolvedSaveRequest> requests) {
    return requests
        .SelectMany(request => request.Request.SatelliteOperations
            .Select(operation => CreateSatelliteSavePlan(request, operation)))
        .Select((plan, index) => plan with { Ordinal = index })
        .ToArray();
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
    var drivingKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.DrivingKeyNames,
        [parentHashKeyColumnName, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var drivingKeyFields = satellite.DrivingKeyNames
        .Select(name => new KeyValuePair<string, string>(
            name,
            GetRequiredValue(operation.DrivingKeyValues, name, nameof(operation.DrivingKeyValues))))
        .ToArray();
    var payloadColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.PayloadColumns.Select(column => column.ColumnName),
        [parentHashKeyColumnName, .. drivingKeyColumnNames, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var payloadFields = satellite.PayloadColumns
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.PayloadValues, column.ColumnName, nameof(operation.PayloadValues))))
        .ToArray();
    var row = new Dictionary<string, object> {
      [parentHashKeyColumnName] = operation.ParentHashKey,
    };

    for (var index = 0; index < drivingKeyFields.Length; index++) {
      row.Add(drivingKeyColumnNames[index], drivingKeyFields[index].Value);
    }

    row.Add(hashDiffColumnName, operation.HashDiff);
    row.Add(loadTimestampColumnName, request.LoadTimestamp);
    row.Add(recordSourceColumnName, request.RecordSource);

    for (var index = 0; index < payloadFields.Length; index++) {
      row.Add(payloadColumnNames[index], payloadFields[index].Value);
    }

    var table = new SatelliteTableProjection(
        tableName,
        parentHashKeyColumnName,
        hashDiffColumnName,
        loadTimestampColumnName,
        drivingKeyColumnNames);
    var seriesKey = new SatelliteSeriesKey(
        operation.ParentHashKey,
        drivingKeyFields.Select(field => field.Value));
    var savedRecord = new DataVaultSavedRecord(
        DataVaultTableKind.Satellite,
        satellite.Name,
        tableName,
        operation.ParentHashKey,
        drivingKeyFields);

    return new SatelliteSavePlan(
        -1,
        table,
        seriesKey,
        operation.ParentHashKey,
        operation.HashDiff,
        request.LoadTimestamp,
        row,
        savedRecord);
  }

  private static async Task<Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff>> LoadLatestSatelliteHashDiffsAsync(
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
      if (!latestByParent.TryGetValue(persistedHashDiff.SeriesKey, out var current) ||
          persistedHashDiff.LoadTimestamp > current.LoadTimestamp) {
        latestByParent[persistedHashDiff.SeriesKey] = persistedHashDiff;
      }
    }

    return latestByParent;
  }

  private static Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff> GetLatestTrackedSatelliteHashDiffs(
      DbContext dbContext,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys) {
    var parentKeySet = parentHashKeys.ToHashSet(StringComparer.Ordinal);
    var latestBySeries = new Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff>();

    foreach (var trackedRow in GetTrackedRows(dbContext, table.TableName)) {
      if (!TryCreateLatestSatelliteHashDiff(trackedRow, table, out var current) ||
          !parentKeySet.Contains(current.SeriesKey.ParentHashKey)) {
        continue;
      }

      if (!latestBySeries.TryGetValue(current.SeriesKey, out var previous) ||
          current.LoadTimestamp > previous.LoadTimestamp) {
        latestBySeries[current.SeriesKey] = current;
      }
    }

    return latestBySeries;
  }

  private static async Task<IReadOnlyList<LatestSatelliteHashDiff>> LoadLatestPersistedSatelliteHashDiffsAsync(
      DbContext dbContext,
      SatelliteTableProjection table,
      IEnumerable<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var rows = dbContext.Set<Dictionary<string, object>>(table.TableName);
    var latestRows = new List<LatestSatelliteHashDiff>();

    foreach (var parentHashKeyBatch in parentHashKeys.Distinct(StringComparer.Ordinal).Chunk(500)) {
      var persistedRows = await rows
          .AsNoTracking()
          .WhereStringPropertyEqualsAny(table.ParentHashKeyColumnName, parentHashKeyBatch)
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);
      var batchRows = persistedRows
          .Select(row => TryCreateLatestSatelliteHashDiff(row, table, out var latestHashDiff)
              ? latestHashDiff
              : null)
          .Where(row => row is not null)
          .Cast<LatestSatelliteHashDiff>()
          .ToArray();

      var batchLatestRows = batchRows
          .GroupBy(row => row.SeriesKey)
          .Select(group => group.OrderByDescending(row => row.LoadTimestamp).First());

      latestRows.AddRange(batchLatestRows);
    }

    return latestRows;
  }

  private static bool ShouldWriteSatelliteRow(
      Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff> latestHashDiffs,
      SatelliteSavePlan plan) {
    return !latestHashDiffs.TryGetValue(plan.SeriesKey, out var latestHashDiff) ||
        !string.Equals(latestHashDiff.HashDiff, plan.HashDiff, StringComparison.Ordinal);
  }

  private static void TrackLatestSatelliteHashDiff(
      Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff> latestHashDiffs,
      SatelliteSavePlan plan) {
    if (!latestHashDiffs.TryGetValue(plan.SeriesKey, out var latestHashDiff) ||
        plan.LoadTimestamp >= latestHashDiff.LoadTimestamp) {
      latestHashDiffs[plan.SeriesKey] = new LatestSatelliteHashDiff(
          plan.SeriesKey,
          plan.HashDiff,
          plan.LoadTimestamp);
    }
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
        TryReadDrivingKeyValues(row, table, out var drivingKeyValues) &&
        TryReadLoadTimestamp(loadTimestampValue, out var loadTimestamp)) {
      latestHashDiff = new LatestSatelliteHashDiff(
          new SatelliteSeriesKey(parentHashKey, drivingKeyValues),
          hashDiff,
          loadTimestamp);
      return true;
    }

    latestHashDiff = new LatestSatelliteHashDiff(
        new SatelliteSeriesKey(string.Empty, []),
        string.Empty,
        DateTimeOffset.MinValue);
    return false;
  }

  private static bool TryReadDrivingKeyValues(
      Dictionary<string, object> row,
      SatelliteTableProjection table,
      out IReadOnlyList<string> drivingKeyValues) {
    var values = new string[table.DrivingKeyColumnNames.Count];
    for (var index = 0; index < table.DrivingKeyColumnNames.Count; index++) {
      if (!row.TryGetValue(table.DrivingKeyColumnNames[index], out var value) ||
          value is not string text) {
        drivingKeyValues = [];
        return false;
      }

      values[index] = text;
    }

    drivingKeyValues = values;
    return true;
  }

  private static void ApplyModelValueFormats(
      DbContext dbContext,
      string tableName,
      Dictionary<string, object> row) {
    var entityType = FindEntityType(dbContext, tableName);
    if (entityType is null) {
      return;
    }

    foreach (var property in entityType.GetProperties()) {
      if (!row.TryGetValue(property.Name, out var value)) {
        continue;
      }

      var valueFormat = property.FindAnnotation(DataVaultAnnotationNames.ProviderValueFormat)?.Value;
      if (valueFormat is DataVaultProviderValueFormat &&
          value is DateTimeOffset loadTimestamp) {
        row[property.Name] = DataVaultLoadTimestampValueConverter.ToProviderValue(property, loadTimestamp);
      }
    }
  }

  private static IEntityType? FindEntityType(DbContext dbContext, string tableName) {
    return dbContext.Model.GetEntityTypes().FirstOrDefault(entity =>
        string.Equals(entity.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string, tableName, StringComparison.Ordinal) ||
        string.Equals(entity.Name, tableName, StringComparison.Ordinal));
  }

  private static bool TryReadLoadTimestamp(object? value, out DateTimeOffset loadTimestamp) {
    return DataVaultLoadTimestampValueConverter.TryReadProviderValue(value, out loadTimestamp);
  }

  private static IEnumerable<Dictionary<string, object>> GetTrackedRows(DbContext dbContext, string tableName) {
    foreach (var entry in dbContext.ChangeTracker.Entries()) {
      if (entry.State == EntityState.Deleted) {
        continue;
      }

      if (entry.Entity is not Dictionary<string, object> row) {
        continue;
      }

      var producedName = entry.Metadata.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string;
      if (string.Equals(producedName ?? entry.Metadata.Name, tableName, StringComparison.Ordinal)) {
        yield return row;
      }
    }
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

  private static TResolver RequireSingleResolver<TResolver>(
      IEnumerable<TResolver> resolvers,
      TResolver fallback,
      string ambiguityMessage)
      where TResolver : class {
    ArgumentNullException.ThrowIfNull(resolvers);
    ArgumentNullException.ThrowIfNull(fallback);

    var resolverArray = resolvers.ToArray();
    foreach (var resolver in resolverArray) {
      if (resolver is null) {
        throw new ArgumentException("Data Vault resolver collections must not contain null values.", nameof(resolvers));
      }
    }

    return resolverArray.Length switch {
      0 => fallback,
      1 => resolverArray[0],
      _ => throw new InvalidOperationException(ambiguityMessage),
    };
  }

  private sealed record SaveOperationResult(DataVaultSavedRecord SavedRecord, bool RowWritten);

  private sealed record UniqueTableProjection(string TableName, string HashKeyColumnName);

  private sealed record UniqueRowSavePlan(
      UniqueTableProjection Table,
      string HashKey,
      Dictionary<string, object> Row,
      DataVaultSavedRecord SavedRecord,
      int Ordinal);

  private sealed class SatelliteTableProjection : IEquatable<SatelliteTableProjection> {
    private readonly string _drivingKeyColumnSignature;

    public SatelliteTableProjection(
        string tableName,
        string parentHashKeyColumnName,
        string hashDiffColumnName,
        string loadTimestampColumnName,
        IEnumerable<string> drivingKeyColumnNames) {
      TableName = tableName;
      ParentHashKeyColumnName = parentHashKeyColumnName;
      HashDiffColumnName = hashDiffColumnName;
      LoadTimestampColumnName = loadTimestampColumnName;
      DrivingKeyColumnNames = drivingKeyColumnNames.ToArray();
      _drivingKeyColumnSignature = DefaultDataVaultSaveService.CreateOrdinalSignature(DrivingKeyColumnNames);
    }

    public string TableName { get; }

    public string ParentHashKeyColumnName { get; }

    public string HashDiffColumnName { get; }

    public string LoadTimestampColumnName { get; }

    public IReadOnlyList<string> DrivingKeyColumnNames { get; }

    public bool Equals(SatelliteTableProjection? other) {
      return other is not null &&
          string.Equals(TableName, other.TableName, StringComparison.Ordinal) &&
          string.Equals(ParentHashKeyColumnName, other.ParentHashKeyColumnName, StringComparison.Ordinal) &&
          string.Equals(HashDiffColumnName, other.HashDiffColumnName, StringComparison.Ordinal) &&
          string.Equals(LoadTimestampColumnName, other.LoadTimestampColumnName, StringComparison.Ordinal) &&
          string.Equals(_drivingKeyColumnSignature, other._drivingKeyColumnSignature, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) {
      return Equals(obj as SatelliteTableProjection);
    }

    public override int GetHashCode() {
      return HashCode.Combine(
          StringComparer.Ordinal.GetHashCode(TableName),
          StringComparer.Ordinal.GetHashCode(ParentHashKeyColumnName),
          StringComparer.Ordinal.GetHashCode(HashDiffColumnName),
          StringComparer.Ordinal.GetHashCode(LoadTimestampColumnName),
          StringComparer.Ordinal.GetHashCode(_drivingKeyColumnSignature));
    }
  }

  private sealed record SatelliteSavePlan(
      int Ordinal,
      SatelliteTableProjection Table,
      SatelliteSeriesKey SeriesKey,
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      Dictionary<string, object> Row,
      DataVaultSavedRecord SavedRecord);

  private sealed record FilteredSatelliteSavePlans(
      IReadOnlyList<SatelliteSavePlan> RowsToWrite,
      IReadOnlyList<SaveOperationResult> Results);

  private sealed record LatestSatelliteHashDiff(
      SatelliteSeriesKey SeriesKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp);

  private sealed class SatelliteSeriesKey : IEquatable<SatelliteSeriesKey> {
    private readonly string _drivingKeyValueSignature;

    public SatelliteSeriesKey(string parentHashKey, IEnumerable<string> drivingKeyValues) {
      ParentHashKey = parentHashKey;
      DrivingKeyValues = drivingKeyValues.ToArray();
      _drivingKeyValueSignature = DefaultDataVaultSaveService.CreateOrdinalSignature(DrivingKeyValues);
    }

    public string ParentHashKey { get; }

    public IReadOnlyList<string> DrivingKeyValues { get; }

    public bool Equals(SatelliteSeriesKey? other) {
      return other is not null &&
          string.Equals(ParentHashKey, other.ParentHashKey, StringComparison.Ordinal) &&
          string.Equals(_drivingKeyValueSignature, other._drivingKeyValueSignature, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) {
      return Equals(obj as SatelliteSeriesKey);
    }

    public override int GetHashCode() {
      return HashCode.Combine(
          StringComparer.Ordinal.GetHashCode(ParentHashKey),
          StringComparer.Ordinal.GetHashCode(_drivingKeyValueSignature));
    }
  }

  private static string CreateOrdinalSignature(IEnumerable<string> values) {
    return string.Concat(values.Select(value => value.Length.ToString(CultureInfo.InvariantCulture) + ":" + value));
  }
}
