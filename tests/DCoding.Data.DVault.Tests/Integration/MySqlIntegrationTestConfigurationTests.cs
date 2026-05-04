using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
public sealed class MySqlIntegrationTestConfigurationTests {
  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsAbsent() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment(_ => null);

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsWhiteSpace() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment(_ => "  ");

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationUsesDeveloperSuppliedConnectionString() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment(
        name => name == MySqlIntegrationTestConfiguration.ConnectionStringEnvironmentVariable
            ? " Server=localhost;Database=dvault_tests;User=dvault;Password=local-secret "
            : null);

    Assert.True(configuration.IsConfigured);
    Assert.Equal(
        "Server=localhost;Database=dvault_tests;User=dvault;Password=local-secret",
        configuration.ConnectionString);
  }

  [Fact]
  public void MissingConfigurationSkipMessageNamesLocalMySqlOptInContract() {
    Assert.Contains(
        "local MySQL configuration is missing",
        MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        MySqlIntegrationTestConfiguration.ConnectionStringEnvironmentVariable,
        MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        "Docker and database provisioning are external to DVault",
        MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
  }
}
