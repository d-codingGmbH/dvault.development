[gicket-bot] PO refinement contract

Summary
- Current-branch evidence supports the same defer recommendation, but the contract is now restated with source-backed claims only: PostgreSQL has a PIT maintenance seam, bridge maintenance remains provider-neutral only, maintained-bridge read evidence already exists, and no bridge push-down implementation child should be opened from this ticket now.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced the inferred bridge-maintenance API assumption with source-backed wording. The current branch shows a PIT-provider seam only: AddDVaultPostgres() registers IDataVaultProviderPitMaintenanceStrategy, AddDVault() registers the provider-neutral IDataVaultBridgeMaintenanceService, and no IDataVaultProviderBridgeMaintenanceStrategy symbol is visible in the current branch. Any future bridge push-down prototype would therefore need new core/provider bridge-maintenance seam work instead of plugging into an existing public bridge-maintenance strategy type.
- critic-item-2: `answered` - The contract no longer relies on an unproven existing bridge-provider type or registration surface. It now states explicitly that the visible public bridge-maintenance surface is provider-neutral only, and that a future bridge push-down lane would first need new core dispatch, provider registration, and bridge-specific fallback/diagnostics work before SQL implementation could be scoped.
- critic-item-3: `answered` - The non-goal around read-side evidence is now source-backed. Current repo docs already record completed maintained-bridge read evidence for PostgreSQL, SQL Server, MySQL, and Oracle in the v0.32 smoke-read bundle, for DB2 in the DB2 hotspot bundle, and for SQLite in the root triplet. This ticket therefore stays on write-side bridge-maintenance push-down feasibility and does not reopen the already-documented maintained-bridge read optimization lane.

Clarifications
- Current branch evidence supports a defer recommendation, not an implementation follow-on: bridge rebuild push-down remains larger than the PIT prototype lane because the visible bridge-maintenance surface is provider-neutral only and no bridge-provider maintenance seam is present in current source.
- AddDVaultPostgres() registers a PostgreSQL PIT maintenance strategy today, but the same current-branch evidence does not show a bridge-maintenance counterpart in core or provider packages.
- Existing bridge read optimization evidence is already documented for maintained bridge rows; that read-side evidence does not prove write-side bridge-maintenance push-down feasibility.
- Current ticket relation evidence in .gicket still shows 06FE4RK80ZXGCZ62CMSAYP164W blocking 06FE4RKGASKV6F7DF0RD1WTAV4, so the immediate downstream action remains documentation of the defer posture rather than a new bridge implementation child.
- No ticket description update, relation change, attachment, child ticket, or planning-document write was applied in this pass.

Scope In
- Evaluate whether a bounded bridge rebuild push-down slice is worthwhile after the PIT prototype boundary work.
- Compare current bridge-maintenance semantics and visible provider seams against the narrower PostgreSQL PIT rebuild prototype lane already present in the repo.
- Record the authoritative defer recommendation, explicit non-goals, and reopen threshold for downstream documentation work.

Scope Out
- Implement any provider-specific bridge-maintenance strategy, core bridge-maintenance dispatch seam, or provider SQL path.
- Add bridge-maintenance dry-run diagnostics, support-bundle exports, deployment artifacts, or runtime orchestration.
- Change current bridge-maintenance semantics, including delete-aware incremental repair or automatic maintenance.
- Re-open already-documented PIT prototype or maintained-bridge read-optimization evidence lanes as if they were still unproven.

Open questions
- none

Follow-up questions
- If hotspot evidence later reopens this area, should the first bridge-maintenance prototype be limited to PostgreSQL full rebuild for many-to-many bridges only?
- Should any later bridge push-down exploration adopt PIT-style gate/fallback diagnostics before executable provider SQL is attempted?
- When 06FE4RKGASKV6F7DF0RD1WTAV4 resumes, should it state bridge push-down as a deferred non-goal until post-PIT prototype hotspot evidence exists?

Risks
- If downstream work treats maintained-bridge read benchmark evidence as proof of bridge-maintenance push-down value, the team may overstate what the repository has actually validated.
- Jumping directly to hierarchy push-down risks mismatching current rebuild semantics around topology shrink, TraversalDepth increases, shortest-path lowering, and cycle handling.
- Opening an implementation ticket now would likely expand from SQL prototyping into new core dispatch, provider registration, bridge-specific fallback vocabulary, and diagnostics contracts, which is larger than this bounded feasibility task.

Split recommendations
- Do not create a bridge implementation child from this ticket now; keep 06FE4RKGASKV6F7DF0RD1WTAV4 as the immediate downstream documentation task.
- If the area is reopened later, split first by many-to-many full rebuild versus hierarchy rebuild, and keep incremental/delete-aware maintenance, diagnostics/deployment surfaces, and non-PostgreSQL providers out of the first slice.

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