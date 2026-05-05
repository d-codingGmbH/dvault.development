using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal abstract class BenchmarkDatabaseProvider {
  protected BenchmarkDatabaseProvider(string providerName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);

    ProviderName = providerName;
  }

  public string ProviderName { get; }

  public virtual DataVaultProviderCapabilityProfile ProviderCapabilities => DataVaultProviderCapabilityProfiles.Sqlite;

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

  public static BenchmarkDatabaseProvider CreateSqlServer(string connectionString) {
    return new SqlServerBenchmarkDatabaseProvider(connectionString);
  }

  public static BenchmarkDatabaseProvider CreateMySql(string connectionString) {
    return new MySqlBenchmarkDatabaseProvider(connectionString);
  }

  public static BenchmarkDatabaseProvider CreateOracle(string connectionString) {
    return new OracleBenchmarkDatabaseProvider(connectionString);
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

  private sealed class SqlServerBenchmarkDatabaseProvider : BenchmarkDatabaseProvider {
    private readonly string _connectionString;

    public SqlServerBenchmarkDatabaseProvider(string connectionString)
        : base(BenchmarkExternalProviderDefinitions.SqlServer.ProviderName) {
      _connectionString = connectionString;
    }

    public override IBenchmarkDatabase CreateDatabase() {
      if (string.IsNullOrWhiteSpace(_connectionString)) {
        throw new InvalidOperationException(
            "SQL Server benchmark rows cannot execute without " +
            BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable +
            ".");
      }

      return new TempSqlServerDatabase(_connectionString);
    }
  }

  private sealed class MySqlBenchmarkDatabaseProvider : BenchmarkDatabaseProvider {
    private readonly string _connectionString;

    public MySqlBenchmarkDatabaseProvider(string connectionString)
        : base(BenchmarkExternalProviderDefinitions.MySql.ProviderName) {
      _connectionString = connectionString;
    }

    public override IBenchmarkDatabase CreateDatabase() {
      if (string.IsNullOrWhiteSpace(_connectionString)) {
        throw new InvalidOperationException(
            "MySQL benchmark rows cannot execute without " +
            BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable +
            ".");
      }

      return new TempMySqlDatabase(_connectionString);
    }

    public override DataVaultProviderCapabilityProfile ProviderCapabilities => DataVaultProviderCapabilityProfiles.MySql;
  }

  private sealed class OracleBenchmarkDatabaseProvider : BenchmarkDatabaseProvider {
    private readonly string _connectionString;

    public OracleBenchmarkDatabaseProvider(string connectionString)
        : base(BenchmarkExternalProviderDefinitions.Oracle.ProviderName) {
      _connectionString = connectionString;
    }

    public override IBenchmarkDatabase CreateDatabase() {
      if (string.IsNullOrWhiteSpace(_connectionString)) {
        throw new InvalidOperationException(
            "Oracle benchmark rows cannot execute without " +
            BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable +
            ".");
      }

      return new TempOracleDatabase(_connectionString);
    }

    public override DataVaultProviderCapabilityProfile ProviderCapabilities => DataVaultProviderCapabilityProfiles.Oracle;
  }
}

internal abstract class SharedExternalBenchmarkDatabase : IBenchmarkDatabase {
  private static readonly string[] ProducedTableNames = [
      "SatOrderProductFulfillment",
      "LinkOrderProduct",
      "SatCustomerProfile",
      "HubOrder",
      "HubProduct",
      "HubCustomer",
      "CustomerProfileBulkHistory",
  ];

  public abstract DbContextOptions<TContext> CreateOptions<TContext>()
      where TContext : DbContext;

  public virtual Task InitializeAsync(DbContext context, CancellationToken cancellationToken) {
    return CleanupAsync(context, cancellationToken);
  }

  public abstract Task CleanupAsync(DbContext context, CancellationToken cancellationToken);

  public void Dispose() {
  }

  protected static IReadOnlyList<string> GetProducedTableNames() {
    return ProducedTableNames;
  }
}

internal sealed class TempSqlServerDatabase : SharedExternalBenchmarkDatabase {
  private readonly string _connectionString;

  public TempSqlServerDatabase(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    _connectionString = connectionString;
  }

  public override DbContextOptions<TContext> CreateOptions<TContext>() {
    var builder = new DbContextOptionsBuilder<TContext>();
    SqlServerBenchmarkReflection.UseSqlServer(builder, _connectionString);

    return builder.Options;
  }

  public override async Task CleanupAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    foreach (var tableName in GetProducedTableNames()) {
      await context.Database.ExecuteSqlRawAsync(
          "IF OBJECT_ID(N'dbo." + SqlServerStringLiteralContent(tableName) + "', N'U') IS NOT NULL DROP TABLE " + QuoteSqlServerIdentifier(tableName),
          cancellationToken).ConfigureAwait(false);
    }
  }

  private static string SqlServerStringLiteralContent(string value) {
    return value.Replace("'", "''", StringComparison.Ordinal);
  }

  private static string QuoteSqlServerIdentifier(string identifier) {
    return "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }
}

internal sealed class TempMySqlDatabase : SharedExternalBenchmarkDatabase {
  private readonly string _connectionString;

  public TempMySqlDatabase(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    _connectionString = connectionString;
  }

  public override DbContextOptions<TContext> CreateOptions<TContext>() {
    var builder = new DbContextOptionsBuilder<TContext>();
    MySqlBenchmarkReflection.UseMySql(builder, _connectionString);

    return builder.Options;
  }

  public override async Task CleanupAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    await context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 0;", cancellationToken).ConfigureAwait(false);
    try {
      foreach (var tableName in GetProducedTableNames()) {
        await context.Database.ExecuteSqlRawAsync(
            "DROP TABLE IF EXISTS " + QuoteMySqlIdentifier(tableName) + ";",
            cancellationToken).ConfigureAwait(false);
      }
    }
    finally {
      await context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 1;", CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static string QuoteMySqlIdentifier(string identifier) {
    return "`" + identifier.Replace("`", "``", StringComparison.Ordinal) + "`";
  }
}

internal sealed class TempOracleDatabase : SharedExternalBenchmarkDatabase {
  private readonly string _connectionString;

  public TempOracleDatabase(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    _connectionString = connectionString;
  }

  public override DbContextOptions<TContext> CreateOptions<TContext>() {
    var builder = new DbContextOptionsBuilder<TContext>();
    OracleBenchmarkReflection.UseOracle(builder, _connectionString);

    return builder.Options;
  }

  public override async Task CleanupAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    foreach (var tableName in GetProducedTableNames()) {
      await context.Database.ExecuteSqlRawAsync(
          "BEGIN EXECUTE IMMEDIATE " +
          SqlLiteral("DROP TABLE " + QuoteOracleIdentifier(tableName) + " PURGE") +
          "; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;",
          cancellationToken).ConfigureAwait(false);
    }
  }

  private static string QuoteOracleIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string SqlLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
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

#pragma warning restore EF1003

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
