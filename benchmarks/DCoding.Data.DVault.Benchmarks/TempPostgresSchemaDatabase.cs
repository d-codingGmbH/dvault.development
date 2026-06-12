using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class TempPostgresSchemaDatabase : IBenchmarkDatabase {
  private readonly string _baseConnectionString;
  private readonly string _schemaName;
  private readonly string _schemaConnectionString;

  public TempPostgresSchemaDatabase(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    _baseConnectionString = DisablePooling(connectionString);
    _schemaName = "dvault_bench_" + Guid.NewGuid().ToString("N");
    _schemaConnectionString = AppendSearchPath(_baseConnectionString, _schemaName);
  }

  public DbContextOptions<TContext> CreateOptions<TContext>()
      where TContext : DbContext {
    var builder = new DbContextOptionsBuilder<TContext>();
    NpgsqlReflection.UseNpgsql(builder, _schemaConnectionString);
    builder.ReplaceService<IModelCacheKeyFactory, BenchmarkDataVaultModelCacheKeyFactory>();

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
    var builder = new DbConnectionStringBuilder {
      ConnectionString = connectionString,
      ["Search Path"] = schemaName,
    };

    return builder.ConnectionString;
  }

  private static string DisablePooling(string connectionString) {
    var builder = new DbConnectionStringBuilder {
      ConnectionString = connectionString,
      ["Pooling"] = "false",
    };

    return builder.ConnectionString;
  }

  private static string QuotePostgresIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}

#pragma warning restore EF1003
