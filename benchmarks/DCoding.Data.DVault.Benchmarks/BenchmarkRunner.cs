using System.Diagnostics;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkRunner {
  public static async Task RunAsync(BenchmarkOptions options, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(options);

    var postgresAvailability = await PostgresBenchmarkAvailability
        .DiscoverAsync(cancellationToken)
        .ConfigureAwait(false);

    await RunAsync(options, postgresAvailability, cancellationToken).ConfigureAwait(false);
  }

  internal static async Task RunAsync(
      BenchmarkOptions options,
      PostgresBenchmarkAvailability postgresAvailability,
      CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(options);
    ArgumentNullException.ThrowIfNull(postgresAvailability);

    var summaries = new List<BenchmarkSummary>();
    var sqliteBenchmarks = CreateSqliteBenchmarks();
    var postgresBenchmarks = CreatePostgresBenchmarks(postgresAvailability.Provider);

    Console.WriteLine("DVault scenario comparison benchmarks");
    Console.WriteLine("Required provider: " + BenchmarkArtifacts.RequiredProviderName);
    Console.WriteLine(
        "PostgreSQL provider: " +
        postgresAvailability.ExecutionStatus +
        (postgresAvailability.SkipReason is null ? string.Empty : " - " + postgresAvailability.SkipReason.DisplayText));
    Console.WriteLine();

    foreach (var benchmark in sqliteBenchmarks) {
      summaries.Add(await ExecuteBenchmarkAsync(benchmark, options, cancellationToken).ConfigureAwait(false));
    }

    if (postgresAvailability.IsAvailable) {
      foreach (var benchmark in postgresBenchmarks) {
        summaries.Add(await ExecuteBenchmarkAsync(benchmark, options, cancellationToken).ConfigureAwait(false));
      }
    }
    else {
      foreach (var benchmark in postgresBenchmarks) {
        summaries.Add(BenchmarkSummary.CreateSkipped(benchmark, postgresAvailability.SkipReason!));
      }
    }

    WriteSummary(summaries);
    Console.WriteLine();
    Console.WriteLine("Recorded " + summaries.Count.ToString(CultureInfo.InvariantCulture) + " benchmark report rows.");
    Console.WriteLine(
        "Executed " +
        summaries.Count(summary => summary.ExecutionStatus == BenchmarkExecutionStatus.Completed).ToString(CultureInfo.InvariantCulture) +
        " benchmark report rows.");
    var skippedCount = summaries.Count(summary => summary.ExecutionStatus == BenchmarkExecutionStatus.Skipped);
    if (skippedCount > 0) {
      Console.WriteLine("Skipped " + skippedCount.ToString(CultureInfo.InvariantCulture) + " benchmark report rows.");
    }

    if (options.ArtifactOutputDirectory is not null) {
      var context = BenchmarkRunContext.Create(options, postgresAvailability);
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
    Console.WriteLine();
    Console.WriteLine("Optional PostgreSQL provider:");
    Console.WriteLine(
        "  Set DVAULT_TEST_POSTGRES_CONNECTION_STRING before restore/build/run to include PostgreSQL comparison rows.");
  }

  private static void WriteSummary(IEnumerable<BenchmarkSummary> summaries) {
    Console.Write(BenchmarkArtifacts.CreateMarkdownTable(summaries));
  }

  private static IScenarioBenchmark[] CreateSqliteBenchmarks() {
    var provider = BenchmarkDatabaseProviders.Sqlite;

    return
    [
        new CustomerProfilePlainEfBenchmark(),
        new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized),
        new CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarios.InsertOnly),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.InsertOnly,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.InsertOnly,
            DataVaultBenchmarkStrategy.SqliteOptimized),
        new CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarios.ChangeHeavy),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.ChangeHeavy,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.ChangeHeavy,
            DataVaultBenchmarkStrategy.SqliteOptimized),
        new OrderProductPlainEfBenchmark(),
        new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized),
    ];
  }

  private static IScenarioBenchmark[] CreatePostgresBenchmarks(BenchmarkDatabaseProvider provider) {
    return
    [
        new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.PostgresOptimized),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.InsertOnly,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.InsertOnly,
            DataVaultBenchmarkStrategy.PostgresOptimized),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.ChangeHeavy,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.ChangeHeavy,
            DataVaultBenchmarkStrategy.PostgresOptimized),
        new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback),
        new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.PostgresOptimized),
    ];
  }

  private static async Task<BenchmarkSummary> ExecuteBenchmarkAsync(
      IScenarioBenchmark benchmark,
      BenchmarkOptions options,
      CancellationToken cancellationToken) {
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

    return BenchmarkSummary.Create(
        benchmark.ScenarioName,
        benchmark.ProviderName,
        benchmark.BaselineName,
        benchmark.StrategyFamily,
        benchmark.DatasetSize,
        benchmark.ChangeRatio,
        elapsedTimes,
        persistedOutcome);
  }
}

internal interface IScenarioBenchmark {
  string ScenarioName { get; }

  string ProviderName { get; }

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
    string ExecutionStatus,
    string SkipReason,
    int Iterations,
    double? MeanMilliseconds,
    double? MinMilliseconds,
    double? MaxMilliseconds,
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
        BenchmarkExecutionStatus.Completed,
        string.Empty,
        elapsedTimes.Count,
        elapsedTimes.Average(value => value.TotalMilliseconds),
        elapsedTimes.Min(value => value.TotalMilliseconds),
        elapsedTimes.Max(value => value.TotalMilliseconds),
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
        "not executed");
  }
}

internal static class BenchmarkExecutionStatus {
  public const string Completed = "completed";
  public const string Skipped = "skipped";
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
