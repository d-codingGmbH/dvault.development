[gicket-bot] PO-critic review contract

Summary
- Delivery contract is specific, source-backed, and has no unresolved Open Questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- src/DCoding.Data.DVault.Privacy/DVaultPrivacyServiceCollectionExtensions.cs directly defines AddDVaultPrivacy(this IServiceCollection, Action<DataVaultPrivacyOptions>) and calls services.AddDVault(), matching the ticket's claimed opt-in startup seam.
- src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs directly defines RegisterEncryptedPayloadAlias(string) and UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider), and its Apply(...) path registers IDataVaultEncryptedPayloadKeyProvider in DI when the supplied provider implements that interface.
- src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs directly rejects unregistered aliases, missing key-provider wiring, providers that do not implement IDataVaultEncryptedPayloadKeyProvider, and declined conversions, which matches the ticket's fail-closed acceptance language.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs covers SQLite round-trip persistence plus unregistered-alias, missing-key-provider, and declined-conversion fail-closed cases named in the delivery contract.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs covers duplicate alias rejection and DI exposure of IDataVaultEncryptedPayloadKeyProvider when the caller-owned provider implements it.
- gicket-read-ticket-comments returned recent PO and runtime comments showing the earlier PO-critic blocker was stale and that the current branch was re-verified against the shipped privacy proof APIs; no newer comment reopens the contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The ticket assumes discoverability is satisfied by updating one first-pass onboarding surface plus cross-links, rather than requiring a dedicated runnable privacy quickstart project in this story.
- The ticket assumes existing unit tests are sufficient Definition-of-Done evidence for the documented pattern, so a new runnable sample project is optional future work rather than current-scope validation.

AC / test suggestions
- Keep at least one acceptance/test check tied to the existing SQLite round-trip and fail-closed negative cases already covered by DataVaultEncryptedPayloadValueConverterTests.
- Add a docs review check that every example uses the exact shipped API names and avoids claims of GDPR/DSGVO compliance, automatic encryption/redaction, or provider-native encryption.

Implementation watchouts
- Do not show converter usage with a provider type that only implements IDataVaultPrivacyKeyProvider; the documented converter path must either use a provider that also implements IDataVaultEncryptedPayloadKeyProvider or explicitly explain the runtime failure.
- Keep alias wording anchored to the existing personalData[].encryptedPayloadAlias term and avoid inventing new metadata APIs or authoring flows.
- Keep crypto-shredding language limited to caller-owned key withdrawal or destruction causing fail-closed reads and writes, not deletion, retention completion, backup purge, PIT or bridge cleanup, historical rewrite, or legal-erasure completion.

Non-blocking notes
- The Follow-Up Questions section contains future-ticket ideas, but it does not block approval because the Open Questions section is explicitly none.

Split recommendations
- No split recommended; the scope is a bounded docs/example pass over already-shipped privacy APIs and existing test-backed proof paths.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment