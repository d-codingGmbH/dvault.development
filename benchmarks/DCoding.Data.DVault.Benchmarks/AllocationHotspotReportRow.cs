namespace DCoding.Data.DVault.Benchmarks;

internal sealed record AllocationHotspotReportRow(
    int Rank,
    string Surface,
    string StepName,
    string WorkloadName,
    string DatasetSize,
    string ChangeRatio,
    int Iterations,
    double MeanAllocatedBytes,
    long MinAllocatedBytes,
    long MaxAllocatedBytes,
    double MeanMilliseconds,
    double MeanCallCount,
    string EvidencePosture,
    string Recommendation);
