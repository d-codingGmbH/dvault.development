using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
public sealed class MySqlLiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingMySqlLiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreateMySqlAsync);
  }
}
