<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Confirmed this is a normal implementation ticket, not a closure-only claim; kept scope on the official MySql.EntityFrameworkCore ordinary hub-parent full-rebuild maintenance lane and made no child-ticket, relation, attachment, or planning-document writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Treat this as a normal implementation handoff, not as landed closure evidence.
- The accepted lane is official MySql.EntityFrameworkCore only on the existing provider-strategy seam; Pomelo remains provider-neutral fallback for this ticket.
- No child tickets, relation changes, attachments, or planning documents were materialized in this PO pass.

### Scope In
- Add one MySQL IDataVaultProviderPitMaintenanceStrategy for RebuildAsync(...) full rebuilds through DefaultDataVaultPitMaintenanceService.
- Register it from AddDVaultMySql() without replacing IDataVaultPitMaintenanceService or disturbing existing MySQL save/read registrations.
- Select it only for clean DbContext, complete maintenance-shape evidence, ordinary hub-parent, non-multi-active PIT full rebuilds, and the official MySql.EntityFrameworkCore provider name.
- Extend PIT-maintenance fallback diagnostics for provider mismatch, unknown or unregistered provider, dirty DbContext, incomplete evidence, unsupported PIT shape, and rollback/savepoint boundary decline; Pomelo must fall back provider-neutrally.
- Add tests and docs that prove rollback-clean local transactions, require verified savepoints for ambient/current transactions, keep MaintainParentsAsync(...) provider-neutral, and keep MySQL PIT read timing separate from maintenance proof.

### Scope Out
- MaintainParentsAsync(...) push-down.
- Pomelo.EntityFrameworkCore.MySql live maintenance execution or maintenance-selection support.
- Shared-driving-key multi-active hub-parent PIT full rebuilds.
- Link-parent PIT full rebuilds.
- Benchmark-backed MySQL PIT maintenance timing claims.
- Bridge maintenance push-down, automatic maintenance, read-time refresh, SaveChanges interception, or background scheduling.

## Acceptance Criteria
- AddDVaultMySql() registers a MySQL IDataVaultProviderPitMaintenanceStrategy while preserving existing MySQL save, latest-satellite read, PIT read, and bridge read registrations.
- DefaultDataVaultPitMaintenanceService selects the MySQL strategy only for clean official MySql.EntityFrameworkCore ordinary hub-parent full rebuilds with complete evidence; Pomelo, other providers, dirty contexts, incomplete evidence, unsupported PIT shapes, MaintainParentsAsync(...), and unverified ambient/current transaction boundaries fall back provider-neutrally.
- The MySQL gate exposes explicit decline causes, including unknown or unregistered provider names and rollback/savepoint boundary failure, instead of a generic strategy decline.
- Tests and docs prove parity for the accepted lane, provider-neutral fallback for deferred shapes and Pomelo, rollback-clean fault/cancellation behavior, and the read-evidence-versus-maintenance-evidence boundary.

## Definition of Done
- Source changes add the MySQL provider PIT-maintenance strategy, registration, gate evaluation, and fallback-cause coverage for the accepted lane.
- Unit coverage exists for MySQL registration, gate acceptance/decline, and provider-bound SQL or command-plan shape.
- Integration coverage exists for official MySql.EntityFrameworkCore rebuild success plus rollback, fault, and cancellation behavior on the accepted ordinary hub-parent shape, while MaintainParentsAsync(...) remains provider-neutral and tested.
- Architecture and performance docs keep claims limited to the official-provider maintenance lane and continue to defer Pomelo validation, broader PIT shapes, and timing claims.

## Implementation Notes
- Use IDataVaultProviderPitMaintenanceStrategy and DefaultDataVaultPitMaintenanceService as the seam; follow the Postgres registration model rather than the SQL Server service-replacement model.
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs currently has no PIT-maintenance registration; add it without disturbing existing MySQL save or read behavior.
- src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs currently exposes EvaluatePostgres(...) and only recognizes PostgresDataVaultPitMaintenanceStrategy; extend known-strategy evaluation for MySQL while keeping MaintainParentsAsync(...) provider-neutral.
- src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs currently lacks explicit rollback/savepoint decline causes; add MySQL-visible fallback vocabulary and do not widen maintenance selection beyond official MySql.EntityFrameworkCore.

## Open Questions
- none

## Follow-Up Questions
- After the official MySql.EntityFrameworkCore lane lands, should Pomelo live PIT-maintenance validation become its own follow-up ticket before any Pomelo maintenance claim is made?
- After the ordinary hub-parent lane lands, should multi-active hub-parent and link-parent PIT rebuilds become separate follow-up tickets or stay deferred?
- Should benchmark-backed MySQL PIT maintenance timing wait until rollback and cancellation parity is in place?

## Risks
- The shared PIT-maintenance gate and fallback vocabulary are narrower than the accepted MySQL decline surface, so the implementation touches shared diagnostics as well as MySQL-specific registration.
- Rollback-clean behavior under ambient or current transactions may differ across MySQL providers; the accepted lane is safe only if local-transaction rollback is proven and unverified savepoint participation declines cleanly.
- MySQL save/read capability registration already covers multiple provider names, so maintenance selection must not widen beyond official MySql.EntityFrameworkCore.
- Existing MySQL PIT read timing could be misquoted as maintenance evidence unless tests and docs keep the read/write boundary explicit.

## Split Recommendations
- No technical split is needed if the ticket stays on the normal implementation path; the current slice is already the smallest justified lane.
- If product later insists on a closure-only treatment, split the real implementation into a separate dev ticket and keep this ticket strictly evidence-only.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Purpose
Implement the accepted MySQL PIT maintenance lane from ticket `06FF43CJ9CJMG7J917RW22QKJC`: provider-specific `IDataVaultPitMaintenanceService.RebuildAsync(...)` push-down for clean ordinary hub-parent PIT full rebuilds only.

## Source Decision
The evaluation accepted only an initial ordinary hub-parent MySQL full-rebuild implementation slice. Shared-driving-key multi-active hub-parent PITs, link-parent non-multi-active PITs, Pomelo live validation, and benchmark-backed maintenance timing remain separate follow-ups. Existing MySQL PIT read timing is not maintenance push-down proof.

## Scope In
- Add a MySQL `IDataVaultProviderPitMaintenanceStrategy` through the existing provider strategy seam.
- Register the strategy from `AddDVaultMySql()` without replacing `IDataVaultPitMaintenanceService`.
- Support `MySql.EntityFrameworkCore` ordinary hub-parent, non-multi-active PIT full rebuilds with a clean `DbContext` and complete maintenance-shape evidence.
- Extend PIT-maintenance gate diagnostics so MySQL fallback causes cover provider-name mismatch, unknown provider, dirty context, incomplete maintenance-shape evidence, unsupported PIT shape, and rollback/savepoint boundary failures.
- Prove rollback behavior for local transactions and require provider-neutral fallback when an ambient caller transaction cannot provide a verified rollback-clean savepoint boundary.
- Add source and test coverage that keeps `MaintainParentsAsync(...)` provider-neutral.

## Scope Out
- `MaintainParentsAsync(...)` push-down.
- Shared-driving-key multi-active hub-parent PIT full rebuilds.
- Link-parent PIT full rebuilds.
- Pomelo live maintenance validation.
- Benchmark-backed maintenance timing.
- Bridge maintenance, automatic maintenance, read-time refresh, `SaveChanges` interception, or background scheduling.

## Acceptance Criteria
- `AddDVaultMySql()` registers a MySQL PIT maintenance strategy while preserving existing save and read registrations.
- The default PIT maintenance service selects the MySQL strategy only for the accepted ordinary hub-parent full-rebuild shape and falls back for all scoped-out shapes.
- Tests prove provider-neutral fallback for provider mismatch, dirty contexts, incomplete maintenance-shape evidence, multi-active PITs, link-parent PITs, `MaintainParentsAsync(...)`, and caller transactions without verified savepoint support.
- Tests prove fault or cancellation does not leave partially refreshed PIT rows when the strategy owns the transaction or uses a verified savepoint.
- Documentation remains clear that existing MySQL PIT read timing is not maintenance push-down evidence.