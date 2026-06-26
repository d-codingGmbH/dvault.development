[gicket-bot] PO refinement contract

Summary
- Refined this checklist child ticket against current repo and live ticket evidence: the alias-coverage report, `personalData` diagnostic behavior, and provider-native privacy caveats are already settled in done prerequisite tickets, so this work is now a bounded documentation update to `docs/production-adoption-checklist.md`. No child tickets, relation changes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 privacy baseline: `DCoding.Data.DVault.Privacy` is optional, provider-neutral, alias-driven EF Core value conversion over ordinary mapped payload properties, not a GDPR/DSGVO guarantee, automatic privacy workflow, provider-native encryption lane, or key-management platform.
- Done prerequisite tickets `06FF43M7AE9DN3K1YXBPB1R574`, `06FF43MQ3AXXK2S5TK65X4Y9S8`, and `06FF43PCN26C70DXX326B9VYA4` settle the alias-coverage report, `personalData` diagnostic behavior, and provider-native caveat baseline for this checklist ticket.
- The current repository now provides a model-only, redaction-safe alias coverage report through `DataVaultPrivacyCoverageReporter.Analyze(...)` with alias statuses `covered` and `registered-but-unmapped` plus key-provider posture classification.
- The current repository also fixes `personalData` semantics: it is additive metadata on satellite payload fields only, keyed by `personalData[].encryptedPayloadAlias`, and it does not by itself create encryption, DDL, provider behavior, key ids, or compliance effects.
- Live relation state is coherent: this ticket is a child of story `06FF43K0B0MJF45078STZ3H6DC`; its incoming blocker tickets are now done; it still blocks downstream release-doc ticket `06FF43WMMC8R3T4ZKVR4312NJC`; no relation cleanup was materialized in this refinement pass.

Scope In
- Update `docs/production-adoption-checklist.md` with explicit privacy preflight steps for projects that opt into `DCoding.Data.DVault.Privacy`.
- Tell adopters to review alias registration and field-level mapping coverage using the existing privacy coverage report, including `registered-but-unmapped` outcomes and the report's key-provider posture.
- Tell adopters how to interpret `personalData` markers during preflight: marked fields must be real satellite payload fields with stable aliases, unmarked payloads remain ordinary, and the metadata is descriptive rather than self-executing.
- Document caller-owned key-provider responsibilities for encrypted payload conversion, including the narrower `IDataVaultEncryptedPayloadKeyProvider` requirement when field-level conversion is used.
- Keep checklist wording explicit about advisory versus fail-closed outcomes when privacy proof or usable alias/converter coverage is missing.

Scope Out
- New privacy runtime behavior, new diagnostics, new coverage-report functionality, or new `personalData` model semantics.
- Provider-native encryption implementation or promises for SQL Server Always Encrypted or TDE, PostgreSQL `pgcrypto`, Oracle `DBMS_CRYPTO` or TDE, MySQL or Pomelo SQL or file or tablespace encryption, SQLite encrypted-file builds, or DB2 native database encryption.
- Claims that DVault or the checklist establishes GDPR/DSGVO compliance, legal erasure completion, retention ownership, or automated crypto-shredding workflows.
- Release-note, README, changelog, manual-publication, or broader package-doc alignment beyond this checklist ticket; downstream release-doc ticket `06FF43WMMC8R3T4ZKVR4312NJC` remains separate.

Open questions
- none

Follow-up questions
- When downstream ticket `06FF43WMMC8R3T4ZKVR4312NJC` resumes, should release-note and README wording reuse the checklist's privacy preflight bullets verbatim or just link to the checklist?
- Do adopters need one small docs or example snippet that shows `DataVaultPrivacyCoverageReport.ToDisplayString()` output, or is checklist guidance plus current API docs enough?
- Should a later docs ticket show advisory `personal-data-privacy-proof-missing` and fail-closed `personal-data-privacy-coverage-unusable` examples side by side?

Risks
- If the checklist collapses advisory and fail-closed cases into one vague warning, adopters may misread optional `personalData` metadata as automatic encryption or miss required converter and key-provider wiring.
- If the checklist enumerates provider-native encryption examples without repeating the guidance-only boundary, readers may treat them as supported runtime capabilities.
- If crypto-shredding wording is loose, readers may infer DVault-owned deletion, backup purge, or compliance completion that the privacy boundary explicitly disclaims.
- Because this ticket still blocks `06FF43WMMC8R3T4ZKVR4312NJC`, delay here cascades into broader v0.48 documentation alignment.

Split recommendations
- No split recommended; repository evidence already bounds this to one checklist-documentation slice, while broader release-doc alignment stays in `06FF43WMMC8R3T4ZKVR4312NJC`.
- Do not widen this ticket into runtime privacy features, examples, or additional public-doc surfaces unless a separate follow-up ticket is created.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment