<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence shows DVault already ships the provider-neutral custom privacy seam and static provider crypto capability facts; this ticket should be refined to the explicit selection contract that preserves the custom path, keeps native selection provider-package-owned, and leaves provider-specific execution to downstream work. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The existing custom implementation baseline is already visible in the repo through AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter.
- Provider-native capability discovery is already a separate completed diagnostics slice via DataVaultProviderCryptoCapabilityCatalog and related done ticket 06FH8RJF2SYBJ8ZM7ZDETDPN78; this ticket should consume that fact model rather than redefine capability reporting.
- The checked-in privacy boundary and done architecture ticket 06FH8RGQZA7D9JZSTSAJEM9B3M do not allow a shared cross-provider native-encryption runtime lane; any native selection must stay explicit and provider-package-owned.
- Live relation state was reviewed and left unchanged; no durable refinement writes were materialized during this run.

### Scope In
- Define the consumer-facing configuration contract that preserves the current caller-owned encrypted-payload path as the default opt-in privacy behavior.
- Define how a future provider package may expose one exact provider-native capability selection without introducing shared provider-name branching in DCoding.Data.DVault or DCoding.Data.DVault.Privacy.
- Require fail-closed behavior when a requested native capability is unsupported, unavailable, incompatible with the active provider/profile/shape, or missing required caller-owned prerequisites.
- Reuse the existing static provider crypto capability facts and redacted privacy diagnostics as the capability evidence lane for any explicit native-selection request.
- Keep the selection contract compatible with alias-driven personal-data metadata and ordinary EF Core mapped-property/value-converter constraints.

### Scope Out
- Implementing provider-native crypto runtime behavior, encrypted DDL, provider SQL crypto calls, capability probing, or key-store integration in this ticket.
- Adding a shared cross-provider native-selection API that auto-negotiates behavior from provider names or live environment checks.
- Silently falling back from an explicitly requested native capability to plaintext persistence, implicit provider behavior, or unmanaged automatic custom/native routing.
- Claiming GDPR/DSGVO compliance, DVault-owned key lifecycle, crypto-shredding workflows, retention workflows, or deletion workflows.
- Removing or weakening the existing caller-owned custom encrypted-payload path.

## Acceptance Criteria
- The refinement contract ratifies the existing AddDVaultPrivacy(...) plus UseCallerOwnedKeyProvider(...) path as the bounded v1 default when no provider-native option is explicitly selected.
- Any provider-native selection is explicit, opt-in, and owned by the matching provider package for one exact reviewed capability; the shared privacy package must not auto-select native behavior from provider identity alone.
- When a caller explicitly requests a native capability that is unsupported or unavailable for the active provider/profile/shape, the flow fails closed with redacted diagnostics and never silently persists plaintext or silently downgrades to implicit behavior.
- The selection contract remains alias-driven and EF Core compatible by building on encryptedPayloadAlias, IDataVaultEncryptedPayloadKeyProvider, and ordinary mapped-property/value-converter constraints rather than new provider-specific metadata fields in the shared model.
- The contract consumes the existing static capability-fact lane for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 and does not require live capability probing by default.
- Provider-native execution proof and fallback tests remain downstream implementation work in ticket 06FH8RMFZSVNW0KKTZT9HMGM8G rather than being absorbed into this configuration-contract ticket.

## Definition of Done
- The ticket-level contract clearly distinguishes the shipped caller-owned custom path from any future provider-specific native opt-in path and aligns with the checked-in privacy boundary documents and done predecessor tickets.
- The refined contract makes the API placement decision explicit: provider-specific native selection belongs in matching provider-package extension methods or provider-owned seams, not in implicit shared dispatch.
- The refined contract preserves the current non-goals: no shared managed native-encryption runtime, no provider-name branching, no live probing by default, and no DVault-owned key lifecycle or compliance workflow.
- A developer can implement the next proof slice without reopening PO decisions about ownership boundary, fail-closed behavior, diagnostics input, or EF Core compatibility.

## Implementation Notes
- DataVaultPrivacyOptions already owns alias registration and caller-owned key-provider wiring; keep that as the baseline custom lane instead of replacing it with a new generic strategy system.
- DataVaultEncryptedPayloadValueConverter already demonstrates the required fail-closed posture for missing aliases, missing providers, marker-only providers, and declined conversions; native-selection behavior must preserve that same failure posture.
- DataVaultProviderCryptoCapabilityCatalog already exposes the reviewed static capability matrix; use it as discovery and diagnostics input rather than as proof that runtime native execution is already supported.
- The checked-in privacy boundary document explicitly keeps provider-native encryption guidance-only in the shared surface until a separate provider-specific ticket owns one exact capability; this ticket should preserve that constraint.
- The least-surprising bounded shape is provider-specific opt-in registration layered beside AddDVaultPrivacy(...), while shared privacy options continue to own encrypted-payload aliases and caller-owned custom provider wiring.
- Current live relation context already places story 06FH8RFJYY09BJJK4MD2KT8BF0 as the parent and task 06FH8RMFZSVNW0KKTZT9HMGM8G as downstream proof work; no relation cleanup was applied in this run.

## Open Questions
- none

## Follow-Up Questions
- Which exact provider/capability should the first provider-specific proof ticket target: SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL SQL crypto, SQLite encrypted-file integration, or DB2 native encryption?
- After the first provider-specific proof lands, should a later docs task publish a consumer-facing matrix that distinguishes guidance-only capability facts from runtime-supported explicit native selections?
- If a future provider-specific native lane needs startup or preflight validation beyond the current static capability facts, should that become a separate opt-in diagnostics ticket rather than widening this configuration contract?

## Risks
- The current ticket title and short draft description can invite over-scoping into a shared cross-provider native runtime feature unless implementers follow the provider-package boundary documented in the repo.
- A silent downgrade from an explicitly requested native capability to some other behavior would violate the existing fail-closed privacy posture and create user-visible ambiguity.
- If future provider-specific APIs drift away from the reviewed capability-fact matrix, diagnostics, documentation, and runtime behavior could diverge.
- Because capability-reporting work is already done, teams may incorrectly assume native execution support already exists unless this ticket keeps discovery/reporting clearly separate from execution/configuration.

## Split Recommendations
- Keep provider-native execution split to one provider and one exact capability per ticket; let 06FH8RMFZSVNW0KKTZT9HMGM8G own the first bounded proof plus fallback tests.
- Keep documentation rollout or consumer guidance updates separate from this configuration-contract ticket instead of widening the current scope.
- If future work needs environment probing, key-store validation, or secret-handling review, split that into a separate opt-in diagnostics/preflight ticket rather than expanding this selection contract.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add an opt-in privacy configuration surface that lets consumers keep custom encrypted payload implementations or select a provider-native crypto capability when available. The API must fail closed when requested native support is unavailable, keep custom implementations supported, avoid implicit provider behavior, and remain compatible with EF Core mapping constraints.

<!-- gicket-bot:developer-delivery-supplement:v1:start -->
## Developer Delivery Supplement

### Implementation Summary
- Moved the consumer-facing provider-native selection surface out of `DataVaultPrivacyOptions` and into the SQL Server provider package via `AddDVaultSqlServerAlwaysEncryptedSelection(...)`.
- Kept `DCoding.Data.DVault.Privacy` focused on provider-neutral alias registration and caller-owned key-provider wiring; the shared package no longer exposes a native-selection record, options property, or registration method.
- Added provider-owned diagnostics for SQL Server Always Encrypted selections, including fail-closed handling for missing caller-owned prerequisite proof names without introducing provider-name auto-dispatch, native runtime encryption, DDL, SQL crypto calls, or live probing.

### Verification
- `dotnet build DVault.slnx --nologo` passed with 0 errors.
- `DVAULT_UPDATE_API_SNAPSHOTS=1 dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests` passed for the full unit suite because the Microsoft Testing Platform ignored the VSTest filter: net8.0 667 passed, net10.0 735 passed.
- `bash tools/check-format.sh` passed.

### Notes
- Provider-native execution proof and fallback behavior remain downstream work for ticket `06FH8RMFZSVNW0KKTZT9HMGM8G`.
- The build still reports existing NuGet/auditing/analyzer warnings; none failed this rework.
<!-- gicket-bot:developer-delivery-supplement:v1:end -->