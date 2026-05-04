[gicket-bot] PO-critic review contract

Summary
- The ticket is bounded and uses the existing Postgres opt-in pattern as its model, but it is not ready for developer handoff because the MySQL EF Core provider/setup contract is still unspecified.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0NC3VNZ5FP9XDYVX9DHW1G/description.md` contains a durable delivery contract with `## Open Questions` set to `none`, and its acceptance criteria require a live `ProviderIntegration.ExternalOptIn` / `Provider=MySQL` test driven by `DVAULT_TEST_MYSQL_CONNECTION_STRING`.
- `git show --stat f22b42a9b8d9b0b240a49405828ea3fa738bdc8d -- .gicket/tickets/06EZ0NC3VNZ5FP9XDYVX9DHW1G` shows the PO handoff commit updated the ticket description, ticket metadata, and handoff comments for this review cycle.
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` shows `AddDVaultMySql()` only calls `services.AddDVault()` and does not register a MySQL-specific save strategy.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` defines the public `IDataVaultSaveService` and `DataVaultSaveRequest` types the contract expects the live smoke test to use.
- `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs` already gives MySQL default-smoke coverage by asserting `services.AddDVaultMySql()` registers the core save service with `expectProviderStrategy: false`.
- `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs`, `tests/DCoding.Data.DVault.Tests/Integration/NpgsqlProviderReflection.cs`, `tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` show the existing external opt-in pattern is Postgres-only and depends on the conditional `Npgsql.EntityFrameworkCore.PostgreSQL` package reference behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING`.
- `README.md` states `Category=ProviderIntegration.ExternalOptIn` is 'currently Postgres' and only documents Postgres opt-in commands for that category.
- A repository search found no existing MySQL integration wiring: `find tests/DCoding.Data.DVault.Tests/Integration -maxdepth 1 -type f` lists `Postgres*` and `Npgsql*` files but no `MySql*` files, and `rg -n "UseMySql|Pomelo|MySql.EntityFrameworkCore|ServerVersion" tests/DCoding.Data.DVault.Tests/Integration src` returned no matches.

Blocking findings
- The contract requires a live MySQL-backed `DbContext`, but it never names the EF Core MySQL provider package or its setup contract. That is implementation-critical in this repo because the only proven external opt-in pattern is Postgres-specific (`Npgsql.EntityFrameworkCore.PostgreSQL` plus `NpgsqlProviderReflection`), and there is no direct repository evidence for any MySQL provider API surface (`UseMySql`, `Pomelo`, `MySql.EntityFrameworkCore`, `ServerVersion`).
- The ticket does not state whether MySQL should mirror the Postgres conditional-restore behavior when the env var is set but the provider package is unavailable. The current Postgres path has explicit skip behavior in `NpgsqlProviderReflection.cs`, but the MySQL contract leaves that automation-stability detail implicit.

Required PO actions
- Name the exact EF Core MySQL provider/package this ticket must use and make that choice part of the durable contract.
- Specify the expected MySQL `DbContext` setup contract for this repo, including any required server-version handling or equivalent provider-specific bootstrap.
- Clarify whether the MySQL path must mirror the Postgres conditional package-restore and missing-provider skip behavior when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is set.

Open issues ledger
- critic-item-1 [required-po-action] Name the exact EF Core MySQL provider/package this ticket must use and make that choice part of the durable contract.
- critic-item-2 [required-po-action] Specify the expected MySQL `DbContext` setup contract for this repo, including any required server-version handling or equivalent provider-specific bootstrap.
- critic-item-3 [required-po-action] Clarify whether the MySQL path must mirror the Postgres conditional package-restore and missing-provider skip behavior when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is set.
- critic-item-4 [blocking-finding] The contract requires a live MySQL-backed `DbContext`, but it never names the EF Core MySQL provider package or its setup contract. That is implementation-critical in this repo because the only proven external opt-in pattern is Postgres-specific (`Npgsql.EntityFrameworkCore.PostgreSQL` plus `NpgsqlProviderReflection`), and there is no direct repository evidence for any MySQL provider API surface (`UseMySql`, `Pomelo`, `MySql.EntityFrameworkCore`, `ServerVersion`).
- critic-item-5 [blocking-finding] The ticket does not state whether MySQL should mirror the Postgres conditional-restore behavior when the env var is set but the provider package is unavailable. The current Postgres path has explicit skip behavior in `NpgsqlProviderReflection.cs`, but the MySQL contract leaves that automation-stability detail implicit.

Missing examples / edge cases
- Configured `DVAULT_TEST_MYSQL_CONNECTION_STRING` but the MySQL EF provider package was not restored or cannot be loaded.
- Whitespace-only MySQL env var normalization and the exact skip message contract for the default path.
- Discovery/category expectations for the MySQL configuration helper tests versus the live MySQL external-opt-in test.

Risky assumptions
- A developer can choose any MySQL EF Core provider without affecting test wiring, restore behavior, or README guidance.
- The chosen provider can exercise the bounded insert-only explicit-save scenario through the provider-neutral fallback writer without extra MySQL-specific prerequisites beyond a connection string.

AC / test suggestions
- Add an explicit acceptance note naming the MySQL EF Core provider package and the expected behavior when the env var is set but the provider package is unavailable.
- Mirror the existing Postgres configuration-contract test shape for MySQL: absent env var, blank env var, trimmed env var, and skip-message assertions.
- Mirror the existing provider-discovery assertions so the MySQL config tests stay `ProviderSmoke.Default` while the live MySQL smoke test is `ProviderIntegration.ExternalOptIn` with `Provider=MySQL`.

Implementation watchouts
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` currently only has the conditional Npgsql package reference and no MySQL project/provider wiring.
- `README.md` currently documents external opt-in coverage as Postgres-only, so provider naming, command examples, and secret-handling guidance will need to stay aligned with the chosen MySQL provider contract.
- The ticket correctly keeps MySQL on the compatibility-only `AddDVaultMySql()` path; implementation should not drift into optimized strategy or capability-profile work.

Non-blocking notes
- The refined contract is otherwise well bounded: `Open Questions` are resolved, scope excludes optimized MySQL behavior, and the default MySQL provider-registration smoke coverage already exists.
- The parent relation points to story `06EZ0NBPWEWAP264B4XP36CXC8` (`Story: Optimize MySQL provider save strategy`), but this ticket's narrower compatibility-only scope is explicitly documented in the refined contract.

Split recommendations
- Keep MySQL-specific optimized save behavior or capability-profile work in a separate follow-up ticket if scope grows beyond one compatibility-path smoke test.
- Keep repository-managed MySQL provisioning or always-on CI automation separate from this ticket's documentation and opt-in test-contract scope.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment