[gicket-bot] PO refinement contract

Summary
- Verified the persisted ticket refinement now answers both PO-critic items: same-length stable-hash `algorithmId` drift (`sha1-v1` versus `sha256-160-v1`) is explicitly fail-closed, and the reviewed support-bundle preflight baseline is named as the authoritative comparison surface. The description update was already persisted before this verification turn; no new bounded writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now states that changing the stable-hash `algorithmId` is a blocking persisted-contract change even when digest length and provider store type stay the same. A `sha1-v1` to `sha256-160-v1` switch must be rejected fail-closed until the caller performs an explicit reviewed migration or reset outside this ticket.
- critic-item-2: `answered` - The authoritative comparison surface is the reviewed `dvault.support-bundle.v1` artifact used as the preflight baseline. Guardrails must compare `algorithmId` together with digest length, storage profile, provider store type, and logical property kind rather than inferring compatibility from width or store type alone.

Clarifications
- Persisted ticket description revision `06FBCX58QQPZQY6G5W84MV487G` already contains the amended Delivery Contract required by the PO-critic return; this turn verified the persisted text against repository evidence and relation state.
- Repository evidence confirms `sha1-v1` and `sha256-160-v1` are distinct built-in stable-hash ids that share the same 20-byte / 40-hex width, so digest length alone is not a safe compatibility key.
- Relation context remains consistent: this ticket is a child of `06F9GF5A8V7G3PAKGRXNYEBW5C` and blocks `06F9GF5N4N3Q685XQPKTM5EC00`.
- The visible provider baseline remains six built-in profiles: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`.

Scope In
- Define the model-level hash-key storage profile contract that separates logical lowercase-hex hash keys from physical provider storage.
- Cover DVault-owned hash-key columns and hash-key-reference columns across hubs, links, satellites, PITs, and bridges.
- Define machine-readable EF metadata, diagnostics, support-bundle, provider-capability, and guardrail facts for storage profile, `algorithmId`, and digest length.
- Define compatibility behavior for the current built-in stable-hash ids, including same-length drift cases such as `sha1-v1` versus `sha256-160-v1`.

Scope Out
- Changing public or logical hash-key surfaces from canonical lowercase hex strings to `byte[]` or provider-specific runtime types.
- Automatic rehashing, dual-write, repair, backfill, or migration tooling for callers who intentionally change persisted algorithm or storage profile.
- Provider-side SQL hashing or changes to the separate `content_hash` contract.
- Implementing DB2 live-schema reading under this ticket.

Open questions
- none

Follow-up questions
- Should a later ticket extend the same storage-profile abstraction to caller-supplied HashDiff columns after the hash-key contract lands?
- Should a later ticket add reviewed migration tooling or data-move guidance for consumers who deliberately convert persisted HexString columns to Binary outside the fail-closed default posture?
- Should a later ticket bring DB2 live-schema reading to parity so runtime drift checks can validate hash-key storage shape against the catalog?

Risks
- This ticket still blocks `06F9GF5N4N3Q685XQPKTM5EC00`, so delayed contract landing continues to hold downstream work.
- If storage-profile facts are not applied consistently across keys and hash-key references, joins, indexes, and cross-table comparisons can drift even when logical hash values still appear valid at the API boundary.
- Changing `algorithmId` or converting persisted text columns to binary remains caller-owned compatibility work; the safe default is rejection rather than inferred migration.
- Provider-scope documentation drift remains possible because older planning text predates the current six-profile baseline.

Split recommendations
- If delivery scope must shrink, split provider-profile plus EF-annotation/storage-profile work from migration, live-schema, and explain/preflight guardrail work while keeping this ticket as the contract parent.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment