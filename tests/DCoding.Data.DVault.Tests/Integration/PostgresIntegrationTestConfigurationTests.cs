using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
public sealed class PostgresIntegrationTestConfigurationTests {
  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsAbsent() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment(_ => null);

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsWhiteSpace() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment(_ => "  ");

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationUsesDeveloperSuppliedConnectionString() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment(
        name => name == PostgresIntegrationTestConfiguration.ConnectionStringEnvironmentVariable
            ? " Host=localhost;Database=dvault_tests;Username=dvault;Password=local-secret "
            : null);

    Assert.True(configuration.IsConfigured);
    Assert.Equal(
        "Host=localhost;Database=dvault_tests;Username=dvault;Password=local-secret",
        configuration.ConnectionString);
  }

  [Fact]
  public void MissingConfigurationSkipMessageNamesLocalPostgresOptInContract() {
    Assert.Contains(
        "local Postgres configuration is missing",
        PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        PostgresIntegrationTestConfiguration.ConnectionStringEnvironmentVariable,
        PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        "Docker and database provisioning are external to DVault",
        PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
  }
}
