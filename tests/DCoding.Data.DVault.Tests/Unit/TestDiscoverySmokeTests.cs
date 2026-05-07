using System.Reflection;
using DCoding.Data.DVault.Tests;
using DCoding.Data.DVault.Tests.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class TestDiscoverySmokeTests {
  [Fact]
  public void UnitProjectRunsAndCanReferenceSharedTestUtilities() {
    using var database = SqliteTestDatabase.CreateInMemory();

    Assert.Contains("mode=memory", database.ConnectionString, StringComparison.OrdinalIgnoreCase);
    Assert.Null(database.DatabasePath);
  }

  [Fact]
  public void UnitProjectOwnsExpectedFastCoverageGroups() {
    var unitAssembly = typeof(TestDiscoverySmokeTests).Assembly;
    var expectedCoverageTypes = new[] {
        typeof(DataVaultMetadataTests),
        typeof(DataVaultModelBuilderExtensionsTests),
        typeof(DataVaultEfMetadataTranslationTests),
        typeof(TechnicalMetadataColumnContractTests),
        typeof(ModelingConventionCoverageTests),
        typeof(DefaultNamingPolicyTests),
        typeof(NamingPolicyTests),
        typeof(StableHashNormalizerTests),
        typeof(StableHashServiceTests),
        typeof(ExplicitDataVaultSaveServiceTests),
        typeof(DataVaultProviderCapabilityProfileTests),
    };

    foreach (var coverageType in expectedCoverageTypes) {
      Assert.Same(unitAssembly, coverageType.Assembly);
    }
  }

  [Fact]
  public void UnitProjectMarksProviderPackageChecksAsDefaultSmokeCoverage() {
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.ProviderPackagesRegisterCoreSaveService),
        [
            ProviderTestCategories.OracleProvider,
            ProviderTestCategories.MySqlProvider,
        ]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.PostgresProviderPackageRegistersOptimizedSaveStrategy),
        [ProviderTestCategories.PostgresProvider]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.SqliteProviderPackageRegistersOptimizedSaveStrategy),
        [ProviderTestCategories.SqliteProvider]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.SqlServerProviderPackageRegistersOptimizedSaveStrategy),
        [ProviderTestCategories.SqlServerProvider]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.SqlServerProviderSaveStrategyAcceptsOnlyCleanSqlServerContexts),
        [ProviderTestCategories.SqlServerProvider]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.SqlServerUniqueInsertSqlUsesSetBasedExistenceDetection),
        [ProviderTestCategories.SqlServerProvider]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.SqlServerSatelliteLookupSqlRanksLatestHashDiffsByParentBatch),
        [ProviderTestCategories.SqlServerProvider]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.SqlServerSatelliteFilterUsesLatestHashDiffAcrossOrderedBatch),
        [ProviderTestCategories.SqlServerProvider]);
    AssertMethodTraits(
        nameof(ExplicitDataVaultSaveServiceTests.SqlServerSavePlansKeepFallbackSavedRecordOrderingForBulkRequests),
        [ProviderTestCategories.SqlServerProvider]);
  }

  [Fact]
  public void UnitProjectRunDoesNotLoadIntegrationTestAssembly() {
    Assert.DoesNotContain(
        AppDomain.CurrentDomain.GetAssemblies(),
        assembly => string.Equals(
            "DCoding.Data.DVault.Tests.Integration",
            assembly.GetName().Name,
            StringComparison.Ordinal));
  }

  private static void AssertMethodTraits(string methodName, string[] expectedProviderTraits) {
    var method = typeof(ExplicitDataVaultSaveServiceTests).GetMethod(methodName);

    Assert.NotNull(method);
    Assert.Contains(
        method.GetCustomAttributes<TraitAttribute>(inherit: true),
        trait => string.Equals(
            trait.Name,
            ProviderTestCategories.CategoryTraitName,
            StringComparison.Ordinal) &&
            string.Equals(
                trait.Value,
                ProviderTestCategories.DefaultProviderSmoke,
                StringComparison.Ordinal));

    foreach (var expectedProviderTrait in expectedProviderTraits) {
      Assert.Contains(
          method.GetCustomAttributes<TraitAttribute>(inherit: true),
          trait => string.Equals(
              trait.Name,
              ProviderTestCategories.ProviderTraitName,
              StringComparison.Ordinal) &&
              string.Equals(trait.Value, expectedProviderTrait, StringComparison.Ordinal));
    }
  }
}
