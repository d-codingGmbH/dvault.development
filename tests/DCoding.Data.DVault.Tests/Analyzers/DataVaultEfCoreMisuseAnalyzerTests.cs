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

    Assert.Equal(["DMV1910", "DMV1911", "DMV1912", "DMV1913", "DMV1914"], analyzer.SupportedDiagnostics.Select(descriptor => descriptor.Id).ToArray());
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
    AssertDescriptor(
        descriptors["DMV1912"],
        "EfCore",
        "Missing DVault model-cache discriminator",
        "source-visible DVault model-shape variation",
        "Replace IModelCacheKeyFactory");
    AssertDescriptor(
        descriptors["DMV1913"],
        "EfCore",
        "Unsafe DVault compiled-model selection",
        "source-visible UseModel(...)",
        "Use compiled models only for one fixed realized DVault model shape");
    AssertDescriptor(
        descriptors["DMV1914"],
        "EfCore",
        "Unsafe DVault DbContext pooling",
        "source-visible AddDbContextPool<TContext>(...)",
        "Use DbContext pooling only for options-only DVault contexts");
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

  [Fact]
  public async Task ReportsMissingCacheKeyWhenDataVaultModelShapeUsesContextInstanceState() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: "",
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema) : base(options) {
              TenantSchema = tenantSchema;
            }

            public string TenantSchema { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));
    var diagnostic = Assert.Single(diagnostics);

    Assert.Equal("DMV1912", diagnostic.Id);
    Assert.Equal("EfCore", diagnostic.Descriptor.Category);
    Assert.Contains(
        "DbContext 'VaultContext' varies its DVault EF model shape from 'TenantSchema'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportVariableShapeWhenVisibleCacheKeyIncludesContextDiscriminators() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            _ = new DbContextOptionsBuilder<VaultContext>()
                .ReplaceService<IModelCacheKeyFactory, VaultModelCacheKeyFactory>();
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema, string tablePrefix) : base(options) {
              TenantSchema = tenantSchema;
              TablePrefix = tablePrefix;
            }

            public string TenantSchema { get; }

            public string TablePrefix { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
              modelBuilder.SharedTypeEntity<Dictionary<string, object>>("HubCustomer", entity => {
                entity.ToTable(TablePrefix + "HubCustomer", TenantSchema);
              });
            }
        """,
        additionalDeclarations: """
          public sealed class VaultModelCacheKeyFactory : IModelCacheKeyFactory {
            public object Create(DbContext context, bool designTime) {
              return context is VaultContext vaultContext
                  ? (context.GetType(), vaultContext.TenantSchema, vaultContext.TablePrefix, designTime)
                  : (object)(context.GetType(), designTime);
            }
          }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportMissingCacheKeyForContextStateOutsideDataVaultShape() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: "",
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string diagnosticLabel) : base(options) {
              DiagnosticLabel = diagnosticLabel;
            }

            public string DiagnosticLabel { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              _ = DiagnosticLabel.Length;
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportMissingCacheKeyWhenCustomCacheKeyComputationIsOpaque() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            _ = new DbContextOptionsBuilder<VaultContext>()
                .ReplaceService<IModelCacheKeyFactory, VaultModelCacheKeyFactory>();
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema) : base(options) {
              TenantSchema = tenantSchema;
            }

            public string TenantSchema { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """,
        additionalDeclarations: """
          public sealed class VaultModelCacheKeyFactory : IModelCacheKeyFactory {
            public object Create(DbContext context, bool designTime) {
              return CacheKeyHelpers.Build(context, designTime);
            }
          }

          public static class CacheKeyHelpers {
            public static object Build(DbContext context, bool designTime) {
              _ = context;
              _ = designTime;

              return new object();
            }
          }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task ReportsMissingCacheKeyWhenDataVaultProfileSelectionUsesContextStateThroughLocal() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: "",
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, bool useOracleProfile) : base(options) {
              UseOracleProfile = useOracleProfile;
            }

            public bool UseOracleProfile { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              var providerCapabilities = UseOracleProfile
                  ? DataVaultProviderCapabilityProfiles.Oracle
                  : DataVaultProviderCapabilityProfiles.Sqlite;

              modelBuilder.ApplyDataVaultMetadata(vault => { }, providerCapabilities);
            }
        """));
    var diagnostic = Assert.Single(diagnostics.Where(diagnostic => diagnostic.Id == "DMV1912"));

    Assert.Contains(
        "DbContext 'VaultContext' varies its DVault EF model shape from 'UseOracleProfile'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportMissingCacheKeyFromOptionsRegistrationSelection() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var useOracle = DateTimeOffset.UtcNow.Day == 1;
            var services = new ServiceCollection();
            services.AddDbContext<VaultContext>(options => {
              if (useOracle) {
                options.UseOracle("Data Source=test");
              } else {
                options.UseSqlite("Data Source=test.db");
              }
            });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options) : base(options) {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task ReportsUnsafeUseModelForVisibleVariableDataVaultShape() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            IModel runtimeModel = new RuntimeModel();
            _ = new DbContextOptionsBuilder<VaultContext>().UseModel(runtimeModel);
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema) : base(options) {
              TenantSchema = tenantSchema;
            }

            public string TenantSchema { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));
    var diagnostic = Assert.Single(diagnostics.Where(diagnostic => diagnostic.Id == "DMV1913"));

    Assert.Equal("EfCore", diagnostic.Descriptor.Category);
    Assert.Contains(
        "UseModel(...) applies a compiled EF model to DVault context 'VaultContext'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportUseModelForFixedDataVaultShape() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            IModel runtimeModel = new RuntimeModel();
            _ = new DbContextOptionsBuilder<VaultContext>().UseModel(runtimeModel);
        """,
        contextMembers: """
            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportUseModelOrPoolingForGetterBackedFixedDataVaultShape() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            IModel runtimeModel = new RuntimeModel();
            _ = new DbContextOptionsBuilder<VaultContext>().UseModel(runtimeModel);

            var services = new ServiceCollection();
            services.AddDbContextPool<VaultContext>(options => { });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options) : base(options) {
            }

            public string TenantSchema => "tenant_a";

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task ReportsUnsafeUseModelWhenDesignRuntimeLaneUsesVariableShape() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var tenantSchema = DateTimeOffset.UtcNow.Day == 1 ? "tenant_a" : "tenant_b";
            var designOptions = new DbContextOptionsBuilder<VaultContext>()
                .ReplaceService<IModelCacheKeyFactory, VaultModelCacheKeyFactory>()
                .Options;
            var designContext = new VaultContext(designOptions, tenantSchema);
            var designModel = designContext.GetService<IDesignTimeModel>().Model;
            var runtimeModel = designContext.GetService<IModelRuntimeInitializer>()
                .Initialize(designModel, designTime: false, validationLogger: null);
            _ = new DbContextOptionsBuilder<VaultContext>()
                .ReplaceService<IModelCacheKeyFactory, VaultModelCacheKeyFactory>()
                .UseModel(runtimeModel);
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema) : base(options) {
              TenantSchema = tenantSchema;
            }

            public string TenantSchema { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """,
        additionalDeclarations: """
          public sealed class VaultModelCacheKeyFactory : IModelCacheKeyFactory {
            public object Create(DbContext context, bool designTime) {
              return context is VaultContext vaultContext
                  ? (context.GetType(), vaultContext.TenantSchema, designTime)
                  : (object)(context.GetType(), designTime);
            }
          }
        """));
    var diagnostic = Assert.Single(diagnostics.Where(diagnostic => diagnostic.Id == "DMV1913"));

    Assert.Contains(
        "UseModel(...) applies a compiled EF model to DVault context 'VaultContext'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportUseModelForVisibleDesignRuntimeModelLane() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var designOptions = new DbContextOptionsBuilder<VaultContext>()
                .ReplaceService<IModelCacheKeyFactory, VaultModelCacheKeyFactory>()
                .Options;
            var designContext = new VaultContext(designOptions, "tenant_a");
            var designModel = designContext.GetService<IDesignTimeModel>().Model;
            var runtimeModel = designContext.GetService<IModelRuntimeInitializer>()
                .Initialize(designModel, designTime: false, validationLogger: null);
            _ = new DbContextOptionsBuilder<VaultContext>()
                .ReplaceService<IModelCacheKeyFactory, VaultModelCacheKeyFactory>()
                .UseModel(runtimeModel);
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema) : base(options) {
              TenantSchema = tenantSchema;
            }

            public string TenantSchema { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """,
        additionalDeclarations: """
          public sealed class VaultModelCacheKeyFactory : IModelCacheKeyFactory {
            public object Create(DbContext context, bool designTime) {
              return context is VaultContext vaultContext
                  ? (context.GetType(), vaultContext.TenantSchema, designTime)
                  : (object)(context.GetType(), designTime);
            }
          }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task ReportsUnsafeDbContextPoolWhenDataVaultProfileSelectionUsesContextStateThroughLocal() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var services = new ServiceCollection();
            services.AddDbContextPool<VaultContext>(options => { });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, bool useOracleProfile) : base(options) {
              UseOracleProfile = useOracleProfile;
            }

            public bool UseOracleProfile { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              var providerCapabilities = UseOracleProfile
                  ? DataVaultProviderCapabilityProfiles.Oracle
                  : DataVaultProviderCapabilityProfiles.Sqlite;

              modelBuilder.ApplyDataVaultMetadata(vault => { }, providerCapabilities);
            }
        """));
    var diagnostic = Assert.Single(diagnostics.Where(diagnostic => diagnostic.Id == "DMV1914"));

    Assert.Contains(
        "AddDbContextPool<VaultContext>(...) pools a DVault context whose visible model shape varies from 'UseOracleProfile'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task ReportsUnsafeDbContextPoolWhenProviderSelectionVariesInRegistration() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var services = new ServiceCollection();
            services.AddDbContextPool<VaultContext>(options => {
              if (useOracle) {
                options.UseOracle("Data Source=test");
              } else {
                options.UseSqlite("Data Source=test.db");
              }
            });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options) : base(options) {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """,
        additionalUsageParameters: ", bool useOracle"));
    var diagnostic = Assert.Single(diagnostics.Where(diagnostic => diagnostic.Id == "DMV1914"));

    Assert.Contains(
        "AddDbContextPool<VaultContext>(...) pools a DVault context whose visible model shape varies from 'useOracle'",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportDbContextPoolWhenProviderSelectionUsesRegistrationServiceProvider() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var services = new ServiceCollection();
            services.AddDbContextPool<VaultContext>((serviceProvider, options) => {
              if (serviceProvider.GetHashCode() == 0) {
                options.UseOracle("Data Source=test");
              } else {
                options.UseSqlite("Data Source=test.db");
              }
            });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options) : base(options) {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportDbContextPoolWhenProviderSelectionUsesOpaqueHelperLocal() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var useOracle = ProviderSelection.ShouldUseOracle();
            var services = new ServiceCollection();
            services.AddDbContextPool<VaultContext>(options => {
              if (useOracle) {
                options.UseOracle("Data Source=test");
              } else {
                options.UseSqlite("Data Source=test.db");
              }
            });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options) : base(options) {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """,
        additionalDeclarations: """
          public static class ProviderSelection {
            public static bool ShouldUseOracle() {
              return DateTimeOffset.UtcNow.Day == 1;
            }
          }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task ReportsUnsafeDbContextPoolForVisibleVariableDataVaultShape() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var services = new ServiceCollection();
            services.AddDbContextPool<VaultContext>(options => { });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema) : base(options) {
              TenantSchema = tenantSchema;
            }

            public string TenantSchema { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));
    var diagnostic = Assert.Single(diagnostics.Where(diagnostic => diagnostic.Id == "DMV1914"));

    Assert.Equal("EfCore", diagnostic.Descriptor.Category);
    Assert.Contains(
        "AddDbContextPool<VaultContext>(...) pools a DVault context",
        diagnostic.GetMessage(),
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task DoesNotReportDbContextPoolForOptionsOnlyFixedDataVaultShape() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var services = new ServiceCollection();
            services.AddDbContextPool<VaultContext>(options => { });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options) : base(options) {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """));

    Assert.Empty(diagnostics);
  }

  [Fact]
  public async Task DoesNotReportUserNamedDbContextPoolMethodOutsideEfRegistrationNamespace() {
    var diagnostics = await AnalyzeAsync(CreateSource(
        usageBody: """
            var services = new ServiceCollection();
            _ = new DbContextOptionsBuilder<VaultContext>()
                .ReplaceService<IModelCacheKeyFactory, VaultModelCacheKeyFactory>();
            UserLifecycleExtensions.AddDbContextPool<VaultContext>(services, options => { });
        """,
        contextMembers: """
            public VaultContext(DbContextOptions<VaultContext> options, string tenantSchema) : base(options) {
              TenantSchema = tenantSchema;
            }

            public string TenantSchema { get; }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
              modelBuilder.HasDefaultSchema(TenantSchema);
              modelBuilder.ApplyDataVaultMetadata(vault => { });
            }
        """,
        additionalDeclarations: """
          public sealed class VaultModelCacheKeyFactory : IModelCacheKeyFactory {
            public object Create(DbContext context, bool designTime) {
              return context is VaultContext vaultContext
                  ? (context.GetType(), vaultContext.TenantSchema, designTime)
                  : (object)(context.GetType(), designTime);
            }
          }

          public static class UserLifecycleExtensions {
            public static IServiceCollection AddDbContextPool<TContext>(
                IServiceCollection services,
                Action<DbContextOptionsBuilder> optionsAction)
                where TContext : DbContext {
              optionsAction(new DbContextOptionsBuilder());

              return services;
            }
          }
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

  private static string CreateSource(
      string usageBody,
      string contextMembers = "",
      string additionalDeclarations = "",
      string additionalUsageParameters = "") {
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
        using Microsoft.EntityFrameworkCore.Infrastructure;
        using Microsoft.EntityFrameworkCore.Metadata;
        using Microsoft.EntityFrameworkCore.Metadata.Builders;
        using Microsoft.Extensions.DependencyInjection;

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
        """ +
        "                DbContextOptionsBuilder optionsBuilder" +
        additionalUsageParameters +
        ") {\n" +
        usageBody +
        """
            }
          }
        """ +
        additionalDeclarations +
        """
        }

        namespace Microsoft.EntityFrameworkCore {
          public class DbContext {
            public DbContext() {
            }

            public DbContext(DbContextOptions options) {
              _ = options;
            }

            public DbSet<TEntity> Set<TEntity>()
                where TEntity : class {
              return new DbSet<TEntity>();
            }

            public DbSet<TEntity> Set<TEntity>(string name)
                where TEntity : class {
              _ = name;

              return new DbSet<TEntity>();
            }

            protected virtual void OnModelCreating(ModelBuilder modelBuilder) {
              _ = modelBuilder;
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

          public class DbContextOptions {
          }

          public sealed class DbContextOptions<TContext> : DbContextOptions
              where TContext : DbContext {
          }

          public class DbContextOptionsBuilder {
            public DbContextOptions Options => new DbContextOptions();
          }

          public sealed class DbContextOptionsBuilder<TContext> : DbContextOptionsBuilder
              where TContext : DbContext {
            public new DbContextOptions<TContext> Options => new DbContextOptions<TContext>();

            public DbContextOptionsBuilder<TContext> ReplaceService<TService, TImplementation>()
                where TImplementation : TService {
              return this;
            }

            public DbContextOptionsBuilder<TContext> UseModel(IModel model) {
              _ = model;

              return this;
            }
          }

          public sealed class ModelBuilder {
            public ModelBuilder HasDefaultSchema(string schema) {
              _ = schema;

              return this;
            }

            public ModelBuilder SharedTypeEntity<TEntity>(
                string name,
                Action<EntityTypeBuilder<TEntity>> buildAction)
                where TEntity : class {
              _ = name;
              buildAction(new EntityTypeBuilder<TEntity>());

              return this;
            }
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

          public static class DbContextOptionsBuilderExtensions {
            public static DbContextOptionsBuilder UseOracle(this DbContextOptionsBuilder optionsBuilder, string connectionString) {
              _ = connectionString;

              return optionsBuilder;
            }

            public static DbContextOptionsBuilder UseSqlite(this DbContextOptionsBuilder optionsBuilder, string connectionString) {
              _ = connectionString;

              return optionsBuilder;
            }
          }
        }

        namespace Microsoft.EntityFrameworkCore.ChangeTracking {
          public sealed class EntityEntry<TEntity>
              where TEntity : class {
          }
        }

        namespace Microsoft.EntityFrameworkCore.Infrastructure {
          public interface IModelCacheKeyFactory {
            object Create(DbContext context, bool designTime);
          }

          public interface IDesignTimeModel {
            IModel Model { get; }
          }

          public interface IModelRuntimeInitializer {
            IModel Initialize(IModel model, bool designTime, object? validationLogger);
          }

          public static class InfrastructureExtensions {
            public static TService GetService<TService>(this DbContext context)
                where TService : class {
              _ = context;

              return default!;
            }
          }
        }

        namespace Microsoft.EntityFrameworkCore.Metadata {
          public interface IModel {
          }

          public sealed class RuntimeModel : IModel {
          }
        }

        namespace Microsoft.EntityFrameworkCore.Metadata.Builders {
          public sealed class EntityTypeBuilder<TEntity>
              where TEntity : class {
            public EntityTypeBuilder<TEntity> ToTable(string name, string schema) {
              _ = name;
              _ = schema;

              return this;
            }
          }
        }

        namespace Microsoft.Extensions.DependencyInjection {
          public interface IServiceCollection {
          }

          public sealed class ServiceCollection : IServiceCollection {
          }

          public static class EntityFrameworkServiceCollectionExtensions {
            public static IServiceCollection AddDbContext<TContext>(
                this IServiceCollection services,
                Action<DbContextOptionsBuilder> optionsAction)
                where TContext : DbContext {
              optionsAction(new DbContextOptionsBuilder());

              return services;
            }

            public static IServiceCollection AddDbContextPool<TContext>(
                this IServiceCollection services,
                Action<DbContextOptionsBuilder> optionsAction)
                where TContext : DbContext {
              optionsAction(new DbContextOptionsBuilder());

              return services;
            }

            public static IServiceCollection AddDbContextPool<TContext>(
                this IServiceCollection services,
                Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
                where TContext : DbContext {
              optionsAction(new EmptyServiceProvider(), new DbContextOptionsBuilder());

              return services;
            }
          }

          public sealed class EmptyServiceProvider : IServiceProvider {
            public object? GetService(Type serviceType) {
              _ = serviceType;

              return null;
            }
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

          public sealed class DataVaultCodeFirstModelBuilder {
          }

          public sealed class DataVaultProviderCapabilityProfile {
          }

          public static class DataVaultProviderCapabilityProfiles {
            public static DataVaultProviderCapabilityProfile Oracle { get; } = new DataVaultProviderCapabilityProfile();

            public static DataVaultProviderCapabilityProfile Sqlite { get; } = new DataVaultProviderCapabilityProfile();
          }

          public static class DataVaultModelBuilderExtensions {
            public static ModelBuilder ApplyDataVaultMetadata(
                this ModelBuilder modelBuilder,
                Action<DataVaultCodeFirstModelBuilder> configureModel) {
              configureModel(new DataVaultCodeFirstModelBuilder());

              return modelBuilder;
            }

            public static ModelBuilder ApplyDataVaultMetadata(
                this ModelBuilder modelBuilder,
                Action<DataVaultCodeFirstModelBuilder> configureModel,
                DataVaultProviderCapabilityProfile providerCapabilities) {
              configureModel(new DataVaultCodeFirstModelBuilder());
              _ = providerCapabilities;

              return modelBuilder;
            }
          }
        }
        """;
  }
}
