<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket against the current repo state: v0.35.0 remains the live docs baseline, the hash-key storage contract and benchmark evidence tickets are done, and this task is bounded to moving user-facing docs to the v0.36.0 / 8.36.0 / 10.36.0 guidance set; no persistent writes were applied in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- README.md, docs/production-adoption-checklist.md, and docs/manual-nuget-publication.md still describe v0.35.0 with package lines 8.35.0 / 10.35.0, and the repository has no v0.36.0 release note yet.
- Done story 06F9GF5FV54DGWY9GA8ZEZWM5R already fixes the architecture baseline: logical hash-key values stay canonical lowercase hexadecimal strings, HexString is the compatible default, Binary is explicit opt-in, and there is no automatic migration, backfill, dual-write, or repair support.
- Done benchmark task 06F9GF66B10J4K7RBDTJ9NQRQC already supplies the documentation evidence bundle under artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/.
- The checked-in footprint sidecar already gives bounded storage deltas for the SQLite baseline: sha256-v1 HexString uses a 64-byte hash-key payload and a 128-byte two-column hash-reference index payload versus Binary at 32 and 64 bytes, and sha256-128-v1 HexString uses 32 and 64 bytes versus Binary at 16 and 32 bytes.
- The visible provider-store baseline is finite and should be documented instead of reopened: SQLite TEXT/BLOB, Oracle VARCHAR2(n CHAR)/RAW(n), PostgreSQL varchar(n)/bytea, SQL Server nvarchar(n)/varbinary(n), DB2 VARCHAR(n)/VARBINARY(n), and MySQL varchar(n)/varbinary(n), with widths derived from the active digest length.
- Diagnostics and support-bundle surfaces already expose stable-hash algorithmId, digestByteLength, digestEncoding, hashKeyStorageProfile, value format, and conversion behavior without raw hash values.
- Live relation context is adequate for refinement: this ticket remains a child of epic 06F9GF5A8V7G3PAKGRXNYEBW5C, and the completed benchmark task can be treated as landed evidence rather than as an open PO blocker.
- No human scope comments, attachments, child-ticket writes, relation edits, description updates, or planning documents were materialized during this refinement run.

### Scope In
- Advance the current documentation baseline from v0.35.0 to v0.36.0 and from package lines 8.35.0 / 10.35.0 to 8.36.0 / 10.36.0 across README, production adoption guidance, package verification or publication notes, and the new release note.
- Document default HexString compatibility and Binary opt-in storage guidance in adopter-facing documentation.
- Document the finite provider column-type matrix for HashKey and ParticipantReference storage, with widths tied to the active digest length.
- Document the no-automatic-migration posture and consumer-owned compatibility responsibility when changing algorithm or storage profile after persistence.
- Document benchmark-backed storage-footprint and lookup or read evidence using the landed SQLite-local artifact bundle.
- Document how diagnostics and support bundles expose algorithm and storage choices without exposing raw hash values.

### Scope Out
- New runtime or provider implementation work for hash-key storage, diagnostics, live-schema, or benchmarks.
- Automatic migration, backfill, rehash, dual-write, repair tooling, or prescriptive data-move automation.
- Changing public hash-key surfaces from canonical lowercase hexadecimal strings to byte[] or provider-native types.
- New provider capability claims such as DB2 live-schema support or non-SQLite measured wins that the repository does not already prove.
- Package publication, release approval, or manual package push execution.
- HashDiff or content-hash contract changes or a new benchmark-harness design.

## Acceptance Criteria
- README current-baseline guidance is advanced from v0.35.0 / 8.35.0 / 10.35.0 to v0.36.0 / 8.36.0 / 10.36.0, and the docs explicitly keep v0.36.0 as the planning or release-note label rather than a consumer NuGet version.
- User-facing docs state that logical hash keys remain lowercase hexadecimal strings, HexString remains the default compatible storage profile, Binary is explicit opt-in only, and DVault does not automatically migrate, backfill, dual-write, or repair persisted keys.
- The updated documentation includes the bounded built-in provider column-type guidance for HashKey and ParticipantReference storage, with widths derived from the active digest length and DB2 live-schema support still documented as unsupported.
- Production adoption and release guidance cite the landed SQLite benchmark bundle and footprint sidecars for storage-footprint and lookup or read tradeoff evidence, and keep any performance claims scoped to that checked-in evidence.
- Diagnostics and support-bundle guidance explains how algorithm and storage choices are surfaced through algorithmId, digestByteLength, digestEncoding, hashKeyStorageProfile, and store-type, value-format, or conversion facts while preserving the redacted output boundary.
- Package verification and manual-publication guidance is refreshed so dual package-line install examples, README validation language, and hash-storage documentation all align with the v0.36.0 baseline.

## Definition of Done
- README, docs/production-adoption-checklist.md, docs/manual-nuget-publication.md, and docs/releases/v0.36.0.md tell one consistent v0.36.0 story without leaving current-baseline references on 0.35 / 8.35 / 10.35.
- Historical v0.35.0 material remains historical; only the current baseline and release-line guidance move forward.
- Documentation links point to the existing hash-key storage contract, stable-hashing contract, benchmark-summary triplet, and hash-key-footprint sidecars instead of duplicating unsupported implementation detail.
- No documentation claim implies new runtime behavior, package publication, automatic migration, DB2 live-schema support, or cross-provider measured wins that the repository does not currently prove.
- The implementation can be completed as a documentation-only change set without further PO decisions.

## Implementation Notes
- Use done story 06F9GF5FV54DGWY9GA8ZEZWM5R as the authoritative contract input and do not reopen the logical string boundary, default HexString baseline, Binary opt-in posture, or no-automatic-migration rule.
- Use the checked-in benchmark evidence from artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/, especially benchmark-summary.md and hash-key-footprint.md, as the measured basis for storage-savings and lookup or read guidance.
- Carry forward the existing release-note pattern: v0.36.0 is the planning or release-note label, while 8.36.0 and 10.36.0 are the consumer package lines.
- Document provider column types from the existing finite matrix and describe widths as active-digest-length dependent instead of hard-coding only the 64-character sha256-v1 case.
- Keep benchmark language bounded to the required SQLite local baseline; optional PostgreSQL, SQL Server, Oracle, MySQL, and DB2 evidence should be mentioned only as future or separate verification unless the repo later checks in matching measured artifacts.
- When explaining diagnostics or support-bundle behavior, preserve the current redaction boundary: metadata facts are visible, but raw hash keys, business keys, and diagnostic payload text are not.
- Manual publication and package-verification wording should stay publication-separate: the docs baseline can move to v0.36.0 without implying that NuGet publication has already happened.

## Open Questions
- none

## Follow-Up Questions
- Should a later follow-up collect and publish provider-specific empirical evidence beyond the required SQLite-local baseline before broader storage or lookup claims are made?
- Should a later release add a consumer migration cookbook for deliberate HexString-to-Binary conversions once the no-automatic-migration baseline is published?

## Risks
- If the current-baseline version move is applied inconsistently, README, adoption, package-verification, and release-note guidance will contradict each other.
- If the docs present the checked-in benchmark numbers as provider-general guarantees, they will overstate a repository baseline that is currently SQLite-local.
- If provider column types or redacted diagnostics facts are summarized too loosely, consumers may mis-size columns or misunderstand what support bundles and explain output actually expose.

## Split Recommendations
- No split is recommended while the task stays bounded to refreshing existing user-facing documentation surfaces that already have landed architecture and benchmark inputs.
- If stakeholders want new provider-specific benchmark runs or a detailed migration playbook in the same release, create follow-up tickets instead of widening this task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update README, provider docs, production adoption guidance, package verification notes, and release notes for planning release v0.36.0 and package outputs 8.36.0/10.36.0. Document default hex compatibility, binary storage opt-in, provider column types, storage savings, no automatic migration support, benchmark evidence, and how diagnostics expose algorithm/storage choices.