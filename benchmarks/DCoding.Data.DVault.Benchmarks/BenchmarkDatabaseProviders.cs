using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.Benchmarks;

internal abstract class BenchmarkDatabaseProvider {
  protected BenchmarkDatabaseProvider(string providerName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);

    ProviderName = providerName;
  }

  public string ProviderName { get; }

  public abstract IBenchmarkDatabase CreateDatabase();
}

internal interface IBenchmarkDatabase : IDisposable {
  DbContextOptions<TContext> CreateOptions<TContext>()
      where TContext : DbContext;

  Task InitializeAsync(DbContext context, CancellationToken cancellationToken);

  Task CleanupAsync(DbContext context, CancellationToken cancellationToken);
}

internal static class BenchmarkDatabaseProviders {
  public static BenchmarkDatabaseProvider Sqlite { get; } = new SqliteBenchmarkDatabaseProvider();

  public static BenchmarkDatabaseProvider CreatePostgres(string connectionString) {
    return new PostgresBenchmarkDatabaseProvider(connectionString);
  }

  private sealed class SqliteBenchmarkDatabaseProvider : BenchmarkDatabaseProvider {
    public SqliteBenchmarkDatabaseProvider()
        : base(BenchmarkArtifacts.RequiredProviderName) {
    }

    public override IBenchmarkDatabase CreateDatabase() {
      return TempSqliteDatabase.Create();
    }
  }

  private sealed class PostgresBenchmarkDatabaseProvider : BenchmarkDatabaseProvider {
    private readonly string _connectionString;

    public PostgresBenchmarkDatabaseProvider(string connectionString)
        : base(PostgresBenchmarkAvailability.ProviderName) {
      _connectionString = connectionString;
    }

    public override IBenchmarkDatabase CreateDatabase() {
      if (string.IsNullOrWhiteSpace(_connectionString)) {
        throw new InvalidOperationException(
            "PostgreSQL benchmark rows cannot execute without " +
            PostgresBenchmarkAvailability.ConnectionStringEnvironmentVariable +
            ".");
      }

      return new TempPostgresSchemaDatabase(_connectionString);
    }
  }
}

internal sealed class TempPostgresSchemaDatabase : IBenchmarkDatabase {
  private readonly string _baseConnectionString;
  private readonly string _schemaName;
  private readonly string _schemaConnectionString;

  public TempPostgresSchemaDatabase(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    _baseConnectionString = connectionString;
    _schemaName = "dvault_bench_" + Guid.NewGuid().ToString("N");
    _schemaConnectionString = AppendSearchPath(connectionString, _schemaName);
  }

  public DbContextOptions<TContext> CreateOptions<TContext>()
      where TContext : DbContext {
    var builder = new DbContextOptionsBuilder<TContext>();
    NpgsqlReflection.UseNpgsql(builder, _schemaConnectionString);

    return builder.Options;
  }

  public async Task InitializeAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    await ExecuteSchemaCommandAsync(
        "CREATE SCHEMA IF NOT EXISTS " + QuotePostgresIdentifier(_schemaName),
        cancellationToken).ConfigureAwait(false);
  }

  public async Task CleanupAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    await ExecuteSchemaCommandAsync(
        "DROP SCHEMA IF EXISTS " + QuotePostgresIdentifier(_schemaName) + " CASCADE",
        cancellationToken).ConfigureAwait(false);
  }

  public void Dispose() {
  }

  private async Task ExecuteSchemaCommandAsync(string commandText, CancellationToken cancellationToken) {
    await using var connection = NpgsqlReflection.CreateConnection(_baseConnectionString);
    await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

    await using var command = connection.CreateCommand();
    command.CommandText = commandText;
    await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static string AppendSearchPath(string connectionString, string schemaName) {
    var separator = connectionString.EndsWith(';') ? string.Empty : ";";

    return connectionString + separator + "Search Path=" + schemaName;
  }

  private static string QuotePostgresIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}

internal static class NpgsqlReflection {
  private const string NpgsqlOptionsExtensionTypeName =
      "Microsoft.EntityFrameworkCore.NpgsqlDbContextOptionsBuilderExtensions, Npgsql.EntityFrameworkCore.PostgreSQL";
  private const string NpgsqlConnectionTypeName = "Npgsql.NpgsqlConnection, Npgsql";

  public static bool IsProviderDependencyAvailable() {
    return GetUseNpgsqlMethod() is not null && GetConnectionType() is not null;
  }

  public static DbConnection CreateConnection(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var connectionType = GetConnectionType();
    if (connectionType is null) {
      throw new InvalidOperationException("Npgsql is not available to the benchmark process.");
    }

    return (DbConnection)Activator.CreateInstance(connectionType, connectionString)!;
  }

  public static void UseNpgsql(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var method = GetUseNpgsqlMethod();
    if (method is null) {
      throw new InvalidOperationException("Npgsql.EntityFrameworkCore.PostgreSQL is not available to the benchmark process.");
    }

    var parameters = method.GetParameters();
    var arguments = new object?[parameters.Length];
    arguments[0] = optionsBuilder;
    arguments[1] = connectionString;

    method.Invoke(null, arguments);
  }

  private static Type? GetConnectionType() {
    var connectionType = Type.GetType(NpgsqlConnectionTypeName, throwOnError: false);
    if (connectionType is null || !typeof(DbConnection).IsAssignableFrom(connectionType)) {
      return null;
    }

    return connectionType;
  }

  private static MethodInfo? GetUseNpgsqlMethod() {
    var extensionType = Type.GetType(NpgsqlOptionsExtensionTypeName, throwOnError: false);
    if (extensionType is null) {
      return null;
    }

    return extensionType
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(method => string.Equals(method.Name, "UseNpgsql", StringComparison.Ordinal) && !method.IsGenericMethod)
        .Select(method => new {
          Method = method,
          Parameters = method.GetParameters(),
        })
        .Where(candidate =>
            candidate.Parameters.Length >= 2 &&
            candidate.Parameters[0].ParameterType.IsAssignableFrom(typeof(DbContextOptionsBuilder)) &&
            candidate.Parameters[1].ParameterType == typeof(string))
        .OrderBy(candidate => candidate.Parameters.Length)
        .Select(candidate => candidate.Method)
        .FirstOrDefault();
  }
}
