[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded v1 HexString-to-Binary manifest validation contract aligned with the migration guide and hash-key storage profile contract; no child tickets, relation changes, description updates, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ratify v1 as an adopter-owned storage-only migration manifest for existing persisted DVault model boundaries from HexString to Binary; it is not a generic profile-conversion framework.
- Ratify the visible built-in provider baseline for v1 manifests as sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1.
- Ratify the visible built-in stable-hash baseline for v1 manifests as sha256-v1 (32 bytes), sha1-v1 (20 bytes), sha256-128-v1 (16 bytes), and sha256-160-v1 (20 bytes), with digest encoding fixed to lowercase-hex-no-prefix.
- The existing blocks relation to 06FGX69QJYHGNKBV8MJ1HG7MMG remains consistent with this ticket defining the contract consumed by downstream implementation; no relation, attachment, or description writes were applied in this refinement pass.

Scope In
- Define the required v1 manifest facts for the selected migration boundary: schema/version identity, reviewed source evidence provenance, provider profile, model-level algorithm and digest facts, and deterministic before/after storage-profile expectations.
- Define complete per-property coverage requirements for every DVault-owned HashKey and ParticipantReference column in scope across hubs, links, satellites, PITs, and bridges.
- Define the validation-finding contract, including deterministic error, warning, and info outputs and the fail-closed rules that block unsafe manifests before any migration step.
- Align the manifest contract vocabulary and guardrails with docs/hash-key-storage-migration.md and docs/plans/hash-key-storage-profile-contract.md.

Scope Out
- Executing migrations, generating schema/data conversion SQL, or performing cutover, repair, backfill, reconciliation, dual-write, or rehash work.
- Changing stable-hash algorithm, digest length, truncation policy, digest encoding, public API hash-key shape, or HashDiff/content-hash behavior.
- Generic rollback or alternate profile flows such as Binary to HexString, same-profile audit manifests, or custom provider-profile expansion beyond the visible built-in baseline.

Open questions
- none

Follow-up questions
- After v1 ships, should a later ticket add contract support for custom provider-profile identifiers beyond the current built-in baseline?
- After the forward migration path is stable, do we need a separate ticket for rollback/readiness audit manifests such as Binary to HexString or same-profile verification runs?
- Should DB2 live-schema evidence parity stay as optional supplemental validation only, or does it need a dedicated follow-up ticket once the broader DB2 catalog-reader roadmap is clearer?

Risks
- If the contract does not pin deterministic finding ordering and classification, downstream diagnostics and tests can become flaky even when the manifest contents are identical.
- Coverage validation depends on complete authoritative inventory data; incomplete support-bundle or translated-metadata capture could underreport PIT, bridge, or participant-reference columns.
- Provider-specific store-type aliases and equivalent persisted-shape normalization can create false mismatches or false passes if the validator does not compare the full fact set and fail closed on ambiguity.

Split recommendations
- No split recommended; the ticket is already bounded to the v1 manifest validation contract and the existing downstream implementation dependency.

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