<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh .gicket and repository evidence shows this story is already cleanly split across four completed child tasks for provider-boundary guidance, privacy diagnostics/support-bundle facts, caller-owned key-provider quickstart proof, and docs alignment; no further PO split or clarification is needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence now matches the story draft: provider-native encryption remains a documented guidance-only boundary, privacy diagnostics and support-bundle facts exist in core, the optional privacy package exposes caller-owned alias-driven EF Core value-converter seams, and docs/examples already describe the same bounded posture.
- No child-ticket creation, relation changes, description updates, attachments, or planning documents were materialized in this PO refinement run.

### Scope In
- Keep the optional privacy extension explicitly opt-in, provider-neutral, and caller-owned for encrypted payload conversion over ordinary EF Core mapped payload properties.
- Document and preserve provider-native encryption as a guidance-only boundary for the visible provider baseline rather than DVault-owned runtime behavior.
- Expose and use redaction-safe privacy diagnostics and support-bundle facts for alias coverage, personal-data coverage, and key-provider posture.
- Keep the runnable or test-backed quickstart baseline for AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), and DataVaultEncryptedPayloadValueConverter fail-closed behavior.
- Keep public docs, examples, changelog, and release-note language aligned with the same non-compliance, non-key-management boundary.

### Scope Out
- Provider-native encryption implementations, encrypted DDL emission, provider SQL crypto dispatch, capability probing, or runtime routing based on native encryption availability.
- Compliance guarantees, GDPR or DSGVO ownership, legal attestation, or DVault-owned key lifecycle management.
- Automatic crypto-shredding, deletion, retention, PIT cleanup, bridge cleanup, backup purge, or legal-erasure workflows.
- Any expansion beyond the current finite provider baseline of SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 unless a new provider-specific ticket owns one exact capability.

## Acceptance Criteria
- The authoritative privacy boundary keeps provider-native encryption unmanaged and guidance-only, and the story does not reopen provider capability scope beyond the documented SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 baseline.
- Diagnostics and support-bundle output provide additive redaction-safe privacy adoption facts for alias coverage, personal-data coverage, and key-provider posture without payload values, key material, secrets, connection details, or database capability probing.
- The v1 example surface remains the explicit caller-owned EF Core proof: AddDVaultPrivacy(...), alias registration, caller-supplied key provider wiring, DataVaultEncryptedPayloadValueConverter, and fail-closed behavior when registration or usable provider support is missing.
- Public docs and examples consistently state that DVault remains an EF Core library seam and does not claim compliance ownership, provider-native encrypted DDL, provider SQL crypto execution, or automatic shredding behavior.

## Definition of Done
- The four existing child tickets remain the authoritative implementation slices for this story, and their completed outcomes together satisfy the story boundary.
- Core diagnostics, the optional privacy package, tests, and public docs stay aligned on explicit opt-in, caller-owned, provider-neutral behavior.
- No additional child split, relation cleanup, or PO clarification is required before PO-critic review.
- Story-level evidence remains bounded to the current repository baseline and does not introduce provider-native runtime encryption or compliance ownership claims.

## Implementation Notes
- Use docs/architecture/dvault-v1-optional-privacy-extension-boundary.md as the authoritative source for the unmanaged guidance-only provider-native encryption boundary.
- Use DataVaultDiagnosticsResult.Privacy and DataVaultPrivacyDiagnostics as the authoritative structured privacy-adoption facts that support-bundle export reuses instead of inventing a separate privacy-only artifact path.
- Use docs/getting-started.md and examples/README.md as the v1 quickstart baseline: AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), DataVaultEncryptedPayloadValueConverter, and fail-closed conversion behavior.
- Preserve the optional-package boundary: future privacy diagnostics work must keep core abstractions independent from DCoding.Data.DVault.Privacy concrete types.
- Current repository evidence already includes supporting tests for diagnostics/support-bundle privacy facts, privacy coverage reporting, and encrypted payload value-converter behavior; refinement should ratify that baseline rather than reopen API shape questions.

## Open Questions
- none

## Follow-Up Questions
- If future provider-native encryption work is approved, which single provider and exact capability should get the first separate ticket?
- When a later package-line baseline changes, should privacy docs and package-verifier expectations be updated in one coordinated pass to avoid drift?

## Risks
- Future edits could blur the provider-neutral alias-driven privacy proof with provider-native encryption or compliance claims across docs, diagnostics, and examples.
- Future changes could accidentally couple core diagnostics types to privacy-package concrete implementations and erode the optional-package boundary.

## Split Recommendations
- No additional split is recommended; the story is already partitioned into completed child tickets for provider boundary, diagnostics/support-bundle facts, quickstart proof, and docs alignment.
- Any later native-encryption feature should be created as a new provider-specific ticket for one exact capability rather than widening this shared story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Improve the optional DVault privacy extension so consumers can adopt encrypted payload seams confidently while DVault stays an EF Core library and does not become a compliance platform.

Acceptance:
- Provider-native encryption capabilities are documented as boundaries, not automatic DVault behavior.
- Privacy diagnostics/support-bundle output helps consumers prove alias coverage and understand unmanaged native encryption responsibilities.
- A runnable or test-backed example demonstrates caller-owned key provider behavior with EF Core value converters.
- The package continues to avoid compliance claims, key lifecycle ownership, provider-native encrypted DDL, SQL crypto dispatch, or automatic shredding execution.