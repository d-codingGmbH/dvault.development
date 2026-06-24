[gicket-bot] PO-critic review contract

Summary
- Repo and ticket evidence agree on a bounded pre-development task: add the missing SQL Server PIT full-rebuild maintenance benchmark lane against an already-proven service boundary and existing artifact contract.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 10 comments, all bot claim/lease/refinement/handover entries; no newer discussion reopens scope after PO refinement.
- `git rev-parse HEAD` returned `603b91e007a1d3a5b17061510ddff3f60d0d5b29`, and `git show --stat --oneline --no-patch HEAD` shows `[06FF43AYQYZKFF400CK5Q84WYR] lease claim po-critic`, so this is still a pre-development review state.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`; `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs` limits provider-specific rebuilds to clean SQL Server ordinary hub-parent PITs and sends `MaintainParentsAsync(...)` to provider-neutral fallback; existing unit and smoke coverage lives in `tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs`.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md`, `docs/plans/provider-optimization-evidence-matrix.md`, and `docs/architecture/dvault-v1-pit-bridge-boundary.md` already define `pit-full-rebuild-maintenance` as a separate row family, require `maintenanceScope=FullRebuild`, and name `SqlServerDataVaultPitMaintenanceService` plus `selectedStrategy=<none>` as the SQL Server/fallback execution-detail contract.
- `git grep -n "pit-full-rebuild-maintenance" -- benchmark-summary.md benchmark-summary.csv benchmark-summary.json` returned no matches, while the current root triplet already contains skipped SQL Server rows for `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not pin the exact final `datasetSize` and `changeRatio` text for the new maintenance row; dev/test should keep those literals deterministic and identical across md/csv/json.

Risky assumptions
- Delivery will reuse the same ordinary hub-parent PIT workload already exercised by the existing SQL Server maintenance smoke/unit tests, not a wider new PIT shape.
- The comparator row will preserve provider-neutral posture via `selectedStrategy=<none>` and bounded fallback-cause tokens instead of copying PIT-read execution-detail conventions.

AC / test suggestions
- Add artifact-level assertions that `pit-full-rebuild-maintenance` appears in `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` with matching scenario/provider/baseline identity.
- Cover the unconfigured-provider case so SQL Server maintenance rows stay visible with `iterations=0`, blank or null metrics, deterministic execution detail, and `persistedOutcome=not executed` when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is absent.
- Assert that the optimized row surfaces `SqlServerDataVaultPitMaintenanceService` and the comparator row surfaces provider-neutral execution with `selectedStrategy=<none>`.

Implementation watchouts
- Do not widen the lane beyond clean ordinary hub-parent full rebuilds; `MaintainParentsAsync(...)`, multi-active PITs, link-parent PITs, dirty contexts, provider mismatch, and no-savepoint caller transactions are already repo-defined fallback or non-goal paths.
- Do not model the optimized lane as provider-strategy registration; the repository evidence shows SQL Server uses a service-replacement boundary.
- Do not let PIT read rows stand in for PIT maintenance evidence.

Non-blocking notes
- Current HEAD is the po-critic lease-claim commit, so lack of implementation evidence on this branch is expected at this gate.
- The ticket is specific enough for developer handoff because the public API/type boundary, artifact contract, and provider-scope exclusions are all directly repo-backed.

Split recommendations
- Keep any checked-in provider-configured SQL Server artifact capture as a separate follow-up after the lane lands.
- Keep PostgreSQL PIT maintenance timing and broader provider-maintenance expansion out of this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment