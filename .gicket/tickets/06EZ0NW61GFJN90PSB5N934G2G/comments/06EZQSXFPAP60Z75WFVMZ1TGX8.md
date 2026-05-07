[gicket-bot] PO refinement contract

Summary
- Refined the multi-active persistence task around opt-in save-path semantics, SQLite baseline coverage, and the handoff boundary to the sibling driving-key contract ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This task implements persistence against the driving-key contract from ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W; it does not redefine that contract.
- Multi-active support is opt-in; ordinary satellites keep the current parent-only save behavior by default.
- For multi-active satellites, duplicate suppression is evaluated per parent hash key plus driving-key value set, not per parent alone.
- Hash diff keeps its existing role as the deterministic descriptive-payload change detector; driving-key values are a separate identity component for the active row series.
- Load timestamp remains the insert-only history axis, so multi-active support extends the current satellite history model rather than replacing it.

Scope In
- Persist the driving-key value set on opt-in multi-active satellite rows and include it in the generated schema and lookup shape needed for repeated saves.
- Update the explicit save path so latest-state checks and unchanged-replay suppression are partitioned by parent hash key plus driving-key value set.
- Allow multiple rows for the same parent at the same load timestamp when their driving-key values differ, while still inserting a new row when a later save changes the hash diff for one driving-key series.
- Add local SQLite baseline coverage that proves unchanged replay suppression, changed-row insertion, and same-parent different-driving-key coexistence.

Scope Out
- Defining the public driving-key modeling contract itself; that belongs to ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W.
- Documentation, examples, and broader support coverage beyond the implementation-proving tests required here; that belongs to ticket 06EZ0NWCA6NEZH8VBJNGW4FVHG.
- PIT tables, bridge tables, SaveChanges interception, and other deferred Data Vault capability families.
- Multi-writer conflict resolution, retry semantics, or provider-specific upsert and merge guarantees beyond the existing v1 save-service baseline.
- Full optimized-strategy parity across non-baseline providers unless needed so those strategies safely decline and fall back.

Open questions
- none

Follow-up questions
- After the provider-neutral path is correct, should SQLite, Postgres, SQL Server, and MySQL optimized strategies gain native multi-active handling or explicitly decline those batches until separate parity tickets land?
- Do we want a later ticket to define explicit conflict behavior for two distinct changed rows in the same parent-plus-driving-key series at the exact same load timestamp?

Risks
- Existing optimized provider strategies currently implement parent-only latest-hash-diff checks; without an explicit decline or parity update they could apply the wrong suppression rules to multi-active requests.
- The documented v1 save-service baseline still does not promise multi-writer conflict handling, so concurrent writers can race on the same parent-plus-driving-key series.

Split recommendations
- No further split is recommended; the parent story is already decomposed into driving-key contract, persistence, and docs/tests tasks.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment