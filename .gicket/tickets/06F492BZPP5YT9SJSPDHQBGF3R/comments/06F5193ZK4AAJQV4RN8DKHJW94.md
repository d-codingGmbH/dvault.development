[gicket-bot] PO refinement contract

Summary
- Refined this story into the shared performance-evidence contract: ratify the current benchmark harness and artifact baseline, add required before/after, allocation, SQL-capture, and regression-budget rules, and keep it as the blocker contract for the related performance and documentation stories.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already provides the bounded v1 baseline: CLI-controlled iterations, warmup, provider filter, persisted `benchmark-summary.md|csv|json` artifacts, run-context metadata, and per-row execution-status reporting; no duplicate routing or ticket split is justified from the visible state.
- SQLite is the required local benchmark provider baseline; PostgreSQL, SQL Server, MySQL, and Oracle are optional external-provider rows that may legitimately remain `skipped` with explicit skip reasons when not configured.
- The current visible benchmark families are sufficient to ratify as the minimum shared baseline: SQLite history/save/read scenarios, latest-index matrix mode, scale matrix mode, and provider-native bulk-ingestion comparisons for configured external providers.
- This story is the shared benchmark-contract blocker under epic `06F492BTNHRPBC7D24E13ECFKM` for tickets `06F492C50WM7V2NE0WZB3774XM`, `06F492CAB2293R7BGJWMWMRKT4`, `06F492CFSJHN0RGXXRG3KT63FM`, `06F492CN76GS3CKM8EFD0C20XM`, and `06F492D05THPGQVT3B3K7853A0`.
- No bounded planning write was materialized in this run because the visible repository and ticket evidence was already sufficient to finalize the refinement contract.

Scope In
- Define the minimum benchmark scenario families and baseline comparisons that count as valid DVault performance evidence.
- Define the provider matrix and skip-policy rules for required SQLite and optional external-provider runs.
- Define the persisted artifact contract for before/after benchmark evidence, including raw artifact filenames, run-context metadata, and per-row result fields.
- Define required allocation and SQL-capture evidence rules when a performance claim depends on query shape, indexing, batching, or materialization behavior.
- Define default regression budgets and exception-reporting rules that downstream tuning stories must honor.

Scope Out
- Implementing the actual tuning changes for read, save, compiled-query, compiled-model, or pooling paths.
- Building dashboards, long-running observability pipelines, or benchmark orchestration infrastructure.
- Making every developer machine execute PostgreSQL, SQL Server, MySQL, and Oracle benchmarks locally.
- Expanding the provider set beyond SQLite plus the currently visible optional external providers.
- Rewriting the existing benchmark harness from scratch instead of extending its current contract.

Open questions
- none

Follow-up questions
- After this contract lands, should the release-note story publish raw benchmark artifact links for every scenario or only summarize representative wins and link the full artifact bundle once per release?
- When Oracle optimization expands beyond the current clean hub/link batch scope, should the provider matrix add Oracle satellite-heavy benchmark scenarios or keep that as a separate follow-up contract change?

Risks
- The current visible harness already persists timing artifacts but not the full allocation and SQL evidence this story wants, so contract work that stops at prose without schema or test updates will let downstream tickets drift.
- External-provider availability is environment-dependent; if the contract is not explicit about `skipped` rows, teams may misread missing PostgreSQL, SQL Server, MySQL, or Oracle evidence as regressions or false failures.
- A contract that does not preserve comparable before/after artifact sets will make later release-note and tuning tickets argue over anecdotal console output instead of repeatable evidence.

Split recommendations
- No split recommended; keep this story as the single contract-definition blocker and push actual performance tuning or provider-specific expansions into the already-related downstream tickets.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment