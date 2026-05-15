using System.Collections.Immutable;
using DCoding.Data.DVault.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace DCoding.Data.DVault.Tests.Analyzers;

public sealed class DataVaultCodeFirstAnalyzerTests {
  [Fact]
  public void SupportedDiagnosticsExposeAnalyzerLocalCodeFirstMetadata() {
    var analyzer = new DataVaultCodeFirstAnalyzer();
    var descriptors = analyzer.SupportedDiagnostics.ToDictionary(descriptor => descriptor.Id, StringComparer.Ordinal);

    Assert.Equal(["DMV1901", "DMV1902"], analyzer.SupportedDiagnostics.Select(descriptor => descriptor.Id).ToArray());
    AssertDescriptor(
        descriptors["DMV1901"],
        "CodeFirst",
        "Unsupported Code-First selector shape",
        "BusinessKey(...), Payload(...), or DrivingKey(...)",
        "Use repeated direct readable scalar member selectors");
    AssertDescriptor(
        descriptors["DMV1902"],
        "CodeFirst",
        "Duplicate Code-First member declaration",
        "repeats a logical member name",
        "Declare each logical member name at most once");
  }

  [Theory]
  [InlineData(
      "BusinessKey",
      "vault.Hub<Customer>(hub => { hub.BusinessKey(customer => new { customer.CustomerId, customer.RegionCode }); });")]
  [InlineData(
      "Payload",
      "vault.Hub<Customer>(hub => { hub.BusinessKey(customer => customer.CustomerId); hub.Satellite(\"Contact\", satellite => { satellite.Payload(customer => customer.EmailAddress.ToUpperInvariant()); }); });")]
  [InlineData(
      "DrivingKey",
      "vault.Hub<Customer>(hub => { hub.BusinessKey(customer => customer.CustomerId); hub.Satellite(\"Contact\", satellite => { satellite.DrivingKey(customer => customer.Contact.EmailAddress); satellite.Payload(customer => customer.EmailAddress); }); });")]
  [InlineData(
      "Payload",
      "vault.Hub<Customer>(hub => { hub.BusinessKey(customer => customer.CustomerId); hub.Satellite(\"Contact\", satellite => { satellite.Payload(customer => customer.Tags); }); });")]
  public async Task ReportsUnsupportedSelectorShapes(string verb, string configureBody) {
    var diagnostics = await AnalyzeAsync(CreateSource(configureBody));
    var diagnostic = Assert.Single(diagnostics);

    Assert.Equal("DMV1901", diagnostic.Id);
    Assert.Equal("CodeFirst", diagnostic.Descriptor.Category);
    Assert.Contains(verb + " selector must target one direct readable scalar member", diagnostic.GetMessage(), StringComparison.Ordinal);
  }

  [Theory]
  [InlineData(
      "BusinessKey",
      "CustomerId",
      "vault.Hub<Customer>(hub => { hub.BusinessKey(customer => customer.CustomerId); hub.BusinessKey(customer => customer.CustomerId); });")]
  [InlineData(
      "Payload",
      "EmailAddress",
      "vault.Hub<Customer>(hub => { hub.BusinessKey(customer => customer.CustomerId); hub.Satellite(\"Contact\", satellite => { satellite.Payload(customer => customer.EmailAddress); satellite.Payload(customer => customer.EmailAddress); }); });")]
  [InlineData(
      "DrivingKey",
      "ContactType",
      "vault.Hub<Customer>(hub => { hub.BusinessKey(customer => customer.CustomerId); hub.Satellite(\"Contact\", satellite => { satellite.DrivingKey(customer => customer.ContactType); satellite.DrivingKey(customer => customer.ContactType); satellite.Payload(customer => customer.EmailAddress); }); });")]
  public async Task ReportsDuplicateMembersInsideOneFluentScope(string verb, string memberName, string configureBody) {
    var diagnostics = await AnalyzeAsync(CreateSource(configureBody));
    var diagnostic = Assert.Single(diagnostics);

    Assert.Equal("DMV1902", diagnostic.Id);
    Assert.Equal("CodeFirst", diagnostic.Descriptor.Category);
    Assert.Contains(verb + " member '" + memberName + "' is declared more than once", diagnostic.GetMessage(), StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportValidDirectReadableScalarMemberDeclarations() {
    var diagnostics = await AnalyzeAsync(CreateSource("""
        vault.Hub<Customer>(hub => {
          hub.BusinessKey(customer => customer.CustomerId);
          hub.BusinessKey(customer => customer.RegionCode);
          hub.Satellite("Contact", satellite => {
            satellite.DrivingKey(customer => customer.ContactType);
            satellite.Payload(customer => customer.EmailAddress);
            satellite.Payload(customer => customer.PhoneNumber);
          });
        });
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportDuplicateMemberNamesAcrossSeparateSatelliteScopes() {
    var diagnostics = await AnalyzeAsync(CreateSource("""
        vault.Hub<Customer>(hub => {
          hub.BusinessKey(customer => customer.CustomerId);
          hub.Satellite("Contact", satellite => {
            satellite.Payload(customer => customer.EmailAddress);
          });
          hub.Satellite("Profile", satellite => {
            satellite.Payload(customer => customer.EmailAddress);
          });
        });
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportSelectorVariablesOutsideTheFirstDirectLambdaSlice() {
    var diagnostics = await AnalyzeAsync(CreateSource("""
        Expression<Func<Customer, string>> emailSelector = customer => customer.EmailAddress;
        vault.Hub<Customer>(hub => {
          hub.BusinessKey(customer => customer.CustomerId);
          hub.Satellite("Contact", satellite => {
            satellite.Payload(emailSelector);
          });
        });
        """));

    Assert.Empty(diagnostics);
  }

  private static void AssertDescriptor(
      DiagnosticDescriptor descriptor,
      string expectedCategory,
      string expectedTitle,
      string expectedExplanationText,
      string expectedRemediationText) {
    Assert.Equal(expectedCategory, descriptor.Category);
    Assert.Equal(expectedTitle, descriptor.Title.ToString());
    Assert.Equal(DiagnosticSeverity.Warning, descriptor.DefaultSeverity);
    Assert.True(descriptor.IsEnabledByDefault);
    Assert.Contains(expectedExplanationText, descriptor.Description.ToString(), StringComparison.Ordinal);
    Assert.Contains(expectedRemediationText, descriptor.Description.ToString(), StringComparison.Ordinal);
  }

  private static async Task<IReadOnlyList<Diagnostic>> AnalyzeAsync(string source) {
    var syntaxTree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);
    var compilation = CSharpCompilation.Create(
        "DVaultAnalyzerSample",
        [syntaxTree],
        CreateReferences(),
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    var compilerDiagnostics = compilation.GetDiagnostics(TestContext.Current.CancellationToken)
        .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
        .ToArray();

    Assert.Empty(compilerDiagnostics);

    var compilationWithAnalyzers = compilation.WithAnalyzers(
        ImmutableArray.Create<DiagnosticAnalyzer>(new DataVaultCodeFirstAnalyzer()),
        options: null);
    var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

    return diagnostics
        .OrderBy(diagnostic => diagnostic.Id, StringComparer.Ordinal)
        .ThenBy(diagnostic => diagnostic.Location.SourceSpan.Start)
        .ToArray();
  }

  private static IReadOnlyList<MetadataReference> CreateReferences() {
    var trustedPlatformAssemblies = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))?
        .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) ??
        [];

    var references = trustedPlatformAssemblies
        .Select(path => MetadataReference.CreateFromFile(path))
        .GroupBy(reference => reference.Display, StringComparer.Ordinal)
        .Select(group => group.First())
        .ToArray();

    return references;
  }

  private static string CreateSource(string configureBody) {
    return """
        using System;
        using System.Linq.Expressions;
        using DCoding.Data.DVault;

        namespace AnalyzerSample {
          public sealed class Customer {
            public ContactInfo Contact { get; init; } = new();

            public string ContactType { get; init; } = "";

            public string CustomerId { get; init; } = "";

            public string EmailAddress { get; init; } = "";

            public string PhoneNumber { get; init; } = "";

            public string RegionCode { get; init; } = "";

            public string[] Tags { get; init; } = [];
          }

          public sealed class ContactInfo {
            public string EmailAddress { get; init; } = "";
          }

          public static class SampleConfiguration {
            public static void Configure(DataVaultCodeFirstModelBuilder vault) {
        """ +
        configureBody +
        """
            }
          }
        }

        namespace DCoding.Data.DVault {
          public sealed class DataVaultCodeFirstModelBuilder {
            public DataVaultCodeFirstHubBuilder<TEntity> Hub<TEntity>(
                Action<DataVaultCodeFirstHubBuilder<TEntity>>? configure = null)
                where TEntity : class {
              var builder = new DataVaultCodeFirstHubBuilder<TEntity>();
              configure?.Invoke(builder);

              return builder;
            }
          }

          public sealed class DataVaultCodeFirstHubBuilder<TEntity>
              where TEntity : class {
            public DataVaultCodeFirstHubBuilder<TEntity> BusinessKey<TProperty>(
                Expression<Func<TEntity, TProperty>> selector) {
              return this;
            }

            public DataVaultCodeFirstHubBuilder<TEntity> Satellite(
                string satelliteName,
                Action<DataVaultCodeFirstSatelliteBuilder<TEntity>>? configure = null) {
              var builder = new DataVaultCodeFirstSatelliteBuilder<TEntity>();
              configure?.Invoke(builder);

              return this;
            }
          }

          public sealed class DataVaultCodeFirstSatelliteBuilder<TEntity>
              where TEntity : class {
            public DataVaultCodeFirstSatelliteBuilder<TEntity> DrivingKey<TProperty>(
                Expression<Func<TEntity, TProperty>> selector) {
              return this;
            }

            public DataVaultCodeFirstSatelliteBuilder<TEntity> Payload<TProperty>(
                Expression<Func<TEntity, TProperty>> selector) {
              return this;
            }
          }
        }
        """;
  }
}
