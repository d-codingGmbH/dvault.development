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
        new CustomerProfileStreamingAsyncSourceBenchmark(chunkSize: 10),
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
    var benchmarks = new List<IScenarioBenchmark> {
        new ProviderNativeBulkIngestionBenchmark(
            provider,
            DataVaultBenchmarkStrategy.ProviderNeutralFallback,
            options.LoadTimestampStorage),
    };

    if (optimizedStrategy == DataVaultBenchmarkStrategy.PostgresOptimized) {
      benchmarks.Add(ProviderNativeBulkIngestionBenchmark.CreatePostgresRetainedDirectOrUnnest(
          provider,
          options.LoadTimestampStorage));
    }
    else if (optimizedStrategy == DataVaultBenchmarkStrategy.MySqlOptimized) {
      benchmarks.Add(ProviderNativeBulkIngestionBenchmark.CreateMySqlRetainedMultiRow(
          provider,
          options.LoadTimestampStorage));
    }

    benchmarks.Add(new ProviderNativeBulkIngestionBenchmark(provider, optimizedStrategy, options.LoadTimestampStorage));
    benchmarks.Add(new LatestSatelliteReadBenchmark(provider, optimizedStrategy, options.LoadTimestampStorage));
    benchmarks.Add(new PitAsOfReadBenchmark(provider, optimizedStrategy, options.LoadTimestampStorage));
    benchmarks.Add(new BridgeTraversalReadBenchmark(provider, optimizedStrategy, options.LoadTimestampStorage));

    return [.. benchmarks];
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
