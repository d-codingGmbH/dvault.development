using System.Reflection;
using System.Runtime.InteropServices;

namespace DCoding.Data.DVault.Tests.Shared;

public sealed class SqliteTestDatabase : IDisposable, IAsyncDisposable {
  private readonly SqliteTestConnection _anchorConnection;
  private readonly string? _temporaryDirectoryPath;
  private bool _disposed;

  private SqliteTestDatabase(SqliteTestConnection anchorConnection, string connectionString, string? databasePath, string? temporaryDirectoryPath) {
    _anchorConnection = anchorConnection;
    ConnectionString = connectionString;
    DatabasePath = databasePath;
    _temporaryDirectoryPath = temporaryDirectoryPath;
  }

  public string ConnectionString { get; }

  public string? DatabasePath { get; }

  public static SqliteTestDatabase CreateInMemory() {
    var connectionString = $"file:dvault-tests-{Guid.NewGuid():N}?mode=memory&cache=shared";

    return Open(connectionString, databasePath: null, temporaryDirectoryPath: null);
  }

  public static SqliteTestDatabase CreateTemporaryFile() {
    var temporaryDirectoryPath = Path.Combine(Path.GetTempPath(), "dvault-tests", Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(temporaryDirectoryPath);

    var databasePath = Path.Combine(temporaryDirectoryPath, "test.db");
    return Open(databasePath, databasePath, temporaryDirectoryPath);
  }

  public SqliteTestConnection CreateOpenConnection() {
    ThrowIfDisposed();

    return SqliteTestConnection.Open(ConnectionString);
  }

  public void Dispose() {
    if (_disposed) {
      return;
    }

    _anchorConnection.Dispose();

    if (_temporaryDirectoryPath is not null && Directory.Exists(_temporaryDirectoryPath)) {
      Directory.Delete(_temporaryDirectoryPath, recursive: true);
    }

    _disposed = true;
  }

  public ValueTask DisposeAsync() {
    Dispose();
    return ValueTask.CompletedTask;
  }

  private static SqliteTestDatabase Open(string connectionString, string? databasePath, string? temporaryDirectoryPath) {
    var anchorConnection = SqliteTestConnection.Open(connectionString);

    return new SqliteTestDatabase(anchorConnection, connectionString, databasePath, temporaryDirectoryPath);
  }

  private void ThrowIfDisposed() {
    ObjectDisposedException.ThrowIf(_disposed, this);
  }
}
