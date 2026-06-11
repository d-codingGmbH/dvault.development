using System.Data.Common;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class BenchmarkProviderAvailability {
  private static readonly TimeSpan DefaultConnectionProbeTimeout = TimeSpan.FromSeconds(15);

  private BenchmarkProviderAvailability(
      BenchmarkExternalProviderDefinition definition,
      string? connectionString,
      BenchmarkSkipReason? skipReason) {
    Definition = definition;
    ConnectionString = connectionString;
    SkipReason = skipReason;
    Provider = definition.CreateProvider(connectionString ?? string.Empty);
  }

  public BenchmarkExternalProviderDefinition Definition { get; }

  public string? ConnectionString { get; }

  public BenchmarkSkipReason? SkipReason { get; }

  public BenchmarkDatabaseProvider Provider { get; }

  public string ProviderName => Definition.ProviderName;

  public bool IsAvailable => SkipReason is null;

  public string ExecutionStatus => IsAvailable ? BenchmarkExecutionStatus.Completed : BenchmarkExecutionStatus.Skipped;

  public static BenchmarkProviderAvailability FromPostgres(PostgresBenchmarkAvailability availability) {
    ArgumentNullException.ThrowIfNull(availability);

    return new BenchmarkProviderAvailability(
        BenchmarkExternalProviderDefinitions.Postgres,
        availability.ConnectionString,
        availability.SkipReason);
  }

  public static Task<BenchmarkProviderAvailability> DiscoverAsync(
      BenchmarkExternalProviderDefinition definition,
      CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(definition);

    return DiscoverAsync(
        definition,
        Environment.GetEnvironmentVariable,
        definition.IsProviderDependencyAvailable,
        definition.TryOpenConnectionAsync,
        cancellationToken);
  }

  internal static async Task<BenchmarkProviderAvailability> DiscoverAsync(
      BenchmarkExternalProviderDefinition definition,
      Func<string, string?> getEnvironmentVariable,
      Func<bool> isProviderDependencyAvailable,
      Func<string, CancellationToken, Task<string?>> tryOpenConnectionAsync,
      CancellationToken cancellationToken,
      TimeSpan? connectionProbeTimeout = null) {
    ArgumentNullException.ThrowIfNull(definition);
    ArgumentNullException.ThrowIfNull(getEnvironmentVariable);
    ArgumentNullException.ThrowIfNull(isProviderDependencyAvailable);
    ArgumentNullException.ThrowIfNull(tryOpenConnectionAsync);

    var connectionString = Normalize(getEnvironmentVariable(definition.ConnectionStringEnvironmentVariable));
    if (connectionString is null) {
      return Skipped(definition, BenchmarkSkipReason.NotConfigured(definition.ConnectionStringEnvironmentVariable));
    }

    if (!isProviderDependencyAvailable()) {
      return Skipped(definition, BenchmarkSkipReason.ProviderDependencyUnavailable(definition.ConnectionStringEnvironmentVariable, definition.PackageName));
    }

    var connectionFailure = await TryOpenConnectionWithTimeoutAsync(
        connectionString,
        tryOpenConnectionAsync,
        connectionProbeTimeout ?? DefaultConnectionProbeTimeout,
        cancellationToken)
        .ConfigureAwait(false);
    if (connectionFailure is not null) {
      return Skipped(definition, BenchmarkSkipReason.ConnectionUnreachable(definition.ConnectionStringEnvironmentVariable, connectionFailure));
    }

    return new BenchmarkProviderAvailability(definition, connectionString, skipReason: null);
  }

  internal static BenchmarkProviderAvailability Skipped(
      BenchmarkExternalProviderDefinition definition,
      BenchmarkSkipReason skipReason) {
    return new BenchmarkProviderAvailability(definition, null, skipReason);
  }

  private static string? Normalize(string? value) {
    if (string.IsNullOrWhiteSpace(value)) {
      return null;
    }

    return value.Trim();
  }

  private static async Task<string?> TryOpenConnectionWithTimeoutAsync(
      string connectionString,
      Func<string, CancellationToken, Task<string?>> tryOpenConnectionAsync,
      TimeSpan timeout,
      CancellationToken cancellationToken) {
    using var timeoutCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    timeoutCancellation.CancelAfter(timeout);

    try {
      return await tryOpenConnectionAsync(connectionString, timeoutCancellation.Token)
          .WaitAsync(timeout, cancellationToken)
          .ConfigureAwait(false);
    }
    catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested && timeoutCancellation.IsCancellationRequested) {
      return CreateConnectionProbeTimeoutMessage(timeout);
    }
    catch (TimeoutException) {
      return CreateConnectionProbeTimeoutMessage(timeout);
    }
  }

  private static string CreateConnectionProbeTimeoutMessage(TimeSpan timeout) {
    return "Timed out after " + timeout.TotalSeconds.ToString("0", CultureInfo.InvariantCulture) + " seconds while opening the connection.";
  }
}
