using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
public sealed class SqlServerLiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingSqlServerLiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreateSqlServerAsync);
  }
}
