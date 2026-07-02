using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultSaveService : IDataVaultSaveService {
  internal const int DefaultChunkedRetainedSatelliteSeriesLimit = 10000;

  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  private readonly int _chunkedRetainedSatelliteSeriesLimit;
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
      IEnumerable<IDataVaultTelemetryObserver> telemetryObservers)
      : this(
          stableHashService,
          stableHashNormalizer,
          loadTimestampResolvers,
          recordSourceResolvers,
          providerSaveStrategies,
          telemetryObservers,
          DefaultChunkedRetainedSatelliteSeriesLimit) {
  }

  internal DefaultDataVaultSaveService(
      IStableHashService stableHashService,
      IStableHashNormalizer stableHashNormalizer,
      IEnumerable<IDataVaultLoadTimestampResolver> loadTimestampResolvers,
      IEnumerable<IDataVaultRecordSourceResolver> recordSourceResolvers,
      IEnumerable<IDataVaultProviderSaveStrategy> providerSaveStrategies,
      IEnumerable<IDataVaultTelemetryObserver> telemetryObservers,
      int chunkedRetainedSatelliteSeriesLimit) {
    ArgumentNullException.ThrowIfNull(stableHashService);
    ArgumentNullException.ThrowIfNull(stableHashNormalizer);
    ArgumentNullException.ThrowIfNull(loadTimestampResolvers);
    ArgumentNullException.ThrowIfNull(recordSourceResolvers);
    ArgumentNullException.ThrowIfNull(providerSaveStrategies);
    ArgumentNullException.ThrowIfNull(telemetryObservers);
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chunkedRetainedSatelliteSeriesLimit);

    _stableHashService = stableHashService;
    _stableHashNormalizer = stableHashNormalizer;
    _chunkedRetainedSatelliteSeriesLimit = chunkedRetainedSatelliteSeriesLimit;
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

    return await SaveChunkedRequestsAsync(dbContext, request, cancellationToken).ConfigureAwait(false);
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      IAsyncEnumerable<DataVaultSaveChunk> chunks,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(chunks);

    return await SaveChunkedRequestsAsync(dbContext, chunks, cancellationToken).ConfigureAwait(false);
  }

  private async Task<DataVaultSaveResult> SaveRequestsAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      DataVaultSaveTelemetryOperationKind operationKind,
      CancellationToken cancellationToken) {
    using var activity = DataVaultActivityTracing.StartSaveActivity(operationKind);
    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultSaveTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      var result = await SaveRequestsCoreAsync(
          dbContext,
          requests,
          continuityState: null,
          selection => strategySelection = selection,
          cancellationToken).ConfigureAwait(false);

      var summary = DataVaultTelemetrySummaryFactory.CreateSaveSummary(
          operationKind,
          DataVaultTelemetryOutcome.Succeeded,
          requests,
          result,
          DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
          strategySelection);
      DataVaultActivityTracing.CompleteSaveActivity(activity, summary);
      DataVaultTelemetryDispatcher.RecordSave(
          _telemetryObservers,
          summary);

      return result;
    }
    catch (OperationCanceledException exception) {
      var summary = DataVaultTelemetrySummaryFactory.CreateSaveSummary(
          operationKind,
          DataVaultTelemetryOutcome.Failed,
          requests,
          result: null,
          DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
          strategySelection);
      DataVaultActivityTracing.CompleteSaveActivity(activity, summary, exception);
      DataVaultTelemetryDispatcher.RecordSave(
          _telemetryObservers,
          summary);
      throw;
    }
    catch (Exception exception) {
      var summary = DataVaultTelemetrySummaryFactory.CreateSaveSummary(
          operationKind,
          DataVaultTelemetryOutcome.Failed,
          requests,
          result: null,
          DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
          strategySelection);
      DataVaultActivityTracing.CompleteSaveActivity(activity, summary, exception);
      DataVaultTelemetryDispatcher.RecordSave(
          _telemetryObservers,
          summary);
      throw;
    }
  }

  private async Task<DataVaultSaveResult> SaveChunkedRequestsAsync(
      DbContext dbContext,
      DataVaultChunkedSaveRequest request,
      CancellationToken cancellationToken) {
    return await SaveChunkedRequestsCoreAsync(
        dbContext,
        async state => {
          foreach (var chunk in request.Chunks) {
            await ProcessSaveChunkAsync(dbContext, state, chunk, nameof(request), cancellationToken).ConfigureAwait(false);
          }
        },
        cancellationToken).ConfigureAwait(false);
  }

  private async Task<DataVaultSaveResult> SaveChunkedRequestsAsync(
      DbContext dbContext,
      IAsyncEnumerable<DataVaultSaveChunk> chunks,
      CancellationToken cancellationToken) {
    return await SaveChunkedRequestsCoreAsync(
        dbContext,
        async state => {
          await foreach (var chunk in chunks.WithCancellation(cancellationToken).ConfigureAwait(false)) {
            await ProcessSaveChunkAsync(dbContext, state, chunk, nameof(chunks), cancellationToken).ConfigureAwait(false);
          }
        },
        cancellationToken).ConfigureAwait(false);
  }

  private async Task<DataVaultSaveResult> SaveChunkedRequestsCoreAsync(
      DbContext dbContext,
      Func<ChunkedSaveExecutionState, Task> processChunksAsync,
      CancellationToken cancellationToken) {
    using var activity = DataVaultActivityTracing.StartSaveActivity(DataVaultSaveTelemetryOperationKind.ChunkedRequest);
    var stopwatch = Stopwatch.StartNew();
    var state = new ChunkedSaveExecutionState(
        new ChunkedSaveStrategySelection(DataVaultTelemetryStrategySelector.GetProviderName(dbContext)),
        new ChunkedSaveContinuityState(_chunkedRetainedSatelliteSeriesLimit));
    DataVaultSaveResult? result = null;
    Exception? failure = null;

    try {
      await processChunksAsync(state).ConfigureAwait(false);

      result = state.ToResult();
      return result;
    }
    catch (Exception exception) {
      failure = exception;
      throw;
    }
    finally {
      state.ContinuityState.Release();
      var summary = DataVaultTelemetrySummaryFactory.CreateSaveSummary(
          DataVaultSaveTelemetryOperationKind.ChunkedRequest,
          result is null ? DataVaultTelemetryOutcome.Failed : DataVaultTelemetryOutcome.Succeeded,
          state.Counts.ToTelemetryCounts(),
          result,
          DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
          state.StrategySelection.ToTelemetrySelection(),
          new DataVaultChunkedSaveTelemetryState(
              state.Counts.ChunkCount,
              state.Counts.ProcessedChunkCount,
              state.ContinuityState.CurrentCount,
              state.ContinuityState.HighWaterCount,
              state.ContinuityState.FallbackCauseKinds,
              state.ContinuityState.UnsupportedShapeKinds));
      DataVaultActivityTracing.CompleteSaveActivity(activity, summary, failure);
      DataVaultTelemetryDispatcher.RecordSave(
          _telemetryObservers,
          summary);
    }
  }

  private async Task ProcessSaveChunkAsync(
      DbContext dbContext,
      ChunkedSaveExecutionState state,
      DataVaultSaveChunk? chunk,
      string parameterName,
      CancellationToken cancellationToken) {
    state.Counts.ChunkCount++;
    if (chunk is null) {
      throw new ArgumentException("Data Vault chunked save requests must not contain null chunks.", parameterName);
    }

    cancellationToken.ThrowIfCancellationRequested();
    if (chunk.Requests.Count == 0) {
      return;
    }

    state.Counts.ProcessedChunkCount++;
    state.Counts.Add(DataVaultTelemetrySummaryFactory.CountSaveRequests(chunk.Requests));

    var chunkResult = await SaveRequestsCoreAsync(
        dbContext,
        chunk.Requests,
        state.ContinuityState,
        state.StrategySelection.Observe,
        cancellationToken).ConfigureAwait(false);

    state.RowsWritten += chunkResult.RowsWritten;
    foreach (var savedRecord in chunkResult.SavedRecords) {
      if (savedRecord.Kind == DataVaultTableKind.Satellite) {
        state.SatelliteSavedRecords.Add(savedRecord);
      }
      else {
        state.UniqueSavedRecords.Add(savedRecord);
      }
    }
  }

  private async Task<DataVaultSaveResult> SaveRequestsCoreAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      ChunkedSaveContinuityState? continuityState,
      Action<DataVaultSaveTelemetryStrategySelection> observeStrategySelection,
      CancellationToken cancellationToken) {
    var resolvedRequests = DataVaultAllocationProfiler.Measure(
        "pre-write save preparation",
        "DefaultDataVaultSaveService.ResolveRequests",
        () => ResolveRequests(requests));
    var strategySelection = DataVaultAllocationProfiler.Measure(
        "pre-write save preparation",
        "DataVaultTelemetryStrategySelector.SelectSaveStrategy",
        () => DataVaultTelemetryStrategySelector.SelectSaveStrategy(dbContext, _providerSaveStrategies, requests));
    observeStrategySelection(strategySelection);

    if (strategySelection.Strategy is not null) {
      var context = new DataVaultProviderSaveStrategyContext(
          dbContext,
          requests,
          resolvedRequests,
          _stableHashService,
          _stableHashNormalizer);
      return await strategySelection.Strategy.SaveAsync(context, cancellationToken).ConfigureAwait(false);
    }

    return await SaveProviderNeutralAsync(dbContext, resolvedRequests, continuityState, cancellationToken).ConfigureAwait(false);
  }

  private async Task<DataVaultSaveResult> SaveProviderNeutralAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultResolvedSaveRequest> resolvedRequests,
      ChunkedSaveContinuityState? continuityState,
      CancellationToken cancellationToken) {
    var savedRecords = new List<DataVaultSavedRecord>();
    var rowsWritten = 0;
    var uniquePlans = DataVaultAllocationProfiler.Measure(
        "pre-write save preparation",
        "DefaultDataVaultSaveService.CreateUniqueRowSavePlans",
        () => CreateUniqueRowSavePlans(resolvedRequests));
    var uniqueResults = await DataVaultAllocationProfiler.MeasureAsync(
        "pre-write save preparation",
        "DefaultDataVaultSaveService.AddUniqueRowsAsync",
        () => AddUniqueRowsAsync(
            dbContext,
            uniquePlans,
            cancellationToken)).ConfigureAwait(false);

    foreach (var result in uniqueResults) {
      savedRecords.Add(result.SavedRecord);
      if (result.RowWritten) {
        rowsWritten++;
      }
    }

    var satelliteResults = await DataVaultAllocationProfiler.MeasureAsync(
        "pre-write save preparation",
        "DefaultDataVaultSaveService.AddSatellitesAsync",
        () => AddSatellitesAsync(
            dbContext,
            resolvedRequests,
            continuityState,
            cancellationToken)).ConfigureAwait(false);
    foreach (var result in satelliteResults) {
      savedRecords.Add(result.SavedRecord);
      if (result.RowWritten) {
        rowsWritten++;
      }
    }

    await DataVaultAllocationProfiler.MeasureAsync(
        "database write boundary",
        "DbContext.SaveChangesAsync",
        () => dbContext.SaveChangesAsync(cancellationToken)).ConfigureAwait(false);

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
    var dependentChildKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        link.DependentChildKeys.Select(column => column.ColumnName),
        [linkHashKeyColumnName, loadTimestampColumnName, recordSourceColumnName, .. participantHashKeyColumnNames]);
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
    var linkHashKey = ComputeHash(participantHashKeyFields.Concat(dependentChildKeyFields));
    var row = new Dictionary<string, object> {
      [linkHashKeyColumnName] = linkHashKey,
      [loadTimestampColumnName] = request.LoadTimestamp,
      [recordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < participantHashKeyFields.Length; index++) {
      row.Add(participantHashKeyColumnNames[index], participantHashKeyFields[index].Value);
    }

    for (var index = 0; index < dependentChildKeyFields.Length; index++) {
      row.Add(dependentChildKeyColumnNames[index], dependentChildKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(tableName, linkHashKeyColumnName),
        linkHashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, tableName, linkHashKey, [], dependentChildKeyFields),
        Ordinal: -1);
  }

  private async Task<IReadOnlyList<SaveOperationResult>> AddSatellitesAsync(
      DbContext dbContext,
      IReadOnlyList<DataVaultResolvedSaveRequest> requests,
      ChunkedSaveContinuityState? continuityState,
      CancellationToken cancellationToken) {
    var plans = DataVaultAllocationProfiler.Measure(
        "pre-write save preparation",
        "DefaultDataVaultSaveService.CreateSatelliteSavePlans",
        () => CreateSatelliteSavePlans(requests));
    var filteredPlans = await DataVaultAllocationProfiler.MeasureAsync(
        "satellite latest-hash-diff replay filtering",
        "DefaultDataVaultSaveService.FilterSatellitePlansAsync",
        () => FilterSatellitePlansAsync(
            dbContext,
            plans,
            continuityState,
            cancellationToken)).ConfigureAwait(false);

    DataVaultAllocationProfiler.Measure(
        "pre-write save preparation",
        "DefaultDataVaultSaveService.StageSatelliteRows",
        () => {
          var rowSetsByTable = new Dictionary<SatelliteTableProjection, DbSet<Dictionary<string, object>>>();
          foreach (var plan in filteredPlans.RowsToWrite) {
            if (!rowSetsByTable.TryGetValue(plan.Table, out var rows)) {
              rows = dbContext.Set<Dictionary<string, object>>(plan.Table.TableName);
              rowSetsByTable.Add(plan.Table, rows);
            }

            ApplyModelValueFormats(dbContext, plan.Table.TableName, plan.Row);
            rows.Add(plan.Row);
          }

          return true;
        });

    return filteredPlans.Results;
  }

  private static IReadOnlyList<SatelliteSavePlan> CreateSatelliteSavePlans(IReadOnlyList<DataVaultResolvedSaveRequest> requests) {
    var planCount = 0;
    foreach (var request in requests) {
      planCount += request.Request.SatelliteOperations.Count;
    }

    var plans = new SatelliteSavePlan[planCount];
    var ordinal = 0;
    foreach (var request in requests) {
      foreach (var operation in request.Request.SatelliteOperations) {
        plans[ordinal] = CreateSatelliteSavePlan(ordinal, request, operation);
        ordinal++;
      }
    }

    return plans;
  }

  private static IReadOnlyList<SatellitePlanTableGroup> GroupSatellitePlansByTable(
      IReadOnlyList<SatelliteSavePlan> plans) {
    var groups = new List<SatellitePlanTableGroup>();
    var groupsByTable = new Dictionary<SatelliteTableProjection, SatellitePlanTableGroup>();

    foreach (var plan in plans) {
      if (!groupsByTable.TryGetValue(plan.Table, out var group)) {
        group = new SatellitePlanTableGroup(plan.Table);
        groupsByTable.Add(plan.Table, group);
        groups.Add(group);
      }

      group.Add(plan);
    }

    return groups;
  }

  private static async Task<FilteredSatelliteSavePlans> FilterSatellitePlansAsync(
      DbContext dbContext,
      IReadOnlyList<SatelliteSavePlan> plans,
      ChunkedSaveContinuityState? continuityState,
      CancellationToken cancellationToken) {
    var results = new SaveOperationResult[plans.Count];
    var rowsToWrite = new List<SatelliteSavePlan>(plans.Count);

    foreach (var group in GroupSatellitePlansByTable(plans)) {
      var latestHashDiffs = await DataVaultAllocationProfiler.MeasureAsync(
          "satellite latest-hash-diff replay filtering",
          "DefaultDataVaultSaveService.LoadLatestSatelliteHashDiffsAsync",
          () => LoadLatestSatelliteHashDiffsAsync(
              dbContext,
              group.Table,
              group.ParentHashKeys,
              cancellationToken)).ConfigureAwait(false);
      DataVaultAllocationProfiler.Measure(
          "satellite latest-hash-diff replay filtering",
          "ChunkedSaveContinuityState.ApplyRetainedState",
          () => {
            continuityState?.ApplyRetainedState(group.Table, latestHashDiffs);
            return true;
          });

      foreach (var plan in group.Plans) {
        var rowWritten = ShouldWriteSatelliteRow(latestHashDiffs, plan);
        if (rowWritten) {
          rowsToWrite.Add(plan);
          TrackLatestSatelliteHashDiff(latestHashDiffs, plan);
          DataVaultAllocationProfiler.Measure(
              "satellite latest-hash-diff replay filtering",
              "ChunkedSaveContinuityState.TrackLatestSatelliteHashDiff",
              () => {
                continuityState?.TrackLatestSatelliteHashDiff(group.Table, plan);
                return true;
              });
        }

        results[plan.Ordinal] = new SaveOperationResult(plan.SavedRecord, rowWritten);
      }
    }

    return new FilteredSatelliteSavePlans(rowsToWrite, results);
  }

  private static SatelliteSavePlan CreateSatelliteSavePlan(
      int ordinal,
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
        ordinal,
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
      IReadOnlySet<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var latestByParent = GetLatestTrackedSatelliteHashDiffs(dbContext, table, parentHashKeys);
    await LoadLatestPersistedSatelliteHashDiffsAsync(
        dbContext,
        table,
        parentHashKeys,
        latestByParent,
        cancellationToken).ConfigureAwait(false);

    return latestByParent;
  }

  private static Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff> GetLatestTrackedSatelliteHashDiffs(
      DbContext dbContext,
      SatelliteTableProjection table,
      IReadOnlySet<string> parentHashKeys) {
    var latestBySeries = new Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff>();

    foreach (var trackedRow in GetTrackedRows(dbContext, table.TableName)) {
      if (!TryCreateLatestSatelliteHashDiff(trackedRow, table, out var current) ||
          !parentHashKeys.Contains(current.SeriesKey.ParentHashKey)) {
        continue;
      }

      if (!latestBySeries.TryGetValue(current.SeriesKey, out var previous) ||
          current.LoadTimestamp > previous.LoadTimestamp) {
        latestBySeries[current.SeriesKey] = current;
      }
    }

    return latestBySeries;
  }

  private static async Task LoadLatestPersistedSatelliteHashDiffsAsync(
      DbContext dbContext,
      SatelliteTableProjection table,
      IReadOnlyCollection<string> parentHashKeys,
      Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff> latestBySeries,
      CancellationToken cancellationToken) {
    if (parentHashKeys.Count == 0) {
      return;
    }

    var rows = dbContext.Set<Dictionary<string, object>>(table.TableName);
    var parentHashKeyBatch = new List<string>(Math.Min(parentHashKeys.Count, 500));

    foreach (var parentHashKey in parentHashKeys) {
      parentHashKeyBatch.Add(parentHashKey);
      if (parentHashKeyBatch.Count == 500) {
        await LoadLatestPersistedSatelliteHashDiffBatchAsync(
            rows,
            table,
            parentHashKeyBatch,
            latestBySeries,
            cancellationToken).ConfigureAwait(false);
        parentHashKeyBatch.Clear();
      }
    }

    if (parentHashKeyBatch.Count > 0) {
      await LoadLatestPersistedSatelliteHashDiffBatchAsync(
          rows,
          table,
          parentHashKeyBatch,
          latestBySeries,
          cancellationToken).ConfigureAwait(false);
    }
  }

  private static async Task LoadLatestPersistedSatelliteHashDiffBatchAsync(
      DbSet<Dictionary<string, object>> rows,
      SatelliteTableProjection table,
      IReadOnlyCollection<string> parentHashKeys,
      Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff> latestBySeries,
      CancellationToken cancellationToken) {
    var persistedRows = await rows
        .AsNoTracking()
        .WhereStringPropertyEqualsAny(table.ParentHashKeyColumnName, parentHashKeys)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    foreach (var persistedRow in persistedRows) {
      if (!TryCreateLatestSatelliteHashDiff(persistedRow, table, out var latestHashDiff)) {
        continue;
      }

      if (!latestBySeries.TryGetValue(latestHashDiff.SeriesKey, out var current) ||
          latestHashDiff.LoadTimestamp > current.LoadTimestamp) {
        latestBySeries[latestHashDiff.SeriesKey] = latestHashDiff;
      }
    }
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
    if (table.DrivingKeyColumnNames.Count == 0) {
      drivingKeyValues = [];
      return true;
    }

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

  private sealed class ChunkedSaveExecutionState(
      ChunkedSaveStrategySelection strategySelection,
      ChunkedSaveContinuityState continuityState) {
    public List<DataVaultSavedRecord> UniqueSavedRecords { get; } = [];

    public List<DataVaultSavedRecord> SatelliteSavedRecords { get; } = [];

    public int RowsWritten { get; set; }

    public SaveAttemptCounts Counts { get; } = new();

    public ChunkedSaveStrategySelection StrategySelection { get; } = strategySelection;

    public ChunkedSaveContinuityState ContinuityState { get; } = continuityState;

    public DataVaultSaveResult ToResult() {
      return new DataVaultSaveResult(RowsWritten, UniqueSavedRecords.Concat(SatelliteSavedRecords));
    }
  }

  private sealed class SaveAttemptCounts {
    private int _hubOperationCount;
    private int _linkOperationCount;
    private int _requestCount;
    private int _satelliteOperationCount;

    public int ChunkCount { get; set; }

    public int ProcessedChunkCount { get; set; }

    public void Add(DataVaultSaveTelemetryCounts counts) {
      _requestCount += counts.RequestCount;
      _hubOperationCount += counts.HubOperationCount;
      _linkOperationCount += counts.LinkOperationCount;
      _satelliteOperationCount += counts.SatelliteOperationCount;
    }

    public DataVaultSaveTelemetryCounts ToTelemetryCounts() {
      return new DataVaultSaveTelemetryCounts(
          _requestCount,
          _hubOperationCount,
          _linkOperationCount,
          _satelliteOperationCount);
    }
  }

  private sealed class ChunkedSaveStrategySelection(string? providerName) {
    private readonly List<DataVaultSaveStrategyFallbackCauseKind> _fallbackCauseKinds = [];
    private bool _mixedSelectedStrategies;
    private string? _selectedStrategyName;
    private DataVaultStagedProviderBulkDiagnostics? _stagedProviderBulk;
    private DataVaultSaveStrategyDiagnosticsStatus _status = DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated;

    public void Observe(DataVaultSaveTelemetryStrategySelection selection) {
      _stagedProviderBulk ??= selection.StagedProviderBulk;
      if (selection.Status == DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback) {
        _status = DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback;
        _fallbackCauseKinds.AddRange(selection.FallbackCauseKinds);
        return;
      }

      if (selection.Status != DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected ||
          _status == DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback) {
        return;
      }

      _status = DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected;
      if (_selectedStrategyName is null) {
        _selectedStrategyName = selection.SelectedStrategyName;
      }
      else if (!string.Equals(_selectedStrategyName, selection.SelectedStrategyName, StringComparison.Ordinal)) {
        _mixedSelectedStrategies = true;
      }
    }

    public DataVaultSaveTelemetryStrategySelection ToTelemetrySelection() {
      return new DataVaultSaveTelemetryStrategySelection(
          Strategy: null,
          _status,
          providerName,
          _status == DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected && !_mixedSelectedStrategies
              ? _selectedStrategyName
              : null,
          _fallbackCauseKinds.Distinct().ToArray(),
          _stagedProviderBulk);
    }
  }

  private sealed class ChunkedSaveContinuityState {
    private readonly List<DataVaultChunkedSaveStateFallbackCauseKind> _fallbackCauseKinds = [];
    private readonly int _maximumRetainedSeriesCount;
    private readonly Dictionary<SatelliteTableProjection, Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff>> _tables = [];
    private readonly List<DataVaultChunkedSaveUnsupportedShapeKind> _unsupportedShapeKinds = [];

    public ChunkedSaveContinuityState(int maximumRetainedSeriesCount) {
      ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumRetainedSeriesCount);

      _maximumRetainedSeriesCount = maximumRetainedSeriesCount;
    }

    public int CurrentCount { get; private set; }

    public int HighWaterCount { get; private set; }

    public IReadOnlyList<DataVaultChunkedSaveStateFallbackCauseKind> FallbackCauseKinds =>
        _fallbackCauseKinds.Distinct().ToArray();

    public IReadOnlyList<DataVaultChunkedSaveUnsupportedShapeKind> UnsupportedShapeKinds =>
        _unsupportedShapeKinds.Distinct().ToArray();

    public void ApplyRetainedState(
        SatelliteTableProjection table,
        Dictionary<SatelliteSeriesKey, LatestSatelliteHashDiff> latestHashDiffs) {
      if (!_tables.TryGetValue(table, out var retainedRows) || retainedRows.Count == 0) {
        return;
      }

      foreach (var retainedRow in retainedRows.Values) {
        if (!latestHashDiffs.TryGetValue(retainedRow.SeriesKey, out var current) ||
            retainedRow.LoadTimestamp > current.LoadTimestamp) {
          latestHashDiffs[retainedRow.SeriesKey] = retainedRow;
        }
      }
    }

    public void TrackLatestSatelliteHashDiff(SatelliteTableProjection table, SatelliteSavePlan plan) {
      if (!_tables.TryGetValue(table, out var retainedRows)) {
        retainedRows = [];
        _tables.Add(table, retainedRows);
      }

      var isNewSeries = !retainedRows.ContainsKey(plan.SeriesKey);
      if (isNewSeries && CurrentCount >= _maximumRetainedSeriesCount) {
        RecordLimitFallback();
        Release();
        retainedRows = [];
        _tables.Add(table, retainedRows);
        isNewSeries = true;
      }

      if (!retainedRows.TryGetValue(plan.SeriesKey, out var latestHashDiff) ||
          plan.LoadTimestamp >= latestHashDiff.LoadTimestamp) {
        retainedRows[plan.SeriesKey] = new LatestSatelliteHashDiff(
            plan.SeriesKey,
            plan.HashDiff,
            plan.LoadTimestamp);
      }

      if (isNewSeries) {
        CurrentCount++;
        HighWaterCount = Math.Max(HighWaterCount, CurrentCount);
      }
    }

    public void Release() {
      _tables.Clear();
      CurrentCount = 0;
    }

    private void RecordLimitFallback() {
      _fallbackCauseKinds.Add(DataVaultChunkedSaveStateFallbackCauseKind.RetainedSatelliteSeriesLimitReached);
      _unsupportedShapeKinds.Add(DataVaultChunkedSaveUnsupportedShapeKind.RetainedSatelliteSeriesLimitExceeded);
    }
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
      DrivingKeyColumnNames = drivingKeyColumnNames is IReadOnlyCollection<string> { Count: 0 }
          ? []
          : drivingKeyColumnNames.ToArray();
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

  private sealed class SatellitePlanTableGroup(SatelliteTableProjection table) {
    public SatelliteTableProjection Table { get; } = table;

    public List<SatelliteSavePlan> Plans { get; } = [];

    public HashSet<string> ParentHashKeys { get; } = new(StringComparer.Ordinal);

    public void Add(SatelliteSavePlan plan) {
      Plans.Add(plan);
      ParentHashKeys.Add(plan.ParentHashKey);
    }
  }

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
      DrivingKeyValues = drivingKeyValues is IReadOnlyCollection<string> { Count: 0 }
          ? []
          : drivingKeyValues.ToArray();
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
    if (values is IReadOnlyCollection<string> collection && collection.Count == 0) {
      return string.Empty;
    }

    var builder = new StringBuilder();
    foreach (var value in values) {
      builder.Append(value.Length.ToString(CultureInfo.InvariantCulture));
      builder.Append(':');
      builder.Append(value);
    }

    return builder.ToString();
  }
}
