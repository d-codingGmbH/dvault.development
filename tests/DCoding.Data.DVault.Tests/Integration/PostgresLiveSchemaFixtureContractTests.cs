using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
public sealed class PostgresLiveSchemaFixtureContractTests {
  [Fact]
  public void FixtureUsesPostgresSchemaIsolationAndCanonicalExpectedSnapshot() {
    var options = ExternalProviderLiveSchemaModelOptions.ForPostgres("dvault_test_contract");

    Assert.Equal("dvault_test_contract", options.DefaultSchema);
    Assert.Equal(string.Empty, options.TableNamePrefix);
    Assert.Same(DataVaultProviderCapabilityProfiles.Postgres, options.ProviderCapabilities);
    Assert.Equal(
        LiveSchemaReaderContractFixture.ProducedTableNames,
        options.ExpectedSnapshot.Tables.Select(table => table.TableName));
    Assert.Contains(
        options.ExpectedSnapshot.Tables.Single(table => table.TableName == "HubCustomer").Columns,
        column => column.ColumnName == "LoadTimestamp" &&
            column.ProviderStorageType == "timestamp with time zone");
  }
}
