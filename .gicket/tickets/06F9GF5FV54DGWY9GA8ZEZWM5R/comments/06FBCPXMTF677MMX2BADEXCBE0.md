[gicket-bot] PO refinement contract

Summary
- Refined the ticket to ratify canonical lowercase-hex logical hash keys, a model-level hash-key storage profile with HexString default and Binary opt-in, six visible provider profiles including DB2, and fail-closed migration/diagnostic boundaries; no persistent ticket or planning writes were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the logical hash-key surface as string-based canonical lowercase hexadecimal across stable-hash results, save/read requests, and diagnostics; this ticket should define physical storage separately without changing those logical/public shapes.
- The selected stable hash algorithm is model-wide through the registered IStableHashService/DataVaultConventions boundary, so this contract should treat digest byte length as one fixed value per translated model, not as a per-row mixed-length concern.
- HexString is the v1 default storage profile and Binary is explicit opt-in only; Binary must round-trip the exact same logical lowercase-hex hash-key values and digest semantics as HexString.
- The visible built-in provider baseline is six capability profiles: sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1; this ticket should align to that repository baseline even though one older planning document still lists only five providers.
- Persisted relation context was verified: this ticket is currently a child of 06F9GF5A8V7G3PAKGRXNYEBW5C and blocks 06F9GF5N4N3Q685XQPKTM5EC00.

Scope In
- Define a model-level hash-key storage profile contract that separates logical hash-key representation from physical provider storage.
- Cover DVault-owned hash-key-shaped columns and references that already carry hash-key semantics: hub keys, link keys, link participant hash-key references, satellite parent hash-key references, PIT parent hash-key references, and bridge endpoint hash-key references.
- Define provider capability, EF annotation, explain/support-bundle, live-schema, and migration-guardrail behavior needed to make the selected storage profile and digest length machine-readable.
- Define compatibility rules for the existing v0.35.0 stable hash algorithm ids and their fixed digest byte lengths when persisted through the selected storage profile.

Scope Out
- Changing public or logical hash-key surfaces from canonical lowercase hex strings to byte[] or provider-specific types.
- Automatic rehashing, backfill, dual-write, repair, or migration tooling when callers change algorithm id or storage profile after data is already persisted.
- Provider-side SQL hashing, security/compliance hashing features, or changes to the separate persistence content_hash contract.
- Implementing a DB2 live-schema reader; DB2 live-schema remains an explicit unsupported boundary unless a later ticket reopens it.
- Redefining caller-supplied HashDiff storage semantics; this ticket is about hash-key storage and hash-key references.

Open questions
- none

Follow-up questions
- Should a later ticket extend the same storage-profile abstraction to caller-supplied HashDiff columns once the hash-key contract is landed and proven?
- Should a later ticket add reviewed migration tooling or data-move guidance for consumers who deliberately convert persisted HexString columns to Binary outside DVault's fail-closed default posture?
- Should a later ticket bring DB2 live-schema reading up to parity so runtime drift checks can validate hash-key storage shape against the catalog for DB2 as well?

Risks
- This ticket remains a dependency for blocked ticket 06F9GF5N4N3Q685XQPKTM5EC00, so delayed contract landing will continue to hold downstream work.
- If hash-key storage profile facts are not applied consistently across keys and hash-key references, joins, indexes, and cross-table comparisons can drift even when logical hash values still look valid at the API boundary.
- Changing algorithm id or converting persisted text columns to binary remains caller-owned compatibility work; the safe default is to reject unsupported transitions rather than infer or auto-migrate data.
- Provider-scope documentation drift is already visible because one older planning contract predates the current DB2 baseline; final implementation documentation needs to align on the six-profile repository baseline to avoid conflicting handoff guidance.

Split recommendations
- If delivery scope needs to shrink, split provider profile plus EF annotation/storage-profile work from migration/live-schema/explain guardrail work while keeping this ticket as the contract parent.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment