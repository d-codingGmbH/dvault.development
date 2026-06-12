<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this as a bounded test-gap ticket: Binary hash-key storage already has unit-level mapping/converter coverage, but the repository still lacks executable schema/save/read coverage for storage-profile behavior. No child tickets, relation edits, description updates, attachments, or planning documents were materialized during this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows Binary hash-key coverage exists today only in unit-level provider-mapping, metadata-translation, and converter tests; no integration test under tests/DCoding.Data.DVault.Tests/Integration opts into HashKeyStorageProfile.Binary yet.
- The bounded v1 provider baseline for this ticket is the existing six built-in profiles: sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1, with HexString as the default and Binary as explicit opt-in.
- Save and read APIs stay on lowercase hexadecimal string values even when the physical store type is binary; this ticket is proving storage compatibility, not changing caller-facing hash-key types.
- DB2 live-schema reading remains explicitly unsupported under the current contract and should stay a negative unsupported-provider assertion rather than becoming a new positive execution target here.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized; the existing parentOf relation from 06F9GF5A8V7G3PAKGRXNYEBW5C and the existing blocks relation to 06F9GF66B10J4K7RBDTJ9NQRQC were left unchanged.

### Scope In
- Add executable SQLite coverage for both HexString and Binary hash-key storage so the suite proves text-backed compatibility and binary-backed persistence with the same caller-facing string boundary.
- Add schema-generation or live-schema-fixture assertions that hash-key and participant-reference columns on hubs, links, satellites, PITs, and bridges size from the active stable-hash digest for both storage profiles.
- Add save and read round-trip coverage where hash keys participate in latest/current, explicit as-of, PIT as-of, and bridge traversal requests under a Binary profile.
- Add the remaining missing negative tests at the schema/save/read boundary for incompatible digest length or storage-profile facts, while reusing existing converter and migration-guardrail unit coverage instead of duplicating it.

### Scope Out
- New hash-key storage profiles beyond HexString and Binary.
- Changes to public hash-key value types, stable-hash algorithm inventory, or provider-specific SQL behavior.
- Automatic rehash, repair, migration/backfill, or dual-write behavior.
- New DB2 live-schema support or mandatory external-database execution coverage for every provider.

## Acceptance Criteria
- A SQLite integration test proves HexString hash keys still persist and read as text, and a paired SQLite integration test proves Binary hash keys persist as blob or bytes while callers still save and read canonical lowercase hex strings.
- Schema coverage proves hash-key and participant-reference columns on generated hub, link, satellite, PIT, and bridge artifacts size from the active stable-hash digest for both HexString and Binary without changing logical names or API shape.
- Read-path coverage proves the Binary profile round-trips hash-key participation through latest/current, explicit as-of, PIT as-of, and bridge traversal request shapes.
- Provider-profile matrix coverage proves the finite built-in baseline projects the expected HexString and Binary store types; DB2 live-schema execution still reports unsupported-provider rather than silently passing.
- Negative coverage fails closed for malformed or incompatible hash-key storage facts at the boundaries exercised by this ticket.

## Definition of Done
- New coverage lands in the existing unit and integration test projects under tests/DCoding.Data.DVault.Tests and reuses existing metadata, schema, PIT, and bridge fixtures instead of creating a parallel test harness.
- Existing HexString baselines continue to pass unchanged, and the new Binary assertions make the storage-profile difference explicit in store type and round-trip behavior.
- At least one executable Binary round-trip test covers save plus read behavior, and at least one provider-matrix or fixture test covers non-SQLite provider store-type projections without requiring new infrastructure.

## Implementation Notes
- Good extension points already exist in SqliteProviderCapabilityProfileTests, SqliteDataVaultSchemaTests, LiveSchemaReaderContractFixtureTests, DataVaultEfMetadataTranslationTests, and the existing PIT, bridge, and read smoke patterns.
- Use SQLite as the executable Binary baseline because the suite already distinguishes TEXT versus BLOB locally without optional provider infrastructure.
- Reuse WithHashKeyStorageProfile(...) and the existing stable-hash helpers so at least one non-64-character digest case is exercised; sha256-128-v1 is a bounded default already present in current unit coverage.
- Mirror the current read-path seeding pattern used by the PIT and bridge smoke tests so the Binary assertions prove both key persistence and request filtering/projection behavior, not just raw column types.
- Do not broaden this ticket into support-bundle, migration authoring, or new provider-strategy work unless a missing assertion is strictly necessary to prove the storage-profile contract.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add optional external-provider Binary smoke execution for PostgreSQL or SQL Server, or is SQLite execution plus provider-matrix contract coverage sufficient for v1?
- Should Binary-profile support-bundle and diagnostics export matrices get their own explicit ticket, or remain indirectly covered by provider-mapping and migration-drift tests?

## Risks
- If implementation stops at provider-mapping unit assertions and skips an executable Binary save/read round-trip, EF conversion or query-translation regressions can still slip through.
- PIT and bridge coverage depend on explicitly seeded maintained tables in the test harness; partial read-path coverage could leave a false impression that Binary support is complete.
- Requiring every external provider to execute Binary round-trips in this ticket would couple a test-only task to optional database infrastructure and likely expand it beyond the current bounded scope.
- Scheduling still depends on the existing incoming blocks relation from 06F9GF5TNAXBCKN5BD9CKD7WVG; refinement did not change dependency state.

## Split Recommendations
- No split recommended while the work stays within existing test and shared-fixture surfaces.
- If later stakeholders want live Binary execution across optional external providers, spin that into a follow-up ticket instead of broadening this test-coverage ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add tests proving HexString remains compatible and Binary storage works across schema generation, save paths, latest/as-of/PIT/bridge read paths where hash keys participate, and provider capability fallbacks. Include negative tests for mismatched algorithm length and storage profile configuration.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Added executable SQLite HexString and Binary hash-key storage coverage under `tests/DCoding.Data.DVault.Tests`.
- Binary storage binds SQLite optimized save/read hash-key values as provider bytes while preserving lowercase hexadecimal strings at caller-facing save and read APIs.
- Added provider-matrix coverage for the six built-in profiles: sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1.
- Repaired the integration discovery contract so the new `HashKeyStorageProfileSqliteTests` class is included in required local SQLite coverage.

Repository artifacts
- `src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs`
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`
- `src/DCoding.Data.DVault.Sqlite/SqliteDataVaultSaveStrategy.cs`
- `src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs`
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs`

Verification
- Passed: `bash tools/check-format.sh`
- Passed: `git diff --check -- tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs`
- Local no-restore test attempt was blocked by missing cached `Microsoft.EntityFrameworkCore.Analyzers` packages `8.0.27` and `10.0.8` in this Linux sandbox. No restore was run because this execution boundary disallows network-dependent behavior.

Notes
- The repair targets the captured workspace failure from `dotnet test DVault.slnx --nologo`: the expected integration test class collection omitted the newly added public SQLite hash-storage test class, causing the collection mismatch at index 16.
- No public hash-key value type changes were made. Binary profile save/read boundaries remain canonical lowercase hexadecimal `string` values.
- DB2 live-schema support was not broadened.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Reworked the tester-returned coverage layout so hash-key storage profile assertions now live in existing schema, save/read, PIT, bridge, live-schema fixture, and provider-matrix test surfaces under `tests/DCoding.Data.DVault.Tests`.
- Removed the standalone `HashKeyStorageProfileSqliteTests` class and its provider discovery entry.
- Binary storage still binds SQLite optimized save/read hash-key values as provider bytes while preserving lowercase hexadecimal strings at caller-facing save/read APIs.
- Existing DB2 live-schema behavior remains an unsupported-provider assertion, not a new positive live execution target.

Repository artifacts
- `src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs`
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`
- `src/DCoding.Data.DVault.Sqlite/SqliteDataVaultSaveStrategy.cs`
- `src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs`
- `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs`
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs`
- `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs`

Verification
- Passed: `bash tools/check-format.sh`
- Passed: `bash tools/check-one-member-per-file.sh`
- Passed: `git diff --check -- tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs`
- Blocked before compilation: `dotnet build DVault.slnx --nologo --no-restore` because the local cache is missing analyzer packages including `Microsoft.EntityFrameworkCore.Analyzers` `8.0.27`/`10.0.8` and `xunit.analyzers` `1.27.0`.
- Blocked before compilation: targeted `dotnet test DVault.slnx --nologo --no-restore --filter ...` for the touched test classes because the same EF analyzer packages are missing from the local cache.

Notes
- The tester return specifically objected to the standalone hash-storage harness. This rework deletes that harness and distributes the coverage into the existing schema, explicit save/read, PIT, bridge, and shared fixture tests named by the ticket contract.
- No restore or network-dependent command was run in this execution boundary.
<!-- gicket-bot:developer-delivery:v1:end -->