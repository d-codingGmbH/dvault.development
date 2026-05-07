<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Sibling ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W is now done, and this ticket can proceed by implementing the attached v1 shared driving-key contract artifact as the concrete source for multi-active opt-in, value passage, validation, ordering, and persistence uniqueness.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The concrete source of truth is the v1 shared contract artifact attached to 06EZ0NVX3RYPTFZKYCYEH9HB8W as 'multi-active-satellite-driving-key-contract.md' (attachment 06EZSBRWD150ATNH9T6FCXYQ2R), mirrored in docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md.
- Ordinary satellites remain the default. A satellite becomes multi-active only when one or more driving keys are declared through the sibling-approved contract surfaces, and ordinary satellites continue to expose empty driving-key collections.
- The finalized opt-in modeling surface is 'DataVaultSatelliteBuilder.DrivingKey(string propertyName)' plus the added 'DataVaultSatelliteMetadata(..., drivingKeyNames)' constructor and 'DrivingKeyNames' property in canonical declaration order.
- The finalized save surface is 'DataVaultSatelliteSaveOperation(..., drivingKeyValues, payloadValues, hashDiff)' plus 'DrivingKeyValues'; driving-key values stay separate from payload values and from hash-diff identity.
- Validation follows the sibling artifact exactly: declared driving-key names must be non-empty, ordinal-unique, and disjoint from payload names; multi-active saves must provide exactly one non-null value for each declared driving-key name and no extra names.
- Canonical multi-column order is declaration order. Caller enumeration order does not change that order, schema stores driving-key columns immediately after the parent hash-key column, and the latest-state partition is (parentHashKey, drivingKeyValue1, ..., drivingKeyValueN).
- No child-ticket, relation, attachment, or planning-document writes were materialized in this clarification pass because the blocker was resolved by existing sibling ticket state and the existing shared contract artifact.

### Scope In
- Implement the sibling-finalized public opt-in surfaces needed for this feature: 'DataVaultSatelliteBuilder.DrivingKey(string propertyName)', the added 'DataVaultSatelliteMetadata(..., drivingKeyNames)' constructor and 'DrivingKeyNames', and the added 'DataVaultSatelliteSaveOperation(..., drivingKeyValues, payloadValues, hashDiff)' constructor and 'DrivingKeyValues', plus required public API snapshot updates.
- Extend satellite schema projection for opt-in multi-active satellites to store ordered driving-key columns immediately after the parent hash-key column and to expand the satellite primary key and relevant index layout to (parentHashKey, drivingKeyValue1, ..., drivingKeyValueN, loadTimestamp).
- Partition latest-state lookup and unchanged replay suppression by parent hash key plus the canonical ordered driving-key value tuple for opt-in multi-active satellites only.
- Permit same-parent same-load-timestamp coexistence for different canonical driving-key tuples while preserving insert-only changed-row history within one ordered parent-plus-driving-key series.
- Add SQLite baseline coverage that proves canonical reordering of supplied driving-key values, deterministic RowsWritten and saved-record ordering, unchanged replay suppression, changed-row insertion, same-parent same-load-timestamp coexistence across different driving-key tuples, and required validation failures.

### Scope Out
- Renaming, redefining, or otherwise revisiting the sibling-finalized driving-key contract; this ticket must implement that contract, not reopen it.
- PIT tables, bridge tables, SaveChanges interception, and unrelated deferred capability families.
- Provider-specific optimized parity beyond safe decline and fallback unless a separate parity ticket explicitly scopes it.
- Multi-writer conflict resolution, retry semantics, or provider-specific merge and upsert guarantees beyond the current explicit save-service baseline.
- Same-series same-load-timestamp changed-row conflict semantics, which the shared contract leaves as follow-up work.
- Documentation and broader usage examples beyond implementation-proving coverage; 06EZ0NWCA6NEZH8VBJNGW4FVHG remains the follow-up owner.

## Acceptance Criteria
- Satellites become multi-active only when one or more driving keys are declared through the sibling-approved opt-in contract, while ordinary satellites keep the current builder, metadata, and save behavior unchanged and expose empty driving-key collections.
- Validation rejects empty or duplicate driving-key names, overlaps with payload names, missing or extra driving-key values, duplicate supplied names, and null driving-key values, while matching supplied names to canonical declaration order regardless of caller enumeration order.
- For opt-in multi-active satellites, translated schema stores driving-key columns immediately after the parent hash-key column and expands the satellite primary key and relevant index layout to (parentHashKey, drivingKeyValue1, ..., drivingKeyValueN, loadTimestamp) in canonical declaration order.
- A replay with the same parent hash key, the same canonical driving-key tuple, and the same latest hash diff writes no new row.
- For the same parent hash key and canonical driving-key tuple, a later changed hash diff inserts a new history row and preserves the earlier row unchanged.
- Rows with the same parent hash key and same load timestamp but different canonical driving-key tuples can both persist without colliding, and SQLite tests plus relevant public API or snapshot coverage prove deterministic RowsWritten, saved-record ordering, and persisted row contents.

## Definition of Done
- The provider-neutral save service and translated satellite schema honor the sibling-approved multi-active uniqueness and ordering rules without regressing hub, link, or ordinary satellite persistence.
- The contract-defined public opt-in and save surfaces are implemented exactly as specified by the shared artifact and are reflected in approved snapshot tests together with the required validation behavior.
- Any provider strategy that cannot yet honor the multi-active rules declines those batches so dispatch falls back to the provider-neutral writer.
- Required local SQLite baseline tests pass for validation failures, canonical ordering, unchanged replay suppression, changed-row insertion, same-parent same-load-timestamp coexistence across different driving-key tuples, and deterministic RowsWritten and saved-record ordering.

## Implementation Notes
- Normative source: sibling ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W at revision 06EZW1RHQJG9NR64PK4V5YZR74 and its attached artifact 'multi-active-satellite-driving-key-contract.md' (attachment 06EZSBRWD150ATNH9T6FCXYQ2R), mirrored in docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md.
- If older ticket prose or pre-contract clarification text differs in specificity from the shared artifact, the shared artifact wins for exact member names, constructor shapes, validation rules, canonical ordering, schema placement, and uniqueness semantics.
- The current non-opt-in baseline from docs/architecture/mvp-data-vault-concepts.md and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs is parent-hash-key plus load-timestamp with technical property order [ParentHashKey, HashDiff, LoadTimestamp, RecordSource] before payload columns; opt-in multi-active work inserts the ordered driving-key columns after ParentHashKey and before HashDiff.
- Keep driving-key values separate from payload values and hashDiff, and keep hashDiff as the payload-only change detector inside one (parentHashKey, drivingKey tuple) series.
- Current provider-neutral and optimized save paths track latest satellite hash diffs by ParentHashKey only; this ticket must either add sibling-approved partitioning or make optimized strategies decline multi-active batches.
- Update public API snapshots and any schema-projection assertions in the same slice as the implementation so the newly introduced contract surfaces and translated column or key order are locked together.
- This clarification pass materialized no child tickets, relation changes, attachments, or planning documents because the blocker was resolved by existing sibling ticket state and existing repository artifacts.

## Open Questions
- none

## Follow-Up Questions
- After the provider-neutral path is correct, should SQLite, Postgres, SQL Server, MySQL, and Oracle optimized strategies implement native multi-active partitioning or continue to decline those batches until separate parity tickets land?
- Should same-series same-load-timestamp changed-row conflict behavior be specified in a later ticket, since the shared contract explicitly leaves that semantic out of scope?

## Risks
- If implementers use stale pre-contract wording instead of the shared artifact, they can drift on exact public member names, value-passage shape, validation behavior, or canonical reordering rules.
- A partial implementation that updates schema projection without matching save validation and latest-state partitioning, or vice versa, can leave public API snapshots and persistence behavior inconsistent.
- Current optimized strategies and their CanSave gates do not yet inspect multi-active request shape; if they neither decline nor add the required partitioning rules, multi-active batches can be mishandled.
- Same-series same-load-timestamp changed-row conflict resolution remains follow-up work and must not be inferred from this ticket.

## Split Recommendations
- No new split is needed. Keep 06EZ0NVX3RYPTFZKYCYEH9HB8W as the completed contract-definition slice, keep this ticket focused on implementing the finalized public surface, schema translation, provider-neutral persistence, and proof coverage, and keep documentation or broader examples in 06EZ0NWCA6NEZH8VBJNGW4FVHG.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: implement persistence behavior for baseline multi-active satellites.

Acceptance Criteria:
- The save path inserts changed multi-active rows and suppresses unchanged duplicates based on parent key plus driving key plus hash diff.
- Tests cover insert-only history behavior for repeated saves and changed values.
- Provider-neutral behavior works in the local SQLite baseline.