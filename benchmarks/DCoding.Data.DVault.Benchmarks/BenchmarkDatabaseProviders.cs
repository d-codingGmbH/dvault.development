using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

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

    public override DataVaultProviderCapabilityProfile ProviderCapabilities => DataVaultProviderCapabilityProfiles.Postgres;
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

    public override DataVaultProviderCapabilityProfile ProviderCapabilities => DataVaultProviderCapabilityProfiles.SqlServer;
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
