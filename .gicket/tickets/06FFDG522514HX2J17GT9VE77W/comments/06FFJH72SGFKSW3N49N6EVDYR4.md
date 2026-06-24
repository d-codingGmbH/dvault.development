[gicket-bot] PO refinement contract

Summary
- Confirmed this is a normal implementation ticket, not a closure-only claim; kept scope on the official MySql.EntityFrameworkCore ordinary hub-parent full-rebuild maintenance lane and made no child-ticket, relation, attachment, or planning-document writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This ticket stays on the normal dev path after PO-critic. Its contract still requires new source, test, and documentation work, so the closure-only runtime routing is the inconsistency to clear.
- critic-item-2: `answered` - The ticket must not remain closure-only. There is no landed implementation evidence or delivered implementation commit to cite, so the correct contract is bounded implementation work.
- critic-item-3: `answered` - The provider boundary remains explicit: only official MySql.EntityFrameworkCore ordinary hub-parent full rebuilds may select the MySQL strategy. Pomelo, multi-active hub-parent PITs, link-parent PITs, and maintenance timing claims stay deferred.
- critic-item-4: `answered` - The closure-only mismatch is resolved by ratifying the ticket as implementation work only. This refinement does not claim landed closure evidence and keeps the future source/test/doc obligations intact.
- critic-item-5: `answered` - There is no closure-only evidence on this branch. The delta is ticket metadata only, and the live code still lacks MySQL PIT-maintenance registration plus the shared MySQL gate and fallback coverage promised by the contract.

Clarifications
- Treat this as a normal implementation handoff, not as landed closure evidence.
- The accepted lane is official MySql.EntityFrameworkCore only on the existing provider-strategy seam; Pomelo remains provider-neutral fallback for this ticket.
- No child tickets, relation changes, attachments, or planning documents were materialized in this PO pass.

Scope In
- Add one MySQL IDataVaultProviderPitMaintenanceStrategy for RebuildAsync(...) full rebuilds through DefaultDataVaultPitMaintenanceService.
- Register it from AddDVaultMySql() without replacing IDataVaultPitMaintenanceService or disturbing existing MySQL save/read registrations.
- Select it only for clean DbContext, complete maintenance-shape evidence, ordinary hub-parent, non-multi-active PIT full rebuilds, and the official MySql.EntityFrameworkCore provider name.
- Extend PIT-maintenance fallback diagnostics for provider mismatch, unknown or unregistered provider, dirty DbContext, incomplete evidence, unsupported PIT shape, and rollback/savepoint boundary decline; Pomelo must fall back provider-neutrally.
- Add tests and docs that prove rollback-clean local transactions, require verified savepoints for ambient/current transactions, keep MaintainParentsAsync(...) provider-neutral, and keep MySQL PIT read timing separate from maintenance proof.

Scope Out
- MaintainParentsAsync(...) push-down.
- Pomelo.EntityFrameworkCore.MySql live maintenance execution or maintenance-selection support.
- Shared-driving-key multi-active hub-parent PIT full rebuilds.
- Link-parent PIT full rebuilds.
- Benchmark-backed MySQL PIT maintenance timing claims.
- Bridge maintenance push-down, automatic maintenance, read-time refresh, SaveChanges interception, or background scheduling.

Open questions
- none

Follow-up questions
- After the official MySql.EntityFrameworkCore lane lands, should Pomelo live PIT-maintenance validation become its own follow-up ticket before any Pomelo maintenance claim is made?
- After the ordinary hub-parent lane lands, should multi-active hub-parent and link-parent PIT rebuilds become separate follow-up tickets or stay deferred?
- Should benchmark-backed MySQL PIT maintenance timing wait until rollback and cancellation parity is in place?

Risks
- The shared PIT-maintenance gate and fallback vocabulary are narrower than the accepted MySQL decline surface, so the implementation touches shared diagnostics as well as MySQL-specific registration.
- Rollback-clean behavior under ambient or current transactions may differ across MySQL providers; the accepted lane is safe only if local-transaction rollback is proven and unverified savepoint participation declines cleanly.
- MySQL save/read capability registration already covers multiple provider names, so maintenance selection must not widen beyond official MySql.EntityFrameworkCore.
- Existing MySQL PIT read timing could be misquoted as maintenance evidence unless tests and docs keep the read/write boundary explicit.

Split recommendations
- No technical split is needed if the ticket stays on the normal implementation path; the current slice is already the smallest justified lane.
- If product later insists on a closure-only treatment, split the real implementation into a separate dev ticket and keep this ticket strictly evidence-only.

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