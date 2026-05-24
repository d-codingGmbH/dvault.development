[gicket-bot] PO refinement contract

Summary
- Refined the performance epic around the already-materialized seven-child breakdown and current repository evidence; no new split, attachment, planning document, or relation write was required.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 performance baseline through docs/plans/performance-evidence-benchmark-artifact-contract.md, docs/architecture/dvault-ef-compiled-compatibility.md, docs/releases/v0.18.0.md, and the root benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet.
- Existing materialized children under this epic are 06F492BZPP5YT9SJSPDHQBGF3R (benchmark artifact contract), 06F492C50WM7V2NE0WZB3774XM (query-shape diagnostics), 06F492CAB2293R7BGJWMWMRKT4 (provider-neutral read allocations), 06F492CFSJHN0RGXXRG3KT63FM (explicit-save change-tracker overhead), 06F492CN76GS3CKM8EFD0C20XM (compiled model/query/pooling evidence), 06F492CTREZEDXVKJ839YGCPWW (provider optimization regression baselines), and 06F492D05THPGQVT3B3K7853A0 (v0.18.0 documentation rollup).
- No description update, relation mutation, attachment, child-ticket creation, or planning-document write was applied or queued in this run.

Scope In
- Reuse one shared performance-evidence contract and benchmark artifact format across the whole epic.
- Bounded query-shape diagnostics that expose translated table identity, filter/order facts, projected columns, expected index baselines, and provider fallback caveats without acting as a raw-SQL advisor.
- Measured provider-neutral read allocation tuning for latest-satellite, PIT as-of, and bridge traversal reads.
- Measured explicit-save change-tracker and batching overhead tuning on the existing IDataVaultSaveService boundary.
- SQLite benchmark evidence for compiled model, compiled query, and DbContext pooling guidance with the documented fixed-model boundaries.
- Provider optimization regression baselines that keep PostgreSQL, SQL Server, MySQL, and Oracle lanes visible as completed or skipped.
- v0.18.0 documentation and release-note rollup that points readers at the checked-in evidence and manual publication boundary.

Scope Out
- Dashboards, observability platforms, workload orchestration, or release automation.
- Raw-SQL advisor behavior, automatic index creation, provider physical-plan promises, or unbounded database tuning guidance.
- Provider-specific magic without measured benefit or without preserving the shared artifact contract.
- Expanding compiled-model, compiled-query, or pooling guarantees beyond the SQLite evidence boundary already documented.
- Creating a second benchmark artifact format or silently dropping optional-provider rows when providers are not configured.

Open questions
- none

Follow-up questions
- Should a later ticket clean up the historical incoming blocks relation from done ticket 06F492BNDPWS9P4EDSV0W7G6VM to reduce graph noise, even though it does not block this epic now?
- Should optional external-provider performance lanes eventually move into provisioned CI or nightly regression infrastructure instead of remaining environment-dependent release or manual evidence?
- After v0.18.0 publication, should compiled-model/query/pooling evidence remain SQLite-only or expand into configured external-provider lanes via a separate follow-up ticket?

Risks
- Final publication approval remains manual, so docs must keep using the pending-approval placeholder until the approval record supplies the exact v0.18.0 date.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle evidence remains environment-dependent, which can leave those rows skipped locally even when the artifact contract is satisfied.

Split recommendations
- No new split recommended; the epic is already materialized as seven done child tickets spanning contract, diagnostics, tuning, benchmark evidence, provider baselines, and documentation rollout.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment