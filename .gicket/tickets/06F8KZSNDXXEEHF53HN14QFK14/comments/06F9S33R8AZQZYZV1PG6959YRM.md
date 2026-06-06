[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with\u0027 at commit \u0027975b9e47f6f1\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with",
    "commitSha": "975b9e47f6f1",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The checked-in example reuses the existing quickstart surface and demonstrates one realistic customer-profile history flow end to end with explicit saves and typed latest/as-of reads.",
      "satisfied": true,
      "reason": "The verified branch reuses the existing shared quickstart surface, and QuickstartHistoryFlow.cs implements one Customer hub plus one CustomerProfile satellite with explicit IDataVaultSaveService SaveAsync requests and typed latest/as-of IDataVaultReadService reads."
    },
    {
      "expectation": "The runnable SQLite path remains the default proof, and the docs make clear that PostgreSQL reuses the same flow behind the existing environment-variable gate.",
      "satisfied": true,
      "reason": "examples/README.md identifies SQLite as the default proof path and documents PostgreSQL as the same shared flow behind DVAULT_TEST_POSTGRES_CONNECTION_STRING, while persisted verification records a successful SQLite run and a clean PostgreSQL skip when the variable is unset."
    },
    {
      "expectation": "The example code or accompanying README shows how to inspect at least one bounded DVault diagnostics surface for the scenario without exposing raw SQL, request keys, connection strings, business keys, hash keys, payload values, or provider message text.",
      "satisfied": true,
      "reason": "QuickstartHistoryFlow.cs inspects IDataVaultDiagnosticsService and IDataVaultReadDiagnosticsService and prints only strategy status, selected strategy, read shape, and fallback presence, and examples/README.md explicitly states that raw SQL, keys, payload values, connection strings, and provider messages are not exposed."
    },
    {
      "expectation": "\u0060examples/README.md\u0060 explains what the scenario demonstrates, how to run it, and the v0.31 guardrails/non-goals: no hosted observability stack, no automatic PIT/bridge maintenance or orchestration, and no new runtime routing promises.",
      "satisfied": true,
      "reason": "examples/README.md explains the shared scenario, build and run steps, the SQLite-first and PostgreSQL-optional provider story, and the v0.31 guardrails that exclude hosted observability stacks, automatic PIT or bridge maintenance, orchestration, and new runtime routing promises."
    },
    {
      "expectation": "All changes stay within the existing examples/documentation boundary and remain compatible with the repository\u0027s normal build/test expectations without committing generated runtime artifacts.",
      "satisfied": true,
      "reason": "The inspected branch delta is limited to examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs and examples/README.md, tester verification succeeded for dotnet test DVault.slnx --nologo and bash tools/check-format.sh, and no generated runtime artifacts appear in the committed delta."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "\u0060dotnet build DVault.slnx --nologo\u0060 passes after the example changes.",
      "satisfied": true,
      "reason": "Persisted developer-delivery verification records dotnet build DVault.slnx --nologo passing on the verified commit, and the tester reran dotnet test DVault.slnx --nologo successfully afterward."
    },
    {
      "expectation": "The SQLite quickstart run path remains executable, and the PostgreSQL quickstart still skips cleanly when \u0060DVAULT_TEST_POSTGRES_CONNECTION_STRING\u0060 is unset.",
      "satisfied": true,
      "reason": "The SQLite quickstart Program.cs remains directly runnable against the shared flow, the PostgreSQL quickstart Program.cs exits successfully when DVAULT_TEST_POSTGRES_CONNECTION_STRING is unset, and persisted verification records both behaviors passing."
    },
    {
      "expectation": "README/example wording stays aligned with the current repository terminology for \u0060AddDVault()\u0060, \u0060AddDVaultTelemetry()\u0060, \u0060IDataVaultSaveService\u0060, \u0060IDataVaultReadService\u0060, and the listener-driven \u0060DCoding.Data.DVault\u0060 ActivitySource.",
      "satisfied": true,
      "reason": "examples/README.md uses the current repository terms AddDVault(), AddDVaultTelemetry(), IDataVaultSaveService, IDataVaultReadService, and the listener-driven DCoding.Data.DVault ActivitySource, and the shared quickstart code uses the save/read service interfaces directly."
    },
    {
      "expectation": "No temporary databases, benchmark artifacts, support bundles, or other generated outputs from running the example are committed.",
      "satisfied": true,
      "reason": "The verified commit modifies only the shared quickstart file and examples/README.md, so no temporary databases, benchmark artifacts, support bundles, or other generated outputs were committed."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027975b9e47f6f1\u0027 on branch \u0027ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with\u0027.",
    "Committed repository path \u0027docs/plans/customer-profile-comparison-contract.md\u0027 exists at verified commit \u0027975b9e47f6f1\u0027.",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: # Customer Profile Comparison Contract",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: Status: v1 shared comparison contract",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: Tickets: 06EXB7RYFJ3YQDB1E4QHPP8034, 06EXB7S6DB97GVVTS2GGZ3CCX8",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: ## Purpose",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: This artifact fixes one shared customer profile history sequence and the exact persisted-outcome assertions that the plain EF and DVault comparison tickets must use. It removes sce...",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: ## Shared Business Scenario",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: - load timestamp: \u00602026-04-29T10:15:00Z\u0060",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: - customer_status: \u0060prospect\u0060",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: - load timestamp: \u00602026-04-29T11:30:00Z\u0060",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: - customer_status: \u0060active\u0060",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: The plain EF baseline uses ordinary EF Core entities and SQLite persistence. Table names and CLR type names may follow normal EF conventions, but the asserted stored history for th...",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: - exactly 2 customer profile satellite rows for that hub, ordered by load timestamp ascending",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: - satellite row 1 stores \u0060customer_name = Alice Adams\u0060, \u0060customer_status = prospect\u0060, \u0060load_timestamp = 2026-04-29T10:15:00Z\u0060, \u0060record_source = crm-import\u0060",
    "Observed committed repository file \u0027docs/plans/customer-profile-comparison-contract.md\u0027: - satellite row 2 stores \u0060customer_name = Alice Baker\u0060, \u0060customer_status = active\u0060, \u0060load_timestamp = 2026-04-29T11:30:00Z\u0060, \u0060record_source = crm-change\u0060",
    "Committed repository path \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027 exists at verified commit \u0027975b9e47f6f1\u0027.",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: namespace DCoding.Data.DVault.Quickstarts.Shared;",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: public sealed class QuickstartVaultContext(DbContextOptions\u003CQuickstartVaultContext\u003E options) : DbContext(options) {",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: private static readonly DateTimeOffset InitialLoadTimestamp = new(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: private static readonly DateTimeOffset ChangedLoadTimestamp = new(2026, 4, 29, 11, 30, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: InitialLoadTimestamp,",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: ChangedLoadTimestamp,",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: InitialLoadTimestamp);",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: \u0022Load timestamps: \u0022 \u002B",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: InitialLoadTimestamp.ToString(\u0022O\u0022, CultureInfo.InvariantCulture) \u002B",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: ChangedLoadTimestamp.ToString(\u0022O\u0022, CultureInfo.InvariantCulture));",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: \u0022, load timestamp=\u0022 \u002B",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: latest.LoadTimestamp.ToString(\u0022O\u0022, CultureInfo.InvariantCulture));",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: asOf.LoadTimestamp.ToString(\u0022O\u0022, CultureInfo.InvariantCulture));",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: row.RequiredDateTimeOffset(\u0022LoadTimestamp\u0022),",
    "Observed committed repository file \u0027examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs\u0027: DateTimeOffset LoadTimestamp,",
    "Committed repository path \u0027examples/README.md\u0027 exists at verified commit \u0027975b9e47f6f1\u0027.",
    "Observed committed repository file \u0027examples/README.md\u0027: # DVault Quickstart Examples",
    "Observed committed repository file \u0027examples/README.md\u0027: These examples run the same bounded customer-profile history flow through the public registry-backed metadata path:",
    "Observed committed repository file \u0027examples/README.md\u0027: - \u0060DCoding.Data.DVault.SqliteQuickstart\u0060 uses SQLite through \u0060AddDVaultSqlite()\u0060 and needs no external infrastructure.",
    "Observed committed repository file \u0027examples/README.md\u0027: - \u0060DCoding.Data.DVault.PostgresQuickstart\u0060 uses PostgreSQL through \u0060AddDVaultPostgres()\u0060 and a developer-managed connection string.",
    "Observed committed repository file \u0027examples/README.md\u0027: Both projects register one shared \u0060DataVaultMetadataModel\u0060 with \u0060AddDVault(options =\u003E options.UseMetadataModel(...))\u0060, opt the DbContext into that registry with \u0060UseDataVaultMetada...",
    "Observed committed repository file \u0027examples/README.md\u0027: The checked-in examples use project references so they can build against the current repository checkout. Published consumer applications should install the same coordinated NuGet ...",
    "Observed committed repository file \u0027examples/README.md\u0027: The SQLite quickstart creates a temporary SQLite database file, creates the DVault schema, writes one customer profile twice with distinct load timestamps and record sources, then ...",
    "Observed committed repository file \u0027examples/README.md\u0027: - the first request saves the \u0060Customer\u0060 hub with the CRM import UTC load timestamp and \u0060crm-import\u0060 record source;",
    "Observed committed repository file \u0027examples/README.md\u0027: - the third request saves the changed \u0060CustomerProfile\u0060 satellite version with the later UTC load timestamp and \u0060crm-change\u0060 record source;",
    "Observed committed repository file \u0027examples/README.md\u0027: \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060 is supported as an optional metadata-only path for applications that already track generated DVault rows themselves. It fills miss...",
    "Observed committed repository file \u0027examples/README.md\u0027: The authoritative ActivitySource, span, event, tag, sampling, omission, and redaction rules live in [DVault V1 Activity Tracing Contract](../docs/architecture/dvault-v1-activity-tr...",
    "Observed committed repository file \u0027examples/README.md\u0027: If \u0060DVAULT_TEST_POSTGRES_CONNECTION_STRING\u0060 is missing or empty, the PostgreSQL quickstart exits successfully before opening a database connection and prints:",
    "Observed committed repository file \u0027examples/README.md\u0027: - Use model-first governance when a reviewed \u0060dvault.model.v1\u0060 JSON artifact should be imported, projected into EF metadata, exported canonically, and compared against generated me...",
    "Observed committed repository file \u0027examples/README.md\u0027: Choose one authoritative declaration path for each model boundary. Do not mix multiple metadata authorities for the same EF model. The runnable quickstarts stay metadata-first; the...",
    "Observed committed repository file \u0027examples/README.md\u0027: Use the v1 design-time workflow for production migration guardrails. It includes the GitHub Actions baseline for pre-integration checks, and the reusable command host is invoked fr...",
    "Observed committed repository file \u0027examples/README.md\u0027: The drift command uses a committed reviewed artifact when one exists. \u0060export\u0060 is for artifact maintenance or reviewed refresh workflows, not the default blocking CI gate.",
    "Observed committed repository file \u0027examples/README.md\u0027: For model-first or metadata-first review evidence, compare the reviewed artifact or metadata model against generated EF metadata with \u0060DataVaultModelDriftReporter.Compare(...)\u0060.",
    "Observed committed repository file \u0027examples/README.md\u0027: Live-schema drift evidence is intentionally bounded. \u0060DataVaultLiveSchemaReader.ReadAsync(context)\u0060 and \u0060DataVaultLiveSchemaDriftReporter.Compare(...)\u0060 provide built-in reader cove...",
    "Observed committed repository file \u0027examples/README.md\u0027: See [DVault Dotnet EF Design-Time Workflow](../docs/architecture/dvault-dotnet-ef-design-time-workflow.md), [Model-First Governance Workflow](../docs/model-first-governance.md), an...",
    "Committed branch delta contains 2 inspectable repository path(s): Modified: examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs, Modified: examples/README.md.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 222 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/developer-experience, area/documentation, area/ef-core, area/examples, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with\u0027.",
    "Ticket history references implementation commit \u0027975b9e47f6f1\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using branch ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with at commit 975b9e47f6f1."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F8KZSNDXXEEHF53HN14QFK14`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with' at commit '975b9e47f6f1'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F8KZSNDXXEEHF53HN14QFK14-task-add-realistic-ef-core-sample-scenarios-with`
- implementation-commit: `975b9e47f6f1`
- implementation-pr: `<none>`
- implementation-change: `<none>`