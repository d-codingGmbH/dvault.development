[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- No epic description update, new attachment, or new planning document was needed because the existing epic description and checked-in contract/docs already match the authoritative scope.

Scope In
- Ratify the bounded v1 hash-key storage profile contract for DVault-owned hash-key and participant-reference columns: compatible `HexString` default plus explicit opt-in `Binary` physical storage.
- Preserve canonical lowercase hexadecimal string semantics at API, EF model, diagnostics, and support-bundle boundaries while allowing provider-specific physical storage optimization underneath.
- Carry storage profile, provider store type, provider value format, stable-hash `algorithmId`, `digestByteLength`, digest encoding, and conversion behavior through provider metadata, diagnostics, preflight, and drift checks.
- Cover the visible built-in provider baseline `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`, plus benchmark and adoption guidance.

Scope Out
- Automatic migration tooling, backfill, dual-write, repair, reconcile, or rehash behavior for persisted hash keys.
- Provider-side SQL hashing or any change to the caller-facing/public hash-key value type away from canonical lowercase hexadecimal `string` values.
- Changes to HashDiff/content-hash behavior or unrelated read/save architecture work.
- DB2 live-schema support beyond continuing to return an explicit unsupported-provider outcome.

Open questions
- none

Follow-up questions
- After queued replay on the owner branch for ticket 06F9GF6CX7WE2JGBDW3QH1GX98, confirm the stale incoming `blocks` relation no longer appears on epic 06F9GF5A8V7G3PAKGRXNYEBW5C.

Risks
- Adopters who change storage profile or stable-hash algorithm after data is persisted still own migration planning; the product intentionally fails closed instead of automating that transition.

Split recommendations
- No further split is recommended; the epic already has a complete six-child decomposition covering contract, conversion, provider mappings, tests, benchmarking, and adoption guidance.
- Any future expansion beyond the bounded `HexString`/`Binary` v1 vocabulary or any future DB2 live-schema support should be tracked as separate follow-up tickets rather than reopening this epic.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment