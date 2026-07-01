[gicket-bot] PO-critic review contract

Summary
- Repository and ticket evidence support developer handoff: the delivery contract is concrete, directly backed by current docs/artifacts/comments, and contains no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FH8REKX113JRZQ42HEB1NVZ8/description.md contains '## Open Questions - none' and names docs/plans/provider-optimization-evidence-matrix.md plus docs/plans/provider-optimization-gap-matrix.md as the canonical surfaces for this ticket.
- docs/plans/provider-optimization-evidence-matrix.md says the root benchmark-summary.md/.csv/.json triplet is the SQLite-local and skipped optional-provider baseline, and the 2026-06-23 closure bundle under artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/ is the current completed-timing source for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save/latest/PIT/bridge rows.
- benchmark-summary.md directly shows PostgreSQL, SQL Server, MySQL, Oracle, and DB2 optional-provider rows as skipped because DVAULT_TEST_*_CONNECTION_STRING values were unset, confirming the root triplet is placeholder guidance rather than completed external-provider timing evidence.
- artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/README.md lists completed closure-bundle rows for PostgreSQL, SQL Server, MySQL, Oracle, and DB2, including DB2 optimized save 101.037 ms, latest read 14.615 ms, PIT read 27.207 ms, and bridge read 4.831 ms.
- docs/performance-profiles.md, docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/releases/v0.46.0.md, and CHANGELOG.md all carry the same closure-bundle guidance and explicitly separate PIT/bridge read timing from PIT maintenance timing and future maintenance follow-up lanes.
- src/DCoding.Data.DVault/IDataVaultProviderPitMaintenanceStrategy.cs defines the PIT-maintenance strategy seam, and src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers DB2 save/read strategies but no DB2 PIT maintenance strategy, matching the docs that DB2 PIT maintenance remains future child work rather than current-ticket scope.
- Related ticket snapshots show sibling save ticket 06FH8RC9F0QEWF356WF7YYNNGM and read ticket 06FH8RDS25081N5S181C7TQGTG are done; parent story 06FH8R9DPSKTNYB46HHVJMZ9P8 remains todo but is-blocked=false.
- git log shows branch ticket/06FH8REKX113JRZQ42HEB1NVZ8-task-record-provider-parity-benchmark-evidence-a at baeefe25a726f4646beebed7ecd0da6231e1d715, and git diff --name-only develop..HEAD shows only .gicket/tickets/06FH8REKX113JRZQ42HEB1NVZ8/** metadata/comment/description files changed so far, which is consistent with a pre-dev refinement branch rather than completed implementation work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A single worked example of the intended scenario/provider/baseline/posture citation format would make the eventual close-out comment faster, but the matrix contract is already sufficient without it.

Risky assumptions
- The handoff assumes developers will treat the delivery-contract block as authoritative over the legacy draft text at the bottom of the description, which still says 'Run or collect the benchmark evidence'.
- The handoff also assumes this ticket can be completed as documentation/evidence ratification even though the current branch diff versus develop contains only ticket metadata updates and no repository doc edits yet.

AC / test suggestions
- When the ticket is completed, cite at least one concrete matrix row using scenario/provider/baseline/posture plus the matching closure-bundle subdirectory so the closure rationale stays auditable.
- If no repository doc edits are ultimately needed, record that explicitly in the completion evidence so the task is not later mistaken for a missed benchmark rerun.

Implementation watchouts
- Do not treat skipped optional-provider rows in the root benchmark-summary triplet as missing external-provider benchmark evidence; use them only as quick baseline and row-identity guidance.
- Do not cite PIT or bridge read timings as PIT maintenance timing evidence; docs/performance-profiles.md and docs/architecture/dvault-v1-pit-bridge-boundary.md both preserve that boundary, and DB2 PIT maintenance remains future child work while Oracle remains deferred.
- Keep scope limited to documentation/evidence publication and ratification; do not reopen closed provider save/read rows or expand into provider runtime code, benchmark reruns, staged DB2 bulk, provider-native chunk execution, or bridge-maintenance push-down.

Non-blocking notes
- none

Split recommendations
- No further split is needed for save/latest-satellite/PIT/bridge evidence publication in this ticket; keep the current save/read/documentation split intact.
- If the team wants more work now, create at most one separate DB2 PIT maintenance child limited to the IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync lane through IDataVaultProviderPitMaintenanceStrategy.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment