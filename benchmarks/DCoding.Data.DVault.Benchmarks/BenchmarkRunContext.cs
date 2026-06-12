using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkRunContext(
    string Provider,
    string OptionalPostgresProvider,
    string PostgresExecutionStatus,
    string PostgresSkipReason,
    int Iterations,
    int WarmupIterations,
    string LoadTimestampStorage,
    string ProviderFilter,
    string OsDescription,
    string OsArchitecture,
    string ProcessArchitecture,
    int ProcessorCount,
    string DotNetRuntimeDescription,
    string DotNetRuntimeVersion,
    IReadOnlyList<BenchmarkHashKeyVariantRunContext> HashKeyVariants,
    IReadOnlyList<BenchmarkProviderRunContext> OptionalProviders) {
  public static BenchmarkRunContext Create(
      BenchmarkOptions options,
      PostgresBenchmarkAvailability postgresAvailability) {
    return Create(
        options,
        postgresAvailability,
        [BenchmarkProviderAvailability.FromPostgres(postgresAvailability)]);
  }

  public static BenchmarkRunContext Create(
      BenchmarkOptions options,
      PostgresBenchmarkAvailability postgresAvailability,
      IReadOnlyList<BenchmarkProviderAvailability> optionalProviders) {
    ArgumentNullException.ThrowIfNull(options);
    ArgumentNullException.ThrowIfNull(postgresAvailability);
    ArgumentNullException.ThrowIfNull(optionalProviders);

    return new BenchmarkRunContext(
        BenchmarkArtifacts.RequiredProviderName,
        PostgresBenchmarkAvailability.ProviderName,
        postgresAvailability.ExecutionStatus,
        postgresAvailability.SkipReason?.DisplayText ?? string.Empty,
        options.Iterations,
        options.WarmupIterations,
        options.LoadTimestampStorage.ToString(),
        options.ProviderFilter,
        RuntimeInformation.OSDescription,
        RuntimeInformation.OSArchitecture.ToString(),
        RuntimeInformation.ProcessArchitecture.ToString(),
        Environment.ProcessorCount,
        RuntimeInformation.FrameworkDescription,
        Environment.Version.ToString(),
        [.. options.EffectiveHashKeyVariants.Select(BenchmarkHashKeyVariantRunContext.FromVariant)],
        [.. optionalProviders.Select(BenchmarkProviderRunContext.FromAvailability)]);
  }
}
