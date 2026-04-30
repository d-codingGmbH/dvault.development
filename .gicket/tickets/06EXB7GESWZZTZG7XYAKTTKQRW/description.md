<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the SQLite table-mapping ticket against verified repository, comment, and relation evidence; no new child tickets, relations, attachments, or planning documents were created in this PO pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified relation context: parent story 06EXB7G6YE4X0GA0CT7EPEFMPR owns the SQLite-usable EF model outcome, upstream task 06EXB7FYXNBPMH8VGQCGP2R41R blocks this work, and this ticket blocks schema-regression follow-up 06EXB7GPRGEJHKFMJ8MVAVF8ZG.
- Verified ticket comment and attachment state: no substantive human refinement comments or ticket attachments are present; the only local comment is the bot claim template.
- Verified repository baseline: ApplyDataVaultMetadata already projects hubs, links, and satellites into provider-neutral EF entity, property, key, and index metadata and stamps produced-name, entity-kind, property-role, technical-column-role, parent-reference, and ordinal annotations.
- Verified deterministic naming baseline: current source and tests already fix names such as HubCustomer, LinkCustomerOrder, SatCustomerContact, CustomerHashKey, HashDiff, LoadTimestamp, RecordSource, and the current primary-key and index compositions.
- No new child tickets, relations, attachments, or planning documents were created in this refinement pass.

### Scope In
- Map the existing provider-neutral EF metadata from ApplyDataVaultMetadata onto SQLite-backed relational table, column, primary-key, and index metadata for hubs, links, and satellites.
- Add the minimum EF relational and SQLite test infrastructure needed to build a model, create the schema in an ephemeral SQLite database, and inspect the resulting table shapes.
- Use the existing DVault-produced names and declared-order baseline as the source of truth for relational names and schema shape.
- Cover representative hub, link, hub-parent satellite, and link-parent satellite models in SQLite-focused tests.

### Scope Out
- Provider-capability abstraction work owned by 06EXB7J6HCA9QZ3DPP5Z03YGJ0.
- Snapshot or diff-style schema regression harness work owned by 06EXB7GPRGEJHKFMJ8MVAVF8ZG.
- Foreign keys, navigations, or other relationship metadata beyond the current provider-neutral baseline.
- Migrations, migration snapshots, non-SQLite providers, advanced configuration hooks, or runtime data-loading behavior.

## Acceptance Criteria
- Given a model built with UseDataVault plus ApplyDataVaultMetadata, SQLite relational metadata maps each hub entity to the expected table name, hash-key primary key, declared-order business-key columns, LoadTimestamp and RecordSource columns, and the expected unique business-key index.
- Given the same path, SQLite relational metadata maps each link entity to the expected table name, relationship hash-key primary key, declared-order participant hash-key columns, LoadTimestamp and RecordSource columns, and the expected non-unique relationship index.
- Given the same path, SQLite relational metadata maps each satellite entity to the expected table name, parent hash-key column, HashDiff, LoadTimestamp, RecordSource, declared-order payload columns, a primary key over parent hash key plus load timestamp, and the expected non-unique parent lookup index for both hub-parent and link-parent satellites.
- A SQLite integration test can create the schema for representative hub, link, and satellite models in an ephemeral database without handwritten DDL or migration artifacts.
- The created SQLite schema exposes the expected table names, technical metadata columns, primary keys, and indexes using the deterministic names already fixed by the current naming and EF-translation tests.
- UseDataVault by itself still records only the conventions marker and does not create DVault tables unless metadata translation is explicitly applied.

## Definition of Done
- Unit and integration coverage under tests/DCoding.Data.DVault.Tests proves relational mappings and successful SQLite schema creation for representative models.
- The library and affected test projects add only the minimal EF relational and SQLite dependencies needed for this ticket and stay on the repository net10.0 and EF Core 10 baseline.
- dotnet test DVault.slnx --nologo passes with the new relational and SQLite coverage included.
- bash tools/check-format.sh passes, and no new provider abstraction, migration pipeline, or advanced configuration surface is introduced.

## Implementation Notes
- Treat the existing DCoding.Data.DVault:ProducedName annotations on entities, properties, keys, and indexes as the v1 source of truth for relational names; do not reimplement naming rules in a separate SQLite-specific naming path.
- Build the SQLite mapping as a layer on top of the current provider-neutral EF translation rather than replacing or bypassing DataVaultEfMetadataTranslator and its role annotations.
- Current source already provides provider-neutral metadata translation, but the main library does not yet expose relational mapping behavior and the integration project does not yet reference the EF Core SQLite provider; plan only the minimal package additions needed for this ticket.
- Use a small test-only DbContext or equivalent model-building path for schema creation, and keep migrations out of scope for this ticket.
- Reuse the existing SqliteTestDatabase helper as the integration anchor and extend shared test helpers only as minimally needed to inspect sqlite_master or equivalent SQLite schema metadata.

## Open Questions
- none

## Follow-Up Questions
- When 06EXB7GPRGEJHKFMJ8MVAVF8ZG is implemented, decide whether long-lived schema regression protection should rely on generated SQL snapshots, sqlite_master capture, or another reviewable artifact.
- If later providers need different physical name escaping, type handling, or feature branching, 06EXB7J6HCA9QZ3DPP5Z03YGJ0 should define that capability boundary without changing the logical DVault naming contract.

## Risks
- If SQLite mapping recomputes names instead of consuming the existing produced-name annotations, the relational schema can drift from the verified provider-neutral naming baseline.
- If tests stop at EF metadata inspection and never create a real SQLite schema, the ticket can appear complete while missing provider-specific integration failures.
- If the implementation introduces foreign keys, migrations, or provider-capability branching here, it will leak scope already isolated into other tickets.

## Split Recommendations
- No additional split is recommended; the current graph already separates provider-neutral EF translation in 06EXB7FYXNBPMH8VGQCGP2R41R, this SQLite mapping task, provider-capability work in 06EXB7J6HCA9QZ3DPP5Z03YGJ0, and schema-regression follow-up in 06EXB7GPRGEJHKFMJ8MVAVF8ZG.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Implement relational mappings needed for Sqlite-backed tests and examples.

## Scope
- Create table, key, and column mappings for each vault structure.

## Acceptance Criteria
- Generated tables include technical metadata columns.
- Sqlite tests can create the schema.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.