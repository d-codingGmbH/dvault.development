using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
public sealed class MySqlLiveSchemaFixtureContractTests {
  [Fact]
  public void FixtureUsesMySqlTablePrefixAndPhysicalIdentifierLengthContract() {
    var options = ExternalProviderLiveSchemaModelOptions.ForMySql("DVL123456789abc_");

    Assert.Null(options.DefaultSchema);
    Assert.Equal("DVL123456789abc_", options.TableNamePrefix);
    Assert.Same(DataVaultProviderCapabilityProfiles.MySql, options.ProviderCapabilities);
    Assert.All(
        options.ExpectedSnapshot.Tables,
        table => Assert.StartsWith("DVL123456789abc_", table.TableName, StringComparison.Ordinal));

    var longIndex = options.ExpectedSnapshot.Tables
        .Single(table => table.TableName == "DVL123456789abc_SatCustomerOrderState")
        .Indexes
        .Single()
        .IndexName;

    Assert.Equal(64, longIndex.Length);
    Assert.StartsWith("IxSatCustomerOrderStateSatelliteParentCustomerOrderHash", longIndex, StringComparison.Ordinal);
  }
}
