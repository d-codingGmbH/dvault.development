using System.Data.Common;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkExternalProviderDefinitions {
  public static BenchmarkExternalProviderDefinition Postgres { get; } = new(
      PostgresBenchmarkAvailability.ProviderName,
      PostgresBenchmarkAvailability.ConnectionStringEnvironmentVariable,
      "Npgsql.EntityFrameworkCore.PostgreSQL",
      BenchmarkDatabaseProviders.CreatePostgres,
      NpgsqlReflection.IsProviderDependencyAvailable,
      TryOpenPostgresConnectionAsync);

  public static BenchmarkExternalProviderDefinition SqlServer { get; } = new(
      "SQL Server external provider",
      "DVAULT_TEST_SQLSERVER_CONNECTION_STRING",
      "Microsoft.EntityFrameworkCore.SqlServer",
      BenchmarkDatabaseProviders.CreateSqlServer,
      SqlServerBenchmarkReflection.IsProviderDependencyAvailable,
      SqlServerBenchmarkReflection.TryOpenConnectionAsync);

  public static BenchmarkExternalProviderDefinition MySql { get; } = new(
      "MySQL external provider",
      "DVAULT_TEST_MYSQL_CONNECTION_STRING",
      "MySql.EntityFrameworkCore",
      BenchmarkDatabaseProviders.CreateMySql,
      MySqlBenchmarkReflection.IsProviderDependencyAvailable,
      MySqlBenchmarkReflection.TryOpenConnectionAsync);

  public static BenchmarkExternalProviderDefinition Oracle { get; } = new(
      "Oracle external provider",
      "DVAULT_TEST_ORACLE_CONNECTION_STRING",
      "Oracle.EntityFrameworkCore",
      BenchmarkDatabaseProviders.CreateOracle,
      OracleBenchmarkReflection.IsProviderDependencyAvailable,
      OracleBenchmarkReflection.TryOpenConnectionAsync);

  public static BenchmarkExternalProviderDefinition Db2 { get; } = new(
      "DB2 external provider",
      "DVAULT_TEST_DB2_CONNECTION_STRING",
      "IBM.EntityFrameworkCore",
      BenchmarkDatabaseProviders.CreateDb2,
      Db2BenchmarkReflection.IsProviderDependencyAvailable,
      Db2BenchmarkReflection.TryOpenConnectionAsync);

  private static async Task<string?> TryOpenPostgresConnectionAsync(
      string connectionString,
      CancellationToken cancellationToken) {
    try {
      await using var connection = NpgsqlReflection.CreateConnection(connectionString);
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
      await connection.CloseAsync().ConfigureAwait(false);

      return null;
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      return BenchmarkProviderDiagnostics.NormalizeExceptionMessage(exception);
    }
  }
}
