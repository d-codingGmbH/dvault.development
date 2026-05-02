using System.Globalization;
using DCoding.Data.DVault.Benchmarks;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

public sealed class BenchmarkScenarioExecutionTests {
  [Fact]
  public async Task LocalBenchmarkRunnerExecutesCustomerAndOrderComparisonsThroughSqlite() {
    var originalOutput = Console.Out;
    using var output = new StringWriter(CultureInfo.InvariantCulture);

    try {
      Console.SetOut(output);

      await BenchmarkRunner
          .RunAsync(new BenchmarkOptions(1, 0), CancellationToken.None)
          .ConfigureAwait(false);
    }
    finally {
      Console.SetOut(originalOutput);
    }

    var text = output.ToString();

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
}
