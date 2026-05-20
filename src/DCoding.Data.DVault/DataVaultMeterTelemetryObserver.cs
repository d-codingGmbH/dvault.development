using System.Diagnostics.Metrics;

namespace DCoding.Data.DVault;

/// <summary>
/// Records explicit DVault save and read telemetry through <see cref="Meter" /> counters and histograms.
/// </summary>
public sealed class DataVaultMeterTelemetryObserver : IDataVaultTelemetryObserver, IDisposable {
  /// <summary>
  /// Gets the built-in DVault meter name.
  /// </summary>
  public const string MeterName = "DCoding.Data.DVault";

  private const string None = "<none>";

  private readonly Counter<long> _readAttempts;
  private readonly Histogram<double> _readDuration;
  private readonly Counter<long> _readFallbackCauses;
  private readonly Histogram<long> _readRequestedKeys;
  private readonly Counter<long> _readReturnedRows;
  private readonly Counter<long> _saveAttempts;
  private readonly Histogram<double> _saveDuration;
  private readonly Counter<long> _saveFallbackCauses;
  private readonly Histogram<long> _saveOperationCount;
  private readonly Histogram<long> _saveRequestCount;
  private readonly Counter<long> _saveRowsWritten;
  private readonly Counter<long> _saveSavedRecords;
  private readonly Meter _meter;

  /// <summary>
  /// Initializes a new meter-backed DVault telemetry observer.
  /// </summary>
  public DataVaultMeterTelemetryObserver() {
    _meter = new Meter(MeterName);
    _saveAttempts = _meter.CreateCounter<long>(
        "dvault.save.attempts",
        unit: "{attempt}",
        description: "Counts explicit DVault save attempts.");
    _saveRowsWritten = _meter.CreateCounter<long>(
        "dvault.save.rows_written",
        unit: "{row}",
        description: "Counts rows written by successful explicit DVault save attempts.");
    _saveSavedRecords = _meter.CreateCounter<long>(
        "dvault.save.saved_records",
        unit: "{record}",
        description: "Counts saved-record summaries returned by successful explicit DVault save attempts.");
    _saveFallbackCauses = _meter.CreateCounter<long>(
        "dvault.save.fallback_causes",
        unit: "{cause}",
        description: "Counts distinct provider-neutral save fallback-cause kinds.");
    _saveDuration = _meter.CreateHistogram<double>(
        "dvault.save.duration",
        unit: "ms",
        description: "Records explicit DVault save attempt durations in milliseconds.");
    _saveRequestCount = _meter.CreateHistogram<long>(
        "dvault.save.request_count",
        unit: "{request}",
        description: "Records explicit save request counts per DVault save attempt.");
    _saveOperationCount = _meter.CreateHistogram<long>(
        "dvault.save.operation_count",
        unit: "{operation}",
        description: "Records hub, link, and satellite operation counts per DVault save attempt.");

    _readAttempts = _meter.CreateCounter<long>(
        "dvault.read.attempts",
        unit: "{attempt}",
        description: "Counts explicit DVault read attempts.");
    _readReturnedRows = _meter.CreateCounter<long>(
        "dvault.read.returned_rows",
        unit: "{row}",
        description: "Counts rows returned by successful explicit DVault read attempts.");
    _readFallbackCauses = _meter.CreateCounter<long>(
        "dvault.read.fallback_causes",
        unit: "{cause}",
        description: "Counts distinct provider-neutral read fallback-cause kinds.");
    _readDuration = _meter.CreateHistogram<double>(
        "dvault.read.duration",
        unit: "ms",
        description: "Records explicit DVault read attempt durations in milliseconds.");
    _readRequestedKeys = _meter.CreateHistogram<long>(
        "dvault.read.requested_keys",
        unit: "{key}",
        description: "Records requested parent or endpoint hash-key counts per DVault read attempt.");
  }

  /// <inheritdoc />
  public void RecordSave(DataVaultSaveTelemetrySummary summary) {
    ArgumentNullException.ThrowIfNull(summary);

    var tags = CreateSaveTags(summary);
    _saveAttempts.Add(1, tags);
    _saveDuration.Record(summary.Duration.TotalMilliseconds, tags);
    _saveRequestCount.Record(summary.RequestCount, tags);
    _saveOperationCount.Record(summary.OperationCount, tags);

    if (summary.Outcome == DataVaultTelemetryOutcome.Succeeded) {
      _saveRowsWritten.Add(summary.RowsWritten, tags);
      _saveSavedRecords.Add(summary.SavedRecordCount, tags);
    }

    foreach (var fallbackCauseKind in summary.FallbackCauseKinds) {
      _saveFallbackCauses.Add(1, AddTag(tags, "dvault.fallback_cause", fallbackCauseKind.ToString()));
    }
  }

  /// <inheritdoc />
  public void RecordRead(DataVaultReadTelemetrySummary summary) {
    ArgumentNullException.ThrowIfNull(summary);

    var tags = CreateReadTags(summary);
    _readAttempts.Add(1, tags);
    _readDuration.Record(summary.Duration.TotalMilliseconds, tags);
    _readRequestedKeys.Record(summary.RequestedKeyCount, tags);

    if (summary.Outcome == DataVaultTelemetryOutcome.Succeeded) {
      _readReturnedRows.Add(summary.ReturnedRowCount, tags);
    }

    foreach (var fallbackCauseKind in summary.FallbackCauseKinds) {
      _readFallbackCauses.Add(1, AddTag(tags, "dvault.fallback_cause", fallbackCauseKind.ToString()));
    }
  }

  /// <summary>
  /// Releases the underlying meter.
  /// </summary>
  public void Dispose() {
    _meter.Dispose();
  }

  private static KeyValuePair<string, object?>[] CreateSaveTags(DataVaultSaveTelemetrySummary summary) {
    return [
        new("dvault.operation", summary.OperationKind.ToString()),
        new("dvault.outcome", summary.Outcome.ToString()),
        new("dvault.strategy_status", summary.StrategyStatus.ToString()),
        new("dvault.provider", summary.ProviderName ?? None),
        new("dvault.selected_strategy", summary.SelectedStrategyName ?? None),
    ];
  }

  private static KeyValuePair<string, object?>[] CreateReadTags(DataVaultReadTelemetrySummary summary) {
    return [
        new("dvault.read_family", summary.Family.ToString()),
        new("dvault.outcome", summary.Outcome.ToString()),
        new("dvault.strategy_status", summary.StrategyStatus.ToString()),
        new("dvault.provider", summary.ProviderName ?? None),
        new("dvault.selected_strategy", summary.SelectedStrategyName ?? None),
    ];
  }

  private static KeyValuePair<string, object?>[] AddTag(
      KeyValuePair<string, object?>[] tags,
      string name,
      object? value) {
    var values = new KeyValuePair<string, object?>[tags.Length + 1];
    Array.Copy(tags, values, tags.Length);
    values[^1] = new KeyValuePair<string, object?>(name, value);

    return values;
  }
}
