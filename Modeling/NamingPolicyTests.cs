using DCoding.Data.DVault.Tests.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit.Modeling;

public sealed class NamingPolicyBridgeTests {
  [Fact]
  public void NamingPolicyHarnessRunsThroughUnitProject() {
    Assert.Equal(0, NamingPolicyTests.Run());
  }
}
