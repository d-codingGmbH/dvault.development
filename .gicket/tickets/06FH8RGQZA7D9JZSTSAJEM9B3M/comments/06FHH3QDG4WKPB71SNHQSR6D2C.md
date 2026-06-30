[gicket-bot] PO refinement contract

Summary
- Resolved the PO-critic finding by correcting the provider-native boundary diagnostics contract to the existing core public type DataVaultProviderNativeEncryptionBoundaryFact at src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs and aligning the acceptance and DoD wording with that source-backed boundary.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now treats the provider-native boundary diagnostics surface as present-state evidence backed by the existing core file src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs, and the acceptance and DoD wording are aligned to that source instead of a nonexistent privacy-package path.
- critic-item-2: `answered` - The contract now names the actual source-backed public boundary-fact contract as DataVaultProviderNativeEncryptionBoundaryFact. The release-note baseline in docs/releases/v0.50.0.md continues to place those facts in the redacted DataVaultPrivacyDiagnostics surface, so developers no longer have to infer the target from prose.
- critic-item-3: `answered` - The earlier citation to src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeEncryptionBoundaryFact.cs was incorrect. Repository evidence shows the existing public type in src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs, so the contract is restated against that core-package path and keeps native encryption diagnostic-only and unmanaged.

Clarifications
- Repository evidence fixes the provider-native boundary diagnostics fact contract to the existing core public type DataVaultProviderNativeEncryptionBoundaryFact in src/DCoding.Data.DVault/DataVaultProviderNativeEncryptionBoundaryFact.cs, not a privacy-package file.
- The shared v1 result remains an opt-in provider-neutral privacy seam plus unmanaged provider-native boundary guidance; this ticket does not approve shared runtime native encryption behavior.
- No child tickets, relation changes, attachments, or planning documents were materialized because the prompt evidence already resolved the blocking citation issue.

Scope In
- Define the finite v1 provider baseline for provider-native boundary facts: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Define the shared provider-neutral privacy seam around personalData[].encryptedPayloadAlias, explicit Encrypt/Decrypt conversion direction, caller-owned key-provider resolution, and fail-closed behavior.
- Define redaction-safe diagnostics and coverage facts, including provider-native boundary facts via DataVaultProviderNativeEncryptionBoundaryFact and alias, key-provider, and personal-data coverage reporting.
- Keep schema and modeling impact bounded to existing satellite payload fields and current Data Vault table families.

Scope Out
- Implementing provider-native encrypted DDL, provider SQL crypto functions, capability probing, runtime dispatch, or provider key-store integration.
- Claiming GDPR/DSGVO compliance or making DVault the owner of key lifecycle, governance, or legal policy.
- Automatic SaveChanges interception, background privacy work, crypto-shredding, row deletion, PIT or bridge cleanup, or migration execution.
- Adding provider-specific schema sections, native-encryption migration manifests, or new first-class Data Vault entity families.

Open questions
- none

Follow-up questions
- Which provider-native capability, if any, should be the first separate provider-specific opt-in ticket: SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL SQL crypto, SQLite encrypted-file integration, or DB2 native encryption?
- Does a later ticket need an explicit read or save helper API beyond the current value-converter and diagnostics proof, or is the existing opt-in seam sufficient for the first adopter rollout?

Risks
- The ticket title can still invite over-scoping unless implementers treat DataVaultProviderNativeEncryptionBoundaryFact as evidence-only and keep provider-native work split into separate provider tickets.
- Documentation drift between architecture, release notes, and code-surface naming could reintroduce the old incorrect privacy-package path or imply managed native encryption support.
- A future provider optimization could bypass alias ownership or fail-closed posture if it is added outside a separate provider-scoped capability ticket.

Split recommendations
- Keep any future provider-native encryption work split to one provider and one exact capability per ticket, with its own provider package surface, fallback rules, tests, and evidence.
- Split broader privacy workflow APIs such as read-helper redaction, pseudonymization flows, or retention metadata review into separate tickets instead of widening this contract.

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