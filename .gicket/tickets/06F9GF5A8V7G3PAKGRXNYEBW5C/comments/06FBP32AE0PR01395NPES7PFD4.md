[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F9GF5A8V7G3PAKGRXNYEBW5C/description.md` contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` = `none`.
- Checked-in contract and adoption surfaces exist at `docs/plans/hash-key-storage-profile-contract.md`, `docs/plans/stable-hashing-contract.md`, `hash-key-footprint.md`, `docs/production-adoption-checklist.md`, and benchmark artifact bundle `artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-<redacted>/`.
- `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs`, `src/DCoding.Data.DVault/BuiltInStableHashService.cs`, and `src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs` directly match the ticket contract: annotation keys exist for storage profile/algorithm/digest/conversion, built-in algorithm ids are bounded to `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`, and binary conversion keeps canonical lowercase hex at the model boundary.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs` covers all six built-in provider profiles and binary store types (`BLOB`, `RAW(20)`, `bytea`, `varbinary(20)`, `VARBINARY(20)`, `varbinary(20)`), and `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` routes DB2 through `UnsupportedDataVaultLiveSchemaReader`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Approval assumes the stale relation file `.gicket/relations/98/5C/06F9GF6CX7WE2JGBDW3QH1GX98--06F9GF5A8V7G3PAKGRXNYEBW5C--blocks.json` remains historical/non-blocking because the related task `06F9GF6CX7WE2JGBDW3QH1GX98` is `done`, its `ticket.json` says `is-blocked=false`, and the epic `ticket.json` also says `is-blocked=false`.
- Benchmark and footprint evidence is intentionally SQLite-only; downstream consumers must not generalize those storage/performance numbers to other providers unless a provider-specific checked-in bundle is added.

AC / test suggestions
- Keep downstream validation explicitly checking that support-bundle and diagnostics surfaces expose `hashKeyStorageProfile`, provider store type, provider value format, `algorithmId`, `digestByteLength`, `digestEncoding`, and conversion behavior together.
- Keep benchmark-related acceptance language scoped to the checked-in SQLite bundle and its linked sidecars unless more provider bundles are added later.

Implementation watchouts
- Binary storage must remain persistence-only; public/request/EF-model/diagnostic boundaries must stay canonical lowercase hexadecimal `string` values.
- Storage-profile or stable-hash changes after persistence must stay fail-closed and caller-owned rather than implying automatic migration, backfill, dual-write, repair, or rehash behavior.

Non-blocking notes
- The contract's only follow-up is relation-cleanup confirmation for the stale incoming `blocks` record; that is tracking hygiene, not a PO-scope ambiguity.

Split recommendations
- No further split is recommended; the epic already has a complete six-child decomposition covering contract, conversion, provider mappings, tests, benchmarking, and adoption guidance.
- Any future expansion beyond bounded `HexString`/`Binary` v1 storage profiles or any future DB2 live-schema support should be tracked as separate follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment