[gicket-bot] PO refinement contract

Summary
- Verified the epic against repository and persisted .gicket state: the contract, evidence rules, SQL Server dry-run prototype, v0.32 documentation, and follow-on threshold-calibration child story are already landed, all five child tickets are done, and no additional planning write is needed before PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already lands the authoritative contract at `docs/plans/provider-specific-sql-artifact-contract.md`, the coordinated release note at `docs/releases/v0.32.0.md`, and source/test support for `dvault sql-artifact` with schema `dvault.sql-artifact.v1`.
- The current implemented prototype boundary remains review-only dry-run output for SQL Server `provider-native-bulk-ingestion`; the supported-provider baseline stays SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, but implemented artifact-export coverage is not broadened beyond the verified SQL Server slice.
- A done documentation child still retains a historical `blocks` relation back to the epic, but the epic itself is not currently blocked and no new relation mutation was required for this refinement pass.
- No child-ticket creation, relation cleanup, description update, attachment binding, or planning-document write was applied in this pass.

Scope In
- Keep this epic as the tracking parent for the existing explicit opt-in, design-time-only provider-specific SQL artifact lane and its completed child split.
- Ratify the landed architecture contract, evidence gate, dry-run prototype, v0.32 documentation posture, and follow-on provider-threshold evidence as the bounded scope already delivered under this epic.
- Preserve the one-provider/one-workload evidence gate using request-bound diagnostics, the shared benchmark artifact triplet, semantic-parity review, and consumer-owned migration compatibility.
- Preserve the repository-wide supported-provider baseline of SQLite, PostgreSQL, SQL Server, MySQL, and Oracle while documenting the narrower currently implemented exporter boundary.

Scope Out
- Runtime dispatch, automatic stored-procedure invocation, interceptors, schedulers, background workers, or any second default DVault runtime path.
- Automatic deployment, automatic cleanup, automatic EF migration synchronization, automatic live-schema mutation, or automatic support-bundle refresh synchronization.
- Claiming implemented artifact exporters for every supported provider or widening support beyond the current repository-visible provider baseline.
- Widening this epic into deployable sidecar SQL payloads, repository storage conventions, or broader operational rollout decisions that belong in separate follow-up tickets.

Open questions
- none

Follow-up questions
- If the lane expands beyond the current dry-run example, which supported provider should be the next explicit exporter after the current SQL Server `provider-native-bulk-ingestion` slice?
- Should a later ticket standardize a consumer-facing repository path convention for reviewed manifests and any future sidecar SQL payload files?
- If deployable SQL payload emission is ever allowed, should it land under a separate follow-up contract instead of widening this epic's current review-only lane?

Risks
- Future readers may overread the supported-provider baseline as implemented artifact-export coverage unless follow-up work keeps the SQL Server-only prototype boundary explicit.
- The live historical `blocks` relation from 06F8KZVRARQPG482YKCQ686PNM back to the epic can confuse relation-only readers even though the epic itself is not currently blocked.
- Later work could incorrectly widen this review-only design-time lane into runtime dispatch or automatic migration behavior unless it stays anchored to the existing non-goals and exact-provider evidence gate.

Split recommendations
- No new split is justified; the live child set already separates architecture contract, evidence rules, dry-run prototype, documentation alignment, and follow-on threshold evidence.
- Create separate follow-up tickets instead of reopening this epic if the team wants additional provider exporters, deployable sidecar SQL payloads, runtime invocation helpers, repository storage conventions, or provider-specific validators.

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