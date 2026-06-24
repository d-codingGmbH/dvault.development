[gicket-bot] PO-critic review contract

Summary
- Ticket is bounded and evidence-backed for pre-development handoff: the persisted contract has no open questions, the repository already proves the PostgreSQL PIT baseline and maintenance Activity parity seam, and the current branch head contains only PO/critic ticket metadata.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket-comments` returned 10 comments; the substantive review content is the PO refinement contract and the later comments are handoff/lease automation, with no newer unresolved reviewer objections recorded.
- Git on `/mnt/c/Projects/DVault` shows HEAD `726d8e376d8c7b269df3ffc7d3c61a6b979006c4` on branch `ticket/06FF43FQ8NRX04T9HZHBMFS0PC-task-add-postgresql-pit-maintenance-selected-fal`; `git show --stat --name-only HEAD` lists only `.gicket/tickets/06FF43FQ8NRX04T9HZHBMFS0PC/...` files, so this is still a pre-development handoff branch.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` directly registers `IDataVaultProviderPitMaintenanceStrategy` as `PostgresDataVaultPitMaintenanceStrategy` through `AddDVaultPostgres()`.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers the provider-neutral `IDataVaultPitMaintenanceService` as `DefaultDataVaultPitMaintenanceService`; no default provider PIT strategy is registered there, which matches the ticket's `NoProviderSpecificStrategyRegistered` fallback case.
- `src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs` currently iterates provider PIT strategies via `strategy.CanRebuild(...)` and otherwise falls straight to the provider-neutral pipeline without selected/fallback diagnostics, which matches the stated gap.
- `src/DCoding.Data.DVault/DataVaultMaintenanceActivity.cs` already exposes `RecordStrategySelected(...)` and `RecordStrategyFallback(...)`, and `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs` uses that maintenance Activity surface as the in-repo parity model.
- `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` plus `src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs` define a finite fallback vocabulary including `ProviderNameMismatch`, `UnknownOrUnregisteredProviderName`, `NoProviderSpecificStrategyRegistered`, `DirtyDbContext`, `UnsupportedPitShape`, `IncompleteMaintenanceShapeEvidence`, and `StrategyDeclined`.
- `tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs` proves the PostgreSQL PIT gate accepts ordinary hub-parent, multi-active hub-parent, and link-parent non-multi-active rebuilds and already returns concrete fallback causes for provider mismatch, dirty context, unsupported shape, and incomplete shape evidence.
- `tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs` proves current PostgreSQL provider-path rebuild behavior after `AddDVaultPostgres()`, while `tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs` already asserts maintenance Activity `ProviderNeutralFallback` evidence for the SQL Server parity path.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete acceptance-test example for the `AddDVault()`-only case that should emit `NoProviderSpecificStrategyRegistered` is implied by the contract but not spelled out as a named scenario.
- A concrete example for when `StrategyDeclined` should appear on PostgreSQL PIT maintenance is not enumerated in the ticket, although the contract correctly scopes it as `when applicable`.

Risky assumptions
- The ticket assumes the existing maintenance Activity surface can carry the needed PostgreSQL selected/fallback facts without reopening scope into a new public diagnostics API; repository evidence supports that assumption today through `DataVaultMaintenanceActivity` and SQL Server parity.
- The ticket assumes fallback capture will reuse `DataVaultProviderPitMaintenanceStrategyGateEvaluator` rather than ad hoc strings; if implementation diverges, the finite-vocabulary requirement could erode.

AC / test suggestions
- Call out one listener-based selected-path assertion that `dvault.strategy.status=ProviderStrategySelected` and `dvault.strategy.type=PostgresDataVaultPitMaintenanceStrategy` for a clean Npgsql full rebuild.
- Call out one listener-based fallback assertion for an `AddDVault()`-only or otherwise unregistered-strategy case that records `NoProviderSpecificStrategyRegistered` on `dvault.fallback.recorded`.
- Call out one redaction assertion that the observed maintenance tags/events do not include SQL text, connection strings, hash keys, driving-key values, or payload values.

Implementation watchouts
- Keep the scope on `IDataVaultPitMaintenanceService.RebuildAsync(...)` only; PostgreSQL `MaintainParentsAsync(...)` remains provider-neutral under this ticket.
- Treat `SqlServerDataVaultPitMaintenanceService` as parity for maintenance Activity behavior, but keep PostgreSQL eligibility and SQL generation unchanged.
- Use the finite existing maintenance fallback vocabulary and avoid turning fallback output into free-text diagnostics that docs/benchmarks cannot cite stably.

Non-blocking notes
- The persisted contract already states `## Open Questions` as `none`; the remaining items are follow-up questions for sibling docs/benchmark tickets, not handoff blockers.
- The current branch head is still the PO-critic claim commit and contains only `.gicket` ticket metadata changes, which is normal for a pre-development critic gate.

Split recommendations
- No further split is needed now; documentation, benchmark, comparator, and transaction follow-up scope is already parked on sibling tickets called out in the contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment