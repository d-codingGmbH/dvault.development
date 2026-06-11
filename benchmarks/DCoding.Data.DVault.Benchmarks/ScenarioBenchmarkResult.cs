using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

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
