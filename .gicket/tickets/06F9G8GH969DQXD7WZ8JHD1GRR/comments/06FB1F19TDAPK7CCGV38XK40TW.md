[gicket-bot] PO refinement contract

Summary
- Refined the DB2 provider support epic as an already-split tracking parent: repository evidence and completed child tickets cover the contract, package, schema/guardrails, integration, package-verification, and documentation lanes, and no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Existing epic children already cover the full bounded DB2 slice: 06F9G8GS08VNH0DT09Q4PC2HRC (contract), 06F9G8GZ384VKA7RVF039WKX1M (provider package), 06F9G8H5HE1CJHQXGC2C2YK7P8 (schema and live-schema guardrails), 06F9G8HBXS7Y42J7XFSQKZ2AZ8 (save/read integration), 06F9G8HJJDJH4KF9VK6TZ8B1Z0 (package verification), and 06F9G8HRZ72XP5Z7FNWM6MBMQC (documentation).
- Repository evidence now includes the concrete DB2 package surface, AddDVaultDb2 registration, explicit DB2 live-schema unsupported handling, opt-in DB2 save/read smoke coverage, and v0.34.0 DB2 release documentation, so the epic no longer needs another scope split.
- Direct read of child ticket 06F9G8GS08VNH0DT09Q4PC2HRC exceeded the local result-byte cap in this slice, but multiple completed child contracts consistently cite it as the authoritative DB2 baseline and the repository state matches that baseline.
- No bounded writes were applied in this refinement run: no new child tickets, relation updates, description updates, attachments, or planning documents.

Scope In
- Track the bounded DB2 provider-support slice across provider package registration, provider capability and schema guardrails, provider-neutral save/read compatibility evidence, package verification, and release/documentation alignment.
- Use DCoding.Data.DVault.Db2 plus AddDVaultDb2 as the explicit consumer-facing DB2 entry point for the DVault package family.
- Keep DB2 validation opt-in and externally provisioned through DVAULT_TEST_DB2_CONNECTION_STRING rather than making DB2 part of the default local repository baseline.

Scope Out
- DB2 provisioning, deployment orchestration, container recipes, CI-owned DB2 infrastructure, credentials, or default local DB2 requirements.
- Provider-native DB2 save/read optimization, DB2 live-schema reader implementation, or platform/tooling work beyond the bounded provider-support lanes already planned.
- Broader provider-matrix expansion, DB2-specific performance claims, or SQL-artifact automation beyond the documented v1 boundaries.

Open questions
- none

Follow-up questions
- none

Risks
- The live relation graph still contains an incoming blocks edge from done documentation ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC, so tracker closure automation may need relation cleanup even though scope evidence is complete.
- Direct read of ticket 06F9G8GS08VNH0DT09Q4PC2HRC exceeded the local result-byte cap in this slice, so this refinement relies on corroborating completed child contracts and repository state for that authoritative DB2 baseline.

Split recommendations
- No additional split recommended; the epic already has six child lanes covering contract, package, schema/guardrails, integration, verification, and documentation, and repository evidence shows those lanes are complete.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment