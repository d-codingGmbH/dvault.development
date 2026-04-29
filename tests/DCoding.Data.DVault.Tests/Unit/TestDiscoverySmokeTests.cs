using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class TestDiscoverySmokeTests
{
    [Fact]
    public void UnitProjectRunsAndCanReferenceSharedTestUtilities()
    {
        using var database = SqliteTestDatabase.CreateInMemory();

        Assert.Contains("mode=memory", database.ConnectionString, StringComparison.OrdinalIgnoreCase);
        Assert.Null(database.DatabasePath);
    }
}
