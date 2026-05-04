[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the repo already exposes the needed provider-strategy and satellite-save contracts, the ticket cleanly scopes Postgres strategy work to code/tests/docs, and live Postgres verification is explicitly split to a sibling ticket.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git -C /mnt/c/Projects/DVault rev-parse --abbrev-ref HEAD returned ticket/06EZ0NA180RA0FQ64KXQTHEVZW-task-implement-postgresql-optimized-hub-link-sat and git -C /mnt/c/Projects/DVault rev-parse HEAD returned 40037fd6c07467b8eec0ad4b7c9e58b95cfdfd29.
- repository listing of src/DCoding.Data.DVault.Postgres returned only src/DCoding.Data.DVault.Postgres/DCoding.Data.DVault.Postgres.csproj and src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs; the extension file only calls services.AddDVault() and returns, so no PostgreSQL strategy exists yet.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs defines IDataVaultProviderSaveStrategy with CanSave(DbContext, IReadOnlyList<DataVaultSaveRequest>) and SaveAsync(DataVaultProviderSaveStrategyContext,...); src/DCoding.Data.DVault/DataVaultSaveService.cs dispatches registered strategies first and otherwise falls back to the built-in writer.
- src/DCoding.Data.DVault/DataVaultSaveService.cs already exposes DataVaultSatelliteSaveOperation and a DataVaultSaveRequest overload with satelliteOperations, so PostgreSQL optimization can fit the existing public save boundary without adding caller-facing API.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs registers SqliteDataVaultSaveStrategy via TryAddEnumerable(...) and its CanSave rejects contexts with Added/Modified/Deleted tracked entries; the ticket's guardrail language matches an existing repo precedent.
- README.md says DCoding.Data.DVault.Sqlite currently registers the optimized SQLite set-based save strategy and that PostgreSQL uses the provider-neutral fallback; docs/architecture/dvault-v1-explicit-save-service.md says PostgreSQL is a compatibility baseline through AddDVaultPostgres() and labels src/DCoding.Data.DVault.Postgres as compatibility-only for v0.5.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs currently asserts AssertProviderRegistration(services => services.AddDVaultPostgres(), expectProviderStrategy: false), while SQLite expects true.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs contains current satellite invariants named DefaultSaveServicePersistsSatelliteRowsOnlyWhenHashDiffChanges and DefaultSaveServiceKeepsBulkSatelliteLatestHashDiffChronological; the delivery contract explicitly points back to those semantics.
- .gicket/tickets/06EZ0NA7CWDYJ7ZS3K5GM0187M/ticket.json exists with title Task: Add opt-in PostgreSQL integration coverage for optimized strategy, and its description says the opt-in tests verify insert-only behavior, unchanged satellite suppression, and changed satellite insertion.
- .gicket/tickets/06EZ0N9TJSXFXH0YZRA3QN2S14/description.md still includes benchmark evidence in story scope, matching the current ticket's follow-up note that benchmark ownership remains a separate story-level concern.
- gicket-read-ticket-comments returned 9 comments, all bot claim/lease/refinement/handover entries with no human clarification thread, and git show --stat --format=fuller --summary e5ce09c310a8 shows the PO->PO-critic handoff commit only touched .gicket ticket metadata files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not spell out a concrete link-parent satellite history example, even though existing Postgres schema coverage already models a link satellite shape (SatCustomerOrderState).
- The contract does not name a duplicate-in-one-bulk-batch hub/link example explicitly; current SQLite behavior and tests imply the intended inserted-row counting, but the Postgres ticket leaves that parity to implementation.

Risky assumptions
- Story 06EZ0N9TJSXFXH0YZRA3QN2S14 still owns benchmark evidence; this ticket assumes benchmark proof can be deferred without blocking the implementation task itself.
- Live PostgreSQL save semantics are not proven by this ticket and are deferred to sibling 06EZ0NA7CWDYJ7ZS3K5GM0187M, which currently still carries needs-po.

AC / test suggestions
- Flip the existing AddDVaultPostgres registration expectation from expectProviderStrategy: false to true in tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs.
- Add explicit Postgres-oriented coverage for unchanged satellite replay, changed payload append, and out-of-order bulk chronology to mirror the existing SQLite semantic tests.
- Add a CanSave decline test for dirty DbContext state analogous to the shared ProviderSqlExecutionContract and SQLite strategy guardrails.

Implementation watchouts
- The integration test project references Npgsql.EntityFrameworkCore.PostgreSQL only when DVAULT_TEST_POSTGRES_CONNECTION_STRING is set, so default local test runs must remain Postgres-free.
- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs snapshots DCoding.Data.DVault.Postgres public API; the contract is correct to avoid caller-facing API expansion.
- Current docs and tests contain explicit fallback-baseline language for PostgreSQL, so partial implementation without doc/test updates would leave the repo internally inconsistent.

Non-blocking notes
- The persisted delivery contract has ## Open Questions -> none, so there is no policy blocker to approve_for_dev.
- git show --stat --format=fuller --summary e5ce09c310a8 records the PO handoff as ticket metadata changes only; no repository product-code change is being reviewed in this PO-critic step.

Split recommendations
- Keep live PostgreSQL integration verification in sibling ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M.
- If benchmark evidence remains required for story 06EZ0N9TJSXFXH0YZRA3QN2S14, track it in a separate follow-up benchmark ticket instead of widening this implementation task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment