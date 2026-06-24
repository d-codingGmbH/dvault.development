[gicket-bot] PO-critic review contract

Summary
- Ticket 06FF438KMPKSBT6KXZ5DBY85QC is now a clear pre-development documentation task: the prior closure-only routing issue was corrected, the maintenance-row scope is bounded, and the persisted contract has no open PO questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF438KMPKSBT6KXZ5DBY85QC/description.md reclassifies the work as an implementation task, keeps the scope on the existing evidence-contract docs, and shows ## Open Questions as none.
- .gicket/tickets/06FF438KMPKSBT6KXZ5DBY85QC/comments/06FFJ6HXA2VCD0VG185PNB2BBC.md explicitly answers the prior PO-critic findings by rejecting closure-only handling and stating that the next step is development work on the contract docs.
- docs/plans/provider-optimization-evidence-matrix.md currently has a Read Matrix with latest-satellite-read, pit-as-of-read, and bridge-traversal-read rows, but no separate PIT full-rebuild maintenance row family yet, which matches the ticket's remaining implementation scope.
- The same matrix's Deferred Bridge Maintenance Push-Down section keeps bridge maintenance out of scope and states that PIT maintenance support is limited to PostgreSQL full rebuild and a narrower SQL Server full rebuild boundary.
- docs/plans/performance-evidence-benchmark-artifact-contract.md already fixes the shared benchmark vocabulary the ticket wants to reuse, including scenario, provider, baseline, executionDetail, preserved artifact triplet, and run context.
- Direct source evidence matches the claimed provider boundary: src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs registers IDataVaultProviderPitMaintenanceStrategy; src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs allows PostgreSQL rebuilds for supported hub-parent and non-multi-active link-parent shapes; src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs falls back for maintain-parents, multi-active PITs, link-parent PITs, dirty contexts, and no-savepoint transactions.
- git diff --name-only HEAD~1..HEAD on branch ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi shows only .gicket ticket files changed at the current tip, which is consistent with a pre-development handoff rather than claimed closure evidence.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- This approval assumes the existing benchmark-artifact vocabulary is sufficient for maintenance rows; if maintenance-row token mapping proves ambiguous during implementation, this same ticket should update docs/plans/performance-evidence-benchmark-artifact-contract.md as already allowed by the contract.
- This approval assumes sibling tickets 06FF43BPP5NRJR3JTY48ZNEKHM, 06FF43AH9SK6J07GV5EKYV3AMM, and 06FF43AYQYZKFF400CK5Q84WYR will reuse the same maintenance scenario naming and artifact-link conventions so later rows stay comparable.

AC / test suggestions
- Verify the updated matrix adds a distinct PIT full-rebuild maintenance scenario family instead of reusing pit-as-of-read or bridge-traversal-read rows.
- Verify the docs state that skipped-placeholder, diagnostics-only, smoke-only, and docs-only guidance rows are not completed maintenance timing claims without a preserved artifact triplet and run context.
- Cross-check the final PostgreSQL and SQL Server maintenance wording against src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs and src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs so the documentation does not widen provider shape support.

Implementation watchouts
- Keep the work on the existing matrix/supporting-contract surfaces; do not create a parallel maintenance document.
- Keep bridge maintenance and MaintainParentsAsync(...) out of scope for this ticket.
- Do not invent a new maintenance-specific fallback taxonomy in prose; reuse bounded fallback facts and provider-neutral fallback posture from the existing contract surfaces.

Non-blocking notes
- Sibling tickets 06FF43BPP5NRJR3JTY48ZNEKHM, 06FF43AH9SK6J07GV5EKYV3AMM, and 06FF43AYQYZKFF400CK5Q84WYR already exist as separate todo tasks, so the intended split is already present in the ticket system.

Split recommendations
- No additional split recommended; the provider-neutral comparator lane plus the PostgreSQL and SQL Server provider-specific lanes are already separated into sibling tickets 06FF43BPP5NRJR3JTY48ZNEKHM, 06FF43AH9SK6J07GV5EKYV3AMM, and 06FF43AYQYZKFF400CK5Q84WYR.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment