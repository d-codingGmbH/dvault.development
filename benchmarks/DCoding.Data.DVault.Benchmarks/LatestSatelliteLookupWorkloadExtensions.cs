using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable EF1003 // Benchmark index variants use fixed produced table and index names with local quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal static class LatestSatelliteLookupWorkloadExtensions {
  public static string ToDisplayText(this LatestSatelliteLookupWorkload workload) {
    return workload switch {
      LatestSatelliteLookupWorkload.UnchangedReplay => "unchanged replay",
      LatestSatelliteLookupWorkload.ChangedReplay => "changed replay",
      _ => throw new ArgumentOutOfRangeException(nameof(workload), workload, "Unsupported latest lookup workload."),
    };
  }
}
