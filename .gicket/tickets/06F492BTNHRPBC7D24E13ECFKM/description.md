<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the performance epic around the already-materialized seven-child breakdown and current repository evidence; no new split, attachment, planning document, or relation write was required.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the v1 performance baseline through docs/plans/performance-evidence-benchmark-artifact-contract.md, docs/architecture/dvault-ef-compiled-compatibility.md, docs/releases/v0.18.0.md, and the root benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet.
- Existing materialized children under this epic are 06F492BZPP5YT9SJSPDHQBGF3R (benchmark artifact contract), 06F492C50WM7V2NE0WZB3774XM (query-shape diagnostics), 06F492CAB2293R7BGJWMWMRKT4 (provider-neutral read allocations), 06F492CFSJHN0RGXXRG3KT63FM (explicit-save change-tracker overhead), 06F492CN76GS3CKM8EFD0C20XM (compiled model/query/pooling evidence), 06F492CTREZEDXVKJ839YGCPWW (provider optimization regression baselines), and 06F492D05THPGQVT3B3K7853A0 (v0.18.0 documentation rollup).
- No description update, relation mutation, attachment, child-ticket creation, or planning-document write was applied or queued in this run.

### Scope In
- Reuse one shared performance-evidence contract and benchmark artifact format across the whole epic.
- Bounded query-shape diagnostics that expose translated table identity, filter/order facts, projected columns, expected index baselines, and provider fallback caveats without acting as a raw-SQL advisor.
- Measured provider-neutral read allocation tuning for latest-satellite, PIT as-of, and bridge traversal reads.
- Measured explicit-save change-tracker and batching overhead tuning on the existing IDataVaultSaveService boundary.
- SQLite benchmark evidence for compiled model, compiled query, and DbContext pooling guidance with the documented fixed-model boundaries.
- Provider optimization regression baselines that keep PostgreSQL, SQL Server, MySQL, and Oracle lanes visible as completed or skipped.
- v0.18.0 documentation and release-note rollup that points readers at the checked-in evidence and manual publication boundary.

### Scope Out
- Dashboards, observability platforms, workload orchestration, or release automation.
- Raw-SQL advisor behavior, automatic index creation, provider physical-plan promises, or unbounded database tuning guidance.
- Provider-specific magic without measured benefit or without preserving the shared artifact contract.
- Expanding compiled-model, compiled-query, or pooling guarantees beyond the SQLite evidence boundary already documented.
- Creating a second benchmark artifact format or silently dropping optional-provider rows when providers are not configured.

## Acceptance Criteria
- The epic leaves one authoritative performance-evidence contract in docs/plans/performance-evidence-benchmark-artifact-contract.md, and downstream performance claims reuse benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json plus comparable before/after artifact sets.
- Query-shape guidance is available through the additive IDataVaultReadDiagnosticsService ReadShape surface and remains bounded to translated table identity, filter columns, deterministic ordering, expected index baselines, projected columns, and provider fallback caveats without raw request values or raw-SQL-advisor claims.
- Measured performance evidence exists for provider-neutral read allocations, explicit-save change-tracker overhead, compiled-model/query/pooling scenarios, and provider optimization regression baselines, with SQLite as the required completed local baseline.
- Optional external-provider rows for PostgreSQL, SQL Server, MySQL, and Oracle remain visible as completed or skipped with normalized reasons instead of disappearing from the artifact set.
- Current user-facing documentation treats v0.18.0 as the baseline release and summarizes the bounded performance claims, evidence locations, and manual publication boundary without inventing broader guarantees or an exact release date before final approval.

## Definition of Done
- The seven existing child tickets under the epic collectively cover the benchmark contract, query-shape diagnostics, provider-neutral read tuning, explicit-save tuning, compiled-model/query/pooling evidence, provider optimization baselines, and v0.18.0 documentation rollup with no unresolved PO-level scope gap.
- Repository evidence includes the benchmark summary triplet, the checked-in benchmark bundles for 06F492CAB2293R7BGJWMWMRKT4, 06F492CFSJHN0RGXXRG3KT63FM, and 06F492CTREZEDXVKJ839YGCPWW, the compiled compatibility note, and the v0.18.0 release notes.
- The documented claim boundary remains explicit: SQLite is required, optional provider lanes may be skipped with normalized reasons, and SQL capture is only required when a claim depends on SQL shape, index usage, batching behavior, or materialization behavior.
- No remaining epic-level blocker requires a new child ticket or new architecture decision before PO-critic review.

## Implementation Notes
- Use the existing child ticket contracts as the authoritative decomposition; no further split is needed unless later work expands beyond the current performance-evidence boundary.
- Treat docs/releases/v0.18.0.md and docs/architecture/dvault-ef-compiled-compatibility.md as the consumer-facing rollup of the measured compiled-model/query/pooling and query-shape guidance boundaries.
- Treat benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, and the artifacts/benchmarks bundles for 06F492CAB2293R7BGJWMWMRKT4, 06F492CFSJHN0RGXXRG3KT63FM, and 06F492CTREZEDXVKJ839YGCPWW as the canonical evidence locations for this epic.
- The incoming blocks relation from done ticket 06F492BNDPWS9P4EDSV0W7G6VM is historical context only; if relation hygiene becomes important, clean it in a separate ticket-admin pass rather than reopening epic scope.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket clean up the historical incoming blocks relation from done ticket 06F492BNDPWS9P4EDSV0W7G6VM to reduce graph noise, even though it does not block this epic now?
- Should optional external-provider performance lanes eventually move into provisioned CI or nightly regression infrastructure instead of remaining environment-dependent release or manual evidence?
- After v0.18.0 publication, should compiled-model/query/pooling evidence remain SQLite-only or expand into configured external-provider lanes via a separate follow-up ticket?

## Risks
- Final publication approval remains manual, so docs must keep using the pending-approval placeholder until the approval record supplies the exact v0.18.0 date.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle evidence remains environment-dependent, which can leave those rows skipped locally even when the artifact contract is satisfied.

## Split Recommendations
- No new split recommended; the epic is already materialized as seven done child tickets spanning contract, diagnostics, tuning, benchmark evidence, provider baselines, and documentation rollout.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Improve DVault performance through evidence-first EF Core analysis and targeted tuning for read/save paths. Non-goals: dashboards, observability platforms, workload orchestration, or provider-specific magic without measurable benefit.