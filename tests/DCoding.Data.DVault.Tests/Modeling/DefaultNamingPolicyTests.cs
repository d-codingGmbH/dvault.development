using System.Reflection;
using System.Runtime.CompilerServices;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Tests.Modeling;

internal static class DefaultNamingPolicyTests {
  internal static int Run() {
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
            new("AddDVault no-option overload is discoverable from DCoding.Data.DVault namespace", AddDVaultNoOptionOverloadIsDiscoverable),
            new("AddDVault optionless startup path builds a service provider", AddDVaultOptionlessStartupPathBuildsServiceProvider),
            new("AddDVault optionless startup path resolves binary hash defaults", AddDVaultOptionlessStartupPathResolvesBinaryHashDefaults),
            new("UseDataVault no-option overload applies default conventions", UseDataVaultNoOptionOverloadAppliesDefaultConventions),
            new("default conventions expose MVP vocabulary and hash defaults", DefaultConventionsExposeMvpVocabularyAndHashDefaults),
    };

    var failures = 0;
    foreach (var test in tests) {
      try {
        test.Run();
        Console.WriteLine("PASS " + test.Name);
      }
      catch (Exception exception) {
        failures++;
        Console.Error.WriteLine("FAIL " + test.Name + ": " + exception.Message);
      }
    }

    return failures == 0 ? 0 : 1;
  }

  private static void TableNamesUseDataVaultPrefixes() {
    var policy = DefaultNamingPolicy.Instance;

    Equal("HubCustomer", policy.GetHubTableName("Customer"));
    Equal("LinkCustomerOrder", policy.GetLinkTableName(null, ["Customer", "Order"]));
    Equal("SatCustomerContact", policy.GetSatelliteTableName("Customer", "Contact"));
  }

  private static void ExplicitLinkNamesTakePrecedence() {
    var policy = DefaultNamingPolicy.Instance;

    Equal("LinkPurchaseEvent", policy.GetLinkTableName("purchase event", ["Customer", "Order"]));
    Equal("LinkEntity", policy.GetLinkTableName("@@@", ["Customer", "Order"]));
  }

  private static void NormalizationHandlesCommonInputForms() {
    var policy = DefaultNamingPolicy.Instance;

    Equal("CustomerAccount", policy.NormalizeObjectName(" customer account "));
    Equal("CustomerAccount", policy.NormalizeObjectName("customer_account"));
    Equal("CustomerAccount", policy.NormalizeObjectName("customer-account"));
    Equal("CustomerAccount", policy.NormalizeObjectName("CustomerAccount"));
    Equal("EmailAddress2", policy.NormalizeColumnName("email_address2"));
  }

  private static void CommonSingularAndPluralObjectInputsAreStable() {
    var policy = DefaultNamingPolicy.Instance;

    Equal(policy.GetHubTableName("Customer"), policy.GetHubTableName("Customers"));
    Equal("HubCustomer", policy.GetHubTableName("Customers"));
  }

  private static void FiniteSingularizationRulesAreDeterministic() {
    var policy = DefaultNamingPolicy.Instance;

    Equal("Company", policy.NormalizeObjectName("companies"));
    Equal("Box", policy.NormalizeObjectName("boxes"));
    Equal("Address", policy.NormalizeObjectName("addresses"));
    Equal("Business", policy.NormalizeObjectName("business"));
  }

  private static void ReservedWordsAndInvalidNamesUseFallbackSuffixes() {
    var policy = DefaultNamingPolicy.Instance;

    Equal("HubSelectEntity", policy.GetHubTableName("Select"));
    Equal("OrderValue", policy.GetColumnName("Order"));
    Equal("HubEntity", policy.GetHubTableName("@@@"));
    Equal("Value", policy.GetColumnName("$%^"));
  }

  private static void TechnicalColumnNamesAreDeterministic() {
    var policy = DefaultNamingPolicy.Instance;

    Equal("CustomerHashKey", policy.GetHashKeyColumnName("Customers"));
    Equal("HashDiff", policy.GetHashDiffColumnName());
    Equal("LoadTimestamp", policy.GetLoadTimestampColumnName());
    Equal("RecordSource", policy.GetRecordSourceColumnName());
  }

  private static void TechnicalColumnCollisionsAppendValue() {
    var policy = DefaultNamingPolicy.Instance;
    var hashKeyName = policy.GetHashKeyColumnName("Customer");

    var columnNames = policy.GetColumnNames(
        ["hash diff", "load_timestamp", "record-source", "customer hash key"],
        [hashKeyName]);

    SequenceEqual(
        ["HashDiffValue", "LoadTimestampValue", "RecordSourceValue", "CustomerHashKeyValue"],
        columnNames);
  }

  private static void DuplicateNormalizedColumnNamesReceiveNumericSuffixes() {
    var policy = DefaultNamingPolicy.Instance;

    var columnNames = policy.GetColumnNames(["customer id", "customer-id", "CustomerId", "Order", "order"]);

    SequenceEqual(["CustomerId", "CustomerId2", "CustomerId3", "OrderValue", "OrderValue2"], columnNames);
  }

  private static void RepeatCallsReturnIdenticalNames() {
    var policy = DefaultNamingPolicy.Instance;
    var properties = new[] { "customer id", "hash diff", "Order", "customer-id" };

    var first = policy.GetColumnNames(properties);
    var second = policy.GetColumnNames(properties);

    SequenceEqual(first, second);
    Equal(policy.GetLinkTableName(null, ["Customers", "Orders"]), policy.GetLinkTableName(null, ["Customer", "Order"]));
  }

  private static void AddDVaultNoOptionOverloadIsDiscoverable() {
    var method = typeof(DVaultServiceCollectionExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "AddDVault" &&
            methodInfo.GetParameters().Length == 1);
    var parameter = method.GetParameters()[0];

    Equal("DCoding.Data.DVault", method.DeclaringType?.Namespace);
    Equal(typeof(IServiceCollection), parameter.ParameterType);
    Equal(typeof(IServiceCollection), method.ReturnType);
    True(method.IsDefined(typeof(ExtensionAttribute), inherit: false), "AddDVault must be an extension method.");
  }

  private static void AddDVaultOptionlessStartupPathBuildsServiceProvider() {
    var services = new ServiceCollection();

    var result = services.AddDVault();

    Same(services, result);

    using var provider = services.BuildServiceProvider(validateScopes: true);

    var namingPolicy = provider.GetRequiredService<DefaultNamingPolicy>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();

    Same(DefaultNamingPolicy.Instance, namingPolicy);
    Same(DataVaultConventions.Default, conventions);
    Same(DefaultNamingPolicy.Instance, conventions.NamingPolicy);
    Equal("HubCustomer", conventions.NamingPolicy.GetHubTableName("Customers"));
  }

  private static void AddDVaultOptionlessStartupPathResolvesBinaryHashDefaults() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    var conventions = provider.GetRequiredService<DataVaultConventions>();

    Same(DataVaultConventions.Default, conventions);
    AssertBinaryHashDefaults(conventions);
  }

  private static void UseDataVaultNoOptionOverloadAppliesDefaultConventions() {
    var modelBuilder = new DataVaultModelBuilder();

    var result = modelBuilder.UseDataVault();

    Same(modelBuilder, result);
    True(modelBuilder.IsDataVaultEnabled, "UseDataVault must enable Data Vault conventions.");
    Same(DataVaultConventions.Default, modelBuilder.Conventions);
    Same(DefaultNamingPolicy.Instance, modelBuilder.Conventions?.NamingPolicy);
    Equal("HubCustomer", modelBuilder.Conventions?.NamingPolicy.GetHubTableName("Customers"));
  }

  private static void DefaultConventionsExposeMvpVocabularyAndHashDefaults() {
    var conventions = DataVaultConventions.Default;

    SequenceEqual(
        [
            DataVaultModelConcept.Hub,
                DataVaultModelConcept.Link,
                DataVaultModelConcept.Satellite,
                DataVaultModelConcept.Bridge,
                DataVaultModelConcept.HashKey,
                DataVaultModelConcept.HashDiff,
                DataVaultModelConcept.LoadTimestamp,
                DataVaultModelConcept.RecordSource,
            ],
        conventions.ModelConcepts);
    SequenceEqual(
        ["dvault_records", "dvault_record_payloads", "dvault_record_metadata"],
        conventions.LogicalObjectNames);
    AssertBinaryHashDefaults(conventions);
    Equal("sha-256", conventions.PersistenceContentHashAlgorithm);
    Equal("dvault.persistence-conventions.v1", conventions.PersistenceConventionVersion);
  }

  private static void AssertBinaryHashDefaults(DataVaultConventions conventions) {
    Equal("sha256-v1", conventions.StableHashAlgorithmId);
    Equal(32, conventions.StableHashDigestByteLength);
    Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
  }

  private static void Equal<T>(T expected, T actual) {
    if (!EqualityComparer<T>.Default.Equals(expected, actual)) {
      throw new InvalidOperationException("Expected " + expected + " but got " + actual + ".");
    }
  }

  private static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual) {
    var expectedArray = expected.ToArray();
    var actualArray = actual.ToArray();

    if (expectedArray.Length != actualArray.Length) {
      throw new InvalidOperationException(
          "Expected " + expectedArray.Length + " values but got " + actualArray.Length + ".");
    }

    for (var index = 0; index < expectedArray.Length; index++) {
      if (!EqualityComparer<T>.Default.Equals(expectedArray[index], actualArray[index])) {
        throw new InvalidOperationException(
            "At index " + index + " expected " + expectedArray[index] + " but got " + actualArray[index] + ".");
      }
    }
  }

  private static void Same(object? expected, object? actual) {
    if (!ReferenceEquals(expected, actual)) {
      throw new InvalidOperationException("Expected matching object references.");
    }
  }

  private static void True(bool condition, string message) {
    if (!condition) {
      throw new InvalidOperationException(message);
    }
  }

  private sealed record TestCase(string Name, Action Run);
}
