using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CompiledModelBenchmark : IScenarioBenchmark {
  private const string OrderBusinessKey = "O-COMPILED-PERF-100";
  private readonly bool _useRuntimeModel;

  public CompiledModelBenchmark(bool useRuntimeModel) {
    _useRuntimeModel = useRuntimeModel;
  }

  public string ScenarioName => "compiled-model-startup";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => _useRuntimeModel
      ? "dvault-usemodel-runtime-model"
      : "dvault-design-model";

  public string StrategyFamily => _useRuntimeModel
      ? "ef-usemodel-runtime-model"
      : "ef-model-build";

  public string DatasetSize => "1 generated order hub row";

  public string ChangeRatio => "runtime model precomputed outside measured operation";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var metadataModel = CompiledEvidenceScenario.CreateOrderMetadataModel();
    var designOptions = CompiledEvidenceScenario.CreateOptions(database.ConnectionString);
    string orderHashKey;

    await using (var context = new CompiledEvidenceContext(
        designOptions,
        metadataModel,
        "compiled-model-seed")) {
      await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
      orderHashKey = await CompiledEvidenceScenario
          .SeedOrderHubAsync(context, OrderBusinessKey, "compiled-model-seed", cancellationToken)
          .ConfigureAwait(false);
    }

    IModel? runtimeModel = null;
    if (_useRuntimeModel) {
      await using var designContext = new CompiledEvidenceContext(
          designOptions,
          metadataModel,
          "compiled-model-design");
      runtimeModel = CompiledEvidenceScenario.CreateRuntimeModel(designContext);
    }

    var measuredModelCacheKey = "compiled-model-measured-" + Guid.NewGuid().ToString("N");
    var measuredOptions = _useRuntimeModel
        ? CompiledEvidenceScenario.CreateRuntimeModelOptions(database.ConnectionString, runtimeModel!)
        : CompiledEvidenceScenario.CreateOptions(database.ConnectionString);
    CompiledHubOrderRead? row = null;
    var elapsed = await BenchmarkClock.MeasureAsync(() => {
      using var context = new CompiledEvidenceContext(
          measuredOptions,
          metadataModel,
          measuredModelCacheKey);
      row = CompiledEvidenceScenario.ReadHubOrder(context, orderHashKey);

      return Task.CompletedTask;
    }).ConfigureAwait(false);

    CompiledEvidenceScenario.AssertHubOrder(row, orderHashKey, OrderBusinessKey, "compiled-model-seed");

    return new ScenarioBenchmarkResult(
        elapsed,
        _useRuntimeModel
            ? "1 generated order hub row read through precomputed UseModel(runtimeModel)"
            : "1 generated order hub row read through ordinary DVault model building");
  }
}
