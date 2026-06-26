<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh repository inspection shows this is a small, test-only refinement around the existing privacy coverage reporter, encrypted-payload converter, and personalData diagnostics surfaces; most named cases already exist, so the remaining work is to close the visible unit-test gaps without changing runtime defaults.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Fresh inspection was taken from the repository only; no ticket writes, relation changes, or planning documents were materialized in this run.
- Treat missing aliases as the bounded existing surfaces already visible in code: unregistered converter aliases in DataVaultEncryptedPayloadValueConverter and registered-but-unmapped aliases in DataVaultPrivacyCoverageReporter.
- Treat marker-only provider as any IDataVaultPrivacyKeyProvider that is not also an IDataVaultEncryptedPayloadKeyProvider; that is the current fail-closed baseline in both converter and diagnostics code.
- Treat null conversion output as a key provider returning null from ConvertEncryptedPayload(...); approved null payloads are already excluded by DataVaultEncryptedPayloadConversionResult.Approved(string).

### Scope In
- Extend unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyCoverageReporterTests.cs for deterministic alias coverage reporting and key-provider posture output.
- Extend unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs for fail-closed converter behavior, including declined and null-result conversions.
- Extend unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs for personal-data diagnostics that flow from alias proof evaluation and EF converter wiring.

### Scope Out
- No new privacy API surface, no change to AddDVault() or opt-in privacy activation defaults, and no change to the library boundary documented in docs/architecture/dvault-v1-optional-privacy-extension-boundary.md.
- No provider-specific integration, migration, or benchmark work; this ticket stays in unit tests and narrowly scoped diagnostics/converter fixes only if new tests expose a defect.
- No support-bundle, release-note, or broader documentation updates.

## Acceptance Criteria
- The unit suite covers deterministic privacy coverage reporter output for covered and registered-but-unmapped aliases, and it verifies none, marker-only, and encrypted-payload-capable key-provider postures without invoking conversion calls.
- The converter unit suite proves fail-closed behavior for unregistered aliases, missing key providers, marker-only providers, declined conversions, and null/no-result conversions, with exception messages that remain redaction-safe and do not echo plaintext payloads.
- The diagnostics unit suite proves personal-data-privacy-proof-missing stays a warning when no privacy proof is configured, and personal-data-privacy-coverage-unusable is raised when alias registration, key-provider posture, proof evaluation, or field-level converter coverage is unusable.
- A DbContext-backed diagnostics case continues to pass only when the marked payload field is wired to DataVaultEncryptedPayloadValueConverter for the same encrypted-payload alias, preserving current fail-closed behavior.

## Definition of Done
- Relevant tests are added or updated in the existing unit-test files under tests/DCoding.Data.DVault.Tests/Unit rather than creating a new parallel test layout.
- Any production-code changes stay limited to src/DCoding.Data.DVault.Privacy and src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs, and only when required to satisfy a new failing test while preserving fail-closed semantics.
- Touched unit tests pass for the privacy reporter, converter, and diagnostics surfaces.

## Implementation Notes
- Current repository evidence already covers several named cases: stable alias coverage display and registered-but-unmapped aliases in DataVaultPrivacyCoverageReporterTests, unregistered/missing-provider/marker-only/declined converter behavior in DataVaultEncryptedPayloadValueConverterTests, and advisory/fail-closed/success personalData diagnostics in DataVaultDiagnosticsTests.
- The visible residual gaps are the explicit null-result converter branch in src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs and untested personal-data proof failure/null-evaluation branches in src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs.
- Prefer exact-string assertions only for the deterministic DataVaultPrivacyCoverageReport.ToDisplayString() contract; for diagnostics exceptions/issues, assert code, severity, and key redaction-safe fragments instead of full-message snapshots.

## Open Questions
- none

## Follow-Up Questions
- After the unit gaps are closed, decide whether any of these privacy scenarios also deserve integration coverage beyond the existing SQLite round-trip proof.
- Consider later whether privacy coverage reporter output should surface through preflight or support-bundle tooling, but that is not required for this ticket.

## Risks
- Most of the ticket's named behaviors are already present in the repository, so the implementation should avoid churn or broader behavior changes and focus on the remaining uncovered fail-closed branches.
- If new tests expose a defect, the fix must preserve redaction safety and the opt-in privacy boundary instead of silently relaxing failure behavior.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Extend unit coverage for alias coverage output, missing aliases, marker-only providers, declined conversions, null conversion output, and personalData diagnostic cases. Acceptance: tests preserve existing fail-closed behavior.