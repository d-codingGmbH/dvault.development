using DVault.Modeling;

namespace DVault.Tests.Modeling;

internal static class DefaultNamingPolicyTests
{
    private static int Main()
    {
        var tests = new TestCase[]
        {
            new("table names use Data Vault prefixes", TableNamesUseDataVaultPrefixes),
            new("explicit link names take precedence", ExplicitLinkNamesTakePrecedence),
            new("normalization handles whitespace punctuation snake kebab and Pascal input", NormalizationHandlesCommonInputForms),
            new("common singular and plural object inputs are stable", CommonSingularAndPluralObjectInputsAreStable),
            new("finite singularization rules are deterministic", FiniteSingularizationRulesAreDeterministic),
            new("reserved words and invalid names use fallback suffixes", ReservedWordsAndInvalidNamesUseFallbackSuffixes),
            new("technical column names are deterministic", TechnicalColumnNamesAreDeterministic),
            new("technical column collisions append Value", TechnicalColumnCollisionsAppendValue),
            new("duplicate normalized column names receive numeric suffixes", DuplicateNormalizedColumnNamesReceiveNumericSuffixes),
            new("repeat calls return identical names", RepeatCallsReturnIdenticalNames),
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

    private static void TableNamesUseDataVaultPrefixes()
    {
        var policy = DefaultNamingPolicy.Instance;

        Equal("HubCustomer", policy.GetHubTableName("Customer"));
        Equal("LinkCustomerOrder", policy.GetLinkTableName(null, ["Customer", "Order"]));
        Equal("SatCustomerContact", policy.GetSatelliteTableName("Customer", "Contact"));
    }

    private static void ExplicitLinkNamesTakePrecedence()
    {
        var policy = DefaultNamingPolicy.Instance;

        Equal("LinkPurchaseEvent", policy.GetLinkTableName("purchase event", ["Customer", "Order"]));
    }

    private static void NormalizationHandlesCommonInputForms()
    {
        var policy = DefaultNamingPolicy.Instance;

        Equal("CustomerAccount", policy.NormalizeObjectName(" customer account "));
        Equal("CustomerAccount", policy.NormalizeObjectName("customer_account"));
        Equal("CustomerAccount", policy.NormalizeObjectName("customer-account"));
        Equal("CustomerAccount", policy.NormalizeObjectName("CustomerAccount"));
        Equal("EmailAddress2", policy.NormalizeColumnName("email_address2"));
    }

    private static void CommonSingularAndPluralObjectInputsAreStable()
    {
        var policy = DefaultNamingPolicy.Instance;

        Equal(policy.GetHubTableName("Customer"), policy.GetHubTableName("Customers"));
        Equal("HubCustomer", policy.GetHubTableName("Customers"));
    }

    private static void FiniteSingularizationRulesAreDeterministic()
    {
        var policy = DefaultNamingPolicy.Instance;

        Equal("Company", policy.NormalizeObjectName("companies"));
        Equal("Box", policy.NormalizeObjectName("boxes"));
        Equal("Address", policy.NormalizeObjectName("addresses"));
        Equal("Business", policy.NormalizeObjectName("business"));
    }

    private static void ReservedWordsAndInvalidNamesUseFallbackSuffixes()
    {
        var policy = DefaultNamingPolicy.Instance;

        Equal("HubSelectEntity", policy.GetHubTableName("Select"));
        Equal("OrderValue", policy.GetColumnName("Order"));
        Equal("HubEntity", policy.GetHubTableName("@@@"));
        Equal("Value", policy.GetColumnName("$%^"));
    }

    private static void TechnicalColumnNamesAreDeterministic()
    {
        var policy = DefaultNamingPolicy.Instance;

        Equal("CustomerHashKey", policy.GetHashKeyColumnName("Customers"));
        Equal("HashDiff", policy.GetHashDiffColumnName());
        Equal("LoadTimestamp", policy.GetLoadTimestampColumnName());
        Equal("RecordSource", policy.GetRecordSourceColumnName());
    }

    private static void TechnicalColumnCollisionsAppendValue()
    {
        var policy = DefaultNamingPolicy.Instance;
        var hashKeyName = policy.GetHashKeyColumnName("Customer");

        var columnNames = policy.GetColumnNames(
            ["hash diff", "load_timestamp", "record-source", "customer hash key"],
            [hashKeyName]);

        SequenceEqual(
            ["HashDiffValue", "LoadTimestampValue", "RecordSourceValue", "CustomerHashKeyValue"],
            columnNames);
    }

    private static void DuplicateNormalizedColumnNamesReceiveNumericSuffixes()
    {
        var policy = DefaultNamingPolicy.Instance;

        var columnNames = policy.GetColumnNames(["customer id", "customer-id", "CustomerId", "Order", "order"]);

        SequenceEqual(["CustomerId", "CustomerId2", "CustomerId3", "OrderValue", "OrderValue2"], columnNames);
    }

    private static void RepeatCallsReturnIdenticalNames()
    {
        var policy = DefaultNamingPolicy.Instance;
        var properties = new[] { "customer id", "hash diff", "Order", "customer-id" };

        var first = policy.GetColumnNames(properties);
        var second = policy.GetColumnNames(properties);

        SequenceEqual(first, second);
        Equal(policy.GetLinkTableName(null, ["Customers", "Orders"]), policy.GetLinkTableName(null, ["Customer", "Order"]));
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException("Expected " + expected + " but got " + actual + ".");
        }
    }

    private static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        var expectedArray = expected.ToArray();
        var actualArray = actual.ToArray();

        if (expectedArray.Length != actualArray.Length)
        {
            throw new InvalidOperationException(
                "Expected " + expectedArray.Length + " values but got " + actualArray.Length + ".");
        }

        for (var index = 0; index < expectedArray.Length; index++)
        {
            if (!EqualityComparer<T>.Default.Equals(expectedArray[index], actualArray[index]))
            {
                throw new InvalidOperationException(
                    "At index " + index + " expected " + expectedArray[index] + " but got " + actualArray[index] + ".");
            }
        }
    }

    private sealed record TestCase(string Name, Action Run);
}