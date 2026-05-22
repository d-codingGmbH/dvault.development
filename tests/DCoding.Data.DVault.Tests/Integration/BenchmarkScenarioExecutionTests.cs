using System.Globalization;
using System.Text.Json;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Benchmarks;
using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class BenchmarkScenarioExecutionTests {
  private const string SqliteProviderName = "SQLite local temporary files";
  private const string PostgresProviderName = "PostgreSQL external provider";
  private const string SqlServerProviderName = "SQL Server external provider";
  private const string MySqlProviderName = "MySQL external provider";
  private const string OracleProviderName = "Oracle external provider";

  private static readonly ExpectedBenchmarkRow[] ExpectedRows =
  [
      CompletedSqlite(
          "customer-profile-history",
          "conventional-ef",
          "classic-ef",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      CompletedSqlite(
          "customer-profile-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      CompletedSqlite(
          "customer-profile-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-insert-only",
          "conventional-ef-bulk",
          "classic-ef",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-insert-only",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-insert-only",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-history",
          "conventional-ef-bulk",
          "classic-ef",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      CompletedSqlite(
          "order-product-fulfillment-history",
          "conventional-ef",
          "classic-ef",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      CompletedSqlite(
          "order-product-fulfillment-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      CompletedSqlite(
          "order-product-fulfillment-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      CompletedSqlite(
          "latest-satellite-read",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read"),
      CompletedSqlite(
          "latest-satellite-read",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read"),
      CompletedSqlite(
          "pit-as-of-read",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots"),
      CompletedSqlite(
          "pit-as-of-read",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots"),
      CompletedSqlite(
          "bridge-traversal-read",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5"),
      CompletedSqlite(
          "bridge-traversal-read",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5"),
      SkippedExternal(
          PostgresProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReason),
      SkippedExternal(
          PostgresProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultpostgres-optimized",
          "postgres-optimized-dvault",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReason),
      SkippedExternal(
          SqlServerProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          SqlServerProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultsqlserver-optimized",
          "sqlserver-optimized-dvault",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultmysql-optimized",
          "mysql-optimized-dvault",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          OracleProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          OracleProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultoracle-optimized",
          "oracle-optimized-dvault",
          "20 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
  ];

  [Fact]
  public async Task LocalBenchmarkRunnerExecutesCustomerAndOrderComparisonsThroughSqlite() {
    var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(1, 0)).ConfigureAwait(false);

    Assert.Contains("Required provider: " + SqliteProviderName, text);
    Assert.Contains(PostgresProviderName + ": skipped - " + NotConfiguredSkipReason, text);

    foreach (var expectedRow in ExpectedRows) {
      Assert.Contains(CreateMarkdownRowPrefix(expectedRow), text);
    }

    Assert.Contains("2 customer profile history rows for C-100", text);
    Assert.Contains("1 customer hub row and 2 profile satellite rows for C-100", text);
    Assert.Contains("100 customer profile history rows for 100 customers", text);
    Assert.Contains("100 customer hubs and 100 profile satellite rows", text);
    Assert.Contains("1000 customer profile history rows for 100 customers", text);
    Assert.Contains("100 customer hubs and 1000 profile satellite rows", text);
    Assert.Contains(
        "1 order, 1 product, 1 relationship, and 2 fulfillment history rows for O-1000/SKU-COFFEE",
        text);
    Assert.Contains(
        "1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE",
        text);
    Assert.Contains("100 latest profile satellite rows read from 1000 seeded profile states", text);
    Assert.Contains("100 PIT as-of rows read across profile and status satellite snapshots", text);
    Assert.Contains("60 bridge traversal rows read from 100 seeded hierarchy rows", text);
    Assert.Contains("Recorded 26 benchmark report rows.", text);
    Assert.Contains("Executed 18 benchmark report rows.", text);
    Assert.Contains("Skipped 8 benchmark report rows.", text);
  }

  [Fact]
  public async Task LocalBenchmarkRunnerEmitsDocumentationArtifactsFromOneRun() {
    var artifactDirectory = Path.Combine(
        Path.GetTempPath(),
        "DVaultBenchmarkArtifacts-" + Guid.NewGuid().ToString("N"));

    try {
      var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(1, 0, artifactDirectory))
          .ConfigureAwait(false);

      Assert.Contains("Wrote benchmark artifacts:", text);

      var markdownPath = Path.Combine(artifactDirectory, "benchmark-summary.md");
      var csvPath = Path.Combine(artifactDirectory, "benchmark-summary.csv");
      var jsonPath = Path.Combine(artifactDirectory, "benchmark-summary.json");

      Assert.True(File.Exists(markdownPath));
      Assert.True(File.Exists(csvPath));
      Assert.True(File.Exists(jsonPath));

      var markdown = await File.ReadAllTextAsync(markdownPath).ConfigureAwait(false);
      Assert.Contains("# DVault Benchmark Summary", markdown);
      Assert.Contains("- Required provider: " + SqliteProviderName, markdown);
      Assert.Contains("- Optional PostgreSQL provider: " + PostgresProviderName, markdown);
      Assert.Contains("- PostgreSQL execution status: skipped", markdown);
      Assert.Contains("- PostgreSQL skip reason: " + NotConfiguredSkipReason, markdown);
      Assert.Contains("- Iterations: 1", markdown);
      Assert.Contains("- Warmup iterations: 0", markdown);
      Assert.Contains("- Load timestamp storage: ProviderDefault", markdown);
      Assert.Contains("- Provider filter: all", markdown);
      Assert.Contains("- OS description: ", markdown);
      Assert.Contains("- OS architecture: ", markdown);
      Assert.Contains("- Process architecture: ", markdown);
      Assert.Contains("- Processor count: ", markdown);
      Assert.Contains("- .NET runtime version: ", markdown);
      Assert.Contains("| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Mean allocated bytes | Min allocated bytes | Max allocated bytes | Persisted outcome |", markdown);

      foreach (var expectedRow in ExpectedRows) {
        Assert.Contains(CreateMarkdownRowPrefix(expectedRow), markdown);
      }

      var csv = await File.ReadAllTextAsync(csvPath).ConfigureAwait(false);
      var csvLines = csv.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      Assert.Equal(27, csvLines.Length);
      Assert.Equal(
          "scenario,provider,baseline,strategyFamily,datasetSize,changeRatio,executionStatus,skipReason,iterations,meanMilliseconds,minMilliseconds,maxMilliseconds,meanAllocatedBytes,minAllocatedBytes,maxAllocatedBytes,persistedOutcome",
          csvLines[0]);

      foreach (var expectedRow in ExpectedRows) {
        Assert.Contains(
            csvLines,
            line => line.StartsWith(CreateCsvRowPrefix(expectedRow), StringComparison.Ordinal));
      }

      using var json = JsonDocument.Parse(await File.ReadAllTextAsync(jsonPath).ConfigureAwait(false));
      var context = json.RootElement.GetProperty("context");
      Assert.Equal(SqliteProviderName, context.GetProperty("provider").GetString());
      Assert.Equal(PostgresProviderName, context.GetProperty("optionalPostgresProvider").GetString());
      Assert.Equal("skipped", context.GetProperty("postgresExecutionStatus").GetString());
      Assert.Equal(NotConfiguredSkipReason, context.GetProperty("postgresSkipReason").GetString());
      Assert.Equal(1, context.GetProperty("iterations").GetInt32());
      Assert.Equal(0, context.GetProperty("warmupIterations").GetInt32());
      Assert.Equal("ProviderDefault", context.GetProperty("loadTimestampStorage").GetString());
      Assert.Equal("all", context.GetProperty("providerFilter").GetString());
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osArchitecture").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("processArchitecture").GetString()));
      Assert.True(context.GetProperty("processorCount").GetInt32() > 0);
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeVersion").GetString()));
      var optionalProviders = context.GetProperty("optionalProviders").EnumerateArray().ToArray();
      Assert.Equal(4, optionalProviders.Length);
      AssertOptionalProviderContext(
          optionalProviders,
          PostgresProviderName,
          BenchmarkExternalProviderDefinitions.Postgres.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReason);
      AssertOptionalProviderContext(
          optionalProviders,
          SqlServerProviderName,
          BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable));
      AssertOptionalProviderContext(
          optionalProviders,
          MySqlProviderName,
          BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable));
      AssertOptionalProviderContext(
          optionalProviders,
          OracleProviderName,
          BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable));

      var results = json.RootElement.GetProperty("results").EnumerateArray().ToArray();
      Assert.Equal(26, results.Length);

      foreach (var expectedRow in ExpectedRows) {
        var matchingResults = results.Where(result =>
            result.GetProperty("scenarioName").GetString() == expectedRow.ScenarioName &&
            result.GetProperty("provider").GetString() == expectedRow.ProviderName &&
            result.GetProperty("baselineName").GetString() == expectedRow.BaselineName &&
            result.GetProperty("strategyFamily").GetString() == expectedRow.StrategyFamily)
            .ToArray();

        var result = Assert.Single(matchingResults);
        Assert.Equal(expectedRow.DatasetSize, result.GetProperty("datasetSize").GetString());
        Assert.Equal(expectedRow.ChangeRatio, result.GetProperty("changeRatio").GetString());
        Assert.Equal(expectedRow.ExecutionStatus, result.GetProperty("executionStatus").GetString());
        Assert.Equal(expectedRow.SkipReason, result.GetProperty("skipReason").GetString());
        Assert.Equal(expectedRow.Iterations, result.GetProperty("iterations").GetInt32());

        if (expectedRow.ExecutionStatus == "skipped") {
          Assert.Equal(JsonValueKind.Null, result.GetProperty("meanMilliseconds").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("minMilliseconds").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("maxMilliseconds").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("meanAllocatedBytes").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("minAllocatedBytes").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("maxAllocatedBytes").ValueKind);
          Assert.Equal("not executed", result.GetProperty("persistedOutcome").GetString());
        }
        else {
          Assert.True(result.GetProperty("meanAllocatedBytes").GetDouble() >= 0);
          Assert.True(result.GetProperty("minAllocatedBytes").GetInt64() >= 0);
          Assert.True(result.GetProperty("maxAllocatedBytes").GetInt64() >= 0);
        }
      }
    }
    finally {
      if (Directory.Exists(artifactDirectory)) {
        Directory.Delete(artifactDirectory, recursive: true);
      }
    }
  }

  [Fact]
  public async Task ProviderNativeBulkBenchmarkProvesSelectedProviderStrategyBeforeTimingNativeRow() {
    var benchmark = new ProviderNativeBulkIngestionBenchmark(
        BenchmarkDatabaseProviders.Sqlite,
        DataVaultBenchmarkStrategy.SqliteOptimized,
        DataVaultLoadTimestampStorage.ProviderDefault);

    var result = await benchmark.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

    Assert.Contains("20 order hubs, 20 product hubs, 20 order-product links, and 2 fulfillment satellite rows", result.PersistedOutcome);
    Assert.True(result.Elapsed > TimeSpan.Zero);
  }

  [Fact]
  public async Task LocalBenchmarkRunnerCanRunLatestSatelliteIndexMatrixForSqlite() {
    var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(
        1,
        0,
        LatestIndexMatrix: true,
        ProviderFilter: BenchmarkProviderFilters.Sqlite)).ConfigureAwait(false);

    Assert.Contains("latest-satellite-lookup-replay", text);
    Assert.Contains("latest-satellite-lookup-change", text);
    Assert.Contains("dvault-adddvaultsqlite-optimized/latest-index-default", text);
    Assert.Contains("dvault-adddvaultsqlite-optimized/latest-index-parent-desc", text);
    Assert.Contains("dvault-adddvaultsqlite-optimized/latest-index-covering", text);
    Assert.Contains("2000 profile satellite rows after unchanged replay latest lookup", text);
    Assert.Contains("2100 profile satellite rows after changed replay latest lookup", text);
    Assert.Contains("Recorded 6 benchmark report rows.", text);
    Assert.Contains("Executed 6 benchmark report rows.", text);
    Assert.DoesNotContain("customer-profile-history", text);
  }

  [Fact]
  public async Task PostgresDiscoveryTreatsMissingEnvironmentVariableAsNotConfiguredSkip() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "  ",
            () => throw new InvalidOperationException("Provider dependency probe should not run."),
            (_, _) => throw new InvalidOperationException("Connection probe should not run."),
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.False(availability.IsAvailable);
    Assert.Equal("skipped", availability.ExecutionStatus);
    Assert.Equal("not configured", availability.SkipReason?.Category);
    Assert.Equal(NotConfiguredSkipReason, availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsUnavailableProviderDependencyBeforeConnecting() {
    var connectionProbeCalled = false;

    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => false,
            (_, _) => {
              connectionProbeCalled = true;
              return Task.FromResult<string?>(null);
            },
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.False(connectionProbeCalled);
    Assert.False(availability.IsAvailable);
    Assert.Equal("provider dependency unavailable", availability.SkipReason?.Category);
    Assert.Contains("Npgsql.EntityFrameworkCore.PostgreSQL", availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsUnreachableConnectionAsSkippedProvider() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => true,
            (_, _) => Task.FromResult<string?>("simulated connection failure"),
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.False(availability.IsAvailable);
    Assert.Equal("connection unreachable", availability.SkipReason?.Category);
    Assert.Contains("simulated connection failure", availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsTimedOutConnectionProbeAsSkippedProvider() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => true,
            async (_, cancellationToken) => {
              await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken).ConfigureAwait(false);

              return null;
            },
            CancellationToken.None,
            TimeSpan.FromMilliseconds(10))
        .ConfigureAwait(false);

    Assert.False(availability.IsAvailable);
    Assert.Equal("connection unreachable", availability.SkipReason?.Category);
    Assert.Contains("Timed out after", availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsConfiguredConnectionAsAvailable() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => true,
            (_, _) => Task.FromResult<string?>(null),
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.True(availability.IsAvailable);
    Assert.Equal("completed", availability.ExecutionStatus);
    Assert.Null(availability.SkipReason);
    Assert.Equal(PostgresProviderName, availability.Provider.ProviderName);
  }

  private static async Task<string> RunBenchmarkAndCaptureOutputAsync(BenchmarkOptions options) {
    var originalOutput = Console.Out;
    using var output = new StringWriter(CultureInfo.InvariantCulture);
    var postgresAvailability = PostgresBenchmarkAvailability.Skipped(BenchmarkSkipReason.NotConfigured());
    var optionalProviders = new[]
    {
        BenchmarkProviderAvailability.FromPostgres(postgresAvailability),
        BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.SqlServer,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
        BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.MySql,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
        BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.Oracle,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
    };

    try {
      Console.SetOut(output);

      await BenchmarkRunner
          .RunAsync(options, postgresAvailability, optionalProviders, CancellationToken.None)
          .ConfigureAwait(false);
    }
    finally {
      Console.SetOut(originalOutput);
    }

    return output.ToString();
  }

  private static string CreateMarkdownRowPrefix(ExpectedBenchmarkRow expectedRow) {
    return "| " +
        expectedRow.ScenarioName +
        " | " +
        expectedRow.ProviderName +
        " | " +
        expectedRow.BaselineName +
        " | " +
        expectedRow.StrategyFamily +
        " | " +
        expectedRow.DatasetSize +
        " | " +
        expectedRow.ChangeRatio +
        " | " +
        expectedRow.ExecutionStatus +
        " | " +
        expectedRow.SkipReason +
        " | " +
        expectedRow.Iterations.ToString(CultureInfo.InvariantCulture) +
        " |";
  }

  private static string CreateCsvRowPrefix(ExpectedBenchmarkRow expectedRow) {
    return string.Join(
        ',',
        expectedRow.ScenarioName,
        expectedRow.ProviderName,
        expectedRow.BaselineName,
        expectedRow.StrategyFamily,
        EscapeCsv(expectedRow.DatasetSize),
        EscapeCsv(expectedRow.ChangeRatio),
        expectedRow.ExecutionStatus,
        EscapeCsv(expectedRow.SkipReason),
        expectedRow.Iterations.ToString(CultureInfo.InvariantCulture)) + ",";
  }

  private static string NotConfiguredSkipReason => BenchmarkSkipReason.NotConfigured().DisplayText;

  private static string NotConfiguredSkipReasonFor(string connectionStringEnvironmentVariable) {
    return BenchmarkSkipReason.NotConfigured(connectionStringEnvironmentVariable).DisplayText;
  }

  private static void AssertOptionalProviderContext(
      JsonElement[] optionalProviders,
      string providerName,
      string connectionStringEnvironmentVariable,
      string skipReason) {
    var provider = Assert.Single(optionalProviders, candidate =>
        candidate.GetProperty("providerName").GetString() == providerName);

    Assert.Equal(connectionStringEnvironmentVariable, provider.GetProperty("connectionStringEnvironmentVariable").GetString());
    Assert.Equal("skipped", provider.GetProperty("executionStatus").GetString());
    Assert.Equal(skipReason, provider.GetProperty("skipReason").GetString());
  }

  private static ExpectedBenchmarkRow CompletedSqlite(
      string scenarioName,
      string baselineName,
      string strategyFamily,
      string datasetSize,
      string changeRatio) {
    return new ExpectedBenchmarkRow(
        scenarioName,
        SqliteProviderName,
        baselineName,
        strategyFamily,
        datasetSize,
        changeRatio,
        "completed",
        string.Empty,
        1);
  }

  private static ExpectedBenchmarkRow SkippedExternal(
      string providerName,
      string scenarioName,
      string baselineName,
      string strategyFamily,
      string datasetSize,
      string changeRatio,
      string skipReason) {
    return new ExpectedBenchmarkRow(
        scenarioName,
        providerName,
        baselineName,
        strategyFamily,
        datasetSize,
        changeRatio,
        "skipped",
        skipReason,
        0);
  }

  private static string EscapeCsv(string value) {
    if (!value.Contains('"') &&
        !value.Contains(',') &&
        !value.Contains('\r') &&
        !value.Contains('\n')) {
      return value;
    }

    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private sealed record ExpectedBenchmarkRow(
      string ScenarioName,
      string ProviderName,
      string BaselineName,
      string StrategyFamily,
      string DatasetSize,
      string ChangeRatio,
      string ExecutionStatus,
      string SkipReason,
      int Iterations);
}
