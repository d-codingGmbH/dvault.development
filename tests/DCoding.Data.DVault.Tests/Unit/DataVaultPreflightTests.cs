using System.Text.Json;
using System.Text.Json.Nodes;
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
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.IdempotencySchema.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.HashKeyStorageMigrationManifest.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.MigrationGuardrail.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.RequestDiagnostics.Status);

    var display = report.ToDisplayString();
    Assert.Contains("DVault preflight: passed, passed 1, blocked 0, skipped 6.", display, StringComparison.Ordinal);
    Assert.Contains("artifact-drift: skipped", display, StringComparison.Ordinal);
    Assert.Contains("snapshot-drift: skipped", display, StringComparison.Ordinal);
    Assert.Contains("idempotency-schema: skipped", display, StringComparison.Ordinal);
    Assert.Contains("hash-key-storage-migration-manifest: skipped", display, StringComparison.Ordinal);
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
  public void RunValidatesHashKeyStorageMigrationManifestAsSeparateOptionalLane() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          HashKeyStorageMigrationManifestJson = CreateValidHashKeyStorageMigrationManifestJson(),
        });

    Assert.Equal(DataVaultPreflightStatus.Passed, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.HashKeyStorageMigrationManifest.Status);
    Assert.NotNull(report.HashKeyStorageMigrationManifest.Report);
    Assert.True(report.HashKeyStorageMigrationManifest.Report.IsValid);
    Assert.Equal(DataVaultPreflightSectionStatus.Skipped, report.MigrationGuardrail.Status);
    Assert.Contains("hash-key-storage-migration-manifest: passed", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("DVault hash-key storage migration manifest: valid", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void RunBlocksWhenHashKeyStorageMigrationManifestHasErrorFindings() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          HashKeyStorageMigrationManifestJson = MutateHashKeyStorageMigrationManifest(
              root => root["schemaVersion"] = "dvault.hash-key-storage-migration.v2"),
          MigrationOperations = [],
        });

    Assert.Equal(DataVaultPreflightStatus.Blocked, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Blocked, report.HashKeyStorageMigrationManifest.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.MigrationGuardrail.Status);
    Assert.NotNull(report.HashKeyStorageMigrationManifest.Report);
    Assert.Contains(
        report.HashKeyStorageMigrationManifest.Report.Findings,
        finding => finding.Code == "hash-key-migration-schema-version-unsupported");
    Assert.Contains("hash-key-storage-migration-manifest: blocked", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("migration-guardrail: passed", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void RunKeepsHashKeyStorageMigrationManifestWarningsNonBlocking() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          HashKeyStorageMigrationManifestJson = MutateHashKeyStorageMigrationManifest(
              root => root["source"]!["capabilityProfileDefaulted"] = true),
        });

    Assert.Equal(DataVaultPreflightStatus.Passed, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.HashKeyStorageMigrationManifest.Status);
    Assert.NotNull(report.HashKeyStorageMigrationManifest.Report);
    Assert.Contains(
        report.HashKeyStorageMigrationManifest.Report.Findings,
        finding => finding.Severity == DataVaultDiagnosticsIssueSeverity.Warning &&
            finding.Code == "hash-key-migration-capability-profile-defaulted");
  }

  [Fact]
  public void RunSerializesHashKeyStorageMigrationManifestFindingsWithoutRawManifestPayload() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          HashKeyStorageMigrationManifestJson = MutateHashKeyStorageMigrationManifest(
              root => root["rawHashKey"] = "raw-secret-hash-key"),
        });

    var json = JsonSerializer.Serialize(report);

    Assert.Contains("hash-key-migration-manifest-compatible", json, StringComparison.Ordinal);
    Assert.DoesNotContain("raw-secret-hash-key", json, StringComparison.Ordinal);
    Assert.DoesNotContain("rawHashKey", json, StringComparison.Ordinal);
  }

  [Fact]
  public void RunRedactsHashKeyStorageMigrationManifestStringFindingValues() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          HashKeyStorageMigrationManifestJson = MutateHashKeyStorageMigrationManifest(
              root => root["target"]!["metadataSourceFingerprint"] = "raw-secret-fingerprint"),
        });

    var json = JsonSerializer.Serialize(report);
    var display = report.ToDisplayString();

    Assert.Equal(DataVaultPreflightStatus.Blocked, report.Status);
    Assert.Contains("hash-key-migration-metadata-source-fingerprint-drift", json, StringComparison.Ordinal);
    Assert.Contains("redacted:metadata-source-fingerprint", json, StringComparison.Ordinal);
    Assert.DoesNotContain("raw-secret-fingerprint", json, StringComparison.Ordinal);
    Assert.Contains("hash-key-migration-metadata-source-fingerprint-drift", display, StringComparison.Ordinal);
    Assert.Contains("<redacted:metadata-source-fingerprint>", display, StringComparison.Ordinal);
    Assert.DoesNotContain("raw-secret-fingerprint", display, StringComparison.Ordinal);
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

  [Fact]
  public void RunEvaluatesExplicitIdempotencyLiveSchemaLaneWithoutOpeningLiveDatabase() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var expectedStructures = DataVaultIdempotencyPreflight.CreateExpectedStructures(
        metadataModel,
        DataVaultProviderCapabilityProfiles.Sqlite);
    using var context = CreateContext(metadataModel);

    var report = DataVaultPreflight.Run(
        new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        new DataVaultPreflightRequest(context, metadataModel) {
          IdempotencyLiveSchemaReadResult = DataVaultLiveSchemaReadResult.Success(
              "Microsoft.EntityFrameworkCore.Sqlite",
              CreateLiveSchema(expectedStructures)),
        });

    Assert.Equal(DataVaultPreflightStatus.Passed, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.IdempotencySchema.Status);
    Assert.NotNull(report.IdempotencySchema.Report);
    Assert.Equal(DataVaultIdempotencyPreflightStatus.Passed, report.IdempotencySchema.Report.Status);
    Assert.Contains("idempotency-schema: passed", report.ToDisplayString(), StringComparison.Ordinal);
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

  private static DataVaultLiveSchemaSnapshot CreateLiveSchema(
      IReadOnlyList<DataVaultIdempotencyPreflightStructure> expectedStructures) {
    return new DataVaultLiveSchemaSnapshot(expectedStructures
        .GroupBy(structure => structure.TableName, StringComparer.Ordinal)
        .Select(group => {
          var primaryKey = group.Single(structure => structure.Kind == "primary-key");
          var indexes = group
              .Where(structure => structure.Kind == "secondary-index")
              .Select(structure => new DataVaultLiveSchemaIndex(
                  structure.Name,
                  structure.ColumnNames,
                  structure.IsUnique,
                  structure.DescendingColumnNames,
                  structure.IncludedColumnNames));
          var columns = group
              .SelectMany(structure => structure.ColumnNames.Concat(structure.IncludedColumnNames))
              .Distinct(StringComparer.Ordinal)
              .Select((columnName, ordinal) => new DataVaultLiveSchemaColumn(columnName, ordinal, "TEXT"));

          return new DataVaultLiveSchemaTable(
              group.Key,
              columns,
              new DataVaultLiveSchemaPrimaryKey(primaryKey.Name, primaryKey.ColumnNames),
              indexes);
        }));
  }

  private static string MutateHashKeyStorageMigrationManifest(Action<JsonObject> mutate) {
    var root = JsonNode.Parse(CreateValidHashKeyStorageMigrationManifestJson())!.AsObject();
    mutate(root);
    return root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
  }

  private static string CreateValidHashKeyStorageMigrationManifestJson() {
    return """
        {
          "schemaVersion": "dvault.hash-key-storage-migration.v1",
          "dryRun": {
            "enabled": true,
            "status": "compatible-review-only",
            "databaseMutation": "none",
            "migrationApplication": "not-run",
            "publicHashKeyBoundary": "lowercase-hex-no-prefix",
            "targetDiagnosticsSourceKind": "unit-test"
          },
          "source": {
            "metadataSourceKind": "model-metadata",
            "metadataSourceFingerprint": "source-fingerprint",
            "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
            "capabilityProfile": "sqlite-v1",
            "capabilityProfileDefaulted": false
          },
          "target": {
            "metadataSourceKind": "model-metadata",
            "metadataSourceFingerprint": "source-fingerprint",
            "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
            "capabilityProfile": "sqlite-v1",
            "capabilityProfileDefaulted": false
          },
          "comparison": {
            "intendedChange": "HexString-to-Binary",
            "compatibilityStatus": "compatible-storage-profile-flip",
            "entryCount": 1,
            "hashKeyColumnCount": 1,
            "participantReferenceColumnCount": 0,
            "ordering": "ordinal by tableName then propertyName"
          },
          "entries": [
            {
              "ordinal": 0,
              "tableName": "HubCustomer",
              "tableKind": "Hub",
              "entityMetadataName": "Customer",
              "propertyName": "CustomerHashKey",
              "propertyRole": "HashKey",
              "technicalRole": "HashKey",
              "logicalPropertyKind": "HashKey",
              "propertyMetadataName": "CustomerHashKey",
              "source": {
                "storageProfile": "HexString",
                "providerStoreType": "TEXT",
                "providerValueFormat": "LowercaseHexText",
                "efClrModelType": "System.String",
                "conversionBehavior": "none-string-model",
                "algorithmId": "sha256-v1",
                "digestByteLength": 32,
                "digestEncoding": "lowercase-hex-no-prefix"
              },
              "target": {
                "storageProfile": "Binary",
                "providerStoreType": "BLOB",
                "providerValueFormat": "LowercaseHexBinary",
                "efClrModelType": "System.String",
                "conversionBehavior": "lowercase-hex-string-to-bytes",
                "algorithmId": "sha256-v1",
                "digestByteLength": 32,
                "digestEncoding": "lowercase-hex-no-prefix"
              }
            }
          ]
        }
        """;
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
