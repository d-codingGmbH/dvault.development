namespace DCoding.Data.DVault.Benchmarks;

internal sealed class PostgresBenchmarkAvailability {
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
      CancellationToken cancellationToken) {
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

    var connectionFailure = await tryOpenConnectionAsync(connectionString, cancellationToken).ConfigureAwait(false);
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
