[gicket-bot] PO-critic review contract

Summary
- Delivery contract is concrete, repository-backed, and has no open questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9GF6CX7WE2JGBDW3QH1GX98/description.md contains PO Handoff decision ready_for_po_critic and an Open Questions section with 'none'.
- artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-<redacted>/hash-key-footprint.md records the cited SQLite deltas: sha256-v1 HexString 64-byte hash-key payload and 128-byte two-column index payload versus Binary 32 and 64 bytes; sha256-128-v1 HexString 32 and 64 bytes versus Binary 16 and 32 bytes.
- README.md, docs/production-adoption-checklist.md, and docs/manual-nuget-publication.md still reference the current v0.35.0 baseline and 8.35.0 / 10.35.0 package lines, matching the ticket's stated documentation gap.
- test -f docs/releases/v0.36.0.md returned exit code 1, so the release-note file required by the Definition of Done does not exist yet.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs derives provider store types from digest length and storage profile: SQLite TEXT/BLOB, Oracle VARCHAR2(n CHAR)/RAW(n), PostgreSQL varchar(n)/bytea, SQL Server nvarchar(n)/varbinary(n), DB2 VARCHAR(n)/VARBINARY(n), and MySQL varchar(n)/varbinary(n).
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs asserts support-bundle diagnostics include algorithmId, digestByteLength, digestEncoding, hashKeyStorageProfile, stableHashAlgorithmId, and conversionBehavior, and exclude raw digest values and secret business keys.
- git log shows the branch tip is the po-critic claim commit 6ab082a03, and git diff --name-only develop..HEAD lists only .gicket/tickets/06F9GF6CX7WE2JGBDW3QH1GX98/... files, which is consistent with a pre-development documentation ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Include at least one digest-width-derived example beyond sha256-v1 so readers do not infer that only 64-character hash keys are supported.
- Make the release note and README explicitly contrast the planning label v0.36.0 with consumer package lines 8.36.0 and 10.36.0.
- When documenting diagnostics/support-bundle behavior, say both what remains visible and what stays redacted.

Risky assumptions
- Readers may overgeneralize the checked-in benchmark evidence unless every performance or storage claim is explicitly scoped to the SQLite-local artifact bundle.
- Readers may mistake v0.36.0 for a consumer NuGet version unless the docs repeat that it is a planning/release-note label only.
- A provider matrix summary that omits digest-length derivation could be read as fixed-width guidance instead of algorithm-dependent sizing.

AC / test suggestions
- Add a doc-review check that README.md, docs/production-adoption-checklist.md, docs/manual-nuget-publication.md, and docs/releases/v0.36.0.md have no leftover current-baseline v0.35.0 / 8.35.0 / 10.35.0 references except where explicitly historical.
- Add a validation check that Binary wording never implies public byte[] hash-key APIs or automatic migration, backfill, dual-write, or repair behavior.
- Add a validation check that provider claims stay inside the repository-proven matrix and that DB2 live-schema remains documented as unsupported.

Implementation watchouts
- Do not broaden benchmark claims beyond the checked-in SQLite artifact bundle; the measured bundle explicitly shows PostgreSQL skipped.
- Do not imply package publication has already happened when moving docs to the v0.36.0 baseline; manual publication remains separate.
- Do not change the logical hash-key boundary in docs; public values remain lowercase hexadecimal strings and Binary is a persistence-only opt-in profile.
- Keep provider widths described as active-digest-length derived rather than as hard-coded sha256-v1-only widths.

Non-blocking notes
- The relation event .gicket/tickets/06F9GF6CX7WE2JGBDW3QH1GX98/events/06F9GF7AW1ZVZ1TNSV9T3V2GER.json shows this ticket blocks epic 06F9GF5A8V7G3PAKGRXNYEBW5C; that is tracking context, not a PO blocker.
- The ticket has no assignees in .gicket/tickets/06F9GF6CX7WE2JGBDW3QH1GX98/ticket.json, which is acceptable for developer handoff under the current automation flow.

Split recommendations
- No split is recommended while the work stays documentation-only and uses the already-landed contract and benchmark artifacts.
- If stakeholders want provider-specific empirical evidence or a migration cookbook in the same release, open follow-up tickets instead of widening this task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment