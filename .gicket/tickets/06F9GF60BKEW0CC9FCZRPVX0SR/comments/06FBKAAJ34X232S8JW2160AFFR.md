[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the persisted contract is clear, bounded, and supported by direct repo evidence that Binary hash-key behavior already exists at the unit/API level while the requested schema/save/read integration gap remains open.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F9GF60BKEW0CC9FCZRPVX0SR/description.md` contains a delivery contract with `## Open Questions` set to `- none` plus explicit acceptance criteria for SQLite HexString/Binary schema, save/read, provider-matrix, and negative coverage.
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageProfile.cs` defines `HexString` and `Binary`; `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs` exposes `WithHashKeyStorageProfile(...)` and keeps Binary on CLR type `string` with conversion behavior `lowercase-hex-string-to-bytes`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs` already has `BinaryHashKeyStorageProfileIsExplicitOptInAndKeepsStringModelBoundary`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` already covers Binary metadata application, converter round-trip, and invalid provider/model values.
- `rg -n "DataVaultHashKeyStorageProfile\.Binary|WithHashKeyStorageProfile\(|LowercaseHexBinary" tests/DCoding.Data.DVault.Tests/Integration -S` returned no matches, which directly supports the contract statement that current integration coverage has not opted into Binary yet.
- Existing bounded extension points are present at `tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs`, `SqliteDataVaultSchemaTests.cs`, `ExplicitDataVaultSaveServiceSqliteTests.cs`, `DataVaultPitReadServiceSqliteTests.cs`, `DataVaultBridgeReadServiceSqliteTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assuming provider-profile matrix assertions alone satisfy the ticket. The contract still requires at least one executable SQLite Binary save-plus-read round-trip across latest/current, explicit as-of, PIT as-of, and bridge traversal request shapes.
- Assuming Binary changes public hash-key types to bytes. Direct source evidence keeps the public/model boundary as lowercase hexadecimal `string` values.
- Assuming DB2 should gain positive live-schema execution here. The contract keeps DB2 as an unsupported-provider negative assertion for this ticket.

AC / test suggestions
- Keep at least one non-64-character digest case in scope, matching the contract note about reusing `sha256-128-v1` and existing stable-hash helpers.
- Make the provider-matrix expectation explicit in assertions for the six built-in profiles so dev can prove HexString defaults versus Binary opt-in without adding external database infrastructure.

Implementation watchouts
- The current integration suite already has separate schema, save, PIT, and bridge SQLite surfaces; the ticket should extend those surfaces instead of introducing a parallel harness.
- `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractFixtureTests.cs` currently asserts provider storage types without Binary cases, so Binary additions there should preserve logical names and shape while varying only physical store type and digest sizing.
- Because the current branch history is ticket-handoff-only, developers should expect to start from the present baseline rather than from partially implemented Binary integration work on this branch.

Non-blocking notes
- The persisted contract is consistent with `docs/plans/hash-key-storage-profile-contract.md`: HexString is default, Binary is explicit opt-in, and API/read/save boundaries remain lowercase hex strings.
- The outgoing `blocks` relation to ticket `06F9GF66B10J4K7RBDTJ9NQRQC` remains follow-up benchmarking work and does not block developer handoff for this test-coverage ticket.

Split recommendations
- No split recommended for the current bounded test-gap scope.
- If stakeholders later want live Binary execution across optional external providers or explicit diagnostics/support-bundle Binary matrices, keep that as follow-up ticket work rather than broadening this handoff.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment