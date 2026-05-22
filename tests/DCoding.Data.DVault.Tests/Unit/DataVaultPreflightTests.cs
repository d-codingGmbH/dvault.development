using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultPreflightTests {
  [Fact]
  public void RunMarksOmittedOptionalLanesSkippedAndPreservesValidationDiagnostics() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var diagnosticsResult = CreateDiagnosticsResult(isValid: true);
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(diagnosticsResult),
        new DataVaultPreflightRequest(context, metadataModel));

    Assert.Equal(DataVaultPreflightStatus.Passed, report.Status);
    Assert.False(report.IsBlocked);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.ValidationProvider.Status);
    Assert.Same(diagnosticsResult, report.ValidationProvider.Report);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.ArtifactDrift.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.SnapshotDrift.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.MigrationGuardrail.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.RequestDiagnostics.Status);

    var display = report.ToDisplayString();
    Assert.Contains("DVault preflight: passed, passed 1, blocked 0, skipped 4.", display, StringComparison.Ordinal);
    Assert.Contains("artifact-drift: skipped", display, StringComparison.Ordinal);
    Assert.Contains("snapshot-drift: skipped", display, StringComparison.Ordinal);
    Assert.Contains("migration-guardrail: skipped", display, StringComparison.Ordinal);
    Assert.Contains("request-diagnostics: skipped", display, StringComparison.Ordinal);
  }

  [Fact]
  public void RunBlocksWhenValidationDiagnosticsContainErrors() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var diagnosticsResult = CreateDiagnosticsResult(isValid: false);
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(diagnosticsResult),
        new DataVaultPreflightRequest(context, metadataModel));

    Assert.Equal(DataVaultPreflightStatus.Blocked, report.Status);
    Assert.True(report.IsBlocked);
    Assert.Equal(DataVaultPreflightSectionStatus.Blocked, report.ValidationProvider.Status);
    Assert.Same(diagnosticsResult, report.ValidationProvider.Report);
    Assert.Contains("validation-provider: blocked", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("preflight-test-invalid", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void RunUsesReviewedArtifactImportForArtifactDrift() {
    var runtimeModel = CreateHubOnlyMetadataModel();
    var reviewedArtifactImport = ImportModel(CreateCustomerContactMetadataModel());
    using var context = CreateContext(runtimeModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, runtimeModel) {
          ReviewedArtifactImport = reviewedArtifactImport,
        });

    Assert.Equal(DataVaultPreflightStatus.Blocked, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Blocked, report.ArtifactDrift.Status);
    Assert.NotNull(report.ArtifactDrift.Report);
    Assert.True(report.ArtifactDrift.Report.HasBlockingDifferences);
    Assert.Contains(
        report.ArtifactDrift.Report.Differences,
        difference => difference.Code == "missing-entity" &&
            difference.LogicalName == "Satellite:Contact");
    Assert.Contains("artifact-drift: blocked", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void RunUsesExpectedImportAsArtifactDriftAuthorityWhenNoReviewedArtifactIsSupplied() {
    var artifactImport = ImportModel(CreateCustomerContactMetadataModel());
    using var context = CreateContext(CreateCustomerContactMetadataModel());

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, artifactImport));

    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.ArtifactDrift.Status);
    Assert.NotNull(report.ArtifactDrift.Report);
    Assert.False(report.ArtifactDrift.Report.HasBlockingDifferences);
    Assert.Contains(
        report.ArtifactDrift.Report.Differences,
        difference => difference.Code == "metadata-source-kind-mismatch");
  }

  [Fact]
  public void RunSeparatesSnapshotPreflightDriftSections() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var context = CreateContext(metadataModel);
    var snapshotModel = CreateSnapshotModel(CreateHubOnlyMetadataModel());

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          SnapshotModel = snapshotModel,
        });

    Assert.Equal(DataVaultPreflightStatus.Blocked, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Blocked, report.SnapshotDrift.Status);
    Assert.NotNull(report.SnapshotDrift.Report);
    Assert.Empty(report.SnapshotDrift.Report.MetadataVersusRuntime.Differences);
    Assert.True(report.SnapshotDrift.Report.MetadataVersusSnapshotModel.HasBlockingDifferences);
    Assert.True(report.SnapshotDrift.Report.RuntimeVersusSnapshotModel.HasBlockingDifferences);
    Assert.Contains("metadata-versus-snapshot-model:", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("runtime-versus-snapshot-model:", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void RunUsesMigrationGuardrailReportWhenOperationsAreSupplied() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var provider = CreateServiceProvider();
    using var context = CreateContext(metadataModel);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();

    var report = DataVaultPreflight.Run(
        diagnostics,
        new DataVaultPreflightRequest(context, metadataModel) {
          MigrationOperations = [
            new DropTableOperation {
              Name = "HubCustomer",
            },
          ],
        });

    Assert.Equal(DataVaultPreflightStatus.Blocked, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Blocked, report.MigrationGuardrail.Status);
    Assert.NotNull(report.MigrationGuardrail.Report);
    Assert.False(report.MigrationGuardrail.Report.IsValid);
    var issue = Assert.Single(report.MigrationGuardrail.Report.Issues);
    Assert.Equal("DVM2006", issue.Code);
    Assert.Equal("migration/DropTable/HubCustomer", issue.Path);
    Assert.Contains("migration-guardrail: blocked", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("DVault migration guardrails: invalid", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void RunPreservesPrecomputedAndFactoryRequestDiagnosticsInOrder() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var precomputedReadDiagnostics = CreateDiagnosticsResult(isValid: true) with {
      ReadStrategy = new DataVaultReadStrategyDiagnostics(
          DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
          ProviderName: "Unit.Provider",
          SelectedStrategyName: "UnitReadStrategy",
          SelectedStrategyPriority: 25,
          Candidates: Array.Empty<DataVaultReadStrategyCandidateDiagnostics>(),
          FallbackCauses: Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };
    var blockingDiagnostics = CreateDiagnosticsResult(isValid: false);
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          RepresentativeDiagnostics = [
            new DataVaultPreflightRepresentativeDiagnostics("latest-contact", precomputedReadDiagnostics),
          ],
          RepresentativeDiagnosticsRequests = [
            new DataVaultPreflightRepresentativeDiagnosticsRequest("blocking-request", _ => blockingDiagnostics),
          ],
        });

    Assert.Equal(DataVaultPreflightStatus.Blocked, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Blocked, report.RequestDiagnostics.Status);
    Assert.NotNull(report.RequestDiagnostics.Report);
    Assert.Equal(2, report.RequestDiagnostics.Report.Results.Count);
    Assert.Equal(1, report.RequestDiagnostics.Report.BlockingResultCount);
    Assert.Same(precomputedReadDiagnostics, report.RequestDiagnostics.Report.Results[0].Diagnostics);
    Assert.Same(blockingDiagnostics, report.RequestDiagnostics.Report.Results[1].Diagnostics);
    Assert.Equal(["latest-contact", "blocking-request"], report.RequestDiagnostics.Report.Results.Select(result => result.Name));
    Assert.Contains("UnitReadStrategy", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("request-diagnostics: blocked", report.ToDisplayString(), StringComparison.Ordinal);
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static PreflightContext CreateContext(DataVaultMetadataModel metadataModel) {
    var options = new DbContextOptionsBuilder<PreflightContext>()
        .UseSqlite("Data Source=:memory:")
        .ReplaceService<IModelCacheKeyFactory, PreflightModelCacheKeyFactory>()
        .Options;

    return new PreflightContext(options, metadataModel);
  }

  private static IReadOnlyModel CreateSnapshotModel(DataVaultMetadataModel metadataModel) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    return modelBuilder.Model;
  }

  private static DataVaultModelImportResult ImportModel(DataVaultMetadataModel metadataModel) {
    return DataVaultModelArtifactImporter.ImportJson(
        DataVaultModelArtifactExporter.ExportJson(metadataModel),
        "preflight-test.model.json");
  }

  private static DataVaultMetadataModel CreateCustomerContactMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        [new DataVaultSatelliteMetadata("Contact", DataVaultMetadataReference.Hub("Customer"), ["EmailAddress"])]);
  }

  private static DataVaultMetadataModel CreateHubOnlyMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        []);
  }

  private static DataVaultDiagnosticsResult CreateDiagnosticsResult(bool isValid) {
    var issue = new DataVaultDiagnosticsIssue(
        DataVaultDiagnosticsIssueSeverity.Error,
        "preflight-test-invalid",
        "The preflight test diagnostics result is invalid.",
        "preflight-test");
    var issues = isValid
        ? Array.Empty<DataVaultDiagnosticsIssue>()
        : new[] { issue };

    return new DataVaultDiagnosticsResult(
        new DataVaultValidationDiagnostics(isValid, issues),
        new DataVaultExplainDiagnostics(
            "preflight-test",
            "test-fingerprint",
            "Unit.Provider",
            DataVaultProviderCapabilityProfiles.Sqlite.ProfileName,
            false,
            DataVaultProviderValueFormat.Text,
            "TEXT",
            DataVaultProviderBehaviorProfiles.ProviderNeutral.ProfileName,
            false,
            Array.Empty<DataVaultEntityExplain>()),
        new DataVaultSaveStrategyDiagnostics(
            DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated,
            ProviderName: "Unit.Provider",
            SelectedStrategyName: null,
            SelectedStrategyPriority: null,
            Candidates: Array.Empty<DataVaultSaveStrategyCandidateDiagnostics>(),
            FallbackCauses: Array.Empty<DataVaultSaveStrategyFallbackCause>()),
        issues);
  }

  private sealed class PreflightContext(
      DbContextOptions<PreflightContext> options,
      DataVaultMetadataModel metadataModel) : DbContext(options) {
    public DataVaultMetadataModel MetadataModel { get; } = metadataModel;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(MetadataModel);
    }
  }

  private sealed class PreflightModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is PreflightContext preflightContext
          ? (context.GetType(), preflightContext.MetadataModel, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed class StubDiagnosticsService(DataVaultDiagnosticsResult result) : IDataVaultDiagnosticsService {
    public DataVaultDiagnosticsResult Analyze(DataVaultMetadataModel metadataModel) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(
        DataVaultMetadataModel metadataModel,
        DataVaultProviderCapabilityProfile providerCapabilities) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(DataVaultMetadataRegistry metadataRegistry) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(
        DataVaultMetadataRegistry metadataRegistry,
        DataVaultProviderCapabilityProfile providerCapabilities) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(Action<DataVaultCodeFirstModelBuilder> configureModel) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(
        Action<DataVaultCodeFirstModelBuilder> configureModel,
        DataVaultProviderCapabilityProfile providerCapabilities) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(DbContext dbContext) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(
        DbContext dbContext,
        DataVaultSaveRequest request) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(
        DbContext dbContext,
        DataVaultBulkSaveRequest request) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(
        DbContext dbContext,
        DataVaultRegistrySaveRequest request) {
      return result;
    }

    public DataVaultDiagnosticsResult Analyze(
        DbContext dbContext,
        DataVaultRegistryBulkSaveRequest request) {
      return result;
    }
  }
}
