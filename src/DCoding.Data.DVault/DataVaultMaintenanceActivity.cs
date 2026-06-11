using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

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
