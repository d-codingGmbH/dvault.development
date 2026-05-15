using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]
public sealed class OracleLiveSchemaFixtureContractTests {
  [Fact]
  public void FixtureUsesOraclePhysicalNameOverridesWithoutChangingColumnContract() {
    var options = ExternalProviderLiveSchemaModelOptions.ForOracle("ABCDEF0123456789");

    Assert.Null(options.DefaultSchema);
    Assert.Same(DataVaultProviderCapabilityProfiles.Oracle, options.ProviderCapabilities);
    Assert.All(options.ExpectedSnapshot.Tables, table => Assert.InRange(table.TableName.Length, 1, 30));
    Assert.All(
        options.ExpectedSnapshot.Tables,
        table => Assert.All(
            table.Indexes.Select(index => index.IndexName).Append(table.PrimaryKey.ConstraintName),
            identifierName => Assert.InRange(identifierName.Length, 1, 30)));
    Assert.Contains(
        options.ExpectedSnapshot.Tables.Single(table => table.TableName.StartsWith("DVHCU", StringComparison.Ordinal)).Columns,
        column => column.ColumnName == "CustomerId" &&
            column.ProviderStorageType == "VARCHAR2(255 CHAR)");
  }
}
