[gicket-bot] PO refinement contract

Summary
- Verified the planning document exists, but the README index addition is only an unstaged worktree diff and the ticket contract still claims it is durable, so the ticket cannot return to PO-critic until that mismatch is fixed.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Verified mismatch. `docs/plans/README.md` adds the contract line only in the current worktree diff, while `HEAD` still lacks it. Because the persisted ticket description claims the index is already durable, item 1 remains unresolved until the README write is durably landed or that claim is removed.
- critic-item-2: `answered` - The Delivery Contract and Implementation Notes are not reconciled with the verified write history. They currently say the planning index was updated durably and that no description update was materialized, but verified repository state shows only an unstaged README diff and no durable HEAD evidence for that line.
- critic-item-3: `cannot_answer` - A clean ready-for-po-critic rerun cannot be completed in this pass because the repository state and the persisted ticket narrative still disagree about README durability. Correct one of those surfaces first, then rerun the PO handoff.
- critic-item-4: `answered` - Confirmed. The authoritative planning document is in `HEAD`, but the README index line required by Acceptance Criteria and Definition of Done is only an unstaged worktree diff.

Clarifications
- `docs/plans/provider-specific-sql-artifact-contract.md` is present in HEAD and remains the authoritative planning contract surface for this lane.
- The `docs/plans/README.md` entry for that contract is present in the current worktree but was not verified in HEAD, so the ticket must not claim the index addition is already durable.
- The bounded architecture decision remains design-time-only, consumer-owned, single-project, and excludes runtime dispatch, standalone CLI behavior, and automatic migration synchronization.
- The existing child split remains sufficient: 06F8KZVCVRPS3NAGQA7J55EAA4 for evidence, 06F8KZV18BQ0GN3CE4G02ATVA0 for the dry-run manifest prototype, and 06F8KZVRARQPG482YKCQ686PNM for documentation alignment.

Scope In
- Define the v1 contract for reviewed design-time provider-specific SQL or stored-procedure artifacts as explicit consumer-owned outputs.
- Fix the authoritative boundaries for single-project design-time generation, deterministic manifest-first review surface, evidence prerequisites, and consumer-owned deployment and invocation responsibilities.
- Keep the first implementation lane dry-run only and aligned with the existing evidence, prototype, and documentation child split.

Scope Out
- Runtime dispatch, automatic stored-procedure invocation, interceptors, background workers, schedulers, or a second default DVault runtime path.
- Automatic deployment, automatic cleanup, EF migration synchronization, live-schema mutation, or support-bundle refresh synchronization.
- A standalone DVault CLI, multi-project design-time orchestration, or providers beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Benchmark-threshold tuning or production-ready provider-specific execution in this ticket.

Open questions
- Will this ticket be corrected by durably persisting the `docs/plans/README.md` index entry, or by updating the ticket description to stop claiming that the index change already landed, before PO handoff is rerun?

Follow-up questions
- Which of the already-supported providers should be used for the first dry-run manifest prototype in 06F8KZV18BQ0GN3CE4G02ATVA0?
- What consumer repository path convention should the later implementation adopt for reviewed manifests and any sidecar SQL payload files?
- After the dry-run prototype and evidence tickets land, should a later ticket permit deployable SQL payload emission, or should the lane remain review-only until more provider evidence exists?
- How should the v0.32 documentation rollout summarize this lane once the prototype and evidence tickets complete?

Risks
- If the stale Delivery Contract text remains in place, PO-critic will continue to reject the ticket because the persisted narrative overstates the owner-branch write history.
- Until the README index entry lands durably, downstream discoverability of the planning contract remains incomplete.
- Future implementation work could widen this lane into runtime dispatch or automatic migration behavior unless it stays anchored to the explicit non-goals in the contract.
- A prototype that skips benchmark or semantic-parity evidence could create unmeasured provider-specific claims that conflict with the repository's performance-evidence posture.

Split recommendations
- No new split is justified now; the current parent and child tickets still separate architecture contract, evidence rules, dry-run prototype, and v0.32 documentation alignment.
- If a later phase wants deployable SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this contract or the dry-run prototype ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment