using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultMigrationOperationDiagnosticsTests {
  [Fact]
  public void MigrationOperationDiagnosticCatalogExposesApprovedDefinitions() {
    var definitions = DataVaultDiagnosticCatalog.MigrationOperationDefinitions;

    Assert.Equal(
        ["DVM2001", "DVM2002", "DVM2003", "DVM2004", "DVM2005", "DVM2006", "DVM2007", "DVM2008", "DVM2009", "DVM2010"],
        definitions.Select(definition => definition.Code));
    Assert.Equal(
        ["error", "error", "error", "warning", "warning", "error", "error", "warning", "error", "error"],
        definitions.Select(definition => definition.Severity));
    Assert.All(definitions, definition => {
      Assert.NotEmpty(definition.Summary);
      Assert.NotEmpty(definition.Remediation);
    });
  }

  [Fact]
  public void AnalyzeMigrationOperationsKeepsSafeMatrixQuiet() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());

    var result = DataVaultMigrationOperationDiagnostics.Analyze(
        baseline,
        [
            new AddColumnOperation {
              Table = "SatCustomerContact",
              Name = "PhoneNumber",
              ClrType = typeof(string),
            },
            new DropTableOperation {
              Name = "LegacyAuditScratch",
            },
            new RenameColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
              NewName = "StatusCode",
            },
            new CreateIndexOperation {
              Table = "SatCustomerContact",
              Name = "IX_SatCustomerContact_EmailAddress",
              Columns = ["EmailAddress"],
              IsUnique = false,
            },
            new RenameIndexOperation {
              Table = "ApplicationAudit",
              Name = "IX_ApplicationAudit_Description",
              NewName = "IX_ApplicationAudit_Description_New",
            },
            new AlterColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
              ClrType = typeof(string),
            },
        ]);

    Assert.Empty(result.Issues.Select(issue => issue.Code + " " + issue.Path + " " + issue.Message));
    Assert.True(result.Validation.IsValid);
  }

  [Fact]
  public void AnalyzeCreateTableOperationsKeepsNonDataVaultAndMatchingDataVaultTablesQuiet() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());

    var result = DataVaultMigrationOperationDiagnostics.Analyze(
        baseline,
        [
            CreateNonDataVaultCreateTableOperation(),
            CreateMatchingCreateTableOperation(baseline, "HubCustomer"),
            CreateMatchingCreateTableOperation(baseline, "LinkCustomerOrder"),
            CreateMatchingCreateTableOperation(baseline, "SatCustomerContact"),
            CreateMatchingCreateTableOperation(baseline, "PitCustomerContact"),
            CreateMatchingCreateTableOperation(baseline, "BridgeCustomerOrder"),
            CreateMatchingCreateTableOperation(baseline, "BridgeSalesRegionHierarchy"),
        ]);

    Assert.Empty(result.Issues.Select(issue => issue.Code + " " + issue.Path + " " + issue.Message));
    Assert.True(result.Validation.IsValid);
  }

  [Fact]
  public void AnalyzeCreateTableOperationsReportsDeterministicFindingsForDataVaultShapeMismatches() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());
    var hubCreate = CreateMatchingCreateTableOperation(baseline, "HubCustomer");
    RemoveCreateTableColumn(hubCreate, "LoadTimestamp");
    hubCreate.Columns.Add(CreateColumn("HubCustomer", "CustomerStatus"));
    hubCreate.PrimaryKey = new AddPrimaryKeyOperation {
      Table = "HubCustomer",
      Name = "PkHubCustomerWrongName",
      Columns = ["CustomerId"],
    };
    var linkCreate = CreateMatchingCreateTableOperation(baseline, "LinkCustomerOrder");
    RemoveCreateTableColumn(linkCreate, "OrderHashKey");
    linkCreate.Columns.Add(CreateColumn("LinkCustomerOrder", "CampaignCode"));
    var multiActiveSatelliteCreate = CreateMatchingCreateTableOperation(baseline, "SatCustomerContactChannel");
    RemoveCreateTableColumn(multiActiveSatelliteCreate, "ContactType");
    var pitCreate = CreateMatchingCreateTableOperation(baseline, "PitCustomerContact");
    RemoveCreateTableColumn(pitCreate, "ContactLoadTimestamp");
    pitCreate.Columns.Add(CreateColumn("PitCustomerContact", "UnauthorizedSnapshot", typeof(DateTimeOffset)));
    var bridgeCreate = CreateMatchingCreateTableOperation(baseline, "BridgeSalesRegionHierarchy");
    RemoveCreateTableColumn(bridgeCreate, "TraversalDepth");

    var result = DataVaultMigrationOperationDiagnostics.Analyze(
        baseline,
        [
            hubCreate,
            linkCreate,
            multiActiveSatelliteCreate,
            pitCreate,
            bridgeCreate,
            new CreateIndexOperation {
              Table = "BridgeCustomerOrder",
              Name = "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
              Columns = ["CustomerHashKey"],
              IsUnique = false,
            },
            new AddPrimaryKeyOperation {
              Table = "HubCustomer",
              Name = "PkHubCustomerWrongAgain",
              Columns = ["CustomerHashKey"],
            },
        ]);

    Assert.False(result.Validation.IsValid);
    Assert.Collection(
        result.Issues,
        issue => AssertIssue(issue, "DVM2002", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/HubCustomer/LoadTimestamp", "MI-2"),
        issue => AssertIssue(issue, "DVM2001", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/HubCustomer/CustomerStatus", "MI-1"),
        issue => AssertIssue(issue, "DVM2010", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/HubCustomer/PkHubCustomerWrongName", "MI-4"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/LinkCustomerOrder/OrderHashKey", "MI-3"),
        issue => AssertIssue(issue, "DVM2001", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/LinkCustomerOrder/CampaignCode", "MI-1"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/SatCustomerContactChannel/ContactType", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/PitCustomerContact/ContactLoadTimestamp", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/PitCustomerContact/UnauthorizedSnapshot", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/BridgeSalesRegionHierarchy/TraversalDepth", "MI-3"),
        issue => AssertIssue(issue, "DVM2010", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey", "MI-4"),
        issue => AssertIssue(issue, "DVM2010", DataVaultDiagnosticsIssueSeverity.Error, "migration/AddPrimaryKey/HubCustomer/PkHubCustomerWrongAgain", "MI-4"));
    Assert.Equal(
        [
            "DVM2002",
            "DVM2001",
            "DVM2010",
            "DVM2003",
            "DVM2001",
            "DVM2003",
            "DVM2003",
            "DVM2003",
            "DVM2003",
            "DVM2010",
            "DVM2010",
        ],
        result.Validation.Issues.Select(issue => issue.Code));
  }

  [Fact]
  public void AnalyzeCreateTableOperationBlocksMissingGeneratedPrimaryKey() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());
    var operation = CreateMatchingCreateTableOperation(baseline, "HubCustomer", includePrimaryKey: false);
    var primaryKey = baseline.Explain.Entities
        .Single(entity => entity.TableName == "HubCustomer")
        .PrimaryKey;

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, [operation]);

    Assert.False(report.IsValid);
    var issue = Assert.Single(report.Issues);
    AssertIssue(
        issue,
        "DVM2010",
        DataVaultDiagnosticsIssueSeverity.Error,
        "migration/CreateTable/HubCustomer/" + primaryKey.Name,
        "MI-4");
    Assert.Contains("without inline generated primary key", issue.Message, StringComparison.Ordinal);
    Assert.Contains(primaryKey.Name, issue.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void AnalyzeMigrationOperationsReportsDeterministicFindingsForDataVaultOperationMatrix() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());

    var result = DataVaultMigrationOperationDiagnostics.Analyze(
        baseline,
        [
            new AddColumnOperation {
              Table = "HubCustomer",
              Name = "CustomerStatus",
              ClrType = typeof(string),
            },
            new DropColumnOperation {
              Table = "SatCustomerContact",
              Name = "HashDiff",
            },
            new DropColumnOperation {
              Table = "SatCustomerContact",
              Name = "CustomerHashKey",
            },
            new DropColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
            },
            new CreateIndexOperation {
              Table = "HubCustomer",
              Name = "IxHubCustomerBusinessKeyCustomerId",
              Columns = ["RecordSource"],
              IsUnique = false,
            },
            new RenameColumnOperation {
              Table = "HubCustomer",
              Name = "LoadTimestamp",
              NewName = "LoadedAt",
            },
            new AlterColumnOperation {
              Table = "HubCustomer",
              Name = "RecordSource",
              ClrType = typeof(string),
            },
            new AlterColumnOperation {
              Table = "LinkCustomerOrder",
              Name = "OrderHashKey",
              ClrType = typeof(string),
            },
            new DropTableOperation {
              Name = "HubCustomer",
            },
            new AddColumnOperation {
              Table = "PitCustomerContact",
              Name = "UnauthorizedSnapshot",
              ClrType = typeof(DateTimeOffset),
            },
            new DropColumnOperation {
              Table = "PitCustomerContact",
              Name = "ContactLoadTimestamp",
            },
            new AlterColumnOperation {
              Table = "BridgeCustomerOrder",
              Name = "OrderHashKey",
              ClrType = typeof(string),
            },
            new DropColumnOperation {
              Table = "BridgeSalesRegionHierarchy",
              Name = "TraversalDepth",
            },
            new DropIndexOperation {
              Table = "BridgeCustomerOrder",
              Name = "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
            },
            new RenameIndexOperation {
              Table = "BridgeCustomerOrder",
              Name = "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
              NewName = "IxBridgeCustomerOrderTraversal",
            },
            new AddPrimaryKeyOperation {
              Table = "HubCustomer",
              Name = "PkHubCustomerWrongName",
              Columns = ["CustomerHashKey"],
            },
            new DropPrimaryKeyOperation {
              Table = "PitCustomerContact",
              Name = "PkPitCustomerContactCustomerHashKeyLoadTimestamp",
            },
            new DropTableOperation {
              Name = "BridgeCustomerOrder",
            },
        ]);

    Assert.False(result.Validation.IsValid);
    Assert.Collection(
        result.Issues,
        issue => AssertIssue(issue, "DVM2001", DataVaultDiagnosticsIssueSeverity.Error, "migration/AddColumn/HubCustomer/CustomerStatus", "MI-1"),
        issue => AssertIssue(issue, "DVM2002", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropColumn/SatCustomerContact/HashDiff", "MI-2"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropColumn/SatCustomerContact/CustomerHashKey", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropColumn/SatCustomerContact/EmailAddress", "MI-3"),
        issue => AssertIssue(issue, "DVM2010", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateIndex/HubCustomer/IxHubCustomerBusinessKeyCustomerId", "MI-4"),
        issue => AssertIssue(issue, "DVM2005", DataVaultDiagnosticsIssueSeverity.Warning, "migration/RenameColumn/HubCustomer/LoadTimestamp", "MI-5"),
        issue => AssertIssue(issue, "DVM2002", DataVaultDiagnosticsIssueSeverity.Error, "migration/AlterColumn/HubCustomer/RecordSource", "MI-2"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/AlterColumn/LinkCustomerOrder/OrderHashKey", "MI-3"),
        issue => AssertIssue(issue, "DVM2006", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropTable/HubCustomer", "MI-5"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/AddColumn/PitCustomerContact/UnauthorizedSnapshot", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropColumn/PitCustomerContact/ContactLoadTimestamp", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/AlterColumn/BridgeCustomerOrder/OrderHashKey", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropColumn/BridgeSalesRegionHierarchy/TraversalDepth", "MI-3"),
        issue => AssertIssue(issue, "DVM2007", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey", "MI-4"),
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/RenameIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey", "MI-4"),
        issue => AssertIssue(issue, "DVM2010", DataVaultDiagnosticsIssueSeverity.Error, "migration/AddPrimaryKey/HubCustomer/PkHubCustomerWrongName", "MI-4"),
        issue => AssertIssue(issue, "DVM2007", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropPrimaryKey/PitCustomerContact/PkPitCustomerContactCustomerHashKeyLoadTimestamp", "MI-4"),
        issue => AssertIssue(issue, "DVM2006", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropTable/BridgeCustomerOrder", "MI-5"));
    Assert.Equal(
        [
            "DVM2001",
            "DVM2002",
            "DVM2003",
            "DVM2003",
            "DVM2010",
            "DVM2002",
            "DVM2003",
            "DVM2006",
            "DVM2003",
            "DVM2003",
            "DVM2003",
            "DVM2003",
            "DVM2007",
            "DVM2010",
            "DVM2007",
            "DVM2006",
        ],
        result.Validation.Issues.Select(issue => issue.Code));
  }

  [Fact]
  public void AnalyzeMigrationOperationsTreatsExplicitRenameOperationsAsIntentionalRiskyChanges() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
        baseline,
        [
            new RenameTableOperation {
              Name = "HubCustomer",
              NewName = "HubClient",
            },
            new RenameColumnOperation {
              Table = "HubCustomer",
              Name = "LoadTimestamp",
              NewName = "LoadedAt",
            },
            new RenameIndexOperation {
              Table = "BridgeCustomerOrder",
              Name = "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
              NewName = "IxBridgeCustomerOrderTraversal",
            },
            new AlterColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
              ClrType = typeof(string),
            },
        ]);

    Assert.True(report.IsValid);
    Assert.True(report.HasFindings);
    Assert.Equal(
        [
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Safe,
        ],
        report.OperationSummaries.Select(summary => summary.Outcome));
    Assert.Equal(
        ["DVM2005", "DVM2005", "DVM2004"],
        report.Issues.Select(issue => issue.Code));
    Assert.DoesNotContain(report.Issues, issue => issue.Code == "DVM2008");
    Assert.All(report.Issues, issue => Assert.Equal(DataVaultDiagnosticsIssueSeverity.Warning, issue.Severity));
  }

  [Fact]
  public void AnalyzeMigrationOperationsReportsSuspiciousDropPlusAddGeneratedStructureReplacements() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
        baseline,
        [
            new DropTableOperation {
              Name = "HubCustomerArchive",
            },
            CreateMatchingCreateTableOperation(baseline, "HubCustomer"),
            new DropColumnOperation {
              Table = "HubCustomer",
              Name = "CustomerLoadTimestampOld",
            },
            CreateGeneratedColumnOperation(baseline, "HubCustomer", "LoadTimestamp"),
            new DropIndexOperation {
              Table = "BridgeCustomerOrder",
              Name = "IxBridgeCustomerOrderTraversalOldOrderHashKeyCustomerHashKey",
            },
            CreateGeneratedIndexOperation(
                baseline,
                "BridgeCustomerOrder",
                "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey"),
            new DropPrimaryKeyOperation {
              Table = "PitCustomerContact",
              Name = "PkPitCustomerContactOldCustomerHashKeyLoadTimestamp",
            },
            CreateGeneratedPrimaryKeyOperation(baseline, "PitCustomerContact"),
        ]);

    Assert.True(report.IsValid);
    Assert.True(report.HasFindings);
    Assert.Collection(
        report.Issues,
        issue => AssertIssue(issue, "DVM2008", DataVaultDiagnosticsIssueSeverity.Warning, "migration/DropTable/HubCustomerArchive", "MI-5"),
        issue => AssertIssue(issue, "DVM2008", DataVaultDiagnosticsIssueSeverity.Warning, "migration/DropColumn/HubCustomer/CustomerLoadTimestampOld", "MI-5"),
        issue => AssertIssue(issue, "DVM2008", DataVaultDiagnosticsIssueSeverity.Warning, "migration/DropIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOldOrderHashKeyCustomerHashKey", "MI-5"),
        issue => AssertIssue(issue, "DVM2008", DataVaultDiagnosticsIssueSeverity.Warning, "migration/DropPrimaryKey/PitCustomerContact/PkPitCustomerContactOldCustomerHashKeyLoadTimestamp", "MI-5"));
    Assert.Equal(
        [
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Safe,
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Safe,
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Safe,
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Safe,
        ],
        report.OperationSummaries.Select(summary => summary.Outcome));
  }

  [Fact]
  public void AnalyzeMigrationOperationsUsesProviderEffectiveIndexShapeAcrossSupportedProfiles() {
    using var provider = CreateServiceProvider();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var metadataModel = CreateMigrationGuardrailMetadataModel();

    foreach (var providerCapabilities in SupportedProviderCapabilityProfiles()) {
      var baseline = diagnostics.Analyze(metadataModel, providerCapabilities);
      var safeIndex = CreateGeneratedIndexOperation(
          baseline,
          "SatCustomerContact",
          FindGeneratedIndex(baseline, "SatCustomerContact").Name);

      var safeReport = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, [safeIndex]);

      Assert.True(safeReport.IsValid);
      Assert.Empty(safeReport.Issues);

      var incompatibleIndex = CreateProviderIncompatibleSatelliteIndexOperation(baseline);
      var incompatibleReport = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, [incompatibleIndex]);

      Assert.False(incompatibleReport.IsValid);
      var issue = Assert.Single(incompatibleReport.Issues);
      AssertIssue(
          issue,
          "DVM2010",
          DataVaultDiagnosticsIssueSeverity.Error,
          "migration/CreateIndex/SatCustomerContact/" + incompatibleIndex.Name,
          "MI-4");
      Assert.Contains(baseline.Explain.CapabilityProfileName, issue.Message, StringComparison.Ordinal);
      Assert.Contains("included", issue.Message, StringComparison.Ordinal);
    }

    var oracleBaseline = diagnostics.Analyze(metadataModel, DataVaultProviderCapabilityProfiles.Oracle);
    var pit = oracleBaseline.Explain.Entities.Single(entity => entity.TableName == "PitCustomerContact");
    var redundantOracleIndex = new CreateIndexOperation {
      Table = pit.TableName,
      Name = "IxPitCustomerContactRedundantPrimaryKeyCoverage",
      Columns = pit.PrimaryKey.PropertyNames.ToArray(),
      IsUnique = false,
    };

    var oracleReport = DataVaultMigrationOperationDiagnostics.AnalyzeReport(oracleBaseline, [redundantOracleIndex]);

    Assert.False(oracleReport.IsValid);
    var oracleIssue = Assert.Single(oracleReport.Issues);
    AssertIssue(
        oracleIssue,
        "DVM2010",
        DataVaultDiagnosticsIssueSeverity.Error,
        "migration/CreateIndex/PitCustomerContact/IxPitCustomerContactRedundantPrimaryKeyCoverage",
        "MI-4");
    Assert.Contains("omits secondary indexes covered by", oracleIssue.Message, StringComparison.Ordinal);
    Assert.Contains("oracle-v1", oracleIssue.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void AnalyzeCreateIndexOperationResolvesDataVaultIncludedIndexAnnotationThroughPhysicalColumns() {
    var baseline = CreatePhysicalIncludedIndexBaseline();
    var entity = baseline.Explain.Entities.Single();
    var index = entity.Indexes.Single();
    var operation = new CreateIndexOperation {
      Table = entity.TableName,
      Name = index.Name,
      Columns = index.PropertyNames.ToArray(),
      IsUnique = index.IsUnique,
      IsDescending = index.PropertyNames
          .Select(propertyName => index.DescendingPropertyNames.Contains(propertyName, StringComparer.Ordinal))
          .ToArray(),
    };
    operation.AddAnnotation(
        DataVaultInternalAnnotationNames.ProviderIncludedIndexPropertyNames,
        new[] { "HashDiff" });

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, [operation]);

    Assert.True(report.IsValid);
    Assert.Empty(report.Issues.Select(issue => issue.Code + " " + issue.Path + " " + issue.Message));
  }

  [Fact]
  public void AnalyzeMigrationOperationsBlocksProviderTimestampStorageDriftAcrossLoadTimestampStorageProfiles() {
    using var provider = CreateServiceProvider();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var metadataModel = CreateMigrationGuardrailMetadataModel();

    foreach (var providerCapabilities in SupportedProviderCapabilityProfiles()
        .SelectMany(CreateLoadTimestampStorageVariants)) {
      var baseline = diagnostics.Analyze(metadataModel, providerCapabilities);
      var loadTimestamp = FindGeneratedColumn(baseline, "HubCustomer", "LoadTimestamp");
      var snapshotReference = FindGeneratedColumn(baseline, "PitCustomerContact", "ContactLoadTimestamp");

      var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
          baseline,
          [
              CreateProviderDriftAlterColumnOperation("HubCustomer", loadTimestamp),
              CreateProviderDriftAlterColumnOperation("PitCustomerContact", snapshotReference),
          ]);

      Assert.False(report.IsValid);
      Assert.Collection(
          report.Issues,
          issue => {
            AssertIssue(
                issue,
                "DVM2002",
                DataVaultDiagnosticsIssueSeverity.Error,
                "migration/AlterColumn/HubCustomer/LoadTimestamp",
                "MI-2");
            Assert.Contains(baseline.Explain.CapabilityProfileName, issue.Message, StringComparison.Ordinal);
            Assert.Contains("provider value format", issue.Message, StringComparison.Ordinal);
          },
          issue => {
            AssertIssue(
                issue,
                "DVM2003",
                DataVaultDiagnosticsIssueSeverity.Error,
                "migration/AlterColumn/PitCustomerContact/ContactLoadTimestamp",
                "MI-3");
            Assert.Contains(baseline.Explain.CapabilityProfileName, issue.Message, StringComparison.Ordinal);
            Assert.Contains("store type", issue.Message, StringComparison.Ordinal);
          });
    }
  }

  [Fact]
  public void AnalyzeMigrationOperationsReportExposesOrderedOperationOutcomes() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());
    MigrationOperation[] operations = [
        CreateMatchingCreateTableOperation(baseline, "HubCustomer"),
        new AddColumnOperation {
          Table = "SatCustomerContact",
          Name = "PhoneNumber",
          ClrType = typeof(string),
        },
        new DropColumnOperation {
          Table = "SatCustomerContact",
          Name = "HashDiff",
        },
        new AlterColumnOperation {
          Table = "HubCustomer",
          Name = "RecordSource",
          ClrType = typeof(string),
        },
        new RenameColumnOperation {
          Table = "HubCustomer",
          Name = "LoadTimestamp",
          NewName = "LoadedAt",
        },
        new CreateIndexOperation {
          Table = "BridgeCustomerOrder",
          Name = "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
          Columns = ["CustomerHashKey"],
          IsUnique = false,
        },
        new AddPrimaryKeyOperation {
          Table = "HubCustomer",
          Name = "PkHubCustomerWrongName",
          Columns = ["CustomerHashKey"],
        },
        new DropTableOperation {
          Name = "BridgeCustomerOrder",
        },
    ];

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, operations);

    Assert.False(report.IsValid);
    Assert.True(report.HasFindings);
    Assert.Equal(
        [
            DataVaultMigrationGuardrailOperationOutcome.Safe,
            DataVaultMigrationGuardrailOperationOutcome.Safe,
            DataVaultMigrationGuardrailOperationOutcome.Incompatible,
            DataVaultMigrationGuardrailOperationOutcome.Incompatible,
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Incompatible,
            DataVaultMigrationGuardrailOperationOutcome.Incompatible,
            DataVaultMigrationGuardrailOperationOutcome.Incompatible,
        ],
        report.OperationSummaries.Select(summary => summary.Outcome));
    Assert.Equal(Enumerable.Range(0, operations.Length), report.OperationSummaries.Select(summary => summary.Ordinal));
    Assert.Equal(
        [
            "migration/CreateTable/HubCustomer",
            "migration/AddColumn/SatCustomerContact/PhoneNumber",
            "migration/DropColumn/SatCustomerContact/HashDiff",
            "migration/AlterColumn/HubCustomer/RecordSource",
            "migration/RenameColumn/HubCustomer/LoadTimestamp",
            "migration/CreateIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
            "migration/AddPrimaryKey/HubCustomer/PkHubCustomerWrongName",
            "migration/DropTable/BridgeCustomerOrder",
        ],
        report.OperationSummaries.Select(summary => summary.Path));
    Assert.Empty(report.OperationSummaries[0].Issues);
    Assert.Empty(report.OperationSummaries[1].Issues);
    Assert.Equal(
        ["DVM2002", "DVM2002", "DVM2005", "DVM2010", "DVM2010", "DVM2006"],
        report.Issues.Select(issue => issue.Code));
    Assert.Equal(report.Issues, report.OperationSummaries.SelectMany(summary => summary.Issues));

    var display = report.ToDisplayString();
    Assert.Contains(
        "provider <none>, capability sqlite-v1, provider behavior provider-neutral-v1",
        display,
        StringComparison.Ordinal);
    Assert.Contains("- safe migration/CreateTable/HubCustomer: no DVM findings", display, StringComparison.Ordinal);
    Assert.Contains("- risky migration/RenameColumn/HubCustomer/LoadTimestamp: findings 1", display, StringComparison.Ordinal);
    Assert.Contains("- incompatible migration/DropTable/BridgeCustomerOrder: findings 1", display, StringComparison.Ordinal);
  }

  [Fact]
  public void AnalyzeMigrationOperationsReportTreatsAnyErrorFindingAsIncompatible() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());
    var hubCreate = CreateMatchingCreateTableOperation(baseline, "HubCustomer");
    RemoveCreateTableColumn(hubCreate, "LoadTimestamp");
    hubCreate.PrimaryKey = new AddPrimaryKeyOperation {
      Table = "HubCustomer",
      Name = "PkHubCustomerWrongName",
      Columns = ["CustomerHashKey"],
    };

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, [hubCreate]);

    var summary = Assert.Single(report.OperationSummaries);
    Assert.Equal(DataVaultMigrationGuardrailOperationOutcome.Incompatible, summary.Outcome);
    Assert.Equal("migration/CreateTable/HubCustomer", summary.Path);
    Assert.Equal(["DVM2002", "DVM2010"], summary.Issues.Select(issue => issue.Code));
    Assert.Equal(
        [DataVaultDiagnosticsIssueSeverity.Error, DataVaultDiagnosticsIssueSeverity.Error],
        summary.Issues.Select(issue => issue.Severity));
  }

  [Fact]
  public void AnalyzeCreateTableOperationReportIncludesRemediationAndDeterministicDisplayString() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());
    var hubCreate = CreateMatchingCreateTableOperation(baseline, "HubCustomer");
    RemoveCreateTableColumn(hubCreate, "RecordSource");

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, [hubCreate]);

    Assert.False(report.IsValid);
    Assert.True(report.HasFindings);
    Assert.Same(report.Diagnostics.Explain, baseline.Explain);

    var issue = Assert.Single(report.Issues);
    Assert.Equal("DVM2002", issue.Code);
    Assert.Equal("migration/CreateTable/HubCustomer/RecordSource", issue.Path);
    Assert.Equal(
        DataVaultDiagnosticCatalog.GetMigrationOperationDefinition("DVM2002").Remediation,
        issue.Remediation);
    Assert.Contains(
        "- Error DVM2002 migration/CreateTable/HubCustomer/RecordSource: MI-2 violation",
        report.ToDisplayString(),
        StringComparison.Ordinal);
  }

  [Fact]
  public void AnalyzeMigrationOperationsReportIncludesRemediationAndDeterministicDisplayString() {
    using var provider = CreateServiceProvider();
    var baseline = provider
        .GetRequiredService<IDataVaultDiagnosticsService>()
        .Analyze(CreateMigrationGuardrailMetadataModel());

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
        baseline,
        [
            new DropTableOperation {
              Name = "BridgeCustomerOrder",
            },
        ]);

    Assert.False(report.IsValid);
    Assert.True(report.HasFindings);
    Assert.Same(report.Diagnostics.Explain, baseline.Explain);

    var issue = Assert.Single(report.Issues);
    Assert.Equal("DVM2006", issue.Code);
    Assert.Equal("migration/DropTable/BridgeCustomerOrder", issue.Path);
    Assert.Equal(
        DataVaultDiagnosticCatalog.GetMigrationOperationDefinition("DVM2006").Remediation,
        issue.Remediation);
    Assert.Equal(
        "DVault migration guardrails: invalid, findings 1, operations 1, provider <none>, capability sqlite-v1, provider behavior provider-neutral-v1" + Environment.NewLine +
        "- incompatible migration/DropTable/BridgeCustomerOrder: findings 1" + Environment.NewLine +
        "  - Error DVM2006 migration/DropTable/BridgeCustomerOrder: MI-5 destructive change: migration drops Data Vault bridge table 'BridgeCustomerOrder' (metadata name 'CustomerOrder', produced name 'BridgeCustomerOrder'). Remediation: " +
        issue.Remediation,
        report.ToDisplayString());
  }

  private static CreateTableOperation CreateNonDataVaultCreateTableOperation() {
    var operation = new CreateTableOperation {
      Name = "ApplicationAudit",
      PrimaryKey = new AddPrimaryKeyOperation {
        Table = "ApplicationAudit",
        Name = "PkApplicationAudit",
        Columns = ["Id"],
      },
    };
    operation.Columns.Add(CreateColumn("ApplicationAudit", "Id", typeof(int)));
    operation.Columns.Add(CreateColumn("ApplicationAudit", "Description"));

    return operation;
  }

  private static CreateTableOperation CreateMatchingCreateTableOperation(
      DataVaultDiagnosticsResult baseline,
      string tableName,
      bool includePrimaryKey = true) {
    var entity = baseline.Explain.Entities.Single(entity => entity.TableName == tableName);
    var operation = new CreateTableOperation {
      Name = tableName,
    };

    foreach (var property in entity.Properties) {
      operation.Columns.Add(CreateColumn(tableName, property));
    }

    if (includePrimaryKey) {
      operation.PrimaryKey = new AddPrimaryKeyOperation {
        Table = tableName,
        Name = entity.PrimaryKey.Name,
        Columns = entity.PrimaryKey.PropertyNames.ToArray(),
      };
    }

    return operation;
  }

  private static AddColumnOperation CreateColumn(
      string tableName,
      string columnName,
      Type? clrType = null,
      string? columnType = null,
      bool isNullable = false) {
    return new AddColumnOperation {
      Table = tableName,
      Name = columnName,
      ClrType = clrType ?? typeof(string),
      ColumnType = columnType,
      IsNullable = isNullable,
    };
  }

  private static AddColumnOperation CreateColumn(
      string tableName,
      DataVaultPropertyExplain property) {
    var operation = CreateColumn(
        tableName,
        property.Name,
        GetColumnClrType(property),
        property.StoreType,
        property.IsNullable);
    operation.AddAnnotation(DataVaultAnnotationNames.ProviderValueFormat, property.ValueFormat);

    return operation;
  }

  private static AddColumnOperation CreateGeneratedColumnOperation(
      DataVaultDiagnosticsResult baseline,
      string tableName,
      string columnName) {
    var property = baseline.Explain.Entities
        .Single(entity => entity.TableName == tableName)
        .Properties
        .Single(property => property.Name == columnName);

    return CreateColumn(tableName, property);
  }

  private static CreateIndexOperation CreateGeneratedIndexOperation(
      DataVaultDiagnosticsResult baseline,
      string tableName,
      string indexName) {
    var index = baseline.Explain.Entities
        .Single(entity => entity.TableName == tableName)
        .Indexes
        .Single(index => index.Name == indexName);

    var operation = new CreateIndexOperation {
      Table = tableName,
      Name = index.Name,
      Columns = index.PropertyNames.ToArray(),
      IsUnique = index.IsUnique,
      IsDescending = index.PropertyNames
          .Select(propertyName => index.DescendingPropertyNames.Contains(propertyName, StringComparer.Ordinal))
          .ToArray(),
    };
    AddIncludedIndexAnnotation(operation, baseline.Explain.CapabilityProfileName, index.IncludedPropertyNames);

    return operation;
  }

  private static AddPrimaryKeyOperation CreateGeneratedPrimaryKeyOperation(
      DataVaultDiagnosticsResult baseline,
      string tableName) {
    var primaryKey = baseline.Explain.Entities
        .Single(entity => entity.TableName == tableName)
        .PrimaryKey;

    return new AddPrimaryKeyOperation {
      Table = tableName,
      Name = primaryKey.Name,
      Columns = primaryKey.PropertyNames.ToArray(),
    };
  }

  private static IReadOnlyList<DataVaultProviderCapabilityProfile> SupportedProviderCapabilityProfiles() {
    return
    [
        DataVaultProviderCapabilityProfiles.Sqlite,
        DataVaultProviderCapabilityProfiles.Oracle,
        DataVaultProviderCapabilityProfiles.Postgres,
        DataVaultProviderCapabilityProfiles.SqlServer,
        DataVaultProviderCapabilityProfiles.MySql,
    ];
  }

  private static IReadOnlyList<DataVaultProviderCapabilityProfile> CreateLoadTimestampStorageVariants(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    return
    [
        providerCapabilities,
        providerCapabilities.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.Iso8601UtcText),
        providerCapabilities.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.UtcTicks),
    ];
  }

  private static DataVaultIndexExplain FindGeneratedIndex(
      DataVaultDiagnosticsResult baseline,
      string tableName) {
    return baseline.Explain.Entities
        .Single(entity => entity.TableName == tableName)
        .Indexes
        .Single();
  }

  private static DataVaultPropertyExplain FindGeneratedColumn(
      DataVaultDiagnosticsResult baseline,
      string tableName,
      string columnName) {
    return baseline.Explain.Entities
        .Single(entity => entity.TableName == tableName)
        .Properties
        .Single(property => property.Name == columnName);
  }

  private static CreateIndexOperation CreateProviderIncompatibleSatelliteIndexOperation(
      DataVaultDiagnosticsResult baseline) {
    var index = FindGeneratedIndex(baseline, "SatCustomerContact");
    var hashDiffColumnName = FindGeneratedColumn(baseline, "SatCustomerContact", "HashDiff").Name;
    var capabilityProfileName = baseline.Explain.CapabilityProfileName;
    if (capabilityProfileName.StartsWith("sqlserver-", StringComparison.Ordinal) ||
        capabilityProfileName.StartsWith("postgres-", StringComparison.Ordinal)) {
      return CreateIndexOperation(
          "SatCustomerContact",
          index,
          index.PropertyNames.Concat(index.IncludedPropertyNames).Distinct(StringComparer.Ordinal).ToArray(),
          includedColumnNames: []);
    }

    if (capabilityProfileName.StartsWith("mysql-", StringComparison.Ordinal)) {
      return CreateIndexOperation(
          "SatCustomerContact",
          index,
          index.PropertyNames,
          [hashDiffColumnName]);
    }

    return CreateIndexOperation(
        "SatCustomerContact",
        index,
        index.PropertyNames.Where(propertyName => !string.Equals(propertyName, hashDiffColumnName, StringComparison.Ordinal)).ToArray(),
        [hashDiffColumnName]);
  }

  private static CreateIndexOperation CreateIndexOperation(
      string tableName,
      DataVaultIndexExplain index,
      IReadOnlyList<string> columnNames,
      IReadOnlyList<string> includedColumnNames) {
    var operation = new CreateIndexOperation {
      Table = tableName,
      Name = index.Name,
      Columns = columnNames.ToArray(),
      IsUnique = index.IsUnique,
      IsDescending = columnNames
          .Select(propertyName => index.DescendingPropertyNames.Contains(propertyName, StringComparer.Ordinal))
          .ToArray(),
    };
    if (includedColumnNames.Count > 0) {
      operation.AddAnnotation("SqlServer:Include", includedColumnNames.ToArray());
    }

    return operation;
  }

  private static AlterColumnOperation CreateProviderDriftAlterColumnOperation(
      string tableName,
      DataVaultPropertyExplain column) {
    var driftValueFormat = column.ValueFormat == DataVaultProviderValueFormat.UtcTicks
        ? DataVaultProviderValueFormat.Iso8601UtcText
        : DataVaultProviderValueFormat.UtcTicks;
    var operation = new AlterColumnOperation {
      Table = tableName,
      Name = column.Name,
      ClrType = driftValueFormat == DataVaultProviderValueFormat.UtcTicks ? typeof(long) : typeof(string),
      ColumnType = driftValueFormat == DataVaultProviderValueFormat.UtcTicks ? "bigint" : "varchar(33)",
      IsNullable = !column.IsNullable,
    };
    operation.AddAnnotation(DataVaultAnnotationNames.ProviderValueFormat, driftValueFormat);

    return operation;
  }

  private static Type GetColumnClrType(DataVaultPropertyExplain property) {
    return property.ClrTypeName switch {
      "System.DateTimeOffset" => typeof(DateTimeOffset),
      "System.Int32" => typeof(int),
      "System.Int64" => typeof(long),
      "System.String" => typeof(string),
      _ => property.LogicalPropertyKind switch {
        DataVaultLogicalPropertyKind.BridgeDepth => typeof(int),
        DataVaultLogicalPropertyKind.LoadTimestamp or
            DataVaultLogicalPropertyKind.SatelliteSnapshotReference => typeof(DateTimeOffset),
        _ => typeof(string),
      },
    };
  }

  private static void AddIncludedIndexAnnotation(
      CreateIndexOperation operation,
      string capabilityProfileName,
      IReadOnlyList<string> includedColumnNames) {
    if (includedColumnNames.Count == 0) {
      return;
    }

    if (capabilityProfileName.StartsWith("sqlserver-", StringComparison.Ordinal)) {
      operation.AddAnnotation("SqlServer:Include", includedColumnNames.ToArray());
      return;
    }

    if (capabilityProfileName.StartsWith("postgres-", StringComparison.Ordinal)) {
      operation.AddAnnotation("Npgsql:IndexInclude", includedColumnNames.ToArray());
    }
  }

  private static DataVaultDiagnosticsResult CreatePhysicalIncludedIndexBaseline() {
    var properties = new DataVaultPropertyExplain[]
    {
        new(
            "CustomerHashKeyDb",
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.HashKey,
            "Customer",
            0,
            DataVaultLogicalPropertyKind.HashKey,
            "sqlserver-v1",
            "nvarchar(64)",
            DataVaultProviderValueFormat.Text) {
          ClrTypeName = typeof(string).FullName!,
          ProducedName = "CustomerHashKey",
        },
        new(
            "LoadTimestampDb",
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.LoadTimestamp,
            "LoadTimestamp",
            1,
            DataVaultLogicalPropertyKind.LoadTimestamp,
            "sqlserver-v1",
            "datetimeoffset",
            DataVaultProviderValueFormat.NativeDateTimeOffset) {
          ClrTypeName = typeof(DateTimeOffset).FullName!,
          ProducedName = "LoadTimestamp",
        },
        new(
            "HashDiffDb",
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.HashDiff,
            "HashDiff",
            2,
            DataVaultLogicalPropertyKind.HashDiff,
            "sqlserver-v1",
            "nvarchar(64)",
            DataVaultProviderValueFormat.Text) {
          ClrTypeName = typeof(string).FullName!,
          ProducedName = "HashDiff",
        },
    };
    var primaryKey = new DataVaultKeyExplain(
        "PkSatCustomerContactPhysical",
        ["CustomerHashKeyDb", "LoadTimestampDb"]) {
      ProducedName = "PkSatCustomerContact",
    };
    var index = new DataVaultIndexExplain(
        "IxSatCustomerContactPhysicalParent",
        ["CustomerHashKeyDb", "LoadTimestampDb"],
        false,
        ["LoadTimestampDb"],
        ["HashDiffDb"]) {
      ProducedName = "IxSatCustomerContactParentCustomerHashKeyLoadTimestamp",
    };
    var entity = new DataVaultEntityExplain(
        "SatCustomerContactPhysical",
        DataVaultTableKind.Satellite,
        "Contact",
        new DataVaultParentReferenceExplain(DataVaultMetadataReferenceKind.Hub, "Customer"),
        properties,
        primaryKey,
        [index],
        [new DataVaultConstraintExplain(
            primaryKey.Name,
            DataVaultConstraintKind.PrimaryKey,
            primaryKey.PropertyNames) {
          ProducedName = primaryKey.ProducedName,
        }]) {
      ProducedName = "SatCustomerContact",
    };
    var explain = new DataVaultExplainDiagnostics(
        "metadata",
        null,
        "Microsoft.EntityFrameworkCore.SqlServer",
        "sqlserver-v1",
        false,
        DataVaultProviderValueFormat.NativeDateTimeOffset,
        "datetimeoffset",
        "sqlserver-v1",
        false,
        [entity]);

    return new DataVaultDiagnosticsResult(
        new DataVaultValidationDiagnostics(true, []),
        explain,
        new DataVaultSaveStrategyDiagnostics(
            DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated,
            null,
            null,
            null,
            [],
            []),
        []);
  }

  private static void RemoveCreateTableColumn(CreateTableOperation operation, string columnName) {
    var column = operation.Columns.Single(column => string.Equals(column.Name, columnName, StringComparison.Ordinal));
    operation.Columns.Remove(column);
  }

  private static void AssertIssue(
      DataVaultDiagnosticsIssue issue,
      string code,
      DataVaultDiagnosticsIssueSeverity severity,
      string path,
      string invariant) {
    Assert.Equal(code, issue.Code);
    Assert.Equal(severity, issue.Severity);
    Assert.Equal(path, issue.Path);
    Assert.Contains(invariant, issue.Message, StringComparison.Ordinal);
    Assert.NotEmpty(DataVaultDiagnosticCatalog.GetMigrationOperationDefinition(code).Remediation);
  }

  private static void AssertIssue(
      DataVaultMigrationGuardrailIssue issue,
      string code,
      DataVaultDiagnosticsIssueSeverity severity,
      string path,
      string invariant) {
    Assert.Equal(code, issue.Code);
    Assert.Equal(severity, issue.Severity);
    Assert.Equal(path, issue.Path);
    Assert.Contains(invariant, issue.Message, StringComparison.Ordinal);
    Assert.NotEmpty(DataVaultDiagnosticCatalog.GetMigrationOperationDefinition(code).Remediation);
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVault();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static DataVaultMetadataModel CreateMigrationGuardrailMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var salesRegion = new DataVaultHubMetadata("SalesRegion", ["Region Code"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var salesRegionParentChild = new DataVaultLinkMetadata(
        "SalesRegionParentChild",
        [
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ParentRegion"),
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ChildRegion"),
        ]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var channel = new DataVaultSatelliteMetadata(
        "ContactChannel",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var customerOrderBridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());
    var salesRegionHierarchyBridge = new DataVaultBridgeMetadata(
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
    var customerContactPit = new DataVaultPitMetadata(customer.ToReference(), ["Contact"]);

    return new DataVaultMetadataModel(
        [customer, order, salesRegion],
        [customerOrder, salesRegionParentChild],
        [contact, channel],
        Array.Empty<DataVaultPointInTimeMetadata>(),
        [customerOrderBridge, salesRegionHierarchyBridge],
        [customerContactPit]);
  }
}
