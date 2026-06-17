[gicket-bot] PO refinement contract

Summary
- Applied the parent delivery-contract rewrite to ratify the existing five provider-specific PIT/bridge follow-up tickets, and queued child-description updates so PostgreSQL, SQL Server, MySQL, and Oracle explicitly target provider-configured timing evidence while DB2 is deferred into planning-only follow-up.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent contract now endorses the five already-persisted provider-specific downstream tickets as the authoritative split and supersedes the earlier grouped PostgreSQL plus SQL Server / MySQL plus Oracle recommendation.
- critic-item-2: `answered` - DB2 is explicitly deferred out of active v0.41 implementation follow-up; existing ticket 06FBSCH65R88BT6PS7XV32NQ1M is now the deferred planning surface for any later DB2 evidence approval rather than an implement-or-reject execution ticket.
- critic-item-3: `answered` - The PostgreSQL, SQL Server, MySQL, and Oracle child tickets now have queued description rewrites that frame them as provider-configured PIT/bridge timing evidence for existing strategy candidates, not generic implement-or-reject work.
- critic-item-4: `answered` - The persisted ticket graph remains the five provider-specific downstream tickets; the mismatch was only in the parent split narrative, and the applied parent rewrite now ratifies that live graph instead of a hypothetical grouped split.
- critic-item-5: `answered` - The DB2 child no longer remains an ambiguous implement-or-reject follow-up; its queued description rewrite aligns it with the parent ticket's defer and planning-only DB2 posture.
- critic-item-6: `answered` - The downstream topology is now authoritative because the parent ticket ratifies the existing five-child split and every child has aligned scope text; downstream needs-po state now reflects each child ticket's own refinement flow, not unresolved ambiguity in the audit ticket.

Clarifications
- The authoritative follow-up split is the already-persisted five-ticket provider graph: PostgreSQL 06FBSCGGN528A2NC6TTA5A99X0, SQL Server 06FBSCGNY2R6PC7P4Y91RD0HVR, MySQL 06FBSCGVAZ5G8NP1TRXFNEP6DW, Oracle 06FBSCH0M358R5J3RGFB6GRDM4, and DB2 06FBSCH65R88BT6PS7XV32NQ1M.
- PostgreSQL, SQL Server, MySQL, and Oracle remain evidence-completion tickets for already-registered PIT and bridge strategy candidates; they are not new public API or strategy-design tickets.
- DB2 remains deferred out of the v0.41 implementation batch and uses the existing DB2 child as planning-only follow-up for any later explicit DB2 evidence approval.
- No relation rewrites, attachments, planning documents, or new child tickets were needed because the existing provider-specific graph was already the correct topology.

Scope In
- Ratify the persisted five-child provider split as the authoritative PIT and bridge follow-up plan.
- Align parent and child ticket descriptions with provider-configured PIT and bridge timing-evidence scope for PostgreSQL, SQL Server, MySQL, and Oracle.
- Carry forward the bounded DB2 defer posture as planning-only follow-up.
- Preserve SQLite as a no-op audit baseline and keep PIT and bridge architecture boundaries tied to the checked-in repository evidence.

Scope Out
- Runtime code changes to PIT or bridge reads, provider dispatch, diagnostics, or maintenance services.
- Benchmark reruns, external database provisioning, connection-string setup, or new measured provider timing claims in this ticket.
- Any regrouping of the already-persisted provider-specific child graph into new combined tickets.
- DB2 boundary expansion, non-SQLite latest-satellite work, or save-strategy follow-up outside this PIT and bridge audit.

Open questions
- none

Follow-up questions
- Which configured PostgreSQL, SQL Server, MySQL, and Oracle environments should stay available for repeat PIT and bridge benchmark reruns after the first evidence pass?
- When the queued child-description mutations replay on their owner branches, should each child ticket immediately re-enter PO-critic or wait until the implementing owner is ready to pick up provider-specific evidence work?
- If DB2 evidence work is later approved, should the team first reopen the narrower v0.34 DB2 boundary explicitly or keep the DB2 child strictly as evidence-planning without implementation scope?

Risks
- The five child-description updates are durable queued mutations on their owner branches; until replay completes, the canonical child branches may briefly lag the parent contract text.
- External-provider PIT and bridge evidence still depends on configured connection strings and benchmark infrastructure, so the active-provider child tickets can still stall after refinement.
- A downstream ticket could over-claim performance if it treats skipped-placeholder or diagnostics-only rows as completed timing evidence without new benchmark artifact triplets.

Split recommendations
- Use existing child 06FBSCGGN528A2NC6TTA5A99X0 for PostgreSQL provider-configured PIT and bridge timing evidence against PostgresDataVaultReadStrategy.
- Use existing child 06FBSCGNY2R6PC7P4Y91RD0HVR for SQL Server provider-configured PIT and bridge timing evidence against SqlServerDataVaultReadStrategy.
- Use existing child 06FBSCGVAZ5G8NP1TRXFNEP6DW for MySQL provider-configured PIT and bridge timing evidence against MySqlDataVaultReadStrategy.
- Use existing child 06FBSCH0M358R5J3RGFB6GRDM4 for Oracle provider-configured PIT and bridge timing evidence against OracleDataVaultReadStrategy.
- Use existing child 06FBSCH65R88BT6PS7XV32NQ1M only as deferred DB2 planning until explicit DB2 evidence scope and environment-backed benchmark work are approved.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment