[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - Resolved. The current branch evidence shows docs/plans/README.md already durably lists provider-specific-sql-artifact-contract.md, so the contract does not need a fallback removal of that claim.
- critic-item-2: `answered` - Resolved. The Delivery Contract and Implementation Notes now match the verified state: the planning contract exists, the README index is durable, and the ticket description update is the authoritative narrative reconciliation for this ticket.
- critic-item-3: `answered` - Satisfied. Repository state and persisted ticket narrative now agree, so the clarification blocker is cleared and the ticket can continue to PO-critic review without additional PO-side refinement.
- critic-item-4: `answered` - Superseded by current branch evidence. The earlier blocking finding described a prior scratch-only mismatch, but the current branch snapshot now shows the README index entry in branch content, so the durability gap is closed.

Clarifications
- docs/plans/provider-specific-sql-artifact-contract.md remains the authoritative planning contract surface for this lane.
- docs/plans/README.md now durably lists provider-specific-sql-artifact-contract.md in the current branch state; the earlier scratch-only mismatch is historical and superseded.
- The persisted ticket description was already reconciled in ticket revision 06F9XQPD2K1SWSAFQ22S0PDG8M, which is the authoritative ticket narrative for the next PO-critic pass.
- The bounded architecture decision remains design-time-only, consumer-owned, single-project, and excludes runtime dispatch, standalone CLI behavior, and automatic migration synchronization.
- The existing child split remains sufficient: 06F8KZVCVRPS3NAGQA7J55EAA4 for evidence, 06F8KZV18BQ0GN3CE4G02ATVA0 for the dry-run manifest prototype, and 06F8KZVRARQPG482YKCQ686PNM for documentation alignment.
- No new child-ticket creation, relation cleanup, attachment binding, or planning-document write was required in this clarification pass.

Scope In
- Define the v1 contract for reviewed design-time provider-specific SQL or stored-procedure artifacts as explicit consumer-owned outputs.
- Fix the authoritative boundaries for single-project design-time generation, deterministic manifest-first review, and prerequisite validation, diagnostics, and benchmark evidence.
- Keep the first implementation lane dry-run only and aligned with the existing evidence, prototype, and documentation child split.
- Ratify the current supported-provider baseline of SQLite, PostgreSQL, SQL Server, MySQL, and Oracle for this contract lane.

Scope Out
- Runtime dispatch, automatic stored-procedure invocation, interceptors, background workers, schedulers, or a second default DVault runtime path.
- Automatic deployment, automatic cleanup, EF migration synchronization, live-schema mutation, or support-bundle refresh synchronization.
- A standalone DVault CLI, multi-project design-time orchestration, or providers beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Benchmark-threshold tuning or production-ready provider-specific execution in this ticket.

Open questions
- none

Follow-up questions
- Which of the already-supported providers should be used for the first dry-run manifest prototype in 06F8KZV18BQ0GN3CE4G02ATVA0?
- What consumer repository path convention should the later implementation adopt for reviewed manifests and any sidecar SQL payload files?
- After the dry-run prototype and evidence tickets land, should a later ticket permit deployable SQL payload emission, or should the lane remain review-only until more provider evidence exists?
- How should the v0.32 documentation rollout summarize this lane once the prototype and evidence tickets complete?

Risks
- If later edits desynchronize the ticket narrative from the durable planning surfaces again, PO-critic should reopen the ticket.
- Future implementation work could widen this lane into runtime dispatch or automatic migration behavior unless it stays anchored to the explicit non-goals in the contract.
- A prototype that skips benchmark or semantic-parity evidence could create unmeasured provider-specific claims that conflict with the repository's performance-evidence posture.

Split recommendations
- No new split is justified now; the current parent and child tickets still separate architecture contract, evidence rules, dry-run prototype, and v0.32 documentation alignment.
- If a later phase wants deployable SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this contract or the dry-run prototype ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment