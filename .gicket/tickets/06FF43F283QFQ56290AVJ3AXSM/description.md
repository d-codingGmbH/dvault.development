<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh repository and .gicket inspection confirms the matrix can be finalized now: MySQL and DB2 each have one accepted future ordinary hub-parent full-rebuild lane through the provider PIT-maintenance strategy seam, Oracle stays deferred, and no PO-blocking questions remain.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current runtime baseline remains unchanged: PostgreSQL and SQL Server are the only provider-native PIT maintenance implementations today; AddDVaultMySql(), AddDVaultOracle(), and AddDVaultDb2() still leave PIT maintenance on the provider-neutral service.
- Completed PIT read timing for MySQL, Oracle, and DB2 is read-side evidence over already-maintained PIT rows and must not be cited as write-side PIT maintenance proof.
- Fresh inspection confirmed the MySQL, Oracle, and DB2 evaluation tickets are done; the live incoming blocks relations into this ticket are historical completion context, not an active blocker, because this ticket is not marked blocked.
- Fresh inspection confirmed MySQL follow-up ticket 06FFDG522514HX2J17GT9VE77W already exists; no Oracle implementation ticket is justified while Oracle stays deferred, and no matching DB2 implementation ticket was visible in .gicket/tickets.
- No persistent planning writes were materialized during this refinement; the contract below therefore ratifies the decision matrix and explicitly recommends the missing DB2 child ticket instead of claiming it already exists.

### Scope In
- Document one provider decision matrix for MySQL, Oracle, and DB2 PIT full-rebuild maintenance using the completed evaluation tickets and repository architecture/evidence docs.
- State accepted, deferred, and rejected or fallback-only PIT shapes per provider, including provider lane, seam choice, rollback gate, and provider-neutral fallback boundary.
- Name follow-up implementation tickets only where a bounded implementation lane is accepted.

### Scope Out
- Implementing MySQL, Oracle, or DB2 PIT maintenance code in this ticket.
- Reopening the existing PostgreSQL or SQL Server PIT maintenance baseline.
- Treating PIT or bridge read strategy registration, smoke coverage, or completed read timing as PIT maintenance proof.
- Oracle or DB2 benchmark-backed PIT maintenance timing claims, bridge maintenance push-down, automatic maintenance, or read-time refresh behavior.

## Acceptance Criteria
- The matrix records the current baseline that MySQL, Oracle, and DB2 register provider-specific save/read surfaces but do not yet ship provider-specific PIT maintenance; provider-neutral PIT maintenance remains the runtime path until accepted implementation tickets land.
- The MySQL row accepts only clean ordinary hub-parent full rebuilds first on MySql.EntityFrameworkCore, defers Pomelo live validation, shared-driving-key multi-active hub-parent, and link-parent non-multi-active shapes, and rejects or falls back for MaintainParentsAsync(...), dirty contexts, provider mismatch, incomplete shape evidence, and unproven savepoint cases.
- The Oracle row states a defer decision: no Oracle PIT maintenance implementation ticket is created now, and any future reopen is limited to an ordinary hub-parent full-rebuild-only candidate after Oracle-specific SQL parity and rollback-clean failure or cancellation proof.
- The DB2 row accepts only one future IBM.EntityFrameworkCore ordinary hub-parent full-rebuild lane through IDataVaultProviderPitMaintenanceStrategy, defers shared-driving-key multi-active and link-parent expansion, and keeps MaintainParentsAsync(...), dirty-context, provider-mismatch, incomplete-shape, and unproven savepoint cases on provider-neutral fallback.
- Each provider row distinguishes evidence status clearly: current provider registration, available read-side smoke or timing evidence, and the absence of write-side PIT maintenance proof.
- Follow-up tickets are named only for accepted bounded work: existing MySQL ticket 06FFDG522514HX2J17GT9VE77W and one bounded DB2 implementation ticket to be created; Oracle gets no implementation child while deferred.

## Definition of Done
- The ticket-authoritative outcome gives one matrix-ready disposition per provider and per relevant PIT shape family.
- The outcome ratifies provider-neutral fallback as the current runtime behavior for MySQL, Oracle, and DB2 until accepted implementation tickets land, and it keeps read-side evidence separate from maintenance claims.
- No PO-blocking question remains after documenting provider lane, shape classification, rollback gate, and evidence status.

## Implementation Notes
- Use docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/performance-profiles.md, docs/plans/provider-optimization-gap-matrix.md, and docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md as the canonical repository evidence surfaces.
- Use the done MySQL evaluation ticket 06FF43CJ9CJMG7J917RW22QKJC plus the existing child ticket 06FFDG522514HX2J17GT9VE77W as the authoritative MySQL source for the accepted ordinary hub-parent lane and the deferred or rejected shapes.
- Use .gicket/tickets/06FF43DC469VQ1N0NQ84KEV6SR/comments/06FFE0MK5F4C1Z8ACDJV9RMY9C.md as the authoritative Oracle defer outcome: ordinary hub-parent remains only a plausible future candidate, while multi-active, link-parent, and parent-maintenance shapes stay provider-neutral.
- Use docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md and .gicket/tickets/06FF43E0JCE7BSBFBWB49HGB4G/comments/06FFE7Z82S5VBRXM0THDBEK7YW.md as the authoritative DB2 source for the accepted IBM.EntityFrameworkCore ordinary hub-parent lane and the deferred or fallback-only shapes.
- Keep architecture wording explicit: accepted MySQL and DB2 work uses the existing provider PIT-maintenance strategy seam, not a SQL Server-style IDataVaultPitMaintenanceService replacement.
- Carry forward the rollback gate for every accepted lane: strategy-owned local transactions must preserve pre-rebuild PIT rows on fault or cancellation, and ambient caller transactions must fall back unless provider-specific savepoint behavior is source-, test-, and live-provider-proven.

## Open Questions
- none

## Follow-Up Questions
- After critic approval, should the owner branch materialize the missing DB2 implementation ticket immediately so this matrix can cite a concrete child id beside MySQL ticket 06FFDG522514HX2J17GT9VE77W?
- When downstream docs ticket 06FF43JEA6C3HNJ6AQA9XY7EC8 resumes, should it cite the MySQL child ticket and the future DB2 child ticket separately from the deferred Oracle row?

## Risks
- Oracle has completed read-side PIT evidence but still lacks PIT maintenance implementation, diagnostics, SQL parity proof, and rollback-clean failure proof; the matrix must not over-promote Oracle from read timing to write-side feasibility.
- MySQL and DB2 accepted lanes remain conditional on rollback-clean delete-plus-insert behavior and provider-specific savepoint limits; overstating ambient-transaction support would widen scope beyond current evidence.
- Live incoming blocks relations from the done MySQL, Oracle, and DB2 evaluation tickets can confuse downstream readers unless the matrix explicitly treats them as historical completion context rather than active blockers.

## Split Recommendations
- Reuse existing MySQL implementation ticket 06FFDG522514HX2J17GT9VE77W as the only MySQL follow-up child for this matrix.
- Create one DB2 implementation ticket limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy.
- Do not create an Oracle implementation child ticket from this matrix; keep Oracle on provider-neutral PIT maintenance until a separate Oracle-specific reopen proves SQL parity and rollback-clean behavior.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Summarize MySQL, Oracle, and DB2 PIT maintenance feasibility outcomes. Acceptance: matrix names accepted, deferred, and rejected shapes with evidence status and follow-up tickets only for accepted bounded work.