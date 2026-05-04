[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the persisted contract is specific, the scope split is evidenced in source and sibling-ticket data, and `## Open Questions` is resolved to `none`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0N9KXZY8BPQN84NV3WDYCG/description.md:29-50` defines the shared execution-contract acceptance criteria and definition of done, and its `## Open Questions` section is `- none`.
- `git -C /mnt/c/Projects/DVault log -1 --oneline --decorate=short` shows HEAD `8a78d1da` on `ticket/06EZ0N9KXZY8BPQN84NV3WDYCG-task-add-shared-provider-sql-execution-contract`.
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:315-396` opens the `DbConnection`, reuses or begins a `DbTransaction`, creates `DbCommand` parameters, and forwards `cancellationToken` into `OpenAsync`, `BeginTransactionAsync`, and `ExecuteNonQueryAsync`.
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-27` registers `SqliteDataVaultSaveStrategy`; `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:14-19`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:14-19`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:14-19`, and `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:14-19` only call `services.AddDVault()` and do not register `IDataVaultProviderSaveStrategy`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:397-412` shows provider dispatch/fallback is owned by `DefaultDataVaultSaveService` via `CanSave(...)` followed by `strategy.SaveAsync(...)`, matching the ticket's scope split.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:795-837` already provides required-local SQLite optimized-path coverage, and `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs:3-7` keeps PostgreSQL reuse opt-in through `DVAULT_TEST_POSTGRES_CONNECTION_STRING`.
- `.gicket/tickets/06EZ0N8HW9PZAFKMM5WQD564VR/events/06EZ0ND66SZZRPTN9M6BXC1XGC.json:1-14` records the observed `parentOf` relation from story `06EZ0N8HW9PZAFKMM5WQD564VR` to this ticket.
- `.gicket/tickets/06EZ0N9AM9AJ3AB8DQ6Y1JBS28/description.md:1-6` assigns fallback-selection, optimized-selection, missing capability registration, and unknown-provider behavior to the sibling ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not name a concrete `unsupported shape` example beyond strategy decline; implementation should anchor that clause to an already observed decline path such as `CanSave(...)` returning false for a non-SQLite provider or dirty tracked changes.

Risky assumptions
- This ticket assumes SQL-boundary decline or fallback signaling can be demonstrated through existing `IDataVaultProviderSaveStrategy.CanSave` semantics; the repository does not expose a separate unsupported-shape result type today.
- This ticket assumes cancellation propagation is proved with doubles or fakes for determinism, because the live SQLite path directly shows token forwarding but not a guaranteed observable cancellation outcome.

AC / test suggestions
- Keep shared assertions at provider-neutral command-behavior boundaries and treat SQLite quoting and parameter-name formatting as implementation details, consistent with `description.md:29-34` and `DVaultSqliteServiceCollectionExtensions.cs:383-447`.
- Reuse the existing `tests/DCoding.Data.DVault.Tests/Shared` support layer and current provider traits so SQLite stays required-local and PostgreSQL stays external opt-in.

Implementation watchouts
- Do not duplicate service-level dispatch tests from sibling ticket `06EZ0N9AM9AJ3AB8DQ6Y1JBS28`; `DefaultDataVaultSaveService` already owns that loop.
- Do not introduce a new public API solely for test wiring; the directly observed public extension seam today is `IDataVaultProviderSaveStrategy` plus `DataVaultProviderSaveStrategyContext` in `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:8-78`.

Non-blocking notes
- The shared test substrate already exists as `tests/DCoding.Data.DVault.Tests/Shared/DCoding.Data.DVault.Tests.Shared.csproj` with reusable provider traits and SQLite test helpers.

Split recommendations
- No split recommended; the parent story and sibling ticket already separate strategy-selection scope from this lower-level SQL execution contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment