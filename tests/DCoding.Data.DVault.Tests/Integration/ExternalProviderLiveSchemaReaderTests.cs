using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
public sealed class PostgresLiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingPostgresLiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreatePostgresAsync);
  }
}

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
public sealed class SqlServerLiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingSqlServerLiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreateSqlServerAsync);
  }
}

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]
public sealed class OracleLiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingOracleLiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreateOracleAsync);
  }
}

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
public sealed class MySqlLiveSchemaReaderTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingMySqlLiveSchemaWithNoDrift() {
    await ExternalProviderLiveSchemaReaderAssertions.AssertReadAsyncMatchesExpectedSnapshotAsync(
        ExternalProviderLiveSchemaFixture.CreateMySqlAsync);
  }
}

internal static class ExternalProviderLiveSchemaReaderAssertions {
  public static async Task AssertReadAsyncMatchesExpectedSnapshotAsync(
      Func<Task<ExternalProviderLiveSchemaFixture>> createFixtureAsync) {
    await using var fixture = await createFixtureAsync().ConfigureAwait(false);
    await using var context = fixture.CreateContext();

    var readResult = await DataVaultLiveSchemaReader.ReadAsync(context).ConfigureAwait(false);

    Assert.Equal(DataVaultLiveSchemaReadStatus.Succeeded, readResult.Status);
    Assert.NotNull(readResult.Snapshot);
    Assert.Equal(
        LiveSchemaReaderContractFixture.CreateSnapshotSignatures(fixture.ExpectedSnapshot),
        LiveSchemaReaderContractFixture.CreateSnapshotSignatures(readResult.Snapshot));
  }
}
