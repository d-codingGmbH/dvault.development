[gicket-bot] PO refinement contract

Summary
- Verified the current-branch privacy proof APIs and tightened the ticket contract to the shipped opt-in alias-driven conversion surface; no split or related-ticket write was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Fresh branch inspection confirms the ticket can cite shipped public APIs instead of inferred placeholders. The current branch exposes AddDVaultPrivacy(...), DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(string), DataVaultPrivacyOptions.UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter, so the contract stays documentation/example-only over existing runtime surface.
- critic-item-2: `answered` - The blocking finding is resolved by current-branch evidence. The public API snapshot, source files, and existing repo docs all show that the opt-in privacy proof surface already ships on this branch, so the delivery contract should describe documentation and example work against those existing APIs rather than imply new privacy runtime types must be created.
- critic-item-3: `answered` - Use precise runtime wording in the contract: the example should call AddDVaultPrivacy(...), register one or more encryptedPayloadAlias values through DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(...), pass a caller-owned provider through UseCallerOwnedKeyProvider(...), ensure that provider implements IDataVaultEncryptedPayloadKeyProvider for encrypted conversion, and apply DataVaultEncryptedPayloadValueConverter to the chosen payload property. Model-first docs should keep personalData[].encryptedPayloadAlias as the metadata term.

Clarifications
- The current branch already ships the public privacy proof surface in src/DCoding.Data.DVault.Privacy: AddDVaultPrivacy(...), DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(string), UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter.
- The docs/example should show a caller-owned provider passed through UseCallerOwnedKeyProvider(...); for encrypted value conversion that provider must implement IDataVaultEncryptedPayloadKeyProvider, because the converter rejects a provider that does not satisfy that interface.
- The model-first alias term remains personalData[].encryptedPayloadAlias; the runtime example should explain alias registration as the explicit opt-in seam for that logical alias instead of inventing new metadata APIs.
- The privacy package remains opt-in and provider-neutral: it adds registration, options, and alias-driven encrypted payload conversion proof only, not compliance guarantees, automatic encryption/redaction, or provider-native encryption.
- Crypto-shredding wording stays caller-owned: loss, withdrawal, or destruction of key material for an encryptedPayloadAlias makes reads or writes fail closed; it does not imply deletion, backup purge, retention completion, or legal-erasure completion.

Scope In
- Add one practical docs/example path that uses the shipped privacy proof surface: AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter.
- Make the privacy example discoverable from README, docs/getting-started.md, examples/README.md, or an equivalent first-pass onboarding surface.
- Explain how runtime alias registration maps to the existing personalData[].encryptedPayloadAlias model-first term without introducing new metadata APIs.
- Document fail-closed behavior, caller ownership, provider caveats, and non-goals in language aligned with the privacy boundary and production checklist.

Scope Out
- No new privacy runtime architecture, public APIs, or metadata authoring features beyond documenting the shipped opt-in proof surface.
- No automatic encryption, redaction, pseudonymization, deletion, SaveChanges interception, or hidden background privacy workflows in core DVault.
- No provider-native encryption integrations, provider-specific encryption DDL, or support claims beyond the bounded current proof baseline.
- No compliance guarantees, legal advice, key-management platform work, retention orchestration, backup purge, or DVault-owned crypto-shredding lifecycle.

Open questions
- none

Follow-up questions
- Should a later ticket add a dedicated runnable privacy quickstart project once the team wants more than a bounded sample section and test-backed proof?
- Should a later provider-specific ticket document one named provider-native encryption lane only after that provider has explicit implementation evidence, diagnostics, and fallback rules?
- Should a later privacy metadata ticket add first-class code-first or registry authoring helpers so the runtime example and model-first example converge on one higher-level authoring story?

Risks
- If the docs blur the difference between UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider) and the converter's runtime requirement for IDataVaultEncryptedPayloadKeyProvider, adopters may wire a provider that compiles but still fails at runtime.
- If example prose drifts from the privacy boundary, readers may infer GDPR/DSGVO compliance or provider-native encryption guarantees that the package does not make.
- A toy key-provider example can be mistaken for production cryptography guidance unless it is explicitly labeled as caller-owned demo code only.

Split recommendations
- No split recommended; the current branch already ships the public privacy proof APIs, boundary docs, and test-backed example pattern, so one bounded docs/example ticket remains appropriate.

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