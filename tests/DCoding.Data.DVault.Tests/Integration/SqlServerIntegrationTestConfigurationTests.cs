using DCoding.Data.DVault.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
public sealed class SqlServerIntegrationTestConfigurationTests {
  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsAbsent() {
    var configuration = SqlServerIntegrationTestConfiguration.FromEnvironment(_ => null);

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationIsMissingWhenConnectionStringIsWhiteSpace() {
    var configuration = SqlServerIntegrationTestConfiguration.FromEnvironment(_ => "  ");

    Assert.False(configuration.IsConfigured);
    Assert.Null(configuration.ConnectionString);
  }

  [Fact]
  public void ConfigurationUsesDeveloperSuppliedConnectionString() {
    var configuration = SqlServerIntegrationTestConfiguration.FromEnvironment(
        name => name == SqlServerIntegrationTestConfiguration.ConnectionStringEnvironmentVariable
            ? " Server=localhost;Database=dvault_tests;User Id=dvault;Password=local-secret;TrustServerCertificate=True "
            : null);

    Assert.True(configuration.IsConfigured);
    Assert.Equal(
        "Server=localhost;Database=dvault_tests;User Id=dvault;Password=local-secret;TrustServerCertificate=True",
        configuration.ConnectionString);
  }

  [Fact]
  public void MissingConfigurationSkipMessageNamesLocalSqlServerOptInContract() {
    Assert.Contains(
        "local SQL Server configuration is missing",
        SqlServerIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        SqlServerIntegrationTestConfiguration.ConnectionStringEnvironmentVariable,
        SqlServerIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
    Assert.Contains(
        "database provisioning is external to DVault",
        SqlServerIntegrationTestConfiguration.MissingConfigurationSkipMessage,
        StringComparison.Ordinal);
  }

  [Fact]
  public void AddDVaultSqlServerRegistersProviderSaveStrategyInDefaultSmokeCoverage() {
    var services = new ServiceCollection();

    services.AddDVaultSqlServer();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.NotEmpty(provider.GetServices<IDataVaultProviderSaveStrategy>());
  }
}
