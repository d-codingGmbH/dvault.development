<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Current-branch evidence supports the same defer recommendation, but the contract is now restated with source-backed claims only: PostgreSQL has a PIT maintenance seam, bridge maintenance remains provider-neutral only, maintained-bridge read evidence already exists, and no bridge push-down implementation child should be opened from this ticket now.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current branch evidence supports a defer recommendation, not an implementation follow-on: bridge rebuild push-down remains larger than the PIT prototype lane because the visible bridge-maintenance surface is provider-neutral only and no bridge-provider maintenance seam is present in current source.
- AddDVaultPostgres() registers a PostgreSQL PIT maintenance strategy today, but the same current-branch evidence does not show a bridge-maintenance counterpart in core or provider packages.
- Existing bridge read optimization evidence is already documented for maintained bridge rows; that read-side evidence does not prove write-side bridge-maintenance push-down feasibility.
- Current ticket relation evidence in .gicket still shows 06FE4RK80ZXGCZ62CMSAYP164W blocking 06FE4RKGASKV6F7DF0RD1WTAV4, so the immediate downstream action remains documentation of the defer posture rather than a new bridge implementation child.
- No ticket description update, relation change, attachment, child ticket, or planning-document write was applied in this pass.

### Scope In
- Evaluate whether a bounded bridge rebuild push-down slice is worthwhile after the PIT prototype boundary work.
- Compare current bridge-maintenance semantics and visible provider seams against the narrower PostgreSQL PIT rebuild prototype lane already present in the repo.
- Record the authoritative defer recommendation, explicit non-goals, and reopen threshold for downstream documentation work.

### Scope Out
- Implement any provider-specific bridge-maintenance strategy, core bridge-maintenance dispatch seam, or provider SQL path.
- Add bridge-maintenance dry-run diagnostics, support-bundle exports, deployment artifacts, or runtime orchestration.
- Change current bridge-maintenance semantics, including delete-aware incremental repair or automatic maintenance.
- Re-open already-documented PIT prototype or maintained-bridge read-optimization evidence lanes as if they were still unproven.

## Acceptance Criteria
- The ticket records an evidence-backed recommendation that bridge rebuild push-down should stay deferred because the current branch exposes a PostgreSQL PIT-provider maintenance seam but no visible bridge-provider maintenance seam, while bridge maintenance semantics remain materially broader than the PIT prototype lane.
- The recommendation distinguishes existing maintained-bridge read optimization evidence from the still-missing write-side bridge-maintenance push-down evidence so downstream work does not treat read-path wins as implementation proof for bridge maintenance.
- The ticket explicitly defers unsupported bridge push-down scope: hierarchy rebuild semantics, incremental and delete-aware maintenance, bridge-specific gate/fallback/diagnostic vocabulary, and provider expansion beyond a later first prototype.
- The outcome states that no bridge implementation child should be created from this ticket now and that 06FE4RKGASKV6F7DF0RD1WTAV4 remains the immediate documentation follow-on.

## Definition of Done
- The PO contract names the defer recommendation, the current-branch source evidence behind it, and the reopen threshold for any later bridge push-down work.
- The contract makes clear that bridge push-down is not pre-approved by the PIT prototype seam or by existing maintained-bridge read benchmark evidence.
- Unsupported future scope is listed explicitly so later tickets do not silently widen from a possible many-to-many full-rebuild prototype into hierarchy, incremental repair, diagnostics, or deployment concerns.
- No blocking PO question remains once the source-backed defer posture and downstream documentation target are captured.

## Implementation Notes
- AddDVaultPostgres() registers IDataVaultProviderPitMaintenanceStrategy via PostgresDataVaultPitMaintenanceStrategy, while AddDVaultSqlServer() registers save/read strategies only; this confirms current branch asymmetry on the PIT side and does not show a bridge-maintenance counterpart.
- AddDVault() registers the provider-neutral IDataVaultPitMaintenanceService and IDataVaultBridgeMaintenanceService, and IDataVaultBridgeMaintenanceService exposes only RebuildBridgeAsync(...) and MaintainBridgeAsync(...).
- Current-branch repository search shows no IDataVaultProviderBridgeMaintenanceStrategy symbol, so a future bridge push-down prototype would need new core/provider API and registration work rather than an assumed existing bridge-maintenance strategy type.
- DataVaultBridgeMaintenanceServiceSqliteTests prove broader bridge semantics than the PIT prototype lane: many-to-many rebuild plus append-only maintenance, hierarchy shortest-path lowering, topology shrink that requires rebuild to increase TraversalDepth, and cycle handling without implicit self rows.
- DataVaultProviderPitMaintenanceStrategyGateEvaluator is PIT-only and already carries provider mismatch, dirty-context, incomplete-maintenance-shape, and unsupported-PIT fallback vocabulary; a bridge push-down lane would need analogous bridge-specific gate and diagnostics work before provider SQL could be justified.
- docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/plans/provider-optimization-evidence-matrix.md, and docs/performance-profiles.md already document maintained-bridge read evidence for SQLite and the supported external-provider bundles, so this ticket should not be treated as a read-side evidence gap.

## Open Questions
- none

## Follow-Up Questions
- If hotspot evidence later reopens this area, should the first bridge-maintenance prototype be limited to PostgreSQL full rebuild for many-to-many bridges only?
- Should any later bridge push-down exploration adopt PIT-style gate/fallback diagnostics before executable provider SQL is attempted?
- When 06FE4RKGASKV6F7DF0RD1WTAV4 resumes, should it state bridge push-down as a deferred non-goal until post-PIT prototype hotspot evidence exists?

## Risks
- If downstream work treats maintained-bridge read benchmark evidence as proof of bridge-maintenance push-down value, the team may overstate what the repository has actually validated.
- Jumping directly to hierarchy push-down risks mismatching current rebuild semantics around topology shrink, TraversalDepth increases, shortest-path lowering, and cycle handling.
- Opening an implementation ticket now would likely expand from SQL prototyping into new core dispatch, provider registration, bridge-specific fallback vocabulary, and diagnostics contracts, which is larger than this bounded feasibility task.

## Split Recommendations
- Do not create a bridge implementation child from this ticket now; keep 06FE4RKGASKV6F7DF0RD1WTAV4 as the immediate downstream documentation task.
- If the area is reopened later, split first by many-to-many full rebuild versus hierarchy rebuild, and keep incremental/delete-aware maintenance, diagnostics/deployment surfaces, and non-PostgreSQL providers out of the first slice.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: evaluate whether bridge rebuild push-down is worthwhile and focused enough after PIT prototypes. Acceptance: recommendation is evidence-backed and unsupported cases are deferred explicitly.