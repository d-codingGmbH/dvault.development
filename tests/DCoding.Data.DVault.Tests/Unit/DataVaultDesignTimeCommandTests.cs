using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultDesignTimeCommandTests {
  [Fact]
  public void RunPrintsHelpAndReturnsUsageErrorsDeterministically() {
    var host = CreateHost();

    var help = Run(host, "--help");
    var unknown = Run(host, "missing");
    var missingArtifact = Run(host, "drift");

    Assert.Equal(0, help.ExitCode);
    Assert.Contains("Usage: dvault validate", help.Output, StringComparison.Ordinal);
    Assert.Contains("dvault support-bundle", help.Output, StringComparison.Ordinal);
    Assert.Empty(help.Error);

    Assert.Equal(2, unknown.ExitCode);
    Assert.Contains("Unknown DVault command 'missing'.", unknown.Error, StringComparison.Ordinal);
    Assert.Contains("Usage: dvault validate", unknown.Error, StringComparison.Ordinal);

    Assert.Equal(2, missingArtifact.ExitCode);
    Assert.Contains("Missing artifact path for drift command.", missingArtifact.Error, StringComparison.Ordinal);
  }

  [Fact]
  public void ValidateReturnsSuccessAndFailureCodesFromDiagnosticsResult() {
    var validHost = CreateHost(diagnostics: new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)));
    var invalidHost = CreateHost(diagnostics: new StubDiagnosticsService(CreateDiagnosticsResult(isValid: false)));

    var valid = Run(validHost, "validate");
    var invalid = Run(invalidHost, "validate");

    Assert.Equal(0, valid.ExitCode);
    Assert.Contains("DVault diagnostics: valid", valid.Output, StringComparison.Ordinal);
    Assert.Empty(valid.Error);

    Assert.Equal(1, invalid.ExitCode);
    Assert.Contains("DVault diagnostics: invalid", invalid.Output, StringComparison.Ordinal);
    Assert.Contains("command-test-invalid", invalid.Output, StringComparison.Ordinal);
    Assert.Empty(invalid.Error);
  }

  [Fact]
  public void ExportEmitsCanonicalJsonAndReportsExporterFailures() {
    var successHost = CreateHost(
        exportSource: DataVaultDesignTimeExportSource.FromMetadataModel(CreateCustomerMetadataModel()));
    var failureHost = CreateHost(
        exportSource: DataVaultDesignTimeExportSource.FromMetadataModel(CreateLegacyPointInTimeMetadataModel()));

    var success = Run(successHost, "export");
    var failure = Run(failureHost, "export");

    Assert.Equal(0, success.ExitCode);
    Assert.Contains("\"schemaVersion\": \"dvault.model.v1\"", success.Output, StringComparison.Ordinal);
    Assert.Contains("\"hubs\"", success.Output, StringComparison.Ordinal);
    Assert.Empty(success.Error);

    Assert.Equal(1, failure.ExitCode);
    Assert.Empty(failure.Output);
    Assert.Contains("DVault export failed:", failure.Error, StringComparison.Ordinal);
    Assert.Contains("Legacy PointInTimeTables metadata is not serializable", failure.Error, StringComparison.Ordinal);
  }

  [Fact]
  public void SupportBundleExportsDeterministicRedactedDiagnosticsAndPreservesRequestBoundStrategies() {
    var diagnostics = CreateSupportBundleDiagnosticsResult();
    var host = CreateHost(createSupportBundleDiagnostics: _ => diagnostics);

    var first = Run(host, "support-bundle");
    var second = Run(host, "support-bundle");

    Assert.Equal(0, first.ExitCode);
    Assert.Equal(first.Output, second.Output);
    Assert.Empty(first.Error);
    Assert.Contains("\"schemaVersion\": \"dvault.support-bundle.v1\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"metadataSourceKind\": \"command-test\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"saveStrategy\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"selectedStrategyName\": \"UnitSaveStrategy\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"readStrategy\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"selectedStrategyName\": \"UnitReadStrategy\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"satelliteSnapshotReferenceStoreType\": \"TEXT\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"typeMappings\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"maximumIdentifierLength\": 64", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"unsupportedIncludedIndexColumnMode\": \"Ignore\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"supportedProviderNames\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"minimumTotalOperationCount\": 50", first.Output, StringComparison.Ordinal);
    Assert.Contains("\"gateRequirements\"", first.Output, StringComparison.Ordinal);
    Assert.Contains("Password=<redacted>", first.Output, StringComparison.Ordinal);
    Assert.Contains("User Id=<redacted>", first.Output, StringComparison.Ordinal);
    Assert.DoesNotContain("hunter2", first.Output, StringComparison.Ordinal);
    Assert.DoesNotContain("admin", first.Output, StringComparison.Ordinal);
    Assert.DoesNotContain("\"liveSchema\"", first.Output, StringComparison.Ordinal);
    Assert.DoesNotContain("\"drift\"", first.Output, StringComparison.Ordinal);
  }

  [Fact]
  public void SupportBundleWritesOutputPathAndIncludesOptInLiveSchemaAndDrift() {
    var artifactJson = DataVaultModelArtifactExporter.ExportJson(CreateCustomerMetadataModel());
    var artifactPath = WriteArtifactFile(artifactJson);
    var bundlePath = Path.Combine(
        Path.GetTempPath(),
        "dvault-command-" + Guid.NewGuid().ToString("N") + ".support-bundle.json");
    var host = CreateHost(
        contextModel: CreateCustomerMetadataModel(),
        liveSchemaReader: new StubLiveSchemaReader(
            DataVaultLiveSchemaReadResult.Unavailable(
                "Unit.Provider",
                "Password=server-secret;Uid=dbadmin;Host=prod")));

    try {
      var result = Run(
          host,
          "support-bundle",
          "--artifact",
          artifactPath,
          "--live-schema",
          "--output",
          bundlePath);
      var bundleJson = File.ReadAllText(bundlePath);

      Assert.Equal(0, result.ExitCode);
      Assert.Contains("Exported DVault support bundle", result.Output, StringComparison.Ordinal);
      Assert.Empty(result.Error);
      Assert.Contains("\"liveSchema\"", bundleJson, StringComparison.Ordinal);
      Assert.Contains("\"drift\"", bundleJson, StringComparison.Ordinal);
      Assert.Contains("Unit.Provider", bundleJson, StringComparison.Ordinal);
      Assert.Contains("Password=<redacted>", bundleJson, StringComparison.Ordinal);
      Assert.Contains("Uid=<redacted>", bundleJson, StringComparison.Ordinal);
      Assert.DoesNotContain("server-secret", bundleJson, StringComparison.Ordinal);
      Assert.DoesNotContain("dbadmin", bundleJson, StringComparison.Ordinal);
    }
    finally {
      File.Delete(artifactPath);
      File.Delete(bundlePath);
    }
  }

  [Fact]
  public void DriftComparesArtifactAgainstDesignTimeModelByDefault() {
    var artifactJson = DataVaultModelArtifactExporter.ExportJson(CreateCustomerMetadataModel());
    var artifactPath = WriteArtifactFile(artifactJson);

    try {
      var matchingHost = CreateHost(contextModel: CreateCustomerMetadataModel());
      var driftedHost = CreateHost(contextModel: CreateHubOnlyMetadataModel());

      var matching = Run(matchingHost, "drift", "--artifact", artifactPath);
      var drifted = Run(driftedHost, "drift", artifactPath);

      Assert.Equal(0, matching.ExitCode);
      Assert.Contains("DVault model drift:", matching.Output, StringComparison.Ordinal);
      Assert.Contains("0 blocking", matching.Output, StringComparison.Ordinal);
      Assert.Contains("metadata-source-kind-mismatch", matching.Output, StringComparison.Ordinal);
      Assert.Empty(matching.Error);

      Assert.Equal(1, drifted.ExitCode);
      Assert.Contains("missing-entity", drifted.Output, StringComparison.Ordinal);
      Assert.Contains("Satellite:CustomerProfile", drifted.Output, StringComparison.Ordinal);
      Assert.Empty(drifted.Error);
    }
    finally {
      File.Delete(artifactPath);
    }
  }

  [Fact]
  public void DriftLiveSchemaLaneUsesClassifiedReaderOutcome() {
    var artifactJson = DataVaultModelArtifactExporter.ExportJson(CreateCustomerMetadataModel());
    var artifactPath = WriteArtifactFile(artifactJson);
    var host = CreateHost(
        contextModel: CreateCustomerMetadataModel(),
        liveSchemaReader: new StubLiveSchemaReader(
            DataVaultLiveSchemaReadResult.UnsupportedProvider("Unit.Provider")));

    try {
      var result = Run(host, "drift", "--artifact", artifactPath, "--live-schema");

      Assert.Equal(1, result.ExitCode);
      Assert.Contains("live-schema-provider-unsupported", result.Output, StringComparison.Ordinal);
      Assert.Contains("Unit.Provider", result.Output, StringComparison.Ordinal);
      Assert.Empty(result.Error);
    }
    finally {
      File.Delete(artifactPath);
    }
  }

  [Fact]
  public void GuardrailEvaluatesResolvedMigrationOperations() {
    using var provider = CreateServiceProvider();
    var host = CreateHost(
        contextModel: CreateCustomerMetadataModel(),
        diagnostics: provider.GetRequiredService<IDataVaultDiagnosticsService>(),
        resolveMigrationOperations: migrationName => migrationName switch {
          "Safe" => Array.Empty<MigrationOperation>(),
          "DropCustomer" => [new DropTableOperation { Name = "HubCustomer" }],
          _ => throw new InvalidOperationException("Unknown migration '" + migrationName + "'."),
        });

    var safe = Run(host, "guardrail", "--migration", "Safe");
    var unsafeResult = Run(host, "guardrail", "DropCustomer");

    Assert.Equal(0, safe.ExitCode);
    Assert.Contains("DVault migration guardrails: valid, findings 0", safe.Output, StringComparison.Ordinal);
    Assert.Empty(safe.Error);

    Assert.Equal(1, unsafeResult.ExitCode);
    Assert.Contains("DVault migration guardrails: invalid, findings 1", unsafeResult.Output, StringComparison.Ordinal);
    Assert.Contains("DVM2006", unsafeResult.Output, StringComparison.Ordinal);
    Assert.Empty(unsafeResult.Error);
  }

  private static CommandRunResult Run(
      DataVaultDesignTimeCommandHost host,
      params string[] args) {
    using var output = new StringWriter();
    using var error = new StringWriter();

    var exitCode = DataVaultDesignTimeCommand.Run(args, output, error, host);

    return new CommandRunResult(exitCode, output.ToString(), error.ToString());
  }

  private static DataVaultDesignTimeCommandHost CreateHost(
      DataVaultMetadataModel? contextModel = null,
      IDataVaultDiagnosticsService? diagnostics = null,
      DataVaultDesignTimeExportSource? exportSource = null,
      Func<string, IEnumerable<MigrationOperation>>? resolveMigrationOperations = null,
      Func<DbContext, DataVaultDiagnosticsResult>? createSupportBundleDiagnostics = null,
      IDataVaultLiveSchemaReader? liveSchemaReader = null) {
    var selectedContextModel = contextModel ?? CreateCustomerMetadataModel();

    return new DataVaultDesignTimeCommandHost(
        diagnostics ?? new StubDiagnosticsService(CreateDiagnosticsResult(isValid: true)),
        () => CreateContext(selectedContextModel),
        exportSource ?? DataVaultDesignTimeExportSource.FromMetadataModel(selectedContextModel),
        resolveMigrationOperations ?? (_ => Array.Empty<MigrationOperation>())) {
      CreateSupportBundleDiagnostics = createSupportBundleDiagnostics,
      LiveSchemaReader = liveSchemaReader,
    };
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVault();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static DesignTimeCommandContext CreateContext(DataVaultMetadataModel metadataModel) {
    var optionsBuilder = new DbContextOptionsBuilder<DesignTimeCommandContext>()
        .UseSqlite("Data Source=:memory:");
    optionsBuilder.UseDataVaultMetadata(metadataModel);

    return new DesignTimeCommandContext(optionsBuilder.Options);
  }

  private static string WriteArtifactFile(string json) {
    var path = Path.Combine(
        Path.GetTempPath(),
        "dvault-command-" + Guid.NewGuid().ToString("N") + ".model.json");
    File.WriteAllText(path, json);

    return path;
  }

  private static DataVaultMetadataModel CreateCustomerMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var profile = new DataVaultSatelliteMetadata(
        "CustomerProfile",
        customer.ToReference(),
        ["Name"]);

    return new DataVaultMetadataModel([customer], [], [profile]);
  }

  private static DataVaultMetadataModel CreateHubOnlyMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        []);
  }

  private static DataVaultMetadataModel CreateLegacyPointInTimeMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var profile = new DataVaultSatelliteMetadata(
        "CustomerProfile",
        customer.ToReference(),
        ["Name"]);
    var legacyPointInTimeTable = new DataVaultPointInTimeMetadata(
        "CustomerPointInTime",
        customer.ToReference(),
        [DataVaultMetadataReference.Satellite("CustomerProfile")]);

    return new DataVaultMetadataModel([customer], [], [profile], [legacyPointInTimeTable]);
  }

  private static DataVaultDiagnosticsResult CreateDiagnosticsResult(bool isValid) {
    var issue = new DataVaultDiagnosticsIssue(
        DataVaultDiagnosticsIssueSeverity.Error,
        "command-test-invalid",
        "The command test diagnostics result is invalid.",
        "command-test");
    var issues = isValid
        ? Array.Empty<DataVaultDiagnosticsIssue>()
        : new[] { issue };
    var validationIssues = isValid
        ? Array.Empty<DataVaultDiagnosticsIssue>()
        : new[] { issue };

    return new DataVaultDiagnosticsResult(
        new DataVaultValidationDiagnostics(isValid, validationIssues),
        new DataVaultExplainDiagnostics(
            "command-test",
            null,
            "Unit.Provider",
            DataVaultProviderCapabilityProfiles.Sqlite.ProfileName,
            false,
            DataVaultProviderValueFormat.Text,
            "TEXT",
            DataVaultProviderBehaviorProfiles.ProviderNeutral.ProfileName,
            false,
            Array.Empty<DataVaultEntityExplain>()) {
          SatelliteSnapshotReferenceValueFormat = DataVaultProviderValueFormat.Iso8601UtcText,
          SatelliteSnapshotReferenceStoreType = "TEXT",
          TypeMappings = [
            new DataVaultProviderTypeMappingExplain(
                DataVaultLogicalPropertyKind.LoadTimestamp,
                typeof(DateTimeOffset).FullName!,
                "TEXT",
                DataVaultProviderValueFormat.Iso8601UtcText),
          ],
          MaximumIdentifierLength = 64,
          AllowsIndexesCoveredByPrimaryKey = true,
          UnsupportedIncludedIndexColumnMode = DataVaultUnsupportedIncludedIndexColumnMode.Ignore,
          SqlFunctionSupport = DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
          ConcurrencySupport = DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
        },
        new DataVaultSaveStrategyDiagnostics(
            DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated,
            ProviderName: "Unit.Provider",
            SelectedStrategyName: null,
            SelectedStrategyPriority: null,
            Candidates: Array.Empty<DataVaultSaveStrategyCandidateDiagnostics>(),
            FallbackCauses: Array.Empty<DataVaultSaveStrategyFallbackCause>()),
        issues);
  }

  private static DataVaultDiagnosticsResult CreateSupportBundleDiagnosticsResult() {
    return CreateDiagnosticsResult(isValid: true) with {
      SaveStrategy = new DataVaultSaveStrategyDiagnostics(
          DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
          ProviderName: "Unit.Provider",
          SelectedStrategyName: "UnitSaveStrategy",
          SelectedStrategyPriority: 10,
          Candidates: [
            new DataVaultSaveStrategyCandidateDiagnostics(
                0,
                "UnitSaveStrategy",
                10,
                true,
                Array.Empty<DataVaultSaveStrategyFallbackCause>()) {
              SupportedProviderNames = ["Unit.Provider"],
              GateRequirements = [
                new DataVaultSaveStrategyGateRequirement(
                    DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
                    MinimumTotalOperationCount: 50),
              ],
            },
            new DataVaultSaveStrategyCandidateDiagnostics(
                1,
                "ProviderNeutralSaveStrategy",
                0,
                false,
                [
                  new DataVaultSaveStrategyFallbackCause(
                      DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
                      "Provider text contained Password=hunter2;User Id=admin."),
                ]) {
              SupportedProviderNames = [],
              GateRequirements = [],
            },
          ],
          FallbackCauses: Array.Empty<DataVaultSaveStrategyFallbackCause>()),
      ReadStrategy = new DataVaultReadStrategyDiagnostics(
          DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
          ProviderName: "Unit.Provider",
          SelectedStrategyName: "UnitReadStrategy",
          SelectedStrategyPriority: 20,
          Candidates: [
            new DataVaultReadStrategyCandidateDiagnostics(
                0,
                "UnitReadStrategy",
                20,
                true,
                Array.Empty<DataVaultReadStrategyFallbackCause>()) {
              SupportedProviderNames = ["Unit.Provider"],
              GateRequirements = [
                new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
              ],
            },
          ],
          FallbackCauses: Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };
  }

  private sealed class DesignTimeCommandContext(DbContextOptions<DesignTimeCommandContext> options) : DbContext(options) {
  }

  private sealed class StubLiveSchemaReader(DataVaultLiveSchemaReadResult result) : IDataVaultLiveSchemaReader {
    public Task<DataVaultLiveSchemaReadResult> ReadAsync(
        DbContext dbContext,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(dbContext);

      return Task.FromResult(result);
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

  private sealed record CommandRunResult(int ExitCode, string Output, string Error);
}
