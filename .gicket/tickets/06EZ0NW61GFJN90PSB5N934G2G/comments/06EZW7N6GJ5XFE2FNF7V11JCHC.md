[gicket-bot] PO refinement contract

Summary
- Sibling ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W is now done, and this ticket can proceed by implementing the attached v1 shared driving-key contract artifact as the concrete source for multi-active opt-in, value passage, validation, ordering, and persistence uniqueness.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - The prerequisite refinement is complete. Treat ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W revision 06EZW1RHQJG9NR64PK4V5YZR74 plus its attached v1 shared contract artifact 'multi-active-satellite-driving-key-contract.md' (attachment 06EZSBRWD150ATNH9T6FCXYQ2R, mirrored in docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md) as the finalized source for the opt-in declaration, the separate driving-key value-passage shape, the validation rules, and the deterministic ordering rules.
- critic-item-2: `answered` - This ticket is now explicitly anchored to that shared artifact. In this ticket, 'opt-in multi-active satellite' means the sibling-defined driving-key opt-in with 'DataVaultSatelliteBuilder.DrivingKey(string propertyName)' and ordered 'DataVaultSatelliteMetadata(..., drivingKeyNames)', and 'driving-key value set' means the sibling-defined 'DataVaultSatelliteSaveOperation(..., drivingKeyValues, payloadValues, hashDiff)' contract with name-matched values reordered to canonical declaration order.
- critic-item-3: `answered` - The earlier prerequisite blocker is closed. Developers no longer need to invent the driving-key contract because sibling ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W is already done and its shared contract artifact resolves the previously open source-of-truth question. The remaining work on this ticket is implementation against that approved contract.
- critic-item-4: `answered` - The repository still shows the ordinary-satellite baseline only, so this ticket now explicitly includes implementing the already-finalized public opt-in surfaces required by persistence instead of inventing alternate behavior: 'DataVaultSatelliteBuilder.DrivingKey(string propertyName)', 'DataVaultSatelliteMetadata(string name, DataVaultMetadataReference parent, IEnumerable<string> descriptiveAttributeNames, IEnumerable<string> drivingKeyNames)' plus 'DrivingKeyNames', and 'DataVaultSatelliteSaveOperation(DataVaultSatelliteMetadata metadata, string parentHashKey, IEnumerable<KeyValuePair<string, string>> drivingKeyValues, IEnumerable<KeyValuePair<string, string>> payloadValues, string hashDiff)' plus 'DrivingKeyValues', along with the required validation and snapshot coverage.

Clarifications
- The concrete source of truth is the v1 shared contract artifact attached to 06EZ0NVX3RYPTFZKYCYEH9HB8W as 'multi-active-satellite-driving-key-contract.md' (attachment 06EZSBRWD150ATNH9T6FCXYQ2R), mirrored in docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md.
- Ordinary satellites remain the default. A satellite becomes multi-active only when one or more driving keys are declared through the sibling-approved contract surfaces, and ordinary satellites continue to expose empty driving-key collections.
- The finalized opt-in modeling surface is 'DataVaultSatelliteBuilder.DrivingKey(string propertyName)' plus the added 'DataVaultSatelliteMetadata(..., drivingKeyNames)' constructor and 'DrivingKeyNames' property in canonical declaration order.
- The finalized save surface is 'DataVaultSatelliteSaveOperation(..., drivingKeyValues, payloadValues, hashDiff)' plus 'DrivingKeyValues'; driving-key values stay separate from payload values and from hash-diff identity.
- Validation follows the sibling artifact exactly: declared driving-key names must be non-empty, ordinal-unique, and disjoint from payload names; multi-active saves must provide exactly one non-null value for each declared driving-key name and no extra names.
- Canonical multi-column order is declaration order. Caller enumeration order does not change that order, schema stores driving-key columns immediately after the parent hash-key column, and the latest-state partition is (parentHashKey, drivingKeyValue1, ..., drivingKeyValueN).
- No child-ticket, relation, attachment, or planning-document writes were materialized in this clarification pass because the blocker was resolved by existing sibling ticket state and the existing shared contract artifact.

Scope In
- Implement the sibling-finalized public opt-in surfaces needed for this feature: 'DataVaultSatelliteBuilder.DrivingKey(string propertyName)', the added 'DataVaultSatelliteMetadata(..., drivingKeyNames)' constructor and 'DrivingKeyNames', and the added 'DataVaultSatelliteSaveOperation(..., drivingKeyValues, payloadValues, hashDiff)' constructor and 'DrivingKeyValues', plus required public API snapshot updates.
- Extend satellite schema projection for opt-in multi-active satellites to store ordered driving-key columns immediately after the parent hash-key column and to expand the satellite primary key and relevant index layout to (parentHashKey, drivingKeyValue1, ..., drivingKeyValueN, loadTimestamp).
- Partition latest-state lookup and unchanged replay suppression by parent hash key plus the canonical ordered driving-key value tuple for opt-in multi-active satellites only.
- Permit same-parent same-load-timestamp coexistence for different canonical driving-key tuples while preserving insert-only changed-row history within one ordered parent-plus-driving-key series.
- Add SQLite baseline coverage that proves canonical reordering of supplied driving-key values, deterministic RowsWritten and saved-record ordering, unchanged replay suppression, changed-row insertion, same-parent same-load-timestamp coexistence across different driving-key tuples, and required validation failures.

Scope Out
- Renaming, redefining, or otherwise revisiting the sibling-finalized driving-key contract; this ticket must implement that contract, not reopen it.
- PIT tables, bridge tables, SaveChanges interception, and unrelated deferred capability families.
- Provider-specific optimized parity beyond safe decline and fallback unless a separate parity ticket explicitly scopes it.
- Multi-writer conflict resolution, retry semantics, or provider-specific merge and upsert guarantees beyond the current explicit save-service baseline.
- Same-series same-load-timestamp changed-row conflict semantics, which the shared contract leaves as follow-up work.
- Documentation and broader usage examples beyond implementation-proving coverage; 06EZ0NWCA6NEZH8VBJNGW4FVHG remains the follow-up owner.

Open questions
- none

Follow-up questions
- After the provider-neutral path is correct, should SQLite, Postgres, SQL Server, MySQL, and Oracle optimized strategies implement native multi-active partitioning or continue to decline those batches until separate parity tickets land?
- Should same-series same-load-timestamp changed-row conflict behavior be specified in a later ticket, since the shared contract explicitly leaves that semantic out of scope?

Risks
- If implementers use stale pre-contract wording instead of the shared artifact, they can drift on exact public member names, value-passage shape, validation behavior, or canonical reordering rules.
- A partial implementation that updates schema projection without matching save validation and latest-state partitioning, or vice versa, can leave public API snapshots and persistence behavior inconsistent.
- Current optimized strategies and their CanSave gates do not yet inspect multi-active request shape; if they neither decline nor add the required partitioning rules, multi-active batches can be mishandled.
- Same-series same-load-timestamp changed-row conflict resolution remains follow-up work and must not be inferred from this ticket.

Split recommendations
- No new split is needed. Keep 06EZ0NVX3RYPTFZKYCYEH9HB8W as the completed contract-definition slice, keep this ticket focused on implementing the finalized public surface, schema translation, provider-neutral persistence, and proof coverage, and keep documentation or broader examples in 06EZ0NWCA6NEZH8VBJNGW4FVHG.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment