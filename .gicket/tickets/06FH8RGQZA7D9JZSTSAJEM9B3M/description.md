<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Resolved the PO-critic finding by correcting the provider-native boundary diagnostics contract to the existing core public type DataVaultProviderNativeEncryptionBoundaryFact at src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs and aligning the acceptance and DoD wording with that source-backed boundary.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence fixes the provider-native boundary diagnostics fact contract to the existing core public type DataVaultProviderNativeEncryptionBoundaryFact in src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs, not a privacy-package file.
- The shared v1 result remains an opt-in provider-neutral privacy seam plus unmanaged provider-native boundary guidance; this ticket does not approve shared runtime native encryption behavior.
- No child tickets, relation changes, attachments, or planning documents were materialized because the prompt evidence already resolved the blocking citation issue.

### Scope In
- Define the finite v1 provider baseline for provider-native boundary facts: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Define the shared provider-neutral privacy seam around personalData[].encryptedPayloadAlias, explicit Encrypt/Decrypt conversion direction, caller-owned key-provider resolution, and fail-closed behavior.
- Define redaction-safe diagnostics and coverage facts, including provider-native boundary facts via DataVaultProviderNativeEncryptionBoundaryFact and alias, key-provider, and personal-data coverage reporting.
- Keep schema and modeling impact bounded to existing satellite payload fields and current Data Vault table families.

### Scope Out
- Implementing provider-native encrypted DDL, provider SQL crypto functions, capability probing, runtime dispatch, or provider key-store integration.
- Claiming GDPR/DSGVO compliance or making DVault the owner of key lifecycle, governance, or legal policy.
- Automatic SaveChanges interception, background privacy work, crypto-shredding, row deletion, PIT or bridge cleanup, or migration execution.
- Adding provider-specific schema sections, native-encryption migration manifests, or new first-class Data Vault entity families.

## Acceptance Criteria
- The contract states that v1 provider-native encryption is unmanaged and guidance-only in the shared DVault surface, and that DVault does not probe encryption settings, emit encrypted DDL, call provider SQL crypto functions, or branch on native encryption availability.
- The contract identifies the shared capability family as alias-driven encrypted-payload conversion with caller-owned Encrypt and Decrypt operations resolved by encryptedPayloadAlias through IDataVaultEncryptedPayloadKeyProvider.
- The contract preserves fail-closed behavior for missing alias registration, missing or marker-only key providers, declined conversions, unsupported providers or shapes, and missing observable converter coverage.
- The contract records personalData[].encryptedPayloadAlias as the only v1 schema and model handoff point and keeps it descriptive rather than a promise of provider storage shape, SQL, migration, or DDL behavior.
- The contract names DataVaultProviderNativeEncryptionBoundaryFact as the current source-backed provider-native boundary fact carrier and requires redaction-safe diagnostics and coverage reporting for boundary status, key-provider posture, alias coverage, and personal-data coverage.
- Docs and implementation notes preserve the current non-goals: no compliance claim, no DVault-owned key lifecycle, no shared provider-native encryption runtime feature, and no automatic data-lifecycle workflows.

## Definition of Done
- Architecture and release-note surfaces align on the same bounded v1 decision: opt-in provider-neutral privacy seam, caller-owned key lifecycle, guidance-only provider-native encryption, and the finite supported-provider baseline.
- Reviewed core and privacy contracts expose the public seam named by this ticket: DataVaultEncryptedPayloadConversionDirection, IDataVaultEncryptedPayloadKeyProvider, DataVaultEncryptedPayloadValueConverter, and DataVaultProviderNativeEncryptionBoundaryFact.
- Tests or equivalent checked-in evidence cover fail-closed states and coverage reporting for missing aliases, unusable key-provider posture, declined conversions, proof-missing versus unusable coverage, and provider-native boundary facts.
- No remaining blocker reopens the provider baseline, ownership boundary, diagnostics carrier, or privacy activation posture before PO-critic review.

## Implementation Notes
- Repository evidence anchors this boundary in docs/architecture/dvault-v1-optional-privacy-extension-boundary.md, docs/plans/dvault-model-v1-schema-contract.md, docs/releases/v0.44.0.md, docs/releases/v0.50.0.md, and src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs.
- Current code already supplies the bounded public seam: DataVaultEncryptedPayloadConversionDirection for explicit Encrypt/Decrypt requests, IDataVaultEncryptedPayloadKeyProvider for caller-owned conversion, DataVaultEncryptedPayloadValueConverter for alias registration and fail-closed conversion, and DataVaultProviderNativeEncryptionBoundaryFact in src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs for provider-native boundary facts.
- Use the core DataVaultProviderNativeEncryptionBoundaryFact type as present-state evidence; do not cite src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeEncryptionBoundaryFact.cs, because that file does not exist in the provided repository context.
- The branch snapshot marks personalData[].encryptedPayloadAlias as missing, so treat that metadata key as the approved contract target for downstream work rather than as already-landed branch code.
- Docs/releases/v0.50.0.md places these boundary facts in the redacted DataVaultPrivacyDiagnostics surface; keep that diagnostics lane evidence-only and do not treat it as approval for a shared cross-provider native crypto feature.

## Open Questions
- none

## Follow-Up Questions
- Which provider-native capability, if any, should be the first separate provider-specific opt-in ticket: SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL SQL crypto, SQLite encrypted-file integration, or DB2 native encryption?
- Does a later ticket need an explicit read or save helper API beyond the current value-converter and diagnostics proof, or is the existing opt-in seam sufficient for the first adopter rollout?

## Risks
- The ticket title can still invite over-scoping unless implementers treat DataVaultProviderNativeEncryptionBoundaryFact as evidence-only and keep provider-native work split into separate provider tickets.
- Documentation drift between architecture, release notes, and code-surface naming could reintroduce the old incorrect privacy-package path or imply managed native encryption support.
- A future provider optimization could bypass alias ownership or fail-closed posture if it is added outside a separate provider-scoped capability ticket.

## Split Recommendations
- Keep any future provider-native encryption work split to one provider and one exact capability per ticket, with its own provider package surface, fallback rules, tests, and evidence.
- Split broader privacy workflow APIs such as read-helper redaction, pseudonymization flows, or retention metadata review into separate tickets instead of widening this contract.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Design the provider-native crypto capability model. Cover provider capability facts, function families, encryption/decryption direction, key ownership, migrations/schema implications, unsupported/conditional states, diagnostics, and non-goals. Preserve existing custom encrypted payload providers and avoid claiming DSGVO/GDPR compliance or managed key lifecycle.