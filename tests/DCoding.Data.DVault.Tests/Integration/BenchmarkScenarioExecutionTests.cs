using System.Globalization;
using System.Text.Json;
using DCoding.Data.DVault.Benchmarks;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

public sealed class BenchmarkScenarioExecutionTests {
  [Fact]
  public async Task LocalBenchmarkRunnerExecutesCustomerAndOrderComparisonsThroughSqlite() {
    var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(1, 0)).ConfigureAwait(false);

    Assert.Contains("Provider: SQLite local temporary files", text);
    Assert.Contains("Postgres, Docker, and external services are not required.", text);
    Assert.Contains("| customer-profile-history | conventional-ef | 1 |", text);
    Assert.Contains("| customer-profile-history | dvault-explicit-save | 1 |", text);
    Assert.Contains("| order-product-fulfillment-history | conventional-ef | 1 |", text);
    Assert.Contains("| order-product-fulfillment-history | dvault-explicit-save | 1 |", text);
    Assert.Contains("2 customer profile history rows for C-100", text);
    Assert.Contains("1 customer hub row and 2 profile satellite rows for C-100", text);
    Assert.Contains(
        "1 order, 1 product, 1 relationship, and 2 fulfillment history rows for O-1000/SKU-COFFEE",
        text);
    Assert.Contains(
        "1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE",
        text);
    Assert.Contains("Executed 4 benchmark baselines.", text);
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
      Assert.Contains("- Provider: SQLite local temporary files", markdown);
      Assert.Contains("- Iterations: 1", markdown);
      Assert.Contains("- Warmup iterations: 0", markdown);
      Assert.Contains("- OS description: ", markdown);
      Assert.Contains("- OS architecture: ", markdown);
      Assert.Contains("- Process architecture: ", markdown);
      Assert.Contains("- Processor count: ", markdown);
      Assert.Contains("- .NET runtime version: ", markdown);
      Assert.Contains("| Scenario | Baseline | Iterations | Mean ms | Min ms | Max ms | Persisted outcome |", markdown);
      Assert.Contains("| customer-profile-history | conventional-ef | 1 |", markdown);
      Assert.Contains("| customer-profile-history | dvault-explicit-save | 1 |", markdown);
      Assert.Contains("| order-product-fulfillment-history | conventional-ef | 1 |", markdown);
      Assert.Contains("| order-product-fulfillment-history | dvault-explicit-save | 1 |", markdown);

      var csv = await File.ReadAllTextAsync(csvPath).ConfigureAwait(false);
      var csvLines = csv.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      Assert.Equal(5, csvLines.Length);
      Assert.Equal(
          "scenario,baseline,iterations,meanMilliseconds,minMilliseconds,maxMilliseconds,persistedOutcome",
          csvLines[0]);
      Assert.Contains(csvLines, line => line.StartsWith("customer-profile-history,conventional-ef,1,", StringComparison.Ordinal));
      Assert.Contains(csvLines, line => line.StartsWith("customer-profile-history,dvault-explicit-save,1,", StringComparison.Ordinal));
      Assert.Contains(csvLines, line => line.StartsWith("order-product-fulfillment-history,conventional-ef,1,", StringComparison.Ordinal));
      Assert.Contains(csvLines, line => line.StartsWith("order-product-fulfillment-history,dvault-explicit-save,1,", StringComparison.Ordinal));

      using var json = JsonDocument.Parse(await File.ReadAllTextAsync(jsonPath).ConfigureAwait(false));
      var context = json.RootElement.GetProperty("context");
      Assert.Equal("SQLite local temporary files", context.GetProperty("provider").GetString());
      Assert.Equal(1, context.GetProperty("iterations").GetInt32());
      Assert.Equal(0, context.GetProperty("warmupIterations").GetInt32());
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osArchitecture").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("processArchitecture").GetString()));
      Assert.True(context.GetProperty("processorCount").GetInt32() > 0);
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeVersion").GetString()));

      var results = json.RootElement.GetProperty("results").EnumerateArray().ToArray();
      Assert.Equal(4, results.Length);
      Assert.Contains(results, result =>
          result.GetProperty("scenarioName").GetString() == "customer-profile-history" &&
          result.GetProperty("baselineName").GetString() == "conventional-ef" &&
          result.GetProperty("iterations").GetInt32() == 1);
      Assert.Contains(results, result =>
          result.GetProperty("scenarioName").GetString() == "customer-profile-history" &&
          result.GetProperty("baselineName").GetString() == "dvault-explicit-save" &&
          result.GetProperty("iterations").GetInt32() == 1);
      Assert.Contains(results, result =>
          result.GetProperty("scenarioName").GetString() == "order-product-fulfillment-history" &&
          result.GetProperty("baselineName").GetString() == "conventional-ef" &&
          result.GetProperty("iterations").GetInt32() == 1);
      Assert.Contains(results, result =>
          result.GetProperty("scenarioName").GetString() == "order-product-fulfillment-history" &&
          result.GetProperty("baselineName").GetString() == "dvault-explicit-save" &&
          result.GetProperty("iterations").GetInt32() == 1);
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
}
