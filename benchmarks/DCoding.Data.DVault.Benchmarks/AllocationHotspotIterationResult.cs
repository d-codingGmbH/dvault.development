using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record AllocationHotspotIterationResult(
    BenchmarkMeasurement Measurement,
    IReadOnlyList<DataVaultAllocationProfilerSample> Samples,
    string PersistedOutcome);
