[gicket-bot] PO-critic review contract

Summary
- Persisted contract is specific, grounded in existing repo APIs and tests, and contains no unresolved open questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGG57K3S7CJQP5QX9AWW3G/description.md persists a Delivery Contract with Open Questions = none, 4 acceptance-criteria bullets, 4 definition-of-done bullets, and 5 implementation notes.
- Direct source evidence for the ratified public contract exists in src/DCoding.Data.DVault/IDataVaultLiveSchemaReader.cs:8-20, DataVaultLiveSchemaReadStatus.cs:6-20, DataVaultLiveSchemaReadResult.cs:14-107, DataVaultLiveSchemaSnapshot.cs:6-23, DataVaultLiveSchemaTable.cs:14-55, DataVaultLiveSchemaColumn.cs:1-12, DataVaultLiveSchemaPrimaryKey.cs:12-33, and DataVaultLiveSchemaIndex.cs:13-40.
- src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:13-35 dispatches only the SQLite built-in reader and returns UnsupportedProvider otherwise; README.md:455-473 documents the same bounded surface and SQLite-first posture.
- src/DCoding.Data.DVault/DataVaultLiveSchemaDriftReporter.cs:109-188 already exposes the explicit IDataVaultLiveSchemaReader CompareAsync overloads that the ticket references for downstream provider-reader tests.
- tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs:12-169 already proves no-drift SQLite success, deterministic difference ordering, UnsupportedProvider, and Unavailable; its CreateMetadataModel() at lines 179-199 uses the Customer/Order/CustomerOrder/Contact/State scenario named in the ticket.
- External opt-in foundations already exist in tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs:3-35, SqlServerIntegrationTestConfiguration.cs:3-35, OracleIntegrationTestConfiguration.cs:3-35, MySqlIntegrationTestConfiguration.cs:3-35, the provider reflection helpers, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:16-20, and tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs:31-190.
- Relation evidence is direct: .gicket/tickets/06F2PGFZWC5PXSDH46RCZPN1CG/events/06F2PGR9SA3E0PBRNZA6ZHP9MC.json records parentOf from story 06F2PGFZWC5PXSDH46RCZPN1CG to this ticket, and .gicket/tickets/06F2PGG57K3S7CJQP5QX9AWW3G/events/06F2PGW579293ZAGA1W72ARBVR.json records blocks from this ticket to 06F2PGG8ZKSYGC8863118H56G8.
- git show --stat 3b18f0595 -- .gicket/tickets/06F2PGG57K3S7CJQP5QX9AWW3G shows the PO handoff commit changed only ticket description, comments, events, and ticket.json; git diff --name-only develop...HEAD returned only .gicket/tickets/06F2PGG57K3S7CJQP5QX9AWW3G/**, so the branch history is ticket-refinement-only rather than partially implemented code.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The AC text does not give an explicit example of the Oracle physical-name override path even though the implementation notes anticipate identifier-length pressure.
- The contract does not include a concrete example for provider catalog storage-type aliases or casing differences, even though DataVaultLiveSchemaColumn carries raw ProviderStorageType text.

Risky assumptions
- Assumes PostgreSQL, SQL Server, Oracle, and MySQL catalogs can all map into the existing DataVaultLiveSchemaPrimaryKey and DataVaultLiveSchemaIndex shapes without reopening the public API.
- Assumes developer-managed external databases grant create and drop permissions for isolated objects; README external-provider guidance already makes those lanes opt-in.

AC / test suggestions
- Require one canonical expected-snapshot fixture artifact reused by SQLite and every external provider-reader suite so ordering and naming remain identical.
- Require one opt-in contract test per external provider for deterministic table, column, primary-key, and index ordering plus exact success versus unavailable status classification.
- Require at least one Oracle-oriented fixture case that exercises physical-name shortening while preserving the same logical drift surface.

Implementation watchouts
- Do not broaden the live-schema surface beyond DVault-owned tables, ordered columns, primary-key name and columns, and secondary indexes; README.md:455-473 and the current public contract are intentionally narrower than full catalog parity.
- Keep default local smoke SQLite-first; the conditional provider package restore pattern in tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:16-20 and the trait enforcement in ProviderIntegrationCategoryDiscoveryTests.cs:31-190 should remain intact.
- Use the explicit IDataVaultLiveSchemaReader drift-reporter overloads instead of changing built-in static dispatch in this ticket; DataVaultLiveSchemaReader.cs:13-35 currently hard-codes SQLite-only dispatch.

Non-blocking notes
- The current branch contains ticket metadata only, which is consistent with a PO refinement handoff and means development will begin from already-landed contract code in src/ and tests/ rather than a new code delta on this branch.

Split recommendations
- No split is needed for this contract-and-fixture ticket. If provider catalog quirks make implementation too large, split Task 06F2PGG8ZKSYGC8863118H56G8 by provider after this contract ticket lands.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment