<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded MySQL PIT maintenance evaluation scoped to full-rebuild push-down only, grounded in the current parent/blocks relation context and the repository's existing PostgreSQL/SQL Server PIT-maintenance baseline.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current repository baseline already limits provider-specific PIT maintenance push-down to PostgreSQL via `IDataVaultProviderPitMaintenanceStrategy` and SQL Server via `SqlServerDataVaultPitMaintenanceService`.
- `AddDVaultMySql()` currently registers MySQL save and latest-satellite/PIT/bridge read strategies only; no MySQL PIT maintenance registration is present today.

### Scope In
- Evaluate feasibility of provider-specific MySQL support for `IDataVaultPitMaintenanceService.RebuildAsync(...)` full rebuilds only.
- Assess the current `AddDVaultMySql()` surface for both supported EF Core MySQL provider names: `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`.
- Compare candidate MySQL full-rebuild support with the existing PostgreSQL and SQL Server PIT-maintenance boundaries.
- Document required diagnostics, fallback gates, transaction behavior, and rollback expectations for any accepted MySQL maintenance lane.

### Scope Out
- `MaintainParentsAsync(...)` push-down.
- Automatic PIT maintenance, read-time refresh, `SaveChanges` interception, background scheduling, or bridge maintenance push-down.
- PIT/bridge read optimization or read-timing work, which is already covered by existing MySQL read evidence.
- Implementing provider code changes or widening non-MySQL provider behavior in this ticket.

## Acceptance Criteria
- The ticket records the current repository baseline that MySQL already has provider-specific save/read registration and completed PIT read evidence, but no provider-specific PIT maintenance path.
- The evaluation explicitly states whether feasibility is shared across both supported MySQL EF Core provider names or whether live repository evidence only proves one package surface today.
- The evaluation names which bounded full-rebuild PIT shapes are acceptable, deferred, or rejected for MySQL, at minimum covering ordinary hub-parent PITs, shared-driving-key multi-active hub-parent PITs, and link-parent non-multi-active PITs.
- The evaluation records transaction and rollback caveats, including whether MySQL can preserve pre-rebuild PIT rows on fault or cancellation or would need a narrower fallback boundary.
- The evaluation states whether a bounded MySQL lane should use the existing provider PIT-maintenance strategy seam or would require a SQL Server-style service replacement, with reasons tied to repository evidence.
- The ticket ends with an explicit implementation or defer recommendation that the blocked matrix task `06FF43F283QFQ56290AVJ3AXSM` can consume directly.

## Definition of Done
- A ticket-visible outcome summarizes accepted, deferred, and rejected MySQL PIT full-rebuild maintenance shapes, provider caveats, fallback boundaries, and the final recommendation.
- The outcome distinguishes repository-proven facts from inference and does not treat existing MySQL PIT read timing as maintenance push-down proof.
- The recommendation is concrete enough for the parent story and blocked matrix task to reuse without reopening ticket-level scope questions.

## Implementation Notes
- Use `docs/architecture/dvault-v1-pit-bridge-boundary.md`, `docs/performance-profiles.md`, and `docs/plans/provider-optimization-gap-matrix.md` as the authoritative behavior and evidence boundaries.
- Start from the current repository fact that `AddDVaultMySql()` registers `MySqlDataVaultSaveStrategy` plus `MySqlDataVaultReadStrategy`, but no `IDataVaultProviderPitMaintenanceStrategy` or MySQL-specific `IDataVaultPitMaintenanceService` replacement.
- The generic provider-maintenance extension seam already exists through `IDataVaultProviderPitMaintenanceStrategy`, but `DataVaultProviderPitMaintenanceStrategyGateEvaluator` currently contains known-strategy evaluation only for `PostgresDataVaultPitMaintenanceStrategy`; any MySQL strategy proposal must account for that gap.
- Use the SQL Server PIT-maintenance service as the comparator for rollback-clean full rebuild behavior, especially around caller transactions and savepoint availability.
- Repository live integration and benchmark package surfaces currently reference `MySql.EntityFrameworkCore`; Pomelo has provider-profile and diagnostic coverage in tests, but no visible live MySQL integration or benchmark package lane in the repository snapshot.
- Phrase the outcome so it can feed the already-related matrix task instead of requiring a second synthesis pass.

## Open Questions
- none

## Follow-Up Questions
- If MySQL full-rebuild push-down is accepted only for the official `MySql.EntityFrameworkCore` package, should Pomelo validation stay out of the initial implementation slice and move to a separate follow-up ticket?
- If MySQL full-rebuild push-down is accepted, should benchmark-backed maintenance timing remain a separate follow-up from the initial source/test/diagnostics implementation, matching the current PIT-maintenance evidence policy?

## Risks
- The repository currently proves live MySQL behavior through `MySql.EntityFrameworkCore`; widening maintenance claims to Pomelo without a live lane would rely on inference rather than direct evidence.
- MySQL transaction or savepoint behavior may not support SQL Server-style rollback-clean full rebuild guarantees, which could force a narrower accepted slice or a defer recommendation.
- Existing completed MySQL PIT read evidence could be misread as maintenance evidence unless the ticket explicitly keeps read-side timing separate from write-side push-down proof.

## Split Recommendations
- Do not split this evaluation further now; the parent story already decomposes provider-specific feasibility work.
- If the evaluation recommends implementation, create a separate bounded implementation ticket for only the accepted MySQL full-rebuild shape and keep any benchmark-backed maintenance-timing work as a distinct follow-up.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Investigate whether MySQL can safely support bounded PIT full-rebuild push-down through EF Core provider APIs. Acceptance: records supported/unsupported shapes, transaction caveats, SQL shape risks, and explicit implementation/defer recommendation.