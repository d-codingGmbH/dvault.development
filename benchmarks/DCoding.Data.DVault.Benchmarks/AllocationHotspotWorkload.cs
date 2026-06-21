namespace DCoding.Data.DVault.Benchmarks;

internal sealed record AllocationHotspotWorkload(
    string WorkloadName,
    string BaselineName,
    string DatasetSize,
    string ChangeRatio,
    string ExecutionDetail,
    Func<int, bool, CancellationToken, Task<AllocationHotspotIterationResult>> ExecuteIterationAsync,
    int IterationCount = 0);
