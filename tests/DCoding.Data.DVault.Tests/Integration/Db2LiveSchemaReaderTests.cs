using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.Db2Provider)]
public sealed class Db2LiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingDb2LiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreateDb2Async);
  }
}
