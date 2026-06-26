[gicket-bot] PO refinement contract

Summary
- Fresh repository inspection shows this is a small, test-only refinement around the existing privacy coverage reporter, encrypted-payload converter, and personalData diagnostics surfaces; most named cases already exist, so the remaining work is to close the visible unit-test gaps without changing runtime defaults.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Fresh inspection was taken from the repository only; no ticket writes, relation changes, or planning documents were materialized in this run.
- Treat missing aliases as the bounded existing surfaces already visible in code: unregistered converter aliases in DataVaultEncryptedPayloadValueConverter and registered-but-unmapped aliases in DataVaultPrivacyCoverageReporter.
- Treat marker-only provider as any IDataVaultPrivacyKeyProvider that is not also an IDataVaultEncryptedPayloadKeyProvider; that is the current fail-closed baseline in both converter and diagnostics code.
- Treat null conversion output as a key provider returning null from ConvertEncryptedPayload(...); approved null payloads are already excluded by DataVaultEncryptedPayloadConversionResult.Approved(string).

Scope In
- Extend unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyCoverageReporterTests.cs for deterministic alias coverage reporting and key-provider posture output.
- Extend unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs for fail-closed converter behavior, including declined and null-result conversions.
- Extend unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs for personal-data diagnostics that flow from alias proof evaluation and EF converter wiring.

Scope Out
- No new privacy API surface, no change to AddDVault() or opt-in privacy activation defaults, and no change to the library boundary documented in docs/architecture/dvault-v1-optional-privacy-extension-boundary.md.
- No provider-specific integration, migration, or benchmark work; this ticket stays in unit tests and narrowly scoped diagnostics/converter fixes only if new tests expose a defect.
- No support-bundle, release-note, or broader documentation updates.

Open questions
- none

Follow-up questions
- After the unit gaps are closed, decide whether any of these privacy scenarios also deserve integration coverage beyond the existing SQLite round-trip proof.
- Consider later whether privacy coverage reporter output should surface through preflight or support-bundle tooling, but that is not required for this ticket.

Risks
- Most of the ticket's named behaviors are already present in the repository, so the implementation should avoid churn or broader behavior changes and focus on the remaining uncovered fail-closed branches.
- If new tests expose a defect, the fix must preserve redaction safety and the opt-in privacy boundary instead of silently relaxing failure behavior.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment