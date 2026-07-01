[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded v1 contract: the optional privacy package stays provider-neutral, privacy diagnostics expose a finite provider-native crypto guidance matrix, and SQL Server Always Encrypted is the only explicit provider-owned native-selection path. No blocking PO questions remain, and no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- V1 scope is additive to the optional privacy extension boundary: callers must explicitly opt into AddDVaultPrivacy(...), keep encrypted-payload aliases stable, and keep key-provider or converter ownership in the application.
- Provider-native crypto remains diagnostics and review evidence only. DVault does not emit encrypted DDL, call provider SQL crypto functions, probe live database encryption state, or route shared runtime behavior based on provider-native capability availability.
- The reviewed provider baseline is finite and repository-backed: SQLite encrypted-file build is unsupported; PostgreSQL deployment encryption and pgcrypto, SQL Server TDE and Always Encrypted, MySQL SQL crypto functions and file or tablespace encryption, Oracle TDE and DBMS_CRYPTO, and DB2 native database encryption are conditional guidance facts.
- AddDVaultSqlServerAlwaysEncryptedSelection(...) is the only v1 explicit provider-owned native crypto selection API. It is alias-driven, opt-in, and must fail closed when prerequisite proof names are missing, reviewed capability facts are unavailable or unsupported, or the active capability profile is not SQL Server.
- No bounded ticket writes were applied or queued in this run.

Scope In
- Privacy diagnostics expose ProviderNativeEncryption and a finite ProviderCryptoCapabilities matrix for the visible provider capability profiles.
- The optional privacy package remains the opt-in entry point for encrypted-payload alias registration and caller-owned key-provider posture.
- SQL Server can register one explicit Always Encrypted selection per encrypted-payload alias through a provider package extension that contributes redaction-safe ProviderNativeCryptoSelections facts.
- Fail-closed validation issues are produced for incompatible profiles, missing prerequisite proof names, unavailable reviewed capability facts, and unsupported reviewed capability facts.
- Documentation, public API, and test coverage keep provider-native selection bounded to provider packages and preserve the existing provider-neutral runtime conversion path.

Scope Out
- Automatic encryption, pseudonymization, redaction, deletion, retention, or GDPR/DSGVO compliance execution.
- Provider-native encrypted DDL generation or provider SQL crypto execution such as pgcrypto, MySQL crypto functions, or DBMS_CRYPTO.
- Live database capability probing, key-store provisioning, driver or enclave setup, or runtime dispatch that switches behavior based on provider-native capability availability.
- Re-encryption, backfill, dual-write, provider migration, historical rewrite, backup purge, crypto-shredding, or key lifecycle ownership.
- New non-SQL Server provider-specific native selection APIs or managed native crypto execution behavior.

Open questions
- none

Follow-up questions
- Which provider, if any, should own the next explicit provider-native crypto selection ticket after SQL Server, and what exact capability family would that ticket claim?
- Should future provider-specific work stay diagnostics-only like the current SQL Server selection surface, or is there appetite for a separately governed runtime integration design later?
- Does product want a follow-up adopter guide or sample focused on reviewing ProviderNativeEncryption, ProviderCryptoCapabilities, and ProviderNativeCryptoSelections during privacy adoption?

Risks
- The current story title can be read as multi-provider native crypto implementation work; without the bounded v1 clarification, delivery could drift into unapproved provider-runtime behavior.
- Consumers may overread reviewed capability facts or SQL Server selection diagnostics as compliance or managed encryption automation unless the non-goals remain explicit in docs and validation output.
- Any future provider profile additions or status changes must update the capability catalog, docs, and tests together or the finite reviewed baseline will become inconsistent.

Split recommendations
- No mandatory split is needed if this story is explicitly bounded to diagnostics guidance plus the SQL Server Always Encrypted selection surface.
- If broader provider-native behavior is later desired, create separate provider-owned follow-up tickets per capability family, such as PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL SQL crypto functions, deployment-at-rest guidance hardening, or any managed runtime integration design.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment