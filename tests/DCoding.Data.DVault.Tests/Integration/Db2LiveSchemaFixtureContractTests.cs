using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.Db2Provider)]
public sealed class Db2LiveSchemaFixtureContractTests {
  [Fact]
  public void FixtureUsesDb2PhysicalNameOverridesWithoutChangingColumnContract() {
    var options = ExternalProviderLiveSchemaModelOptions.ForDb2("ABCDEF0123456789");

    Assert.Null(options.DefaultSchema);
    Assert.Same(DataVaultProviderCapabilityProfiles.Db2, options.ProviderCapabilities);
    Assert.All(options.ExpectedSnapshot.Tables, table => Assert.InRange(table.TableName.Length, 1, 128));
    Assert.All(
        options.ExpectedSnapshot.Tables,
        table => Assert.All(
            table.Indexes.Select(index => index.IndexName).Append(table.PrimaryKey.ConstraintName),
            identifierName => Assert.InRange(identifierName.Length, 1, 128)));
    Assert.Contains(
        options.ExpectedSnapshot.Tables.Single(table => table.TableName.StartsWith("DVHCU", StringComparison.Ordinal)).Columns,
        column => column.ColumnName == "CustomerId" &&
            column.ProviderStorageType == "VARCHAR(255)");
  }
}
