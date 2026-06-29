[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository baseline already includes DataVaultHashKeyStorageMigrationManifestValidator, DataVaultHashKeyStorageMigrationValidationResult, and the hash-key-storage-migration design-time command. This ticket is integration work over that existing validator, not a new manifest contract.
- The v1 preflight input is caller-owned and explicit: consumers provide a serialized dvault.hash-key-storage-migration.v1 manifest to the library surface; the library must not discover files, open live schema, or read repository paths for this lane.
- Manifest validation must surface as its own diagnostics/preflight result, not as extra EF migration-guardrail findings, because the repository already models migration guardrails and aggregate preflight lanes as separate contracts.
- Any diagnostics or support-bundle projection for this lane must stay redacted and structural-only, reusing the manifest contract's error, warning, and info findings and never emitting raw hash keys, raw business keys, SQL, credentials, or raw support-bundle payload data.

Scope In
- Add an explicit hash-key storage migration manifest-validation lane to the existing preflight/diagnostics boundary.
- Validate caller-supplied dvault.hash-key-storage-migration.v1 JSON with the existing manifest validator and preserve its structured findings.
- Expose manifest-validation status/output separately from EF migration operation guardrails.
- Cover success, failure, omitted-input skip semantics, and redacted serialization behavior with tests.

Scope Out
- Changing the manifest schema/version or redefining the HexString-to-Binary v1 contract.
- Executing migrations, generating DDL or DML, backfilling data, opening live databases automatically, or discovering manifest files automatically.
- Merging manifest validation into EF migration-guardrail findings or redesigning existing guardrail rules.
- Publishing a new standalone support-bundle artifact format or embedding raw manifest/support-bundle payloads.

Open questions
- none

Follow-up questions
- If adopters later want manifest-validation details inside dvault.support-bundle.v1, decide in a later additive ticket whether that projection should carry full structural coverage entries or a smaller summary view; this ticket only needs the shared redaction boundary.
- Verify during implementation scheduling whether the live incoming blocks relation from 06FGX69QJYHGNKBV8MJ1HG7MMG is still active, since this refinement did not materialize any relation cleanup.

Risks
- Adding a new public preflight/diagnostics lane changes structured output and display text; omitted-input and serialization compatibility need explicit regression tests.
- If developers fold manifest findings into migration-guardrail issues, the implementation will violate the ticket's required separation between manifest validation and EF migration guardrails.
- If diagnostics/support-bundle projection reuses raw manifest text instead of the structured validation result, it risks breaching the redaction boundary documented for hash-key migration planning.

Split recommendations
- No split recommended: the repository already contains the validator, manifest command, and aggregate preflight scaffolding, so the remaining work is a bounded integration and test task.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment