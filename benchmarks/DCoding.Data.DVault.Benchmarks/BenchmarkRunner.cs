using System.Diagnostics;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkRunner {
  private static readonly IScenarioBenchmark[] Benchmarks =
  [
      new CustomerProfilePlainEfBenchmark(),
      new CustomerProfileDataVaultBenchmark(),
      new OrderProductPlainEfBenchmark(),
      new OrderProductDataVaultBenchmark(),
  ];

  public static async Task RunAsync(BenchmarkOptions options, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(options);

    var summaries = new List<BenchmarkSummary>();

    Console.WriteLine("DVault scenario comparison benchmarks");
    Console.WriteLine("Provider: SQLite local temporary files");
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
          benchmark.BaselineName,
          elapsedTimes,
          persistedOutcome));
    }

    WriteSummary(summaries);
    Console.WriteLine();
    Console.WriteLine("Executed " + summaries.Count.ToString(CultureInfo.InvariantCulture) + " benchmark baselines.");
  }

  public static void WriteUsage() {
    Console.WriteLine("Usage:");
    Console.WriteLine(
        "  dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --iterations <n>  Number of measured iterations. Default: 5.");
    Console.WriteLine("  --warmup <n>      Number of unreported warmup iterations. Default: 1.");
  }

  private static void WriteSummary(IEnumerable<BenchmarkSummary> summaries) {
    Console.WriteLine("| Scenario | Baseline | Iterations | Mean ms | Min ms | Max ms | Persisted outcome |");
    Console.WriteLine("| --- | --- | ---: | ---: | ---: | ---: | --- |");

    foreach (var summary in summaries) {
      Console.WriteLine(
          "| " +
          summary.ScenarioName +
          " | " +
          summary.BaselineName +
          " | " +
          summary.Iterations.ToString(CultureInfo.InvariantCulture) +
          " | " +
          FormatMilliseconds(summary.MeanMilliseconds) +
          " | " +
          FormatMilliseconds(summary.MinMilliseconds) +
          " | " +
          FormatMilliseconds(summary.MaxMilliseconds) +
          " | " +
          summary.PersistedOutcome +
          " |");
    }
  }

  private static string FormatMilliseconds(double value) {
    return value.ToString("F3", CultureInfo.InvariantCulture);
  }
}

internal interface IScenarioBenchmark {
  string ScenarioName { get; }

  string BaselineName { get; }

  Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken);
}

internal sealed record ScenarioBenchmarkResult(TimeSpan Elapsed, string PersistedOutcome);

internal sealed record BenchmarkSummary(
    string ScenarioName,
    string BaselineName,
    int Iterations,
    double MeanMilliseconds,
    double MinMilliseconds,
    double MaxMilliseconds,
    string PersistedOutcome) {
  public static BenchmarkSummary Create(
      string scenarioName,
      string baselineName,
      IReadOnlyList<TimeSpan> elapsedTimes,
      string persistedOutcome) {
    if (elapsedTimes.Count == 0) {
      throw new ArgumentException("At least one benchmark iteration is required.", nameof(elapsedTimes));
    }

    return new BenchmarkSummary(
        scenarioName,
        baselineName,
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
