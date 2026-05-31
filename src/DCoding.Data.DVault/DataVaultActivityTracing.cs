using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultActivityTracing {
  public const string ActivitySourceName = "DCoding.Data.DVault";
  public const string PitRebuildOperation = "dvault.maintenance.pit.rebuild";
  public const string PitMaintainParentsOperation = "dvault.maintenance.pit.maintain_parents";
  public const string BridgeRebuildOperation = "dvault.maintenance.bridge.rebuild";
  public const string BridgeMaintainIncrementalOperation = "dvault.maintenance.bridge.maintain_incremental";

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

  public const string MaintenanceNoOpEvent = "dvault.maintenance.noop";
  public const string FailureRecordedEvent = "dvault.failure.recorded";

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

  private static readonly ActivitySource Source = new(ActivitySourceName);

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

    if (duration < TimeSpan.FromSeconds(10)) {
      return "1_9s";
    }

    return "ge_10s";
  }

  internal static string GetFailureClass(Exception exception) {
    if (exception is OperationCanceledException) {
      return CancellationFailureClass;
    }

    if (exception is TimeoutException || exception.InnerException is TimeoutException) {
      return TimeoutFailureClass;
    }

    if (exception is NotSupportedException) {
      return UnsupportedShapeFailureClass;
    }

    if (exception is ArgumentException) {
      return ValidationFailureClass;
    }

    if (exception is DbUpdateException || exception is DbUpdateConcurrencyException) {
      return ProviderFailureClass;
    }

    if (exception is InvalidOperationException &&
        exception.Message.StartsWith("DVault ", StringComparison.Ordinal)) {
      return exception.InnerException is null
          ? ValidationFailureClass
          : ProviderFailureClass;
    }

    return UnknownFailureClass;
  }

  internal static string GetFailureKind(Exception exception) {
    return exception is OperationCanceledException
        ? CancellationFailureKind
        : FaultFailureKind;
  }

  internal static string GetFailureOutcome(Exception exception) {
    return exception is OperationCanceledException
        ? CanceledOutcome
        : FaultOutcome;
  }

  internal static string GetExceptionType(Exception exception) {
    return exception.GetType().Name;
  }

  internal static string Success => SuccessOutcome;
}

internal readonly struct DataVaultMaintenanceActivity : IDisposable {
  private readonly Activity? _activity;
  private readonly long _startTimestamp;

  public DataVaultMaintenanceActivity(Activity activity, long startTimestamp) {
    _activity = activity;
    _startTimestamp = startTimestamp;
  }

  public void RecordSuccess(
      int affectedRowCount,
      int? parentKeyCount,
      bool isNoOp) {
    if (_activity is null) {
      return;
    }

    _activity.SetStatus(ActivityStatusCode.Ok);
    _activity.SetTag(DataVaultActivityTracing.OutcomeTag, DataVaultActivityTracing.Success);
    _activity.SetTag(DataVaultActivityTracing.AffectedRowCountTag, affectedRowCount);
    _activity.SetTag(
        DataVaultActivityTracing.DurationBucketTag,
        DataVaultActivityTracing.GetDurationBucket(Stopwatch.GetElapsedTime(_startTimestamp)));

    if (parentKeyCount.HasValue) {
      _activity.SetTag(DataVaultActivityTracing.ParentKeyCountTag, parentKeyCount.Value);
    }

    if (isNoOp && _activity.IsAllDataRequested) {
      _activity.AddEvent(new ActivityEvent(DataVaultActivityTracing.MaintenanceNoOpEvent));
    }
  }

  public void RecordFailure(Exception exception) {
    if (_activity is null) {
      return;
    }

    var failureKind = DataVaultActivityTracing.GetFailureKind(exception);
    var failureClass = DataVaultActivityTracing.GetFailureClass(exception);
    var exceptionType = DataVaultActivityTracing.GetExceptionType(exception);

    _activity.SetStatus(ActivityStatusCode.Error);
    _activity.SetTag(DataVaultActivityTracing.OutcomeTag, DataVaultActivityTracing.GetFailureOutcome(exception));
    _activity.SetTag(DataVaultActivityTracing.FailureKindTag, failureKind);
    _activity.SetTag(DataVaultActivityTracing.FailureClassTag, failureClass);
    _activity.SetTag(DataVaultActivityTracing.ExceptionTypeTag, exceptionType);
    _activity.SetTag(
        DataVaultActivityTracing.DurationBucketTag,
        DataVaultActivityTracing.GetDurationBucket(Stopwatch.GetElapsedTime(_startTimestamp)));

    if (!_activity.IsAllDataRequested) {
      return;
    }

    var tags = new ActivityTagsCollection {
      { DataVaultActivityTracing.FailureKindTag, failureKind },
      { DataVaultActivityTracing.FailureClassTag, failureClass },
      { DataVaultActivityTracing.ExceptionTypeTag, exceptionType },
    };
    _activity.AddEvent(new ActivityEvent(DataVaultActivityTracing.FailureRecordedEvent, default, tags));
  }

  public void Dispose() {
    _activity?.Dispose();
  }
}
