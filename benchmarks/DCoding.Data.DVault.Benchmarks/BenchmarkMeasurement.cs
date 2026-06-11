using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkMeasurement(TimeSpan Elapsed, long AllocatedBytes);
