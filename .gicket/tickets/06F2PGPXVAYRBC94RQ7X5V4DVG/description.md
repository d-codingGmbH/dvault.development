<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified local .gicket ticket/comment/relation state and current repository docs/source; refined this ticket to a bounded v0.15.0 documentation pass that aligns README and release records with the already-shipped bridge maintenance, PIT maintenance, current/as-of convenience reads, and SQLite PIT/bridge read optimization surface.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current ticket comments contain only bot claim/lease entries and no human scope changes, and no ticket attachments are present in the repository-local ticket store.
- The repository already contains docs/releases/v0.15.0.md, so this ticket should revise that existing release record in place rather than create a new release-note file.
- Current source and tests confirm the shipped v0.15 surface spans four delivered deltas: explicit bridge maintenance, explicit PIT maintenance, current/as-of satellite convenience overloads, and SQLite provider-aware PIT/bridge read optimization with provider-neutral fallback.
- Current README and adopter guidance are only partially aligned: current/as-of convenience reads and SQLite read dispatch are already documented, but the README v0.15 summary/limitations and docs/production-adoption-checklist.md still describe PIT rows as caller-populated and frame v0.15.0 as bridge-only.
- No child tickets, relation writes, attachments, or planning documents were materialized in this refinement pass because the repository already contains the needed feature split and planning context.

### Scope In
- Update README.md so the read-model guidance clearly states that PIT-backed reads consume explicitly maintained PIT tables through IDataVaultPitMaintenanceService, bridge reads consume explicitly maintained bridge tables through IDataVaultBridgeMaintenanceService, and AddDVaultSqlite() is the only repository-proven optimized PIT/bridge read path with provider-neutral fallback elsewhere.
- Revise the README.md v0.15.0 summary and limitation sections so they reflect the full shipped release surface instead of only bridge maintenance.
- Rewrite docs/releases/v0.15.0.md as the coordinated release record for bridge maintenance, PIT maintenance, current/as-of convenience overloads, and SQLite PIT/bridge read optimization, while preserving explicit-service boundaries and provider-evidence limits.
- Update adopter-facing supporting docs that still carry stale release posture or stale PIT guidance, including docs/production-adoption-checklist.md and any current-baseline user doc that still points at v0.14.0 as the active release baseline.
- Keep release-note validation evidence tied to committed source and tests that already prove the shipped surface.

### Scope Out
- Any product-code, API, diagnostics, benchmark, or test behavior changes beyond documentation-only edits.
- New PIT or bridge maintenance features, registry-backed PIT maintenance APIs, provider-specific PIT/bridge optimization beyond the existing SQLite path, or changes to current/as-of query semantics.
- Relation cleanup, child-ticket creation, or planning-document materialization unless a later refinement pass finds a new bounded planning gap; none is justified by the current local evidence.
- Historical release-note rewrites beyond small cross-links needed to make v0.15.0 the clear current baseline.

## Acceptance Criteria
- README.md explicitly documents the shipped PIT maintenance surface through IDataVaultPitMaintenanceService, DataVaultPitRebuildRequest, and DataVaultPitParentMaintenanceRequest, and it describes PIT-backed reads as consuming explicitly maintained PIT rows rather than caller-populated or implicitly refreshed rows.
- README.md and any touched adopter guidance preserve the current/as-of satellite convenience overloads as additive wrappers over the existing DataVaultLatestSatelliteReadRequest baseline and keep bridge maintenance documented as an explicit caller-invoked service boundary.
- Public docs describe only SQLite as the repository-proven optimized PIT/bridge read provider path and state that unsupported providers or unsupported shapes fall back to the provider-neutral read pipelines without implicit maintenance side effects.
- docs/releases/v0.15.0.md covers the coordinated shipped delta for bridge maintenance, PIT maintenance, current/as-of convenience reads, and SQLite PIT/bridge read optimization, and its compatibility and limitation sections no longer state that PIT maintenance is outside the release.
- docs/production-adoption-checklist.md and any other touched current-baseline adopter doc no longer describe PIT rows as caller-populated-only and no longer point readers at v0.14.0 as the active public baseline when v0.15.0 is intended to be current.
- The v0.15.0 release record cites committed source and test evidence for PIT maintenance, bridge maintenance, current/as-of convenience reads, and SQLite read-strategy dispatch using the actual repository files that back those claims.

## Definition of Done
- README.md, docs/releases/v0.15.0.md, and docs/production-adoption-checklist.md are internally consistent about explicit service boundaries: PIT and bridge maintenance are caller-invoked, current/as-of convenience reads remain additive over the latest-satellite baseline, and SQLite-only optimization claims stay bounded to repository evidence.
- Any adopter-facing doc still treated as a current-baseline reference no longer points readers at v0.14.0 as the active release posture once this ticket is complete.
- Release-note validation evidence names the actual committed source and test files that back PIT maintenance, current/as-of convenience reads, bridge maintenance, and SQLite read optimization.
- No child tickets, relation writes, attachments, or planning documents are introduced for this ticket unless implementation uncovers a new bounded documentation gap that is not visible in the current repository evidence.

## Implementation Notes
- Use the existing code as authoritative: src/DCoding.Data.DVault/DataVaultPitMaintenanceService.cs, DataVaultPitRebuildRequest.cs, DataVaultPitParentMaintenanceRequest.cs, and DefaultDataVaultPitMaintenanceService.cs define the PIT maintenance surface. Do not document registry-backed PIT maintenance because no such public adapter exists in the repository.
- Keep the current README read examples and current/as-of convenience terminology; those sections already document ReadCurrentSatellite... and ReadAsOfSatellite... correctly and only need compatibility framing, not a new API-design pass.
- Treat src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs and the PIT/bridge read tests as the boundary for optimized read claims; do not imply PostgreSQL, SQL Server, Oracle, or MySQL PIT/bridge read optimization.
- Revise docs/releases/v0.15.0.md in place: its current bridge-only framing and PIT-maintenance-outside-this-release wording are stale against the committed source and tests.
- Useful evidence files already in-repo include tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.
- Local evidence already provides the planning chain via docs/plans/pit-maintenance-service-v1-contract.md and the done sibling tickets, so no new split, attachment, or planning document was needed in this PO pass.

## Open Questions
- none

## Follow-Up Questions
- After v0.15.0 adopter docs land, should a later docs-only cleanup sweep update deeper architecture notes that still label v0.14.0 as the current baseline even when user-facing guidance has moved to v0.15.0?
- Should a later release add a runnable PIT maintenance example or quickstart, since the current repository evidence is source/tests plus README guidance rather than a dedicated sample?

## Risks
- If the docs pass only tweaks the release notes and misses README/adoption baseline text, consumers will still read conflicting guidance about whether PIT rows are caller-populated or explicitly maintained.
- If the release notes over-claim provider-aware read optimization beyond SQLite, the public record will outrun the repository's benchmark and test evidence.
- If the docs blur PIT maintenance with PIT reads or bridge maintenance with bridge reads, callers may infer implicit refresh behavior that the shipped services intentionally do not provide.

## Split Recommendations
- No new split is recommended. The repository already has the durable feature split across bridge maintenance, PIT maintenance, current/as-of convenience reads, and provider-aware read optimization; this ticket should stay a documentation-only consolidation pass over those completed slices.
- If the team later wants broader architecture-doc refresh or new runnable examples, track that work in separate follow-up tickets rather than widening this v0.15.0 release-note and adopter-guidance pass.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Document maintenance and query operations for adopters.

## Scope
- Refine and complete the work for "Update v0.15.0 documentation and release notes" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.