[gicket-bot] PO refinement contract

Summary
- Repository review shows PostgreSQL PIT full rebuild currently reuses an ambient caller transaction without a strategy-owned savepoint or an explicit decline path, while current docs and tests do not prove rollback-clean caller-transaction behavior. This refinement bounds the ticket to either add safe savepoint-backed behavior or explicitly decline and document that transaction boundary.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current source evidence in src/DCoding.Data.DVault.Postgres/PostgresDataVaultPitMaintenanceStrategy.cs opens a local transaction only when no current transaction exists; when a caller transaction is already active it reuses that DbTransaction directly and only rolls back locally owned transactions.
- Current PostgreSQL PIT-maintenance gate evidence in src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs covers provider-name, dirty-context, shape, and incomplete-evidence checks only; it does not currently express a caller-transaction/savepoint guard.
- Current PostgreSQL PIT-maintenance tests in tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs prove supported-shape happy paths only and do not prove rollback-clean fault/cancellation behavior inside an ambient caller transaction.
- Current documentation in docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/performance-profiles.md, and docs/releases/v0.45.0.md describes PostgreSQL clean full-rebuild support without qualifying caller-transaction rollback behavior.

Scope In
- Review and tighten PostgreSQL provider-specific PIT full-rebuild transaction behavior for IDataVaultPitMaintenanceService.RebuildAsync(...).
- Add one explicit safe boundary for ambient caller transactions: either a strategy-owned savepoint rollback path or a documented provider-neutral decline/fallback path when rollback-clean behavior cannot be proven.
- Add the required gate/diagnostic, tests, and documentation updates so PostgreSQL PIT maintenance does not silently imply rollback-clean caller-transaction safety it has not proven.
- Preserve the existing PostgreSQL supported full-rebuild PIT shape matrix unless the transaction-safety review proves a narrower safe lane is required.

Scope Out
- Changing provider-neutral PIT maintenance semantics or the explicit caller-owned PIT-maintenance model.
- Changing MaintainParentsAsync(...) scope or adding PostgreSQL provider-specific maintain-parents work.
- Adding new bridge-maintenance, MySQL, or SQL Server feature work beyond using them as precedent for the PostgreSQL decision.
- Adding benchmark-backed PIT-maintenance performance claims; this ticket is about transaction safety and boundary clarity, not timing promotion.

Open questions
- none

Follow-up questions
- If PostgreSQL initially declines ambient caller transactions instead of adding savepoint support now, should a later parity ticket add rollback-clean savepoint support for that lane once the bounded implementation and live evidence are justified?
- After this ticket lands, should the PIT-maintenance fallback vocabulary be normalized across PostgreSQL and future MySQL provider work so savepoint-related decline reasons share one documented contract?

Risks
- If the implementation changes the PostgreSQL gate to decline some ambient-transaction scenarios, callers may see more provider-neutral fallback than before; diagnostics and docs must make that behavior intentional and visible.
- If savepoint-backed support is chosen, the repository needs live Npgsql integration evidence for fault and cancellation behavior; otherwise the docs could still overstate safety.
- Transaction-boundary changes must not regress the already-proven PostgreSQL supported-shape rebuild coverage for ordinary, shared-driving-key multi-active, or link-parent non-multi-active PIT full rebuilds.

Split recommendations
- No split recommended; the transaction-boundary decision, diagnostics, tests, and documentation updates are one bounded refinement and should stay in a single ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment