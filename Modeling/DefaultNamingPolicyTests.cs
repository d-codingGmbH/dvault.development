using DCoding.Data.DVault.Tests.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit.Modeling;

public sealed class DefaultNamingPolicyBridgeTests {
  [Fact]
  public void DefaultNamingPolicyHarnessRunsThroughUnitProject() {
    Assert.Equal(0, DefaultNamingPolicyTests.Run());
  }
}
