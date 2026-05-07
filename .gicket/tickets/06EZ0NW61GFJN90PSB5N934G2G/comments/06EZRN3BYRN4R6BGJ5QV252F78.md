[gicket-bot] PO refinement contract

Summary
- Ticket requires additional substantive product clarification before continuing.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `cannot_answer` - Ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W is still todo/needs-po at revision 06EZ0Y4PSCQP9HWDFKTBY5JX4C, and its persisted description is still only a goal plus three generic bullets. The exact multi-active opt-in declaration, separate driving-key value-passage shape, validation rules, and deterministic multi-column ordering contract are not yet finalized there, so this ticket cannot treat that dependency as handoff-ready.
- critic-item-2: `cannot_answer` - This ticket already points at 06EZ0NVX3RYPTFZKYCYEH9HB8W as its contract owner, but there is still no finalized sibling contract revision or attached artifact to cite as the concrete source of truth. Until that sibling is refined, this ticket must stay in needs_po_clarification and explicitly wait to cite the finalized opt-in and driving-key contract.
- critic-item-3: `answered` - Confirmed. This ticket explicitly says it implements persistence against the sibling driving-key contract and does not redefine it, while its current contract still says Open Questions none. Because 06EZ0NVX3RYPTFZKYCYEH9HB8W remains unresolved, approving this task now would force developers to invent the missing caller-visible contract themselves.
- critic-item-4: `answered` - Confirmed. The public modeling and save-request surface still exposes only satellite payload names plus parentHashKey, payloadValues, and hashDiff at save time. The translated schema and save paths still treat satellites as parent-hash-key-plus-load-timestamp rows with latest-hash-diff tracking keyed only by ParentHashKey, so multi-active opt-in and driving-key passage are not publicly defined yet.

Clarifications
- The architecture baseline still treats multi-active satellites as a deferred opt-in capability layered on top of the ordinary parent-hash-key-plus-load-timestamp satellite shape.
- The current public modeling surface is still DataVaultSatelliteBuilder.Payload(string) plus DataVaultSatelliteMetadata(name, parent, descriptiveAttributeNames); no persisted multi-active opt-in or driving-key metadata contract exists yet.
- The current explicit save surface is still DataVaultSatelliteSaveOperation(metadata, parentHashKey, payloadValues, hashDiff); separate driving-key value passage must come from sibling ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W before this task can proceed.
- No child-ticket, relation, or planning-document writes were materialized in this run because the blocker is upstream contract finalization, not missing decomposition.

Scope In
- After 06EZ0NVX3RYPTFZKYCYEH9HB8W finalizes the contract, extend opt-in multi-active satellite schema projection with the sibling-approved ordered driving-key columns and persisted value storage.
- Partition unchanged replay suppression and latest-hash-diff checks by parent hash key plus the ordered driving-key value set for opt-in multi-active satellites only.
- Permit same-parent same-load-timestamp coexistence for different ordered driving-key value sets while preserving later changed-row history insertion for one ordered series.
- Add SQLite baseline coverage and any required schema or public-API snapshot updates that prove RowsWritten, saved-record ordering, unchanged replay suppression, changed-row insertion, and coexistence.

Scope Out
- Finalizing or renaming the public multi-active modeling and save-request contract; sibling ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W owns that work.
- PIT tables, bridge tables, SaveChanges interception, and other unrelated deferred capability families.
- Multi-writer conflict resolution, retry semantics, or provider-specific upsert and merge guarantees beyond the current explicit save-service baseline.
- Provider-specific optimized parity beyond safe decline and fallback unless separately scoped.
- Documentation and broader examples beyond implementation-proving coverage; ticket 06EZ0NWCA6NEZH8VBJNGW4FVHG owns that follow-up.

Open questions
- Which exact finalized contract revision or attached artifact from ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W will define the opt-in declaration, separate driving-key value-passage shape, validation rules, and deterministic multi-column ordering that this ticket must cite before returning to PO-critic?

Follow-up questions
- After the provider-neutral path is correct, should SQLite, Postgres, SQL Server, MySQL, and Oracle optimized strategies gain native multi-active handling or explicitly decline those batches until separate parity tickets land?
- Do we want a later ticket to define explicit conflict behavior for two distinct changed rows in the same parent-plus-driving-key series at the exact same load timestamp?

Risks
- If implementation starts before the sibling contract lands, developers will have to invent caller-visible API names, validation rules, and ordering behavior outside the approved scope of this ticket.
- Current satellite primary keys and indexes remain parent hash key plus load timestamp, so same-parent same-load-timestamp different-driving-key rows still collide until schema changes include the sibling-approved driving-key columns.
- Current provider-neutral and optimized save paths track latest satellite hash diffs by ParentHashKey only, and optimized strategy CanSave gates do not inspect request shape, so multi-active batches can be mishandled unless they decline or gain parity.
- The explicit save-service baseline still does not promise multi-writer conflict handling for one parent-plus-driving-key series.

Split recommendations
- No new split is needed. Keep the existing decomposition: finalize 06EZ0NVX3RYPTFZKYCYEH9HB8W first, then resume this persistence ticket, with documentation and broader examples still handled by 06EZ0NWCA6NEZH8VBJNGW4FVHG.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment