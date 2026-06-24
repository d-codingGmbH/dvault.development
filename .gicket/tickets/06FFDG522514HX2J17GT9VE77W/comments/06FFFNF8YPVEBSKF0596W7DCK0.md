[gicket-bot] PO refinement contract

Summary
- Verified the current ticket against live repository and ticket state. The scope is already bounded to the accepted official-MySQL ordinary hub-parent full-rebuild lane, and no child tickets, relation changes, attachments, or planning documents were needed in this PO pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Source ticket 06FF43CJ9CJMG7J917RW22QKJC is done and already fixed the implementation boundary: use the existing provider strategy seam, keep the lane official MySql.EntityFrameworkCore only for now, and defer Pomelo validation, multi-active PITs, link-parent PITs, and maintenance timing.
- Repository code confirms AddDVaultMySql() currently registers MySQL save and read strategies only, while AddDVaultPostgres() registers IDataVaultProviderPitMaintenanceStrategy and AddDVaultSqlServer() replaces IDataVaultPitMaintenanceService; the MySQL ticket should follow the Postgres-style strategy seam, not the SQL Server service-replacement path.
- No child tickets, relation updates, description writes, attachments, or planning documents were materialized in this pass.

Scope In
- Add one MySQL IDataVaultProviderPitMaintenanceStrategy for RebuildAsync(...) full rebuilds through DefaultDataVaultPitMaintenanceService.
- Register that strategy from AddDVaultMySql() without replacing IDataVaultPitMaintenanceService or altering existing MySQL save/read registrations.
- Support only clean DbContext, ordinary hub-parent, non-multi-active PIT full rebuilds backed by the official MySql.EntityFrameworkCore provider name and complete generated maintenance-shape evidence.
- Extend MySQL PIT-maintenance fallback diagnostics to surface provider mismatch, unknown or unregistered provider, dirty DbContext, incomplete maintenance-shape evidence, unsupported PIT shape, and rollback or savepoint boundary decline causes.
- Prove rollback-clean behavior for strategy-owned local transactions and provider-neutral fallback when an ambient or current transaction cannot provide a verified savepoint boundary.
- Add tests that keep MaintainParentsAsync(...) provider-neutral.

Scope Out
- MaintainParentsAsync(...) push-down.
- Pomelo.EntityFrameworkCore.MySql live maintenance validation.
- Shared-driving-key multi-active hub-parent PIT full rebuilds.
- Link-parent PIT full rebuilds.
- Benchmark-backed PIT maintenance timing.
- Bridge maintenance push-down, automatic maintenance, read-time refresh, SaveChanges interception, or background scheduling.

Open questions
- none

Follow-up questions
- After the official MySql.EntityFrameworkCore lane lands, should Pomelo live PIT-maintenance validation be tracked as its own follow-up ticket before any Pomelo maintenance claim is made?
- After the ordinary hub-parent lane lands, is there enough value to open separate follow-ups for shared-driving-key multi-active hub-parent PIT rebuilds and link-parent PIT rebuilds, or should those remain deferred until a broader provider-maintenance expansion pass?
- Should benchmark-backed MySQL PIT maintenance timing be scheduled only after rollback and cancellation parity is in place, matching the current evidence policy?

Risks
- The current generic PIT-maintenance fallback vocabulary and known-strategy evaluator are narrower than the ticket's accepted MySQL decline surface, so implementation will touch shared maintenance diagnostics rather than only adding one provider class.
- Rollback-clean behavior under ambient or current transactions may diverge between MySQL providers; the accepted lane is safe only if the implementation proves local-transaction rollback and declines unverified savepoint participation.
- The repository's live MySQL evidence currently tracks MySql.EntityFrameworkCore, not Pomelo PIT maintenance execution, so widening claims beyond the official provider would overstate the checked-in evidence.
- Existing completed MySQL PIT read timing could still be misquoted as maintenance evidence unless the docs and tests keep the read and write boundary explicit.

Split recommendations
- No additional split is required for this ticket; the current scope is already the smallest accepted implementation slice.
- Keep Pomelo live maintenance validation, multi-active hub-parent maintenance, link-parent maintenance, and benchmark-backed maintenance timing as separate downstream follow-ups rather than expanding this implementation ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment