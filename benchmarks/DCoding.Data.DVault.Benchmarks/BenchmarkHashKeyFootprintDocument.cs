namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkHashKeyFootprintDocument(
    BenchmarkRunContext Context,
    IReadOnlyList<BenchmarkHashKeyFootprintRow> Rows);
