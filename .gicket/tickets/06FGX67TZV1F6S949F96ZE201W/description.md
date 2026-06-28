<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded v1 HexString-to-Binary manifest validation contract aligned with the migration guide and hash-key storage profile contract; no child tickets, relation changes, description updates, or planning documents were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ratify v1 as an adopter-owned storage-only migration manifest for existing persisted DVault model boundaries from HexString to Binary; it is not a generic profile-conversion framework.
- Ratify the visible built-in provider baseline for v1 manifests as sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1.
- Ratify the visible built-in stable-hash baseline for v1 manifests as sha256-v1 (32 bytes), sha1-v1 (20 bytes), sha256-128-v1 (16 bytes), and sha256-160-v1 (20 bytes), with digest encoding fixed to lowercase-hex-no-prefix.
- The existing blocks relation to 06FGX69QJYHGNKBV8MJ1HG7MMG remains consistent with this ticket defining the contract consumed by downstream implementation; no relation, attachment, or description writes were applied in this refinement pass.

### Scope In
- Define the required v1 manifest facts for the selected migration boundary: schema/version identity, reviewed source evidence provenance, provider profile, model-level algorithm and digest facts, and deterministic before/after storage-profile expectations.
- Define complete per-property coverage requirements for every DVault-owned HashKey and ParticipantReference column in scope across hubs, links, satellites, PITs, and bridges.
- Define the validation-finding contract, including deterministic error, warning, and info outputs and the fail-closed rules that block unsafe manifests before any migration step.
- Align the manifest contract vocabulary and guardrails with docs/hash-key-storage-migration.md and docs/plans/hash-key-storage-profile-contract.md.

### Scope Out
- Executing migrations, generating schema/data conversion SQL, or performing cutover, repair, backfill, reconciliation, dual-write, or rehash work.
- Changing stable-hash algorithm, digest length, truncation policy, digest encoding, public API hash-key shape, or HashDiff/content-hash behavior.
- Generic rollback or alternate profile flows such as Binary to HexString, same-profile audit manifests, or custom provider-profile expansion beyond the visible built-in baseline.

## Acceptance Criteria
- The contract defines the mandatory manifest sections and required fields for v1, including schema/version id, selected model boundary, provider profile id, reviewed source evidence provenance, model-level algorithmId, digestByteLength, digestEncoding, and explicit expected source/target storage profiles.
- The contract defines the per-column facts that must be present for every in-scope DVault-owned HashKey and ParticipantReference: logical property kind, table name, column name, source and target storage profile, provider store type, provider value format, EF CLR model type, conversion behavior, algorithmId, digestByteLength, and digest encoding.
- Validation fails with error findings for missing required fields, missing or duplicate in-scope coverage, mixed or ambiguous source/target profiles within the selected boundary, unsupported provider/profile values, algorithm or digest drift, encoding drift, or compatibility decisions based only on width/store-type matches.
- The finding contract distinguishes error, warning, and info, where warning is reserved for non-blocking evidence gaps such as unavailable supplemental live-schema checks and info summarizes recognized baseline facts and coverage totals; finding production and ordering are deterministic for the same manifest input.
- The contract states that reviewed dvault.support-bundle.v1 or equivalent translated EF metadata is the authoritative preflight baseline, live-schema evidence is supplemental where provider support exists, and validation never attempts migration execution when the manifest is invalid or ambiguous.

## Definition of Done
- The ticket handoff leaves no v1 architecture ambiguity about the allowed storage profiles, built-in provider/profile baseline, or built-in stable-hash sizing baseline.
- Downstream delivery updates the contract/documentation surface using the same terminology as the migration guide and hash-key storage profile contract, without introducing conflicting profile or algorithm vocabulary.
- Downstream delivery includes a bounded positive/negative validation matrix or equivalent tests covering complete coverage success, missing coverage, mixed-profile rejection, algorithm/digest drift, and the sha1-v1 versus sha256-160-v1 same-size incompatibility case.
- The delivered validation contract clearly separates blocking errors from non-blocking warnings/info and preserves a deterministic output shape suitable for diagnostics and automation.

## Implementation Notes
- Use the reviewed support bundle or equivalent translated EF metadata as the authoritative inventory of in-scope DVault-owned hash-key and hash-key-reference properties; treat provider-specific live-schema facts as additive evidence instead of the sole source of truth.
- Treat the migration boundary as a complete selected model boundary, not an arbitrary partial table subset: every in-scope hub, link, satellite, PIT, and bridge HashKey/ParticipantReference column must appear exactly once in the manifest coverage set.
- For v1, ratify the storage-only path as HexString source to Binary target while keeping caller-facing hash-key values and EF model CLR exposure as lowercase hexadecimal strings.
- Do not accept store type, column width, or payload size alone as proof of compatibility; the manifest validator must compare the full persisted compatibility fact set because equal lengths can hide incompatible algorithms.
- Keep the existing blocks dependency on 06FGX69QJYHGNKBV8MJ1HG7MMG; that downstream implementation ticket should consume this contract rather than reopen the baseline decisions above.

## Open Questions
- none

## Follow-Up Questions
- After v1 ships, should a later ticket add contract support for custom provider-profile identifiers beyond the current built-in baseline?
- After the forward migration path is stable, do we need a separate ticket for rollback/readiness audit manifests such as Binary to HexString or same-profile verification runs?
- Should DB2 live-schema evidence parity stay as optional supplemental validation only, or does it need a dedicated follow-up ticket once the broader DB2 catalog-reader roadmap is clearer?

## Risks
- If the contract does not pin deterministic finding ordering and classification, downstream diagnostics and tests can become flaky even when the manifest contents are identical.
- Coverage validation depends on complete authoritative inventory data; incomplete support-bundle or translated-metadata capture could underreport PIT, bridge, or participant-reference columns.
- Provider-specific store-type aliases and equivalent persisted-shape normalization can create false mismatches or false passes if the validator does not compare the full fact set and fail closed on ambiguity.

## Split Recommendations
- No split recommended; the ticket is already bounded to the v1 manifest validation contract and the existing downstream implementation dependency.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the deterministic validation contract for dvault.hash-key-storage-migration.v1 manifests.

Acceptance:
- Required fields, provider/profile facts, digest sizes, table/column coverage, and expected before/after storage profiles are specified.
- Validation distinguishes errors, warnings, and informational findings.
- The contract rejects ambiguous mixed-profile or missing coverage cases without attempting migration execution.
- The contract aligns with the existing Hash-Key Storage Migration Guide.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Implemented the v1 hash-key storage migration manifest validation contract in the repository documentation.
- Kept the contract bounded to adopter-owned `HexString` to `Binary` storage-profile migration for the selected DVault model boundary.

### Repository Artifacts
- `docs/hash-key-storage-migration.md`
- `docs/plans/hash-key-storage-profile-contract.md`

### Verification
- `git diff --check -- docs/hash-key-storage-migration.md docs/plans/hash-key-storage-profile-contract.md` passed.
- `bash tools/check-format.sh` passed.

### Notes
- No source or test code was changed; the downstream implementation ticket can consume this contract for executable validator behavior.
<!-- gicket-bot:developer-delivery:v1:end -->