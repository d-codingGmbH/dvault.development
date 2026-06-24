[gicket-bot] PO-critic review contract

Summary
- The delivery contract is repository-anchored, bounded to existing PIT maintenance test surfaces, and has no unresolved PO questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF43HQ8E0435ZZSRZQQJW1HC/comments/06FFF789X2N4TCJW4H7BA90AG0.md` published the same bounded PO refinement summary, and no newer ticket comment reopens scope or introduces a new unresolved PO question.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` directly fixes the PIT maintenance boundary: PostgreSQL provider-native rebuilds are limited to ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs, while SQL Server provider-native rebuilds are limited to ordinary hub-parent full rebuilds and otherwise fall back provider-neutrally.
- `tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs` already asserts PostgreSQL PIT gate acceptance for supported shapes plus fallback causes for provider mismatch, dirty DbContext, unsupported PIT shape, and incomplete evidence; `tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs` already covers supported PostgreSQL happy-path rebuilds for ordinary, multi-active, and link-parent PIT shapes when configured.
- `tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs` already covers the SQL Server rebuild candidate gate, provider-mismatch rebuild fallback, and `MaintainParentsAsync(...)` provider-neutral no-op fallback with `ProviderNeutralFallback` activity evidence.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers `DefaultDataVaultPitMaintenanceService` for `AddDVault()`, `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` adds `IDataVaultProviderPitMaintenanceStrategy`, and `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`, so the missing-registration scenario is directly grounded in source.
- `git diff --name-status develop...HEAD` shows only `.gicket/tickets/06FF43HQ8E0435ZZSRZQQJW1HC/**` metadata changes, and `git log --oneline --decorate -n 12` shows head `82f0b28441` is a po-critic lease-claim commit above PO handoff commits, so this remains a pre-development gate review rather than a code-review state.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The SQL Server missing-registration case will be exercised intentionally as a provider-neutral `AddDVault()` / no-service-replacement scenario, not accidentally re-proved as mere provider mismatch; the contract's Risks section calls this out and the service-registration source confirms the distinction.
- PostgreSQL fallback proof will stay mostly deterministic in unit or SQLite-backed provider-neutral tests and will not rely only on opt-in live Npgsql integration, consistent with the Definition of Done.

AC / test suggestions
- Keep the fallback matrix explicit by naming separate scenarios for PostgreSQL and SQL Server provider mismatch, dirty context, unsupported shape, and missing registration, with both behavioral parity and explicit fallback-surface assertions where those surfaces already exist.
- Preserve the existing supported-shape happy-path coverage as a regression guard while concentrating the new fallback proof in the existing unit/service-level test files named in the contract.

Implementation watchouts
- PostgreSQL missing registration is strategy-based (`IDataVaultProviderPitMaintenanceStrategy`), while SQL Server missing registration is service-replacement-based (`IDataVaultPitMaintenanceService`); tests that do not distinguish those seams could prove the wrong behavior.
- The enum member `NoProviderSpecificStrategyRegistered` exists in `src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs`, but the contract correctly avoids forcing a new emitted diagnostics surface unless an existing PIT maintenance surface already produces it.

Non-blocking notes
- The downstream documentation ticket `06FF43JEA6C3HNJ6AQA9XY7EC8` is described in the contract/comments as downstream and non-blocking for this pre-development task.
- Current branch changes are ticket metadata only; absence of implementation diffs is not a PO blocker on this pre-development handoff.

Split recommendations
- No split recommended; the repository evidence and contract already bound the work to existing PIT maintenance test surfaces.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment