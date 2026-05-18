[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is concrete, open questions are resolved, and repository evidence matches both the current external-provider test harness and the exact bulk-coverage gap this task is meant to close.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGNT7DF4DVNKYWDFZC8DEM/description.md` contains `## Open Questions` followed by `- none`, so the persisted delivery contract has no unresolved open questions.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` exposes the exact bulk entry point the ticket targets: `IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest, CancellationToken)`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` all register provider-specific `IDataVaultProviderSaveStrategy` implementations, so the ticket is anchored to existing provider strategy entry points rather than speculative APIs.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` contains the current gate contract the ticket cites: Postgres requires the Npgsql provider and a clean context; SQL Server requires a clean context plus at least 50 total operations and at most 500 satellite operations; MySQL requires a supported MySQL provider plus at least 50 total operations; Oracle requires a clean Oracle context plus at least 50 total operations; multi-active satellite batches are declined.
- `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixtures.cs` defines `ExternalProviderLiveSchemaFixture.CreatePostgresAsync/CreateSqlServerAsync/CreateOracleAsync/CreateMySqlAsync`, and `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaReaderTests.cs` already uses that shared external-provider harness for all four scoped providers.
- Repository search for bulk-entry usage in the current external-provider live tests found no `DataVaultBulkSaveRequest` usage in `PostgresOptimizedDataVaultSaveServiceTests.cs`, `SqlServerDataVaultSmokeTests.cs`, `OracleDataVaultSmokeTests.cs`, or `MySqlExplicitDataVaultSaveServiceTests.cs`; the same search only matched SQLite integration coverage in `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:644` and `:744`.
- `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` currently classifies live external-provider coverage as Postgres optimized save + live schema, SQL Server smoke + live schema, Oracle smoke + live schema, and MySQL explicit save + live schema, which matches the ticket's claim that non-Postgres providers still lack explicit bulk-path proof.
- `tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs`, the four `*IntegrationTestConfiguration.cs` files, `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`, and `README.md:493-632` all align on the existing opt-in contract: `Category=ProviderIntegration.ExternalOptIn`, provider traits per provider, and `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, and `DVAULT_TEST_MYSQL_CONNECTION_STRING` with conditional external-provider restore markers.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Provider-decline fallback cases, dirty-DbContext rejection, and multi-active satellite rejection are not spelled out as examples, but the contract explicitly treats them as out of scope follow-on work rather than missing pre-dev requirements.

Risky assumptions
- Repository docs are not fully aligned on Oracle scope: `docs/architecture/dvault-v1-explicit-save-service.md:54-65` still describes Oracle as hub/link-only, while `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs` contains satellite-plan handling. The ticket assumes source is the authoritative implementation baseline until docs ticket `06F2PGP2B2RZGGK3CVKK5WRRP8` reconciles the wording.
- README sections at `README.md:591-632` still use smoke-oriented wording for SQL Server and Oracle and integration wording for MySQL. The ticket assumes only narrow guidance updates are needed here unless the bulk coverage changes commands, filters, or prerequisites.

AC / test suggestions
- Keep the native-path proof requirement tied to an observable fallback guard already used in repo tests, such as zero fallback-tracked DVault rows or equivalent request-bound strategy evidence, so all four providers use one defensible acceptance standard.

Implementation watchouts
- `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` hard-codes the expected public integration test types, so any new bulk test classes will need matching discovery/category updates.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` conditionally restores PostgreSQL, SQL Server, Oracle, and MySQL provider packages only when the matching `DVAULT_TEST_*_CONNECTION_STRING` property is non-empty; MySQL also relies on the non-secret restore marker described in the README.
- The shared external-provider harness already exists in `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixtures.cs`; the contract is correctly steering implementation away from creating a second live-provider infrastructure layer.

Non-blocking notes
- The contract's follow-up questions about README wording and benchmark comparability are appropriately framed as downstream coordination, not as blockers for starting provider bulk integration coverage.

Split recommendations
- No split change recommended. The existing ticket graph already separates fallback baseline (`06F2PGN4GPQCGC5WHZQBGP4SD0`), provider-native strategy work (`06F2PGNGVQ3TZZWSABAK5SNFK4`), this external-provider coverage task, benchmarks (`06F2PGNZBRNCQ1SV2KKP6F3BA8`), and broader docs closure (`06F2PGP2B2RZGGK3CVKK5WRRP8`).

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment