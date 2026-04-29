using DCoding.Data.DVault;

namespace DVault.Tests;

internal static class TechnicalMetadataColumnContractTests
{
    private static int Main()
    {
        var tests = new TestCase[]
        {
            new("default contract set contains the closed v1 role set", DefaultContractSetContainsClosedV1RoleSet),
            new("default contracts expose v1 default and effective names", DefaultContractsExposeV1DefaultAndEffectiveNames),
            new("overrides preserve role identity and default contract metadata", OverridesPreserveRoleIdentityAndDefaultContractMetadata),
            new("default contracts use one reusable contract shape", DefaultContractsUseOneReusableContractShape),
        };

        var failures = 0;
        foreach (var test in tests)
        {
            try
            {
                test.Run();
                Console.WriteLine("PASS " + test.Name);
            }
            catch (Exception exception)
            {
                failures++;
                Console.Error.WriteLine("FAIL " + test.Name + ": " + exception.Message);
            }
        }

        return failures == 0 ? 0 : 1;
    }

    private static void DefaultContractSetContainsClosedV1RoleSet()
    {
        var contracts = TechnicalMetadataColumnContract.Defaults;

        Equal(4, contracts.Count);
        ContainsRole(contracts, TechnicalMetadataColumnRole.HashKey);
        ContainsRole(contracts, TechnicalMetadataColumnRole.HashDiff);
        ContainsRole(contracts, TechnicalMetadataColumnRole.LoadTimestamp);
        ContainsRole(contracts, TechnicalMetadataColumnRole.RecordSource);
        Equal(4, contracts.Select(contract => contract.Role).Distinct().Count());
    }

    private static void DefaultContractsExposeV1DefaultAndEffectiveNames()
    {
        AssertDefaultContract(TechnicalMetadataColumnRole.HashKey, "HashKey");
        AssertDefaultContract(TechnicalMetadataColumnRole.HashDiff, "HashDiff");
        AssertDefaultContract(TechnicalMetadataColumnRole.LoadTimestamp, "LoadTimestamp");
        AssertDefaultContract(TechnicalMetadataColumnRole.RecordSource, "RecordSource");
    }

    private static void OverridesPreserveRoleIdentityAndDefaultContractMetadata()
    {
        AssertOverride(TechnicalMetadataColumnRole.HashKey, "HashKey", "CustomerHashKey");
        AssertOverride(TechnicalMetadataColumnRole.HashDiff, "HashDiff", "CustomerHashDiff");
        AssertOverride(TechnicalMetadataColumnRole.LoadTimestamp, "LoadTimestamp", "LoadedAtUtc");
        AssertOverride(TechnicalMetadataColumnRole.RecordSource, "RecordSource", "SourceSystem");
    }

    private static void DefaultContractsUseOneReusableContractShape()
    {
        foreach (var contract in TechnicalMetadataColumnContract.Defaults)
        {
            Equal(typeof(TechnicalMetadataColumnContract), contract.GetType());
            NotWhiteSpace(contract.SemanticPurpose);
            Equal(TechnicalMetadataColumnRequiredness.RequiredWhenDeclared, contract.RequirednessExpectation);
            NotWhiteSpace(contract.DefaultEffectiveColumnName);
            NotWhiteSpace(contract.EffectiveColumnName);
        }
    }

    private static void AssertDefaultContract(TechnicalMetadataColumnRole role, string expectedName)
    {
        var contract = TechnicalMetadataColumnContract.ForRole(role);

        Equal(role, contract.Role);
        Equal(expectedName, contract.DefaultEffectiveColumnName);
        Equal(expectedName, contract.EffectiveColumnName);
        Equal(TechnicalMetadataColumnRequiredness.RequiredWhenDeclared, contract.RequirednessExpectation);
    }

    private static void AssertOverride(TechnicalMetadataColumnRole role, string expectedDefaultName, string overrideName)
    {
        var original = TechnicalMetadataColumnContract.ForRole(role);
        var overridden = original.WithEffectiveColumnName(overrideName);

        Equal(role, overridden.Role);
        Equal(original.SemanticPurpose, overridden.SemanticPurpose);
        Equal(original.RequirednessExpectation, overridden.RequirednessExpectation);
        Equal(expectedDefaultName, overridden.DefaultEffectiveColumnName);
        Equal(overrideName, overridden.EffectiveColumnName);
        Equal(expectedDefaultName, original.EffectiveColumnName);
    }

    private static void ContainsRole(
        IEnumerable<TechnicalMetadataColumnContract> contracts,
        TechnicalMetadataColumnRole role)
    {
        if (!contracts.Any(contract => contract.Role == role))
        {
            throw new InvalidOperationException("Expected role " + role + " was not found.");
        }
    }

    private static void NotWhiteSpace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException("Expected a non-empty value.");
        }
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException("Expected " + expected + " but got " + actual + ".");
        }
    }

    private sealed record TestCase(string Name, Action Run);
}
