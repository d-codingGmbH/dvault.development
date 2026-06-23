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