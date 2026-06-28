<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Reconciled this ticket to consume the current dry-run dvault.hash-key-storage-migration.v1 shape, scoped producer-schema changes out of the task, defined inline invalid-manifest fixture sourcing, and refreshed dependency wording so 06FGX67TZV1F6S949F96ZE201W is landed upstream context rather than an active blocker.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative validator input for this ticket is the current emitted dvault.hash-key-storage-migration.v1 dry-run manifest with top-level schemaVersion, dryRun, source, target, comparison, and entries.
- entries is the v1 coverage surface; source and target carry boundary and provider provenance, and comparison carries aggregate compatibility and count facts.
- Deterministic error, warning, and info findings are validator output for that manifest input, not a requirement that the current producer already serializes a validation node.
- Ticket 06FGX67TZV1F6S949F96ZE201W is already done; keep it as landed upstream contract context, not as a current blocker. This ticket still blocks downstream wiring ticket 06FGX6B9KQME0NJ8B810239DG0.
- No child-ticket, attachment, relation, or planning-document write is required to resolve this refinement; the scope is clarified in-place.

### Scope In
- Parse and validate the existing emitted dvault.hash-key-storage-migration.v1 manifest shape from hash-key-storage-migration.
- Accept a current producer artifact as valid input when its semantic contents match the bounded HexString-to-Binary v1 baseline.
- Produce deterministic validator findings and redacted diagnostics for malformed or semantically invalid current-shape manifests.
- Add validator-side test coverage that builds valid and invalid current-shape manifests without requiring producer-emitted invalid artifacts.

### Scope Out
- Changing the existing hash-key-storage-migration exporter top-level JSON shape or reusing dvault.hash-key-storage-migration.v1 for an incompatible serialized contract.
- Introducing a new manifest version or a producer-side embedded validation section in this ticket.
- Requiring the dry-run producer to emit invalid manifests or altering its fail-closed no-output-on-drift behavior.
- Migration execution, live database inspection by default, SQL generation, EF model mutation, backfill, rehash, repair, or rollback orchestration.

## Acceptance Criteria
- At least one validator acceptance case feeds the validator a manifest matching the current emitted top-level shape schemaVersion, dryRun, source, target, comparison, and entries, and that artifact validates successfully when it preserves the checked-in HexString-to-Binary storage-only semantics.
- The validator maps the current serialized shape to the v1 semantic contract: source and target prove boundary and provider facts, entries is complete column coverage, and comparison plus per-entry facts prove the intended HexString-to-Binary change and aggregate counts.
- The validator returns deterministic error findings for malformed or semantically invalid current-shape manifests, including missing required sections or per-entry facts, duplicate or missing coverage identity, mixed or ambiguous source or target profiles, unsupported provider, profile, value-format, conversion, or hash facts, algorithm drift, digest-length drift, or digest-encoding drift.
- Invalid-manifest tests use deterministic inline or helper-built current-shape JSON fixtures derived from a known-valid producer artifact shape; the ticket does not depend on the fail-closed producer to emit invalid output files.
- Warning findings remain limited to non-blocking supplemental-evidence gaps after authoritative source evidence is complete, info findings remain deterministic and redacted, and overall finding order remains stable by severity, code, table, column, and JSON path.

## Definition of Done
- Implementation lands under the existing DVault source and test layout with validator-side automated coverage for one valid current-producer artifact and the bounded invalid current-shape fixture cases.
- Tests cover invalid schemaVersion, missing coverage, duplicate coverage, unsupported provider, profile, value-format, conversion, or hash facts, mixed storage-profile cases, algorithm, digest-length, or digest-encoding drift, and deterministic finding ordering.
- The validator surface stays diagnostics and preflight only and does not mutate the producer, emit a new manifest version, or require live database access.
- Checked-in code and tests continue to honor the visible built-in provider profile and stable-hash baselines already present in repository code.
- Ticket wording and risks reflect that 06FGX67TZV1F6S949F96ZE201W is done upstream context while 06FGX6B9KQME0NJ8B810239DG0 remains the active downstream dependent.

## Implementation Notes
- Treat schemaVersion, dryRun, source, target, comparison, and entries as the only serialized v1 input keys for this ticket.
- Semantic mapping for the existing v1 input is fixed as follows: selectedModelBoundary and reviewedSourceEvidence come from the reviewed source support-bundle provenance represented by source metadataSourceKind and metadataSourceFingerprint together with the paired target endpoint; providerProfileId is the matching capabilityProfile recorded on source and target; modelHashFacts and expectedStorageProfiles are enforced from the repeated per-entry source and target facts plus comparison summary; coverage is entries; validation findings are produced by the validator output, not by a required serialized validation input node.
- Build invalid test inputs by starting from a valid current-shape manifest helper and mutating or removing sections or entry facts in memory, matching the repository's existing inline JSON contract-test pattern for support-bundle-driven generator tests.
- Keep the visible provider baseline limited to sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1, and keep the visible stable-hash baseline limited to sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1 with unchanged digest encoding lowercase-hex-no-prefix.
- Do not require or expect producer changes in DataVaultHashKeyStorageMigrationManifestExporter or the hash-key-storage-migration command as part of this task.

## Open Questions
- none

## Follow-Up Questions
- If the team wants the producer artifact itself to serialize conceptual keys such as selectedModelBoundary, coverage, or validation, should that be a separate versioned successor ticket instead of changing dvault.hash-key-storage-migration.v1 in place?
- When downstream ticket 06FGX6B9KQME0NJ8B810239DG0 wires this validator into preflight, should its consumer-facing report surface expose the validator findings directly or only a summarized preflight status plus redacted finding payload?

## Risks
- Repository docs currently describe conceptual field names that do not match the checked-in serialized v1 producer shape; if no later doc-alignment follow-up is taken, contributors may reintroduce the same ambiguity.
- Because invalid manifests are hand-built current-shape fixtures rather than producer-emitted files, exporter schema changes in a future ticket must update validator fixtures in lockstep.
- This ticket still sits directly upstream of 06FGX6B9KQME0NJ8B810239DG0, so validator result-shape or finding-code drift can ripple into downstream preflight wiring even though the original contract-definition ticket is already done.

## Split Recommendations
- No split is needed while this ticket stays validator-only and consumes the existing producer output.
- If the team later wants to change the producer JSON shape, embed validation into the artifact, or publish a successor schema version, create a separate follow-up ticket rather than widening this task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement a small parser/validator for dvault.hash-key-storage-migration.v1 manifests according to the accepted contract.

Acceptance:
- Valid manifests produce deterministic validation results.
- Invalid schema version, missing table/column coverage, digest-size mismatch, and unsafe mixed storage profile cases are covered by tests.
- The validator does not execute migrations, inspect live databases, or alter EF models.
- Public API shape stays narrow and appropriate for diagnostics/preflight usage.