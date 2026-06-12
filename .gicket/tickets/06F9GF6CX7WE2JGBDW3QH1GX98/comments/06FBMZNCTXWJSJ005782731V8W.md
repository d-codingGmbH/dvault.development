[gicket-bot] PO refinement contract

Summary
- Refined the ticket against the current repo state: v0.35.0 remains the live docs baseline, the hash-key storage contract and benchmark evidence tickets are done, and this task is bounded to moving user-facing docs to the v0.36.0 / 8.36.0 / 10.36.0 guidance set; no persistent writes were applied in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- README.md, docs/production-adoption-checklist.md, and docs/manual-nuget-publication.md still describe v0.35.0 with package lines 8.35.0 / 10.35.0, and the repository has no v0.36.0 release note yet.
- Done story 06F9GF5FV54DGWY9GA8ZEZWM5R already fixes the architecture baseline: logical hash-key values stay canonical lowercase hexadecimal strings, HexString is the compatible default, Binary is explicit opt-in, and there is no automatic migration, backfill, dual-write, or repair support.
- Done benchmark task 06F9GF66B10J4K7RBDTJ9NQRQC already supplies the documentation evidence bundle under artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-<redacted>/.
- The checked-in footprint sidecar already gives bounded storage deltas for the SQLite baseline: sha256-v1 HexString uses a 64-byte hash-key payload and a 128-byte two-column hash-reference index payload versus Binary at 32 and 64 bytes, and sha256-128-v1 HexString uses 32 and 64 bytes versus Binary at 16 and 32 bytes.
- The visible provider-store baseline is finite and should be documented instead of reopened: SQLite TEXT/BLOB, Oracle VARCHAR2(n CHAR)/RAW(n), PostgreSQL varchar(n)/bytea, SQL Server nvarchar(n)/varbinary(n), DB2 VARCHAR(n)/VARBINARY(n), and MySQL varchar(n)/varbinary(n), with widths derived from the active digest length.
- Diagnostics and support-bundle surfaces already expose stable-hash algorithmId, digestByteLength, digestEncoding, hashKeyStorageProfile, value format, and conversion behavior without raw hash values.
- Live relation context is adequate for refinement: this ticket remains a child of epic 06F9GF5A8V7G3PAKGRXNYEBW5C, and the completed benchmark task can be treated as landed evidence rather than as an open PO blocker.
- No human scope comments, attachments, child-ticket writes, relation edits, description updates, or planning documents were materialized during this refinement run.

Scope In
- Advance the current documentation baseline from v0.35.0 to v0.36.0 and from package lines 8.35.0 / 10.35.0 to 8.36.0 / 10.36.0 across README, production adoption guidance, package verification or publication notes, and the new release note.
- Document default HexString compatibility and Binary opt-in storage guidance in adopter-facing documentation.
- Document the finite provider column-type matrix for HashKey and ParticipantReference storage, with widths tied to the active digest length.
- Document the no-automatic-migration posture and consumer-owned compatibility responsibility when changing algorithm or storage profile after persistence.
- Document benchmark-backed storage-footprint and lookup or read evidence using the landed SQLite-local artifact bundle.
- Document how diagnostics and support bundles expose algorithm and storage choices without exposing raw hash values.

Scope Out
- New runtime or provider implementation work for hash-key storage, diagnostics, live-schema, or benchmarks.
- Automatic migration, backfill, rehash, dual-write, repair tooling, or prescriptive data-move automation.
- Changing public hash-key surfaces from canonical lowercase hexadecimal strings to byte[] or provider-native types.
- New provider capability claims such as DB2 live-schema support or non-SQLite measured wins that the repository does not already prove.
- Package publication, release approval, or manual package push execution.
- HashDiff or content-hash contract changes or a new benchmark-harness design.

Open questions
- none

Follow-up questions
- Should a later follow-up collect and publish provider-specific empirical evidence beyond the required SQLite-local baseline before broader storage or lookup claims are made?
- Should a later release add a consumer migration cookbook for deliberate HexString-to-Binary conversions once the no-automatic-migration baseline is published?

Risks
- If the current-baseline version move is applied inconsistently, README, adoption, package-verification, and release-note guidance will contradict each other.
- If the docs present the checked-in benchmark numbers as provider-general guarantees, they will overstate a repository baseline that is currently SQLite-local.
- If provider column types or redacted diagnostics facts are summarized too loosely, consumers may mis-size columns or misunderstand what support bundles and explain output actually expose.

Split recommendations
- No split is recommended while the task stays bounded to refreshing existing user-facing documentation surfaces that already have landed architecture and benchmark inputs.
- If stakeholders want new provider-specific benchmark runs or a detailed migration playbook in the same release, create follow-up tickets instead of widening this task.

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