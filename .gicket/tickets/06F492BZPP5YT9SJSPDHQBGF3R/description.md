<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this story into the shared performance-evidence contract: ratify the current benchmark harness and artifact baseline, add required before/after, allocation, SQL-capture, and regression-budget rules, and keep it as the blocker contract for the related performance and documentation stories.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence already provides the bounded v1 baseline: CLI-controlled iterations, warmup, provider filter, persisted `benchmark-summary.md|csv|json` artifacts, run-context metadata, and per-row execution-status reporting; no duplicate routing or ticket split is justified from the visible state.
- SQLite is the required local benchmark provider baseline; PostgreSQL, SQL Server, MySQL, and Oracle are optional external-provider rows that may legitimately remain `skipped` with explicit skip reasons when not configured.
- The current visible benchmark families are sufficient to ratify as the minimum shared baseline: SQLite history/save/read scenarios, latest-index matrix mode, scale matrix mode, and provider-native bulk-ingestion comparisons for configured external providers.
- This story is the shared benchmark-contract blocker under epic `06F492BTNHRPBC7D24E13ECFKM` for tickets `06F492C50WM7V2NE0WZB3774XM`, `06F492CAB2293R7BGJWMWMRKT4`, `06F492CFSJHN0RGXXRG3KT63FM`, `06F492CN76GS3CKM8EFD0C20XM`, and `06F492D05THPGQVT3B3K7853A0`.
- No bounded planning write was materialized in this run because the visible repository and ticket evidence was already sufficient to finalize the refinement contract.

### Scope In
- Define the minimum benchmark scenario families and baseline comparisons that count as valid DVault performance evidence.
- Define the provider matrix and skip-policy rules for required SQLite and optional external-provider runs.
- Define the persisted artifact contract for before/after benchmark evidence, including raw artifact filenames, run-context metadata, and per-row result fields.
- Define required allocation and SQL-capture evidence rules when a performance claim depends on query shape, indexing, batching, or materialization behavior.
- Define default regression budgets and exception-reporting rules that downstream tuning stories must honor.

### Scope Out
- Implementing the actual tuning changes for read, save, compiled-query, compiled-model, or pooling paths.
- Building dashboards, long-running observability pipelines, or benchmark orchestration infrastructure.
- Making every developer machine execute PostgreSQL, SQL Server, MySQL, and Oracle benchmarks locally.
- Expanding the provider set beyond SQLite plus the currently visible optional external providers.
- Rewriting the existing benchmark harness from scratch instead of extending its current contract.

## Acceptance Criteria
- The story defines one authoritative performance-evidence contract that downstream performance tickets must reuse instead of inventing ticket-specific benchmark formats.
- The contract ratifies the current shared artifact trio `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` as required outputs for persisted benchmark evidence, and it requires before/after runs to be stored as two comparable artifact sets under one explicit scenario or ticket label.
- The contract ratifies the current run-context baseline as required metadata: iterations, warmup iterations, load-timestamp storage, provider filter, OS description, OS architecture, process architecture, processor count, .NET runtime description/version, provider execution status, and provider skip reason when applicable.
- The contract ratifies the minimum scenario/provider baseline from the visible harness: required SQLite scenario comparisons, latest-index and scale matrix modes when relevant, and provider-native bulk-ingestion comparisons for PostgreSQL, SQL Server, MySQL, and Oracle only when those providers are configured.
- The contract requires benchmark result rows to preserve the current dimensions of scenario, provider, baseline, strategy family, dataset size, change ratio, execution status, skip reason, iterations, mean/min/max milliseconds, and persisted outcome, and to extend the persisted evidence with allocation metrics for measured runs.
- The contract requires SQL capture to be stored with the same before/after evidence set for scenarios whose claim depends on emitted query shape, index usage, or batching behavior; save-path scenarios that only claim change-tracker or allocation wins do not need duplicate SQL capture unless emitted SQL is part of the claim.
- The contract defines default regression gates: the targeted metric must improve or hold, required SQLite non-target mean-time and allocation regressions over 5% fail by default, configured optional-provider regressions over 10% must be explicitly called out and justified, and skipped optional providers are acceptable only when the artifact records the skip reason instead of omitting the row.

## Definition of Done
- An authoritative repository-facing contract exists for performance evidence and benchmark artifacts, and it is specific enough that the related tuning and documentation tickets can reference it without reopening baseline questions.
- The contract names the minimum scenario families, provider matrix, before/after storage rule, required metadata fields, SQL-capture rule, allocation rule, and default regression budgets.
- The benchmark documentation and/or contract tests are updated so the required artifact filenames and core row/context fields cannot drift silently from the agreed contract.
- The ticket outcome leaves no ambiguity about when skipped provider rows are acceptable and when missing evidence fails a performance claim.

## Implementation Notes
- Use the current benchmark harness as the v1 baseline rather than replacing it: reuse the existing CLI controls for `--iterations`, `--warmup`, `--scale`, `--latest-indexes`, `--load-timestamp-storage`, `--provider`, and the current artifact trio.
- Keep SQLite as the required local proof path because the visible tests and artifact writer already treat `SQLite local temporary files` as the required provider baseline.
- Keep PostgreSQL, SQL Server, MySQL, and Oracle opt-in through connection-string discovery, and preserve explicit `skipped` rows for unconfigured providers so downstream tickets do not confuse missing rows with missing contract coverage.
- Add allocation evidence without removing the current timing fields; the cleanest v1 shape is to append allocation columns/properties to the existing CSV/JSON and mirror them in the markdown summary.
- For query/index-focused claims, capture the representative SQL per measured baseline/scenario pair and store it beside the before/after artifact set instead of burying it in ad hoc console logs.
- This ticket should remain contract/documentation/test focused; the actual optimizations stay in related tickets `06F492C50WM7V2NE0WZB3774XM`, `06F492CAB2293R7BGJWMWMRKT4`, `06F492CFSJHN0RGXXRG3KT63FM`, `06F492CN76GS3CKM8EFD0C20XM`, and `06F492D05THPGQVT3B3K7853A0`.

## Open Questions
- none

## Follow-Up Questions
- After this contract lands, should the release-note story publish raw benchmark artifact links for every scenario or only summarize representative wins and link the full artifact bundle once per release?
- When Oracle optimization expands beyond the current clean hub/link batch scope, should the provider matrix add Oracle satellite-heavy benchmark scenarios or keep that as a separate follow-up contract change?

## Risks
- The current visible harness already persists timing artifacts but not the full allocation and SQL evidence this story wants, so contract work that stops at prose without schema or test updates will let downstream tickets drift.
- External-provider availability is environment-dependent; if the contract is not explicit about `skipped` rows, teams may misread missing PostgreSQL, SQL Server, MySQL, or Oracle evidence as regressions or false failures.
- A contract that does not preserve comparable before/after artifact sets will make later release-note and tuning tickets argue over anecdotal console output instead of repeatable evidence.

## Split Recommendations
- No split recommended; keep this story as the single contract-definition blocker and push actual performance tuning or provider-specific expansions into the already-related downstream tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define how performance work proves value: benchmark scenarios, provider matrix, warmup rules, allocation metrics, SQL capture, before/after storage, and acceptable regression thresholds. This ticket should unblock tuning work.