[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff. The contract is current, `## Open Questions` is `none`, and repository evidence confirms the PostgreSQL PIT maintenance capability exists while the `pit-full-rebuild-maintenance` benchmark lane is still missing.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF43AH9SK6J07GV5EKYV3AMM/description.md` contains the PostgreSQL-only delivery contract and `## Open Questions` -> `none`.
- `rg -n pit-full-rebuild-maintenance /mnt/c/Projects/DVault/tests /mnt/c/Projects/DVault/src /mnt/c/Projects/DVault/benchmark-summary.md /mnt/c/Projects/DVault/benchmark-summary.json` returned no matches, confirming the lane is absent from code, tests, and the root artifact triplet at branch head `5045c4614d`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` registers `IDataVaultProviderPitMaintenanceStrategy` via `PostgresDataVaultPitMaintenanceStrategy`, and `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` bounds that optimized path to provider-name match, clean DbContext, no current transaction, complete maintenance-shape evidence, and supported PIT shapes.
- `benchmark-summary.md` currently contains PostgreSQL skipped placeholder rows for `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read`, but no `pit-full-rebuild-maintenance` row family; `git diff --name-only c153fe42fb...5045c4614d` lists only `.gicket/tickets/06FF43AH9SK6J07GV5EKYV3AMM/**` files, consistent with a pre-development ticket state.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Keep the three approved PostgreSQL full-rebuild shapes deterministically identifiable in row metadata or `executionDetail` so later evidence can cite which boundary shape ran.
- Preserve both skipped-placeholder rows when PostgreSQL is unconfigured and provider-neutral fallback rows when the PostgreSQL path is declined by transaction, dirty-context, or unsupported-shape gates.

Risky assumptions
- Treating `pit-as-of-read` or `bridge-traversal-read` rows as PIT maintenance evidence would violate the shared benchmark contract.
- Assuming the PostgreSQL optimized path can run inside an ambient caller transaction would conflict with the current `CurrentTransactionSavepointUnavailable` fallback gate.

AC / test suggestions
- Verifier coverage should assert scenario `pit-full-rebuild-maintenance`, provider `PostgreSQL external provider`, baselines `dvault-adddvault-fallback` and `dvault-adddvaultpostgres-optimized`, and required `maintenanceScope=FullRebuild` / `selectedStrategy` tokens.
- Skipped-placeholder coverage should assert `iterations=0`, blank or null metrics, normalized Postgres skip reason, and deterministic planned execution detail when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is absent.
- Fallback-path coverage should assert `selectedStrategy=<none>` and bounded PIT maintenance `fallbackCauses` when the optimized PostgreSQL path is declined.

Implementation watchouts
- Do not widen this ticket to SQL Server/MySQL/Oracle/DB2 lanes, bridge maintenance, automatic PIT refresh, or `MaintainParentsAsync(...)`.
- Do not add maintenance-specific artifact files or schema columns; reuse the existing `benchmark-summary.md`, `.csv`, and `.json` triplet.

Non-blocking notes
- The follow-up question about future SQL Server scenario-family reuse is post-landing and is not an unresolved `## Open Questions` blocker.

Split recommendations
- Keep SQL Server PIT full-rebuild benchmarking as a sibling ticket because its runtime seam and fallback vocabulary differ from PostgreSQL.
- Keep any future MySQL, Oracle, or DB2 PIT maintenance benchmarking separate until those provider lanes are explicitly implemented or accepted.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment