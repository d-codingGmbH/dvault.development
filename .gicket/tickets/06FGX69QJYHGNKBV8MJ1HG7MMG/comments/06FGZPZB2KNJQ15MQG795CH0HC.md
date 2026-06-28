[gicket-bot] PO refinement contract

Summary
- Reconciled this ticket to consume the current dry-run dvault.hash-key-storage-migration.v1 shape, scoped producer-schema changes out of the task, defined inline invalid-manifest fixture sourcing, and refreshed dependency wording so 06FGX67TZV1F6S949F96ZE201W is landed upstream context rather than an active blocker.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This ticket preserves the current emitted dvault.hash-key-storage-migration.v1 JSON shape. The validator must accept the existing top-level schemaVersion, dryRun, source, target, comparison, and entries artifact emitted by the checked-in exporter/tests; it must not replace that shape or reuse the same schemaVersion for an incompatible serialized form. The docs/plans conceptual fields are satisfied by mapping selectedModelBoundary, reviewedSourceEvidence, providerProfileId, modelHashFacts, expectedStorageProfiles, and coverage onto the existing sections, while validation findings are produced by the validator output rather than required as an existing serialized top-level input key.
- critic-item-2: `answered` - No producer-shape change belongs to this ticket. This task remains validator-only: it consumes the current dry-run manifest shape, adds validator-side tests against that shape, and does not modify the existing hash-key-storage-migration exporter, command, or serialized schema. Any producer JSON reshaping, embedded validation section, or versioned successor manifest belongs to a separate follow-up ticket.
- critic-item-3: `answered` - Invalid-manifest scenarios for this validator are expected to come from deterministic hand-authored current-shape test fixtures, not from the fail-closed producer. Validator tests should start from a known-valid current producer artifact shape and then mutate or remove sections and per-entry facts in memory to create missing-coverage, duplicate-coverage, unsupported-value, mixed-profile, and drift cases.
- critic-item-4: `answered` - The delivery contract wording is refreshed to treat ticket 06FGX67TZV1F6S949F96ZE201W as landed upstream contract context, not as a current blocker. This ticket still blocks downstream wiring ticket 06FGX6B9KQME0NJ8B810239DG0, and the lingering blocks relation from the done upstream ticket is historical dependency context rather than an active open blocker because the current ticket snapshot is is-blocked=false.
- critic-item-5: `answered` - The blocking v1 shape disagreement is resolved by ratifying the checked-in emitted manifest as the validator input for this ticket. A manifest emitted today by hash-key-storage-migration is intended valid input for the validator when its contents satisfy the bounded HexString-to-Binary rules; this ticket does not replace that shape or introduce a same-version alternative.
- critic-item-6: `answered` - The invalid-manifest source ambiguity is resolved by making validator findings a consumer-side validation result over current-shape JSON fixtures, not a producer-emitted invalid artifact. The validator must return deterministic error, warning, and info findings for hand-built invalid manifests that preserve the current top-level shape but contain structural or semantic faults; producer fail-closed behavior remains unchanged.

Clarifications
- The authoritative validator input for this ticket is the current emitted dvault.hash-key-storage-migration.v1 dry-run manifest with top-level schemaVersion, dryRun, source, target, comparison, and entries.
- entries is the v1 coverage surface; source and target carry boundary and provider provenance, and comparison carries aggregate compatibility and count facts.
- Deterministic error, warning, and info findings are validator output for that manifest input, not a requirement that the current producer already serializes a validation node.
- Ticket 06FGX67TZV1F6S949F96ZE201W is already done; keep it as landed upstream contract context, not as a current blocker. This ticket still blocks downstream wiring ticket 06FGX6B9KQME0NJ8B810239DG0.
- No child-ticket, attachment, relation, or planning-document write is required to resolve this refinement; the scope is clarified in-place.

Scope In
- Parse and validate the existing emitted dvault.hash-key-storage-migration.v1 manifest shape from hash-key-storage-migration.
- Accept a current producer artifact as valid input when its semantic contents match the bounded HexString-to-Binary v1 baseline.
- Produce deterministic validator findings and redacted diagnostics for malformed or semantically invalid current-shape manifests.
- Add validator-side test coverage that builds valid and invalid current-shape manifests without requiring producer-emitted invalid artifacts.

Scope Out
- Changing the existing hash-key-storage-migration exporter top-level JSON shape or reusing dvault.hash-key-storage-migration.v1 for an incompatible serialized contract.
- Introducing a new manifest version or a producer-side embedded validation section in this ticket.
- Requiring the dry-run producer to emit invalid manifests or altering its fail-closed no-output-on-drift behavior.
- Migration execution, live database inspection by default, SQL generation, EF model mutation, backfill, rehash, repair, or rollback orchestration.

Open questions
- none

Follow-up questions
- If the team wants the producer artifact itself to serialize conceptual keys such as selectedModelBoundary, coverage, or validation, should that be a separate versioned successor ticket instead of changing dvault.hash-key-storage-migration.v1 in place?
- When downstream ticket 06FGX6B9KQME0NJ8B810239DG0 wires this validator into preflight, should its consumer-facing report surface expose the validator findings directly or only a summarized preflight status plus redacted finding payload?

Risks
- Repository docs currently describe conceptual field names that do not match the checked-in serialized v1 producer shape; if no later doc-alignment follow-up is taken, contributors may reintroduce the same ambiguity.
- Because invalid manifests are hand-built current-shape fixtures rather than producer-emitted files, exporter schema changes in a future ticket must update validator fixtures in lockstep.
- This ticket still sits directly upstream of 06FGX6B9KQME0NJ8B810239DG0, so validator result-shape or finding-code drift can ripple into downstream preflight wiring even though the original contract-definition ticket is already done.

Split recommendations
- No split is needed while this ticket stays validator-only and consumes the existing producer output.
- If the team later wants to change the producer JSON shape, embed validation into the artifact, or publish a successor schema version, create a separate follow-up ticket rather than widening this task.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment