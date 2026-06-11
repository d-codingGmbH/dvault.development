using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

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
