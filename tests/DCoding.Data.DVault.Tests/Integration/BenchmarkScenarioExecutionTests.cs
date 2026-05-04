using System.Globalization;
using System.Text.Json;
using DCoding.Data.DVault.Benchmarks;
using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class BenchmarkScenarioExecutionTests {
  private const string ProviderName = "SQLite local temporary files";

  private static readonly ExpectedBenchmarkRow[] ExpectedRows =
  [
      new(
          "customer-profile-history",
          "conventional-ef",
          "classic-ef",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      new(
          "customer-profile-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      new(
          "customer-profile-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      new(
          "customer-profile-bulk-insert-only",
          "conventional-ef-bulk",
          "classic-ef",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      new(
          "customer-profile-bulk-insert-only",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      new(
          "customer-profile-bulk-insert-only",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      new(
          "customer-profile-bulk-history",
          "conventional-ef-bulk",
          "classic-ef",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      new(
          "customer-profile-bulk-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      new(
          "customer-profile-bulk-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      new(
          "order-product-fulfillment-history",
          "conventional-ef",
          "classic-ef",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      new(
          "order-product-fulfillment-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      new(
          "order-product-fulfillment-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
  ];

  [Fact]
  public async Task LocalBenchmarkRunnerExecutesCustomerAndOrderComparisonsThroughSqlite() {
    var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(1, 0)).ConfigureAwait(false);

    Assert.Contains("Provider: " + ProviderName, text);
    Assert.Contains("Postgres, Docker, and external services are not required.", text);

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
    Assert.Contains("Executed 12 benchmark baselines.", text);
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
      Assert.Contains("- Provider: " + ProviderName, markdown);
      Assert.Contains("- Iterations: 1", markdown);
      Assert.Contains("- Warmup iterations: 0", markdown);
      Assert.Contains("- OS description: ", markdown);
      Assert.Contains("- OS architecture: ", markdown);
      Assert.Contains("- Process architecture: ", markdown);
      Assert.Contains("- Processor count: ", markdown);
      Assert.Contains("- .NET runtime version: ", markdown);
      Assert.Contains("| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Iterations | Mean ms | Min ms | Max ms | Persisted outcome |", markdown);

      foreach (var expectedRow in ExpectedRows) {
        Assert.Contains(CreateMarkdownRowPrefix(expectedRow), markdown);
      }

      var csv = await File.ReadAllTextAsync(csvPath).ConfigureAwait(false);
      var csvLines = csv.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      Assert.Equal(ExpectedRows.Length + 1, csvLines.Length);
      Assert.Equal(
          "scenario,provider,baseline,strategyFamily,datasetSize,changeRatio,iterations,meanMilliseconds,minMilliseconds,maxMilliseconds,persistedOutcome",
          csvLines[0]);

      foreach (var expectedRow in ExpectedRows) {
        Assert.Contains(
            csvLines,
            line => line.StartsWith(CreateCsvRowPrefix(expectedRow), StringComparison.Ordinal));
      }

      using var json = JsonDocument.Parse(await File.ReadAllTextAsync(jsonPath).ConfigureAwait(false));
      var context = json.RootElement.GetProperty("context");
      Assert.Equal(ProviderName, context.GetProperty("provider").GetString());
      Assert.Equal(1, context.GetProperty("iterations").GetInt32());
      Assert.Equal(0, context.GetProperty("warmupIterations").GetInt32());
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osArchitecture").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("processArchitecture").GetString()));
      Assert.True(context.GetProperty("processorCount").GetInt32() > 0);
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeVersion").GetString()));

      var results = json.RootElement.GetProperty("results").EnumerateArray().ToArray();
      Assert.Equal(ExpectedRows.Length, results.Length);

      foreach (var expectedRow in ExpectedRows) {
        var matchingResults = results.Where(result =>
            result.GetProperty("scenarioName").GetString() == expectedRow.ScenarioName &&
            result.GetProperty("provider").GetString() == ProviderName &&
            result.GetProperty("baselineName").GetString() == expectedRow.BaselineName &&
            result.GetProperty("strategyFamily").GetString() == expectedRow.StrategyFamily)
            .ToArray();

        var result = Assert.Single(matchingResults);
        Assert.Equal(expectedRow.DatasetSize, result.GetProperty("datasetSize").GetString());
        Assert.Equal(expectedRow.ChangeRatio, result.GetProperty("changeRatio").GetString());
        Assert.Equal(1, result.GetProperty("iterations").GetInt32());
      }
    }
    finally {
      if (Directory.Exists(artifactDirectory)) {
        Directory.Delete(artifactDirectory, recursive: true);
      }
    }
  }

  private static async Task<string> RunBenchmarkAndCaptureOutputAsync(BenchmarkOptions options) {
    var originalOutput = Console.Out;
    using var output = new StringWriter(CultureInfo.InvariantCulture);

    try {
      Console.SetOut(output);

      await BenchmarkRunner
          .RunAsync(options, CancellationToken.None)
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
        ProviderName +
        " | " +
        expectedRow.BaselineName +
        " | " +
        expectedRow.StrategyFamily +
        " | " +
        expectedRow.DatasetSize +
        " | " +
        expectedRow.ChangeRatio +
        " | 1 |";
  }

  private static string CreateCsvRowPrefix(ExpectedBenchmarkRow expectedRow) {
    return string.Join(
        ',',
        expectedRow.ScenarioName,
        ProviderName,
        expectedRow.BaselineName,
        expectedRow.StrategyFamily,
        EscapeCsv(expectedRow.DatasetSize),
        EscapeCsv(expectedRow.ChangeRatio),
        "1") + ",";
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
      string BaselineName,
      string StrategyFamily,
      string DatasetSize,
      string ChangeRatio);
}
