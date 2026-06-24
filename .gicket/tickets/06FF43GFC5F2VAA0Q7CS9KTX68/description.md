<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository review shows PostgreSQL PIT full rebuild currently reuses an ambient caller transaction without a strategy-owned savepoint or an explicit decline path, while current docs and tests do not prove rollback-clean caller-transaction behavior. This refinement bounds the ticket to either add safe savepoint-backed behavior or explicitly decline and document that transaction boundary.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current source evidence in src/DCoding.Data.DVault.Postgres/PostgresDataVaultPitMaintenanceStrategy.cs opens a local transaction only when no current transaction exists; when a caller transaction is already active it reuses that DbTransaction directly and only rolls back locally owned transactions.
- Current PostgreSQL PIT-maintenance gate evidence in src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs covers provider-name, dirty-context, shape, and incomplete-evidence checks only; it does not currently express a caller-transaction/savepoint guard.
- Current PostgreSQL PIT-maintenance tests in tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs prove supported-shape happy paths only and do not prove rollback-clean fault/cancellation behavior inside an ambient caller transaction.
- Current documentation in docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/performance-profiles.md, and docs/releases/v0.45.0.md describes PostgreSQL clean full-rebuild support without qualifying caller-transaction rollback behavior.

### Scope In
- Review and tighten PostgreSQL provider-specific PIT full-rebuild transaction behavior for IDataVaultPitMaintenanceService.RebuildAsync(...).
- Add one explicit safe boundary for ambient caller transactions: either a strategy-owned savepoint rollback path or a documented provider-neutral decline/fallback path when rollback-clean behavior cannot be proven.
- Add the required gate/diagnostic, tests, and documentation updates so PostgreSQL PIT maintenance does not silently imply rollback-clean caller-transaction safety it has not proven.
- Preserve the existing PostgreSQL supported full-rebuild PIT shape matrix unless the transaction-safety review proves a narrower safe lane is required.

### Scope Out
- Changing provider-neutral PIT maintenance semantics or the explicit caller-owned PIT-maintenance model.
- Changing MaintainParentsAsync(...) scope or adding PostgreSQL provider-specific maintain-parents work.
- Adding new bridge-maintenance, MySQL, or SQL Server feature work beyond using them as precedent for the PostgreSQL decision.
- Adding benchmark-backed PIT-maintenance performance claims; this ticket is about transaction safety and boundary clarity, not timing promotion.

## Acceptance Criteria
- PostgreSQL provider-specific PIT full rebuilds must not execute delete-plus-insert work inside an ambient caller transaction unless the implementation can restore the pre-rebuild PIT rows on fault or cancellation through a proven strategy-owned savepoint or equivalent rollback-clean mechanism.
- If rollback-clean ambient-transaction support is not implemented, AddDVaultPostgres() must decline that provider-specific rebuild boundary and fall back to provider-neutral maintenance with an explicit diagnostic/fallback cause rather than silently claiming the optimized path is safe.
- Repository tests must cover the reviewed ambient-transaction outcome: either prove savepoint-backed rollback-clean behavior on fault/cancellation or prove provider-neutral fallback for the declined caller-transaction boundary, while keeping existing supported-shape rebuild parity coverage intact.
- Current documentation that describes PostgreSQL PIT-maintenance support must be updated to match the chosen boundary and must not imply caller-transaction safety beyond what the source and tests prove.

## Definition of Done
- The PostgreSQL PIT-maintenance selection and execution path makes ambient caller-transaction behavior explicit and deterministic instead of implicit.
- Unit-level gate or diagnostic coverage exists for the transaction/savepoint decision so the fallback reason is visible and regression-testable.
- Integration coverage exists for the chosen behavior under a configured PostgreSQL provider, in addition to the existing supported-shape rebuild coverage.
- Authoritative current-branch documentation is aligned with the shipped behavior and no longer overstates PostgreSQL PIT full-rebuild safety under caller transactions.

## Implementation Notes
- src/DCoding.Data.DVault.Postgres/PostgresDataVaultPitMaintenanceStrategy.cs is the current source of concern: it reuses Database.CurrentTransaction?.GetDbTransaction() and does not create or roll back to a strategy-owned savepoint, unlike the SQL Server precedent in src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs.
- src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs should gain an explicit PostgreSQL transaction-boundary guard and a distinct fallback cause instead of burying the behavior under a generic guard-failure story.
- tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs currently proves happy-path supported shapes only; extend it with an ambient-transaction scenario and either a fault/cancellation rollback assertion or an explicit provider-neutral fallback assertion.
- Keep the existing PostgreSQL supported-shape baseline bounded to ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active full rebuilds unless the transaction review shows one of those shapes cannot safely share the chosen boundary.
- Keep MaintainParentsAsync(...) on its current provider-neutral path; this ticket is specifically about PostgreSQL RebuildAsync(...) full-rebuild transaction safety.
- Update the PostgreSQL PIT-maintenance wording in docs/architecture/dvault-v1-pit-bridge-boundary.md and docs/performance-profiles.md at minimum, and update any additional current-branch release/baseline narrative that still implies broader PostgreSQL transaction safety than the repository proves.

## Open Questions
- none

## Follow-Up Questions
- If PostgreSQL initially declines ambient caller transactions instead of adding savepoint support now, should a later parity ticket add rollback-clean savepoint support for that lane once the bounded implementation and live evidence are justified?
- After this ticket lands, should the PIT-maintenance fallback vocabulary be normalized across PostgreSQL and future MySQL provider work so savepoint-related decline reasons share one documented contract?

## Risks
- If the implementation changes the PostgreSQL gate to decline some ambient-transaction scenarios, callers may see more provider-neutral fallback than before; diagnostics and docs must make that behavior intentional and visible.
- If savepoint-backed support is chosen, the repository needs live Npgsql integration evidence for fault and cancellation behavior; otherwise the docs could still overstate safety.
- Transaction-boundary changes must not regress the already-proven PostgreSQL supported-shape rebuild coverage for ordinary, shared-driving-key multi-active, or link-parent non-multi-active PIT full rebuilds.

## Split Recommendations
- No split recommended; the transaction-boundary decision, diagnostics, tests, and documentation updates are one bounded refinement and should stay in a single ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Review caller-transaction behavior for PostgreSQL PIT full rebuilds. Acceptance: either add safe savepoint/fallback behavior or document and test the declined transaction boundary so partial maintenance cannot be silently claimed as safe.