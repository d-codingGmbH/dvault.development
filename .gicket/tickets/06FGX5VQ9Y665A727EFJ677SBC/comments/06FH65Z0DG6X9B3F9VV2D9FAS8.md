[gicket-bot] PO refinement contract

Summary
- Queued one bounded follow-up to align the repository contract with the implemented six-key manifest shape and kept the parent story in PO because the checked-in contract still treats findings as serialized manifest input.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - I did not reopen done child 06FGX67TZV1F6S949F96ZE201W; instead I created one bounded follow-up titled Task: Align hash-key storage profile contract with six-key manifest shape, queued for replay on develop as outbox mutation-b5e25dc043c4bd47.
- critic-item-2: `answered` - This parent stays in PO in this pass. The contract below no longer treats the story as ready for PO-critic; it explicitly tracks the remaining repository inconsistency and waits for the queued follow-up to land before claiming that the ticket contract and cited repository contract agree that findings are validator or preflight output.
- critic-item-3: `answered` - I am treating the manifest-shape mismatch as an active blocking inconsistency, not as resolved on this branch: docs/plans/hash-key-storage-profile-contract.md still lists deterministic validation findings among required top-level manifest facts, while the implemented exporter and validator or preflight lane use a serialized v1 artifact of only schemaVersion, dryRun, source, target, comparison, and entries, with findings produced as output.

Clarifications
- Ratify v1 as an adopter-owned, review-only HexString-to-Binary migration-planning lane for existing persisted DVault model boundaries; it is not a generic profile-conversion framework or migration runner.
- Ratify the implemented serialized manifest baseline as top-level schemaVersion, dryRun, source, target, comparison, and entries, with deterministic findings produced by validator or preflight output rather than required as serialized manifest input.
- Queued one bounded follow-up ticket, Task: Align hash-key storage profile contract with six-key manifest shape, for replay on develop as outbox mutation-b5e25dc043c4bd47; no relation write was possible yet because the queued create-ticket result did not return a materialized ticket id.
- The visible built-in provider-profile baseline remains sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1, and the visible built-in stable-hash baseline remains sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1 with digest encoding lowercase-hex-no-prefix.
- The existing parentOf relations to 06FGX67TZV1F6S949F96ZE201W, 06FGX69QJYHGNKBV8MJ1HG7MMG, 06FGX6B9KQME0NJ8B810239DG0, and 06FGX6CRPG02ZWGE62QWSG42EC remain intact; all four are done and no relation cleanup was required in this pass.

Scope In
- Story-level verification of dvault.hash-key-storage-migration.v1 across contract definition, validator behavior, preflight or diagnostics integration, and end-user documentation.
- Tracking and closing the remaining inconsistency between the parent story contract and docs/plans/hash-key-storage-profile-contract.md for the six-key serialized v1 manifest shape.
- Complete selected-boundary coverage rules for DVault-owned HashKey and ParticipantReference facts across hubs, links, satellites, PITs, and bridges for HexString-to-Binary planning.
- Deterministic redacted error, warning, and info findings as validator or preflight output that fail closed before EF migrations, storage-profile changes, or caller-owned data-conversion scripts are attempted.

Scope Out
- Executing migrations, generating SQL or data-conversion scripts, automatic backfill, dual-write, repair, reconcile, rehash, or live cutover orchestration.
- Changing public hash-key values away from lowercase hexadecimal strings or changing stable-hash algorithm, digest length, truncation policy, or digest encoding as part of this story.
- Reusing this v1 lane for generic profile-conversion flows, rollback automation, or custom provider profiles beyond the visible built-in baseline.
- Merging manifest validation into EF migration-guardrail findings or requiring automatic manifest discovery from files or live databases.
- Landing repository documentation edits directly from this PO pass; that work is queued through the follow-up ticket instead.

Open questions
- none

Follow-up questions
- If the team later wants a successor manifest version with different serialized top-level keys or embedded validation payloads, should that be handled in a separate versioned ticket instead of changing dvault.hash-key-storage-migration.v1 in place?
- Should a later documentation ticket add a redacted sample dvault.hash-key-storage-migration.v1 manifest or sample preflight output to make the validation lane more concrete for adopters?
- After v1, is there value in separate follow-up work for custom provider-profile identifiers or rollback and readiness audit manifests such as Binary-to-HexString verification runs?
- If the queued develop replay materializes without a link back to this parent, should a later planning pass add an explicit parentOf relation to the new follow-up ticket?

Risks
- Until the queued follow-up lands on develop, this parent story cannot truthfully claim that the visible repository contract is internally consistent about findings versus serialized manifest shape.
- Future drift between conceptual documentation and the checked-in serialized v1 shape could reintroduce ambiguity if later tickets change exporter fields without updating docs and tests together.
- Coverage correctness still depends on authoritative support-bundle or translated-metadata capture for the full selected boundary; incomplete source evidence can underreport PIT, bridge, or participant-reference columns.

Split recommendations
- No further functional split is recommended beyond the already queued bounded follow-up ticket for contract alignment; once replay materializes it on develop, link it back to this parent if runtime does not do so automatically.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [blocked/po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment