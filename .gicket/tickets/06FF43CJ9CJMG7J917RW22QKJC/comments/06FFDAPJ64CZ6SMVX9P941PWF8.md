[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded MySQL PIT maintenance evaluation scoped to full-rebuild push-down only, grounded in the current parent/blocks relation context and the repository's existing PostgreSQL/SQL Server PIT-maintenance baseline.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository baseline already limits provider-specific PIT maintenance push-down to PostgreSQL via `IDataVaultProviderPitMaintenanceStrategy` and SQL Server via `SqlServerDataVaultPitMaintenanceService`.
- `AddDVaultMySql()` currently registers MySQL save and latest-satellite/PIT/bridge read strategies only; no MySQL PIT maintenance registration is present today.

Scope In
- Evaluate feasibility of provider-specific MySQL support for `IDataVaultPitMaintenanceService.RebuildAsync(...)` full rebuilds only.
- Assess the current `AddDVaultMySql()` surface for both supported EF Core MySQL provider names: `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`.
- Compare candidate MySQL full-rebuild support with the existing PostgreSQL and SQL Server PIT-maintenance boundaries.
- Document required diagnostics, fallback gates, transaction behavior, and rollback expectations for any accepted MySQL maintenance lane.

Scope Out
- `MaintainParentsAsync(...)` push-down.
- Automatic PIT maintenance, read-time refresh, `SaveChanges` interception, background scheduling, or bridge maintenance push-down.
- PIT/bridge read optimization or read-timing work, which is already covered by existing MySQL read evidence.
- Implementing provider code changes or widening non-MySQL provider behavior in this ticket.

Open questions
- none

Follow-up questions
- If MySQL full-rebuild push-down is accepted only for the official `MySql.EntityFrameworkCore` package, should Pomelo validation stay out of the initial implementation slice and move to a separate follow-up ticket?
- If MySQL full-rebuild push-down is accepted, should benchmark-backed maintenance timing remain a separate follow-up from the initial source/test/diagnostics implementation, matching the current PIT-maintenance evidence policy?

Risks
- The repository currently proves live MySQL behavior through `MySql.EntityFrameworkCore`; widening maintenance claims to Pomelo without a live lane would rely on inference rather than direct evidence.
- MySQL transaction or savepoint behavior may not support SQL Server-style rollback-clean full rebuild guarantees, which could force a narrower accepted slice or a defer recommendation.
- Existing completed MySQL PIT read evidence could be misread as maintenance evidence unless the ticket explicitly keeps read-side timing separate from write-side push-down proof.

Split recommendations
- Do not split this evaluation further now; the parent story already decomposes provider-specific feasibility work.
- If the evaluation recommends implementation, create a separate bounded implementation ticket for only the accepted MySQL full-rebuild shape and keep any benchmark-backed maintenance-timing work as a distinct follow-up.

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