namespace DCoding.Data.DVault.Benchmarks;

internal sealed record AllocationHotspotReportDocument(
    string SchemaVersion,
    string TicketId,
    BenchmarkRunContext Context,
    IReadOnlyList<AllocationHotspotWorkloadSummary> Workloads,
    IReadOnlyList<AllocationHotspotReportRow> RankedHotspots,
    IReadOnlyList<string> OptimizationOrder);
