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
  public void AnalyzeMigrationOperationsReportsDeterministicFindingsForSixOperationMatrix() {
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
        issue => AssertIssue(issue, "DVM2006", DataVaultDiagnosticsIssueSeverity.Error, "migration/DropTable/HubCustomer", "MI-5"));
    Assert.Equal(
        ["DVM2001", "DVM2002", "DVM2003", "DVM2002", "DVM2003", "DVM2006"],
        result.Validation.Issues.Select(issue => issue.Code));
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
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var channel = new DataVaultSatelliteMetadata(
        "ContactChannel",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);

    return new DataVaultMetadataModel([customer, order], [customerOrder], [contact, channel]);
  }
}
