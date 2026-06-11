using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

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
