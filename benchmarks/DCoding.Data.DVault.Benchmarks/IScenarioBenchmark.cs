using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal interface IScenarioBenchmark {
  string ScenarioName { get; }

  string ProviderName { get; }

  string BaselineName { get; }

  string StrategyFamily { get; }

  string DatasetSize { get; }

  string ChangeRatio { get; }

  Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken);
}
