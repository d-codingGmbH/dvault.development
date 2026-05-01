[gicket-bot] PO refinement contract

Summary
- Resolved the PO-critic blockers by fixing the v1 satellite save contract to caller-supplied ParentHashKey plus deterministic HashDiff and by defining satellite-visible results on the explicit save-service boundary; no split or planning document was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket now fixes the satellite request shape as an additive DataVaultSaveRequest extension with SatelliteOperations. Each satellite operation must carry the target satellite metadata identity, an explicit ParentHashKey for the owning hub or link row, the payload values to persist, and a caller-supplied deterministic HashDiff. LoadTimestamp and RecordSource remain request-level inputs on the existing explicit save-service boundary.
- critic-item-2: `answered` - The caller-visible result contract is now fixed as an additive extension of DataVaultSaveResult.SavedRecords rather than a separate DTO. Satellite save operations must produce deterministic SavedRecords entries alongside hub and link entries; each satellite entry identifies the satellite metadata and table and returns the parent hash key as its HashKey value. RowsWritten continues to count only rows actually inserted, so unchanged satellite operations still appear in SavedRecords while contributing 0 inserted rows.
- critic-item-3: `answered` - The acceptance contract now locks the hash-diff strategy: callers submit a deterministic HashDiff on each satellite operation, and the save path compares that supplied value against the latest persisted row for the same parent. This ticket does not ask developers to invent in-save hash-diff computation, and the SQLite-oriented tests may use explicit text HashDiff values to prove unchanged versus changed behavior.
- critic-item-4: `answered` - The blocking gap around request shape is resolved. Satellite persistence is now explicitly scoped as a new SatelliteOperations collection on DataVaultSaveRequest with required inputs of satellite metadata identity, ParentHashKey, payload values, and caller-supplied HashDiff.
- critic-item-5: `answered` - The blocking gap around result semantics is resolved. Satellite outcomes must surface through DataVaultSaveResult.SavedRecords under the same deterministic result surface as other save operations, with RowsWritten preserving inserted-row counting only.
- critic-item-6: `answered` - Hash-diff ownership is now fixed at the ticket level: the caller owns deterministic HashDiff construction and submits it explicitly on the satellite operation, while the save service only persists and compares that supplied value. Payload field participation and normalization are intentionally not computed inside this ticket's save path.

Clarifications
- This ticket extends the explicit IDataVaultSaveService/DataVaultSaveRequest boundary by adding SatelliteOperations alongside the existing hub and link operations; it does not introduce SaveChanges interception or another implicit write path.
- Each satellite operation must include the target satellite metadata identity, an explicit ParentHashKey for the owning hub or link row, the payload values to persist, and a caller-supplied deterministic HashDiff.
- Parent resolution in v1 is by explicit ParentHashKey only. This ticket does not require the save service to derive a parent from business keys or from another operation in the same request.
- The save service persists the caller-supplied HashDiff as provided and compares it only to the latest persisted satellite row for the same parent hash key in the same satellite table.
- A payload that returns to an older historical value after an intervening change still counts as changed because comparison is against the current latest version for that parent, not any historical match.
- DataVaultSaveResult.SavedRecords must include deterministic satellite outcome entries. For a satellite entry, Kind is Satellite, MetadataName and TableName identify the satellite, and HashKey returns the parent hash key because satellites do not define an independent hash key in the v1 model.
- DataVaultSaveResult.RowsWritten continues to count only rows inserted by the current save call; an unchanged satellite operation still returns a satellite SavedRecord entry but contributes 0 to RowsWritten.
- No child-ticket split, relation write, attachment, or planning-document materialization was needed for this refinement.

Scope In
- Add satellite persistence support to the explicit save-service flow alongside the current hub and link save operations.
- Add the caller-visible satellite request contract on DataVaultSaveRequest by accepting satellite operations with explicit ParentHashKey, payload values, and caller-supplied HashDiff.
- Persist parent hash key, payload columns, hash diff, load timestamp, and record source using the repository's existing satellite metadata and naming conventions.
- Suppress insertion when the latest persisted hash diff for the same parent hash key is unchanged, and insert a new historical row when it differs.
- Extend the caller-visible save result contract so DataVaultSaveResult.SavedRecords surfaces deterministic satellite outcome entries alongside the existing hub/link entries.
- Add automated SQLite-oriented tests covering changed, unchanged, parent-scoped historization, and the agreed satellite request/result behavior.

Scope Out
- PIT tables, bridge tables, multi-active satellites, and other deferred post-MVP Data Vault capabilities.
- Provider-specific upsert, retry, multi-writer conflict handling, or non-SQLite optimization behavior beyond the current v1 baseline.
- A new implicit persistence mechanism such as EF SaveChanges interception.
- Repository-wide hash-diff algorithm, normalization, or field-selection rules inside the save service; v1 caller or domain code owns construction of the deterministic HashDiff submitted on the request boundary.
- A higher-level convenience API that derives ParentHashKey or HashDiff from arbitrary domain models without the caller supplying them explicitly.

Open questions
- none

Follow-up questions
- Should a later ticket add a higher-level caller API that derives ParentHashKey and HashDiff from domain payload models instead of requiring those inputs explicitly on SatelliteOperations?
- Should multi-active satellite semantics be planned as a separate capability ticket instead of being inferred from this v1 historization rule?
- Should provider-specific conflict handling or SQL-side change detection be revisited once non-SQLite provider profiles are scheduled?

Risks
- Because HashDiff is caller-supplied in v1, inconsistent field selection or normalization across callers can cause false changed or unchanged outcomes until a higher-level domain contract is introduced.
- The current provider baseline does not declare multi-writer concurrency support, so unchanged-row suppression and latest-version comparison remain based on deterministic pre-insert lookup rather than provider-neutral conflict handling.
- If downstream callers immediately need single-call parent creation plus satellite historization without precomputed parent hash keys, a follow-up convenience API may be needed even though this ticket intentionally keeps the save-service contract explicit.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment