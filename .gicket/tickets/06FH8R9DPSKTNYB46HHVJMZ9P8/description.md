<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ratified the story as a tracking refinement over already-checked-in provider optimization closure evidence: matrix refresh, save parity, read parity, and documentation/evidence children are already split and the repository baseline already closes the save, latest-satellite, PIT, and bridge timing rows. No blocking PO clarification remains; the only bounded future implementation lane worth separate tracking is DB2 ordinary hub-parent PIT full-rebuild maintenance.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as the authoritative planning and row-lookup surfaces for this story.
- Treat artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/ as the completed-timing source for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save, latest-satellite, PIT, and bridge rows; the repository-root benchmark-summary.* triplet remains the quick SQLite plus skipped-placeholder optional-provider baseline.
- The current live split is already finite and should be ratified: done child 06FH8RATZGZRVAJVC4ERV0ACYW refreshed the matrix, done child 06FH8RC9F0QEWF356WF7YYNNGM owns save parity, done child 06FH8RDS25081N5S181C7TQGTG owns read parity, done child 06FH8REKX113JRZQ42HEB1NVZ8 owns evidence/docs publication, and this story currently blocks downstream release-note task 06FH8RP1SBVZ7K3K48ERGZSMQC.
- Incoming blocks relations from done children 06FH8RATZGZRVAJVC4ERV0ACYW, 06FH8RC9F0QEWF356WF7YYNNGM, 06FH8RDS25081N5S181C7TQGTG, and 06FH8REKX113JRZQ42HEB1NVZ8 are historical workflow context, not a refinement blocker, because the source tickets are done and the story ticket is not marked blocked.
- PIT and bridge read timing stays separate from PIT maintenance timing: MySQL ordinary hub-parent PIT full rebuild already landed in 06FFDG522514HX2J17GT9VE77W, Oracle PIT maintenance remains deferred, and DB2 PIT maintenance remains provider-neutral until a separate bounded implementation ticket lands.

### Scope In
- Ratify the existing provider optimization closure baseline for provider-native save, latest-satellite read, PIT read, and bridge read rows across PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Keep the story as the parent planning surface that references the existing matrix-refresh, save-parity, read-parity, and documentation/evidence children rather than reopening those lanes.
- Preserve the bounded runtime fallback posture for dirty contexts, provider mismatch, unsupported latest-satellite shapes, incomplete read-shape evidence, stale PIT/bridge maintenance, and unsupported bridge-maintenance push-down.
- Record the one accepted future maintenance-specific expansion lane: DB2 ordinary hub-parent RebuildAsync(...) PIT full rebuild through IDataVaultProviderPitMaintenanceStrategy as a separate future child, not as implied work inside the closed save/read/doc tickets.

### Scope Out
- Fresh benchmark reruns, external-provider provisioning, or artifact regeneration for already-closed save, latest-satellite, PIT, or bridge rows.
- Reopening PostgreSQL, SQL Server, MySQL, Oracle, or DB2 completed save, latest-satellite, PIT, or bridge timing rows as open parity gaps.
- Oracle PIT maintenance, bridge-maintenance push-down, staged DB2 bulk, provider-native chunk execution, binary-storage compatibility remediation, or other deferred fallback boundaries.
- v0.51.0 release-note, changelog, package-validation, and publication-surface work owned by ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.

## Acceptance Criteria
- The story contract points to docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as the canonical planning and evidence lookup surfaces.
- The contract states that the 2026-06-23 provider optimization closure bundle is the authoritative completed-timing source for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save, latest-satellite, PIT, and bridge rows, while the root benchmark-summary.* files remain quick SQLite and skipped-placeholder guidance only.
- The contract ratifies the existing child split and does not reopen save, read, or documentation work that is already bounded in tickets 06FH8RC9F0QEWF356WF7YYNNGM, 06FH8RDS25081N5S181C7TQGTG, and 06FH8REKX113JRZQ42HEB1NVZ8 after matrix-refresh ticket 06FH8RATZGZRVAJVC4ERV0ACYW.
- The contract keeps PIT maintenance separate from PIT read timing and names DB2 ordinary hub-parent full-rebuild push-down as the only accepted future implementation lane worth separate tracking from this story.
- No acceptance text requires new repository runtime code, new benchmark execution, or relation cleanup before PO-critic review.

## Definition of Done
- Reviewers can treat the current repository matrices, closure bundle, performance profile, PIT/bridge boundary doc, and v0.46.0 release notes as the authoritative provider-optimization closure baseline for this story.
- Closed provider save/read rows are not restated as open work, and remaining behavior is classified as bounded fallback, deferred maintenance, or future separate-child work with a finite reason.
- The story remains a coherent parent planning surface with no PO blocker about provider set, evidence source, or child-ticket ownership.
- Open questions are empty; any later DB2 PIT maintenance expansion or historical relation cleanup is non-blocking follow-up rather than a PO handoff blocker.

## Implementation Notes
- Repository evidence already aligns this story to the closure baseline: docs/plans/provider-optimization-gap-matrix.md marks P0-P3 save/read rows closed, docs/plans/provider-optimization-evidence-matrix.md names the closure bundle as the completed-timing source, docs/performance-profiles.md and docs/architecture/dvault-v1-pit-bridge-boundary.md preserve the fallback and read-versus-maintenance boundaries, and docs/releases/v0.46.0.md publishes the closure baseline.
- The current branch already contains the relevant provider code and diagnostics surfaces for the closed read/save lanes, including provider strategy registrations and fallback gates under src/DCoding.Data.DVault.*, so this story should not be treated as fresh implementation discovery.
- docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md and docs/architecture/dvault-v1-pit-bridge-boundary.md narrow the only accepted future DB2 maintenance lane to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy with provider-neutral fallback for dirty contexts, unsupported shapes, incomplete evidence, and caller transactions without proven savepoint rollback.
- No bounded planning writes, child-ticket creation, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass.
- Current live relation evidence is consistent with treating this as a tracking parent: it has parentOf links to the matrix-refresh, save, read, and documentation children, outgoing blocks to release-note task 06FH8RP1SBVZ7K3K48ERGZSMQC, and historical incoming blocks from done children that do not reopen scope.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- The current one-line ticket description still reads like broad implementation discovery; without this refinement, downstream reviewers can reopen already-closed save/read rows or ask for duplicate benchmark reruns.
- Because the repository-root benchmark-summary.* files still contain skipped optional-provider rows, reviewers can misread placeholders as missing evidence unless the closure bundle and evidence matrix remain the cited sources.
- The accepted DB2 PIT maintenance lane is not materialized as a child ticket in the current live relation set, so that future work can be lost if the team wants to pursue maintenance parity later.

## Split Recommendations
- Do not split save, read, or documentation scope any further; those lanes are already bounded by tickets 06FH8RC9F0QEWF356WF7YYNNGM, 06FH8RDS25081N5S181C7TQGTG, and 06FH8REKX113JRZQ42HEB1NVZ8.
- If provider-maintenance expansion is prioritized, create one separate child limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy for DB2.
- Keep Oracle PIT maintenance, maintenance timing evidence collection, bridge-maintenance push-down, staged DB2 bulk, provider-native chunk execution, and binary-storage remediation as separate later tickets rather than enlarging this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Bring supported provider packages closer to parity where benchmark evidence shows meaningful save/read/PIT/bridge gaps. Keep changes inside EF Core provider libraries, retain provider-neutral fallback, and document any gap that remains due to provider limits or missing live infrastructure.