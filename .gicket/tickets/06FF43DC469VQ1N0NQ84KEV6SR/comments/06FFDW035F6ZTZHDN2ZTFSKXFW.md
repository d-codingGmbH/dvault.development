[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted delivery contract is current, has no open questions, and matches the repository's actual Oracle/Postgres/SQL Server PIT-maintenance surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `/mnt/c/Projects/DVault/.gicket/tickets/06FF43DC469VQ1N0NQ84KEV6SR/description.md` currently contains `## Open Questions` -> `- none`, plus acceptance criteria requiring shape-by-shape Oracle feasibility, transaction/savepoint risk capture, and an explicit implement-or-defer recommendation.
- `/mnt/c/Projects/DVault/src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` registers Oracle provider capability, save strategy, and read/PIT-read/bridge-read strategies; `rg -n "IDataVaultPitMaintenanceService|IDataVaultProviderPitMaintenanceStrategy|PitMaintenance" /mnt/c/Projects/DVault/src/DCoding.Data.DVault.Oracle -S` returned no Oracle PIT maintenance implementation matches.
- `/mnt/c/Projects/DVault/src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` registers `IDataVaultProviderPitMaintenanceStrategy` via `PostgresDataVaultPitMaintenanceStrategy`, while `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`, matching the comparison baseline named in the ticket.
- `/mnt/c/Projects/DVault/src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` directly encodes PostgreSQL full-rebuild gating for provider match, clean context, complete shape evidence, hub-or-link parents, non-empty satellites, distinct satellite references, and link-parent non-multi-active constraints; `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs` limits SQL Server to ordinary hub-parent full rebuilds and falls back for `MaintainParentsAsync(...)`, multi-active, link-parent, dirty-context, and no-savepoint cases.
- `/mnt/c/Projects/DVault/docs/architecture/dvault-v1-pit-bridge-boundary.md` states the accepted PIT maintenance push-down baseline is intentionally asymmetric: PostgreSQL has a provider strategy, SQL Server has a narrower service replacement with rollback-clean behavior, and provider paths remain explicit caller-invoked fallbacks rather than automatic maintenance.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `50cf61ea67e961f3788150bfe91cb6ee36ee4e8d`, and `git -C /mnt/c/Projects/DVault log --oneline --decorate -n 3 -- .gicket/tickets/06FF43DC469VQ1N0NQ84KEV6SR src/DCoding.Data.DVault.Oracle docs/architecture/dvault-v1-pit-bridge-boundary.md src/DCoding.Data.DVault.Postgres/PostgresDataVaultPitMaintenanceStrategy.cs src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` shows the PO handoff commit `8f6e364017` followed by PO-critic lease/claim commits, so this branch is still at the expected pre-development review stage.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If the investigation finds a narrow Oracle candidate, have it explicitly say whether ambient caller transactions without savepoint-equivalent rollback must always fall back to provider-neutral maintenance.
- If unsupported Oracle shapes are narrower than PostgreSQL's current gate, have the final output call out duplicate-satellite, zero-satellite, and link-parent multi-active cases as explicit fallback territory instead of leaving them implicit.

Risky assumptions
- Oracle PIT read strategy and read benchmark evidence are not sufficient proof of PIT rebuild push-down safety; the ticket correctly treats them as comparison context only.
- Oracle EF Core transaction surfaces may not provide SQL Server-style rollback-clean failure behavior for full rebuilds, so an implementation recommendation could require a different seam or a defer outcome.
- The existing provider-strategy seam may or may not fit Oracle cleanly; the ticket already preserves the option that Oracle could need SQL Server-style service ownership instead of a PostgreSQL-style strategy.

AC / test suggestions
- Ask the investigation output to state one explicit disposition per shape: ordinary hub-parent full rebuild, shared-driving-key multi-active hub-parent full rebuild, link-parent non-multi-active full rebuild, and any maintain-parents scope.
- Have the investigation cite the exact repository baselines it compared against: `PostgresDataVaultPitMaintenanceStrategy`, `SqlServerDataVaultPitMaintenanceService`, `DataVaultProviderPitMaintenanceStrategyGateEvaluator`, and `docs/architecture/dvault-v1-pit-bridge-boundary.md`.
- Require the final recommendation to say what diagnostics/fallback behavior remains for unsupported Oracle requests so downstream implementation does not widen the maintenance boundary by accident.

Implementation watchouts
- Keep PIT maintenance explicit and caller-owned; no read-time refresh, `SaveChanges` interception, startup automation, or background scheduling.
- Do not let Oracle read-optimization artifacts be used as substitute proof for maintenance push-down viability.
- Any Oracle provider path must preserve provider-neutral fallback on provider mismatch, dirty context, incomplete shape evidence, unsupported PIT shapes, and rollback-clean failure/cancellation concerns.

Non-blocking notes
- `/mnt/c/Projects/DVault/.gicket/relations/SR/SM/06FF43DC469VQ1N0NQ84KEV6SR--06FF43F283QFQ56290AVJ3AXSM--blocks.json` shows this investigation ticket currently blocks downstream ticket `06FF43F283QFQ56290AVJ3AXSM`, which increases the value of a bounded recommendation but does not add PO ambiguity.

Split recommendations
- No split is needed before development; keep this ticket as the bounded investigation and open a separate implementation ticket only if the investigation proves a narrowly guarded Oracle full-rebuild candidate.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment