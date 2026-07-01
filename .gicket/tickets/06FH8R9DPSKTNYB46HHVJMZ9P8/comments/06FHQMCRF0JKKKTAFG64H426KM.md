[gicket-bot] PO refinement contract

Summary
- Ratified the story as a tracking refinement over already-checked-in provider optimization closure evidence: matrix refresh, save parity, read parity, and documentation/evidence children are already split and the repository baseline already closes the save, latest-satellite, PIT, and bridge timing rows. No blocking PO clarification remains; the only bounded future implementation lane worth separate tracking is DB2 ordinary hub-parent PIT full-rebuild maintenance.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as the authoritative planning and row-lookup surfaces for this story.
- Treat artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/ as the completed-timing source for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save, latest-satellite, PIT, and bridge rows; the repository-root benchmark-summary.* triplet remains the quick SQLite plus skipped-placeholder optional-provider baseline.
- The current live split is already finite and should be ratified: done child 06FH8RATZGZRVAJVC4ERV0ACYW refreshed the matrix, done child 06FH8RC9F0QEWF356WF7YYNNGM owns save parity, done child 06FH8RDS25081N5S181C7TQGTG owns read parity, done child 06FH8REKX113JRZQ42HEB1NVZ8 owns evidence/docs publication, and this story currently blocks downstream release-note task 06FH8RP1SBVZ7K3K48ERGZSMQC.
- Incoming blocks relations from done children 06FH8RATZGZRVAJVC4ERV0ACYW, 06FH8RC9F0QEWF356WF7YYNNGM, 06FH8RDS25081N5S181C7TQGTG, and 06FH8REKX113JRZQ42HEB1NVZ8 are historical workflow context, not a refinement blocker, because the source tickets are done and the story ticket is not marked blocked.
- PIT and bridge read timing stays separate from PIT maintenance timing: MySQL ordinary hub-parent PIT full rebuild already landed in 06FFDG522514HX2J17GT9VE77W, Oracle PIT maintenance remains deferred, and DB2 PIT maintenance remains provider-neutral until a separate bounded implementation ticket lands.

Scope In
- Ratify the existing provider optimization closure baseline for provider-native save, latest-satellite read, PIT read, and bridge read rows across PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Keep the story as the parent planning surface that references the existing matrix-refresh, save-parity, read-parity, and documentation/evidence children rather than reopening those lanes.
- Preserve the bounded runtime fallback posture for dirty contexts, provider mismatch, unsupported latest-satellite shapes, incomplete read-shape evidence, stale PIT/bridge maintenance, and unsupported bridge-maintenance push-down.
- Record the one accepted future maintenance-specific expansion lane: DB2 ordinary hub-parent RebuildAsync(...) PIT full rebuild through IDataVaultProviderPitMaintenanceStrategy as a separate future child, not as implied work inside the closed save/read/doc tickets.

Scope Out
- Fresh benchmark reruns, external-provider provisioning, or artifact regeneration for already-closed save, latest-satellite, PIT, or bridge rows.
- Reopening PostgreSQL, SQL Server, MySQL, Oracle, or DB2 completed save, latest-satellite, PIT, or bridge timing rows as open parity gaps.
- Oracle PIT maintenance, bridge-maintenance push-down, staged DB2 bulk, provider-native chunk execution, binary-storage compatibility remediation, or other deferred fallback boundaries.
- v0.51.0 release-note, changelog, package-validation, and publication-surface work owned by ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.

Open questions
- none

Follow-up questions
- none

Risks
- The current one-line ticket description still reads like broad implementation discovery; without this refinement, downstream reviewers can reopen already-closed save/read rows or ask for duplicate benchmark reruns.
- Because the repository-root benchmark-summary.* files still contain skipped optional-provider rows, reviewers can misread placeholders as missing evidence unless the closure bundle and evidence matrix remain the cited sources.
- The accepted DB2 PIT maintenance lane is not materialized as a child ticket in the current live relation set, so that future work can be lost if the team wants to pursue maintenance parity later.

Split recommendations
- Do not split save, read, or documentation scope any further; those lanes are already bounded by tickets 06FH8RC9F0QEWF356WF7YYNNGM, 06FH8RDS25081N5S181C7TQGTG, and 06FH8REKX113JRZQ42HEB1NVZ8.
- If provider-maintenance expansion is prioritized, create one separate child limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy for DB2.
- Keep Oracle PIT maintenance, maintenance timing evidence collection, bridge-maintenance push-down, staged DB2 bulk, provider-native chunk execution, and binary-storage remediation as separate later tickets rather than enlarging this story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment