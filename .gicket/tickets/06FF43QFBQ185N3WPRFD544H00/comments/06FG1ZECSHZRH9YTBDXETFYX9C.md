[gicket-bot] PO-critic review contract

Summary
- Persisted contract is specific, repository-backed, and has no open questions; the ticket is ready for developer handoff for the checklist-only update.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF43QFBQ185N3WPRFD544H00/description.md contains the persisted Delivery Contract with scoped work, 6 acceptance criteria, 4 Definition of Done bullets, 8 implementation notes, and Open Questions -> none.
- git show --name-only --format=medium HEAD shows head bc23a9ad0e1788bee81e3bc321a6a339fb8d9e20 is a po-critic lease-claim commit touching only .gicket/tickets/06FF43QFBQ185N3WPRFD544H00/*; no docs/ files are part of that commit.
- A direct rg search for DataVaultPrivacyCoverageReporter, personalData, personal-data-privacy-proof-missing, and IDataVaultEncryptedPayloadKeyProvider in docs/production-adoption-checklist.md returned no matches, and the file headings are still the broad baseline sections rather than a privacy-preflight subsection.
- src/DCoding.Data.DVault.Privacy/DataVaultPrivacyCoverageReporter.cs exposes DataVaultPrivacyCoverageReporter.Analyze(...) for both DbContext and IModel, and src/DCoding.Data.DVault.Privacy/DataVaultPrivacyCoverageReport.cs emits alias statuses covered and registered-but-unmapped plus key-provider postures none, marker-only, and encrypted-payload-capable.
- src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs emits advisory personal-data-privacy-proof-missing when no privacy proof is configured and fail-closed personal-data-privacy-coverage-unusable when proof exists but usable coverage is missing.
- src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs registers UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider), while src/DCoding.Data.DVault.Privacy/IDataVaultEncryptedPayloadKeyProvider.cs and docs/getting-started.md confirm field-level encrypted payload conversion requires the narrower IDataVaultEncryptedPayloadKeyProvider capability.
- docs/package-compatibility.md, docs/getting-started.md, and docs/architecture/dvault-v1-optional-privacy-extension-boundary.md all align on the privacy package being optional/provider-neutral and not a GDPR/DSGVO guarantee, automatic encryption/redaction feature, or provider-native encryption lane.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A later docs follow-up could add one small DataVaultPrivacyCoverageReport.ToDisplayString() example if adopter feedback shows the checklist bullets alone are too abstract.
- A later docs follow-up could show advisory personal-data-privacy-proof-missing and fail-closed personal-data-privacy-coverage-unusable side by side, but the current contract is already sufficient for developer handoff.

Risky assumptions
- Implementation stays scoped to docs/production-adoption-checklist.md and does not expand into README, release notes, or new runtime behavior.
- Downstream release-doc ticket 06FF43WMMC8R3T4ZKVR4312NJC will reuse the settled vocabulary instead of reopening privacy semantics.

AC / test suggestions
- During doc review, check the final wording against the exact repository names DataVaultPrivacyCoverageReporter.Analyze(...), registered-but-unmapped, personal-data-privacy-proof-missing, personal-data-privacy-coverage-unusable, IDataVaultPrivacyKeyProvider, and IDataVaultEncryptedPayloadKeyProvider to avoid naming drift.
- Verify the final checklist explicitly tells adopters when metadata-only review is insufficient and a configured DbContext or EF model is needed to observe converter coverage.

Implementation watchouts
- Keep the advisory vs fail-closed split explicit; do not collapse optional personalData metadata and proof-enabled unusable coverage into one vague warning.
- Do not imply that metadata-only artifact review proves field-level converter wiring; converter coverage is observed from a configured DbContext or EF model.
- Keep provider-native caveats finite and guidance-only; do not imply DVault emits encrypted DDL, calls provider SQL crypto functions, or routes behavior by native encryption availability.
- If crypto-shredding is mentioned, describe only caller-owned key withdrawal or loss making payloads undecryptable, not deletion, backup purge, retention completion, or legal attestation.

Non-blocking notes
- Current branch head bc23a9ad0e1788bee81e3bc321a6a339fb8d9e20 contains only ticket and lease metadata changes, so the developer will still need to make the actual checklist edit.
- The ticket still blocks downstream release-doc ticket 06FF43WMMC8R3T4ZKVR4312NJC, which increases delivery pressure but does not make the contract unclear.

Split recommendations
- No split recommended; the work remains one bounded checklist-documentation slice in docs/production-adoption-checklist.md.
- Do not widen this ticket into runtime privacy features, new diagnostics, README or release-note alignment, or extra public-doc surfaces without a separate follow-up ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment