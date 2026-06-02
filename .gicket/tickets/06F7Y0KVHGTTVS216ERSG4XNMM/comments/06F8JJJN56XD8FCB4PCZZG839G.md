[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in\u0027 at commit \u0027de0963eab7e9\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in",
    "commitSha": "de0963eab7e9",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Consumers can evaluate an explicit idempotency preflight check against a configured \u0060DbContext\u0060 and receive deterministic pass/block/skip results for the relevant DVault operation families without opening a live database unless they opted into that lane.",
      "satisfied": true,
      "reason": "Satisfied by the additive \u0060DataVaultIdempotencyPreflight\u0060 surface, optional \u0060DataVaultPreflightRequest.IdempotencyLiveSchemaReadResult\u0060, skipped-lane behavior in \u0060DataVaultPreflight\u0060 when no live input is supplied, and passing tests that show explicit pass/skip evaluation without implicit live-database access."
    },
    {
      "expectation": "For every hub, link, satellite, PIT, and bridge present in the authoritative metadata, the check validates expected named primary-key constraints and idempotency/access-path secondary indexes for presence, column order, uniqueness, and provider-specific effective shape.",
      "satisfied": true,
      "reason": "Satisfied by provider-shaped expected-structure comparison in \u0060DataVaultIdempotencyPreflight\u0060 plus unit coverage for hub, link, satellite, PIT, and bridge baselines, including primary-key and secondary-index name, presence, column-order, uniqueness, descending-column, include-column, and redundant-index caveat checks."
    },
    {
      "expectation": "Missing or mismatched structures produce redacted provider-aware findings that identify the translated DVault table and explain the schema-level remediation boundary; valid structures report clean pass state.",
      "satisfied": true,
      "reason": "Satisfied by the structured \u0060DataVaultIdempotencyPreflightFinding\u0060 and report types and tests showing clean pass with empty findings, blocking findings for missing or mismatched structures, and redacted output that identifies table, operation family, structure, property path, and remediation boundary without leaking raw provider details."
    },
    {
      "expectation": "Requested live checks surface \u0060UnsupportedProvider\u0060 and \u0060Unavailable\u0060 as explicit provider-aware outcomes, while omitted live inputs remain skipped rather than auto-discovered or silently ignored.",
      "satisfied": true,
      "reason": "Satisfied by explicit \u0060Passed\u0060, \u0060Blocked\u0060, \u0060Skipped\u0060, \u0060UnsupportedProvider\u0060, and \u0060UnavailableLiveSchema\u0060 outcomes, \u0060DataVaultPreflight\u0060 skip behavior when live input is omitted, and tests covering unsupported and unavailable live-schema reads as explicit provider-aware results."
    },
    {
      "expectation": "Tests cover valid, missing, mismatched, and provider-caveat scenarios, including at least one default local SQLite path and targeted coverage for provider-specific included-index or redundant-index behavior.",
      "satisfied": true,
      "reason": "Satisfied by successful \u0060dotnet test DVault.slnx --nologo\u0060, \u0060SqliteIdempotencyPreflightTests\u0060 for the default local SQLite path, unit coverage for valid/missing/mismatched cases, and provider-caveat coverage for included-column and PK-covered redundant-index behavior."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The library exposes the additive idempotency-preflight result surface and wires it into the existing preflight flow only through explicit optional inputs or lane selection, preserving current caller behavior by default.",
      "satisfied": true,
      "reason": "Satisfied by the new idempotency preflight/report surface and the existing aggregate preflight wiring through the optional \u0060IdempotencyLiveSchemaReadResult\u0060 input only; default preflight behavior remains unchanged and skipped when that input is absent."
    },
    {
      "expectation": "Deterministic machine-readable and displayable output exists for pass, block, skip, unsupported-provider, and unavailable-live-schema cases, and the output remains within the existing redaction boundary.",
      "satisfied": true,
      "reason": "Satisfied by the machine-readable status/report/finding surface, deterministic \u0060ToDisplayString()\u0060 output, explicit pass/block/skip/unsupported/unavailable states, and redaction-tested handling that omits raw connection or provider-secret details."
    },
    {
      "expectation": "Unit and integration coverage lock the hub, link, satellite, PIT, and bridge baselines plus provider-shape caveats, and any required API snapshot or public-surface approvals are updated.",
      "satisfied": true,
      "reason": "Satisfied by passing \u0060dotnet test DVault.slnx --nologo\u0060, committed unit and SQLite integration coverage for the new lane, provider-discovery coverage, and branch-delta evidence that \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 was updated."
    },
    {
      "expectation": "Implementation reuses the current translated naming and provider-capability rules instead of hard-coding SQLite-only expectations or inventing a parallel index vocabulary.",
      "satisfied": true,
      "reason": "Satisfied by provider-capability-driven expected-structure creation and live-schema comparison, the \u0060DataVaultLiveSchemaIndex\u0060 and \u0060DataVaultLiveSchemaReader\u0060 metadata expansion for descending/include-column data, and tests proving provider-shape normalization instead of SQLite-only hard-coding."
    },
    {
      "expectation": "No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass, and the current epic/doc-task relation state remains unchanged.",
      "satisfied": true,
      "reason": "Satisfied semantically because the persisted supplemental description resolves the earlier wording conflict by scoping the \u0027no description updates\u0027 clause to the PO refinement activity; with that clarification, no child-ticket, attachment, planning-document, or applied relation-change evidence remains that blocks tester pass."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027de0963eab7e9\u0027 on branch \u0027ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in\u0027.",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: using Microsoft.EntityFrameworkCore.Infrastructure;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata.Conventions;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: providerCapabilities.WithLoadTimestampStorage(expectedImport.LoadTimestampStorage));",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs\u0027: .WithLoadTimestampStorage(expectedImport.LoadTimestampStorage);",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs\u0027: /// Machine-readable idempotency preflight finding scoped to one Data Vault table and operation family.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs\u0027: public sealed record DataVaultIdempotencyPreflightFinding(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs\u0027: DataVaultModelDriftSeverity Severity,",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs\u0027: /// Structured and displayable result for one explicit Data Vault idempotency schema preflight check.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs\u0027: /// \u003C/summary\u003E",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStatus.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStatus.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStatus.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStatus.cs\u0027: /// Status assigned to one explicit Data Vault idempotency preflight evaluation.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStatus.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStatus.cs\u0027: public enum DataVaultIdempotencyPreflightStatus {",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs\u0027: /// Expected provider-shaped schema structure used by Data Vault idempotency and bounded read operations.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs\u0027: public sealed record DataVaultIdempotencyPreflightStructure(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs\u0027: string TableName,",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs\u0027: /// Describes one live Data Vault table secondary index.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs\u0027: public sealed class DataVaultLiveSchemaIndex {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs\u0027: /// Initializes a new live secondary index description.",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027: using System.Data;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027: using System.Data.Common;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs\u0027: \u0022WHEN t.typname = \u0027timestamptz\u0027 THEN \u0027timestamp with time zone\u0027 \u0022 \u002B",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultPreflight.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflight.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflight.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflight.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflight.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflight.cs\u0027: /// Composes existing Data Vault diagnostics, drift, guardrail, and request-bound diagnostics into one preflight report.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflight.cs\u0027: /// \u003C/summary\u003E",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0027: /// Structured aggregate Data Vault preflight report with deterministic section status and preserved lane reports.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0027: /// \u003C/summary\u003E",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0027: /// \u003Csummary\u003E",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: using System.Reflection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Integration;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: public sealed class ProviderIntegrationCategoryDiscoveryTests {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: private static readonly Type[] RequiredLocalSqliteCoverageTypes = [",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0027: typeof(SqlServerBatchScriptTests),",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027 exists at verified commit \u0027de0963eab7e9\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Integration;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: structure.ColumnNames.SequenceEqual([\u0022CustomerHashKey\u0022, \u0022LoadTimestamp\u0022, \u0022HashDiff\u0022]) \u0026\u0026",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/SqliteIdempotencyPreflightTests.cs\u0027: structure.DescendingColumnNames.SequenceEqual([\u0022LoadTimestamp\u0022]));",
    "Committed branch delta contains 15 inspectable repository path(s): Added: src/DCoding.Data.DVault/DataVaultIdempotencyPreflight.cs, Added: src/DCoding.Data.DVault/DataVaultIdempotencyPreflightFinding.cs, Added: src/DCoding.Data.DVault/DataVaultIdempotencyPreflightReport.cs, Added: src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStatus.cs, Added: src/DCoding.Data.DVault/DataVaultIdempotencyPreflightStructure.cs, Modified: src/DCoding.Data.DVault/DataVaultLiveSchemaIndex.cs, Modified: src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs, Modified: src/DCoding.Data.DVault/DataVaultPreflight.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 214 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/provider-support, area/schema, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in\u0027.",
    "Ticket history references implementation commit \u0027de0963eab7e9\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to the integrator gate using branch \u0060ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in\u0060 at commit \u0060de0963eab7e9\u0060."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F7Y0KVHGTTVS216ERSG4XNMM`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in' at commit 'de0963eab7e9'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in`
- implementation-commit: `de0963eab7e9`
- implementation-pr: `<none>`
- implementation-change: `<none>`