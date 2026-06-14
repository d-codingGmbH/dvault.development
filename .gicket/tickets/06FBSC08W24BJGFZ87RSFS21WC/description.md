<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratified the bounded v1 hash-key storage-profile contract: existing diagnostics, explain, and support-bundle surfaces already carry the selected storage facts, so this ticket should stay focused on explicit coverage and redaction-safe acceptance language.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence fixes the v1 storage-profile vocabulary to HexString and Binary; this ticket should not introduce a third persisted storage-profile token.
- Selected hash-key storage facts are already modeled on explain type mappings and translated hash-key/reference properties, and support bundles serialize that same explain payload.
- No child tickets, description updates, attachments, or relation changes were materialized in this refinement pass; the existing blocks relation to 06FBSC0TMZBXVVECGQGESWPCY4 remains unchanged.

### Scope In
- Report selected hash-key storage facts through the existing diagnostics/explain/support-bundle surfaces for hash keys and participant references.
- Keep HexString versus Binary selection visible in both structured and human-readable diagnostics without exposing secret-bearing values.
- Add or ratify tests that distinguish the supported selection scenarios called out by the ticket wording while staying inside the bounded v1 storage vocabulary.

### Scope Out
- Any new public hash-key value type, new diagnostics API surface, or third v1 storage-profile vocabulary item.
- Raw business keys, raw hash-key values, request values, SQL text, provider plans, or other secret-bearing diagnostics payloads.
- Migration, backfill, repair, dual-write, or provider-side hashing behavior outside the already selected storage metadata facts.

## Acceptance Criteria
- Structured explain output reports the selected hash-key storage facts for both type mappings and translated hash-key/participant-reference properties: hashKeyStorageProfile, provider store type, provider value format, stableHashAlgorithmId, digestByteLength, digestEncoding, and conversionBehavior.
- Human-readable diagnostics include the selected hash-key storage summary alongside stable-hash facts without including raw business-key or hash-key values.
- dvault.support-bundle.v1 serializes the same selected hash-key storage facts under diagnostics.explain and preserves the current redaction boundary.
- Tests distinguish the supported selection scenarios in this ticket: default hex-compatible output, explicit binary selection, and any provider/profile-preselected binary projection already supported by the visible API, without inventing a third storage-profile vocabulary.

## Definition of Done
- Relevant unit or integration tests pass for HexString and Binary diagnostics/support-bundle reporting and prove raw keys and raw digest values remain absent.
- Ticket wording, tests, and implementation stay aligned with the bounded v1 storage-profile contract in docs/plans/hash-key-storage-profile-contract.md.
- The refinement remains ticket-bounded with no additional planning artifacts or relation rewrites required from the current evidence.

## Implementation Notes
- Use docs/plans/hash-key-storage-profile-contract.md as the authority: diagnostics and support bundles must carry metadata facts for hash keys and participant references, not payload values.
- Visible repository surfaces already expose these facts through DataVaultProviderTypeMappingExplain, DataVaultPropertyExplain, DataVaultDiagnosticsResult.ToDisplayString(), and DataVaultSupportBundleExporter; the likely work is to ratify or extend coverage around those surfaces rather than invent new output channels.
- Treat binary-first as a selection-path distinction, not as a third persisted storage profile; the visible v1 storage vocabulary remains HexString and Binary.
- Keep the current live ticket relation state consistent with this refinement: the existing blocks edge to 06FBSC0TMZBXVVECGQGESWPCY4 is not changed here.

## Open Questions
- none

## Follow-Up Questions
- After this ticket lands, should public adopter guidance include one concrete binary diagnostics/support-bundle example for migration and support playbooks?

## Risks
- The acceptance wording can be misread as requiring a third storage-profile enum; without the bounded clarification above, implementation could accidentally widen the v1 contract.
- If tests only cover HexString and one Binary selection path, a regression in a provider/profile-preselected Binary path could escape even though the shared diagnostics surface is the same.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Surface the selected hash storage profile in existing diagnostics/support-bundle/explain outputs without leaking sensitive data. Acceptance: binary-first, explicit binary, and hex-compatible profiles are distinguishable in tests.