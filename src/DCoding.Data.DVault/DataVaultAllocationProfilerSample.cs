namespace DCoding.Data.DVault;

internal sealed record DataVaultAllocationProfilerSample(
    string Surface,
    string StepName,
    string WorkloadName,
    int Iteration,
    long AllocatedBytes,
    TimeSpan Elapsed);
