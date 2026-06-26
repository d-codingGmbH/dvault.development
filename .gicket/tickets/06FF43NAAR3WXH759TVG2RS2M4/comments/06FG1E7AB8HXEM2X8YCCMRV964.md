[gicket-bot] PO-critic review contract

Summary
- Delivery contract is clear, scoped to existing privacy test surfaces, and supported by visible source/test evidence; ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Branch context identifies `ticket/06FF43NAAR3WXH759TVG2RS2M4-task-extend-privacy-diagnostics-and-converter-te` at head `51c71bddee9262c6282226988a1a86d5e74d4c6b` with a clean worktree.
- Referenced source `src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs` directly shows fail-closed checks for unregistered alias, missing key provider, marker-only provider, declined conversion, and a dedicated null-result branch that throws when `ConvertEncryptedPayload(...)` returns no result.
- Referenced source `src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs` directly contains `EvaluatePersonalDataCoverage(...)` and `EvaluatePersonalDataConverterCoverage(...)`, including no-proof warning, proof-null/proof-exception handling, and field-level `DataVaultEncryptedPayloadValueConverter` alias matching.
- Referenced tests `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyCoverageReporterTests.cs` already cover deterministic covered vs registered-but-unmapped alias display and none/marker-only/encrypted-payload-capable key-provider postures without conversion calls.
- Referenced tests `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs` already cover unregistered alias, missing provider, marker-only provider, declined conversion, and redaction-safe exception assertions.
- Referenced tests `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs` already cover `personal-data-privacy-proof-missing`, unusable privacy coverage without encrypted-payload provider, metadata-only missing converter coverage, and DbContext success when `SatCustomerProfile.EmailAddress` is wired to `DataVaultEncryptedPayloadValueConverter`.
- Referenced document `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` states privacy behavior is opt-in and must not change `AddDVault()` defaults, matching the ticket scope-out.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Explicit unit coverage for a key provider returning null from `ConvertEncryptedPayload(...)` remains part of the residual gap called out in the contract.
- Explicit diagnostics coverage for `IDataVaultPersonalDataCoverageProof.EvaluateEncryptedPayloadAlias(...)` returning null and throwing supported exceptions is the other visible residual gap called out in the contract.

Risky assumptions
- The ticket assumes the remaining uncovered branches can be closed without widening the privacy API surface; if a new failing test reveals a defect, any fix must stay within the already named production seams.

AC / test suggestions
- Keep diagnostics assertions focused on issue code, severity, and redaction-safe fragments rather than full-message snapshots, as the contract already instructs.
- Use the existing test files instead of adding parallel test layouts so the residual branches stay tied to the visible source seams already named in the contract.

Implementation watchouts
- Preserve fail-closed and redaction-safe behavior in `DataVaultEncryptedPayloadValueConverter`; exception messages must not echo plaintext payloads.
- Preserve the opt-in privacy boundary from `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md`; no `AddDVault()` default behavior changes are in scope.
- Any production fix exposed by the new tests should remain limited to `src/DCoding.Data.DVault.Privacy` or `src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs`, matching the contract.

Non-blocking notes
- The contract is already refined enough for developer handoff because it names concrete existing files, visible source seams, acceptance outcomes, and scope boundaries without unresolved open questions.
- Most requested behaviors are already present in the repository, so the work is a narrow gap-closing task rather than a broad privacy feature introduction.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment