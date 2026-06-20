[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the persisted contract is specific, `## Open Questions` is `none`, and repository evidence clearly identifies the missing DB2 reader, existing DB2 scaffolding, and the active docs/tests that must move off the unsupported posture.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FEA6G6R742K2RJ9Q7CY173JR/description.md contains `### PO Handoff` with `decision: ready_for_po_critic` and `## Open Questions` with `- none`, so the persisted delivery contract has no unresolved refinement items.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` defines `Db2ProviderName = "IBM.EntityFrameworkCore"` and currently maps it to `Db2UnsupportedReader`; `src/DCoding.Data.DVault/UnsupportedDataVaultLiveSchemaReader.cs` returns `DataVaultLiveSchemaReadResult.UnsupportedProvider(...)`.
- `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs` still contains `Db2LiveSchemaBoundaryIsExplicitlyUnsupportedUntilAReaderExists`, asserting `IBM.EntityFrameworkCore` returns `UnsupportedProvider` today.
- `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs` already defines the DB2 live-schema contract surface through `DataVaultProviderCapabilityProfiles.Db2`, including `VARCHAR(64)` hash keys, `VARCHAR(33)` load timestamps, 128-character identifier projection, and DB2 satellite index columns that include `HashDiff`.
- `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixture.cs` and `ExternalProviderLiveSchemaReaderAssertions.cs` already provide the shared external live-schema harness for PostgreSQL, SQL Server, Oracle, and MySQL, while `tests/DCoding.Data.DVault.Tests/Integration/Db2IntegrationTestConfiguration.cs`, `Db2ProviderReflection.cs`, and `Db2DataVaultSmokeTests.cs` prove existing DB2 opt-in provider scaffolding behind `DVAULT_TEST_DB2_CONNECTION_STRING`.
- Active docs currently advertise or imply no DB2 live-schema reader in `README.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, `docs/performance-profiles.md`, `docs/releases/v0.42.0.md`, and `docs/plans/hash-key-storage-profile-contract.md`.
- `git rev-parse HEAD` returned `ef5b102bc68c9c8cd23b751f32cf264f3ebc2695`, `git log --oneline -- .gicket/tickets/06FEA6G6R742K2RJ9Q7CY173JR` shows the later PO/PO-critic ticket-metadata commits, and `git diff --stat d246f7d84511c1f66ea7185f9c30f9896cdc6f71..HEAD -- . ':(exclude).gicket'` returned no output, so no repo implementation has landed since the PO refinement baseline.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not pin exact bounded message text for DB2 unavailable outcomes; it only requires classified and redacted results, so tests should avoid depending on provider-specific prose.
- The contract does not enumerate every doc file that still carries unsupported DB2 live-schema guidance; implementers will need a repo-wide search rather than updating only the named README/adoption/model-first surfaces.

Risky assumptions
- The acceptance text treats "missing configuration" as an unavailable live-schema case even though `DataVaultLiveSchemaReader.ReadAsync` takes an already-constructed caller-owned `DbContext`; that example likely has to be satisfied at the consumer/test-harness boundary rather than inside the core reader itself.
- The durable contract still cites scratch ref `d246f7d84511c1f66ea7185f9c30f9896cdc6f71`, while runtime context and `git rev-parse HEAD` show `ef5b102bc68c9c8cd23b751f32cf264f3ebc2695`; only `.gicket` changed, but future reviewers could misread the stale SHA without the diff evidence.

AC / test suggestions
- Replace `Db2LiveSchemaBoundaryIsExplicitlyUnsupportedUntilAReaderExists` in `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs` with a dispatch test proving `IBM.EntityFrameworkCore` no longer routes to `UnsupportedDataVaultLiveSchemaReader`.
- Add a DB2 external-provider live-schema reader test that mirrors `PostgresLiveSchemaReaderTests.cs`/`SqlServerLiveSchemaReaderTests.cs` and asserts snapshot parity against `LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Db2)`.
- Add a unit-level DB2 unavailable-result test that proves redaction when failure text contains connection-string, credential, host, or raw provider error details, because `CatalogDataVaultLiveSchemaReader.ReadAsync` currently passes `exception.Message` into `DataVaultLiveSchemaReadResult.Unavailable(...)`.

Implementation watchouts
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` currently hard-codes `IBM.EntityFrameworkCore` to `Db2UnsupportedReader`; the ticket must change dispatch without regressing unsupported handling for truly unsupported providers.
- `src/DCoding.Data.DVault/CatalogDataVaultLiveSchemaReader.cs` currently turns `DbException`/`InvalidOperationException` into `DataVaultLiveSchemaReadResult.Unavailable(providerName, exception.Message)`; DB2-specific logic must redact before that message reaches preflight or support-bundle surfaces.
- `DataVaultProviderCapabilityProfiles.Db2` and `LiveSchemaReaderContractFixtureTests.cs` already fix the DB2 physical-shape expectations: `VARCHAR(64)` hash keys, `VARCHAR(33)` load timestamps, 128-character identifier projection, `allowsIndexesCoveredByPrimaryKey = false`, and appended `HashDiff` satellite index keys.
- The shared external live-schema harness currently has Postgres/SqlServer/Oracle/MySql entry points but no DB2 variant, while `Db2DataVaultSmokeTests.cs` uses a separate smoke schema; reusing the shared fixture/model-options path is the cleanest way to satisfy the ticket's implementation note and avoid a second divergent DB2 schema contract.

Non-blocking notes
- The PO refinement traceability SHA is stale in the description, but the direct repo diff confirms the branch has only ticket metadata writes after the PO handoff and still no implementation change outside `.gicket`.

Split recommendations
- No split recommended: repository evidence keeps this bounded to one DB2 live-schema reader change, matching tests, and documentation alignment.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment