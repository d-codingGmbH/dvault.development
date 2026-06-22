<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the current-branch privacy proof APIs and tightened the ticket contract to the shipped opt-in alias-driven conversion surface; no split or related-ticket write was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current branch already ships the public privacy proof surface in src/DCoding.Data.DVault.Privacy: AddDVaultPrivacy(...), DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(string), UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter.
- The docs/example should show a caller-owned provider passed through UseCallerOwnedKeyProvider(...); for encrypted value conversion that provider must implement IDataVaultEncryptedPayloadKeyProvider, because the converter rejects a provider that does not satisfy that interface.
- The model-first alias term remains personalData[].encryptedPayloadAlias; the runtime example should explain alias registration as the explicit opt-in seam for that logical alias instead of inventing new metadata APIs.
- The privacy package remains opt-in and provider-neutral: it adds registration, options, and alias-driven encrypted payload conversion proof only, not compliance guarantees, automatic encryption/redaction, or provider-native encryption.
- Crypto-shredding wording stays caller-owned: loss, withdrawal, or destruction of key material for an encryptedPayloadAlias makes reads or writes fail closed; it does not imply deletion, backup purge, retention completion, or legal-erasure completion.

### Scope In
- Add one practical docs/example path that uses the shipped privacy proof surface: AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter.
- Make the privacy example discoverable from README, docs/getting-started.md, examples/README.md, or an equivalent first-pass onboarding surface.
- Explain how runtime alias registration maps to the existing personalData[].encryptedPayloadAlias model-first term without introducing new metadata APIs.
- Document fail-closed behavior, caller ownership, provider caveats, and non-goals in language aligned with the privacy boundary and production checklist.

### Scope Out
- No new privacy runtime architecture, public APIs, or metadata authoring features beyond documenting the shipped opt-in proof surface.
- No automatic encryption, redaction, pseudonymization, deletion, SaveChanges interception, or hidden background privacy workflows in core DVault.
- No provider-native encryption integrations, provider-specific encryption DDL, or support claims beyond the bounded current proof baseline.
- No compliance guarantees, legal advice, key-management platform work, retention orchestration, backup purge, or DVault-owned crypto-shredding lifecycle.

## Acceptance Criteria
- Repository docs contain one practical privacy proof example that calls AddDVaultPrivacy(...), registers at least one encrypted payload alias with RegisterEncryptedPayloadAlias(...), passes a caller-owned provider through UseCallerOwnedKeyProvider(...), and applies DataVaultEncryptedPayloadValueConverter to a payload property.
- The example and surrounding guidance make the type boundary explicit: UseCallerOwnedKeyProvider(...) accepts IDataVaultPrivacyKeyProvider, but encrypted payload conversion requires a provider implementation that also satisfies IDataVaultEncryptedPayloadKeyProvider.
- The documentation states that missing alias registration, missing key-provider wiring, or a declined caller-owned conversion fail closed and do not silently store plaintext or silently treat ciphertext as decrypted payload data.
- The documentation states that DCoding.Data.DVault.Privacy is an optional provider-neutral proof package, not a GDPR/DSGVO compliance guarantee, not automatic encryption or redaction, and not a provider-native encryption feature.
- The documentation explains crypto-shredding as caller-owned key withdrawal or destruction for an encryptedPayloadAlias and explicitly excludes row deletion, historical rewrite, PIT or bridge cleanup, backup purge, retention completion, and legal-erasure completion.
- Provider caveats remain bounded: the example reuses the SQLite-friendly proof path or another clearly bounded provider-neutral path and does not imply broader provider-native support.

## Definition of Done
- The chosen docs/example surfaces are updated and cross-linked so an adopter can discover the privacy proof from the current README or first-pass onboarding path.
- Sample code and prose use the exact shipped API names and current signatures on this branch and remain aligned with the privacy boundary, production checklist, and package compatibility wording already in the repository.
- The example is validated by compiling or running the checked-in sample surface, or by current tests that prove alias registration, DI wiring, SQLite round-trip, and fail-closed behavior for the documented pattern.
- No new documentation claims GDPR/DSGVO compliance, provider-native encryption, automatic deletion, automatic redaction, implicit background privacy workflows, or DVault-owned key lifecycle behavior.

## Implementation Notes
- Reuse the existing proof pattern in tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs for the sample shape: SQLite round-trip, alias-based converter wiring, and fail-closed cases already exist.
- Reuse tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs for registration facts: AddDVaultPrivacy(...) registers core defaults and privacy configuration, rejects duplicate aliases, and exposes IDataVaultEncryptedPayloadKeyProvider from DI when the supplied provider implements it.
- Keep wording precise around the current signatures: UseCallerOwnedKeyProvider(...) takes IDataVaultPrivacyKeyProvider, while DataVaultEncryptedPayloadValueConverter requires configuration.KeyProvider to be an IDataVaultEncryptedPayloadKeyProvider at runtime.
- Keep the example provider-neutral and ordinary-payload-based; the converter stores caller-prepared encrypted text through ordinary mapping and should not imply special encrypted column types or provider-specific SQL.
- No child-ticket split, attachment write, relation change, or related-ticket description update was needed for this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add a dedicated runnable privacy quickstart project once the team wants more than a bounded sample section and test-backed proof?
- Should a later provider-specific ticket document one named provider-native encryption lane only after that provider has explicit implementation evidence, diagnostics, and fallback rules?
- Should a later privacy metadata ticket add first-class code-first or registry authoring helpers so the runtime example and model-first example converge on one higher-level authoring story?

## Risks
- If the docs blur the difference between UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider) and the converter's runtime requirement for IDataVaultEncryptedPayloadKeyProvider, adopters may wire a provider that compiles but still fails at runtime.
- If example prose drifts from the privacy boundary, readers may infer GDPR/DSGVO compliance or provider-native encryption guarantees that the package does not make.
- A toy key-provider example can be mistaken for production cryptography guidance unless it is explicitly labeled as caller-owned demo code only.

## Split Recommendations
- No split recommended; the current branch already ships the public privacy proof APIs, boundary docs, and test-backed example pattern, so one bounded docs/example ticket remains appropriate.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: add a practical privacy extension example and docs for encrypted personal-data attributes and crypto-shredding posture. Acceptance: docs state limitations, DSGVO wording, provider caveats, and caller responsibilities.