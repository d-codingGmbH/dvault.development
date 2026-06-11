using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

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
