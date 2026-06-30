[gicket-bot] PO refinement contract

Summary
- Repository evidence already ratifies a bounded v1 provider-native crypto story: a finite reviewed capability matrix, one explicit SQL Server Always Encrypted selection seam, and a caller-owned converter path that remains the shared runtime default. This ticket is ready for PO-critic as a documentation-alignment task across the named public docs; no child split or planning document is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repo-backed capability matrix is already finite and should be documented as the v1 default: SQLite encrypted-file build is `unsupported`; PostgreSQL deployment encryption and `pgcrypto`, SQL Server TDE and Always Encrypted, MySQL SQL crypto functions and file-or-tablespace encryption, Oracle TDE and `DBMS_CRYPTO`, and DB2 native database encryption are `conditional` guidance facts.
- The only current explicit provider-owned native selection surface in the repository is SQL Server `AddDVaultSqlServerAlwaysEncryptedSelection(...)`; it emits redaction-safe `ProviderNativeCryptoSelections` facts and fails closed when prerequisite proof names, capability facts, or the active capability profile do not line up.
- The shared runtime privacy lane remains caller-owned alias registration plus `DataVaultEncryptedPayloadValueConverter`; provider-native selection does not replace custom conversion, auto-route provider behavior, or enable provider-native execution by default.
- This ticket remains the documentation half that supports story `06FH8RFJYY09BJJK4MD2KT8BF0`; the implementation proof ticket `06FH8RMFZSVNW0KKTZT9HMGM8G` is already done and provides the concrete repo evidence the docs should ratify.

Scope In
- Document the finite reviewed provider-native crypto capability matrix and clearly distinguish guidance-only capability facts from DVault-managed runtime behavior.
- Document the current SQL Server Always Encrypted opt-in selection path, including caller-owned prerequisite proof names, redaction-safe diagnostics and support-bundle visibility, and fail-closed rejection behavior.
- Document coexistence with the existing caller-owned encrypted-payload alias and value-converter path, including that custom implementations remain first-class and are not silently replaced.
- Document adoption and migration caveats for moving toward or away from provider-native usage proofs: no automatic re-encryption, backfill, dual-write, provider migration, key-store setup, or provider provisioning.
- Align README, Getting Started, Production Adoption Checklist, Package Compatibility, and current release-note or changelog language around the same ownership boundary, crypto-shredding limit, deletion and backup-purge non-goals, and finite provider baseline.

Scope Out
- Adding new provider-native runtime execution beyond the existing SQL Server proof-level selection seam.
- Adding cross-provider native crypto dispatch, capability probing, encrypted DDL generation, SQL crypto invocation, or provider-name auto-routing in shared code.
- Owning compliance, legal erasure, deletion, retention, backup purge or shredding, key lifecycle, KMS or HSM integration, or provider and database provisioning workflows.
- Documenting MariaDB as a separate capability profile or widening the supported-provider baseline beyond SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.

Open questions
- none

Follow-up questions
- If a later provider-specific ticket adds actual runtime execution for SQL Server Always Encrypted or another provider, should adopter guidance split into provider-specific docs instead of one shared bounded matrix?
- Should a later release add one dedicated privacy diagnostics page that consolidates alias coverage, provider capability facts, native-selection facts, and support-bundle review workflow for adopters?

Risks
- If docs overstate `conditional` capabilities as implemented runtime behavior, adopters may infer unsupported DDL, probing, or key-management behavior.
- If docs omit coexistence and migration caveats, adopters may expect automatic cutover from custom conversion to native crypto and make unsafe rollout assumptions.
- If docs do not restate non-goals around deletion, backup purge or shredding, and provider provisioning, privacy language could drift into compliance promises the codebase explicitly rejects.

Split recommendations
- No split is recommended; the current scope is already bounded to documentation alignment around the existing provider matrix and the single SQL Server proof-level selection seam.

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