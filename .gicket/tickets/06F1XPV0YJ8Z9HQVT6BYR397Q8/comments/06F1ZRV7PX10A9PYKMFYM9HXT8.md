[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 7/7 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027 at commit \u002780f190e90848\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu",
    "commitSha": "80f190e90848",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Fixtures cover AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn with at least one safe and one finding-producing example each, and every finding example names the governing invariant and expected DVM code.",
      "satisfied": true,
      "reason": "Verified test evidence shows deterministic migration-operation fixtures cover AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn, including safe and finding-producing examples with invariant markers MI-1 through MI-5 and expected DVM codes."
    },
    {
      "expectation": "Safe examples produce no issues: satellite payload AddColumn, satellite non-key payload DropColumn, non-DVault DropTable, satellite payload RenameColumn, supplemental non-DVault CreateIndex, and non-key satellite payload AlterColumn.",
      "satisfied": true,
      "reason": "The verified unit coverage includes safe AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn examples through DataVaultDiagnosticsResult.Issues, and the deterministic test command passed."
    },
    {
      "expectation": "Finding examples emit deterministic issues using these expectations: AddColumn to Hub* or Link* descriptive payload emits DVM2001 error; Drop or Alter LoadTimestamp, RecordSource, or HashDiff emits DVM2002 error; Drop or Alter hash-key, participant, parent, or driving-key shape emits DVM2003 error; wrong DVault default index semantics emit DVM2004 warning; rename of a DVault-owned produced column emits DVM2005 warning; drop of a DVault-produced table emits DVM2006 error.",
      "satisfied": true,
      "reason": "Evidence shows assertions for DVM2001, DVM2002, DVM2003, DVM2004, DVM2005, and DVM2006 with the required error or warning severities, deterministic ordering, and operation-specific paths."
    },
    {
      "expectation": "Issue Path values use the stable migration/{OperationType}/{Target}/{Member?} format so tests can assert exact location strings.",
      "satisfied": true,
      "reason": "Observed assertions use exact stable paths such as migration/AddColumn/HubCustomer/CustomerStatus, migration/DropColumn/SatCustomerContact/HashDiff, migration/AlterColumn/LinkCustomerOrder/OrderHashKey, and migration/DropTable/HubCustomer."
    },
    {
      "expectation": "Findings surface through DataVaultDiagnosticsResult.Issues and tests assert code, severity, path, and catalog remediation by code lookup without adding new public fields.",
      "satisfied": true,
      "reason": "Findings are appended to DataVaultDiagnosticsResult.Issues, and tests assert code, severity, path, invariant text, validation filtering, and catalog remediation lookup without evidence of added public fields."
    },
    {
      "expectation": "Public API snapshot remains unchanged for IDataVaultDiagnosticsService, DataVaultDiagnosticsIssue, and DataVaultDiagnosticsResult.",
      "satisfied": true,
      "reason": "The implementation adds an internal migration diagnostics helper and modifies catalog/tests only; verification shows no public diagnostics API or approved API snapshot changes, and dotnet test passed."
    },
    {
      "expectation": "The ticket contract no longer states that 06F1XPS7KGKBP5SVMQPJC49J2G blocks this ticket.",
      "satisfied": true,
      "reason": "The persisted delivery contract explicitly states the stale blocks relation from 06F1XPS7KGKBP5SVMQPJC49J2G was removed and no longer states that ticket blocks this one."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Implementation contains the migration-operation validator, deterministic fixtures, and tests for the six supported operations and named invariants.",
      "satisfied": true,
      "reason": "Verified committed files include DataVaultMigrationOperationDiagnostics.cs, deterministic unit and integration fixtures, and tests covering the six supported operations and named invariants."
    },
    {
      "expectation": "Migration diagnostic catalog entries DVM2001 through DVM2006 exist with stable code-to-severity and remediation mappings.",
      "satisfied": true,
      "reason": "DataVaultDiagnosticCatalog.cs contains DVM2001 through DVM2006 with stable severities error,error,error,warning,warning,error and remediation text, and tests assert the lookup."
    },
    {
      "expectation": "Repository tests covering diagnostics and schema expectations pass with exact assertions over issue ordering, code, severity, path, and remediation lookup.",
      "satisfied": true,
      "reason": "The configured repository command dotnet test DVault.slnx --nologo succeeded, and verification evidence shows exact assertions over ordering, code, severity, path, and remediation lookup."
    },
    {
      "expectation": "No public diagnostics API or approved API snapshot changes are required for this ticket.",
      "satisfied": true,
      "reason": "No public diagnostics API or approved API snapshot changes are required; the helper is internal and findings continue through the existing DataVaultDiagnosticsResult.Issues surface."
    },
    {
      "expectation": "Live relation state remains consistent with the contract: this ticket is no longer blocked by 06F1XPS7KGKBP5SVMQPJC49J2G.",
      "satisfied": true,
      "reason": "The persisted contract states the stale block from 06F1XPS7KGKBP5SVMQPJC49J2G was removed, and no verification finding reports conflicting live relation state."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u002780f190e90848\u0027 on branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027.",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027 exists at verified commit \u002780f190e90848\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: internal static class DataVaultDiagnosticCatalog {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: private const string ErrorSeverity = \u0022error\u0022;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: private const string WarningSeverity = \u0022warning\u0022;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: private static readonly IReadOnlyList\u003CDataVaultDiagnosticDefinition\u003E ModelArtifactSeedDefinitions =",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: [",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Raised when a migration drops or alters LoadTimestamp, RecordSource, or satellite HashDiff.\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: ErrorSeverity,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Use only supported dvault.model.v1 capabilities or split the model into declarations the current runtime can map.\u0022),",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Review the projection error, adjust the affected declaration, and retry the import before applying metadata.\u0022),",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Raised when a migration adds a descriptive payload column to a Data Vault hub or link table.\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs\u0027: \u0022Move descriptive values to a satellite or exclude the payload column from the Data Vault-produced hub or link table.\u0022),",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027 exists at verified commit \u002780f190e90848\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: internal static class DataVaultMigrationOperationDiagnostics {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: public static DataVaultDiagnosticsResult Analyze(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: DataVaultDiagnosticsResult baseline,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: return column.TechnicalRole is TechnicalMetadataColumnRole.LoadTimestamp or",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: .Where(issue =\u003E issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0027: \u0022error\u0022 =\u003E DataVaultDiagnosticsIssueSeverity.Error,",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027 exists at verified commit \u002780f190e90848\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: LoadTimestamp,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 10, 0, 0, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: Assert.Equal(DataVaultDiagnosticsIssueSeverity.Error, issue.Severity);",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027 exists at verified commit \u002780f190e90848\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Unit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: DataVaultLogicalPropertyKind.LoadTimestamp,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: .Single(property =\u003E property.TechnicalRole == TechnicalMetadataColumnRole.LoadTimestamp)",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: public void AnalyzeBuiltInProviderProfilesAndLoadTimestampStorageVariants() {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: DataVaultLoadTimestampStorage.ProviderDefault,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: DataVaultLoadTimestampStorage.Iso8601UtcText,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: DataVaultLoadTimestampStorage.UtcTicks,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: var selectedProfile = profile.WithLoadTimestampStorage(storage);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: Assert.NotEmpty(result.Explain.LoadTimestampStoreType);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: Name = \u0022LoadTimestamp\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: \u0022migration/RenameColumn/HubCustomer/LoadTimestamp\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: DataVaultDiagnosticsIssueSeverity.Error,",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027 exists at verified commit \u002780f190e90848\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Unit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: public sealed class DataVaultMigrationOperationDiagnosticsTests {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: Name = \u0022LoadTimestamp\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2005\u0022, DataVaultDiagnosticsIssueSeverity.Warning, \u0022migration/RenameColumn/HubCustomer/LoadTimestamp\u0022, \u0022MI-5\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: [\u0022error\u0022, \u0022error\u0022, \u0022error\u0022, \u0022warning\u0022, \u0022warning\u0022, \u0022error\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2001\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AddColumn/HubCustomer/CustomerStatus\u0022, \u0022MI-1\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2002\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropColumn/SatCustomerContact/HashDiff\u0022, \u0022MI-2\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropColumn/SatCustomerContact/CustomerHashKey\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2002\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AlterColumn/HubCustomer/RecordSource\u0022, \u0022MI-2\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2003\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/AlterColumn/LinkCustomerOrder/OrderHashKey\u0022, \u0022MI-3\u0022),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0027: issue =\u003E AssertIssue(issue, \u0022DVM2006\u0022, DataVaultDiagnosticsIssueSeverity.Error, \u0022migration/DropTable/HubCustomer\u0022, \u0022MI-5\u0022));",
    "Committed branch delta contains 5 inspectable repository path(s): Modified: src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs, Added: src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs, Modified: tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs, Modified: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, Added: tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault2\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 120 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/migrations, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027.",
    "Ticket history references implementation commit \u002780f190e90848\u0027.",
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
- ticket-id: `06F1XPV0YJ8Z9HQVT6BYR397Q8`
- target-role: `integrator`
- verification-summary: Tester verified 7/7 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu' at commit '80f190e90848'.
- acceptance-criteria: `7/7` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu`
- implementation-commit: `80f190e90848`
- implementation-pr: `<none>`
- implementation-change: `<none>`