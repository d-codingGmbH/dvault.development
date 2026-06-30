[gicket-bot] PO refinement contract

Summary
- Repository evidence shows DVault already ships the provider-neutral custom privacy seam and static provider crypto capability facts; this ticket should be refined to the explicit selection contract that preserves the custom path, keeps native selection provider-package-owned, and leaves provider-specific execution to downstream work. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The existing custom implementation baseline is already visible in the repo through AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter.
- Provider-native capability discovery is already a separate completed diagnostics slice via DataVaultProviderCryptoCapabilityCatalog and related done ticket 06FH8RJF2SYBJ8ZM7ZDETDPN78; this ticket should consume that fact model rather than redefine capability reporting.
- The checked-in privacy boundary and done architecture ticket 06FH8RGQZA7D9JZSTSAJEM9B3M do not allow a shared cross-provider native-encryption runtime lane; any native selection must stay explicit and provider-package-owned.
- Live relation state was reviewed and left unchanged; no durable refinement writes were materialized during this run.

Scope In
- Define the consumer-facing configuration contract that preserves the current caller-owned encrypted-payload path as the default opt-in privacy behavior.
- Define how a future provider package may expose one exact provider-native capability selection without introducing shared provider-name branching in DCoding.Data.DVault or DCoding.Data.DVault.Privacy.
- Require fail-closed behavior when a requested native capability is unsupported, unavailable, incompatible with the active provider/profile/shape, or missing required caller-owned prerequisites.
- Reuse the existing static provider crypto capability facts and redacted privacy diagnostics as the capability evidence lane for any explicit native-selection request.
- Keep the selection contract compatible with alias-driven personal-data metadata and ordinary EF Core mapped-property/value-converter constraints.

Scope Out
- Implementing provider-native crypto runtime behavior, encrypted DDL, provider SQL crypto calls, capability probing, or key-store integration in this ticket.
- Adding a shared cross-provider native-selection API that auto-negotiates behavior from provider names or live environment checks.
- Silently falling back from an explicitly requested native capability to plaintext persistence, implicit provider behavior, or unmanaged automatic custom/native routing.
- Claiming GDPR/DSGVO compliance, DVault-owned key lifecycle, crypto-shredding workflows, retention workflows, or deletion workflows.
- Removing or weakening the existing caller-owned custom encrypted-payload path.

Open questions
- none

Follow-up questions
- Which exact provider/capability should the first provider-specific proof ticket target: SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL SQL crypto, SQLite encrypted-file integration, or DB2 native encryption?
- After the first provider-specific proof lands, should a later docs task publish a consumer-facing matrix that distinguishes guidance-only capability facts from runtime-supported explicit native selections?
- If a future provider-specific native lane needs startup or preflight validation beyond the current static capability facts, should that become a separate opt-in diagnostics ticket rather than widening this configuration contract?

Risks
- The current ticket title and short draft description can invite over-scoping into a shared cross-provider native runtime feature unless implementers follow the provider-package boundary documented in the repo.
- A silent downgrade from an explicitly requested native capability to some other behavior would violate the existing fail-closed privacy posture and create user-visible ambiguity.
- If future provider-specific APIs drift away from the reviewed capability-fact matrix, diagnostics, documentation, and runtime behavior could diverge.
- Because capability-reporting work is already done, teams may incorrectly assume native execution support already exists unless this ticket keeps discovery/reporting clearly separate from execution/configuration.

Split recommendations
- Keep provider-native execution split to one provider and one exact capability per ticket; let 06FH8RMFZSVNW0KKTZT9HMGM8G own the first bounded proof plus fallback tests.
- Keep documentation rollout or consumer guidance updates separate from this configuration-contract ticket instead of widening the current scope.
- If future work needs environment probing, key-store validation, or secret-handling review, split that into a separate opt-in diagnostics/preflight ticket rather than expanding this selection contract.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment