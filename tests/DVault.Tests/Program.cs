namespace DVault.Tests;

internal static class Program
{
    private static int Main()
    {
        var failures = 0;
        failures += Modeling.DefaultNamingPolicyTests.Run();
        failures += Modeling.NamingPolicyTests.Run();

        return failures == 0 ? 0 : 1;
    }
}