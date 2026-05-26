using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkRunner {
  public static async Task RunAsync(BenchmarkOptions options, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(options);

    Console.WriteLine("Discovering benchmark providers for filter '" + options.ProviderFilter + "'.");
    var postgresAvailability = ShouldRunProvider(options, BenchmarkExternalProviderDefinitions.Postgres.ProviderName)
        ? await PostgresBenchmarkAvailability
            .DiscoverAsync(cancellationToken)
            .ConfigureAwait(false)
        : PostgresBenchmarkAvailability.Skipped(BenchmarkSkipReason.NotConfigured());
    var sqlServerAvailability = ShouldRunProvider(options, BenchmarkExternalProviderDefinitions.SqlServer.ProviderName)
        ? await BenchmarkProviderAvailability
            .DiscoverAsync(BenchmarkExternalProviderDefinitions.SqlServer, cancellationToken)
            .ConfigureAwait(false)
        : BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.SqlServer,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable));
    var mySqlAvailability = ShouldRunProvider(options, BenchmarkExternalProviderDefinitions.MySql.ProviderName)
        ? await BenchmarkProviderAvailability
            .DiscoverAsync(BenchmarkExternalProviderDefinitions.MySql, cancellationToken)
            .ConfigureAwait(false)
        : BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.MySql,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable));
    var oracleAvailability = ShouldRunProvider(options, BenchmarkExternalProviderDefinitions.Oracle.ProviderName)
        ? await BenchmarkProviderAvailability
            .DiscoverAsync(BenchmarkExternalProviderDefinitions.Oracle, cancellationToken)
            .ConfigureAwait(false)
        : BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.Oracle,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable));
    var optionalProviders = new[] {
        BenchmarkProviderAvailability.FromPostgres(postgresAvailability),
        sqlServerAvailability,
        mySqlAvailability,
        oracleAvailability,
    }
        .Where(availability => ShouldRunProvider(options, availability.ProviderName))
        .ToArray();

    await RunAsync(options, postgresAvailability, optionalProviders, cancellationToken).ConfigureAwait(false);
  }

  internal static async Task RunAsync(
      BenchmarkOptions options,
      PostgresBenchmarkAvailability postgresAvailability,
      IReadOnlyList<BenchmarkProviderAvailability> optionalProviders,
      CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(options);
    ArgumentNullException.ThrowIfNull(postgresAvailability);
    ArgumentNullException.ThrowIfNull(optionalProviders);

    var summaries = new List<BenchmarkSummary>();
    var selectedOptionalProviders = optionalProviders
        .Where(availability => ShouldRunProvider(options, availability.ProviderName))
        .ToArray();
    var sqliteBenchmarks = ShouldRunProvider(options, BenchmarkArtifacts.RequiredProviderName)
        ? options.LatestIndexMatrix
            ? CreateLatestIndexBenchmarks(BenchmarkDatabaseProviders.Sqlite, DataVaultBenchmarkStrategy.SqliteOptimized, options)
            : options.ScaleMatrix
            ? CreateScaleBenchmarks(BenchmarkDatabaseProviders.Sqlite, DataVaultBenchmarkStrategy.SqliteOptimized, options)
            : CreateSqliteBenchmarks(options)
        : [];
    var providerStrategies = new Dictionary<string, DataVaultBenchmarkStrategy>(StringComparer.Ordinal) {
      [BenchmarkExternalProviderDefinitions.Postgres.ProviderName] = DataVaultBenchmarkStrategy.PostgresOptimized,
      [BenchmarkExternalProviderDefinitions.SqlServer.ProviderName] = DataVaultBenchmarkStrategy.SqlServerOptimized,
      [BenchmarkExternalProviderDefinitions.MySql.ProviderName] = DataVaultBenchmarkStrategy.MySqlOptimized,
      [BenchmarkExternalProviderDefinitions.Oracle.ProviderName] = DataVaultBenchmarkStrategy.OracleOptimized,
    };

    Console.WriteLine("DVault scenario comparison benchmarks");
    Console.WriteLine("Required provider: " + BenchmarkArtifacts.RequiredProviderName);
    foreach (var availability in selectedOptionalProviders) {
      Console.WriteLine(
          availability.ProviderName +
          ": " +
          availability.ExecutionStatus +
          (availability.SkipReason is null ? string.Empty : " - " + availability.SkipReason.DisplayText));
    }

    Console.WriteLine();

    foreach (var benchmark in sqliteBenchmarks) {
      summaries.Add(await TryExecuteBenchmarkAsync(benchmark, options, cancellationToken).ConfigureAwait(false));
    }

    foreach (var availability in selectedOptionalProviders) {
      var strategy = providerStrategies[availability.ProviderName];
      var providerBenchmarks = options.LatestIndexMatrix
          ? CreateLatestIndexBenchmarks(availability.Provider, strategy, options)
          : options.ScaleMatrix
          ? CreateScaleBenchmarks(availability.Provider, strategy, options)
          : CreateProviderBenchmarks(availability.Provider, strategy, options);
      if (availability.IsAvailable) {
        foreach (var benchmark in providerBenchmarks) {
          summaries.Add(await TryExecuteBenchmarkAsync(benchmark, options, cancellationToken).ConfigureAwait(false));
        }
      }
      else {
        foreach (var benchmark in providerBenchmarks) {
          summaries.Add(BenchmarkSummary.CreateSkipped(benchmark, availability.SkipReason!));
        }
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
      var context = BenchmarkRunContext.Create(options, postgresAvailability, selectedOptionalProviders);
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
    Console.WriteLine("  --scale           Run only customer profile scale scenarios across configured providers.");
    Console.WriteLine("  --latest-indexes  Run only seeded latest-satellite lookup scenarios across index variants.");
    Console.WriteLine("  --load-timestamp-storage <provider-default|iso8601-utc-text|utc-ticks>");
    Console.WriteLine("                    Physical Data Vault load-timestamp storage to project. Default: provider-default.");
    Console.WriteLine("  --provider <all|sqlite|postgres|sqlserver|mysql|oracle>");
    Console.WriteLine("                    Provider set to execute. Default: all.");
    Console.WriteLine();
    Console.WriteLine("Optional PostgreSQL provider:");
    Console.WriteLine(
        "  Set DVAULT_TEST_POSTGRES_CONNECTION_STRING before restore/build/run to include PostgreSQL comparison rows.");
    Console.WriteLine("Optional SQL Server provider:");
    Console.WriteLine(
        "  Set DVAULT_TEST_SQLSERVER_CONNECTION_STRING before restore/build/run to include SQL Server comparison rows.");
    Console.WriteLine("Optional MySQL provider:");
    Console.WriteLine(
        "  Set DVAULT_TEST_MYSQL_CONNECTION_STRING before restore/build/run to include MySQL comparison rows.");
    Console.WriteLine("Optional Oracle provider:");
    Console.WriteLine(
        "  Set DVAULT_TEST_ORACLE_CONNECTION_STRING before restore/build/run to include Oracle comparison rows.");
  }

  private static void WriteSummary(IEnumerable<BenchmarkSummary> summaries) {
    Console.Write(BenchmarkArtifacts.CreateMarkdownTable(summaries));
  }

  private static bool ShouldRunProvider(BenchmarkOptions options, string providerName) {
    return options.ProviderFilter == BenchmarkProviderFilters.All ||
        options.ProviderFilter == ProviderFilterFor(providerName);
  }

  private static string ProviderFilterFor(string providerName) {
    if (string.Equals(providerName, BenchmarkArtifacts.RequiredProviderName, StringComparison.Ordinal)) {
      return BenchmarkProviderFilters.Sqlite;
    }

    if (string.Equals(providerName, BenchmarkExternalProviderDefinitions.Postgres.ProviderName, StringComparison.Ordinal)) {
      return BenchmarkProviderFilters.Postgres;
    }

    if (string.Equals(providerName, BenchmarkExternalProviderDefinitions.SqlServer.ProviderName, StringComparison.Ordinal)) {
      return BenchmarkProviderFilters.SqlServer;
    }

    if (string.Equals(providerName, BenchmarkExternalProviderDefinitions.MySql.ProviderName, StringComparison.Ordinal)) {
      return BenchmarkProviderFilters.MySql;
    }

    if (string.Equals(providerName, BenchmarkExternalProviderDefinitions.Oracle.ProviderName, StringComparison.Ordinal)) {
      return BenchmarkProviderFilters.Oracle;
    }

    return BenchmarkProviderFilters.All;
  }

  private static IScenarioBenchmark[] CreateSqliteBenchmarks(BenchmarkOptions options) {
    var provider = BenchmarkDatabaseProviders.Sqlite;

    return
    [
        new CustomerProfilePlainEfBenchmark(),
        new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),
        new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),
        new CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarios.InsertOnly),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.InsertOnly,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback,
            options.LoadTimestampStorage),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.InsertOnly,
            DataVaultBenchmarkStrategy.SqliteOptimized,
            options.LoadTimestampStorage),
        new CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarios.ChangeHeavy),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.ChangeHeavy,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback,
            options.LoadTimestampStorage),
        new CustomerProfileBulkDataVaultBenchmark(
            provider,
            CustomerProfileBulkScenarios.ChangeHeavy,
            DataVaultBenchmarkStrategy.SqliteOptimized,
            options.LoadTimestampStorage),
        new CustomerProfileStreamingMaterializedBenchmark(),
        new CustomerProfileStreamingChunkedBenchmark(chunkSize: 10),
        new CustomerProfileStreamingChunkedBenchmark(chunkSize: 5),
        new OrderProductPlainEfBenchmark(),
        new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),
        new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),
        new LatestSatelliteReadBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),
        new LatestSatelliteReadBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),
        new PitAsOfReadBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),
        new PitAsOfReadBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),
        new BridgeTraversalReadBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),
        new BridgeTraversalReadBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),
        new CompiledModelBenchmark(useRuntimeModel: false),
        new CompiledModelBenchmark(useRuntimeModel: true),
        new CompiledQueryBenchmark(compiledQuery: false),
        new CompiledQueryBenchmark(compiledQuery: true),
        new DbContextPoolingBenchmark(pooled: false),
        new DbContextPoolingBenchmark(pooled: true),
    ];
  }

  private static IScenarioBenchmark[] CreateProviderBenchmarks(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy optimizedStrategy,
      BenchmarkOptions options) {
    return
    [
        new ProviderNativeBulkIngestionBenchmark(
            provider,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback,
            options.LoadTimestampStorage),
        new ProviderNativeBulkIngestionBenchmark(provider, optimizedStrategy, options.LoadTimestampStorage),
    ];
  }

  private static IScenarioBenchmark[] CreateScaleBenchmarks(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy optimizedStrategy,
      BenchmarkOptions options) {
    var benchmarks = new List<IScenarioBenchmark>();

    foreach (var scenario in CustomerProfileBulkScenarios.ScaleMatrix) {
      benchmarks.Add(new CustomerProfileBulkPlainEfBenchmark(provider, scenario));
      benchmarks.Add(new CustomerProfileBulkDataVaultBenchmark(
          provider,
          scenario,
          DataVaultBenchmarkStrategy.ProviderNeutralFallback,
          options.LoadTimestampStorage));
      benchmarks.Add(new CustomerProfileBulkDataVaultBenchmark(provider, scenario, optimizedStrategy, options.LoadTimestampStorage));
    }

    return [.. benchmarks];
  }

  private static IScenarioBenchmark[] CreateLatestIndexBenchmarks(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy optimizedStrategy,
      BenchmarkOptions options) {
    var benchmarks = new List<IScenarioBenchmark>();

    foreach (var variant in LatestSatelliteLookupIndexVariant.GetVariants(provider.ProviderName)) {
      benchmarks.Add(new LatestSatelliteLookupIndexBenchmark(
          provider,
          optimizedStrategy,
          options.LoadTimestampStorage,
          variant,
          LatestSatelliteLookupWorkload.UnchangedReplay));
      benchmarks.Add(new LatestSatelliteLookupIndexBenchmark(
          provider,
          optimizedStrategy,
          options.LoadTimestampStorage,
          variant,
          LatestSatelliteLookupWorkload.ChangedReplay));
    }

    return [.. benchmarks];
  }

  private static async Task<BenchmarkSummary> ExecuteBenchmarkAsync(
      IScenarioBenchmark benchmark,
      BenchmarkOptions options,
      CancellationToken cancellationToken) {
    for (var iteration = 0; iteration < options.WarmupIterations; iteration++) {
      await benchmark.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }

    var elapsedTimes = new List<TimeSpan>();
    var allocatedBytes = new List<long>();
    var persistedOutcome = string.Empty;
    var executionDetail = string.Empty;
    for (var iteration = 0; iteration < options.Iterations; iteration++) {
      var result = await benchmark.ExecuteAsync(cancellationToken).ConfigureAwait(false);
      elapsedTimes.Add(result.Elapsed);
      allocatedBytes.Add(result.AllocatedBytes);
      persistedOutcome = result.PersistedOutcome;
      executionDetail = result.ExecutionDetail;
    }

    return BenchmarkSummary.Create(
        benchmark.ScenarioName,
        benchmark.ProviderName,
        benchmark.BaselineName,
        benchmark.StrategyFamily,
        benchmark.DatasetSize,
        benchmark.ChangeRatio,
        elapsedTimes,
        allocatedBytes,
        persistedOutcome,
        string.IsNullOrWhiteSpace(executionDetail)
            ? BenchmarkExecutionDetails.CreatePlanned(benchmark)
            : executionDetail);
  }

  private static async Task<BenchmarkSummary> TryExecuteBenchmarkAsync(
      IScenarioBenchmark benchmark,
      BenchmarkOptions options,
      CancellationToken cancellationToken) {
    try {
      Console.WriteLine(
          "Running " +
          benchmark.ProviderName +
          " / " +
          benchmark.ScenarioName +
          " / " +
          benchmark.StrategyFamily +
          " / " +
          options.LoadTimestampStorage +
          "...");
      return await ExecuteBenchmarkAsync(benchmark, options, cancellationToken).ConfigureAwait(false);
    }
    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) {
      throw;
    }
    catch (Exception exception) {
      return BenchmarkSummary.CreateFailed(benchmark, exception);
    }
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

internal sealed record BenchmarkMeasurement(TimeSpan Elapsed, long AllocatedBytes);

internal sealed record ScenarioBenchmarkResult(
    BenchmarkMeasurement Measurement,
    string PersistedOutcome,
    string ExecutionDetail) {
  public ScenarioBenchmarkResult(
      BenchmarkMeasurement measurement,
      string persistedOutcome)
      : this(measurement, persistedOutcome, string.Empty) {
  }

  public TimeSpan Elapsed => Measurement.Elapsed;

  public long AllocatedBytes => Measurement.AllocatedBytes;
}

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

internal static class BenchmarkExecutionDetails {
  public static string CreatePlanned(IScenarioBenchmark benchmark) {
    ArgumentNullException.ThrowIfNull(benchmark);

    return "scenario=" + benchmark.ScenarioName +
        "; provider=" + benchmark.ProviderName +
        "; baseline=" + benchmark.BaselineName +
        "; strategyFamily=" + benchmark.StrategyFamily +
        "; executionPath=" + GetExecutionPath(benchmark);
  }

  public static string CreateSaveStrategyDetail(
      IScenarioBenchmark benchmark,
      DataVaultDiagnosticsResult diagnostics,
      int requestCount,
      int hubOperationCount,
      int linkOperationCount,
      int satelliteOperationCount) {
    ArgumentNullException.ThrowIfNull(benchmark);
    ArgumentNullException.ThrowIfNull(diagnostics);

    return CreatePlanned(benchmark) +
        "; saveStrategyStatus=" + diagnostics.SaveStrategy.Status +
        "; provider=" + (diagnostics.SaveStrategy.ProviderName ?? "<none>") +
        "; selectedStrategy=" + (diagnostics.SaveStrategy.SelectedStrategyName ?? "<none>") +
        "; candidates=" + diagnostics.SaveStrategy.Candidates.Count.ToString(CultureInfo.InvariantCulture) +
        "; fallbackCauses=" + FormatFallbackCauses(diagnostics.SaveStrategy.FallbackCauses) +
        "; requestCount=" + requestCount.ToString(CultureInfo.InvariantCulture) +
        "; hubOperations=" + hubOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; linkOperations=" + linkOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; satelliteOperations=" + satelliteOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; nativeBulkGate=clean-context,no-multi-active-satellites,provider-eligible-bulk-request" +
        FormatStagedProviderBulk(diagnostics.SaveStrategy.StagedProviderBulk);
  }

  private static string GetExecutionPath(IScenarioBenchmark benchmark) {
    return benchmark.StrategyFamily switch {
      DataVaultBenchmarkHelpers.ClassicEfStrategyFamily => "classic EF baseline",
      DataVaultBenchmarkHelpers.ProviderNeutralFallbackStrategyFamily =>
          "DVault provider-neutral fallback path; selectedStrategy=<none>",
      DataVaultBenchmarkHelpers.SqliteOptimizedStrategyFamily =>
          "DVault SQLite optimized path; selectedStrategy=" + GetSqliteStrategyName(benchmark.ScenarioName),
      DataVaultBenchmarkHelpers.PostgresOptimizedStrategyFamily =>
          "DVault PostgreSQL staged bulk save path; transfer=COPY; selectedStrategy=PostgresDataVaultSaveStrategy; " +
          "smallBatchBoundary=direct-or-unnest",
      DataVaultBenchmarkHelpers.SqlServerOptimizedStrategyFamily =>
          "DVault SQL Server staged native bulk save path; transfer=SqlBulkCopy; selectedStrategy=SqlServerDataVaultSaveStrategy",
      DataVaultBenchmarkHelpers.MySqlOptimizedStrategyFamily =>
          "DVault MySQL staged bulk save path; selectedStrategy=MySqlStagedDataVaultSaveStrategy",
      DataVaultBenchmarkHelpers.OracleOptimizedStrategyFamily =>
          "DVault Oracle direct optimized save path; selectedStrategy=OracleDataVaultSaveStrategy; " +
          "oracleBulkBoundary=direct-oracle-batching; stagedOracleBulk=not-selected-no-measured-win",
      "ef-model-build" => "ordinary EF model-building startup path",
      "ef-usemodel-runtime-model" => "precomputed EF runtime model path",
      "direct-ef-query" => "ordinary direct EF query path",
      "compiled-ef-query" => "EF.CompileQuery path",
      "non-pooled-dvault-context" => "AddDbContext DVault context path",
      "pooled-dvault-context" => "AddDbContextPool DVault context path",
      _ => "benchmark-defined path",
    };
  }

  private static string GetSqliteStrategyName(string scenarioName) {
    return scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read"
        ? "SqliteDataVaultReadStrategy"
        : "SqliteDataVaultSaveStrategy";
  }

  private static string FormatFallbackCauses(IReadOnlyList<DataVaultSaveStrategyFallbackCause> fallbackCauses) {
    if (fallbackCauses.Count == 0) {
      return "none";
    }

    return string.Join("|", fallbackCauses.Select(cause => cause.Kind.ToString()));
  }

  private static string FormatStagedProviderBulk(DataVaultStagedProviderBulkDiagnostics? stagedProviderBulk) {
    if (stagedProviderBulk is null) {
      return string.Empty;
    }

    return "; stagedProviderBulkPhase=" + stagedProviderBulk.LifecyclePhase +
        "; stagedProviderBulkCaveat=" + stagedProviderBulk.ProviderCaveatKind +
        "; stagedProviderBulkOperations=" + stagedProviderBulk.OperationCount.ToString(CultureInfo.InvariantCulture);
  }
}

internal static class BenchmarkExecutionStatus {
  public const string Completed = "completed";
  public const string Failed = "failed";
  public const string Skipped = "skipped";
}

internal static class BenchmarkClock {
  public static async Task<BenchmarkMeasurement> MeasureAsync(Func<Task> operation) {
    ArgumentNullException.ThrowIfNull(operation);

    // Avoid charging pending finalizers from a previous scenario to this row.
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);
    var stopwatch = Stopwatch.StartNew();
    await operation().ConfigureAwait(false);
    stopwatch.Stop();
    var allocatedAfter = GC.GetTotalAllocatedBytes(precise: true);
    var allocatedBytes = Math.Max(0, allocatedAfter - allocatedBefore);

    return new BenchmarkMeasurement(stopwatch.Elapsed, allocatedBytes);
  }
}
