using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkProviderRunContext(
    string ProviderName,
    string ConnectionStringEnvironmentVariable,
    string ExecutionStatus,
    string SkipReason) {
  public static BenchmarkProviderRunContext FromAvailability(BenchmarkProviderAvailability availability) {
    ArgumentNullException.ThrowIfNull(availability);

    return new BenchmarkProviderRunContext(
        availability.ProviderName,
        availability.Definition.ConnectionStringEnvironmentVariable,
        availability.ExecutionStatus,
        availability.SkipReason?.DisplayText ?? string.Empty);
  }
}
