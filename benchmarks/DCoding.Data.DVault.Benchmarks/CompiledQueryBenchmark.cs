using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CompiledQueryBenchmark : IScenarioBenchmark {
  private const string OrderBusinessKey = "O-COMPILED-QUERY-PERF-100";
  private readonly bool _compiledQuery;

  public CompiledQueryBenchmark(bool compiledQuery) {
    _compiledQuery = compiledQuery;
  }

  public string ScenarioName => "compiled-query-hub-read";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => _compiledQuery
      ? "ef-compilequery"
      : "ordinary-ef-query";

  public string StrategyFamily => _compiledQuery
      ? "compiled-ef-query"
      : "direct-ef-query";

  public string DatasetSize => "1 generated order hub row";

  public string ChangeRatio => "stable shared-type table projection";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var metadataModel = CompiledEvidenceScenario.CreateOrderMetadataModel();
    var options = CompiledEvidenceScenario.CreateOptions(database.ConnectionString);
    string orderHashKey;

    await using (var context = new CompiledEvidenceContext(options, metadataModel, "compiled-query")) {
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      orderHashKey = await CompiledEvidenceScenario
          .SeedOrderHubAsync(context, OrderBusinessKey, "compiled-query-seed", cancellationToken)
          .ConfigureAwait(false);
    }

    CompiledHubOrderRead? row = null;
    var elapsed = await BenchmarkClock.MeasureAsync(() => {
      using var context = new CompiledEvidenceContext(options, metadataModel, "compiled-query");
      row = _compiledQuery
          ? CompiledEvidenceScenario.ReadHubOrderCompiled(context, orderHashKey)
          : CompiledEvidenceScenario.ReadHubOrder(context, orderHashKey);

      return Task.CompletedTask;
    }).ConfigureAwait(false);

    CompiledEvidenceScenario.AssertHubOrder(row, orderHashKey, OrderBusinessKey, "compiled-query-seed");

    return new ScenarioBenchmarkResult(
        elapsed,
        _compiledQuery
            ? "1 generated order hub row read through EF.CompileQuery stable projection"
            : "1 generated order hub row read through equivalent ordinary EF projection");
  }
}
