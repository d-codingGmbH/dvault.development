[gicket-bot] PO refinement contract

Summary
- Refined the ticket against current repository evidence and ratified the v1 outcome: DVault exposes an opt-in, provider-neutral privacy seam with alias-driven caller-owned encrypted-payload conversion, while provider-native encryption remains an unmanaged guidance-only boundary.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 default: treat this ticket as defining the provider-native encryption boundary and privacy contract, not as approving a shared runtime native-encryption implementation.
- The authoritative model/schema handoff is additive `personalData[].encryptedPayloadAlias` metadata on satellite payload fields; it is descriptive only and does not imply provider columns, store types, SQL, algorithms, keys, migrations, or encrypted DDL.
- The approved shared runtime lane is explicit opt-in privacy activation through `AddDVaultPrivacy(...)` plus alias-driven caller-owned encrypted-payload conversion such as `DataVaultEncryptedPayloadValueConverter`; existing `AddDVault()`, save/read services, PIT/bridge maintenance, hashing, and telemetry stay unchanged unless the caller opts in.
- Caller-owned custom encrypted payload providers are preserved through the existing `IDataVaultEncryptedPayloadKeyProvider` request/result seam with explicit `Encrypt` and `Decrypt` directions.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run because repository evidence was already sufficient for bounded refinement.

Scope In
- Define the finite provider baseline named in repository docs and diagnostics: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2, with MySQL kept as the repository MySQL profile rather than a separate MariaDB capability profile.
- Define the shared provider-neutral crypto/privacy contract around encrypted-payload aliases, explicit conversion directions, caller-owned key-provider resolution, and fail-closed behavior for missing alias/provider approval or unsupported coverage.
- Define bounded schema and modeling implications: personal-data markers apply only to existing satellite payload fields and preserve current Data Vault table kinds, row-history semantics, driving-key semantics, load timestamp, record source, and ordinary EF payload mapping.
- Define redaction-safe diagnostics and coverage facts for provider-native encryption boundary status, key-provider posture, alias coverage, personal-data coverage, and unsupported or fallback states.

Scope Out
- Implementing provider-native encrypted DDL, provider SQL crypto functions, provider capability probing, provider key-store integration, or runtime dispatch based on native encryption availability.
- Claiming GDPR or DSGVO compliance, compliance attestation, legal guidance, or turning DVault into a governance, retention, or subject-rights workflow platform.
- Owning key creation, storage, escrow, rotation, destruction timing, alias-to-key registry, HSM or KMS integration, or any other key lifecycle workflow.
- Automatic `SaveChanges` interception, hidden background privacy work, automatic crypto-shredding execution, row deletion, PIT/bridge cleanup, backup purge, or migration execution.
- Adding provider-specific schema sections, migration manifests for native encryption, or new first-class Data Vault entity families for privacy state.

Open questions
- none

Follow-up questions
- Which exact provider-native capability, if any, should be pursued first as a later provider-specific opt-in ticket: SQL Server Always Encrypted, PostgreSQL `pgcrypto`, Oracle `DBMS_CRYPTO`, MySQL SQL crypto, SQLite encrypted-file integration, or DB2 native encryption?
- Does a later ticket need an additional explicit read or save helper API beyond the current value-converter and diagnostics proof, or is the existing opt-in seam sufficient for the first consumer rollout?

Risks
- The ticket title can invite over-scoping; without the documented boundary, implementers may incorrectly treat it as approval for shared provider-native encryption support.
- Documentation drift between architecture, release notes, and diagnostics could reintroduce accidental compliance or key-lifecycle claims even though the repository baseline rejects them.
- A future provider-specific optimization could accidentally bypass fail-closed behavior or alias-driven ownership unless it stays behind a separate provider ticket with its own diagnostics and evidence.

Split recommendations
- Keep any future provider-native encryption work split into one provider and one exact capability per ticket, with its own provider package surface, fallback rules, tests, and evidence.
- If broader privacy workflow APIs are still desired, split them from this contract ticket into separate explicit capabilities such as read-helper redaction, pseudonymization, or retention metadata review rather than widening the shared v1 boundary.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment