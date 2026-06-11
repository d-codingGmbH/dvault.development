using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkSummary(
    string ScenarioName,
    string Provider,
    string BaselineName,
    string StrategyFamily,
    string DatasetSize,
    string ChangeRatio,
    string ExecutionStatus,
    string SkipReason,
    int Iterations,
    double? MeanMilliseconds,
    double? MinMilliseconds,
    double? MaxMilliseconds,
    double? MeanAllocatedBytes,
    long? MinAllocatedBytes,
    long? MaxAllocatedBytes,
    string ExecutionDetail,
    string PersistedOutcome) {
  public static BenchmarkSummary Create(
      string scenarioName,
      string providerName,
      string baselineName,
      string strategyFamily,
      string datasetSize,
      string changeRatio,
      IReadOnlyList<TimeSpan> elapsedTimes,
      IReadOnlyList<long> allocatedBytes,
      string persistedOutcome,
      string executionDetail) {
    ArgumentException.ThrowIfNullOrWhiteSpace(scenarioName);
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);
    ArgumentException.ThrowIfNullOrWhiteSpace(baselineName);
    ArgumentException.ThrowIfNullOrWhiteSpace(strategyFamily);
    ArgumentException.ThrowIfNullOrWhiteSpace(datasetSize);
    ArgumentException.ThrowIfNullOrWhiteSpace(changeRatio);
    ArgumentException.ThrowIfNullOrWhiteSpace(persistedOutcome);
    ArgumentException.ThrowIfNullOrWhiteSpace(executionDetail);
    ArgumentNullException.ThrowIfNull(elapsedTimes);
    ArgumentNullException.ThrowIfNull(allocatedBytes);

    if (elapsedTimes.Count == 0) {
      throw new ArgumentException("At least one benchmark iteration is required.", nameof(elapsedTimes));
    }

    if (allocatedBytes.Count != elapsedTimes.Count) {
      throw new ArgumentException("Allocation measurements must match benchmark iterations.", nameof(allocatedBytes));
    }

    return new BenchmarkSummary(
        scenarioName,
        providerName,
        baselineName,
        strategyFamily,
        datasetSize,
        changeRatio,
        BenchmarkExecutionStatus.Completed,
        string.Empty,
        elapsedTimes.Count,
        elapsedTimes.Average(value => value.TotalMilliseconds),
        elapsedTimes.Min(value => value.TotalMilliseconds),
        elapsedTimes.Max(value => value.TotalMilliseconds),
        allocatedBytes.Average(),
        allocatedBytes.Min(),
        allocatedBytes.Max(),
        executionDetail,
        persistedOutcome);
  }

  public static BenchmarkSummary CreateSkipped(
      IScenarioBenchmark benchmark,
      BenchmarkSkipReason skipReason) {
    ArgumentNullException.ThrowIfNull(benchmark);
    ArgumentNullException.ThrowIfNull(skipReason);

    return new BenchmarkSummary(
        benchmark.ScenarioName,
        benchmark.ProviderName,
        benchmark.BaselineName,
        benchmark.StrategyFamily,
        benchmark.DatasetSize,
        benchmark.ChangeRatio,
        BenchmarkExecutionStatus.Skipped,
        skipReason.DisplayText,
        0,
        null,
        null,
        null,
        null,
        null,
        null,
        BenchmarkExecutionDetails.CreatePlanned(benchmark),
        "not executed");
  }

  public static BenchmarkSummary CreateFailed(
      IScenarioBenchmark benchmark,
      Exception exception) {
    ArgumentNullException.ThrowIfNull(benchmark);
    ArgumentNullException.ThrowIfNull(exception);

    return new BenchmarkSummary(
        benchmark.ScenarioName,
        benchmark.ProviderName,
        benchmark.BaselineName,
        benchmark.StrategyFamily,
        benchmark.DatasetSize,
        benchmark.ChangeRatio,
        BenchmarkExecutionStatus.Failed,
        BenchmarkProviderDiagnostics.NormalizeExceptionMessage(exception),
        0,
        null,
        null,
        null,
        null,
        null,
        null,
        BenchmarkExecutionDetails.CreatePlanned(benchmark),
        "not executed");
  }
}
