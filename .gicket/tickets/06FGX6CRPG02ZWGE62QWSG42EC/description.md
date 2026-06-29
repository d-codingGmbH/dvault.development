<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh repo inspection shows the export command, validator, preflight lane, migration guide, and README baseline already exist. This refinement narrows the ticket to end-to-end documentation alignment for the existing `dvault.hash-key-storage-migration.v1` dry-run validation flow, with no ticket writes or child splits materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket documents an existing machine-checkable path rather than creating new product behavior: the repository already contains the `hash-key-storage-migration` design-time export verb, the `dvault.hash-key-storage-migration.v1` manifest schema, `DataVaultHashKeyStorageMigrationManifestValidator`, and the optional manifest-validation lane on `DataVaultPreflight.Run(...)`.
- Treat `docs/releases/v0.49.0.md` as the current release-notes baseline that still needs this migration-flow pointer; the older `docs/releases/v0.43.0.md` mention is historical context, not the current baseline.
- The packaged README is the root `README.md` reused by the runtime/provider/privacy packages, so package-verifier expectation changes are only in scope if the touched README wording crosses existing packaged-guidance assertions.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement.

### Scope In
- Document the exact adopter-owned dry-run flow for existing persisted `HexString` storage: capture reviewed source evidence, export a `dvault.hash-key-storage-migration.v1` manifest, validate/review it, and only then plan EF migration or data-conversion work.
- Align the Hash-Key Storage Migration Guide, current README guidance, and current release notes so they consistently route existing `HexString` users to the validated dry-run path while keeping binary-first guidance for new schemas.
- State consistently that DVault compares reviewed source facts against the current design-time model, fails closed on drift, and does not apply migrations, backfill keys, dual-write, or automatically rewrite persisted hash keys.
- Update package-verifier expectations only if root `README.md` changes affect packaged README assertions.

### Scope Out
- No product-code changes to the manifest exporter, validator, preflight engine, provider capability baseline, or migration guardrails.
- No migration runner, repair path, DDL/DML generator, automatic backfill, dual-write, rollback automation, or live-database orchestration.
- No change to the stable-hash algorithm set, digest-length baseline, digest encoding contract, or supported provider-profile list.
- No provider-specific cutover scripts or operational runbooks beyond documentation that points adopters to caller-owned migration work.

## Acceptance Criteria
- The migration documentation shows a complete pre-change sequence for existing persisted `HexString` storage: capture reviewed source support-bundle or equivalent metadata evidence, run the `hash-key-storage-migration` dry-run export, and validate/review the resulting `dvault.hash-key-storage-migration.v1` manifest before any EF migration or data conversion is attempted.
- The documented validation flow makes the machine-checkable boundary explicit: structural or compatibility drift blocks the flow, warnings are non-structural only, and the same docs make clear that DVault is validating a review artifact rather than executing the migration.
- README and current release notes explicitly route existing `HexString` users to the validated dry-run manifest path and preserve the separate message that binary-first is the recommendation for new schemas only.
- If README wording changes touch packaged guidance, package-verifier expectations remain aligned; if packaged README assertions are unaffected, no verifier expectation change is required.

## Definition of Done
- A reader can determine from the updated docs which command or API surfaces produce the manifest, which surface validates it, what facts are checked, and what work remains caller-owned after validation.
- The migration guide, README, and current release notes tell one consistent story about binary-first for new schemas, reviewed dry-run validation for existing persisted `HexString` storage, and DVault's explicit non-goals.
- Any touched README-backed package-verification assertions or related documentation checks are updated to match the final wording.

## Implementation Notes
- Repository evidence already covers most of the contract in `docs/hash-key-storage-migration.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, `docs/production-adoption-checklist.md`, and `README.md`; the refinement gap is end-to-end discoverability and current release-note alignment, not missing core behavior.
- Use the existing bounded repo defaults when writing the docs: supported provider profiles are `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`; built-in stable-hash ids are `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- Prefer the current consumer-owned preflight framing when describing validation: `DataVaultPreflight.Run(...)` already exposes a separate optional hash-key-storage-migration-manifest section, while the direct validator type is the lower-level machine-checkable surface.
- Keep the docs explicit that public hash-key values remain lowercase hexadecimal strings even when binary physical storage is selected, and that equal digest size does not prove algorithm compatibility, especially for the `sha1-v1` versus `sha256-160-v1` case.
- The root `README.md` is packed into the runtime, provider, and privacy packages and is checked by `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, so any README edits should be reviewed against existing packaged-guidance assertions.

## Open Questions
- none

## Follow-Up Questions
- Should a later docs ticket add a redacted sample `dvault.hash-key-storage-migration.v1` manifest or preflight output to make the validation lane more concrete for adopters?

## Risks
- If the docs update touches root `README.md`, packaged README verification can fail unless the existing package-verifier assertions are still satisfied or updated intentionally.
- If the update only amends the migration guide and skips current release notes, the public documentation baseline will remain inconsistent even though the underlying exporter and validator flow already exist.

## Split Recommendations
- No split recommended; the visible repository scope is bounded to documentation alignment around an already implemented export and validation flow.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update the Hash-Key Storage Migration Guide and related docs for the machine-checkable manifest validation path.

Acceptance:
- Docs show how to produce and validate a dry-run manifest before changing EF migrations or data.
- Docs state that DVault does not execute the migration or automatically rewrite persisted keys.
- README and release notes point existing HexString users to the validated dry-run flow.
- Package verifier expectations are updated only if packaged README guidance changes.