<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository baseline already includes DataVaultHashKeyStorageMigrationManifestValidator, DataVaultHashKeyStorageMigrationValidationResult, and the hash-key-storage-migration design-time command. This ticket is integration work over that existing validator, not a new manifest contract.
- The v1 preflight input is caller-owned and explicit: consumers provide a serialized dvault.hash-key-storage-migration.v1 manifest to the library surface; the library must not discover files, open live schema, or read repository paths for this lane.
- Manifest validation must surface as its own diagnostics/preflight result, not as extra EF migration-guardrail findings, because the repository already models migration guardrails and aggregate preflight lanes as separate contracts.
- Any diagnostics or support-bundle projection for this lane must stay redacted and structural-only, reusing the manifest contract's error, warning, and info findings and never emitting raw hash keys, raw business keys, SQL, credentials, or raw support-bundle payload data.

### Scope In
- Add an explicit hash-key storage migration manifest-validation lane to the existing preflight/diagnostics boundary.
- Validate caller-supplied dvault.hash-key-storage-migration.v1 JSON with the existing manifest validator and preserve its structured findings.
- Expose manifest-validation status/output separately from EF migration operation guardrails.
- Cover success, failure, omitted-input skip semantics, and redacted serialization behavior with tests.

### Scope Out
- Changing the manifest schema/version or redefining the HexString-to-Binary v1 contract.
- Executing migrations, generating DDL or DML, backfilling data, opening live databases automatically, or discovering manifest files automatically.
- Merging manifest validation into EF migration-guardrail findings or redesigning existing guardrail rules.
- Publishing a new standalone support-bundle artifact format or embedding raw manifest/support-bundle payloads.

## Acceptance Criteria
- Consumers can pass a serialized dvault.hash-key-storage-migration.v1 manifest through the existing preflight-style request or equivalent diagnostics path, and the library validates it with DataVaultHashKeyStorageMigrationManifestValidator.
- Aggregate preflight reports manifest validation in a dedicated lane that is distinct from migration-guardrail, with blocking behavior when manifest findings include one or more error severities.
- When manifest input is omitted, the preflight lane behaves like other optional lanes and reports a deterministic skipped or no-input outcome instead of inventing discovery behavior.
- If diagnostics or support-bundle output is extended for this lane, it preserves only structural manifest-validation facts or findings and emits no raw hash-key values or other secret-bearing data.
- Tests cover valid manifests, invalid manifests, deterministic display or serialization, and clear separation between manifest-validation results and EF migration-guardrail results.

## Definition of Done
- The public preflight request/report surface includes an explicit optional manifest-validation path with deterministic status and display behavior.
- The diagnostics result shape can carry the manifest-validation outcome as a separate structured section when this lane is used.
- Existing standalone manifest-validator behavior and the hash-key-storage-migration design-time command remain compatible.
- Unit tests cover lane skipping, blocking errors, non-blocking warnings/info, and any diagnostics/support-bundle serialization touched by the change.
- Relevant workflow/documentation text for design-time preflight/diagnostics is updated if the public surface changes.

## Implementation Notes
- Follow the current DataVaultPreflight pattern: explicit caller-owned optional input, skipped when absent, blocked when the evaluated report contains errors.
- Reuse DataVaultHashKeyStorageMigrationValidationResult and DataVaultHashKeyStorageMigrationValidationFinding as the canonical manifest-validation shape instead of mapping them into a second issue model.
- Keep the manifest-validation lane separate from DataVaultMigrationGuardrailReport; both may appear in the same aggregate preflight run, but they must remain independently readable.
- Use the current visible manifest baseline as the v1 default: supported built-in provider profiles, HexString source profile, Binary target profile, built-in stable-hash ids, and deterministic finding ordering.
- If DataVaultSupportBundle serialization is touched, project only the structured validation result or its structural facts; do not embed raw manifest text or rehydrate source support-bundle contents.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement.

## Open Questions
- none

## Follow-Up Questions
- If adopters later want manifest-validation details inside dvault.support-bundle.v1, decide in a later additive ticket whether that projection should carry full structural coverage entries or a smaller summary view; this ticket only needs the shared redaction boundary.
- Verify during implementation scheduling whether the live incoming blocks relation from 06FGX69QJYHGNKBV8MJ1HG7MMG is still active, since this refinement did not materialize any relation cleanup.

## Risks
- Adding a new public preflight/diagnostics lane changes structured output and display text; omitted-input and serialization compatibility need explicit regression tests.
- If developers fold manifest findings into migration-guardrail issues, the implementation will violate the ticket's required separation between manifest validation and EF migration guardrails.
- If diagnostics/support-bundle projection reuses raw manifest text instead of the structured validation result, it risks breaching the redaction boundary documented for hash-key migration planning.

## Split Recommendations
- No split recommended: the repository already contains the validator, manifest command, and aggregate preflight scaffolding, so the remaining work is a bounded integration and test task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Make the manifest validator available through the existing preflight or diagnostics boundary.

Acceptance:
- Consumers can include a hash-key storage migration manifest in a preflight-style request or equivalent diagnostics path.
- The resulting report clearly separates manifest validation from EF migration operation guardrails.
- Support-bundle output, if extended, includes only structural migration-plan facts and no raw hash-key values.
- Tests cover successful and failing manifest-validation reports.