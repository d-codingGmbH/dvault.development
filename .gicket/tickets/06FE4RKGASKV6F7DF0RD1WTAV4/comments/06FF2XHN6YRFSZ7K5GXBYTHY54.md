[gicket-bot] PO-critic review contract

Summary
- Ticket 06FE4RKGASKV6F7DF0RD1WTAV4 is clear and evidence-backed for a bounded documentation follow-through on the current PIT maintenance prototypes, deferred bridge-maintenance push-down posture, and missing v0.45.0 release-note surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06FE4RKGASKV6F7DF0RD1WTAV4 marks `PO Handoff` as `ready_for_po_critic` and `## Open Questions` as `none`; the contract scopes only docs/architecture, docs/performance, docs/releases/v0.45.0.md, and CHANGELOG.md.
- Local git inspection confirmed branch `ticket/06FE4RKGASKV6F7DF0RD1WTAV4-task-update-pit-and-bridge-push-down-architectur` at commit `0ecba2751b761211af72ab43877a88ec3aac7764`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` registers `IDataVaultProviderPitMaintenanceStrategy` via `PostgresDataVaultPitMaintenanceStrategy`; `tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs` directly cover ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PIT rebuild support.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`; `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs` and `tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs` show SQL Server accepts only clean ordinary hub-parent rebuilds and falls back for `MaintainParentsAsync`, link-parent, multi-active, provider mismatch, dirty context, and no-savepoint cases.
- `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs` verifies SQL Server ordinary PIT rebuild parity with provider-neutral output plus rollback-clean behavior on failure and cancellation.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers provider-neutral `IDataVaultBridgeMaintenanceService` to `DefaultDataVaultBridgeMaintenanceService`, `rg` found no `IDataVaultProviderBridgeMaintenanceStrategy`, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs` covers many-to-many rebuild and incremental maintenance, hierarchy shortest-path lowering, topology-shrink rebuild, and no implicit self rows.
- Current repo state still needs the documented follow-through: `docs/releases/v0.45.0.md` is absent, `CHANGELOG.md` starts at `v0.44.0`, `docs/architecture/dvault-v1-pit-bridge-boundary.md` still names only the PostgreSQL PIT maintenance seam in its bridge-maintenance posture text, and `docs/performance-profiles.md` still carries a v0.43.0 and v0.44.0 baseline.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Call out the SQL Server edge cases already proven in repo evidence: `MaintainParentsAsync` stays provider-neutral, link-parent PITs stay unsupported for SQL Server push-down, multi-active PITs stay unsupported, and failure or cancellation must leave pre-rebuild rows intact.
- Keep bridge-maintenance semantics explicit in the docs: hierarchy topology shrink requires rebuild, incremental bridge maintenance is not delete-aware, and maintained-bridge read timing is not bridge write-side push-down evidence.

Risky assumptions
- Assumes v0.45.0 should follow the existing release-label pattern where the release note and changelog record the exploration outcome without widening into the separate README and package-version alignment sweep for `8.45.0` and `10.45.0`.
- Assumes the docs will distinguish the two PIT maintenance seams correctly: PostgreSQL is a provider strategy registration, while SQL Server is a service replacement with narrower eligibility.

AC / test suggestions
- Cite the exact source and test evidence already named in the contract so the docs do not regress into generic prose: `DVaultPostgresServiceCollectionExtensions.cs`, `PostgresProviderCapabilityTests.cs`, `PostgresPitMaintenanceServiceTests.cs`, `DVaultSqlServerServiceCollectionExtensions.cs`, `SqlServerDataVaultPitMaintenanceService.cs`, `SqlServerDataVaultSmokeTests.cs`, `DVaultServiceCollectionExtensions.cs`, and `DataVaultBridgeMaintenanceServiceSqliteTests.cs`.
- Keep the release note and performance guide anchored to `docs/plans/provider-optimization-evidence-matrix.md` for evidence posture, so prototype availability is not promoted into a new benchmark claim without an artifact triplet.

Implementation watchouts
- Do not document SQL Server as if it registered `IDataVaultProviderPitMaintenanceStrategy`; the repository shows a replacement of `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService` instead.
- Do not describe bridge maintenance as provider-specific or imply an `IDataVaultProviderBridgeMaintenanceStrategy` seam; the provider-neutral bridge service is still the only write-side surface in the repo.
- Do not blur PIT maintenance prototype availability with measured performance evidence; the current benchmark-backed read claims live in the evidence matrix and linked artifact bundles.

Non-blocking notes
- `git log --oneline -n 5 -- docs/architecture/dvault-v1-pit-bridge-boundary.md docs/performance-profiles.md CHANGELOG.md docs/releases` showed recent history led by `783bfef30 [06FE4RK80ZXGCZ62CMSAYP164W] AUTO-INTEGRATION squash into develop` and `c9ca6ec0e Finalize DVault v0.44.0 release`, which is consistent with this ticket being a bounded documentation follow-on rather than a reopened implementation ticket.
- The current repo already contains bridge-defer language and evidence-matrix citations; the needed work is to refresh them to the current PIT prototype asymmetry and v0.45.0 release surfaces.

Split recommendations
- Keep the existing decomposition. If the work expands into README, package compatibility, manual publication, local validation, package verifier, or broader `8.45.0` and `10.45.0` consumer-version guidance, keep that in a separate release-alignment ticket instead of widening this docs follow-through task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment