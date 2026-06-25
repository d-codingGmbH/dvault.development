[gicket-bot] PO-critic review contract

Summary
- The ticket is ready for developer handoff: the delivery contract has no open questions, the repository already contains a concrete PostgreSQL binary-first baseline and opt-in validation path, and the remaining work is a bounded docs-only parity pass with one clear stale-version hotspot.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket `06FF43T2EK3CBYHTR287YWC5NR` states `## Open Questions` = `none` and scopes the work to documentation-only PostgreSQL parity notes with no product-code, provisioning, or runtime-behavior changes.
- `README.md:68-83` shows the primary quickstart snippet is still SQLite-only (`AddDVaultSqlite()` plus `UseSqlite(...)`), and `docs/getting-started.md:15-29` likewise gives a provider-neutral registration section with a SQLite-only example, so a broader quickstart parity gap currently exists.
- `examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs:6-24` already demonstrates the intended runnable baseline: `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `UseBinaryFirstProfile()`, `AddDVaultPostgres()`, `UseNpgsql(connectionString)`, and `UseDataVaultMetadata()`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:9-29` defines the public `AddDVaultPostgres(IServiceCollection)` extension, providing direct source evidence for the API the docs need to reference.
- `examples/DCoding.Data.DVault.PostgresQuickstart/DCoding.Data.DVault.PostgresQuickstart.csproj:9-13` and `examples/README.md:57` confirm the normal EF Core PostgreSQL provider package is `Npgsql.EntityFrameworkCore.PostgreSQL`.
- `docs/local-validation.md:44` and `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs:3-11` show PostgreSQL validation is opt-in behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, while `examples/DCoding.Data.DVault.PostgresQuickstart/README.md:3-11` and `:73-79` preserve the developer-managed database / no default provisioning boundary.
- `examples/README.md:29-59` still advertises stale `8.45.0` and `10.45.0` package lines, while `README.md:18-43` and `docs/local-validation.md:17-18` show the current repository baseline is `8.47.0` / `10.47.0`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract assumes the developer will update at least one primary quickstart surface beyond the already PostgreSQL-aware `examples/README.md`; the natural candidates are `README.md` or `docs/getting-started.md`, which currently still read SQLite-first/provider-neutral.
- If `examples/README.md` is touched for parity wording, its stale `8.45.0` / `10.45.0` install blocks need to be corrected in the same ticket so the docs do not keep visible version drift.

AC / test suggestions
- Review the final doc diff against `README.md`, `docs/getting-started.md`, `examples/README.md`, and `docs/local-validation.md` to ensure any touched install commands use only `8.47.0` / `10.47.0` and that PostgreSQL guidance names both `DCoding.Data.DVault.Postgres` and `Npgsql.EntityFrameworkCore.PostgreSQL`.
- Check that at least one adopter-facing quickstart surface explicitly pairs `AddDVaultPostgres()` with `UseNpgsql(connectionString)` and mentions the opt-in `DVAULT_TEST_POSTGRES_CONNECTION_STRING` path instead of implying PostgreSQL is part of the default validation lane.

Implementation watchouts
- Do not duplicate full container lifecycle content into broad quickstart docs; route readers to `examples/DCoding.Data.DVault.PostgresQuickstart/README.md` for fixture details.
- Preserve the boundary that a missing `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is a successful skip path, not a default validation failure.
- Keep binary-first wording aligned with the existing migration caveats so the parity note does not imply automatic migration from existing `HexString` setups.

Non-blocking notes
- No ticket comments or closure evidence amendments are present in the supplied persisted snapshot.
- The repository already contains enough concrete PostgreSQL baseline material, so this is a documentation-routing task rather than a discovery ticket.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment