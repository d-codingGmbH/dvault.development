using System.Collections.Immutable;
using DCoding.Data.DVault.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace DCoding.Data.DVault.Tests.Analyzers;

public sealed class DataVaultEfCoreMisuseAnalyzerTests {
  [Fact]
  public void SupportedDiagnosticsExposeEfCoreMisuseMetadata() {
    var analyzer = new DataVaultEfCoreMisuseAnalyzer();
    var descriptors = analyzer.SupportedDiagnostics.ToDictionary(descriptor => descriptor.Id, StringComparer.Ordinal);

    Assert.Equal(["DMV1910", "DMV1911"], analyzer.SupportedDiagnostics.Select(descriptor => descriptor.Id).ToArray());
    AssertDescriptor(
        descriptors["DMV1910"],
        "EfCore",
        "Unsupported generated DVault DbSet exposure",
        "source-visible DVault generated shared-type tables",
        "context.Set<Dictionary<string, object>>(producedName)");
    AssertDescriptor(
        descriptors["DMV1911"],
        "EfCore",
        "Unsafe direct generated DVault table write",
        "source-visible DVault produced table name",
        "Use IDataVaultSaveService");
  }

  [Fact]
  public async Task ReportsGeneratedSharedTypeDbSetExposedOnDbContext() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: "",
        contextMembers: """
            public DbSet<Dictionary<string, object>> HubOrders => Set<Dictionary<string, object>>("HubOrder");
        """));
    var diagnostic = Assert.Single(diagnostics);

    Assert.Equal("DMV1910", diagnostic.Id);
    Assert.Equal("EfCore", diagnostic.Descriptor.Category);
    Assert.Contains(
        "DbContext member 'HubOrders' exposes DVault generated shared-type table 'HubOrder'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportPrivateGeneratedSetCacheOrOrdinaryEntityDbSetMembers() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: "",
        contextMembers: """
            private DbSet<Dictionary<string, object>> HubOrders => Set<Dictionary<string, object>>("HubOrder");

            public DbSet<Customer> CustomersAgain => Set<Customer>();
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportArbitraryDictionarySharedTypeDbSetMembers() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: "",
        contextMembers: """
            public DbSet<Dictionary<string, object>> LookupRows => Set<Dictionary<string, object>>("LookupRows");
        """));

    Assert.Empty(diagnostics);
  }

  [Theory]
  [InlineData(
      "Add",
      "context.Set<Dictionary<string, object>>(\"HubOrder\").Add(new Dictionary<string, object>());")]
  [InlineData(
      "AddAsync",
      "await context.Set<Dictionary<string, object>>(\"HubOrder\").AddAsync(new Dictionary<string, object>());")]
  [InlineData(
      "AddRange",
      "context.Set<Dictionary<string, object>>(\"HubOrder\").AddRange(new Dictionary<string, object>());")]
  [InlineData(
      "AddRangeAsync",
      "await context.Set<Dictionary<string, object>>(\"HubOrder\").AddRangeAsync(new Dictionary<string, object>());")]
  [InlineData(
      "Attach",
      "context.Set<Dictionary<string, object>>(\"HubOrder\").Attach(new Dictionary<string, object>());")]
  [InlineData(
      "Remove",
      "context.Set<Dictionary<string, object>>(\"HubOrder\").Remove(new Dictionary<string, object>());")]
  [InlineData(
      "Update",
      "context.Set<Dictionary<string, object>>(\"HubOrder\").Update(new Dictionary<string, object>());")]
  public async Task ReportsDirectGeneratedSharedTypeWrites(string methodName, string usageBody) {
    var diagnostics = await AnalyzeAsync(CreateSource(usageBody));
    var diagnostic = Assert.Single(diagnostics);

    Assert.Equal("DMV1911", diagnostic.Id);
    Assert.Equal("EfCore", diagnostic.Descriptor.Category);
    Assert.Contains(
        methodName + "(...) writes directly to DVault generated shared-type table 'HubOrder'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportArbitraryDictionarySharedTypeWrites() {
    var diagnostics = await AnalyzeAsync(CreateSource("""
        context.Set<Dictionary<string, object>>("LookupRows").Add(new Dictionary<string, object>());
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportVisibleMetadataInterceptorOptInLane() {
    var diagnostics = await AnalyzeAsync(CreateSource("""
        optionsBuilder.UseDataVaultSaveChangesMetadataInterceptor(options =>
            options.UseLoadTimestamp(DateTimeOffset.UtcNow).UseRecordSource("seed"));

        context.Set<Dictionary<string, object>>("HubOrder").Add(new Dictionary<string, object>());
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportDocumentedGeneratedTableReadPatterns() {
    var diagnostics = await AnalyzeAsync(CreateSource("""
        var rows = context.Set<Dictionary<string, object>>("HubOrder")
            .AsNoTracking()
            .Where(row => EF.Property<string>(row, "OrderHashKey") == "HK")
            .Select(row => EF.Property<string>(row, "RecordSource"))
            .ToArray();

        var compiled = EF.CompileQuery((VaultContext current, string orderHashKey) =>
            current.Set<Dictionary<string, object>>("HubOrder")
                .AsNoTracking()
                .Where(row => EF.Property<string>(row, "OrderHashKey") == orderHashKey)
                .Select(row => EF.Property<string>(row, "RecordSource"))
                .Single());

        _ = rows;
        _ = compiled(context, "HK");
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportExplicitSaveServiceBoundaryOrInterceptorRegistration() {
    var diagnostics = await AnalyzeAsync(CreateSource("""
        await saveService.SaveAsync(context, request);

        optionsBuilder.UseDataVaultSaveChangesMetadataInterceptor(options =>
            options.UseLoadTimestamp(DateTimeOffset.UtcNow).UseRecordSource("seed"));
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
    using var workspace = CreateWorkspaceWithDocument(source, out var document);
    var compilation = await document.Project.GetCompilationAsync(TestContext.Current.CancellationToken);
    Assert.NotNull(compilation);

    var compilerDiagnostics = compilation.GetDiagnostics(TestContext.Current.CancellationToken)
        .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
        .ToArray();

    Assert.Empty(compilerDiagnostics);

    var compilationWithAnalyzers = compilation.WithAnalyzers(
        ImmutableArray.Create<DiagnosticAnalyzer>(new DataVaultEfCoreMisuseAnalyzer()),
        options: null);
    var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

    return diagnostics
        .OrderBy(diagnostic => diagnostic.Id, StringComparer.Ordinal)
        .ThenBy(diagnostic => diagnostic.Location.SourceSpan.Start)
        .ToArray();
  }

  private static AdhocWorkspace CreateWorkspaceWithDocument(string source, out Document document) {
    var workspace = new AdhocWorkspace();
    var projectInfo = ProjectInfo.Create(
            ProjectId.CreateNewId(),
            VersionStamp.Create(),
            "DVaultEfMisuseAnalyzerSample",
            "DVaultEfMisuseAnalyzerSample",
            LanguageNames.CSharp)
        .WithMetadataReferences(CreateReferences())
        .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    var project = workspace.AddProject(projectInfo);
    document = workspace.AddDocument(project.Id, "Sample.cs", SourceText.From(source));

    return workspace;
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

  private static string CreateSource(string usageBody, string contextMembers = "") {
    return """
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Linq.Expressions;
        using System.Threading;
        using System.Threading.Tasks;
        using DCoding.Data.DVault;
        using Microsoft.EntityFrameworkCore;
        using Microsoft.EntityFrameworkCore.ChangeTracking;

        namespace AnalyzerSample {
          public sealed class Customer {
            public string CustomerId { get; init; } = "";
          }

          public sealed class VaultContext : DbContext {
            public DbSet<Customer> Customers => Set<Customer>();
        """ +
        contextMembers +
        """
          }

          public static class SampleUsage {
            public static async Task ExecuteAsync(
                VaultContext context,
                IDataVaultSaveService saveService,
                DataVaultSaveRequest request,
                DbContextOptionsBuilder optionsBuilder) {
        """ +
        usageBody +
        """
            }
          }
        }

        namespace Microsoft.EntityFrameworkCore {
          public class DbContext {
            public DbSet<TEntity> Set<TEntity>()
                where TEntity : class {
              return new DbSet<TEntity>();
            }

            public DbSet<TEntity> Set<TEntity>(string name)
                where TEntity : class {
              _ = name;

              return new DbSet<TEntity>();
            }
          }

          public class DbSet<TEntity>
              where TEntity : class {
            public EntityEntry<TEntity> Add(TEntity entity) {
              return new EntityEntry<TEntity>();
            }

            public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity) {
              return new ValueTask<EntityEntry<TEntity>>(new EntityEntry<TEntity>());
            }

            public void AddRange(params TEntity[] entities) {
            }

            public Task AddRangeAsync(params TEntity[] entities) {
              return Task.CompletedTask;
            }

            public EntityEntry<TEntity> Attach(TEntity entity) {
              return new EntityEntry<TEntity>();
            }

            public void AttachRange(params TEntity[] entities) {
            }

            public EntityEntry<TEntity> Remove(TEntity entity) {
              return new EntityEntry<TEntity>();
            }

            public void RemoveRange(params TEntity[] entities) {
            }

            public EntityEntry<TEntity> Update(TEntity entity) {
              return new EntityEntry<TEntity>();
            }

            public void UpdateRange(params TEntity[] entities) {
            }
          }

          public sealed class DbContextOptionsBuilder {
          }

          public static class EF {
            public static Func<TContext, TParameter, TResult> CompileQuery<TContext, TParameter, TResult>(
                Expression<Func<TContext, TParameter, TResult>> queryExpression) {
              return queryExpression.Compile();
            }

            public static TProperty Property<TProperty>(object entity, string propertyName) {
              _ = entity;
              _ = propertyName;

              return default!;
            }
          }

          public static class EntityFrameworkQueryableExtensions {
            public static IQueryable<TEntity> AsNoTracking<TEntity>(this DbSet<TEntity> source)
                where TEntity : class {
              _ = source;

              return Enumerable.Empty<TEntity>().AsQueryable();
            }
          }
        }

        namespace Microsoft.EntityFrameworkCore.ChangeTracking {
          public sealed class EntityEntry<TEntity>
              where TEntity : class {
          }
        }

        namespace DCoding.Data.DVault {
          public interface IDataVaultSaveService {
            Task<DataVaultSaveResult> SaveAsync(
                DbContext dbContext,
                DataVaultSaveRequest request,
                CancellationToken cancellationToken = default);
          }

          public sealed class DataVaultSaveRequest {
          }

          public sealed class DataVaultSaveResult {
          }

          public sealed class DataVaultSaveChangesMetadataInterceptorOptions {
            public DataVaultSaveChangesMetadataInterceptorOptions UseLoadTimestamp(DateTimeOffset loadTimestamp) {
              _ = loadTimestamp;

              return this;
            }

            public DataVaultSaveChangesMetadataInterceptorOptions UseRecordSource(string recordSource) {
              _ = recordSource;

              return this;
            }
          }

          public static class DataVaultDbContextOptionsBuilderExtensions {
            public static DbContextOptionsBuilder UseDataVaultSaveChangesMetadataInterceptor(
                this DbContextOptionsBuilder optionsBuilder,
                Func<DataVaultSaveChangesMetadataInterceptorOptions, DataVaultSaveChangesMetadataInterceptorOptions> configure) {
              _ = configure(new DataVaultSaveChangesMetadataInterceptorOptions());

              return optionsBuilder;
            }
          }
        }
        """;
  }
}
