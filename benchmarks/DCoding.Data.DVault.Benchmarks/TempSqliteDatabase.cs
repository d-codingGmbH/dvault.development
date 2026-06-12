using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class TempSqliteDatabase : IBenchmarkDatabase {
  private readonly string _directoryPath;
  private bool _disposed;

  private TempSqliteDatabase(string directoryPath, string connectionString) {
    _directoryPath = directoryPath;
    ConnectionString = connectionString;
  }

  public string ConnectionString { get; }

  public static TempSqliteDatabase Create() {
    var directoryPath = Path.Combine(Path.GetTempPath(), "dvault-benchmarks", Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(directoryPath);

    var databasePath = Path.Combine(directoryPath, "benchmark.db");
    return new TempSqliteDatabase(directoryPath, "Data Source=" + databasePath + ";Pooling=False");
  }

  public DbContextOptions<TContext> CreateOptions<TContext>()
      where TContext : DbContext {
    return new DbContextOptionsBuilder<TContext>()
        .UseSqlite(ConnectionString)
        .ReplaceService<IModelCacheKeyFactory, BenchmarkDataVaultModelCacheKeyFactory>()
        .Options;
  }

  public Task InitializeAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    return Task.CompletedTask;
  }

  public Task CleanupAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    return Task.CompletedTask;
  }

  public void Dispose() {
    if (_disposed) {
      return;
    }

    if (Directory.Exists(_directoryPath)) {
      Directory.Delete(_directoryPath, recursive: true);
    }

    _disposed = true;
  }
}
