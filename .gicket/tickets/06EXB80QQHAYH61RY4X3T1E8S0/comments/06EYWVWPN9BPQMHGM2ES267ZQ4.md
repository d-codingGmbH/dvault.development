[gicket-bot] PO-critic review contract

Summary
- Ticket contract matches the current repository baseline and adjacent completed testing tickets; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB80QQHAYH61RY4X3T1E8S0/description.md` contains the persisted delivery contract and `## Open Questions` is explicitly `none`.
- Latest PO handoff comment `.gicket/tickets/06EXB80QQHAYH61RY4X3T1E8S0/comments/06EYWTQE31PSKVNX2MVAYEBJG0.md` records outcome `po-refinement-ready`, says the durable refinement contract was updated, and says the ticket is ready for handoff to role `po-critic`.
- `find tests/DCoding.Data.DVault.Tests/Integration -maxdepth 1 -type f` shows SQLite-heavy integration coverage plus `PostgresIntegrationTestConfiguration.cs` and `PostgresDataVaultSchemaTests.cs`, and no SQL Server, Oracle, or MySQL integration test files are present in that directory.
- `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs` defines `DVAULT_TEST_POSTGRES_CONNECTION_STRING` and the skip text `Postgres integration tests are skipped because local Postgres configuration is missing...`, while `tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs` calls `Assert.Skip(...)` when configuration is absent.
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` exposes public `AddDVaultSqlite()` and registers `SqliteDataVaultSaveStrategy`; `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `...SqlServer/...`, `...Oracle/...`, and `...MySql/...` each expose public `AddDVault*()` wrappers over `AddDVault()`.
- `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs` already verifies that Postgres, SQL Server, Oracle, and MySql provider packages register the core save service and that only SQLite registers the optimized provider strategy, which matches the ticket's smoke-coverage baseline.
- `.gicket/relations/S0/HC/06EXB80QQHAYH61RY4X3T1E8S0--06EXB82RW6PV2NFG088G6BPFHC--blocks.json` confirms this ticket is the upstream source of truth for downstream CI work, which matches the persisted contract wording.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not yet give one concrete developer-facing invocation example for selecting the eventual opt-in external-provider slice after categories are added.
- Future SQL Server, Oracle, and MySQL live-fixture work is intentionally deferred, so this ticket only implies that those future fixtures should follow the same opt-in pattern unless a later ticket says otherwise.

Risky assumptions
- Implementation must keep the new integration-category contract aligned with the existing unit-project provider smoke coverage owned by `06EXB80FPE3REH11RQ1YR6BW1G` instead of duplicating or drifting from it.
- Downstream CI work on `06EXB82RW6PV2NFG088G6BPFHC` will infer default-versus-opt-in behavior from this ticket, so relying on undocumented runner/filter semantics would be risky if the implementation does not leave a repository-visible proof.

AC / test suggestions
- Add one repository-visible discovery proof for the default-run boundary, analogous to existing discovery smoke tests, so the downstream CI ticket can consume a stable contract.
- Include an explicit repository command example that contrasts default `dotnet test DVault.slnx --nologo` behavior with an opt-in external-provider invocation using `DVAULT_TEST_POSTGRES_CONNECTION_STRING`.

Implementation watchouts
- Keep `tests/DCoding.Data.DVault.Tests/Integration` as the primary surface and avoid solving this by creating a new test assembly; the persisted contract rules that out.
- Do not reinterpret SQL Server, Oracle, or MySql provider packages as requiring live integration fixtures in v1; current repository evidence supports smoke coverage only for those providers.

Non-blocking notes
- `README.md` already documents Postgres integration as opt-in and skipped by default, so the ticket's delivery should stay consistent with that local-validation contract.

Split recommendations
- No split recommended; the persisted contract already keeps unit grouping, Postgres opt-in, and downstream CI as separate tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment