using System.Reflection;
using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

public sealed class ProviderIntegrationCategoryDiscoveryTests {
  private static readonly Type[] RequiredLocalSqliteCoverageTypes = [
      typeof(BenchmarkScenarioExecutionTests),
      typeof(DataVaultBridgeReadServiceSqliteTests),
      typeof(DataVaultCompiledCompatibilitySqliteTests),
      typeof(DataVaultDiagnosticsIntegrationTests),
      typeof(DataVaultMetadataRegistrationIntegrationTests),
      typeof(DataVaultPitReadServiceSqliteTests),
      typeof(DataVaultSaveChangesMetadataInterceptorSqliteTests),
      typeof(DataVaultSaveStrategySelectionTests),
      typeof(DataVaultTypedMapperSaveServiceSqliteTests),
      typeof(DataVaultTypedSatelliteReadServiceSqliteTests),
      typeof(ExplicitDataVaultSaveServiceSqliteTests),
      typeof(NormalEfOrderProductSqliteTests),
      typeof(PlainEfCustomerProfileHistorySqliteTests),
      typeof(SqliteDataVaultSchemaTests),
      typeof(SqliteLiveSchemaDriftTests),
      typeof(SqliteProviderCapabilityProfileTests),
      typeof(SqliteProviderSqlExecutionContractTests),
      typeof(SqliteTestDatabaseTests),
  ];

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void IntegrationTestClassesDeclareProviderCategoryBoundaries() {
    var expectedTypes = RequiredLocalSqliteCoverageTypes
        .Concat(
            [
                typeof(MySqlExplicitDataVaultSaveServiceTests),
                typeof(MySqlIntegrationTestConfigurationTests),
                typeof(PostgresDataVaultSchemaTests),
                typeof(PostgresOptimizedDataVaultSaveServiceTests),
                typeof(PostgresIntegrationTestConfigurationTests),
                typeof(OracleDataVaultSmokeTests),
                typeof(OracleIntegrationTestConfigurationTests),
                typeof(ProviderIntegrationCategoryDiscoveryTests),
                typeof(SqlServerBatchScriptTests),
                typeof(SqlServerDataVaultSmokeTests),
                typeof(SqlServerIntegrationTestConfigurationTests),
            ])
        .Select(type => type.FullName!)
        .OrderBy(name => name, StringComparer.Ordinal)
        .ToArray();
    var discoveredTypes = typeof(ProviderIntegrationCategoryDiscoveryTests)
        .Assembly
        .GetTypes()
        .Where(type => type is { IsClass: true, IsPublic: true } && ContainsFact(type))
        .Select(type => type.FullName!)
        .OrderBy(name => name, StringComparer.Ordinal)
        .ToArray();

    Assert.Equal(expectedTypes, discoveredTypes);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void SqliteIntegrationTestsAreRequiredLocalProviderCoverage() {
    foreach (var coverageType in RequiredLocalSqliteCoverageTypes) {
      AssertTrait(
          coverageType,
          ProviderTestCategories.CategoryTraitName,
          ProviderTestCategories.RequiredLocalProviderIntegration);
      AssertTrait(
          coverageType,
          ProviderTestCategories.ProviderTraitName,
          ProviderTestCategories.SqliteProvider);
    }
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LivePostgresIntegrationTestsAreExternalProviderOptInCoverage() {
    AssertTrait(
        typeof(PostgresDataVaultSchemaTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.ExternalProviderIntegration);
    AssertTrait(
        typeof(PostgresDataVaultSchemaTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.PostgresProvider);
    AssertTrait(
        typeof(PostgresOptimizedDataVaultSaveServiceTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.ExternalProviderIntegration);
    AssertTrait(
        typeof(PostgresOptimizedDataVaultSaveServiceTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.PostgresProvider);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LiveOracleIntegrationTestsAreExternalProviderOptInCoverage() {
    AssertTrait(
        typeof(OracleDataVaultSmokeTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.ExternalProviderIntegration);
    AssertTrait(
        typeof(OracleDataVaultSmokeTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.OracleProvider);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LiveMySqlIntegrationTestsAreExternalProviderOptInCoverage() {
    AssertTrait(
        typeof(MySqlExplicitDataVaultSaveServiceTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.ExternalProviderIntegration);
    AssertTrait(
        typeof(MySqlExplicitDataVaultSaveServiceTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.MySqlProvider);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LiveSqlServerIntegrationTestsAreExternalProviderOptInCoverage() {
    AssertTrait(
        typeof(SqlServerDataVaultSmokeTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.ExternalProviderIntegration);
    AssertTrait(
        typeof(SqlServerDataVaultSmokeTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.SqlServerProvider);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void PostgresConfigurationContractTestsRemainDefaultProviderSmokeCoverage() {
    AssertTrait(
        typeof(PostgresIntegrationTestConfigurationTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.DefaultProviderSmoke);
    AssertTrait(
        typeof(PostgresIntegrationTestConfigurationTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.PostgresProvider);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void OracleConfigurationContractTestsRemainDefaultProviderSmokeCoverage() {
    AssertTrait(
        typeof(OracleIntegrationTestConfigurationTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.DefaultProviderSmoke);
    AssertTrait(
        typeof(OracleIntegrationTestConfigurationTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.OracleProvider);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void MySqlConfigurationContractTestsRemainDefaultProviderSmokeCoverage() {
    AssertTrait(
        typeof(MySqlIntegrationTestConfigurationTests),
        ProviderTestCategories.CategoryTraitName,
        ProviderTestCategories.DefaultProviderSmoke);
    AssertTrait(
        typeof(MySqlIntegrationTestConfigurationTests),
        ProviderTestCategories.ProviderTraitName,
        ProviderTestCategories.MySqlProvider);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void SqlServerSupportContractTestsRemainDefaultProviderSmokeCoverage() {
    foreach (var coverageType in new[] {
        typeof(SqlServerBatchScriptTests),
        typeof(SqlServerIntegrationTestConfigurationTests),
    }) {
      AssertTrait(
          coverageType,
          ProviderTestCategories.CategoryTraitName,
          ProviderTestCategories.DefaultProviderSmoke);
      AssertTrait(
          coverageType,
          ProviderTestCategories.ProviderTraitName,
          ProviderTestCategories.SqlServerProvider);
    }
  }

  private static bool ContainsFact(Type type) {
    return type
        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .Any(method => method.GetCustomAttributes<FactAttribute>(inherit: true).Any());
  }

  private static void AssertTrait(MemberInfo member, string name, string value) {
    Assert.Contains(
        member.GetCustomAttributes<TraitAttribute>(inherit: true),
        trait => string.Equals(trait.Name, name, StringComparison.Ordinal) &&
            string.Equals(trait.Value, value, StringComparison.Ordinal));
  }
}
