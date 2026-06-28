using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
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
    Assert.Contains("dvault sql-artifact", help.Output, StringComparison.Ordinal);
    Assert.Contains("dvault hash-key-storage-migration", help.Output, StringComparison.Ordinal);
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
    Assert.Contains("\"minimumTotalOperationCount\": 100", first.Output, StringComparison.Ordinal);
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
  public void SqlArtifactWritesDeterministicDryRunManifestForSqlServerWorkload() {
    var diagnostics = CreateSqlServerSqlArtifactDiagnosticsResult();
    var host = CreateHost(createSupportBundleDiagnostics: _ => diagnostics);
    var firstPath = WriteSqlArtifactManifestFilePath();
    var secondPath = WriteSqlArtifactManifestFilePath();

    try {
      var first = Run(host, "sql-artifact", "--output", firstPath);
      var second = Run(
          host,
          "sql-artifact",
          "--workload",
          "provider-native-bulk-ingestion",
          "--output",
          secondPath);
      var firstJson = File.ReadAllText(firstPath);
      var secondJson = File.ReadAllText(secondPath);

      Assert.Equal(0, first.ExitCode);
      Assert.Equal(0, second.ExitCode);
      Assert.Empty(first.Error);
      Assert.Empty(second.Error);
      Assert.Contains("Exported DVault SQL artifact dry-run manifest", first.Output, StringComparison.Ordinal);
      Assert.Equal(firstJson, secondJson);

      using var document = JsonDocument.Parse(firstJson);
      var root = document.RootElement;
      Assert.Equal("dvault.sql-artifact.v1", root.GetProperty("schemaVersion").GetString());
      Assert.True(root.GetProperty("dryRun").GetProperty("enabled").GetBoolean());
      Assert.Equal("review-only", root.GetProperty("dryRun").GetProperty("status").GetString());
      Assert.Equal("not-generated", root.GetProperty("dryRun").GetProperty("runtimeDispatch").GetString());
      Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", root.GetProperty("provider").GetProperty("name").GetString());
      Assert.Equal("SQL Server external provider", root.GetProperty("provider").GetProperty("externalProviderLabel").GetString());
      Assert.Equal("sqlserver-v1", root.GetProperty("provider").GetProperty("capabilityProfile").GetString());
      Assert.Equal("SqlServerDataVaultSaveStrategy", root.GetProperty("provider").GetProperty("selectedStrategy").GetString());
      Assert.Equal("command-test", root.GetProperty("metadataSource").GetProperty("kind").GetString());
      Assert.Equal("fingerprint-1", root.GetProperty("metadataSource").GetProperty("fingerprint").GetString());

      var workload = root.GetProperty("workload");
      Assert.Equal("provider-native-bulk-ingestion", workload.GetProperty("label").GetString());
      Assert.Equal(300, workload.GetProperty("orderProductPairCount").GetInt32());
      Assert.Equal(600, workload.GetProperty("hubOperationCount").GetInt32());
      Assert.Equal(300, workload.GetProperty("orderProductLinkCount").GetInt32());
      Assert.Equal(300, workload.GetProperty("linkOperationCount").GetInt32());
      Assert.Equal(3, workload.GetProperty("fulfillmentSatelliteOperationCount").GetInt32());
      Assert.Equal(3, workload.GetProperty("satelliteOperationCount").GetInt32());
      Assert.Equal(1, workload.GetProperty("unchangedReplayCount").GetInt32());
      Assert.Equal(903, workload.GetProperty("totalOperationCount").GetInt32());
      Assert.Equal("SqlBulkCopy", workload.GetProperty("transfer").GetString());
      Assert.Equal("100-plus-operations; mixedBatchBoundary=900-plus-operations", workload.GetProperty("nativeBulkBoundary").GetString());
      Assert.Equal("temporary-staging-table", workload.GetProperty("cleanupBoundary").GetString());

      var evidence = root.GetProperty("evidence");
      var artifactTriplet = evidence
          .GetProperty("benchmarkArtifactTriplet")
          .EnumerateArray()
          .Select(item => item.GetString() ?? string.Empty)
          .ToArray();
      Assert.Equal(["benchmark-summary.md", "benchmark-summary.csv", "benchmark-summary.json"], artifactTriplet);
      var benchmarkRows = evidence.GetProperty("benchmarkRows").EnumerateArray().ToArray();
      Assert.Equal(2, benchmarkRows.Length);
      Assert.Contains(
          benchmarkRows,
          row => row.GetProperty("baseline").GetString() == "dvault-adddvault-fallback" &&
              row.GetProperty("role").GetString() == "provider-neutral-fallback");
      Assert.Contains(
          benchmarkRows,
          row => row.GetProperty("baseline").GetString() == "dvault-adddvaultsqlserver-optimized" &&
              row.GetProperty("role").GetString() == "sqlserver-optimized-dry-run-reference");

      Assert.Equal("create-support-bundle-diagnostics", root.GetProperty("requestDiagnostics").GetProperty("sourceKind").GetString());
      Assert.Equal("ProviderStrategySelected", root.GetProperty("requestDiagnostics").GetProperty("saveStrategyStatus").GetString());
      Assert.Empty(root.GetProperty("sidecarPayloads").EnumerateArray());
      Assert.Empty(root.GetProperty("entries")[0].GetProperty("payloadFiles").EnumerateArray());
      Assert.DoesNotContain("hunter2", firstJson, StringComparison.Ordinal);
      Assert.DoesNotContain("DVAULT_TEST_SQLSERVER_CONNECTION_STRING", firstJson, StringComparison.Ordinal);
    }
    finally {
      File.Delete(firstPath);
      File.Delete(secondPath);
    }
  }

  [Fact]
  public void SqlArtifactRejectsMissingOutputUnsupportedWorkloadAndNonSqlServerDiagnostics() {
    var host = CreateHost(createSupportBundleDiagnostics: _ => CreateSqlServerSqlArtifactDiagnosticsResult());
    var missingOutput = Run(host, "sql-artifact");

    Assert.Equal(2, missingOutput.ExitCode);
    Assert.Contains("Missing output path for sql-artifact command.", missingOutput.Error, StringComparison.Ordinal);

    var unsupportedWorkload = Run(
        host,
        "sql-artifact",
        "--workload",
        "customer-profile-history",
        "--output",
        WriteSqlArtifactManifestFilePath());
    Assert.Equal(2, unsupportedWorkload.ExitCode);
    Assert.Contains("Unsupported SQL artifact workload 'customer-profile-history'.", unsupportedWorkload.Error, StringComparison.Ordinal);

    var wrongProviderPath = WriteSqlArtifactManifestFilePath();
    var wrongProviderHost = CreateHost(createSupportBundleDiagnostics: _ => CreateSqlServerSqlArtifactDiagnosticsResult(
        providerName: "Microsoft.EntityFrameworkCore.Sqlite",
        capabilityProfileName: DataVaultProviderCapabilityProfiles.Sqlite.ProfileName,
        selectedStrategyName: "SqliteDataVaultSaveStrategy"));

    try {
      var wrongProvider = Run(wrongProviderHost, "sql-artifact", "--output", wrongProviderPath);

      Assert.Equal(1, wrongProvider.ExitCode);
      Assert.Contains("DVault sql-artifact failed:", wrongProvider.Error, StringComparison.Ordinal);
      Assert.Contains(
          "requires explain diagnostics for provider 'Microsoft.EntityFrameworkCore.SqlServer'",
          wrongProvider.Error,
          StringComparison.Ordinal);
      Assert.False(File.Exists(wrongProviderPath));
    }
    finally {
      File.Delete(wrongProviderPath);
    }
  }

  [Fact]
  public void HashKeyStorageMigrationWritesDeterministicDryRunManifestForHexToBinaryPreflight() {
    var metadataModel = CreateHashKeyStorageMigrationCoverageMetadataModel();
    var sourceDiagnostics = CreateHashKeyStorageMigrationDiagnostics(
        metadataModel,
        DataVaultHashKeyStorageProfile.HexString);
    var targetDiagnostics = CreateHashKeyStorageMigrationDiagnostics(
        metadataModel,
        DataVaultHashKeyStorageProfile.Binary);
    var sourcePath = WriteSupportBundleFile(DataVaultSupportBundleExporter.ExportJson(sourceDiagnostics));
    var firstPath = WriteHashKeyStorageMigrationManifestFilePath();
    var secondPath = WriteHashKeyStorageMigrationManifestFilePath();
    var migrationResolverInvoked = false;
    var host = CreateHost(
        contextModel: metadataModel,
        resolveMigrationOperations: _ => {
          migrationResolverInvoked = true;
          return Array.Empty<MigrationOperation>();
        },
        createSupportBundleDiagnostics: _ => targetDiagnostics);

    try {
      var first = Run(
          host,
          "hash-key-storage-migration",
          "--source",
          sourcePath,
          "--output",
          firstPath);
      var second = Run(
          host,
          "hash-key-storage-migration",
          "-s",
          sourcePath,
          "-o",
          secondPath);
      var firstJson = File.ReadAllText(firstPath);
      var secondJson = File.ReadAllText(secondPath);

      Assert.Equal(0, first.ExitCode);
      Assert.Equal(0, second.ExitCode);
      Assert.Empty(first.Error);
      Assert.Empty(second.Error);
      Assert.Contains("Exported DVault hash-key storage migration dry-run manifest", first.Output, StringComparison.Ordinal);
      Assert.Equal(firstJson, secondJson);
      Assert.False(migrationResolverInvoked);

      using var document = JsonDocument.Parse(firstJson);
      var root = document.RootElement;
      Assert.Equal("dvault.hash-key-storage-migration.v1", root.GetProperty("schemaVersion").GetString());
      Assert.True(root.GetProperty("dryRun").GetProperty("enabled").GetBoolean());
      Assert.Equal("compatible-review-only", root.GetProperty("dryRun").GetProperty("status").GetString());
      Assert.Equal("none", root.GetProperty("dryRun").GetProperty("databaseMutation").GetString());
      Assert.Equal("not-run", root.GetProperty("dryRun").GetProperty("migrationApplication").GetString());
      Assert.Equal("lowercase-hex-no-prefix", root.GetProperty("dryRun").GetProperty("publicHashKeyBoundary").GetString());
      Assert.Equal("create-support-bundle-diagnostics", root.GetProperty("dryRun").GetProperty("targetDiagnosticsSourceKind").GetString());
      Assert.Equal("model-metadata", root.GetProperty("source").GetProperty("metadataSourceKind").GetString());
      Assert.Equal("sqlite-v1", root.GetProperty("source").GetProperty("capabilityProfile").GetString());
      Assert.Equal("sqlite-v1", root.GetProperty("target").GetProperty("capabilityProfile").GetString());

      var comparison = root.GetProperty("comparison");
      Assert.Equal("HexString-to-Binary", comparison.GetProperty("intendedChange").GetString());
      Assert.Equal("compatible-storage-profile-flip", comparison.GetProperty("compatibilityStatus").GetString());
      Assert.Equal(16, comparison.GetProperty("entryCount").GetInt32());
      Assert.Equal(8, comparison.GetProperty("hashKeyColumnCount").GetInt32());
      Assert.Equal(8, comparison.GetProperty("participantReferenceColumnCount").GetInt32());

      var entries = root.GetProperty("entries").EnumerateArray().ToArray();
      Assert.Equal(Enumerable.Range(0, entries.Length), entries.Select(entry => entry.GetProperty("ordinal").GetInt32()));
      Assert.Contains(entries, entry => entry.GetProperty("tableKind").GetString() == "Hub");
      Assert.Contains(entries, entry => entry.GetProperty("tableKind").GetString() == "Link");
      Assert.Contains(entries, entry => entry.GetProperty("tableKind").GetString() == "Satellite");
      Assert.Contains(entries, entry => entry.GetProperty("tableKind").GetString() == "Pit");
      Assert.Contains(entries, entry => entry.GetProperty("tableKind").GetString() == "Bridge");

      Assert.Contains(entries, entry =>
          entry.GetProperty("tableName").GetString() == "HubCustomer" &&
          entry.GetProperty("propertyName").GetString() == "CustomerHashKey" &&
          entry.GetProperty("logicalPropertyKind").GetString() == "HashKey");
      Assert.Contains(entries, entry =>
          entry.GetProperty("tableName").GetString() == "LinkCustomerOrder" &&
          entry.GetProperty("propertyName").GetString() == "OrderHashKey" &&
          entry.GetProperty("logicalPropertyKind").GetString() == "ParticipantReference");
      Assert.Contains(entries, entry =>
          entry.GetProperty("tableName").GetString() == "PitCustomerProfileStatus" &&
          entry.GetProperty("propertyName").GetString() == "CustomerHashKey" &&
          entry.GetProperty("logicalPropertyKind").GetString() == "HashKey");
      Assert.Contains(entries, entry =>
          entry.GetProperty("tableName").GetString() == "BridgeCustomerOrder" &&
          entry.GetProperty("propertyName").GetString() == "OrderHashKey" &&
          entry.GetProperty("logicalPropertyKind").GetString() == "ParticipantReference");

      foreach (var entry in entries) {
        var source = entry.GetProperty("source");
        var target = entry.GetProperty("target");
        Assert.Equal("HexString", source.GetProperty("storageProfile").GetString());
        Assert.Equal("Binary", target.GetProperty("storageProfile").GetString());
        Assert.Equal("LowercaseHexText", source.GetProperty("providerValueFormat").GetString());
        Assert.Equal("LowercaseHexBinary", target.GetProperty("providerValueFormat").GetString());
        Assert.Equal("System.String", source.GetProperty("efClrModelType").GetString());
        Assert.Equal("System.String", target.GetProperty("efClrModelType").GetString());
        Assert.Equal("none-string-model", source.GetProperty("conversionBehavior").GetString());
        Assert.Equal("lowercase-hex-string-to-bytes", target.GetProperty("conversionBehavior").GetString());
        Assert.Equal("sha256-v1", source.GetProperty("algorithmId").GetString());
        Assert.Equal("sha256-v1", target.GetProperty("algorithmId").GetString());
        Assert.Equal(32, source.GetProperty("digestByteLength").GetInt32());
        Assert.Equal(32, target.GetProperty("digestByteLength").GetInt32());
        Assert.Equal("lowercase-hex-no-prefix", source.GetProperty("digestEncoding").GetString());
        Assert.Equal("lowercase-hex-no-prefix", target.GetProperty("digestEncoding").GetString());
      }

      var validation = DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(firstJson);
      Assert.True(validation.IsValid, validation.ToDisplayString());
      Assert.Contains(validation.Findings, finding =>
          finding.Severity == DataVaultDiagnosticsIssueSeverity.Info &&
          finding.Code == "hash-key-migration-manifest-compatible");
    }
    finally {
      File.Delete(sourcePath);
      File.Delete(firstPath);
      File.Delete(secondPath);
    }
  }

  [Fact]
  public void HashKeyStorageMigrationFailsClosedForAlgorithmAndDigestDrift() {
    var metadataModel = CreateCustomerMetadataModel();
    var sourceDiagnostics = CreateHashKeyStorageMigrationDiagnostics(
        metadataModel,
        DataVaultHashKeyStorageProfile.HexString);
    var targetDiagnostics = CreateHashKeyStorageMigrationDiagnostics(
        metadataModel,
        DataVaultHashKeyStorageProfile.Binary,
        stableHashAlgorithmId: "sha256-128-v1");
    var sourcePath = WriteSupportBundleFile(DataVaultSupportBundleExporter.ExportJson(sourceDiagnostics));
    var outputPath = WriteHashKeyStorageMigrationManifestFilePath();
    var host = CreateHost(
        contextModel: metadataModel,
        createSupportBundleDiagnostics: _ => targetDiagnostics);

    try {
      var result = Run(
          host,
          "hash-key-storage-migration",
          "--source",
          sourcePath,
          "--output",
          outputPath);

      Assert.Equal(1, result.ExitCode);
      Assert.Empty(result.Output);
      Assert.Contains("DVault hash-key-storage-migration failed:", result.Error, StringComparison.Ordinal);
      Assert.Contains("changed algorithmId", result.Error, StringComparison.Ordinal);
      Assert.Contains("changed digestByteLength", result.Error, StringComparison.Ordinal);
      Assert.False(File.Exists(outputPath));
    }
    finally {
      File.Delete(sourcePath);
      File.Delete(outputPath);
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
    return CreateServiceProvider(stableHashAlgorithmId: null);
  }

  private static ServiceProvider CreateServiceProvider(string? stableHashAlgorithmId) {
    var services = new ServiceCollection();
    if (stableHashAlgorithmId is null) {
      services.AddDVault();
    }
    else {
      services.AddDVault(options => options.UseStableHashAlgorithm(stableHashAlgorithmId));
    }

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

  private static string WriteSqlArtifactManifestFilePath() {
    return Path.Combine(
        Path.GetTempPath(),
        "dvault-command-" + Guid.NewGuid().ToString("N") + ".sql-artifact.json");
  }

  private static string WriteSupportBundleFile(string json) {
    var path = Path.Combine(
        Path.GetTempPath(),
        "dvault-command-" + Guid.NewGuid().ToString("N") + ".support-bundle.json");
    File.WriteAllText(path, json);

    return path;
  }

  private static string WriteHashKeyStorageMigrationManifestFilePath() {
    return Path.Combine(
        Path.GetTempPath(),
        "dvault-command-" + Guid.NewGuid().ToString("N") + ".hash-key-storage-migration.json");
  }

  private static DataVaultMetadataModel CreateCustomerMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var profile = new DataVaultSatelliteMetadata(
        "CustomerProfile",
        customer.ToReference(),
        ["Name"]);

    return new DataVaultMetadataModel([customer], [], [profile]);
  }

  private static DataVaultMetadataModel CreateHashKeyStorageMigrationCoverageMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var salesRegion = new DataVaultHubMetadata("SalesRegion", ["RegionCode"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var salesRegionParentChild = new DataVaultLinkMetadata(
        "SalesRegionParentChild",
        [
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ParentRegion"),
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ChildRegion"),
        ]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());
    var hierarchyBridge = new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                salesRegion.ToReference(),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Descendant,
                salesRegion.ToReference(),
                "ChildRegion"),
        ]);

    return new DataVaultMetadataModel(
        [customer, order, salesRegion],
        [customerOrder, salesRegionParentChild],
        [profile, status],
        Array.Empty<DataVaultPointInTimeMetadata>(),
        [bridge, hierarchyBridge],
        [pit]);
  }

  private static DataVaultDiagnosticsResult CreateHashKeyStorageMigrationDiagnostics(
      DataVaultMetadataModel metadataModel,
      DataVaultHashKeyStorageProfile storageProfile,
      string? stableHashAlgorithmId = null) {
    using var provider = CreateServiceProvider(stableHashAlgorithmId);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var profile = DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
        storageProfile,
        "sha256-v1",
        32);

    return diagnostics.Analyze(metadataModel, profile);
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
                    MinimumTotalOperationCount: 100),
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

  private static DataVaultDiagnosticsResult CreateSqlServerSqlArtifactDiagnosticsResult(
      string providerName = "Microsoft.EntityFrameworkCore.SqlServer",
      string capabilityProfileName = "sqlserver-v1",
      string selectedStrategyName = "SqlServerDataVaultSaveStrategy") {
    return new DataVaultDiagnosticsResult(
        new DataVaultValidationDiagnostics(true, Array.Empty<DataVaultDiagnosticsIssue>()),
        new DataVaultExplainDiagnostics(
            "command-test",
            "fingerprint-1",
            providerName,
            capabilityProfileName,
            false,
            DataVaultProviderValueFormat.NativeDateTimeOffset,
            "datetimeoffset",
            DataVaultProviderBehaviorProfiles.ProviderNeutral.ProfileName,
            false,
            Array.Empty<DataVaultEntityExplain>()),
        new DataVaultSaveStrategyDiagnostics(
            DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
            ProviderName: providerName,
            SelectedStrategyName: selectedStrategyName,
            SelectedStrategyPriority: 100,
            Candidates: [
                new DataVaultSaveStrategyCandidateDiagnostics(
                    0,
                    selectedStrategyName,
                    100,
                    true,
                    Array.Empty<DataVaultSaveStrategyFallbackCause>()) {
                  SupportedProviderNames = [providerName],
                  GateRequirements = [
                      new DataVaultSaveStrategyGateRequirement(
                          DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
                          MinimumTotalOperationCount: 100),
                      new DataVaultSaveStrategyGateRequirement(
                          DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
                          MinimumTotalOperationCount: 900),
                      new DataVaultSaveStrategyGateRequirement(
                          DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold,
                          MaximumSatelliteOperationCount: 500),
                  ],
                },
            ],
            FallbackCauses: Array.Empty<DataVaultSaveStrategyFallbackCause>()),
        Array.Empty<DataVaultDiagnosticsIssue>());
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
