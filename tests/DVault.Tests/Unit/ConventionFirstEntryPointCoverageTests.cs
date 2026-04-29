using DVault.Tests.Modeling;
using Xunit;

namespace DVault.Tests.Unit;

public sealed class ConventionFirstEntryPointCoverageTests
{
    [Fact]
    public void ModelingCoverageExercisesConventionFirstPublicEntryPoints()
    {
        Assert.Equal(0, DefaultNamingPolicyTests.Run());
        Assert.Equal(0, NamingPolicyTests.Run());
    }
}