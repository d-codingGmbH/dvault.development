using DCoding.Data.DVault.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace DCoding.Data.DVault.Tests.Analyzers;

public sealed class DataVaultMappingSourceGeneratorTests {
  [Fact]
  public void GeneratesDeterministicHubLinkAndSatelliteMapperSources() {
    var result = RunGenerator(RuntimeStubs + """
        namespace ConsumerApp {

        using DCoding.Data.DVault;

        [DataVaultHubMapping("Customer")]
        [DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
        [DataVaultBusinessKeyBinding(1, "Region Code", nameof(RegionCode))]
        public sealed record CustomerSource(string CustomerId, string RegionCode);

        [DataVaultLinkMapping("CustomerOrder")]
        [DataVaultLinkParticipantBinding(0, "Customer", nameof(CustomerHashKey))]
        [DataVaultLinkParticipantBinding(1, "Order", nameof(OrderHashKey))]
        public sealed record CustomerOrderSource(string CustomerHashKey, string OrderHashKey);

        [DataVaultHubSatelliteMapping("Customer", "Profile")]
        [DataVaultSatelliteParentHashKeyBinding(nameof(CustomerHashKey))]
        [DataVaultSatellitePayloadBinding(0, "customer_name", nameof(CustomerName))]
        [DataVaultSatellitePayloadBinding(1, "customer_status", nameof(CustomerStatus))]
        [DataVaultSatelliteHashDiffBinding(nameof(HashDiff))]
        public sealed record CustomerProfileSource(
            string CustomerHashKey,
            string CustomerName,
            string CustomerStatus,
            string HashDiff);

        [DataVaultHubSatelliteMapping("Customer", "ContactChannel")]
        [DataVaultSatelliteParentHashKeyBinding(nameof(CustomerHashKey))]
        [DataVaultSatelliteDrivingKeyBinding(0, "Contact Type", nameof(ContactType))]
        [DataVaultSatelliteDrivingKeyBinding(1, "Region Code", nameof(RegionCode))]
        [DataVaultSatellitePayloadBinding(0, "Email Address", nameof(EmailAddress))]
        [DataVaultSatelliteHashDiffBinding(nameof(HashDiff))]
        public sealed record CustomerContactSource(
            string CustomerHashKey,
            string ContactType,
            string RegionCode,
            string EmailAddress,
            string HashDiff);
        }
        """);

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var hubSource = AssertGeneratedSource(result, "CustomerSourceDataVaultHubMapping.g.cs");
    Assert.Contains("public const string HubName = \"Customer\";", hubSource, StringComparison.Ordinal);
    Assert.Contains("public static global::DCoding.Data.DVault.IDataVaultHubMapper<global::ConsumerApp.CustomerSource> CreateMapper()", hubSource, StringComparison.Ordinal);
    Assert.Contains("new(\"Customer Id\", source.CustomerId)", hubSource, StringComparison.Ordinal);
    Assert.Contains("new(\"Region Code\", source.RegionCode)", hubSource, StringComparison.Ordinal);

    var linkSource = AssertGeneratedSource(result, "CustomerOrderSourceDataVaultLinkMapping.g.cs");
    Assert.Contains("public const string LinkName = \"CustomerOrder\";", linkSource, StringComparison.Ordinal);
    Assert.Contains("ProducedParticipantNames", linkSource, StringComparison.Ordinal);
    Assert.Contains("ParticipantHubNames", linkSource, StringComparison.Ordinal);
    Assert.Contains("new(\"Customer\", source.CustomerHashKey)", linkSource, StringComparison.Ordinal);
    Assert.Contains("new(\"Order\", source.OrderHashKey)", linkSource, StringComparison.Ordinal);

    var ordinarySatelliteSource = AssertGeneratedSource(result, "CustomerProfileSourceDataVaultHubSatelliteMapping.g.cs");
    Assert.Contains("public const string ParentHubName = \"Customer\";", ordinarySatelliteSource, StringComparison.Ordinal);
    Assert.Contains("public const string SatelliteName = \"Profile\";", ordinarySatelliteSource, StringComparison.Ordinal);
    Assert.Contains("global::DCoding.Data.DVault.Modeling.DataVaultMetadataReference.Hub(\"Customer\")", ordinarySatelliteSource, StringComparison.Ordinal);
    Assert.Contains("new(\"customer_name\", source.CustomerName)", ordinarySatelliteSource, StringComparison.Ordinal);
    Assert.Contains("new(\"customer_status\", source.CustomerStatus)", ordinarySatelliteSource, StringComparison.Ordinal);
    Assert.DoesNotContain("Contact Type", ordinarySatelliteSource, StringComparison.Ordinal);

    var multiActiveSatelliteSource = AssertGeneratedSource(result, "CustomerContactSourceDataVaultHubSatelliteMapping.g.cs");
    Assert.Contains("DrivingKeyNames", multiActiveSatelliteSource, StringComparison.Ordinal);
    Assert.Contains("new(\"Contact Type\", source.ContactType)", multiActiveSatelliteSource, StringComparison.Ordinal);
    Assert.Contains("new(\"Region Code\", source.RegionCode)", multiActiveSatelliteSource, StringComparison.Ordinal);
    Assert.Contains("new(\"Email Address\", source.EmailAddress)", multiActiveSatelliteSource, StringComparison.Ordinal);
  }

  [Fact]
  public void GeneratesRoleBearingSameHubLinkMapperByProducedParticipantNames() {
    var result = RunGenerator(RuntimeStubs + """
        namespace ConsumerApp {

        using DCoding.Data.DVault;

        [DataVaultLinkMapping("CustomerIdentityMatch")]
        [DataVaultLinkParticipantBinding(0, "SourceCustomer", nameof(SourceCustomerHashKey))]
        [DataVaultLinkParticipantBinding(1, "MatchedCustomer", nameof(MatchedCustomerHashKey))]
        public sealed record CustomerIdentityMatchSource(string SourceCustomerHashKey, string MatchedCustomerHashKey);
        }
        """);

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var source = AssertGeneratedSource(result, "CustomerIdentityMatchSourceDataVaultLinkMapping.g.cs");
    Assert.Contains("public const string LinkName = \"CustomerIdentityMatch\";", source, StringComparison.Ordinal);
    Assert.Contains("ProducedParticipantNames", source, StringComparison.Ordinal);
    Assert.Contains("ParticipantHubNames => ProducedParticipantNames", source, StringComparison.Ordinal);
    Assert.Contains("new(\"SourceCustomer\", source.SourceCustomerHashKey)", source, StringComparison.Ordinal);
    Assert.Contains("new(\"MatchedCustomer\", source.MatchedCustomerHashKey)", source, StringComparison.Ordinal);
  }

  [Fact]
  public void ReportsDuplicateLinkProducedParticipantDiagnostic() {
    var result = RunGenerator(RuntimeStubs + """
        namespace ConsumerApp {

        using DCoding.Data.DVault;

        [DataVaultLinkMapping("EmployeeReportsTo")]
        [DataVaultLinkParticipantBinding(0, "Employee", nameof(ManagerHashKey))]
        [DataVaultLinkParticipantBinding(1, "Employee", nameof(EmployeeHashKey))]
        public sealed record EmployeeReportsToSource(string ManagerHashKey, string EmployeeHashKey);
        }
        """);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1955", diagnostic.Id);
    Assert.Contains("declares produced participant name 'Employee' more than once", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsAmbiguousAndBlankMappingDeclarations() {
    var result = RunGenerator(RuntimeStubs + """
        namespace ConsumerApp {

        using DCoding.Data.DVault;

        [DataVaultHubMapping("Customer")]
        [DataVaultLinkMapping("CustomerOrder")]
        [DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
        public sealed record AmbiguousCustomerSource(string CustomerId);

        [DataVaultHubMapping(" ")]
        [DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
        public sealed record BlankCustomerSource(string CustomerId);
        }
        """);

    Assert.Contains(result.GeneratorDiagnostics, diagnostic => diagnostic.Id == "DMV1950" &&
        diagnostic.GetMessage().Contains("exactly one", StringComparison.Ordinal));
    Assert.Contains(result.GeneratorDiagnostics, diagnostic => diagnostic.Id == "DMV1952" &&
        diagnostic.GetMessage().Contains("hub target name", StringComparison.Ordinal));
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsDuplicateBindingOrderAndNames() {
    var result = RunGenerator(RuntimeStubs + """
        namespace ConsumerApp {

        using DCoding.Data.DVault;

        [DataVaultHubMapping("Customer")]
        [DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
        [DataVaultBusinessKeyBinding(0, "Region Code", nameof(RegionCode))]
        [DataVaultBusinessKeyBinding(1, "Customer Id", nameof(CustomerCode))]
        public sealed record CustomerSource(string CustomerId, string RegionCode, string CustomerCode);
        }
        """);

    Assert.Contains(result.GeneratorDiagnostics, diagnostic => diagnostic.Id == "DMV1953" &&
        diagnostic.GetMessage().Contains("order 0", StringComparison.Ordinal));
    Assert.Contains(result.GeneratorDiagnostics, diagnostic => diagnostic.Id == "DMV1954" &&
        diagnostic.GetMessage().Contains("Customer Id", StringComparison.Ordinal));
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsMissingAndInvalidSatelliteBindings() {
    var result = RunGenerator(RuntimeStubs + """
        namespace ConsumerApp {

        using DCoding.Data.DVault;

        [DataVaultHubSatelliteMapping("Customer", "Profile")]
        [DataVaultSatelliteParentHashKeyBinding(nameof(CustomerHashKey))]
        [DataVaultSatellitePayloadBinding(0, "customer_status", nameof(CustomerStatus))]
        public sealed record CustomerProfileSource(string CustomerHashKey, string CustomerStatus);

        [DataVaultHubMapping("Customer")]
        [DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
        public sealed record InvalidCustomerSource(int CustomerId);
        }
        """);

    Assert.Contains(result.GeneratorDiagnostics, diagnostic => diagnostic.Id == "DMV1951" &&
        diagnostic.GetMessage().Contains("hash-diff", StringComparison.Ordinal));
    Assert.Contains(result.GeneratorDiagnostics, diagnostic => diagnostic.Id == "DMV1952" &&
        diagnostic.GetMessage().Contains("CustomerId", StringComparison.Ordinal));
    Assert.Empty(result.GeneratedSources);
  }

  private static string AssertGeneratedSource(GeneratorRunResult result, string hintName) {
    Assert.True(result.GeneratedSources.TryGetValue(hintName, out var source), "Missing generated source " + hintName);
    return source;
  }

  private static GeneratorRunResult RunGenerator(string source) {
    var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);
    var syntaxTree = CSharpSyntaxTree.ParseText(source, parseOptions);
    var compilation = CSharpCompilation.Create(
        "DVaultGeneratorSample",
        [syntaxTree],
        CreateReferences(),
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    GeneratorDriver driver = CSharpGeneratorDriver.Create(
        [new DataVaultMappingSourceGenerator().AsSourceGenerator()],
        parseOptions: parseOptions);
    driver = driver.RunGeneratorsAndUpdateCompilation(
        compilation,
        out var outputCompilation,
        out var generatorDiagnostics,
        TestContext.Current.CancellationToken);

    var runResult = driver.GetRunResult();
    var generatedSources = runResult.Results
        .SelectMany(result => result.GeneratedSources)
        .ToDictionary(sourceResult => sourceResult.HintName, sourceResult => sourceResult.SourceText.ToString(), StringComparer.Ordinal);
    var compilationErrors = outputCompilation.GetDiagnostics(TestContext.Current.CancellationToken)
        .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
        .ToArray();

    return new GeneratorRunResult(
        generatorDiagnostics.ToArray(),
        compilationErrors,
        generatedSources);
  }

  private static IReadOnlyList<MetadataReference> CreateReferences() {
    var trustedPlatformAssemblies = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))?
        .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) ??
        [];

    return trustedPlatformAssemblies
        .Select(path => MetadataReference.CreateFromFile(path))
        .GroupBy(reference => reference.Display, StringComparer.Ordinal)
        .Select(group => group.First())
        .ToArray();
  }

  private sealed record GeneratorRunResult(
      IReadOnlyList<Diagnostic> GeneratorDiagnostics,
      IReadOnlyList<Diagnostic> CompilationErrors,
      IReadOnlyDictionary<string, string> GeneratedSources);

  private const string RuntimeStubs = """
      using System;
      using System.Collections.Generic;

      namespace DCoding.Data.DVault {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
        public sealed class DataVaultHubMappingAttribute : Attribute {
          public DataVaultHubMappingAttribute(string hubName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
        public sealed class DataVaultLinkMappingAttribute : Attribute {
          public DataVaultLinkMappingAttribute(string linkName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
        public sealed class DataVaultHubSatelliteMappingAttribute : Attribute {
          public DataVaultHubSatelliteMappingAttribute(string parentHubName, string satelliteName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
        public sealed class DataVaultBusinessKeyBindingAttribute : Attribute {
          public DataVaultBusinessKeyBindingAttribute(int order, string businessKeyName, string sourceMemberName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
        public sealed class DataVaultLinkParticipantBindingAttribute : Attribute {
          public DataVaultLinkParticipantBindingAttribute(int order, string participantHubName, string sourceMemberName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
        public sealed class DataVaultLinkDependentChildKeyBindingAttribute : Attribute {
          public DataVaultLinkDependentChildKeyBindingAttribute(int order, string dependentChildKeyName, string sourceMemberName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
        public sealed class DataVaultSatelliteParentHashKeyBindingAttribute : Attribute {
          public DataVaultSatelliteParentHashKeyBindingAttribute(string sourceMemberName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
        public sealed class DataVaultSatelliteDrivingKeyBindingAttribute : Attribute {
          public DataVaultSatelliteDrivingKeyBindingAttribute(int order, string drivingKeyName, string sourceMemberName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
        public sealed class DataVaultSatellitePayloadBindingAttribute : Attribute {
          public DataVaultSatellitePayloadBindingAttribute(int order, string payloadName, string sourceMemberName) { }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
        public sealed class DataVaultSatelliteHashDiffBindingAttribute : Attribute {
          public DataVaultSatelliteHashDiffBindingAttribute(string sourceMemberName) { }
        }

        public interface IDataVaultHubMapper<in TSource>
            where TSource : notnull {
          DataVaultRegistryHubSaveOperation Map(TSource source);
        }

        public interface IDataVaultLinkMapper<in TSource>
            where TSource : notnull {
          DataVaultRegistryLinkSaveOperation Map(TSource source);
        }

        public interface IDataVaultSatelliteMapper<in TSource>
            where TSource : notnull {
          DataVaultRegistrySatelliteSaveOperation Map(TSource source);
        }

        public sealed class DataVaultRegistryHubSaveOperation {
          public DataVaultRegistryHubSaveOperation(string hubName, IEnumerable<KeyValuePair<string, string>> businessKeyValues) { }
        }

        public sealed class DataVaultRegistryLinkSaveOperation {
          public DataVaultRegistryLinkSaveOperation(string linkName, IEnumerable<KeyValuePair<string, string>> participantHashKeyValues) { }

          public DataVaultRegistryLinkSaveOperation(
              string linkName,
              IEnumerable<KeyValuePair<string, string>> participantHashKeyValues,
              IEnumerable<KeyValuePair<string, string>> dependentChildKeyValues) { }
        }

        public sealed class DataVaultRegistrySatelliteSaveOperation {
          public DataVaultRegistrySatelliteSaveOperation(
              Modeling.DataVaultMetadataReference parent,
              string satelliteName,
              string parentHashKey,
              IEnumerable<KeyValuePair<string, string>> payloadValues,
              string hashDiff) { }

          public DataVaultRegistrySatelliteSaveOperation(
              Modeling.DataVaultMetadataReference parent,
              string satelliteName,
              string parentHashKey,
              IEnumerable<KeyValuePair<string, string>> drivingKeyValues,
              IEnumerable<KeyValuePair<string, string>> payloadValues,
              string hashDiff) { }
        }
      }

      namespace DCoding.Data.DVault.Modeling {
        public sealed class DataVaultMetadataReference {
          public static DataVaultMetadataReference Hub(string name) => new();
        }
      }
      """;
}
