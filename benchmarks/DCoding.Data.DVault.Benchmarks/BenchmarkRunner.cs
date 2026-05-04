using System.Diagnostics;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkRunner {
  private static readonly IScenarioBenchmark[] Benchmarks =
  [
      new CustomerProfilePlainEfBenchmark(),
      new CustomerProfileDataVaultBenchmark(DataVaultBenchmarkStrategy.ProviderNeutralFallback),
      new CustomerProfileDataVaultBenchmark(DataVaultBenchmarkStrategy.SqliteOptimized),
      new CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarios.InsertOnly),
      new CustomerProfileBulkDataVaultBenchmark(
          CustomerProfileBulkScenarios.InsertOnly,
          DataVaultBenchmarkStrategy.ProviderNeutralFallback),
      new CustomerProfileBulkDataVaultBenchmark(
          CustomerProfileBulkScenarios.InsertOnly,
          DataVaultBenchmarkStrategy.SqliteOptimized),
      new CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarios.ChangeHeavy),
      new CustomerProfileBulkDataVaultBenchmark(
          CustomerProfileBulkScenarios.ChangeHeavy,
          DataVaultBenchmarkStrategy.ProviderNeutralFallback),
      new CustomerProfileBulkDataVaultBenchmark(
          CustomerProfileBulkScenarios.ChangeHeavy,
          DataVaultBenchmarkStrategy.SqliteOptimized),
      new OrderProductPlainEfBenchmark(),
      new OrderProductDataVaultBenchmark(DataVaultBenchmarkStrategy.ProviderNeutralFallback),
      new OrderProductDataVaultBenchmark(DataVaultBenchmarkStrategy.SqliteOptimized),
  ];

  public static async Task RunAsync(BenchmarkOptions options, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(options);

    var summaries = new List<BenchmarkSummary>();

    Console.WriteLine("DVault scenario comparison benchmarks");
    Console.WriteLine("Provider: " + BenchmarkArtifacts.ProviderName);
    Console.WriteLine("Postgres, Docker, and external services are not required.");
    Console.WriteLine();

    foreach (var benchmark in Benchmarks) {
      for (var iteration = 0; iteration < options.WarmupIterations; iteration++) {
        await benchmark.ExecuteAsync(cancellationToken).ConfigureAwait(false);
      }

      var elapsedTimes = new List<TimeSpan>();
      var persistedOutcome = string.Empty;
      for (var iteration = 0; iteration < options.Iterations; iteration++) {
        var result = await benchmark.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        elapsedTimes.Add(result.Elapsed);
        persistedOutcome = result.PersistedOutcome;
      }

      summaries.Add(BenchmarkSummary.Create(
          benchmark.ScenarioName,
          BenchmarkArtifacts.ProviderName,
          benchmark.BaselineName,
          benchmark.StrategyFamily,
          benchmark.DatasetSize,
          benchmark.ChangeRatio,
          elapsedTimes,
          persistedOutcome));
    }

    WriteSummary(summaries);
    Console.WriteLine();
    Console.WriteLine("Executed " + summaries.Count.ToString(CultureInfo.InvariantCulture) + " benchmark baselines.");

    if (options.ArtifactOutputDirectory is not null) {
      var context = BenchmarkRunContext.Create(options);
      var artifactPaths = await BenchmarkArtifacts
          .WriteAsync(options.ArtifactOutputDirectory, context, summaries, cancellationToken)
          .ConfigureAwait(false);

      Console.WriteLine("Wrote benchmark artifacts:");
      Console.WriteLine("  " + artifactPaths.MarkdownPath);
      Console.WriteLine("  " + artifactPaths.CsvPath);
      Console.WriteLine("  " + artifactPaths.JsonPath);
    }
  }

  public static void WriteUsage() {
    Console.WriteLine("Usage:");
    Console.WriteLine(
        "  dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchmarks");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --iterations <n>  Number of measured iterations. Default: 5.");
    Console.WriteLine("  --warmup <n>      Number of unreported warmup iterations. Default: 1.");
    Console.WriteLine("  --output <dir>    Directory for benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json.");
  }

  private static void WriteSummary(IEnumerable<BenchmarkSummary> summaries) {
    Console.Write(BenchmarkArtifacts.CreateMarkdownTable(summaries));
  }
}

internal interface IScenarioBenchmark {
  string ScenarioName { get; }

  string BaselineName { get; }

  string StrategyFamily { get; }

  string DatasetSize { get; }

  string ChangeRatio { get; }

  Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken);
}

internal sealed record ScenarioBenchmarkResult(TimeSpan Elapsed, string PersistedOutcome);

internal sealed record BenchmarkSummary(
    string ScenarioName,
    string Provider,
    string BaselineName,
    string StrategyFamily,
    string DatasetSize,
    string ChangeRatio,
    int Iterations,
    double MeanMilliseconds,
    double MinMilliseconds,
    double MaxMilliseconds,
    string PersistedOutcome) {
  public static BenchmarkSummary Create(
      string scenarioName,
      string providerName,
      string baselineName,
      string strategyFamily,
      string datasetSize,
      string changeRatio,
      IReadOnlyList<TimeSpan> elapsedTimes,
      string persistedOutcome) {
    ArgumentException.ThrowIfNullOrWhiteSpace(scenarioName);
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);
    ArgumentException.ThrowIfNullOrWhiteSpace(baselineName);
    ArgumentException.ThrowIfNullOrWhiteSpace(strategyFamily);
    ArgumentException.ThrowIfNullOrWhiteSpace(datasetSize);
    ArgumentException.ThrowIfNullOrWhiteSpace(changeRatio);
    ArgumentException.ThrowIfNullOrWhiteSpace(persistedOutcome);
    ArgumentNullException.ThrowIfNull(elapsedTimes);

    if (elapsedTimes.Count == 0) {
      throw new ArgumentException("At least one benchmark iteration is required.", nameof(elapsedTimes));
    }

    return new BenchmarkSummary(
        scenarioName,
        providerName,
        baselineName,
        strategyFamily,
        datasetSize,
        changeRatio,
        elapsedTimes.Count,
        elapsedTimes.Average(value => value.TotalMilliseconds),
        elapsedTimes.Min(value => value.TotalMilliseconds),
        elapsedTimes.Max(value => value.TotalMilliseconds),
        persistedOutcome);
  }
}

internal static class BenchmarkClock {
  public static async Task<TimeSpan> MeasureAsync(Func<Task> operation) {
    ArgumentNullException.ThrowIfNull(operation);

    var stopwatch = Stopwatch.StartNew();
    await operation().ConfigureAwait(false);
    stopwatch.Stop();

    return stopwatch.Elapsed;
  }
}
