using DCoding.Data.DVault.Tests;
using DCoding.Data.DVault.Tests.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class ConventionFirstEntryPointCoverageTests {
  [Fact]
  public void ModelingCoverageExercisesConventionFirstPublicEntryPoints() {
    Assert.Equal(0, DefaultNamingPolicyTests.Run());
    Assert.Equal(0, NamingPolicyTests.Run());
  }

  [Fact]
  public void TechnicalMetadataCoverageExercisesReusableColumnContractHarness() {
    Assert.Equal(0, TechnicalMetadataColumnContractTests.Run());
  }
}
