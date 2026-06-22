[gicket-bot] PO refinement contract

Summary
- Refined this task to a repo-backed defer recommendation: bridge rebuild push-down should not advance now, and ticket 06FE4RKGASKV6F7DF0RD1WTAV4 should document that defer posture.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repo evidence supports a no-implementation recommendation for this ticket: bridge rebuild push-down is not focused enough to advance beyond evaluation.
- Bridge read optimization is already a separate solved lane for maintained bridge rows; the missing question here is write-side maintenance push-down, not bridge traversal reads.
- Core and provider registration evidence is asymmetric: AddDVaultPostgres registers a PIT maintenance strategy, but no IDataVaultProviderBridgeMaintenanceStrategy surface or bridge maintenance registration exists in core or provider packages.
- Existing bridge maintenance covers both many-to-many and hierarchy rebuild semantics; hierarchy handling includes shortest-path updates, topology-shrink rebuild correction, and cycle handling without self rows, which is materially broader than the current PIT rebuild prototype lane.
- Relation evidence shows this ticket blocks 06FE4RKGASKV6F7DF0RD1WTAV4, so the immediate follow-on is documentation that bridge push-down remains deferred.

Scope In
- Evaluate whether a bounded bridge rebuild push-down slice is worthwhile after the PIT prototype boundary work.
- Compare bridge maintenance complexity and current provider seams against the PIT-only prototype path already present in the repo.
- Produce the authoritative recommendation and explicit defer list needed for downstream documentation and architecture updates.

Scope Out
- Implement any provider-specific bridge maintenance strategy, core dispatch seam, or bridge SQL path.
- Add bridge maintenance dry-run diagnostics, support-bundle outputs, or deployment/runtime orchestration.
- Change current bridge maintenance semantics, including delete-aware incremental repair or automatic maintenance.
- Reopen PIT prototype tickets or bridge read optimization evidence that already exists for maintained bridge rows.

Open questions
- none

Follow-up questions
- If later hotspot evidence reopens this area, should the first bridge maintenance prototype be limited to PostgreSQL full rebuild for many-to-many bridges only?
- Should any future bridge push-down exploration adopt the PIT dry-run diagnostics and redaction posture before executable provider paths are attempted?
- When 06FE4RKGASKV6F7DF0RD1WTAV4 resumes, should it state bridge push-down as a deferred non-goal until post-PIT prototype hotspot evidence exists?

Risks
- If downstream work treats bridge read optimization evidence as proof of bridge maintenance push-down value, the team may overstate what the repository has actually validated.
- Jumping straight to hierarchy push-down risks mismatching current rebuild semantics around topology shrink, TraversalDepth increases, and cycle handling.
- Creating an implementation ticket now would likely expand from SQL prototyping into new core dispatch, fallback, and diagnostics contracts, which is larger than this bounded feasibility task.

Split recommendations
- Do not create a bridge implementation child from this ticket now; keep 06FE4RKGASKV6F7DF0RD1WTAV4 as the only immediate downstream task.
- If the area is reopened later, split first by many-to-many full rebuild versus hierarchy rebuild, and keep incremental and delete-aware maintenance, deployment artifacts, and non-PostgreSQL providers out of the first slice.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment