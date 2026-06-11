using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkSkipReason(string Category, string Message) {
  public string DisplayText => Category + ": " + Message;

  public static BenchmarkSkipReason NotConfigured() {
    return NotConfigured(PostgresBenchmarkAvailability.ConnectionStringEnvironmentVariable);
  }

  public static BenchmarkSkipReason NotConfigured(string connectionStringEnvironmentVariable) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionStringEnvironmentVariable);

    return new BenchmarkSkipReason(
        "not configured",
        connectionStringEnvironmentVariable + " is not set or empty.");
  }

  public static BenchmarkSkipReason ProviderDependencyUnavailable() {
    return ProviderDependencyUnavailable(
        PostgresBenchmarkAvailability.ConnectionStringEnvironmentVariable,
        "Npgsql.EntityFrameworkCore.PostgreSQL");
  }

  public static BenchmarkSkipReason ProviderDependencyUnavailable(
      string connectionStringEnvironmentVariable,
      string packageName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionStringEnvironmentVariable);
    ArgumentException.ThrowIfNullOrWhiteSpace(packageName);

    return new BenchmarkSkipReason(
        "provider dependency unavailable",
        packageName + " is not available to the benchmark process. Set " +
        connectionStringEnvironmentVariable +
        " before restore/build/run so the conditional package reference is included.");
  }

  public static BenchmarkSkipReason ConnectionUnreachable(string message) {
    return ConnectionUnreachable(PostgresBenchmarkAvailability.ConnectionStringEnvironmentVariable, message);
  }

  public static BenchmarkSkipReason ConnectionUnreachable(string connectionStringEnvironmentVariable, string message) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionStringEnvironmentVariable);
    ArgumentException.ThrowIfNullOrWhiteSpace(message);

    return new BenchmarkSkipReason(
        "connection unreachable",
        "A provider connection could not be opened with " +
        connectionStringEnvironmentVariable +
        ". " +
        message);
  }
}
