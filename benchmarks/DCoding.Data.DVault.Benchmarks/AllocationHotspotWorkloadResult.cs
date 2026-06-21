using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record AllocationHotspotWorkloadResult(
    AllocationHotspotWorkload Workload,
    IReadOnlyList<BenchmarkMeasurement> IterationMeasurements,
    IReadOnlyList<DataVaultAllocationProfilerSample> Samples,
    string PersistedOutcome) {
  public AllocationHotspotWorkloadSummary ToSummary() {
    return new AllocationHotspotWorkloadSummary(
        Workload.WorkloadName,
        Workload.DatasetSize,
        Workload.ChangeRatio,
        PersistedOutcome);
  }
}
