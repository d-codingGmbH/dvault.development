using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultActivityTracing {
  public const string SourceName = "DCoding.Data.DVault";
  public const string ActivitySourceName = SourceName;

  public const string PitRebuildOperation = "dvault.maintenance.pit.rebuild";
  public const string PitMaintainParentsOperation = "dvault.maintenance.pit.maintain_parents";
  public const string BridgeRebuildOperation = "dvault.maintenance.bridge.rebuild";
  public const string BridgeMaintainIncrementalOperation = "dvault.maintenance.bridge.maintain_incremental";
  public const string SaveSingleRequestOperation = "dvault.save.single_request";
  public const string SaveBulkRequestOperation = "dvault.save.bulk_request";
  public const string SaveChunkedRequestOperation = "dvault.save.chunked_request";
  public const string ReadLatestSatelliteOperation = "dvault.read.latest_satellite";
  public const string ReadPitOperation = "dvault.read.pit";
  public const string ReadBridgeOperation = "dvault.read.bridge";

  public const string OperationTag = "dvault.operation";
  public const string ProviderTag = "dvault.provider";
  public const string OutcomeTag = "dvault.outcome";
  public const string FailureKindTag = "dvault.failure.kind";
  public const string FailureClassTag = "dvault.failure.class";
  public const string ExceptionTypeTag = "dvault.exception.type";
  public const string DurationBucketTag = "dvault.duration.bucket";
  public const string MaintenanceKindTag = "dvault.maintenance.kind";
  public const string ReadModelKindTag = "dvault.read_model.kind";
  public const string ParentKeyCountTag = "dvault.parent_key.count";
  public const string AffectedRowCountTag = "dvault.affected_row.count";
  public const string RebuildScopeTag = "dvault.rebuild.scope";
  public const string StrategyStatusTag = "dvault.strategy.status";
  public const string StrategyTypeTag = "dvault.strategy.type";
  public const string FallbackCauseTag = "dvault.fallback.cause";

  public const string MaintenanceNoOpEvent = "dvault.maintenance.noop";
  public const string FailureRecordedEvent = "dvault.failure.recorded";
  public const string StrategySelectedEvent = "dvault.strategy.selected";
  public const string FallbackRecordedEvent = "dvault.fallback.recorded";

  public const string ReadModeCurrent = "Current";
  public const string ReadModeAsOf = "AsOf";
  public const string ReadModeTraversal = "Traversal";

  public const string PitReadModelKind = "Pit";
  public const string BridgeReadModelKind = "Bridge";
  public const string PitRebuildMaintenanceKind = "PitRebuild";
  public const string PitMaintainParentsMaintenanceKind = "PitMaintainParents";
  public const string BridgeRebuildMaintenanceKind = "BridgeRebuild";
  public const string BridgeMaintainIncrementalMaintenanceKind = "BridgeMaintainIncremental";
  public const string FullRebuildScope = "Full";
  public const string ParentsRebuildScope = "Parents";
  public const string IncrementalRebuildScope = "Incremental";

  private const string SuccessOutcome = "success";
  private const string FaultOutcome = "fault";
  private const string CanceledOutcome = "canceled";
  private const string FaultFailureKind = "fault";
  private const string CancellationFailureKind = "cancellation";
  private const string ValidationFailureClass = "validation";
  private const string UnsupportedShapeFailureClass = "unsupported_shape";
  private const string ProviderFailureClass = "provider";
  private const string TimeoutFailureClass = "timeout";
  private const string CancellationFailureClass = "cancellation";
  private const string UnknownFailureClass = "unknown";

  private static readonly ActivitySource Source = new(SourceName);

  public static DataVaultMaintenanceActivity StartMaintenanceActivity(
      DbContext dbContext,
      string operation,
      string maintenanceKind,
      string readModelKind,
      string rebuildScope) {
    if (!Source.HasListeners()) {
      return default;
    }

    var activity = Source.StartActivity(operation, ActivityKind.Internal);
    if (activity is null) {
      return default;
    }

    activity.SetTag(OperationTag, operation);
    activity.SetTag(MaintenanceKindTag, maintenanceKind);
    activity.SetTag(ReadModelKindTag, readModelKind);
    activity.SetTag(RebuildScopeTag, rebuildScope);

    var providerName = TryGetProviderName(dbContext);
    if (!string.IsNullOrWhiteSpace(providerName)) {
      activity.SetTag(ProviderTag, providerName);
    }

    return new DataVaultMaintenanceActivity(activity, Stopwatch.GetTimestamp());
  }

  public static Activity? StartSaveActivity(DataVaultSaveTelemetryOperationKind operationKind) {
    return Source.StartActivity(GetSaveOperationName(operationKind), ActivityKind.Internal);
  }

  public static Activity? StartReadActivity(DataVaultReadTelemetryFamily family) {
    return Source.StartActivity(GetReadOperationName(family), ActivityKind.Internal);
  }

  public static async Task<IReadOnlyList<T>> TraceReadAsync<T>(
      DbContext dbContext,
      DataVaultReadTelemetryFamily family,
      string readMode,
      int requestedKeyCount,
      Func<Task<IReadOnlyList<T>>> read) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(read);

    using var activity = StartReadActivity(family);
    if (activity is null) {
      return await read().ConfigureAwait(false);
    }

    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultReadTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      var rows = await read().ConfigureAwait(false);
      CompleteReadActivity(
          activity,
          DataVaultTelemetrySummaryFactory.CreateReadSummary(
              family,
              DataVaultTelemetryOutcome.Succeeded,
              requestedKeyCount,
              rows.Count,
              DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
              strategySelection),
          readMode);

      return rows;
    }
    catch (Exception exception) {
      CompleteReadActivity(
          activity,
          DataVaultTelemetrySummaryFactory.CreateReadSummary(
              family,
              DataVaultTelemetryOutcome.Failed,
              requestedKeyCount,
              returnedRowCount: 0,
              DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
              strategySelection),
          readMode,
          exception);
      throw;
    }
  }

  public static void CompleteSaveActivity(
      Activity? activity,
      DataVaultSaveTelemetrySummary summary,
      Exception? exception = null) {
    if (activity is null) {
      return;
    }

    CompleteCommonActivity(
        activity,
        GetSaveOperationName(summary.OperationKind),
        summary.Duration,
        summary.ProviderName,
        summary.StrategyStatus.ToString(),
        summary.SelectedStrategyName,
        exception);

    if (!activity.IsAllDataRequested) {
      return;
    }

    activity.SetTag("dvault.save.mode", summary.OperationKind.ToString());
    activity.SetTag("dvault.request.count", summary.RequestCount);
    activity.SetTag("dvault.operation.count", summary.HubOperationCount + summary.LinkOperationCount + summary.SatelliteOperationCount);
    activity.SetTag("dvault.row.count", summary.RowsWritten);
    activity.SetTag("dvault.saved_record.count", summary.SavedRecordCount);

    if (summary.ChunkCount > 0) {
      activity.SetTag("dvault.chunk.count", summary.ChunkCount);
    }

    if (summary.ProcessedChunkCount > 0) {
      activity.SetTag("dvault.processed_chunk.count", summary.ProcessedChunkCount);
    }

    if (summary.RetainedStateHighWaterCount > 0) {
      activity.SetTag("dvault.retained_state.high_water", summary.RetainedStateHighWaterCount);
    }

    foreach (var fallbackCause in summary.FallbackCauseKinds) {
      RecordFallbackCause(activity, fallbackCause.ToString());
    }

    foreach (var fallbackCause in summary.ChunkedStateFallbackCauseKinds) {
      RecordFallbackCause(activity, fallbackCause.ToString());
    }

    foreach (var unsupportedShape in summary.UnsupportedShapeKinds) {
      activity.AddEvent(new ActivityEvent(
          "dvault.fallback.recorded",
          tags: new ActivityTagsCollection {
            ["dvault.unsupported_shape"] = unsupportedShape.ToString(),
          }));
    }
  }

  public static void CompleteReadActivity(
      Activity? activity,
      DataVaultReadTelemetrySummary summary,
      string readMode,
      Exception? exception = null) {
    if (activity is null) {
      return;
    }

    CompleteCommonActivity(
        activity,
        GetReadOperationName(summary.Family),
        summary.Duration,
        summary.ProviderName,
        summary.StrategyStatus.ToString(),
        summary.SelectedStrategyName,
        exception);

    if (!activity.IsAllDataRequested) {
      return;
    }

    activity.SetTag("dvault.read.family", summary.Family.ToString());
    activity.SetTag("dvault.read.mode", readMode);
    activity.SetTag("dvault.requested_key.count", summary.RequestedKeyCount);
    activity.SetTag("dvault.returned_row.count", summary.ReturnedRowCount);

    foreach (var fallbackCause in summary.FallbackCauseKinds) {
      RecordFallbackCause(activity, fallbackCause.ToString());
    }
  }

  public static string GetLatestSatelliteReadMode(DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return request.AsOf is null ? ReadModeCurrent : ReadModeAsOf;
  }

  private static string? TryGetProviderName(DbContext dbContext) {
    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
    catch (NotSupportedException) {
      return null;
    }
  }

  private static void CompleteCommonActivity(
      Activity activity,
      string operationName,
      TimeSpan duration,
      string? providerName,
      string strategyStatus,
      string? selectedStrategyName,
      Exception? exception) {
    activity.SetStatus(exception is null ? ActivityStatusCode.Ok : ActivityStatusCode.Error);

    if (!activity.IsAllDataRequested) {
      return;
    }

    activity.SetTag(OperationTag, operationName);
    activity.SetTag(OutcomeTag, GetOutcome(exception));
    activity.SetTag(DurationBucketTag, GetDurationBucket(duration));
    if (!string.IsNullOrWhiteSpace(providerName)) {
      activity.SetTag(ProviderTag, providerName);
    }

    activity.SetTag(StrategyStatusTag, strategyStatus);
    if (!string.IsNullOrWhiteSpace(selectedStrategyName)) {
      activity.SetTag(StrategyTypeTag, selectedStrategyName);
      activity.AddEvent(new ActivityEvent(
          StrategySelectedEvent,
          tags: new ActivityTagsCollection {
            [StrategyStatusTag] = strategyStatus,
            [StrategyTypeTag] = selectedStrategyName,
          }));
    }

    if (exception is null) {
      return;
    }

    var failureKind = GetFailureKind(exception);
    var failureClass = GetFailureClass(exception);
    var exceptionType = GetExceptionType(exception);
    activity.SetTag(FailureKindTag, failureKind);
    activity.SetTag(FailureClassTag, failureClass);
    activity.SetTag(ExceptionTypeTag, exceptionType);
    activity.AddEvent(new ActivityEvent(
        FailureRecordedEvent,
        tags: new ActivityTagsCollection {
          [FailureKindTag] = failureKind,
          [FailureClassTag] = failureClass,
          [ExceptionTypeTag] = exceptionType,
        }));
  }

  private static void RecordFallbackCause(Activity activity, string fallbackCause) {
    activity.AddEvent(new ActivityEvent(
        FallbackRecordedEvent,
        tags: new ActivityTagsCollection {
          [FallbackCauseTag] = fallbackCause,
        }));
  }

  private static string GetSaveOperationName(DataVaultSaveTelemetryOperationKind operationKind) {
    return operationKind switch {
      DataVaultSaveTelemetryOperationKind.SingleRequest => SaveSingleRequestOperation,
      DataVaultSaveTelemetryOperationKind.BulkRequest => SaveBulkRequestOperation,
      DataVaultSaveTelemetryOperationKind.ChunkedRequest => SaveChunkedRequestOperation,
      _ => throw new ArgumentOutOfRangeException(nameof(operationKind), operationKind, "Unknown Data Vault save operation kind."),
    };
  }

  private static string GetReadOperationName(DataVaultReadTelemetryFamily family) {
    return family switch {
      DataVaultReadTelemetryFamily.LatestSatellite => ReadLatestSatelliteOperation,
      DataVaultReadTelemetryFamily.Pit => ReadPitOperation,
      DataVaultReadTelemetryFamily.Bridge => ReadBridgeOperation,
      _ => throw new ArgumentOutOfRangeException(nameof(family), family, "Unknown Data Vault read telemetry family."),
    };
  }

  private static string GetOutcome(Exception? exception) {
    if (exception is null) {
      return SuccessOutcome;
    }

    return GetFailureOutcome(exception);
  }

  internal static string GetDurationBucket(TimeSpan duration) {
    if (duration < TimeSpan.FromMilliseconds(10)) {
      return "lt_10ms";
    }

    if (duration < TimeSpan.FromMilliseconds(100)) {
      return "10_99ms";
    }

    if (duration < TimeSpan.FromSeconds(1)) {
      return "100_999ms";
    }

    return duration < TimeSpan.FromSeconds(10) ? "1_9s" : "ge_10s";
  }

  internal static string GetFailureClass(Exception exception) {
    ArgumentNullException.ThrowIfNull(exception);

    if (exception is OperationCanceledException) {
      return CancellationFailureClass;
    }

    if (exception is TimeoutException || exception.InnerException is TimeoutException) {
      return TimeoutFailureClass;
    }

    if (exception is DbUpdateException || exception is DbException) {
      return ProviderFailureClass;
    }

    if (exception is NotSupportedException) {
      return UnsupportedShapeFailureClass;
    }

    if (exception is ArgumentException) {
      return ValidationFailureClass;
    }

    if (exception is InvalidOperationException invalidOperationException) {
      return ClassifyInvalidOperationFailure(invalidOperationException);
    }

    return UnknownFailureClass;
  }

  internal static string GetFailureKind(Exception exception) {
    ArgumentNullException.ThrowIfNull(exception);

    return exception is OperationCanceledException
        ? CancellationFailureKind
        : FaultFailureKind;
  }

  internal static string GetFailureOutcome(Exception exception) {
    ArgumentNullException.ThrowIfNull(exception);

    return exception is OperationCanceledException
        ? CanceledOutcome
        : FaultOutcome;
  }

  internal static string GetExceptionType(Exception exception) {
    ArgumentNullException.ThrowIfNull(exception);

    return exception.GetType().Name;
  }

  internal static string Success => SuccessOutcome;

  private static string ClassifyInvalidOperationFailure(InvalidOperationException exception) {
    return IsUnsupportedShapeInvalidOperation(exception.Message)
        ? UnsupportedShapeFailureClass
        : exception.InnerException is null
            ? ValidationFailureClass
            : ProviderFailureClass;
  }

  private static bool IsUnsupportedShapeInvalidOperation(string message) {
    return message.StartsWith("DVault bridge read failed:", StringComparison.Ordinal) ||
        message.StartsWith("DVault PIT read failed:", StringComparison.Ordinal) ||
        message.StartsWith("DVault typed bridge projection failed", StringComparison.Ordinal) ||
        message.StartsWith("DVault typed PIT projection failed", StringComparison.Ordinal) ||
        message.StartsWith("DVault typed satellite projection failed", StringComparison.Ordinal) ||
        message.Contains("unsupported", StringComparison.OrdinalIgnoreCase);
  }
}
