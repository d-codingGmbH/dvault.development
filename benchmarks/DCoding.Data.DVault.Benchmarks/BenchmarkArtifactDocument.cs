using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkArtifactDocument(
    BenchmarkRunContext Context,
    IReadOnlyList<BenchmarkSummary> Results);
