using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class TempDb2Database : SharedExternalBenchmarkDatabase {
  private readonly string _connectionString;

  public TempDb2Database(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    _connectionString = connectionString;
  }

  public override DbContextOptions<TContext> CreateOptions<TContext>() {
    var builder = new DbContextOptionsBuilder<TContext>();
    Db2BenchmarkReflection.UseDb2(builder, _connectionString);
    builder.ReplaceService<IModelCacheKeyFactory, BenchmarkDataVaultModelCacheKeyFactory>();

    return builder.Options;
  }

  public override async Task EnsureCreatedAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    await context.GetService<IRelationalDatabaseCreator>()
        .CreateTablesAsync(cancellationToken)
        .ConfigureAwait(false);
  }

  public override async Task CleanupAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    foreach (var tableName in GetProducedTableNames()) {
      await DropTableIfExistsAsync(context, tableName, cancellationToken).ConfigureAwait(false);
      await DropTableIfExistsAsync(context, tableName.ToUpperInvariant(), cancellationToken).ConfigureAwait(false);
    }
  }

  private static async Task DropTableIfExistsAsync(
      DbContext context,
      string tableName,
      CancellationToken cancellationToken) {
    try {
      await context.Database.ExecuteSqlRawAsync(
          "DROP TABLE " + QuoteDb2Identifier(tableName),
          cancellationToken).ConfigureAwait(false);
    }
    catch (Exception exception) when (IsUndefinedDb2Object(exception)) {
    }
  }

  private static bool IsUndefinedDb2Object(Exception exception) {
    var message = exception.ToString();

    return message.Contains("SQLSTATE=42704", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("SQL0204N", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("undefined name", StringComparison.OrdinalIgnoreCase);
  }

  private static string QuoteDb2Identifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}

#pragma warning restore EF1003
