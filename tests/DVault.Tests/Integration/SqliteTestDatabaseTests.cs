using DVault.Tests.Shared;
using Xunit;

namespace DVault.Tests.Integration;

public sealed class SqliteTestDatabaseTests
{
    [Fact]
    public void InMemoryDatabaseIsSharedAcrossConnectionsUntilDisposed()
    {
        using var database = SqliteTestDatabase.CreateInMemory();

        using (var writeConnection = database.CreateOpenConnection())
        {
            writeConnection.ExecuteNonQuery(
                """
                CREATE TABLE entries (value TEXT NOT NULL);
                INSERT INTO entries (value) VALUES ('sqlite helper smoke');
                """);
        }

        using var readConnection = database.CreateOpenConnection();

        Assert.Equal("sqlite helper smoke", readConnection.ExecuteScalarString("SELECT value FROM entries"));
    }

    [Fact]
    public void TemporaryFileDatabaseDeletesItsDirectoryOnDispose()
    {
        string databasePath;
        string databaseDirectoryPath;

        using (var database = SqliteTestDatabase.CreateTemporaryFile())
        {
            databasePath = Assert.IsType<string>(database.DatabasePath);
            databaseDirectoryPath = Assert.IsType<string>(Path.GetDirectoryName(databasePath));

            using var connection = database.CreateOpenConnection();
            connection.ExecuteNonQuery("CREATE TABLE entries (id INTEGER NOT NULL);");

            Assert.True(File.Exists(databasePath));
        }

        Assert.False(Directory.Exists(databaseDirectoryPath));
    }
}