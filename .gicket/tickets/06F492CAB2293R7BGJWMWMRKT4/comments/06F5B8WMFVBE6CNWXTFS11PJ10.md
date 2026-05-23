[gicket-bot] PO refinement contract

Summary
- Refined the ticket to use the existing SQLite benchmark harness and artifact contract as the baseline for provider-neutral latest/as-of/PIT/bridge read allocation tuning; no child tickets or relation/document writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already provides the benchmark baseline: the harness emits provider-neutral read rows plus benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json for the required SQLite local baseline.
- Current benchmark evidence already shows material provider-neutral allocation gaps versus the SQLite optimized read rows for latest-satellite, PIT as-of, and bridge traversal reads, so tuning is justified without reopening scope discovery.
- Scope stays on provider-neutral read tuning only; provider-specific read strategies, save-path scenarios, compiled-model or compiled-query work, DbContext pooling, and PIT or bridge maintenance writes are outside this ticket unless used only as unchanged guardrails.
- Relation state was verified and left unchanged: this ticket remains a child of 06F492BTNHRPBC7D24E13ECFKM, has incoming blocks from 06F492BZPP5YT9SJSPDHQBGF3R and 06F492C50WM7V2NE0WZB3774XM, and outgoing blocks to 06F492CTREZEDXVKJ839YGCPWW and 06F492D05THPGQVT3B3K7853A0.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Measure provider-neutral allocation and materialization overhead on the existing benchmarked read baselines for latest/current satellite reads, as-of/PIT-backed reads, and bridge traversal reads.
- Tune provider-neutral read-service, query, and materialization code paths only where before/after evidence on the same scenario shows a real improvement.
- Preserve the existing public provider-neutral read boundary, including IDataVaultReadService request shapes, raw record shapes, and caller-owned projector patterns.

Scope Out
- Provider-specific optimized read-strategy work such as AddDVaultSqlite or non-SQLite provider-native optimization.
- Save-path or bulk-ingestion performance work, plus compiled-model startup, compiled-query, and DbContext-pooling benchmarks except as unchanged guardrails.
- New public read APIs, PIT or bridge maintenance semantics, metadata-model expansion, or release-note work beyond recording benchmark evidence for this ticket.

Open questions
- none

Follow-up questions
- If one read family needs a materially larger refactor than the others, should it become a dedicated follow-up ticket after this bounded evidence pass rather than expanding this story in place?
- After provider-neutral wins are measured, do we want a separate backlog pass to evaluate whether any of the same hotspots justify provider-specific read-strategy follow-up outside SQLite?

Risks
- The checked-in benchmark summary is a single-iteration SQLite snapshot, so noisy or machine-specific deltas can mislead prioritization unless before and after comparisons reuse the same scenario and preserve full run context.
- Allocation reductions may shift cost into slower SQL or more complex query shapes; claims that depend on emitted SQL or index behavior need SQL capture, not only allocation numbers.
- Trying to close every gap to the SQLite optimized row in one pass could over-expand the story; prioritize the biggest provider-neutral wins first and document smaller residual gaps.

Split recommendations
- No immediate split is required from current evidence; keep one ticket unless profiling shows one read family needs an isolated architectural change that would dilute the bounded provider-neutral tuning scope.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment