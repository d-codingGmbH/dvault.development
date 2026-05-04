[gicket-bot] PO-critic review contract

Summary
- The ticket is now bounded and developer-ready: the durable contract names the MySQL test provider, the opt-in/skip behavior to mirror, the live smoke objective, and the README and discovery updates needed for handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket` returned ticket `06EZ0NC3VNZ5FP9XDYVX9DHW1G` at revision `06EZ5ZAXEPVN2Z017KGMNV966W` with `## Open Questions` set to `none` and a delivery contract that now names `Pomelo.EntityFrameworkCore.MySql`, `UseMySql`, `ServerVersion.AutoDetect(connectionString)`, conditional restore, and a `ProviderIntegration.ExternalOptIn` / `Provider=MySQL` live smoke test.
- `.gicket/tickets/06EZ0NC3VNZ5FP9XDYVX9DHW1G/description.md` contains the same persisted contract, including scope-in for a MySQL configuration helper, reflection bootstrap helper, one live smoke test, provider discovery updates, and README guidance.
- `git show --stat --oneline dafd9461f9c2 -- .gicket/tickets/06EZ0NC3VNZ5FP9XDYVX9DHW1G` shows the latest PO handoff commit updated the ticket description, ticket metadata, comments, and events for this review cycle.
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` shows `AddDVaultMySql()` currently delegates to `services.AddDVault()` only, which matches the contract's compatibility-only scope and no MySQL-specific save strategy requirement.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` directly defines the public `IDataVaultSaveService` and `DataVaultSaveRequest` types the live MySQL smoke test is expected to use.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` already demonstrates the conditional external-provider pattern with `Npgsql.EntityFrameworkCore.PostgreSQL` behind `$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)`, and `PostgresIntegrationTestConfiguration.cs` plus `NpgsqlProviderReflection.cs` show the skip and restore-guidance model the ticket tells developers to mirror for MySQL.
- `tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs` already defines `Provider=MySQL`, and `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs` already keeps MySQL in default-smoke provider registration coverage with `expectProviderStrategy: false`.
- `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` currently enumerates only Postgres external opt-in and configuration coverage, which makes the required MySQL discovery-category updates explicit and testable.
- `README.md` currently says `Category=ProviderIntegration.ExternalOptIn` is 'currently Postgres' and only documents Postgres opt-in commands, so the ticket's README update scope is concrete and necessary.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly spell out the MySQL test-user privilege and cleanup expectation the way the README already does for Postgres temporary schemas; developers will need to choose and document a safe cleanup boundary.

Risky assumptions
- A repository search for the named Pomelo bootstrap surface returned no local matches, so the exact `UseMySql` and `ServerVersion.AutoDetect` API contract is not evidenced inside the repo and still depends on the external provider exposing that surface as named in the ticket.
- The live MySQL smoke path still assumes a developer-managed database is available during restore, build, and test when the env var is present, which the contract correctly calls out as an external dependency.

AC / test suggestions
- If the team wants less execution ambiguity, add one line in the acceptance criteria or README clarifying whether the configured MySQL user must be able to create and drop test tables, schemas, or databases for the live smoke run.

Implementation watchouts
- The integration test project currently references core, SQLite, benchmarks, and shared projects but not `src/DCoding.Data.DVault.MySql`; the `AddDVaultMySql()` live smoke path will need that provider package project available alongside the conditional Pomelo package.
- MySQL discovery updates will have to keep configuration-contract tests in `ProviderSmoke.Default` and mark only the live smoke test as `Category=ProviderIntegration.ExternalOptIn` with `Provider=MySQL`, because `ProviderIntegrationCategoryDiscoveryTests.cs` currently hard-codes the discovered test set.
- README text currently says external opt-in coverage is 'currently Postgres', so documentation must be updated in the same ticket to avoid contradicting the new MySQL path.

Non-blocking notes
- The prior PO-critic blocker in the comment history was the missing provider and setup contract; the current delivery contract answers that by naming Pomelo, the `UseMySql` bootstrap, and the conditional restore and skip behavior.

Split recommendations
- Keep richer MySQL parity work, always-on CI provisioning, and MariaDB compatibility as separate follow-up tickets, consistent with the existing `## Split Recommendations` block.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment