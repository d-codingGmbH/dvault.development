using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.Db2Provider)]
public sealed class Db2IntegrationTestConfigurationTests {
  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsAbsent() {
    var configuration = Db2IntegrationTestConfiguration.FromEnvironment(_ => null);

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsWhiteSpace() {
    var configuration = Db2IntegrationTestConfiguration.FromEnvironment(_ => "  ");

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationUsesDeveloperSuppliedConnectionString() {
    var configuration = Db2IntegrationTestConfiguration.FromEnvironment(
        name => name == Db2IntegrationTestConfiguration.ConnectionStringEnvironmentVariable
            ? " Server=localhost:50000;Database=dvault;UID=dvault;PWD=local-secret "
            : null);

    Assert.True(configuration.IsConfigured);
    Assert.Equal(
        "Server=localhost:50000;Database=dvault;UID=dvault;PWD=local-secret",
        configuration.ConnectionString);
  }

  [Fact]
  public void MissingConfigurationSkipMessageNamesLocalDb2OptInContract() {
    Assert.Contains(
        "local DB2 configuration is missing",
        Db2IntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        Db2IntegrationTestConfiguration.ConnectionStringEnvironmentVariable,
        Db2IntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        "DB2 database provisioning is external to DVault",
        Db2IntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
  }
}
