namespace DCoding.Data.DVault;

internal sealed class DataVaultAllocationProfilerSession : IDisposable {
  private readonly Action<DataVaultAllocationProfilerSession?> _setCurrentSession;
  private readonly DataVaultAllocationProfilerSession? _previousSession;
  private readonly List<DataVaultAllocationProfilerSample> _samples = [];
  private bool _disposed;

  internal DataVaultAllocationProfilerSession(
      string workloadName,
      int iteration,
      DataVaultAllocationProfilerSession? previousSession,
      Action<DataVaultAllocationProfilerSession?> setCurrentSession) {
    ArgumentException.ThrowIfNullOrWhiteSpace(workloadName);
    ArgumentOutOfRangeException.ThrowIfNegative(iteration);
    ArgumentNullException.ThrowIfNull(setCurrentSession);

    WorkloadName = workloadName;
    Iteration = iteration;
    _previousSession = previousSession;
    _setCurrentSession = setCurrentSession;
    _setCurrentSession(this);
  }

  public string WorkloadName { get; }

  public int Iteration { get; }

  public IReadOnlyList<DataVaultAllocationProfilerSample> Samples => _samples;

  internal void Record(string surface, string stepName, long allocatedBytes, TimeSpan elapsed) {
    if (_disposed) {
      return;
    }

    _samples.Add(new DataVaultAllocationProfilerSample(
        surface,
        stepName,
        WorkloadName,
        Iteration,
        allocatedBytes,
        elapsed));
  }

  public void Dispose() {
    if (_disposed) {
      return;
    }

    _disposed = true;
    _setCurrentSession(_previousSession);
  }
}
