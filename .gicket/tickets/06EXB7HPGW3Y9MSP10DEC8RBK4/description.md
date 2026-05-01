<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Resolved the PO-critic blockers by fixing the v1 satellite save contract to caller-supplied ParentHashKey plus deterministic HashDiff and by defining satellite-visible results on the explicit save-service boundary; no split or planning document was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket extends the explicit IDataVaultSaveService/DataVaultSaveRequest boundary by adding SatelliteOperations alongside the existing hub and link operations; it does not introduce SaveChanges interception or another implicit write path.
- Each satellite operation must include the target satellite metadata identity, an explicit ParentHashKey for the owning hub or link row, the payload values to persist, and a caller-supplied deterministic HashDiff.
- Parent resolution in v1 is by explicit ParentHashKey only. This ticket does not require the save service to derive a parent from business keys or from another operation in the same request.
- The save service persists the caller-supplied HashDiff as provided and compares it only to the latest persisted satellite row for the same parent hash key in the same satellite table.
- A payload that returns to an older historical value after an intervening change still counts as changed because comparison is against the current latest version for that parent, not any historical match.
- DataVaultSaveResult.SavedRecords must include deterministic satellite outcome entries. For a satellite entry, Kind is Satellite, MetadataName and TableName identify the satellite, and HashKey returns the parent hash key because satellites do not define an independent hash key in the v1 model.
- DataVaultSaveResult.RowsWritten continues to count only rows inserted by the current save call; an unchanged satellite operation still returns a satellite SavedRecord entry but contributes 0 to RowsWritten.
- No child-ticket split, relation write, attachment, or planning-document materialization was needed for this refinement.

### Scope In
- Add satellite persistence support to the explicit save-service flow alongside the current hub and link save operations.
- Add the caller-visible satellite request contract on DataVaultSaveRequest by accepting satellite operations with explicit ParentHashKey, payload values, and caller-supplied HashDiff.
- Persist parent hash key, payload columns, hash diff, load timestamp, and record source using the repository's existing satellite metadata and naming conventions.
- Suppress insertion when the latest persisted hash diff for the same parent hash key is unchanged, and insert a new historical row when it differs.
- Extend the caller-visible save result contract so DataVaultSaveResult.SavedRecords surfaces deterministic satellite outcome entries alongside the existing hub/link entries.
- Add automated SQLite-oriented tests covering changed, unchanged, parent-scoped historization, and the agreed satellite request/result behavior.

### Scope Out
- PIT tables, bridge tables, multi-active satellites, and other deferred post-MVP Data Vault capabilities.
- Provider-specific upsert, retry, multi-writer conflict handling, or non-SQLite optimization behavior beyond the current v1 baseline.
- A new implicit persistence mechanism such as EF SaveChanges interception.
- Repository-wide hash-diff algorithm, normalization, or field-selection rules inside the save service; v1 caller or domain code owns construction of the deterministic HashDiff submitted on the request boundary.
- A higher-level convenience API that derives ParentHashKey or HashDiff from arbitrary domain models without the caller supplying them explicitly.

## Acceptance Criteria
- DataVaultSaveRequest supports satellite save operations alongside the existing hub and link operations on the explicit IDataVaultSaveService boundary.
- Each satellite save operation requires the target satellite metadata identity, an explicit ParentHashKey for the owning hub or link row, the payload values to persist, and a caller-supplied deterministic HashDiff; LoadTimestamp and RecordSource continue to come from the request-level boundary.
- When a satellite row already exists for a parent hash key, saving another version with the same supplied HashDiff as the latest persisted row for that same parent does not insert a new satellite row.
- When a satellite row already exists for a parent hash key, saving a version with a different supplied HashDiff inserts a new satellite row and preserves the earlier historical row, even if the new payload matches an older non-latest historical version.
- Change detection is scoped to the same satellite table, the same ParentHashKey, and the current latest persisted version for that parent, not to unrelated parents or any historical match anywhere in the table.
- A changed insert persists the expected ParentHashKey, payload values, caller-supplied HashDiff, LoadTimestamp, and RecordSource through the existing SQLite EF Core baseline.
- DataVaultSaveResult.SavedRecords returns deterministic satellite outcome entries in addition to the existing hub/link entries; each satellite entry identifies the satellite and returns the parent hash key as its HashKey value, while RowsWritten still counts only rows actually inserted by the save call.
- Automated SQLite-oriented tests may use explicit text HashDiff values and must prove unchanged, changed, parent-scoped, and result-surface behavior for satellite saves without regressing the existing hub/link idempotent save baseline.

## Definition of Done
- All acceptance criteria pass in automated tests under the existing tests/DCoding.Data.DVault.Tests baseline.
- The public save-service contract and implementation align on the explicit v1 boundary: request-level LoadTimestamp and RecordSource plus hub, link, and satellite operations on DataVaultSaveRequest.
- The implementation reuses the existing translated satellite metadata conventions for parent hash key, hash diff, load timestamp, record source, and historical keying rather than introducing a separate satellite schema shape.
- The caller-visible result contract is updated so satellite saves have explicit SavedRecords behavior under the same deterministic result surface as other save operations.
- Implementation and tests follow the shared standards artifact and the referenced v1 Data Vault concept, naming, and stable-hashing guidance.

## Implementation Notes
- Treat satellite support as an additive extension of the existing IDataVaultSaveService/DataVaultSaveRequest boundary; do not introduce an interceptor-based or implicit persistence path.
- Add a SatelliteOperations collection to DataVaultSaveRequest and keep LoadTimestamp and RecordSource as request-level metadata shared by hub, link, and satellite operations.
- Use the existing satellite metadata translator and naming conventions. Satellite writes should target the translated satellite table for the requested metadata and use ParentHashKey as the lookup key for latest-version comparison.
- Do not compute or validate HashDiff inside the save path for this ticket. Persist and compare the caller-supplied deterministic HashDiff value as-is, because docs/plans/stable-hashing-contract.md leaves participating fields to domain-specific contracts and docs/architecture/mvp-data-vault-concepts.md allows explicit hash diff values in the SQLite-oriented MVP baseline.
- When a request mixes operation types, satellite operations still require an explicit ParentHashKey; this ticket does not require hidden same-request parent-resolution logic from newly submitted hub or link operations.
- Extend the current SavedRecords surface instead of introducing a separate satellite-only result DTO, and preserve deterministic hub-then-link-then-satellite ordering in request order.
- Preserve the current SQLite-focused provider baseline and deterministic pre-insert lookup approach for duplicate suppression and latest-version detection.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add a higher-level caller API that derives ParentHashKey and HashDiff from domain payload models instead of requiring those inputs explicitly on SatelliteOperations?
- Should multi-active satellite semantics be planned as a separate capability ticket instead of being inferred from this v1 historization rule?
- Should provider-specific conflict handling or SQL-side change detection be revisited once non-SQLite provider profiles are scheduled?

## Risks
- Because HashDiff is caller-supplied in v1, inconsistent field selection or normalization across callers can cause false changed or unchanged outcomes until a higher-level domain contract is introduced.
- The current provider baseline does not declare multi-writer concurrency support, so unchanged-row suppression and latest-version comparison remain based on deterministic pre-insert lookup rather than provider-neutral conflict handling.
- If downstream callers immediately need single-call parent creation plus satellite historization without precomputed parent hash keys, a follow-up convenience API may be needed even though this ticket intentionally keeps the save-service contract explicit.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Implement satellite historization behavior for changed payloads.

## Scope
- Compare hash diffs and insert new satellite versions only for changed data.

## Acceptance Criteria
- Changed payload tests create new rows.
- Unchanged payload tests do not create new rows.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.