using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]
public sealed class OracleIntegrationTestConfigurationTests {
  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsAbsent() {
    var configuration = OracleIntegrationTestConfiguration.FromEnvironment(_ => null);

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsWhiteSpace() {
    var configuration = OracleIntegrationTestConfiguration.FromEnvironment(_ => "  ");

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationUsesDeveloperSuppliedConnectionString() {
    var configuration = OracleIntegrationTestConfiguration.FromEnvironment(
        name => name == OracleIntegrationTestConfiguration.ConnectionStringEnvironmentVariable
            ? " User Id=dvault;Password=local-secret;Data Source=localhost:1521/FREEPDB1 "
            : null);

    Assert.True(configuration.IsConfigured);
    Assert.Equal(
        "User Id=dvault;Password=local-secret;Data Source=localhost:1521/FREEPDB1",
        configuration.ConnectionString);
  }

  [Fact]
  public void MissingConfigurationSkipMessageNamesLocalOracleOptInContract() {
    Assert.Contains(
        "local Oracle configuration is missing",
        OracleIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        OracleIntegrationTestConfiguration.ConnectionStringEnvironmentVariable,
        OracleIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        "Oracle database provisioning is external to DVault",
        OracleIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
  }
}
