using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class TempMySqlDatabase : SharedExternalBenchmarkDatabase {
  private readonly string _connectionString;

  public TempMySqlDatabase(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    _connectionString = connectionString;
  }

  public override DbContextOptions<TContext> CreateOptions<TContext>() {
    var builder = new DbContextOptionsBuilder<TContext>();
    MySqlBenchmarkReflection.UseMySql(builder, _connectionString);
    builder.ReplaceService<IModelCacheKeyFactory, BenchmarkDataVaultModelCacheKeyFactory>();

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
