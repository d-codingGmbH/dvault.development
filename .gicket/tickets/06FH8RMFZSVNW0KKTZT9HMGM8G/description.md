<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Implement the first bounded provider-native crypto usage proof on the provider-owned SQL Server Always Encrypted selection path introduced by the upstream configuration-contract work.
- Keep the shared privacy package provider-neutral: no provider-name auto-routing, no shared native runtime encryption dispatcher, no encrypted DDL generation, no SQL crypto invocation, and no live probing by default.
- Treat this ticket as proof and guardrail work: show that explicit provider-owned native selection can be configured, diagnosed, and failed closed, while caller-owned custom encrypted-payload implementations remain supported.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The feasible first provider path for this ticket is SQL Server Always Encrypted through the provider-owned `AddDVaultSqlServerAlwaysEncryptedSelection(...)` surface.
- Provider-native execution itself remains bounded to proof/fallback tests unless the provider package can demonstrate a safe reviewed runtime path without widening shared ownership.
- Live-provider coverage is optional and must stay behind explicit `DVAULT_TEST_*` gates; unit-level proof and diagnostics coverage must not require a local SQL Server instance.
- Custom/caller-owned encrypted-payload conversion remains first-class and must not be removed, weakened, or silently replaced by native selection.

### Scope In
- Add or tighten tests that prove SQL Server native selection is explicit, provider-owned, alias-driven, and visible through redacted diagnostics.
- Add fail-closed tests for missing caller-owned prerequisites, incompatible provider profile, unsupported or unavailable capability facts, and unsupported-provider paths.
- Add regression coverage showing the existing custom encrypted-payload converter/key-provider path still works when no provider-native option is selected.
- Add support-bundle or diagnostics assertions that native-selection messages remain redacted and do not expose connection strings, secrets, raw SQL, or key material.
- If a live SQL Server Always Encrypted proof is added, gate it behind explicit environment variables and make skipped/no-environment behavior deterministic.

### Scope Out
- Implementing a shared cross-provider native-encryption runtime, provider-name auto-negotiation, encrypted DDL generation, SQL crypto function calls, key-store integration, or live database probing by default.
- Claiming GDPR/DSGVO compliance, DVault-owned key lifecycle, crypto-shredding, retention, deletion, or operational key management.
- Expanding this ticket to PostgreSQL, Oracle, MySQL, SQLite, or DB2 runtime execution beyond fallback/unsupported diagnostics tests.
- Reworking the upstream provider-native capability catalog except for narrowly required test seams or redaction-safe diagnostics assertions.

## Acceptance Criteria
- SQL Server Always Encrypted selection is exercised through a provider-owned API/seam, not through `DataVaultPrivacyOptions` or shared provider-name dispatch.
- The explicit selection remains alias-driven and compatible with the existing privacy metadata and EF Core mapped-property/value-converter model.
- Missing caller-owned prerequisite proofs fail closed with a redacted diagnostics issue and do not silently downgrade to plaintext, custom conversion, or implicit native behavior.
- Incompatible provider profile or unsupported/unavailable capability facts fail closed with redacted diagnostics.
- The caller-owned custom encrypted-payload path still works when native selection is not configured.
- Live-provider tests, if any, are skipped unless the documented `DVAULT_TEST_*` environment variables are present and valid.

## Definition of Done
- Unit tests cover provider-owned SQL Server selection, fail-closed fallback cases, custom-path preservation, and redaction behavior.
- Public API snapshots are updated only where the provider-owned seam intentionally changes public surface.
- The implementation does not add shared native runtime dispatch, provider-name branching, live probing by default, or DVault-owned key lifecycle behavior.
- Verification evidence is recorded in the ticket handoff, including build/test commands and any intentionally skipped gated live-provider tests.

## Implementation Notes
- Prefer SQL Server Always Encrypted as the first proof because the upstream configuration ticket already established the provider-owned selection entry point there.
- Keep fallback tests deterministic and local where possible; live database coverage is additive and must not be required for normal CI or bot execution.
- Use existing diagnostics/support-bundle redaction conventions rather than introducing a separate reporting lane.
- If implementation discovers that actual native runtime execution needs a larger provider-specific design, keep this ticket focused on proof/fail-closed coverage and create a follow-up instead of widening the shared privacy package.

## Open Questions
- none

## Follow-Up Candidates
- Add a provider-specific runtime execution proof for one exact SQL Server Always Encrypted shape if this ticket determines that the current provider seam is only diagnostic/proof-level.
- Add equivalent provider-owned proof tickets for PostgreSQL, Oracle, MySQL, SQLite, or DB2 after the SQL Server pattern is accepted.

## Risks
- Over-scoping into a shared cross-provider native runtime would violate the privacy boundary and make future provider behavior ambiguous.
- A silent downgrade from requested native behavior to plaintext or implicit custom behavior would violate the fail-closed privacy posture.
- Live-provider tests can make the bot brittle if they are not strictly gated and deterministic when credentials or provider setup are absent.
<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add a bounded provider-native crypto usage proof for the most feasible provider path plus fallback and unsupported-provider tests for the rest. Use live-provider tests only behind DVAULT_TEST_* gates. Prove that native selection is explicit, custom implementations still work, unsupported capabilities fail closed, and diagnostics remain redacted.