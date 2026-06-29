[gicket-bot] PO refinement contract

Summary
- Fresh repository and ticket evidence shows this parent story is already bounded by four done child tickets covering the manifest contract, validator, preflight integration, and documentation flow; no relation, attachment, description, or planning-document writes were needed in this refinement pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ratify v1 as an adopter-owned, review-only HexString-to-Binary migration-planning lane for existing persisted DVault model boundaries; it is not a generic profile-conversion framework or migration runner.
- Ratify the current serialized manifest baseline as top-level `schemaVersion`, `dryRun`, `source`, `target`, `comparison`, and `entries`; deterministic validation findings are validator output, not a required serialized input node.
- Ratify the visible built-in provider-profile baseline for this lane as `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`.
- Ratify the visible built-in stable-hash baseline as `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`, with digest encoding fixed to `lowercase-hex-no-prefix`.
- The existing `parentOf` relations to `06FGX67TZV1F6S949F96ZE201W`, `06FGX69QJYHGNKBV8MJ1HG7MMG`, `06FGX6B9KQME0NJ8B810239DG0`, and `06FGX6CRPG02ZWGE62QWSG42EC` remain consistent; all four child tickets are `done` and no relation cleanup was required for this PO pass.

Scope In
- Story-level verification of `dvault.hash-key-storage-migration.v1` across contract definition, validator behavior, preflight or diagnostics integration, and end-user documentation.
- Complete selected-boundary coverage rules for DVault-owned `HashKey` and `ParticipantReference` facts across hubs, links, satellites, PITs, and bridges for HexString-to-Binary planning.
- Deterministic redacted `error`, `warning`, and `info` findings that fail closed before EF migrations, storage-profile changes, or caller-owned data-conversion scripts are attempted.
- Documentation and diagnostics that keep binary-first guidance for new schemas separate from reviewed dry-run migration planning for existing persisted storage.

Scope Out
- Executing migrations, generating SQL or data-conversion scripts, automatic backfill, dual-write, repair, reconcile, rehash, or live cutover orchestration.
- Changing public hash-key values away from lowercase hexadecimal strings or changing stable-hash algorithm, digest length, truncation policy, or digest encoding as part of this story.
- Reusing this v1 lane for generic profile-conversion flows, rollback automation, or custom provider profiles beyond the visible built-in baseline.
- Merging manifest validation into EF migration-guardrail findings or requiring automatic manifest discovery from files or live databases.

Open questions
- none

Follow-up questions
- If the team later wants a successor manifest version with different serialized top-level keys or embedded validation payloads, should that be handled in a separate versioned ticket instead of changing `dvault.hash-key-storage-migration.v1` in place?
- Should a later documentation ticket add a redacted sample `dvault.hash-key-storage-migration.v1` manifest or sample preflight output to make the validation lane more concrete for adopters?
- After v1, is there value in separate follow-up work for custom provider-profile identifiers or rollback and readiness audit manifests such as Binary-to-HexString verification runs?

Risks
- Future drift between conceptual documentation and the checked-in serialized v1 shape could reintroduce ambiguity if later tickets change exporter fields without updating docs and tests together.
- Coverage correctness still depends on authoritative support-bundle or translated-metadata capture for the full selected boundary; incomplete source evidence can underreport PIT, bridge, or participant-reference columns.
- If future work collapses manifest findings into migration-guardrail output or emits raw manifest or support-bundle payloads, it will break the current separation and redaction boundary.

Split recommendations
- No further split recommended; the parent already has four bounded child tickets for contract, validator, preflight integration, and documentation, and all four are done.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment