[gicket-bot] PO-critic review contract

Summary
- Clear pre-development story anchored to the existing benchmark contract, harness, tests, and provider strategy surface; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 10 comments for 06F492CTREZEDXVKJ839YGCPWW and the returned bodies are all gicket-bot automation comments; no human comment or attachment evidence changed the scope.
- docs/plans/performance-evidence-benchmark-artifact-contract.md defines the shared benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json contract, the required run-context fields, the required row fields, the optional provider matrix, SQL-capture layout, and regression-budget rules.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs and benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs already implement the synchronized artifact pipeline and BenchmarkSummary row shape, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs asserts 32 result rows plus 4 optionalProviders entries.
- benchmark-summary.md lines 57-64 and benchmark-summary.csv lines 26-33 already show provider-native-bulk-ingestion rows for PostgreSQL, SQL Server, MySQL, and Oracle with normalized skipped reasons instead of omission.
- Direct provider compatibility evidence exists in src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs, src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs, and src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs; benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs maps those strategies into benchmark baselines.
- benchmarks/DCoding.Data.DVault.Benchmarks/ProviderNativeBulkIngestionBenchmark.cs already calls IDataVaultDiagnosticsService.Analyze(context, scenario.Request) and asserts provider strategy selection before timing the optimized row, giving a concrete source-backed audit hook for the requested optimization evidence.
- git rev-parse HEAD resolved to 72b3eb3b1a02edf2eca5f372635f89b6363d7fbf, git diff --stat 72b3eb3b1a02edf2eca5f372635f89b6363d7fbf..HEAD was empty, and git show --stat --summary HEAD shows only po-critic lease-claim changes under .gicket/tickets/06F492CTREZEDXVKJ839YGCPWW/, so the branch is still in pre-development ticket state.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete example currently shows what the non-SQL execution-detail fallback should look like when raw SQL is too unstable to persist.
- The contract does not yet illustrate how execution detail should be recorded for a failed optimized row versus a skipped provider row within the before/after label.

Risky assumptions
- Assumes the shared artifact contract can be extended for execution-detail evidence without breaking downstream consumers, even though current BenchmarkSummary and benchmark-summary outputs only carry the existing row fields.
- Assumes a diagnostics-derived detail string is acceptable audit evidence when generated SQL is not practical to persist; the ticket leaves the minimum content of that detail implicit.

AC / test suggestions
- Keep one objective acceptance example for a SQL-backed execution-detail artifact and one non-SQL-backed execution-detail artifact so review does not depend on ad hoc interpretation.
- Require before/after artifact validation to fail when an optimized provider row is present but its execution-detail evidence is missing or empty.

Implementation watchouts
- Execution-detail changes need one consistent contract across BenchmarkSummary in BenchmarkRunner.cs, all three writers in BenchmarkArtifacts.cs, and BenchmarkScenarioExecutionTests.cs.
- The repository root sample currently shows single-run benchmark-summary files, while this story asks for labeled before/after persistence; comparability metadata must survive that move.
- Optional external-provider rows must stay visible as skipped with normalized reasons; current docs, tests, and root artifacts already depend on that behavior.

Non-blocking notes
- The persisted delivery contract says the story is currently blocked by 06F492CAB2293R7BGJWMWMRKT4, 06F492CFSJHN0RGXXRG3KT63FM, and 06F492CN76GS3CKM8EFD0C20XM; that is a sequencing issue, not a PO-clarity defect.

Split recommendations
- No split needed; the existing contract and repository evidence already bound this as one benchmark-contract extension story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment