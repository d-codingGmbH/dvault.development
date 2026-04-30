[gicket-bot] PO refinement contract

Summary
- Refined the SQLite table-mapping ticket against verified repository, comment, and relation evidence; no new child tickets, relations, attachments, or planning documents were created in this PO pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified relation context: parent story 06EXB7G6YE4X0GA0CT7EPEFMPR owns the SQLite-usable EF model outcome, upstream task 06EXB7FYXNBPMH8VGQCGP2R41R blocks this work, and this ticket blocks schema-regression follow-up 06EXB7GPRGEJHKFMJ8MVAVF8ZG.
- Verified ticket comment and attachment state: no substantive human refinement comments or ticket attachments are present; the only local comment is the bot claim template.
- Verified repository baseline: ApplyDataVaultMetadata already projects hubs, links, and satellites into provider-neutral EF entity, property, key, and index metadata and stamps produced-name, entity-kind, property-role, technical-column-role, parent-reference, and ordinal annotations.
- Verified deterministic naming baseline: current source and tests already fix names such as HubCustomer, LinkCustomerOrder, SatCustomerContact, CustomerHashKey, HashDiff, LoadTimestamp, RecordSource, and the current primary-key and index compositions.
- No new child tickets, relations, attachments, or planning documents were created in this refinement pass.

Scope In
- Map the existing provider-neutral EF metadata from ApplyDataVaultMetadata onto SQLite-backed relational table, column, primary-key, and index metadata for hubs, links, and satellites.
- Add the minimum EF relational and SQLite test infrastructure needed to build a model, create the schema in an ephemeral SQLite database, and inspect the resulting table shapes.
- Use the existing DVault-produced names and declared-order baseline as the source of truth for relational names and schema shape.
- Cover representative hub, link, hub-parent satellite, and link-parent satellite models in SQLite-focused tests.

Scope Out
- Provider-capability abstraction work owned by 06EXB7J6HCA9QZ3DPP5Z03YGJ0.
- Snapshot or diff-style schema regression harness work owned by 06EXB7GPRGEJHKFMJ8MVAVF8ZG.
- Foreign keys, navigations, or other relationship metadata beyond the current provider-neutral baseline.
- Migrations, migration snapshots, non-SQLite providers, advanced configuration hooks, or runtime data-loading behavior.

Open questions
- none

Follow-up questions
- When 06EXB7GPRGEJHKFMJ8MVAVF8ZG is implemented, decide whether long-lived schema regression protection should rely on generated SQL snapshots, sqlite_master capture, or another reviewable artifact.
- If later providers need different physical name escaping, type handling, or feature branching, 06EXB7J6HCA9QZ3DPP5Z03YGJ0 should define that capability boundary without changing the logical DVault naming contract.

Risks
- If SQLite mapping recomputes names instead of consuming the existing produced-name annotations, the relational schema can drift from the verified provider-neutral naming baseline.
- If tests stop at EF metadata inspection and never create a real SQLite schema, the ticket can appear complete while missing provider-specific integration failures.
- If the implementation introduces foreign keys, migrations, or provider-capability branching here, it will leak scope already isolated into other tickets.

Split recommendations
- No additional split is recommended; the current graph already separates provider-neutral EF translation in 06EXB7FYXNBPMH8VGQCGP2R41R, this SQLite mapping task, provider-capability work in 06EXB7J6HCA9QZ3DPP5Z03YGJ0, and schema-regression follow-up in 06EXB7GPRGEJHKFMJ8MVAVF8ZG.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment