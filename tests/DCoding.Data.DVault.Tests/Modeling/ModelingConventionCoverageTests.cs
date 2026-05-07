using Xunit;

namespace DCoding.Data.DVault.Tests.Modeling;

public sealed class ModelingConventionCoverageTests {
  [Fact]
  public void DefaultNamingPolicyScenariosPass() {
    Assert.Equal(0, DefaultNamingPolicyTests.Run());
  }

  [Fact]
  public void DataVaultNamingPolicyScenariosPass() {
    Assert.Equal(0, NamingPolicyTests.Run());
  }
}
