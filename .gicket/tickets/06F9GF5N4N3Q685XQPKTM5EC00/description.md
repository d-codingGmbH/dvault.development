<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence fixes the v1 boundary: public and EF CLR-facing hash-key values stay canonical lowercase hexadecimal string values while binary storage is a persistence-only opt-in profile.
- Related ticket 06F9GF5FV54DGWY9GA8ZEZWM5R is done, so its older blocks relation is treated as historical routing context; the live ticket snapshot for 06F9GF5N4N3Q685XQPKTM5EC00 is is-blocked=false.
- This run updated the ticket description as the authoritative handoff surface and did not materialize child tickets, relation changes, attachments, or planning documents.

### Scope In
- Provider-neutral EF conversion and comparison behavior for DVault-owned HashKey and ParticipantReference properties when HashKeyStorageProfile.Binary is selected.
- Using the active stable-hash algorithmId and digestByteLength to translate canonical lowercase hex strings to fixed-length digest bytes and back.
- Deterministic null, equality, snapshot, and invalid-input behavior needed for EF change tracking, key comparison, and round-trip persistence tests.

### Scope Out
- Changing public or EF CLR-facing hash-key values from string to byte[].
- HashDiff or content-hash storage changes.
- Provider-specific binary store-type selection and capability mapping, tracked by 06F9GF5TNAXBCKN5BD9CKD7WVG.
- Broad schema, save, and read integration coverage, tracked by 06F9GF60BKEW0CC9FCZRPVX0SR.

## Acceptance Criteria
- When a model selects HashKeyStorageProfile.Binary, translated EF metadata applies provider-neutral conversion for DVault-owned HashKey and ParticipantReference properties while keeping the model and public value boundary as canonical lowercase hex string.
- The binary conversion path uses the active stable-hash algorithmId and digestByteLength and rejects invalid hex payloads or mismatched digest sizes with deterministic failures.
- EF comparison and snapshot behavior remains stable for equivalent canonical values, nulls, and change-tracking scenarios under the binary profile.
- HexString remains the default storage profile and preserves the existing none-string-model behavior without regression.
- Tests cover round-tripping for the built-in digest sizes plus equality, null handling, and invalid-input cases.

## Definition of Done
- The ticket description remains the authoritative handoff surface and reflects the binary profile contract, scope boundaries, and test expectations.
- Implementation is confined to the provider-neutral EF metadata projection and conversion layer and preserves existing storage-profile annotations and metadata facts.
- Automated tests prove binary round-trip, comparer or snapshot semantics, null behavior, and deterministic failure cases.
- No provider-specific mapping or broader integration-test work is pulled into this story.

## Implementation Notes
- Repository evidence already places binary hash conversion in DataVaultEfMetadataTranslator via HashKeyConversionBehavior and LowercaseHexStringToBytesConverter; this story should ratify that translator path as the provider-neutral implementation point and add or finish comparer coverage there.
- DataVaultProviderCapabilityProfile already sizes Binary mappings by algorithmId and digestByteLength, so this story should preserve those metadata facts instead of introducing a new configuration surface.
- Current live relation context remains parentOf 06F9GF5A8V7G3PAKGRXNYEBW5C and blocks 06F9GF5TNAXBCKN5BD9CKD7WVG; provider-specific store types remain downstream work.

## Open Questions
- none

## Follow-Up Questions
- After this provider-neutral conversion layer lands, should 06F9GF5TNAXBCKN5BD9CKD7WVG add any provider-specific capability diagnostics beyond the bounded binary store-type mappings already planned?
- After both this story and 06F9GF5TNAXBCKN5BD9CKD7WVG land, 06F9GF60BKEW0CC9FCZRPVX0SR should confirm end-to-end schema, save, and read behavior across the supported-provider baseline.

## Risks
- If comparer semantics do not align with the existing string model boundary, EF change tracking or key reuse can behave inconsistently even when byte conversion round-trips.
- A persisted blocks relation from done ticket 06F9GF5FV54DGWY9GA8ZEZWM5R still exists in relation files; treat it as historical until runtime cleanup occurs because the live ticket snapshot already reports is-blocked=false.

## Split Recommendations
- No further split is recommended. The current ticket is already bounded between done contract ticket 06F9GF5FV54DGWY9GA8ZEZWM5R, downstream provider-mapping ticket 06F9GF5TNAXBCKN5BD9CKD7WVG, and separate integration-test ticket 06F9GF60BKEW0CC9FCZRPVX0SR.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement provider-neutral EF conversion/comparison support so canonical hash values can be stored as fixed-length bytes where enabled. Preserve existing public string/canonical hex workflows unless the contract intentionally introduces a value object, and add tests for round-tripping, equality, null handling, and invalid digest sizes.