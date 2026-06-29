[gicket-bot] PO refinement contract

Summary
- Fresh repo inspection shows the export command, validator, preflight lane, migration guide, and README baseline already exist. This refinement narrows the ticket to end-to-end documentation alignment for the existing `dvault.hash-key-storage-migration.v1` dry-run validation flow, with no ticket writes or child splits materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket documents an existing machine-checkable path rather than creating new product behavior: the repository already contains the `hash-key-storage-migration` design-time export verb, the `dvault.hash-key-storage-migration.v1` manifest schema, `DataVaultHashKeyStorageMigrationManifestValidator`, and the optional manifest-validation lane on `DataVaultPreflight.Run(...)`.
- Treat `docs/releases/v0.49.0.md` as the current release-notes baseline that still needs this migration-flow pointer; the older `docs/releases/v0.43.0.md` mention is historical context, not the current baseline.
- The packaged README is the root `README.md` reused by the runtime/provider/privacy packages, so package-verifier expectation changes are only in scope if the touched README wording crosses existing packaged-guidance assertions.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement.

Scope In
- Document the exact adopter-owned dry-run flow for existing persisted `HexString` storage: capture reviewed source evidence, export a `dvault.hash-key-storage-migration.v1` manifest, validate/review it, and only then plan EF migration or data-conversion work.
- Align the Hash-Key Storage Migration Guide, current README guidance, and current release notes so they consistently route existing `HexString` users to the validated dry-run path while keeping binary-first guidance for new schemas.
- State consistently that DVault compares reviewed source facts against the current design-time model, fails closed on drift, and does not apply migrations, backfill keys, dual-write, or automatically rewrite persisted hash keys.
- Update package-verifier expectations only if root `README.md` changes affect packaged README assertions.

Scope Out
- No product-code changes to the manifest exporter, validator, preflight engine, provider capability baseline, or migration guardrails.
- No migration runner, repair path, DDL/DML generator, automatic backfill, dual-write, rollback automation, or live-database orchestration.
- No change to the stable-hash algorithm set, digest-length baseline, digest encoding contract, or supported provider-profile list.
- No provider-specific cutover scripts or operational runbooks beyond documentation that points adopters to caller-owned migration work.

Open questions
- none

Follow-up questions
- Should a later docs ticket add a redacted sample `dvault.hash-key-storage-migration.v1` manifest or preflight output to make the validation lane more concrete for adopters?

Risks
- If the docs update touches root `README.md`, packaged README verification can fail unless the existing package-verifier assertions are still satisfied or updated intentionally.
- If the update only amends the migration guide and skips current release notes, the public documentation baseline will remain inconsistent even though the underlying exporter and validator flow already exist.

Split recommendations
- No split recommended; the visible repository scope is bounded to documentation alignment around an already implemented export and validation flow.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment