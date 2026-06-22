[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the delivery contract is now source-backed, `## Open Questions` is `none`, and the defer/no-child recommendation matches current repository and relation evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Earlier blocker comment `06FF15SEAN3WK6NKHQ0W3Y1F28.md:11-22` cited unresolved open questions and inferred API claims; current contract at `.gicket/tickets/06FE4RK80ZXGCZ62CMSAYP164W/description.md:41-50` replaces that with source-backed implementation notes and `## Open Questions` = `none`.
- `git show --stat --oneline HEAD` at `7d19ab9440` and `git diff --name-only a2d2dc06bdb491f0472a30e859a64cc8994a146a..HEAD` show this branch changes only `.gicket/tickets/06FE4RK80ZXGCZ62CMSAYP164W/**`, so this remains a pre-development ticket-contract branch rather than an implementation branch.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-27` registers `IDataVaultProviderPitMaintenanceStrategy` via `PostgresDataVaultPitMaintenanceStrategy`, while `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:21-27` registers save/read strategies only.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:28-31` registers provider-neutral `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`, and `src/DCoding.Data.DVault/IDataVaultBridgeMaintenanceService.cs:16-31` exposes only `RebuildBridgeAsync(...)` and `MaintainBridgeAsync(...)`.
- `rg -n IDataVaultProviderBridgeMaintenanceStrategy src tests docs` returned no matches, which supports the contract's claim that no bridge-provider maintenance seam is present in current source.
- `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs:45-52,64-89` only recognizes `PostgresDataVaultPitMaintenanceStrategy` and carries PIT-specific provider/fallback vocabulary; no analogous bridge evaluator exists.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs:15-67,181-245,248-339` covers many-to-many rebuild plus append-only maintenance, hierarchy shortest-path lowering, topology shrink that needs rebuild to increase `TraversalDepth`, and cycle handling without implicit self rows.
- `docs/performance-profiles.md:54,81-90,415-457` and `docs/plans/provider-optimization-evidence-matrix.md:10,44-48` already treat bridge optimization evidence as maintained-read evidence, not write-side bridge-maintenance push-down proof.
- `.gicket/tickets/06FE4RK80ZXGCZ62CMSAYP164W/events/06FE4RMGBT1P6QC6KH37RGBCG0.json` records `06FE4RK80ZXGCZ62CMSAYP164W` blocking `06FE4RKGASKV6F7DF0RD1WTAV4`, and comment `06FF1Y6Y55RM9QXE8D9N7PQZSG.md:10-19` shows the owner-branch follow-up for that docs ticket is queued.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The reopen threshold is only implied through follow-up wording about later hotspot evidence in `description.md:53-55`; the downstream docs ticket should turn that into one crisp declarative gate.

Risky assumptions
- Readers may still conflate maintained-bridge read benchmarks with write-side bridge push-down feasibility unless the follow-on docs explicitly restate that read-path wins are not maintenance-path proof.

AC / test suggestions
- When `06FE4RKGASKV6F7DF0RD1WTAV4` is refined, cite the exact current-source evidence for the PIT seam in `DVaultPostgresServiceCollectionExtensions.cs` and `PostgresProviderCapabilityTests.cs`, and the bridge-semantic breadth in `DataVaultBridgeMaintenanceServiceSqliteTests.cs`, instead of restating them generically.

Implementation watchouts
- Any future bridge push-down prototype would need new core/provider API, registration, and bridge-specific gate/fallback diagnostics; nothing in current source proves an existing provider-maintenance seam for bridges.
- This ticket branch is `.gicket`-only from merge-base to HEAD, so any later code work should happen in a separate implementation ticket rather than widening this feasibility task.

Non-blocking notes
- `docs/architecture/dvault-v1-pit-bridge-boundary.md:108-110` still says provider-specific PIT or bridge maintenance strategies are unsupported in v1, which conflicts with current source evidence for PostgreSQL PIT maintenance and reinforces the need for the documentation follow-on ticket.

Split recommendations
- Keep `06FE4RKGASKV6F7DF0RD1WTAV4` as the immediate downstream documentation task; do not open a bridge implementation child from this ticket now.
- If the area reopens later, split first by PostgreSQL many-to-many full rebuild versus hierarchy rebuild, and keep incremental/delete-aware maintenance, diagnostics/deployment surfaces, and non-PostgreSQL providers out of the first slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment