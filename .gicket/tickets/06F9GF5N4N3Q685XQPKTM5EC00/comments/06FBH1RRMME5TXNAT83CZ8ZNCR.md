[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence fixes the v1 boundary: public and EF CLR-facing hash-key values stay canonical lowercase hexadecimal string values while binary storage is a persistence-only opt-in profile.
- Related ticket 06F9GF5FV54DGWY9GA8ZEZWM5R is done, so its older blocks relation is treated as historical routing context; the live ticket snapshot for 06F9GF5N4N3Q685XQPKTM5EC00 is is-blocked=false.
- This run updated the ticket description as the authoritative handoff surface and did not materialize child tickets, relation changes, attachments, or planning documents.

Scope In
- Provider-neutral EF conversion and comparison behavior for DVault-owned HashKey and ParticipantReference properties when HashKeyStorageProfile.Binary is selected.
- Using the active stable-hash algorithmId and digestByteLength to translate canonical lowercase hex strings to fixed-length digest bytes and back.
- Deterministic null, equality, snapshot, and invalid-input behavior needed for EF change tracking, key comparison, and round-trip persistence tests.

Scope Out
- Changing public or EF CLR-facing hash-key values from string to byte[].
- HashDiff or content-hash storage changes.
- Provider-specific binary store-type selection and capability mapping, tracked by 06F9GF5TNAXBCKN5BD9CKD7WVG.
- Broad schema, save, and read integration coverage, tracked by 06F9GF60BKEW0CC9FCZRPVX0SR.

Open questions
- none

Follow-up questions
- After this provider-neutral conversion layer lands, should 06F9GF5TNAXBCKN5BD9CKD7WVG add any provider-specific capability diagnostics beyond the bounded binary store-type mappings already planned?
- After both this story and 06F9GF5TNAXBCKN5BD9CKD7WVG land, 06F9GF60BKEW0CC9FCZRPVX0SR should confirm end-to-end schema, save, and read behavior across the supported-provider baseline.

Risks
- If comparer semantics do not align with the existing string model boundary, EF change tracking or key reuse can behave inconsistently even when byte conversion round-trips.
- A persisted blocks relation from done ticket 06F9GF5FV54DGWY9GA8ZEZWM5R still exists in relation files; treat it as historical until runtime cleanup occurs because the live ticket snapshot already reports is-blocked=false.

Split recommendations
- No further split is recommended. The current ticket is already bounded between done contract ticket 06F9GF5FV54DGWY9GA8ZEZWM5R, downstream provider-mapping ticket 06F9GF5TNAXBCKN5BD9CKD7WVG, and separate integration-test ticket 06F9GF60BKEW0CC9FCZRPVX0SR.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment