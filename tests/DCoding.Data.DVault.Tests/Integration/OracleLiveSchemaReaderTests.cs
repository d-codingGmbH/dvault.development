using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]
public sealed class OracleLiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingOracleLiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreateOracleAsync);
  }
}
