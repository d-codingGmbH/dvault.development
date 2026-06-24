[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the delivery contract is specific, internally consistent, has no open questions, and is directly backed by current ticket, relation, documentation, source, and test evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF437W1CHG9QVJPGZM4Y98AR/description.md contains `## Open Questions` -> `none` and acceptance criteria that explicitly separate `pit-full-rebuild-maintenance` from `pit-as-of-read` and `bridge-traversal-read`.
- Ticket comments `06FFQG9TPTVM3WRXK844THE5B0.md` and `06FFQGMX2P24RJ8D19VZ9MJ9ZG.md` record PO handoff `ready_for_po_critic` and runtime outcome `po-refinement-ready`.
- `git show --stat --summary HEAD^..HEAD` on branch `ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c` shows current HEAD `45185e5702e3d78b1a979c46305b1c3dd74e7146` is only the PO-critic lease-claim metadata update under `.gicket/tickets/06FF437W1CHG9QVJPGZM4Y98AR/`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:27`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:31`, and `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:22` match the contract: PostgreSQL/MySQL register `IDataVaultProviderPitMaintenanceStrategy`; SQL Server replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`.
- `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` and `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` register save/read/PIT-read/bridge-read services only; `docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md` states DB2 still uses `DefaultDataVaultPitMaintenanceService` provider-neutral maintenance today.
- `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/performance-evidence-benchmark-artifact-contract.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md`, `docs/performance-profiles.md`, `docs/releases/v0.47.0.md`, and `CHANGELOG.md` all align on the same boundary: PIT maintenance timing uses `pit-full-rebuild-maintenance` plus the benchmark triplet/run context, while PIT/bridge read rows remain read-side evidence only.
- `src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs` and `src/DCoding.Data.DVault.SqlServer/SqlServerPitMaintenanceFallbackCauseKind.cs` expose the bounded fallback vocabularies named in the contract, and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs` cover the documented provider gates, manifest mapping, and fallback behavior.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assumes the v0.47 documentation surfaces remain the intended handoff baseline and are not superseded by a later PO edit before development starts.

AC / test suggestions
- Keep dev-handoff validation tied to the exact manifest tokens already named in the contract: `maintenanceScope=FullRebuild`, `selectedStrategy`, `fallbackCauses`, `pitShapeBoundary`, and `readShape=null` for PIT maintenance rows.
- Keep one explicit acceptance/test check that Oracle and DB2 wording stays non-timing / provider-neutral unless a dedicated PIT-maintenance artifact triplet is added.

Implementation watchouts
- Do not treat `pit-as-of-read` or `bridge-traversal-read` benchmark rows as PIT maintenance timing evidence.
- SQL Server scope is clean ordinary hub-parent `RebuildAsync(...)` only; `MaintainParentsAsync(...)`, link-parent, multi-active, dirty-context, provider-mismatch, and no-savepoint cases remain fallback/non-goal behavior.
- MySQL PIT maintenance scope is official `MySql.EntityFrameworkCore` ordinary hub-parent full rebuilds only; Pomelo remains outside the maintenance-timing claim surface.
- Oracle remains deferred and DB2 remains provider-neutral/future-lane only for this story's accepted scope.

Non-blocking notes
- The current branch head is a PO-critic lease-claim commit rather than an implementation commit, which is normal for this pre-development PO quality gate.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment