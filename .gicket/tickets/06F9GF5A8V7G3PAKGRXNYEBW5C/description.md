<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No epic description update, new attachment, or new planning document was needed because the existing epic description and checked-in contract/docs already match the authoritative scope.

### Scope In
- Ratify the bounded v1 hash-key storage profile contract for DVault-owned hash-key and participant-reference columns: compatible `HexString` default plus explicit opt-in `Binary` physical storage.
- Preserve canonical lowercase hexadecimal string semantics at API, EF model, diagnostics, and support-bundle boundaries while allowing provider-specific physical storage optimization underneath.
- Carry storage profile, provider store type, provider value format, stable-hash `algorithmId`, `digestByteLength`, digest encoding, and conversion behavior through provider metadata, diagnostics, preflight, and drift checks.
- Cover the visible built-in provider baseline `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`, plus benchmark and adoption guidance.

### Scope Out
- Automatic migration tooling, backfill, dual-write, repair, reconcile, or rehash behavior for persisted hash keys.
- Provider-side SQL hashing or any change to the caller-facing/public hash-key value type away from canonical lowercase hexadecimal `string` values.
- Changes to HashDiff/content-hash behavior or unrelated read/save architecture work.
- DB2 live-schema support beyond continuing to return an explicit unsupported-provider outcome.

## Acceptance Criteria
- DVault exposes a bounded storage-profile vocabulary where `HexString` remains the compatible default and `Binary` is explicit opt-in for DVault-owned hash-key and hash-key-reference columns.
- For both storage profiles, callers and EF model CLR properties still supply, receive, compare, inspect, and diagnose hash keys as canonical lowercase hexadecimal strings without prefixes.
- Built-in provider capability profiles and translated metadata expose storage profile, provider store type, provider value format, active stable-hash `algorithmId`, declared `digestByteLength`, digest encoding, and conversion behavior for every hash-key and participant-reference column.
- Provider mappings and conversions size both `HexString` and `Binary` storage by the active digest length across SQLite, Oracle, PostgreSQL, SQL Server, DB2, and Pomelo MySQL, with provider-appropriate native store types.
- Migration, preflight, and live-schema guardrails fail closed on persisted hash-key compatibility changes such as storage profile, algorithm id, digest byte length, provider store type, provider value format, or conversion behavior; DB2 live-schema reading remains explicitly unsupported instead of silently passing.
- Consumer-facing documentation explains benchmark/footprint evidence, supported provider profiles, and that any post-persistence storage-profile or algorithm change is caller-owned migration work.

## Definition of Done
- The epic's existing child split is complete for contract definition, provider-neutral conversion, provider-specific mappings, schema/read tests, benchmark evidence, and adoption documentation.
- Repository source and tests prove the bounded built-in stable-hash algorithm ids and the persistence-only binary conversion path without changing the public hash-key string boundary.
- Checked-in documentation surfaces cover the stable hashing contract, hash-key storage profile contract, footprint/benchmark evidence, and consumer adoption guidance.

## Implementation Notes
- Use the existing `DataVaultAnnotationNames` surface as the authoritative metadata channel for hash-key storage profile, provider value format, stable-hash algorithm id, digest byte length, digest encoding, and conversion behavior.
- The built-in stable-hash baseline visible in repository code is finite and versioned: `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- Provider capability profile code and unit tests already establish the v1 provider matrix: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`, with `Binary` mapping to `BLOB`, `RAW(n)`, `bytea`, or `varbinary(n)` as appropriate.
- Binary storage is persistence-only; `DataVaultHashKeyProviderValueConverter` converts canonical lowercase hex strings to bytes and back using the declared digest byte length and rejects non-canonical values.
- No further child-ticket split, attachment write, or planning-document write is justified from current evidence.

## Open Questions
- none

## Follow-Up Questions
- After queued replay on the owner branch for ticket 06F9GF6CX7WE2JGBDW3QH1GX98, confirm the stale incoming `blocks` relation no longer appears on epic 06F9GF5A8V7G3PAKGRXNYEBW5C.

## Risks
- Adopters who change storage profile or stable-hash algorithm after data is persisted still own migration planning; the product intentionally fails closed instead of automating that transition.

## Split Recommendations
- No further split is recommended; the epic already has a complete six-child decomposition covering contract, conversion, provider mappings, tests, benchmarking, and adoption guidance.
- Any future expansion beyond the bounded `HexString`/`Binary` v1 vocabulary or any future DB2 live-schema support should be tracked as separate follow-up tickets rather than reopening this epic.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add efficient physical storage profiles for DVault hash keys without turning DVault into a migration tool. Keep canonical hash semantics stable, preserve current hex string behavior as a compatible default, add opt-in binary storage for supported providers, and document consumer-owned migration responsibilities.