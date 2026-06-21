namespace DCoding.Data.DVault.Benchmarks;

internal sealed record AllocationHotspotWorkloadSummary(
    string WorkloadName,
    string DatasetSize,
    string ChangeRatio,
    string PersistedOutcome);
