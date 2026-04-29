using System.Reflection;
using System.Runtime.InteropServices;

namespace DCoding.Data.DVault.Tests.Shared;

public sealed class SqliteTestDatabase : IDisposable, IAsyncDisposable
{
    private readonly SqliteTestConnection _anchorConnection;
    private readonly string? _temporaryDirectoryPath;
    private bool _disposed;

    private SqliteTestDatabase(SqliteTestConnection anchorConnection, string connectionString, string? databasePath, string? temporaryDirectoryPath)
    {
        _anchorConnection = anchorConnection;
        ConnectionString = connectionString;
        DatabasePath = databasePath;
        _temporaryDirectoryPath = temporaryDirectoryPath;
    }

    public string ConnectionString { get; }

    public string? DatabasePath { get; }

    public static SqliteTestDatabase CreateInMemory()
    {
        var connectionString = $"file:dvault-tests-{Guid.NewGuid():N}?mode=memory&cache=shared";

        return Open(connectionString, databasePath: null, temporaryDirectoryPath: null);
    }

    public static SqliteTestDatabase CreateTemporaryFile()
    {
        var temporaryDirectoryPath = Path.Combine(Path.GetTempPath(), "dvault-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectoryPath);

        var databasePath = Path.Combine(temporaryDirectoryPath, "test.db");
        return Open(databasePath, databasePath, temporaryDirectoryPath);
    }

    public SqliteTestConnection CreateOpenConnection()
    {
        ThrowIfDisposed();

        return SqliteTestConnection.Open(ConnectionString);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _anchorConnection.Dispose();

        if (_temporaryDirectoryPath is not null && Directory.Exists(_temporaryDirectoryPath))
        {
            Directory.Delete(_temporaryDirectoryPath, recursive: true);
        }

        _disposed = true;
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    private static SqliteTestDatabase Open(string connectionString, string? databasePath, string? temporaryDirectoryPath)
    {
        var anchorConnection = SqliteTestConnection.Open(connectionString);

        return new SqliteTestDatabase(anchorConnection, connectionString, databasePath, temporaryDirectoryPath);
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}

public sealed class SqliteTestConnection : IDisposable
{
    private const int SqliteOk = 0;
    private const int SqliteRow = 100;
    private const int SqliteDone = 101;
    private const int SqliteOpenReadWrite = 0x00000002;
    private const int SqliteOpenCreate = 0x00000004;
    private const int SqliteOpenUri = 0x00000040;

    private IntPtr _database;

    private SqliteTestConnection(IntPtr database)
    {
        _database = database;
    }

    public static SqliteTestConnection Open(string connectionString)
    {
        var result = NativeMethods.sqlite3_open_v2(
            connectionString,
            out var database,
            SqliteOpenReadWrite | SqliteOpenCreate | SqliteOpenUri,
            zVfs: null);

        if (result != SqliteOk)
        {
            var message = database == IntPtr.Zero ? "SQLite failed to open the database." : GetErrorMessage(database);
            Close(database);
            throw new InvalidOperationException(message);
        }

        return new SqliteTestConnection(database);
    }

    public void ExecuteNonQuery(string commandText)
    {
        ThrowIfDisposed();

        var result = NativeMethods.sqlite3_exec(_database, commandText, callback: IntPtr.Zero, firstArgument: IntPtr.Zero, out var errorMessagePointer);
        if (result == SqliteOk)
        {
            return;
        }

        var errorMessage = errorMessagePointer == IntPtr.Zero
            ? GetErrorMessage(_database)
            : Marshal.PtrToStringUTF8(errorMessagePointer) ?? "SQLite command failed.";

        if (errorMessagePointer != IntPtr.Zero)
        {
            NativeMethods.sqlite3_free(errorMessagePointer);
        }

        throw new InvalidOperationException(errorMessage);
    }

    public string? ExecuteScalarString(string commandText)
    {
        ThrowIfDisposed();

        var prepareResult = NativeMethods.sqlite3_prepare_v2(_database, commandText, byteCount: -1, out var statement, tail: IntPtr.Zero);
        if (prepareResult != SqliteOk)
        {
            throw new InvalidOperationException(GetErrorMessage(_database));
        }

        try
        {
            var stepResult = NativeMethods.sqlite3_step(statement);
            return stepResult switch
            {
                SqliteRow => Marshal.PtrToStringUTF8(NativeMethods.sqlite3_column_text(statement, columnIndex: 0)),
                SqliteDone => null,
                _ => throw new InvalidOperationException(GetErrorMessage(_database))
            };
        }
        finally
        {
            NativeMethods.sqlite3_finalize(statement);
        }
    }

    public void Dispose()
    {
        if (_database == IntPtr.Zero)
        {
            return;
        }

        Close(_database);
        _database = IntPtr.Zero;
    }

    private static void Close(IntPtr database)
    {
        if (database != IntPtr.Zero)
        {
            NativeMethods.sqlite3_close_v2(database);
        }
    }

    private static string GetErrorMessage(IntPtr database)
    {
        return Marshal.PtrToStringUTF8(NativeMethods.sqlite3_errmsg(database)) ?? "SQLite command failed.";
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_database == IntPtr.Zero, this);
    }

    private static class NativeMethods
    {
        private const string SqliteLibraryName = "sqlite3";

        static NativeMethods()
        {
            NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, ResolveSqliteLibrary);
        }

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sqlite3_open_v2(
            string filename,
            out IntPtr database,
            int flags,
            string? zVfs);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sqlite3_close_v2(IntPtr database);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sqlite3_errmsg(IntPtr database);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sqlite3_exec(
            IntPtr database,
            string sql,
            IntPtr callback,
            IntPtr firstArgument,
            out IntPtr errorMessage);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sqlite3_free(IntPtr pointer);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sqlite3_prepare_v2(
            IntPtr database,
            string sql,
            int byteCount,
            out IntPtr statement,
            IntPtr tail);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sqlite3_step(IntPtr statement);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sqlite3_column_text(IntPtr statement, int columnIndex);

        [DllImport(SqliteLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sqlite3_finalize(IntPtr statement);

        private static IntPtr ResolveSqliteLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != SqliteLibraryName)
            {
                return IntPtr.Zero;
            }

            foreach (var candidate in new[] { "sqlite3", "libsqlite3.so.0", "libsqlite3.so", "winsqlite3", "winsqlite3.dll", "sqlite3.dll", "libsqlite3.dylib" })
            {
                if (NativeLibrary.TryLoad(candidate, assembly, searchPath, out var handle))
                {
                    return handle;
                }
            }

            return IntPtr.Zero;
        }
    }
}
