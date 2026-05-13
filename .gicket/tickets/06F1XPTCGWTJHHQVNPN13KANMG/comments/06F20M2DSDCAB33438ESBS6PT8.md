[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault\u0027 at commit \u0027923e624ce4f5\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault",
    "commitSha": "923e624ce4f5",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Consumers can run the guardrail against generated MigrationOperation input and a DVault metadata baseline from a metadata model, registry, Code-First declaration, or configured DbContext, and the analysis does not require a live database connection.",
      "satisfied": true,
      "reason": "Evidence shows public AnalyzeReport overloads for MigrationOperation input using diagnostics baselines, metadata model, registry, Code-First callback, and configured DbContext; integration coverage and passing tests support operation without a live database round-trip."
    },
    {
      "expectation": "Safe changes remain quiet: non-DVault tables are ignored, and safe satellite payload evolution does not emit findings.",
      "satisfied": true,
      "reason": "Unit coverage is evidenced for quiet and finding cases, and the verification command passed; this supports non-DVault tables staying ignored and safe satellite payload evolution remaining quiet."
    },
    {
      "expectation": "Risky changes to DVault-owned hub, link, satellite, PIT, or bridge tables emit stable DVM diagnostics with deterministic severity, code, path, message, and remediation guidance.",
      "satisfied": true,
      "reason": "Guardrail report/issue artifacts expose stable severity, code, path, message, display rendering, and catalog remediation, with unit assertions over DVM diagnostics for hub, link, satellite, PIT, and bridge cases."
    },
    {
      "expectation": "Guardrails cover required technical columns, stable key/parent/participant/driving columns, PIT snapshot-reference columns, hierarchy bridge TraversalDepth, DVault-owned table drops, and missing or mismatched DVault primary-key/index/uniqueness contracts.",
      "satisfied": true,
      "reason": "Evidence covers technical-column checks, key and relationship column checks, PIT snapshot-reference columns, bridge TraversalDepth, table drops, index/primary-key operations, and the rework commit adds mismatched primary-key validation."
    },
    {
      "expectation": "Hub and link payload-column additions are reported as insert-only violations instead of being treated as safe schema growth.",
      "satisfied": true,
      "reason": "Catalog and unit-test evidence show hub/link payload AddColumn operations reported through DVM2001 insert-only diagnostics rather than treated as safe schema growth."
    },
    {
      "expectation": "Documentation includes one pre-integration example that shows how to surface the structured result and fail a local script or CI/build step before applying a migration.",
      "satisfied": true,
      "reason": "The committed documentation path exists, developer delivery records the concise metadata-only pre-apply usage example, and verification inspected the committed doc at the target commit."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The chosen reusable guardrail API is public, covered by API snapshot updates if needed, and returns a stable diagnostics/report contract suitable for automation.",
      "satisfied": true,
      "reason": "The public guardrail API/report types are committed, the API snapshot was updated, and the report contract is described as automation-friendly with Issues, validity state, and display rendering."
    },
    {
      "expectation": "Unit tests cover quiet and finding cases across hub, link, satellite, PIT, and bridge baselines with representative EF migration operation types.",
      "satisfied": true,
      "reason": "The committed unit test file includes representative quiet and finding coverage across hub, link, satellite, PIT, and bridge migration operation scenarios, and dotnet test passed."
    },
    {
      "expectation": "Any new migration guardrail catalog entries define code, severity, category, summary, explanation, and remediation in the central diagnostics catalog pattern.",
      "satisfied": true,
      "reason": "No distinct new DVM2xxx code was required; reused/updated migration guardrail catalog entries are in the central catalog pattern with severity, category-style catalog data, summaries/explanations, and remediation text."
    },
    {
      "expectation": "Integration coverage proves the guardrail can run from a configured DbContext without applying a migration or requiring a live database round-trip.",
      "satisfied": true,
      "reason": "Integration tests are committed and passed, with evidence that the guardrail runs from a configured DbContext over migration operations without applying a migration or requiring a live database."
    },
    {
      "expectation": "A minimal doc/example is added and kept consistent with current package names, current branch limitations, and the no-SQL-parsing design.",
      "satisfied": true,
      "reason": "The minimal doc/example is committed in docs/plans/deferred-data-vault-capabilities.md and developer-delivery evidence frames it as metadata-only, pre-apply, and aligned with the no-SQL-parsing design."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027923e624ce4f5\u0027 on branch \u0027ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault\u0027.",
    "Committed repository path \u0027docs/plans/deferred-data-vault-capabilities.md\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: # Deferred Data Vault Capability Decision Record",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: Status: v0.5 architecture decision with PIT and bridge metadata baselines",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: Ticket: 06EZ0NSHJVC9SD2KS6PWWNHPJM",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: Decision date: 2026-05-05",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: ## Purpose",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: This record publishes the v0.5 architecture stance for deferred Data Vault capability families. It consolidates the earlier deferred-capabilities note and the optional advanced-con...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: - Deterministic default conventions for technical names, metadata, stable hashing, load timestamps, and record sources.",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: - The explicit \u0060IDataVaultSaveService\u0060 write boundary, where callers supply load timestamp, record source, and vault row intent instead of relying on hidden \u0060SaveChanges\u0060 intercept...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: Advanced hooks are also opt-in. Naming, hashing, record source, timestamp, and provider behavior may become configurable extension categories, but unset hooks inherit the default b...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: The MVP baseline explains how DVault represents business identity, relationships, and descriptive history through hubs, links, satellites, hash keys, hash diffs, load timestamps, a...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: | Multi-active satellites | Multi-active satellites can represent multiple simultaneous descriptive records for one parent at the same load window. | Multi-active modeling needs ex...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: | Advanced hooks | Hooks let advanced users adapt naming, hashing, lineage, timestamps, and provider behavior without destabilizing defaults. | Hook behavior must be scoped by cate...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: - The architecture documents hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources as the MVP concept set.",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: - Default naming, hashing, record source, timestamp, and provider behavior are deterministic defaults.",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: - Advanced hook implementation depth for naming, hashing, record source, timestamp, and provider behavior.",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: The default translated table for that declaration is \u0060PitCustomerProfileStatus\u0060. Its canonical column order is \u0060[CustomerHashKey, LoadTimestamp, ProfileLoadTimestamp, StatusLoadTim...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: The repository still contains the older public \u0060DataVaultPointInTimeMetadata\u0060 and \u0060DataVaultModelBuilder.PointInTime(...)\u0060 modeling surface. That surface is separate from this PIT ...",
    "Observed committed repository file \u0027docs/plans/deferred-data-vault-capabilities.md\u0027: The record is intentionally architecture-level. It does not implement runtime row population behavior, define provider-specific optimization posture, or replace the current MVP hub...",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: internal static class DataVaultDiagnosticCatalog {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: private const string ErrorSeverity = \u0022error\u0022;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: private const string WarningSeverity = \u0022warning\u0022;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: private static readonly IReadOnlyList\u003CDataVaultDiagnosticDefinition\u003E ModelArtifactSeedDefinitions =",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: [",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Raised when a migration drops or alters LoadTimestamp, RecordSource, satellite HashDiff, or PIT load-timestamp columns.\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: ErrorSeverity,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Use only supported dvault.model.v1 capabilities or split the model into declarations the current runtime can map.\u0022),",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Review the projection error, adjust the affected declaration, and retry the import before applying metadata.\u0022),",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Raised when a migration adds a descriptive payload column to a Data Vault hub or link table.\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Move descriptive values to a satellite or exclude the payload column from the Data Vault-produced hub or link table.\u0022),",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs\u0027: /// Automation-friendly migration guardrail issue with central remediation guidance.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs\u0027: /// \u003Cparam name=\u0022Severity\u0022\u003EThe deterministic guardrail severity.\u003C/param\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs\u0027: /// \u003Cparam name=\u0022Code\u0022\u003EThe stable DVM diagnostic code.\u003C/param\u003E",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027: /// Structured Data Vault migration guardrail report suitable for local scripts, tests, and build steps.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs\u0027: /// Gets a value indicating whether the underlying diagnostics result contains no error-severity validation issues.",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: /// Analyzes generated EF Core migration operations against a Data Vault diagnostics explain baseline.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: return column.TechnicalRole is TechnicalMetadataColumnRole.LoadTimestamp or",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: .Where(issue =\u003E issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: /// \u003Creturns\u003EA structured guardrail report for local scripts, tests, or build steps.\u003C/returns\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: \u0022error\u0022 =\u003E DataVaultDiagnosticsIssueSeverity.Error,",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: LoadTimestamp,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 10, 0, 0, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: Assert.Equal(DataVaultDiagnosticsIssueSeverity.Error, issue.Severity);",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Unit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: public sealed class DataVaultMigrationOperationDiagnosticsTests {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: Name = \u0022LoadTimestamp\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: Name = \u0022ContactLoadTimestamp\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: Name = \u0022PkPitCustomerContactCustomerHashKeyLoadTimestamp\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2005\u0022, DataVaultDiagnosticsIssueSeverity.Warning, \u0022migration/RenameColumn/HubCustomer/LoadTimestamp\u0022, \u0022MI-5\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropColumn/PitCustomerContact/ContactLoadTimestamp\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2004\u0022, DataVaultDiagnosticsIssueSeverity.Warning, \u0022migration/DropPrimaryKey/PitCustomerContact/PkPitCustomerContactCustomerHashKeyLoadTimestamp\u0022, \u0022M...",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: [\u0022error\u0022, \u0022error\u0022, \u0022error\u0022, \u0022warning\u0022, \u0022warning\u0022, \u0022error\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2001\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AddColumn/HubCustomer/CustomerStatus\u0022, \u0022MI-1\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2002\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropColumn/SatCustomerContact/HashDiff\u0022, \u0022MI-2\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropColumn/SatCustomerContact/CustomerHashKey\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2002\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AlterColumn/HubCustomer/RecordSource\u0022, \u0022MI-2\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AlterColumn/LinkCustomerOrder/OrderHashKey\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2006\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropTable/HubCustomer\u0022, \u0022MI-5\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AddColumn/PitCustomerContact/UnauthorizedSnapshot\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AlterColumn/BridgeCustomerOrder/OrderHashKey\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropColumn/BridgeSalesRegionHierarchy/TraversalDepth\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2006\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropTable/BridgeCustomerOrder\u0022, \u0022MI-5\u0022));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: \u0022DVault migration guardrails: invalid, findings 1\u0022 \u002B Environment.NewLine \u002B",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027 exists at verified commit \u0027923e624ce4f5\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # DVault public API snapshot",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Package: DCoding.Data.DVault",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Assembly: DCoding.Data.DVault",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Generated from built assembly output.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Update intentionally with: DVAULT_UPDATE_API_SNAPSHOTS=1 dotnet test DVault.slnx --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: type public static class DCoding.Data.DVault.DVaultServiceCollectionExtensions",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: method public static Microsoft.EntityFrameworkCore.ModelBuilder ApplyDataVaultMetadata(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder, System.Action\u003CDCoding.Data.DVau...",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: ctor public DataVaultExplainDiagnostics(string MetadataSourceKind, string? MetadataSourceFingerprint, string? ProviderName, string CapabilityProfileName, bool CapabilityProfileDefa...",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: property public DCoding.Data.DVault.DataVaultProviderValueFormat LoadTimestampValueFormat { get; init; }",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: property public string LoadTimestampStoreType { get; init; }",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: value Error = 2",
    "Committed branch delta contains 8 inspectable repository path(s): Modified: docs/plans/deferred-data-vault-capabilities.md, Modified: src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs, Added: src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs, Added: src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs, Modified: src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs, Modified: tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs, Modified: tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs, Modified: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault2\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 122 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/migrations, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.2].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F1XPVPKVGYKCV04PY98TSS78-story-add-dvault-design-time-services-for-dotnet\u0027.",
    "Ticket history references implementation commit \u0027923e624ce4f5\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Route the ticket to the configured integrator gate for final acceptance review."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F1XPTCGWTJHHQVNPN13KANMG`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault' at commit '923e624ce4f5'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault`
- implementation-commit: `923e624ce4f5`
- implementation-pr: `<none>`
- implementation-change: `<none>`