[gicket-bot] PO-critic review contract

Summary
- Ticket 06F9G8HBXS7Y42J7XFSQKZ2AZ8 is refined enough for developer handoff; the contract is bounded to DB2 external opt-in integration coverage and the remaining gaps are implementation watchouts, not PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git rev-parse showed branch ticket/06F9G8HBXS7Y42J7XFSQKZ2AZ8-story-add-db2-save-and-read-integration-coverage at 3d77348b79e82c5345dacc77f410fbb4db23cbc9; git log shows later commits 0c1ff2249, babe60252, and 3d77348b7 after a2317f2f84b07998327e06ba0b0846b8c334dabf, and git diff --name-only versus develop listed only .gicket/tickets/06F9G8HBXS7Y42J7XFSQKZ2AZ8 files, so there is no partial implementation diff on the branch.
- .gicket/tickets/06F9G8HBXS7Y42J7XFSQKZ2AZ8/description.md contains Open Questions: none, scopes DB2 to external opt-in save/read coverage, and explicitly scopes out DB2 optimized strategies and live-schema reader work.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj currently conditionally references MySql.EntityFrameworkCore, Npgsql.EntityFrameworkCore.PostgreSQL, Oracle.EntityFrameworkCore, and Microsoft.EntityFrameworkCore.SqlServer plus SQLite, but no DCoding.Data.DVault.Db2 project reference or IBM.EntityFrameworkCore package line.
- tests/DCoding.Data.DVault.Tests/Integration/MySqlIntegrationTestConfiguration.cs uses DVAULT_TEST_MYSQL_CONNECTION_STRING with an opt-in skip message, and tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs defines ProviderIntegration.ExternalOptIn, so the repository already has a concrete external-provider gating pattern to mirror for DB2.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers capability and provider-behavior wiring only, while src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs and src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs also register provider save/read strategies; this matches the ticket's provider-neutral fallback expectation for DB2.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderBehaviorTests.cs already verifies AddDVaultDb2() selects db2-provider-v1, and tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs explicitly treats IBM.EntityFrameworkCore live-schema reads as unsupported until a reader exists.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not pin the exact DB2 connection-string environment variable name; the existing repository pattern strongly suggests DVAULT_TEST_DB2_CONNECTION_STRING.
- The contract asks for representative latest, as-of, PIT, and bridge reads but does not nominate one canonical maintained DB2 dataset or fixture shape, so the developer will need to choose a repository-consistent test shape.

Risky assumptions
- It assumes the team will interpret the dual-target requirement consistently: Definition of Done explicitly requires net8.0 and net10.0 build parity, but it does not explicitly say whether live DB2 execution must be demonstrated on both targets or on one target plus build coverage on the other.
- It assumes DB2 diagnostics should show provider-neutral fallback with no selected strategy and the usual no-provider-specific-strategy signal, which is consistent with current diagnostics tests but not spelled out by exact enum name in the acceptance criteria.

AC / test suggestions
- Name the DB2 gate explicitly as DVAULT_TEST_DB2_CONNECTION_STRING to prevent naming drift from the existing DVAULT_TEST_<PROVIDER>_CONNECTION_STRING pattern.
- Add one explicit diagnostics expectation that DB2 save and read coverage must not report a provider-specific strategy name and must preserve provider-neutral fallback evidence where applicable.
- State whether the live DB2 lane must execute on both net8.0 and net10.0 or whether one live run plus dual-target build coverage is sufficient.

Implementation watchouts
- Keep DB2 opt-in only in tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj so default local validation stays SQLite-backed and DB2-free when the gate is unset.
- Do not imply DB2 optimized save or read behavior: src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs currently adds capability and provider-behavior wiring only, unlike the PostgreSQL and SQLite provider packages that register strategy services.
- Do not expand into live-schema reader or drift scope; tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs already codifies IBM.EntityFrameworkCore as explicitly unsupported there.

Non-blocking notes
- README.md currently advertises DCoding.Data.DVault.Db2 install lines and IBM.EntityFrameworkCore, while quick inspection of docs/releases/v0.33.0.md found no DB2 or IBM references; the ticket correctly leaves that documentation alignment as follow-up rather than current scope.
- The description still references a2317f2f84b07998327e06ba0b0846b8c334dabf as the no-implementation baseline, and the later branch commits are ticket-state commits rather than source changes, so this does not create a PO-level contradiction.

Split recommendations
- No split recommended; the current story is already bounded to one DB2 external opt-in integration slice, and the existing package-verification follow-up can remain separate.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment