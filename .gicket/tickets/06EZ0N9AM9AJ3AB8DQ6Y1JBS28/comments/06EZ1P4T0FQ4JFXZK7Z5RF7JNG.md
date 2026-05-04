[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the contract matches the current AddDVault fallback and SQLite-only optimized dispatch model, and the repo still lacks explicit strategy-selection regression coverage.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Current branch inspection shows `ticket/06EZ0N9AM9AJ3AB8DQ6Y1JBS28-task-add-strategy-selection-tests-for-fallback-v` at HEAD `f3c314587e33efd57f18e1bde0c16e2f66145e96`.
- `docs/architecture/dvault-v1-explicit-save-service.md` states that the v0.5 baseline is provider-neutral `AddDVault()` / `IDataVaultSaveService`, and that SQLite is the only provider-specific optimized baseline.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-25` registers only the core `IDataVaultSaveService` for `AddDVault()`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:397-412` iterates registered `IDataVaultProviderSaveStrategy` instances, calls `CanSave(...)`, and otherwise falls back to the built-in save path.
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-27` registers `IDataVaultProviderSaveStrategy`, and `:38-46` limits the optimized path to `Microsoft.EntityFrameworkCore.Sqlite` with a clean change tracker.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:14-19`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:14-19`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:14-19`, and `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:14-19` only call `AddDVault()`, matching the non-optimized compatibility baseline for those providers.
- `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:42-54` verifies provider registration shape, `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:796-838` verifies the SQLite optimized happy path, and `tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderSqlExecutionContractTests.cs:12-38` verifies optimized SQL contract behavior.
- The repo search `rg -n "unknown provider|missing capability|fallback path|fallback save|dispatch" tests src docs` returned no matches, so the current suite does not already advertise those strategy-selection scenarios explicitly.
- Branch history on the save-service/doc/test surface includes `31d277ac feat: Enhance Data Vault Save Service with Bulk Save Functionality` and `a3d0282f Fix sqlite set-based save fallback compile`, which confirms this ticket targets established dispatch behavior rather than speculative future API.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- This assumes developers will model `missing capability registration` at the DI / `IDataVaultProviderSaveStrategy` boundary, not by expanding `DataVaultProviderCapabilityProfiles.Sqlite`, which serves the metadata-translator surface.
- This assumes the selected path can be asserted through current `IDataVaultSaveService` behavior or a minimal non-production test seam without widening the production API.

AC / test suggestions
- Add one explicit fallback-selection case on a SQLite `DbContext` wired with `AddDVault()` only, so the regression signal distinguishes `SQLite provider present` from `SQLite optimized strategy registered`.
- Add one explicit non-SQLite or stub-provider case that proves a registered optimized strategy is skipped when `CanSave(...)` is false, with assertion text naming the selected path or missing registration reason.

Implementation watchouts
- Do not treat `DataVaultProviderCapabilityProfiles.Sqlite` in `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` as the save-dispatch switch; the dispatch gate is `IDataVaultProviderSaveStrategy.CanSave(...)`.
- Keep the tests entering through `IDataVaultSaveService.SaveAsync(...)`; direct `SqliteDataVaultSaveStrategy.SaveAsync(...)` calls would bypass the selection logic in `src/DCoding.Data.DVault/DataVaultSaveService.cs:397-412`.

Non-blocking notes
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` already exercises fallback persistence on SQLite with `services.AddDVault()`, so the new work should emphasize explicit selection and diagnostics rather than re-proving row persistence.
- `gicket-read-ticket-comments` returned bot refinement and handoff history in the bounded sample; no additional stakeholder clarification was visible that would block developer handoff.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment