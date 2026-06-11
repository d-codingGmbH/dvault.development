using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class PostgresBenchmarkAvailability {
  private static readonly TimeSpan DefaultConnectionProbeTimeout = TimeSpan.FromSeconds(15);

  public const string ConnectionStringEnvironmentVariable = "DVAULT_TEST_POSTGRES_CONNECTION_STRING";
  public const string ProviderName = "PostgreSQL external provider";

  private PostgresBenchmarkAvailability(string? connectionString, BenchmarkSkipReason? skipReason) {
    ConnectionString = connectionString;
    SkipReason = skipReason;
    Provider = BenchmarkDatabaseProviders.CreatePostgres(connectionString ?? string.Empty);
  }

  public string? ConnectionString { get; }

  public BenchmarkSkipReason? SkipReason { get; }

  public BenchmarkDatabaseProvider Provider { get; }

  public bool IsAvailable => SkipReason is null;

  public string ExecutionStatus => IsAvailable ? BenchmarkExecutionStatus.Completed : BenchmarkExecutionStatus.Skipped;

  public static Task<PostgresBenchmarkAvailability> DiscoverAsync(CancellationToken cancellationToken) {
    return DiscoverAsync(
        Environment.GetEnvironmentVariable,
        NpgsqlReflection.IsProviderDependencyAvailable,
        TryOpenConnectionAsync,
        cancellationToken);
  }

  internal static PostgresBenchmarkAvailability Skipped(BenchmarkSkipReason skipReason) {
    ArgumentNullException.ThrowIfNull(skipReason);

    return new PostgresBenchmarkAvailability(null, skipReason);
  }

  internal static async Task<PostgresBenchmarkAvailability> DiscoverAsync(
      Func<string, string?> getEnvironmentVariable,
      Func<bool> isProviderDependencyAvailable,
      Func<string, CancellationToken, Task<string?>> tryOpenConnectionAsync,
      CancellationToken cancellationToken,
      TimeSpan? connectionProbeTimeout = null) {
    ArgumentNullException.ThrowIfNull(getEnvironmentVariable);
    ArgumentNullException.ThrowIfNull(isProviderDependencyAvailable);
    ArgumentNullException.ThrowIfNull(tryOpenConnectionAsync);

    var connectionString = Normalize(getEnvironmentVariable(ConnectionStringEnvironmentVariable));
    if (connectionString is null) {
      return Skipped(BenchmarkSkipReason.NotConfigured());
    }

    if (!isProviderDependencyAvailable()) {
      return Skipped(BenchmarkSkipReason.ProviderDependencyUnavailable());
    }

    var connectionFailure = await TryOpenConnectionWithTimeoutAsync(
        connectionString,
        tryOpenConnectionAsync,
        connectionProbeTimeout ?? DefaultConnectionProbeTimeout,
        cancellationToken)
        .ConfigureAwait(false);
    if (connectionFailure is not null) {
      return Skipped(BenchmarkSkipReason.ConnectionUnreachable(connectionFailure));
    }

    return new PostgresBenchmarkAvailability(connectionString, skipReason: null);
  }

  private static async Task<string?> TryOpenConnectionAsync(
      string connectionString,
      CancellationToken cancellationToken) {
    try {
      await using var connection = NpgsqlReflection.CreateConnection(connectionString);
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
      await connection.CloseAsync().ConfigureAwait(false);

      return null;
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      return NormalizeExceptionMessage(exception);
    }
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

  private static string? Normalize(string? value) {
    if (string.IsNullOrWhiteSpace(value)) {
      return null;
    }

    return value.Trim();
  }

  private static string NormalizeExceptionMessage(Exception exception) {
    var message = exception.GetBaseException().Message
        .Replace("\r", " ", StringComparison.Ordinal)
        .Replace("\n", " ", StringComparison.Ordinal)
        .Trim();

    const int maximumMessageLength = 240;
    if (message.Length <= maximumMessageLength) {
      return message;
    }

    return message[..maximumMessageLength] + "...";
  }
}
