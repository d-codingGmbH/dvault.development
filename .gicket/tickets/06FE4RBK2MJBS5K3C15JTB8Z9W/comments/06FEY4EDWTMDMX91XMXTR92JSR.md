[gicket-bot] PO refinement contract

Summary
- Refined this as one bounded docs/story ticket: add a discoverable practical example for the shipped `DCoding.Data.DVault.Privacy` proof APIs and align the surrounding docs with the existing optional-privacy, fail-closed, caller-owned-key boundary. No child-ticket split is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 scope is documentation and example work around the already-shipped privacy proof surface, not new privacy runtime architecture. Use the existing public APIs `AddDVaultPrivacy(...)`, `DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(...)`, `DataVaultPrivacyOptions.UseCallerOwnedKeyProvider(...)`, `IDataVaultEncryptedPayloadKeyProvider`, and `DataVaultEncryptedPayloadValueConverter` as the example baseline.
- Ratify the local SQLite-friendly proof as the practical example default. The repository already uses SQLite as the default runnable baseline, and the privacy converter round-trip proof already exists in tests; other providers can be documented as bounded compatibility notes rather than separate runnable samples.
- Use GDPR/DSGVO wording only as project-context language. The docs must explicitly say the privacy package does not make an application GDPR/DSGVO compliant, does not provide legal advice, and does not certify erasure, retention, or governance workflows.
- For this ticket, crypto-shredding means caller-owned withdrawal, loss, or destruction of key material for an `encryptedPayloadAlias`. The documented DVault behavior after that event is explicit fail-closed conversion/read failure, not automatic deletion, purge, PIT or bridge cleanup, backup purge, or legal-erasure completion.

Scope In
- Add one practical code example that shows opt-in privacy registration, encrypted-payload alias registration, a caller-owned key provider, and `DataVaultEncryptedPayloadValueConverter` applied to a personal-data payload property.
- Make the privacy example discoverable from the current onboarding surfaces such as `README.md`, `docs/getting-started.md`, `examples/README.md`, or an equivalent linked privacy document.
- Document the limitations, provider caveats, caller responsibilities, and fail-closed behavior of the optional privacy proof in language consistent with `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md`, `README.md`, and `docs/production-adoption-checklist.md`.
- Explain how the example's alias-driven runtime seam relates to the existing `personalData[].encryptedPayloadAlias` model-first terminology without introducing new metadata APIs or new default behavior.

Scope Out
- New automatic encryption, redaction, pseudonymization, deletion, or `SaveChanges` interception behavior in core DVault.
- Provider-native encryption integrations, provider-specific encryption DDL, separate MariaDB or other new provider profiles, or new provider capability guarantees beyond the current finite baseline.
- Compliance guarantees, legal guidance, retention execution, purge orchestration, backup management, key management infrastructure, KMS or HSM integration, or DVault-owned crypto-shredding workflows.
- New code-first, registry, or model-first privacy metadata authoring features beyond documenting the existing alias semantics and current proof APIs.

Open questions
- none

Follow-up questions
- Should a later ticket add a dedicated runnable privacy quickstart project once the team wants more than a bounded sample section and test-backed proof?
- Should a later provider-specific ticket document one named provider-native encryption lane only after that provider has explicit implementation evidence, diagnostics, and fallback rules?
- Should a later privacy metadata ticket add first-class code-first or registry authoring helpers for personal-data metadata so the runtime example and model-first example converge on one higher-level authoring story?

Risks
- This area is easy to over-document. If the example or prose drifts from the boundary doc, adopters may wrongly infer GDPR/DSGVO compliance or provider-native encryption guarantees.
- A demonstration key provider can be misread as production security guidance unless the docs explicitly mark it as illustrative caller-owned code.
- If provider wording is too broad, readers may infer support for providers, storage types, or encryption capabilities outside the current finite built-in baseline.

Split recommendations
- No split recommended; the repository already contains the privacy proof APIs, architecture boundary, and tests, so one bounded example plus documentation alignment fits a single ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment