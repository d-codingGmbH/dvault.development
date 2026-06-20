<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the MySQL latest-satellite ticket as evidence-backed tuning work: current 2026-06-20 code/tests already prove strategy registration, gate behavior, and the window-function SQL shape, while measured MySQL latest-satellite timing still remains a gap; no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence on 2026-06-20 already proves MySQL latest-satellite strategy registration, gate behavior, SQL-shape coverage, and configured integration selection, so this ticket should refine evidence-backed tuning work rather than reopen basic provider strategy design.
- The checked-in v0.32.0 smoke-read artifact dated 2026-06-07 still matters as the last preserved configured MySQL latest-satellite benchmark lane, and it completed through provider-neutral fallback with `selectedStrategy=<none>` because provider-specific latest-satellite dispatch was not yet registered in that artifact baseline.
- The current root benchmark triplet keeps the MySQL latest-satellite row as a skipped placeholder when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset, with `selectedStrategy=MySqlDataVaultReadStrategy`, `plannedReadStrategy=MySqlDataVaultReadStrategy`, `readShape=LatestSatellite`, and `persistedOutcome=not executed`; that row is guidance, not measured timing evidence.
- MySQL PIT and bridge timing are already closed separately by the 2026-06-07 configured smoke-read bundle, so this ticket must not reuse PIT/bridge timing as proof for latest-satellite tuning.
- Live relation state still includes an incoming `blocks` link from done task `06FE4QP6FB892E7TJMB47A3MSR` and an incoming `relates` link from done story `06FE4QNWP9606HTB92MTVQMYDG`; treat them as historical routing context, while the outgoing `blocks` link to `06FE4QRMXVGJVA65ZR5MZ817K8` remains the active downstream documentation dependency.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Verify the current MySQL latest-satellite capability posture from repository code, tests, diagnostics-facing docs, benchmark artifacts, and live ticket relations.
- Tune or affirm MySQL latest-satellite strategy selection and SQL shape only for supported MySQL-provider hub-parent, non-multi-active current/as-of satellite reads.
- Require evidence-backed comparison against provider-neutral fallback before claiming any MySQL latest-satellite improvement.
- Keep diagnostics and benchmark tokens aligned so `selectedStrategy`, `plannedReadStrategy`, `readShape=LatestSatellite`, and fallback causes remain consistent across artifacts and docs.
- Preserve the downstream handoff to docs ticket `06FE4QRMXVGJVA65ZR5MZ817K8` once the MySQL latest-satellite tuning decision is settled.

### Scope Out
- Broadening latest-satellite support beyond MySQL-provider hub-parent, non-multi-active satellites or adding a new public read API.
- Using MySQL PIT/bridge completed timing or skipped root guidance rows as substitute proof of MySQL latest-satellite improvement.
- Automatic PIT/bridge maintenance, raw SQL advisor or physical-plan promises, and provider-specific physical design guarantees.
- Promoting measured MySQL latest-satellite timing in release-facing docs without a provider-configured artifact triplet and preserved run context.
- Implementing relation housekeeping or downstream documentation delivery beyond recording the current dependency state.

## Acceptance Criteria
- The ticket records the current MySQL latest-satellite runtime boundary: `AddDVaultMySql()` registers `MySqlDataVaultReadStrategy`, and optimized latest-satellite reads are only eligible for MySQL-provider hub-parent, non-multi-active current/as-of satellite requests; provider mismatch, link-parent satellites, or multi-active satellites fall back to provider-neutral reads.
- The ticket records the current MySQL SQL-shape baseline at planning level: latest-satellite reads use `ROW_NUMBER() OVER (PARTITION BY ParentHashKey ORDER BY LoadTimestamp DESC)` with parent-hash-key `IN` batching and an optional `LoadTimestamp <= asOf` predicate before selecting one row per parent.
- Any tuning change keeps deterministic row-selection semantics and preserves diagnostics visibility so supported runs can still explain why `MySqlDataVaultReadStrategy` was selected or why provider-neutral fallback was used.
- Any claim of MySQL latest-satellite improvement is backed by provider-configured evidence with preserved run context; the skipped 2026-06-20 root placeholder row and the historical 2026-06-07 configured fallback row cannot be promoted into completed timing evidence by themselves.
- The ticket keeps MySQL PIT/bridge completed timing separate from latest-satellite tuning and does not widen supported shapes, provider set, or public fallback behavior.
- If evidence does not show a bounded MySQL latest-satellite win, keeping the existing window-function path and documenting the deferral is an acceptable outcome.

## Definition of Done
- A reviewer can tell whether the outcome is 'keep current MySQL latest-satellite shape' or 'apply tuned MySQL latest-satellite shape' without reopening architecture or scope questions.
- Code, tests, benchmark evidence, and docs all agree that MySQL latest-satellite remains a bounded evidence-gap/tuning lane separate from MySQL PIT/bridge timing.
- Supported current/as-of MySQL latest-satellite reads preserve provider-neutral result semantics and explicit fallback behavior after any tuning change.
- Downstream docs ticket `06FE4QRMXVGJVA65ZR5MZ817K8` has a clear handoff on whether to document a new measured MySQL latest-satellite claim or keep the current deferral language.
- No surface produced by this ticket implies completed MySQL latest-satellite timing without configured evidence.

## Implementation Notes
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs registers `MySqlDataVaultReadStrategy` for `IDataVaultProviderReadStrategy`, `IDataVaultProviderPitReadStrategy`, and `IDataVaultProviderBridgeReadStrategy`.
- src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs delegates latest/PIT/bridge eligibility to `DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(...)` and reuses the relational raw-read implementation.
- src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs and tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs lock the current MySQL latest-satellite SQL shape to a `ROW_NUMBER()` window over parent hash key ordered by descending load timestamp, with an optional as-of cutoff parameter.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs proves the latest-satellite gate accepts both `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`, and fails closed with `ProviderNameMismatch`, `UnsupportedSatelliteParent`, and `MultiActiveSatelliteUnsupported`.
- tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs proves a configured MySQL context selects `MySqlDataVaultReadStrategy` for latest and as-of reads and returns the expected rows.
- docs/plans/provider-optimization-gap-matrix.md P0.03, docs/plans/provider-optimization-evidence-matrix.md, docs/performance-profiles.md, and docs/releases/v0.42.0.md already separate current MySQL strategy registration and SQL-shape evidence from missing completed MySQL latest-satellite timing.
- The current root `benchmark-summary.*` row for MySQL latest-satellite is a skipped placeholder when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset, while the preserved `v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607` artifact records latest-satellite provider-neutral fallback but MySQL PIT/bridge provider-strategy selection.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- After any MySQL latest-satellite SQL-shape change, should a later configured evidence pass exercise both `MySql.EntityFrameworkCore` and `Pomelo.EntityFrameworkCore.MySql`, or is one configured provider lane sufficient because the strategy implementation is shared?

## Risks
- The current 2026-06-20 root MySQL latest-satellite row is intentionally skipped; if downstream work treats that guidance row as measured timing, release or performance docs will overclaim MySQL behavior.
- The 2026-06-07 configured smoke-read bundle proves MySQL PIT/bridge timing but latest-satellite provider-neutral fallback; mixing those lanes would misstate latest-satellite evidence.
- Configured latest-satellite integration evidence currently exercises `MySql.EntityFrameworkCore`; if the SQL shape changes, the shared implementation still needs to remain safe for `Pomelo.EntityFrameworkCore.MySql`, which is currently covered by gate and SQL-shape tests rather than a checked-in configured benchmark lane.
- Until the stale incoming `blocks` relation from done task `06FE4QP6FB892E7TJMB47A3MSR` is cleaned up, relation-driven automation or human readers may misread current dependency state.

## Split Recommendations
- No additional split is justified. The shared latest-satellite lane normalization is already done in `06FE4QP6FB892E7TJMB47A3MSR`, sibling provider follow-ups already exist for PostgreSQL `06FE4QPR8TF8R6PXNM3RMXN8JG`, SQL Server `06FE4QQ0YTHD7624MGVPKKK1C0`, and Oracle `06FE4QQJCJH7J9AWQTPDR5DSSG`, and downstream docs work already sits in `06FE4QRMXVGJVA65ZR5MZ817K8`.
- If a later workflow wants relation cleanup, handle the stale incoming `blocks` edge from done ticket `06FE4QP6FB892E7TJMB47A3MSR` as housekeeping rather than as a new child split.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: use normalized evidence to tune MySQL latest-satellite strategy selection or SQL shape where justified. Acceptance: fallback remains available and docs/diagnostics explain the chosen path.

<!-- gicket-bot:developer-delivery-supplement:v1:start -->
## Developer Delivery Supplement

### Outcome
- Decision: keep the current MySQL latest-satellite window-function shape; no SQL-shape tuning change was applied.
- The branch records the scoped MySQL completed optimized timing baseline from `artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.*` with `selectedStrategy=MySqlDataVaultReadStrategy`, `plannedReadStrategy=MySqlDataVaultReadStrategy`, and `readShape=LatestSatellite`.
- The branch explicitly does not claim a MySQL latest-satellite improvement over provider-neutral fallback, because the checked-in bundle does not include a matching MySQL provider-neutral latest-satellite comparator row.

### Repository Updates
- `docs/plans/provider-optimization-evidence-matrix.md` now promotes the MySQL `latest-satellite-read` row to scoped `completed-timing` for the ticket bundle and keeps PostgreSQL, SQL Server, Oracle, and DB2 latest-satellite rows in their existing guidance or evidence-gap lanes.
- `docs/plans/provider-optimization-gap-matrix.md` now closes P0.03 as a MySQL completed optimized timing baseline while preserving the fallback and future-comparator boundary.
- `docs/performance-profiles.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md`, `docs/production-adoption-checklist.md`, and `docs/releases/v0.42.0.md` now cite the MySQL ticket bundle consistently and keep PIT/bridge evidence separate from latest-satellite evidence.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` now verifies the checked-in MySQL latest-satellite row, the matrix/gap wording, and the no-improvement-claim language.

### Verification
- `bash tools/check-format.sh`: passed.
- `timeout 1800 dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --no-restore --filter "FullyQualifiedName~MySqlLatestSatelliteEvidenceArtifactRecordsOptimizedReadSelectionWithoutImprovementClaim"`: passed. Microsoft.Testing.Platform ignored the VSTest filter and ran the integration project for both targets; net8.0 reported 222 total, 0 failed, 197 succeeded, 25 skipped, and net10.0 reported 244 total, 0 failed, 219 succeeded, 25 skipped. External-provider skips were due to missing local `DVAULT_TEST_*` connection strings.
- `dotnet build DVault.slnx --nologo` was interrupted after more than 20 minutes when it stopped progressing after provider outputs.
- `timeout 1800 dotnet build DVault.slnx --nologo --no-restore` timed out after 30 minutes after compiling most projects. The observed warning was the existing `SQLitePCLRaw.lib.e_sqlite3` NU1903 advisory from the SQLite quickstart project.

### Downstream Handoff
- Downstream documentation may cite the ticket bundle as completed MySQL optimized latest-satellite timing for the current supported shape.
- Downstream documentation must not cite this ticket as a provider-neutral fallback improvement win unless a later bundle adds a matching comparator row or another explicit comparator.
<!-- gicket-bot:developer-delivery-supplement:v1:end -->