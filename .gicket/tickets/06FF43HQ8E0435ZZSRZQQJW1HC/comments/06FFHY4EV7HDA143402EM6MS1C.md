[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa\u0027 at commit \u0027c2183b2c30a6\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa",
    "commitSha": "c2183b2c30a6",
    "pullRequestReference": null,
    "changeReference": null,
    "branchOwnerProvenance": {
      "ticketId": "06FF43HQ8E0435ZZSRZQQJW1HC",
      "ownerBranch": "ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa",
      "sourceCommitSha": "c2183b2c30a6",
      "baseBranch": "develop",
      "producingRole": "test",
      "producingRunId": "10cccbce5ac7417bbbfe87ee77fa36ef",
      "producingInstanceId": "hp-ai-2026-001.1"
    }
  },
  "acceptanceCriteria": [
    {
      "expectation": "Repository tests prove PostgreSQL PIT maintenance declines to provider-neutral fallback for provider mismatch, dirty tracked context, and unsupported PIT rebuild shapes that fall outside the documented PostgreSQL baseline.",
      "satisfied": true,
      "reason": "PostgreSQL coverage is already present in repository tests: PostgresProviderCapabilityTests accepts the documented supported shapes and explicitly records provider-mismatch, dirty-DbContext, and unsupported-shape fallback causes for out-of-baseline rebuild requests."
    },
    {
      "expectation": "Repository tests prove SQL Server PIT maintenance declines to provider-neutral fallback for provider mismatch, dirty tracked context, and unsupported PIT rebuild shapes that fall outside the documented SQL Server baseline.",
      "satisfied": true,
      "reason": "The verified commit extends SqlServerDataVaultPitMaintenanceServiceTests with provider-neutral rebuild assertions for provider mismatch, dirty context, and unsupported multi-active PIT shape, while the existing candidate-gate test still distinguishes unsupported parent and multi-active fallback causes outside the SQL Server ordinary-hub baseline."
    },
    {
      "expectation": "Repository tests prove that omitting provider-specific PIT maintenance registration does not break PIT rebuilds and still executes the provider-neutral path rather than depending on provider-native PIT SQL.",
      "satisfied": true,
      "reason": "The new missing-registration test builds services with AddDVault() only, asserts no IDataVaultProviderPitMaintenanceStrategy registrations exist, resolves DefaultDataVaultPitMaintenanceService, and verifies rebuild rows are still produced through the provider-neutral path."
    },
    {
      "expectation": "Fallback assertions verify both behavioral outcome parity, such as correct rebuilt rows or no-op results, and the existing explicit fallback surface available for that path, such as gate fallback causes or maintenance activity fallback events.",
      "satisfied": true,
      "reason": "Fallback assertions cover both behavior and explicit fallback surfaces: PostgreSQL gate tests assert specific fallback cause kinds, and SQL Server rebuild and maintain-parents tests assert provider-neutral results together with ProviderNeutralFallback activity/event causes where those diagnostics already exist."
    },
    {
      "expectation": "Supported happy-path provider tests continue to pass without weakening the current clean-context provider-native coverage.",
      "satisfied": true,
      "reason": "Supported happy-path coverage remains in place and the verified dotnet test command succeeded, so the existing clean-context provider-native coverage was not weakened."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The fallback matrix is covered in the existing unit and integration test layout under tests/DCoding.Data.DVault.Tests rather than through new ad hoc harnesses.",
      "satisfied": true,
      "reason": "The work stays inside the existing tests/DCoding.Data.DVault.Tests unit and integration layout; the only inspectable repository delta at the verified commit is the existing SQL Server PIT maintenance test file."
    },
    {
      "expectation": "PostgreSQL-specific fallback coverage is deterministic where possible, with live-provider integration kept only for scenarios that actually require Npgsql execution.",
      "satisfied": true,
      "reason": "PostgreSQL fallback proof remains deterministic in unit coverage through PostgresProviderCapabilityTests, while the live-provider integration file stays focused on supported Npgsql happy-path rebuilds."
    },
    {
      "expectation": "SQL Server fallback coverage preserves the current service-level pattern and clearly distinguishes missing-registration, provider-mismatch, dirty-context, and unsupported-shape cases.",
      "satisfied": true,
      "reason": "SQL Server coverage now has distinct tests for provider mismatch, dirty context, missing registration, and unsupported shape, preserving the established service-level pattern and making each fallback case explicit."
    },
    {
      "expectation": "No production-code scope is added unless a failing test shows the repository already violates the documented provider-neutral fallback contract.",
      "satisfied": true,
      "reason": "The verified branch delta adds no production-code changes; it only updates tests, which matches the contract requirement to avoid production scope expansion absent a proven defect."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027c2183b2c30a6\u0027 on branch \u0027ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa\u0027.",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests\u0027 exists at verified commit \u0027c2183b2c30a6\u0027.",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests\u0027 is a tracked directory.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Analyzers/\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj\u0027.",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027 exists at verified commit \u0027c2183b2c30a6\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: using System.Diagnostics;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: Assert.Contains(\u0022WITH [__dvault_pit_timestamps] AS\u0022, insertCommandText, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: Assert.Contains(\u0022SELECT TOP(1) [snapshot0].[LoadTimestamp]\u0022, insertCommandText, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: Assert.Contains(\u0022SELECT TOP(1) [snapshot1].[LoadTimestamp]\u0022, insertCommandText, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: Assert.Contains(\u0022ORDER BY [snapshot0].[LoadTimestamp] DESC\u0022, insertCommandText, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: Assert.Contains(\u0022ORDER BY [snapshot1].[LoadTimestamp] DESC\u0022, insertCommandText, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: var staleTimestamp = Utc(2026, 5, 4, 9, 30);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: var contactTimestamp = Utc(2026, 5, 4, 10, 0);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: var profileTimestamp = Utc(2026, 5, 4, 10, 30);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: AddPitRow(context, customerHashKey, staleTimestamp, contactTimestamp: null, profileTimestamp: null);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: AddContactRow(context, customerHashKey, contactTimestamp);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: AddProfileRow(context, customerHashKey, profileTimestamp);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: row =\u003E AssertPitRow(row, customerHashKey, contactTimestamp, contactTimestamp, profileTimestamp: null),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: row =\u003E AssertPitRow(row, customerHashKey, profileTimestamp, contactTimestamp, profileTimestamp));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: var unsavedPitTimestamp = Utc(2026, 5, 4, 9, 45);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0027: AddPitRow(context, customerHashKey, unsavedPitTimestamp, contactTimestamp: null, profileTimestamp: null);",
    "Committed branch delta contains 1 inspectable repository path(s): Modified: tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: C:\\Projects\\DVault\\examples\\DCoding.Data.DVault.SqliteQuickstart\\DCoding.Data.DVault.SqliteQuickstart.csproj : warning NU1903: Package \u0027SQLitePCLRaw.lib.e_sqlite3\u0027 2.1.11 has a known high severity vulnerability, https://github.com/advisories/GHSA-2m69-gcr7-jv3q [C:\\Projects\\DVault\\DVault.slnx]",
    "Observed stdout: Restored C:\\Projects\\DVault\\tests\\DCoding.Data.DVault.Tests\\Unit\\DCoding.Data.DVault.Tests.Unit.csproj (in 186 ms).",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 701 C# files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/provider-support, area/read-models, area/tests, automation/bot-ready, needs-test, provider/postgres, provider/sqlserver, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa\u0027.",
    "Ticket history references implementation commit \u0027c2183b2c30a6\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using branch ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa at verified commit c2183b2c30a6."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FF43HQ8E0435ZZSRZQQJW1HC`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa' at commit 'c2183b2c30a6'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa`
- implementation-commit: `c2183b2c30a6`
- implementation-pr: `<none>`
- implementation-change: `<none>`