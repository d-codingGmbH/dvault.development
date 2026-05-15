[gicket-bot] PO refinement contract

Summary
- Refined this as one bounded implementation ticket for first-class PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers on top of the already-landed shared contract and fixture layer; no child tickets, relation changes, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Recent ticket comments are automation claim and lease comments only; there is no human clarification to reconcile.
- Local ticket-store evidence confirms Story 06F2PGFZWC5PXSDH46RCZPN1CG parentOf this task and done Task 06F2PGG57K3S7CJQP5QX9AWW3G blocks it; the blocker is already complete and supplies the authoritative shared contract and fixture baseline.
- Current branch code keeps DataVaultLiveSchemaReader SQLite-first today: SQLite has the only built-in reader, while recognized non-SQLite providers still fall through to UnsupportedProvider.
- The shared v1 live-schema contract is already fixed in code and tests through IDataVaultLiveSchemaReader, DataVaultLiveSchemaReadResult, DataVaultLiveSchemaReadStatus, LiveSchemaReaderContractFixture, and ExternalProviderLiveSchemaFixture.
- The finite v1 provider baseline for this ticket is PostgreSQL, SQL Server, Oracle, and MySQL, with MySQL covering both Pomelo.EntityFrameworkCore.MySql and MySql.EntityFrameworkCore provider names already recognized elsewhere in the repository.

Scope In
- Implement built-in catalog readers and dispatch for PostgreSQL, SQL Server, Oracle, and MySQL in the live-schema path used by DataVaultLiveSchemaReader.ReadAsync(...).
- Return the existing classified outcomes and snapshot object graph without changing the public contract: Succeeded, Unavailable, and UnsupportedProvider plus DataVaultLiveSchemaSnapshot, DataVaultLiveSchemaTable, DataVaultLiveSchemaColumn, DataVaultLiveSchemaPrimaryKey, and DataVaultLiveSchemaIndex.
- Read only the bounded DVault-owned schema surface already defined for drift comparison: produced tables, ordered columns with native storage types, named primary keys, and secondary indexes.
- Add provider-specific external opt-in integration coverage that provisions isolated objects through the existing reusable fixture helpers and compares the read result against the canonical expected snapshot.
- Keep provider-specific naming or schema adjustments bounded to physical catalog lookup needs such as schema-qualified names, Oracle identifier shortening, and MySQL table prefixes.

Scope Out
- Adding new public API types, statuses, snapshot fields, or drift-difference semantics.
- Expanding live-schema reading to foreign keys, views, sequences, triggers, arbitrary non-DVault objects, migration repair, or automatic schema changes.
- Reworking provider save strategies, provider package layout, or introducing new runtime configuration knobs for live-schema reading.
- Broad README, release-note, CI, or local-provisioning rollout work beyond minimal coherence updates; the dedicated documentation follow-up remains Task 06F2PGHA0EXJRGDHM4GQM7NPYR.

Open questions
- none

Follow-up questions
- After first-class readers land, should Task 06F2PGHA0EXJRGDHM4GQM7NPYR add SQL Server, Oracle, and MySQL fixture walkthroughs comparable to the existing PostgreSQL guidance?
- Do later CI or tooling tickets want opt-in smoke commands for the new provider readers, or should live-provider verification remain developer-managed and off by default?

Risks
- Provider catalogs differ in schema scoping, identifier casing, index metadata shape, and storage-type text, so normalization mistakes can create false drift or hide real mismatches.
- Oracle physical-name limits and MySQL dual provider-name support make identifier and dispatch handling more brittle than the existing SQLite path.
- External opt-in coverage depends on configured provider packages and create/drop permissions in developer-managed databases, so some defects may remain latent when those lanes are not exercised.

Split recommendations
- Keep this as one implementation ticket for now; the shared contract and fixture work is already split out, the provider set is finite, and the remaining work is one bounded dispatch-plus-reader slice. Re-split by provider only if catalog-specific helper code or external test setup becomes materially larger during development.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment