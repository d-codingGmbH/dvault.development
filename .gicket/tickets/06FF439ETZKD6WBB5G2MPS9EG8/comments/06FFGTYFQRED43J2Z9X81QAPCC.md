[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 4/4 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi\u0027 at commit \u00277d3331faf212\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi",
    "commitSha": "7d3331faf212",
    "pullRequestReference": null,
    "changeReference": null,
    "branchOwnerProvenance": {
      "ticketId": "06FF439ETZKD6WBB5G2MPS9EG8",
      "ownerBranch": "ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi",
      "sourceCommitSha": "7d3331faf212",
      "baseBranch": "develop",
      "producingRole": "test",
      "producingRunId": "6bd1a2dc36cb4df9b1608c1b6368b845",
      "producingInstanceId": "hp-ai-2026-001.1"
    }
  },
  "acceptanceCriteria": [
    {
      "expectation": "Performance guidance explicitly distinguishes completed read timing evidence from PIT maintenance prototype availability and states that maintained read-model rows are a prerequisite, not part of the measured read timing.",
      "satisfied": true,
      "reason": "Satisfied: \u0060docs/performance-profiles.md:24,225,471,505\u0060 explicitly separates PIT/bridge read timing from maintenance evidence, states maintained read-model rows are prerequisites, and says maintenance work is not part of the measured timing."
    },
    {
      "expectation": "Architecture guidance explicitly states that PIT and bridge reads consume already-maintained rows, and provider read strategy selection or completed read timings do not imply provider-side maintenance execution.",
      "satisfied": true,
      "reason": "Satisfied: \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md:14,34,128\u0060 says PIT and bridge reads consume already-maintained rows, treats completed PIT/bridge timing as read-side evidence only, and rejects provider-side maintenance inference."
    },
    {
      "expectation": "Updated docs cite the existing maintenance evidence contract for PIT maintenance and preserve bridge maintenance push-down as deferred.",
      "satisfied": true,
      "reason": "Satisfied: \u0060docs/performance-profiles.md:24,71,225\u0060 cites the existing v0.45.0 release notes and architecture boundary as the maintenance contract, and \u0060docs/plans/provider-optimization-gap-matrix.md:15\u0060 preserves bridge maintenance push-down as deferred."
    },
    {
      "expectation": "No updated text promotes PIT or bridge read evidence into write-side maintenance timing, maintenance implementation proof, or maintenance push-down approval.",
      "satisfied": true,
      "reason": "Satisfied: \u0060docs/performance-profiles.md:225,505\u0060, \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md:34,76\u0060, and \u0060docs/plans/provider-optimization-gap-matrix.md:15\u0060 explicitly say the read evidence does not prove maintenance timing, provider SQL maintenance execution, automatic refresh, or push-down approval."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "docs/performance-profiles.md and docs/architecture/dvault-v1-pit-bridge-boundary.md use aligned read-versus-maintenance evidence language.",
      "satisfied": true,
      "reason": "Satisfied: the two required output docs use aligned read-versus-maintenance language across \u0060docs/performance-profiles.md:24,71,225,471,505\u0060 and \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md:14,34,128\u0060."
    },
    {
      "expectation": "Any added citations point only to already-checked-in repository evidence such as the v0.45.0 PIT maintenance baseline, the Provider Optimization Evidence Matrix, and the existing architecture boundary.",
      "satisfied": true,
      "reason": "Satisfied: the added citations in \u0060docs/performance-profiles.md:24,71,225\u0060 stay within checked-in repository evidence (\u0060docs/releases/v0.45.0.md\u0060 and \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md\u0060), and the architecture note continues to use checked-in \u0060docs/plans/provider-optimization-evidence-matrix.md\u0060 as the canonical read-row lookup."
    },
    {
      "expectation": "No updated document claims benchmark-backed maintenance timing without a preserved maintenance-specific artifact lane.",
      "satisfied": true,
      "reason": "Satisfied: the updated text keeps benchmark-backed maintenance timing unclaimed by stating the v0.45.0 PIT maintenance prototypes are source/test evidence only and by forbidding use of PIT/bridge read rows as maintenance throughput proof."
    },
    {
      "expectation": "Bridge maintenance push-down remains explicitly deferred and tied to a future separate evidence, diagnostics, parity, and benchmark lane.",
      "satisfied": true,
      "reason": "Satisfied: bridge maintenance push-down remains explicitly deferred, with future work tied to separate evidence, diagnostics, parity coverage, and preserved benchmark lanes in \u0060docs/performance-profiles.md:82,505\u0060, \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md:76\u0060, and \u0060docs/plans/provider-optimization-gap-matrix.md:15\u0060."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u00277d3331faf212\u0027 on branch \u0027ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi\u0027.",
    "Committed repository path \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027 exists at verified commit \u00277d3331faf212\u0027.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: # DVault V1 PIT And Bridge Boundary",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Status: v1 implementation note",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Ticket: 06F5Q91M0PM17RP43ZQRPBDXP0",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Current public baseline: [DVault v0.46.0 Release Notes](../releases/v0.46.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: DB2 provider package baseline: [DVault v0.34.0 Release Notes](../releases/v0.34.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Read-optimization expansion baseline: [DVault v0.28.0 Release Notes](../releases/v0.28.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: PIT reads target one \u0060DataVaultPitMetadata\u0060 declaration, explicit parent hash keys, and an \u0060asOf\u0060 timestamp. \u0060ReadPitRowsAsync(...)\u0060 returns raw \u0060DataVaultPitReadRecord\u0060 rows. \u0060Rea...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: PIT/bridge feature-introduction baseline: [DVault v0.21.0 Release Notes](../releases/v0.21.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: \u0060AddDVaultSqlite()\u0060, \u0060AddDVaultPostgres()\u0060, \u0060AddDVaultSqlServer()\u0060, \u0060AddDVaultMySql()\u0060, \u0060AddDVaultOracle()\u0060, and \u0060AddDVaultDb2()\u0060 register repository-proven diagnostics-gated optim...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: - PostgreSQL and SQL Server provider paths both stay request-gated. Provider-name mismatch, dirty tracked contexts, unsupported PIT shapes, incomplete provider evidence, and provid...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Ticket \u006006FF43CJ9CJMG7J917RW22QKJC\u0060 evaluated MySQL PIT maintenance push-down for \u0060IDataVaultPitMaintenanceService.RebuildAsync(...)\u0060 full rebuilds only. The current repository bas...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The feasible next implementation lane is a MySQL \u0060IDataVaultProviderPitMaintenanceStrategy\u0060, not a SQL Server-style service replacement. The existing strategy seam already dispatch...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The accepted implementation recommendation is deliberately narrow: implement and validate only clean, ordinary hub-parent MySQL full rebuilds first, using the live \u0060MySql.EntityFra...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Rollback is the decisive gate for any accepted MySQL lane. A provider strategy may preserve pre-rebuild PIT rows when it owns a local transaction and can roll back delete-plus-inse...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The runtime metadata path supports:",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The public \u0060dvault.model.v1\u0060 PIT artifact shape remains hub-parent-only and continues to use the \u0060hub\u0060 field. Runtime link-parent PIT maintenance and reads do not imply model-first...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Maintained-bridge read evidence proves provider read-strategy selection over already-maintained bridge rows. It does not prove write-side bridge-maintenance push-down value, SQL sh...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The reopen threshold is concrete hotspot evidence that provider-neutral bridge maintenance, not bridge reads, is a material bottleneck after the bounded PIT provider-maintenance pr...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The public read request contract is provider-neutral. \u0060AddDVaultSqlite()\u0060, \u0060AddDVaultPostgres()\u0060, \u0060AddDVaultSqlServer()\u0060, \u0060AddDVaultMySql()\u0060, \u0060AddDVaultOracle()\u0060, and \u0060AddDVaultDb2...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: \u0060IDataVaultReadDiagnosticsService\u0060 is the diagnostics boundary for read strategy and read-shape evidence. Request-bound diagnostics keep provider strategy selection in \u0060ReadStrateg...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: ## Evidence",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: - [DataVaultPitMaintenanceServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs) covers PIT rebuild, parent maintena...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: - [DataVaultProviderReadStrategyTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs) covers PostgreSQL, SQL Server, MySQL, Oracle, and DB2 la...",
    "Committed repository path \u0027docs/performance-profiles.md\u0027 exists at verified commit \u00277d3331faf212\u0027.",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: # Performance Profiles",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Status: v0.46.0 provider optimization closure baseline with carried-forward v0.45.0 PIT maintenance prototype documentation overlay",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: This guide is the detailed performance-profile reference for the current DVault performance-guidance baseline. It carries forward the v0.31.0 adopter decision tree, the v0.32.0 pro...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: ## Evidence Baseline",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use the root benchmark artifact triplet as the quick local SQLite and skipped-provider baseline for the row names and timing values in this guide:",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - [benchmark-summary.md](../benchmark-summary.md)",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The completed read rows in that bundle do not all have the same prerequisite. Latest-satellite rows time supported satellite reads over seeded satellite history. PIT and bridge row...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - [benchmark-summary.md](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.md)",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - [benchmark-summary.csv](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.csv)",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - [benchmark-summary.json](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.json)",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The v0.42.0 release validation also includes a one-iteration Windows-host to Podman-DB2 run over the same scoped lanes. Treat it as confirming evidence for the current local DB2 se...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use the provider binary-vs-hex hash-key matrix as a scoped provider-configured storage-profile evidence bundle. It ran one iteration and no warmup on Windows/.NET 10.0.9 with provi...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use the allocation hotspot artifacts as bounded DVault-owned allocation evidence. The first bundle records the hotspot baseline, and the comparative before/after bundle is the prim...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use [Provider Optimization Evidence Matrix](plans/provider-optimization-evidence-matrix.md) as the canonical lookup surface for provider optimization row identity, evidence posture...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use [Provider Optimization Gap Matrix](plans/provider-optimization-gap-matrix.md) as the canonical closure and follow-up boundary surface. Its P0-P3 rows now close PostgreSQL, SQL ...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The v0.45.0 PIT maintenance prototypes are source and test evidence, not benchmark-backed timing evidence. Do not cite the provider-maintenance work as a performance win unless a l...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: This section is the maintenance evidence contract for the performance guide. It complements [DVault v0.45.0 Release Notes](releases/v0.45.0.md) and [DVault V1 PIT And Bridge Bounda...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - MySQL: ticket \u006006FF43CJ9CJMG7J917RW22QKJC\u0060 evaluated full-rebuild feasibility and recommends a future \u0060IDataVaultProviderPitMaintenanceStrategy\u0060 lane only for clean ordinary hub-...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - Fallback and rollback remain part of the claim boundary. Unsupported or mismatched maintenance requests fall back through provider-neutral maintenance, and SQL Server full-rebuil...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: MySQL PIT maintenance claims must not cite the completed MySQL \u0060pit-as-of-read\u0060 row as maintenance proof. A future MySQL ordinary hub-parent implementation must prove provider-name...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Bridge maintenance push-down remains explicitly deferred. Existing \u0060bridge-traversal-read\u0060 timing rows are read-side evidence over already-maintained bridge rows; they are not evid...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: ## v0.43.0 Evidence And Tuning Boundary",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Root benchmark artifact triplet | Local SQLite completed timing rows, skipped optional-provider row identity, run context, and deterministic execution details. | Completed Postgr...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | 2026-06-23 provider optimization closure bundle | Current completed external-provider timing for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 provider-native save rows plus lat...",
    "Committed repository path \u0027docs/plans/provider-optimization-gap-matrix.md\u0027 exists at verified commit \u00277d3331faf212\u0027.",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: # Provider Optimization Gap Matrix",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: Status: v2 provider optimization closure matrix",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: Ticket: 06FBSC4HSXFJ5FM6GWECH2CTGG",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: ## Purpose",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: This document turns the current provider optimization evidence into a prioritized backlog matrix for later save and read strategy work. It uses [Provider Optimization Evidence Matr...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: The matrix now separates closed provider optimization gaps from ongoing runtime boundaries:",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - PostgreSQL \u0060latest-satellite-read\u0060 moved from capability-only closure to completed timing evidence in the 2026-06-23 provider optimization closure bundle. The completed row selec...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - PostgreSQL, SQL Server, MySQL, Oracle, and DB2 \u0060pit-as-of-read\u0060 and \u0060bridge-traversal-read\u0060 rows are closed by the closure bundle as read-side timing over already-maintained PIT/...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - Remaining boundaries are runtime fallback boundaries, not open optimization gaps: provider mismatch, dirty contexts for save work, unsupported latest-satellite shapes, stale or i...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - Canonical evidence rows: [Provider Optimization Evidence Matrix](provider-optimization-evidence-matrix.md).",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - MySQL latest-satellite provider-configured evidence for ticket \u006006FE4QQ9VF7B74E60CXEHSS5XW\u0060: [benchmark-summary.md](../../artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-la...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - v0.32.0 smoke-read provider-configured PIT/bridge evidence for PostgreSQL, SQL Server, MySQL, and Oracle: [benchmark-summary.md](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVA...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - SQL Server bulk-threshold provider-configured evidence: [sqlserver-threshold-decision.md](../../sqlserver-threshold-decision.md), [benchmark-summary.md](../../artifacts/benchmark...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - DB2 hotspot provider-configured evidence for clean-context save plus supported latest-satellite/PIT/bridge reads: [benchmark-summary.md](../../artifacts/benchmarks/06FE4QR3DD7EFZ...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - DB2 host-to-Podman release validation for the same scoped DB2 lanes: [benchmark-summary.md](../../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - Read optimization release baseline: [DVault v0.28.0 Release Notes](../releases/v0.28.0.md).",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - Provider save threshold and artifact-lane baseline: [DVault v0.32.0 Release Notes](../releases/v0.32.0.md).",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - DB2 release posture: [DVault v0.34.0 Release Notes](../releases/v0.34.0.md).",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: The v0.42 release gate uses this matrix as the deferred-work and stop-condition surface, not as a timing evidence source. A gap row may justify follow-up work only inside the provi...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: Apply these tuning boundaries when deciding whether a future change still fits the closed evidence:",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - Oracle save work keeps direct optimized batching at 50-plus total operations and no more than 10000 satellite operations; staged Oracle bulk remains unclaimed without new complet...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: - DB2 work stays limited to clean-context set-based save with the measured 1000-row command cap plus diagnostics-gated latest-satellite/PIT/bridge rows; staged DB2 bulk, provider-n...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: Provider-neutral fallback remains the public behavior whenever a required \u0060DVAULT_TEST_*\u0060 connection string is unset, provider diagnostics do not select the expected strategy, the ...",
    "Observed committed repository file \u0027docs/plans/provider-optimization-gap-matrix.md\u0027: | Scenario | Current baseline | Evidence posture | Measured evidence or comparator | Stop condition or fallback boundary | Sources |",
    "Committed branch delta contains 3 inspectable repository path(s): Modified: docs/architecture/dvault-v1-pit-bridge-boundary.md, Modified: docs/performance-profiles.md, Modified: docs/plans/provider-optimization-gap-matrix.md.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: C:\\Projects\\DVault\\examples\\DCoding.Data.DVault.SqliteQuickstart\\DCoding.Data.DVault.SqliteQuickstart.csproj : warning NU1903: Package \u0027SQLitePCLRaw.lib.e_sqlite3\u0027 2.1.11 has a known high severity vulnerability, https://github.com/advisories/GHSA-2m69-gcr7-jv3q [C:\\Projects\\DVault\\DVault.slnx]",
    "Observed stdout: All projects are up-to-date for restore.",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 701 C# files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/documentation, area/performance, area/read-models, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi\u0027.",
    "Ticket history references implementation commit \u00277d3331faf212\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator; tester gate passed on commit \u00607d3331faf212\u0060 with green \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 evidence."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FF439ETZKD6WBB5G2MPS9EG8`
- target-role: `integrator`
- verification-summary: Tester verified 4/4 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi' at commit '7d3331faf212'.
- acceptance-criteria: `4/4` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi`
- implementation-commit: `7d3331faf212`
- implementation-pr: `<none>`
- implementation-change: `<none>`