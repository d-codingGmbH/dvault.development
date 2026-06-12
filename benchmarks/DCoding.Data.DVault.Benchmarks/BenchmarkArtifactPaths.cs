using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkArtifactPaths(
    string MarkdownPath,
    string CsvPath,
    string JsonPath,
    BenchmarkHashKeyFootprintArtifactPaths? HashKeyFootprintPaths = null);
