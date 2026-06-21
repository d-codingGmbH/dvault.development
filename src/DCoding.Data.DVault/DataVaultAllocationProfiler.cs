using System.Diagnostics;
using System.Threading;

namespace DCoding.Data.DVault;

internal static class DataVaultAllocationProfiler {
  private static readonly AsyncLocal<DataVaultAllocationProfilerSession?> CurrentSession = new();

  public static bool IsEnabled => CurrentSession.Value is not null;

  public static DataVaultAllocationProfilerSession StartSession(string workloadName, int iteration) {
    return new DataVaultAllocationProfilerSession(workloadName, iteration, CurrentSession.Value, session => CurrentSession.Value = session);
  }

  public static T Measure<T>(string surface, string stepName, Func<T> operation) {
    ArgumentException.ThrowIfNullOrWhiteSpace(surface);
    ArgumentException.ThrowIfNullOrWhiteSpace(stepName);
    ArgumentNullException.ThrowIfNull(operation);

    var session = CurrentSession.Value;
    if (session is null) {
      return operation();
    }

    var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);
    var stopwatch = Stopwatch.StartNew();
    try {
      return operation();
    }
    finally {
      stopwatch.Stop();
      session.Record(
          surface,
          stepName,
          Math.Max(0, GC.GetTotalAllocatedBytes(precise: true) - allocatedBefore),
          stopwatch.Elapsed);
    }
  }

  public static async Task<T> MeasureAsync<T>(string surface, string stepName, Func<Task<T>> operation) {
    ArgumentException.ThrowIfNullOrWhiteSpace(surface);
    ArgumentException.ThrowIfNullOrWhiteSpace(stepName);
    ArgumentNullException.ThrowIfNull(operation);

    var session = CurrentSession.Value;
    if (session is null) {
      return await operation().ConfigureAwait(false);
    }

    var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);
    var stopwatch = Stopwatch.StartNew();
    try {
      return await operation().ConfigureAwait(false);
    }
    finally {
      stopwatch.Stop();
      session.Record(
          surface,
          stepName,
          Math.Max(0, GC.GetTotalAllocatedBytes(precise: true) - allocatedBefore),
          stopwatch.Elapsed);
    }
  }

  public static async Task MeasureAsync(string surface, string stepName, Func<Task> operation) {
    ArgumentException.ThrowIfNullOrWhiteSpace(surface);
    ArgumentException.ThrowIfNullOrWhiteSpace(stepName);
    ArgumentNullException.ThrowIfNull(operation);

    var session = CurrentSession.Value;
    if (session is null) {
      await operation().ConfigureAwait(false);
      return;
    }

    var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);
    var stopwatch = Stopwatch.StartNew();
    try {
      await operation().ConfigureAwait(false);
    }
    finally {
      stopwatch.Stop();
      session.Record(
          surface,
          stepName,
          Math.Max(0, GC.GetTotalAllocatedBytes(precise: true) - allocatedBefore),
          stopwatch.Elapsed);
    }
  }
}
