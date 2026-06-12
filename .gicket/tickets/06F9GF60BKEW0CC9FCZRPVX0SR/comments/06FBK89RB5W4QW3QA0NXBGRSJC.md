[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded test-gap ticket: Binary hash-key storage already has unit-level mapping/converter coverage, but the repository still lacks executable schema/save/read coverage for storage-profile behavior. No child tickets, relation edits, description updates, attachments, or planning documents were materialized during this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows Binary hash-key coverage exists today only in unit-level provider-mapping, metadata-translation, and converter tests; no integration test under tests/DCoding.Data.DVault.Tests/Integration opts into HashKeyStorageProfile.Binary yet.
- The bounded v1 provider baseline for this ticket is the existing six built-in profiles: sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1, with HexString as the default and Binary as explicit opt-in.
- Save and read APIs stay on lowercase hexadecimal string values even when the physical store type is binary; this ticket is proving storage compatibility, not changing caller-facing hash-key types.
- DB2 live-schema reading remains explicitly unsupported under the current contract and should stay a negative unsupported-provider assertion rather than becoming a new positive execution target here.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized; the existing parentOf relation from 06F9GF5A8V7G3PAKGRXNYEBW5C and the existing blocks relation to 06F9GF66B10J4K7RBDTJ9NQRQC were left unchanged.

Scope In
- Add executable SQLite coverage for both HexString and Binary hash-key storage so the suite proves text-backed compatibility and binary-backed persistence with the same caller-facing string boundary.
- Add schema-generation or live-schema-fixture assertions that hash-key and participant-reference columns on hubs, links, satellites, PITs, and bridges size from the active stable-hash digest for both storage profiles.
- Add save and read round-trip coverage where hash keys participate in latest/current, explicit as-of, PIT as-of, and bridge traversal requests under a Binary profile.
- Add the remaining missing negative tests at the schema/save/read boundary for incompatible digest length or storage-profile facts, while reusing existing converter and migration-guardrail unit coverage instead of duplicating it.

Scope Out
- New hash-key storage profiles beyond HexString and Binary.
- Changes to public hash-key value types, stable-hash algorithm inventory, or provider-specific SQL behavior.
- Automatic rehash, repair, migration/backfill, or dual-write behavior.
- New DB2 live-schema support or mandatory external-database execution coverage for every provider.

Open questions
- none

Follow-up questions
- Should a later ticket add optional external-provider Binary smoke execution for PostgreSQL or SQL Server, or is SQLite execution plus provider-matrix contract coverage sufficient for v1?
- Should Binary-profile support-bundle and diagnostics export matrices get their own explicit ticket, or remain indirectly covered by provider-mapping and migration-drift tests?

Risks
- If implementation stops at provider-mapping unit assertions and skips an executable Binary save/read round-trip, EF conversion or query-translation regressions can still slip through.
- PIT and bridge coverage depend on explicitly seeded maintained tables in the test harness; partial read-path coverage could leave a false impression that Binary support is complete.
- Requiring every external provider to execute Binary round-trips in this ticket would couple a test-only task to optional database infrastructure and likely expand it beyond the current bounded scope.
- Scheduling still depends on the existing incoming blocks relation from 06F9GF5TNAXBCKN5BD9CKD7WVG; refinement did not change dependency state.

Split recommendations
- No split recommended while the work stays within existing test and shared-fixture surfaces.
- If later stakeholders want live Binary execution across optional external providers, spin that into a follow-up ticket instead of broadening this test-coverage ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment