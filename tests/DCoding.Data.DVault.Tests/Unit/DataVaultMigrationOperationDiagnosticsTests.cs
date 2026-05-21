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
        ["DVM2001", "DVM2002", "DVM2003", "DVM2004", "DVM2005", "DVM2006"],
        definitions.Select(definition => definition.Code));
    Assert.Equal(
        ["error", "error", "error", "warning", "warning", "error"],
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
            new DropColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
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

    Assert.True(result.Validation.IsValid);
    Assert.Empty(result.Issues);
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

    Assert.True(result.Validation.IsValid);
    Assert.Empty(result.Issues);
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
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/CreateTable/HubCustomer/PkHubCustomerWrongName", "MI-4"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/LinkCustomerOrder/OrderHashKey", "MI-3"),
        issue => AssertIssue(issue, "DVM2001", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/LinkCustomerOrder/CampaignCode", "MI-1"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/SatCustomerContactChannel/ContactType", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/PitCustomerContact/ContactLoadTimestamp", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/PitCustomerContact/UnauthorizedSnapshot", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/CreateTable/BridgeSalesRegionHierarchy/TraversalDepth", "MI-3"),
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/CreateIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey", "MI-4"),
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/AddPrimaryKey/HubCustomer/PkHubCustomerWrongAgain", "MI-4"));
    Assert.Equal(
        [
            "DVM2002",
            "DVM2001",
            "DVM2003",
            "DVM2001",
            "DVM2003",
            "DVM2003",
            "DVM2003",
            "DVM2003",
        ],
        result.Validation.Issues.Select(issue => issue.Code));
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
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/CreateIndex/HubCustomer/IxHubCustomerBusinessKeyCustomerId", "MI-4"),
        issue => AssertIssue(issue, "DVM2005", DataVaultDiagnosticsIssueSeverity.Warning, "migration/RenameColumn/HubCustomer/LoadTimestamp", "MI-5"),
        issue => AssertIssue(issue, "DVM2002", DataVaultDiagnosticsIssueSeverity.Error, "migration/AlterColumn/HubCustomer/RecordSource", "MI-2"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/AlterColumn/LinkCustomerOrder/OrderHashKey", "MI-3"),
        issue => AssertIssue(issue, "DVM2006", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropTable/HubCustomer", "MI-5"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/AddColumn/PitCustomerContact/UnauthorizedSnapshot", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropColumn/PitCustomerContact/ContactLoadTimestamp", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/AlterColumn/BridgeCustomerOrder/OrderHashKey", "MI-3"),
        issue => AssertIssue(issue, "DVM2003", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropColumn/BridgeSalesRegionHierarchy/TraversalDepth", "MI-3"),
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/DropIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey", "MI-4"),
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/RenameIndex/BridgeCustomerOrder/IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey", "MI-4"),
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/AddPrimaryKey/HubCustomer/PkHubCustomerWrongName", "MI-4"),
        issue => AssertIssue(issue, "DVM2004", DataVaultDiagnosticsIssueSeverity.Warning, "migration/DropPrimaryKey/PitCustomerContact/PkPitCustomerContactCustomerHashKeyLoadTimestamp", "MI-4"),
        issue => AssertIssue(issue, "DVM2006", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropTable/BridgeCustomerOrder", "MI-5"));
    Assert.Equal(
        [
            "DVM2001",
            "DVM2002",
            "DVM2003",
            "DVM2002",
            "DVM2003",
            "DVM2006",
            "DVM2003",
            "DVM2003",
            "DVM2003",
            "DVM2003",
            "DVM2006",
        ],
        result.Validation.Issues.Select(issue => issue.Code));
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
            DataVaultMigrationGuardrailOperationOutcome.Risky,
            DataVaultMigrationGuardrailOperationOutcome.Risky,
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
        ["DVM2002", "DVM2002", "DVM2005", "DVM2004", "DVM2004", "DVM2006"],
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
    Assert.Equal(["DVM2002", "DVM2004"], summary.Issues.Select(issue => issue.Code));
    Assert.Equal(
        [DataVaultDiagnosticsIssueSeverity.Error, DataVaultDiagnosticsIssueSeverity.Warning],
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
        "  - Error DVM2006 migration/DropTable/BridgeCustomerOrder: MI-5 violation: migration drops Data Vault-produced table 'BridgeCustomerOrder'. Remediation: " +
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
      operation.Columns.Add(CreateColumn(tableName, property.Name, GetColumnClrType(property)));
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
      Type? clrType = null) {
    return new AddColumnOperation {
      Table = tableName,
      Name = columnName,
      ClrType = clrType ?? typeof(string),
    };
  }

  private static Type GetColumnClrType(DataVaultPropertyExplain property) {
    return property.LogicalPropertyKind switch {
      DataVaultLogicalPropertyKind.BridgeDepth => typeof(int),
      DataVaultLogicalPropertyKind.LoadTimestamp or
          DataVaultLogicalPropertyKind.SatelliteSnapshotReference => typeof(DateTimeOffset),
      _ => typeof(string),
    };
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
