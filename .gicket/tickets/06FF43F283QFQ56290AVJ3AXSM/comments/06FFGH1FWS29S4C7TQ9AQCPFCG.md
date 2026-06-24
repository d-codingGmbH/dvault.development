[gicket-bot] PO refinement contract

Summary
- Fresh repository and .gicket inspection confirms the matrix can be finalized now: MySQL and DB2 each have one accepted future ordinary hub-parent full-rebuild lane through the provider PIT-maintenance strategy seam, Oracle stays deferred, and no PO-blocking questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current runtime baseline remains unchanged: PostgreSQL and SQL Server are the only provider-native PIT maintenance implementations today; AddDVaultMySql(), AddDVaultOracle(), and AddDVaultDb2() still leave PIT maintenance on the provider-neutral service.
- Completed PIT read timing for MySQL, Oracle, and DB2 is read-side evidence over already-maintained PIT rows and must not be cited as write-side PIT maintenance proof.
- Fresh inspection confirmed the MySQL, Oracle, and DB2 evaluation tickets are done; the live incoming blocks relations into this ticket are historical completion context, not an active blocker, because this ticket is not marked blocked.
- Fresh inspection confirmed MySQL follow-up ticket 06FFDG522514HX2J17GT9VE77W already exists; no Oracle implementation ticket is justified while Oracle stays deferred, and no matching DB2 implementation ticket was visible in .gicket/tickets.
- No persistent planning writes were materialized during this refinement; the contract below therefore ratifies the decision matrix and explicitly recommends the missing DB2 child ticket instead of claiming it already exists.

Scope In
- Document one provider decision matrix for MySQL, Oracle, and DB2 PIT full-rebuild maintenance using the completed evaluation tickets and repository architecture/evidence docs.
- State accepted, deferred, and rejected or fallback-only PIT shapes per provider, including provider lane, seam choice, rollback gate, and provider-neutral fallback boundary.
- Name follow-up implementation tickets only where a bounded implementation lane is accepted.

Scope Out
- Implementing MySQL, Oracle, or DB2 PIT maintenance code in this ticket.
- Reopening the existing PostgreSQL or SQL Server PIT maintenance baseline.
- Treating PIT or bridge read strategy registration, smoke coverage, or completed read timing as PIT maintenance proof.
- Oracle or DB2 benchmark-backed PIT maintenance timing claims, bridge maintenance push-down, automatic maintenance, or read-time refresh behavior.

Open questions
- none

Follow-up questions
- After critic approval, should the owner branch materialize the missing DB2 implementation ticket immediately so this matrix can cite a concrete child id beside MySQL ticket 06FFDG522514HX2J17GT9VE77W?
- When downstream docs ticket 06FF43JEA6C3HNJ6AQA9XY7EC8 resumes, should it cite the MySQL child ticket and the future DB2 child ticket separately from the deferred Oracle row?

Risks
- Oracle has completed read-side PIT evidence but still lacks PIT maintenance implementation, diagnostics, SQL parity proof, and rollback-clean failure proof; the matrix must not over-promote Oracle from read timing to write-side feasibility.
- MySQL and DB2 accepted lanes remain conditional on rollback-clean delete-plus-insert behavior and provider-specific savepoint limits; overstating ambient-transaction support would widen scope beyond current evidence.
- Live incoming blocks relations from the done MySQL, Oracle, and DB2 evaluation tickets can confuse downstream readers unless the matrix explicitly treats them as historical completion context rather than active blockers.

Split recommendations
- Reuse existing MySQL implementation ticket 06FFDG522514HX2J17GT9VE77W as the only MySQL follow-up child for this matrix.
- Create one DB2 implementation ticket limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy.
- Do not create an Oracle implementation child ticket from this matrix; keep Oracle on provider-neutral PIT maintenance until a separate Oracle-specific reopen proves SQL parity and rollback-clean behavior.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment