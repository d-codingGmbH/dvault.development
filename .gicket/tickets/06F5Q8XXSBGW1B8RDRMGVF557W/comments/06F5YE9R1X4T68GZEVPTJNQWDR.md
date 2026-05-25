[gicket-bot] PO-critic review contract

Summary
- Source-backed delivery contract, no open questions, and clear benchmark artifact boundaries; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q8XXSBGW1B8RDRMGVF557W/description.md:8,48-49` marks the ticket `ready_for_po_critic` and shows `## Open Questions` as `- none`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:45,528,561` already exposes `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...)`, `DataVaultChunkedSaveRequest`, and `DataVaultSaveChunk`.
- `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs:131-135,212-232` already carries chunk diagnostics including `ChunkCount`, `ProcessedChunkCount`, `RetainedStateHighWaterCount`, and `ChunkedStateFallbackCauseKinds`.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:<redacted>` contains chunked-save integration coverage for empty chunks, multi-chunk ordering, cancellation, transaction participation, repeated-row reuse, and retained-state behavior.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:19,313,362,378-382` locks the current benchmark artifact schema/header, expects 32 result rows, and requires non-empty `executionDetail` values.
- `benchmark-summary.md:5-64` and `benchmark-summary.json:46-635` show the current checked-in benchmark evidence covers 32 baseline rows across existing scenarios and optional-provider skipped rows; no chunked-save benchmark row is present yet, which matches this being a pre-development story.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md:16-27,75,91,106,110` already defines the artifact trio, before/after storage, skipped-row semantics, `executionDetail` requirements, and regression budgets the story must reuse.
- `git show --stat --oneline --summary bc97b4018` touched only `.gicket/...` ticket files, and `git diff --name-only 765f50f61984ed0fbcd5b1d2af08f014af9e5797..HEAD -- . ':(exclude).gicket/**'` returned no non-ticket paths, so the current branch state is still PO refinement rather than implementation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not pin exact chunk sizes or chunk counts for the benchmark rows; implementation should choose bounded values and keep input parity with the materialized baseline.
- The contract does not say whether empty-chunk no-op behavior needs a benchmark row or remains semantic baseline only; current integration tests already cover the behavior.

Risky assumptions
- Assumes a SQLite-focused chunked-save comparison is sufficient for v1 because scope explicitly excludes mandatory provider-specific chunk matrices.
- Assumes chunk boundary visibility can be expressed through current `executionDetail` or existing metadata fields without changing the artifact schema.
- Assumes the lingering `blocks` relation records are historical routing state because the two upstream tickets are already marked done.

AC / test suggestions
- Add at least one SQLite row pair that compares chunked-save against an equivalent ordered materialized explicit-save input built from the same logical requests.
- Update row-count-sensitive benchmark tests in `BenchmarkScenarioExecutionTests.cs` when chunked rows are added, while preserving artifact header and skipped-row assertions.
- Assert that completed chunked rows record timing and allocation metrics and that `executionDetail` includes chunk boundary and exercised save-path evidence.
- If any release-facing performance claim is made, check in before/after artifact bundles under one explicit benchmark label.

Implementation watchouts
- `BenchmarkScenarioExecutionTests.cs:362` currently expects exactly 32 results, so new chunked rows will require synchronized test updates.
- `README.md:769-777` and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` currently describe the existing scenario matrix only; developer work must update prose so the chunked benchmark path is discoverable.
- Optional-provider skipped semantics are contractual in `docs/plans/performance-evidence-benchmark-artifact-contract.md` and cannot be dropped when adding SQLite chunked rows.
- Keep the work anchored to the existing chunked-save API and telemetry surface instead of reopening API design or artifact-schema design.

Non-blocking notes
- The branch currently contains no non-`.gicket` changes relative to its `develop` merge-base, which is consistent with this PO-critic gate happening before development.
- Root benchmark artifacts already keep PostgreSQL, SQL Server, MySQL, and Oracle rows visible as skipped, which matches the story scope and shared artifact contract.

Split recommendations
- No split is needed while the work stays limited to SQLite chunked-save evidence, existing artifact files, and benchmark/docs updates.
- If implementation expands into provider-specific chunk optimizations, new public API surface, or a broader chunk matrix, open a follow-up ticket instead.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment