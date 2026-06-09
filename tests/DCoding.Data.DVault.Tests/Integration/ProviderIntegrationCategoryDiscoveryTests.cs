using System.Reflection;
using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

public sealed class ProviderIntegrationCategoryDiscoveryTests {
  private static readonly Type[] RequiredLocalSqliteCoverageTypes = [
#if NET10_0
      typeof(BenchmarkScenarioExecutionTests),
#endif
      typeof(DataVaultBridgeMaintenanceServiceSqliteTests),
      typeof(DataVaultBridgeReadServiceSqliteTests),
      typeof(DataVaultCompiledCompatibilitySqliteTests),
      typeof(DataVaultDiagnosticsIntegrationTests),
      typeof(DataVaultMetadataRegistrationIntegrationTests),
      typeof(DataVaultPitMaintenanceServiceSqliteTests),
      typeof(DataVaultPitReadServiceSqliteTests),
      typeof(DataVaultSaveChangesGuardInterceptorSqliteTests),
      typeof(DataVaultSaveChangesMetadataInterceptorSqliteTests),
      typeof(DataVaultSaveStrategySelectionTests),
      typeof(DataVaultTelemetrySqliteTests),
      typeof(DataVaultTypedMapperSaveServiceSqliteTests),
      typeof(DataVaultTypedSatelliteReadServiceSqliteTests),
      typeof(ExplicitDataVaultSaveServiceSqliteTests),
      typeof(NormalEfOrderProductSqliteTests),
      typeof(PlainEfCustomerProfileHistorySqliteTests),
      typeof(SqliteDataVaultSchemaTests),
      typeof(SqliteIdempotencyPreflightTests),
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
                typeof(MySqlLiveSchemaFixtureContractTests),
                typeof(MySqlLiveSchemaReaderTests),
                typeof(PostgresDataVaultSchemaTests),
                typeof(PostgresOptimizedDataVaultSaveServiceTests),
                typeof(PostgresIntegrationTestConfigurationTests),
                typeof(PostgresLiveSchemaFixtureContractTests),
                typeof(PostgresLiveSchemaReaderTests),
                typeof(OracleDataVaultSmokeTests),
                typeof(OracleIntegrationTestConfigurationTests),
                typeof(OracleLiveSchemaFixtureContractTests),
                typeof(OracleLiveSchemaReaderTests),
                typeof(ProviderIntegrationCategoryDiscoveryTests),
                typeof(SqlServerBatchScriptTests),
                typeof(SqlServerDataVaultSmokeTests),
                typeof(SqlServerIntegrationTestConfigurationTests),
                typeof(SqlServerLiveSchemaFixtureContractTests),
                typeof(SqlServerLiveSchemaReaderTests),
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
    foreach (var coverageType in new[] {
        typeof(PostgresDataVaultSchemaTests),
        typeof(PostgresOptimizedDataVaultSaveServiceTests),
        typeof(PostgresLiveSchemaReaderTests),
    }) {
      AssertTrait(
          coverageType,
          ProviderTestCategories.CategoryTraitName,
          ProviderTestCategories.ExternalProviderIntegration);
      AssertTrait(
          coverageType,
          ProviderTestCategories.ProviderTraitName,
          ProviderTestCategories.PostgresProvider);
    }
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LiveOracleIntegrationTestsAreExternalProviderOptInCoverage() {
    foreach (var coverageType in new[] {
        typeof(OracleDataVaultSmokeTests),
        typeof(OracleLiveSchemaReaderTests),
    }) {
      AssertTrait(
          coverageType,
          ProviderTestCategories.CategoryTraitName,
          ProviderTestCategories.ExternalProviderIntegration);
      AssertTrait(
          coverageType,
          ProviderTestCategories.ProviderTraitName,
          ProviderTestCategories.OracleProvider);
    }
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LiveMySqlIntegrationTestsAreExternalProviderOptInCoverage() {
    foreach (var coverageType in new[] {
        typeof(MySqlExplicitDataVaultSaveServiceTests),
        typeof(MySqlLiveSchemaReaderTests),
    }) {
      AssertTrait(
          coverageType,
          ProviderTestCategories.CategoryTraitName,
          ProviderTestCategories.ExternalProviderIntegration);
      AssertTrait(
          coverageType,
          ProviderTestCategories.ProviderTraitName,
          ProviderTestCategories.MySqlProvider);
    }
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LiveSqlServerIntegrationTestsAreExternalProviderOptInCoverage() {
    foreach (var coverageType in new[] {
        typeof(SqlServerDataVaultSmokeTests),
        typeof(SqlServerLiveSchemaReaderTests),
    }) {
      AssertTrait(
          coverageType,
          ProviderTestCategories.CategoryTraitName,
          ProviderTestCategories.ExternalProviderIntegration);
      AssertTrait(
          coverageType,
          ProviderTestCategories.ProviderTraitName,
          ProviderTestCategories.SqlServerProvider);
    }
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

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  public void LiveSchemaFixtureContractTestsRemainDefaultProviderSmokeCoverage() {
    foreach (var (coverageType, providerName) in new[] {
        (typeof(PostgresLiveSchemaFixtureContractTests), ProviderTestCategories.PostgresProvider),
        (typeof(SqlServerLiveSchemaFixtureContractTests), ProviderTestCategories.SqlServerProvider),
        (typeof(OracleLiveSchemaFixtureContractTests), ProviderTestCategories.OracleProvider),
        (typeof(MySqlLiveSchemaFixtureContractTests), ProviderTestCategories.MySqlProvider),
    }) {
      AssertTrait(
          coverageType,
          ProviderTestCategories.CategoryTraitName,
          ProviderTestCategories.DefaultProviderSmoke);
      AssertTrait(
          coverageType,
          ProviderTestCategories.ProviderTraitName,
          providerName);
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
