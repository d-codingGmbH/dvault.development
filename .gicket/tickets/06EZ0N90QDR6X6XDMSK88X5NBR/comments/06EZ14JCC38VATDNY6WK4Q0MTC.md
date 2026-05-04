[gicket-bot] PO-critic review contract

Summary
- Persisted contract is repo-aligned, bounded, and has no unresolved open questions; approve for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git log --oneline --max-count=3` on branch `ticket/06EZ0N90QDR6X6XDMSK88X5NBR-task-document-provider-optimization-capability-m` shows `ab4d4a11` PO->PO-critic handoff followed only by gicket metadata commits `03136c34` and `201059eb`; `git diff --name-only ab4d4a11..201059eb` lists only `.gicket/tickets/06EZ0N90QDR6X6XDMSK88X5NBR/**`, so repository source/docs evidence did not drift after refinement.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers the provider-neutral `IDataVaultSaveService` through `AddDVault()`, matching the contract's compatibility-baseline definition.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` defines `DataVaultProviderCapabilityProfiles.Sqlite`; `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` registers `SqliteDataVaultSaveStrategy`, batches writes through `ExecuteSqliteInsertRowsAsync`, and performs satellite existence filtering in `LoadLatestSatelliteHashDiffsAsync(... parentHashKeyBatch.Contains ...)`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` each only call `services.AddDVault();` and do not register a provider save strategy.
- `tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs` defines `ProviderIntegration.RequiredLocal`, `ProviderIntegration.ExternalOptIn`, and `ProviderSmoke.Default`; `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` marks the SQLite integration suite as required-local and `PostgresDataVaultSchemaTests` as external opt-in.
- `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs` gates Postgres validation on `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, and `tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs` skips unless configured.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` both describe SQLite local temporary-file benchmarking and explicitly state that Postgres/Docker/external services are not required.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract assumes no new non-SQLite provider save strategy or integration harness lands before implementation; the ticket already records that this would require a quick repo recheck.
- The contract assumes PostgreSQL remains the only externally opt-in validation path backed by current repo evidence; direct source evidence is limited to the Postgres-specific test/configuration files above.

AC / test suggestions
- Acceptance review should check for exactly five provider rows and verify the validation legend maps cleanly to `ProviderIntegration.RequiredLocal`, `ProviderIntegration.ExternalOptIn`, and the 'not required in v0.5' state.
- Acceptance review should cross-check the SQLite row against `DataVaultProviderCapabilityProfiles.Sqlite`, `DVaultSqliteServiceCollectionExtensions.cs`, and `BenchmarkScenarioExecutionTests`, and the PostgreSQL row against `PostgresDataVaultSchemaTests` plus `PostgresIntegrationTestConfiguration.cs`.

Implementation watchouts
- Keep the matrix explicit that only SQLite has required v0.5 provider-specific optimization obligations; the other four providers are compatibility-baseline rows.
- Do not generalize benchmark coverage beyond SQLite local temporary files or imply non-SQLite performance validation.
- Do not overstate validation for SQL Server, Oracle, or MySQL; current repo evidence supports provider packages and core-service registration, not additional external-validation promises.

Non-blocking notes
- Repository evidence uses Postgres-named packages/tests (`src/DCoding.Data.DVault.Postgres`, `PostgresDataVaultSchemaTests`) while the ticket wants the user-facing row label `PostgreSQL`; the finished document should bridge that wording explicitly rather than imply a naming mismatch.
- `ProviderSmoke.Default` exists in test vocabulary, but the SQL Server/Oracle/MySQL rows still need to read as 'not required in v0.5' so the matrix does not imply provider-specific smoke suites for those providers.

Split recommendations
- No split recommended; the persisted contract already bounds one concise five-provider matrix, and the PO refinement comment `06EZ0ZXPM0CG39BV0837PWWK4R` states the same.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment