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
  public void UnitProjectRunDoesNotLoadIntegrationTestAssembly() {
    Assert.DoesNotContain(
        AppDomain.CurrentDomain.GetAssemblies(),
        assembly => string.Equals(
            "DCoding.Data.DVault.Tests.Integration",
            assembly.GetName().Name,
            StringComparison.Ordinal));
  }
}
