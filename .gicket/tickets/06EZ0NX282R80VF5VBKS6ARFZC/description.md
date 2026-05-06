<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ticket 06EZ0NX282R80VF5VBKS6ARFZC is already a bounded child of 06EZ0NWKC9ZME5BSCJFSQEQ02R, and repository evidence supports refining it as a provider-behavior hook task with default inheritance plus explicit provider-package override registration; no child tickets, relation changes, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The only persisted comments are bot claim/lease entries; there are no human comments adding scope or conflicting decisions.
- The ticket already has one incoming `parentOf` relation from 06EZ0NWKC9ZME5BSCJFSQEQ02R, so this task remains part of the advanced-hooks decomposition and does not need further split materialization.
- For this ticket, default inheritance means the provider-behavior hook remains optional and inherits the existing core baseline when unset; it must not make normal `AddDVault()` setup require extra configuration.
- Current repository evidence already establishes the visible baseline: `AddDVault(Action<DataVaultOptions>)` is the optional advanced DI surface, `DataVaultProviderCapabilityProfileSelection` provides capability fallback, and `IDataVaultProviderSaveStrategy` plus the core save service already use explicit registration with provider-neutral fallback.

### Scope In
- Expose one provider-behavior hook contract that deferred capabilities can consume instead of hard-coding provider names or provider-specific branches in core capability code.
- Keep the hook optional and default-inheriting so an unset provider-behavior hook preserves the current baseline behavior.
- Allow provider packages to supply explicit provider-behavior overrides through the existing registration style used for provider packages and optional DVault services.
- Add regression coverage proving that missing or incompatible provider overrides leave existing behavior unchanged, and that explicitly registered overrides are selected only when their registration path is in place.

### Scope Out
- Do not implement PIT, bridge, multi-active, or other deferred capability families in this ticket.
- Do not redefine naming, hashing, record source, or timestamp semantics through the provider-behavior hook; those remain separate hook categories.
- Do not make advanced provider configuration required for ordinary `AddDVault()` or `ApplyDataVaultMetadata()` usage.
- Do not use this ticket to introduce broad provider-specific option matrices, migrations, DDL policy, or a wider provider release posture rewrite.
- Do not broaden provider-name capability-profile commitments beyond the registration/profile behavior already visible in the repository unless that is strictly required for the bounded hook surface itself.

## Acceptance Criteria
- Core DVault code exposes a provider-behavior hook surface that deferred capabilities can call without hard-coding provider details in core.
- When no provider-behavior override is registered, the hook inherits the existing default baseline and does not change current observable behavior for model translation or save-path fallback.
- Provider packages can register an explicit provider-behavior override, and that override is isolated to provider behavior rather than silently changing naming, hashing, record source, or timestamp behavior.
- Tests prove that absent overrides preserve the existing baseline and that explicit provider registrations are the only path that changes provider behavior for this hook.
- If the implementation introduces or changes public API, the public API snapshot coverage is updated to reflect the approved hook surface.

## Definition of Done
- The hook surface, default implementation, and provider override registration path are implemented in the bounded advanced-hook scope.
- Unit or integration tests cover default inheritance, explicit override selection, and unchanged fallback behavior when no override applies.
- The implementation preserves zero-configuration startup for callers that continue to use the current default path.
- Any new public surface is represented in the approved API snapshot tests and any required code-level documentation reflects the default-inheriting behavior.

## Implementation Notes
- Use the existing optional-configuration pattern as the baseline: `DataVaultOptions` already carries category-specific resolver overrides, so provider behavior should follow the same optional category model instead of creating a separate required startup path.
- Use the existing provider boundary as the architectural guardrail: `IDataVaultProviderSaveStrategy` and the core save dispatcher already prove explicit registration plus fallback, and `DataVaultProviderCapabilityProfileSelection` already proves deterministic default selection behavior.
- Keep the hook category-scoped. The provider-behavior hook may wrap or replace provider behavior for deferred capabilities, but it must not become a back door for naming, hashing, record source, or timestamp changes.
- Current visible provider-profile evidence is intentionally narrower than the save-strategy surface: SQLite and MySQL startup extensions auto-register provider-name capability profiles, Oracle has a visible profile but no startup auto-registration, and Postgres/SqlServer currently prove save-strategy registration without visible provider capability profiles. The new hook must not assume a broader profile-registration matrix than that.
- Regression coverage should mirror the existing fallback/selection style already exercised in `DataVaultSaveStrategySelectionTests`, `MySqlProviderCapabilityTests`, and the API snapshot tests.

## Open Questions
- none

## Follow-Up Questions
- After the generic provider-behavior hook boundary lands, should its first public surface be documented as stable immediately or treated as experimental for one release cycle?
- Which provider ecosystems, if any, should receive deeper provider-specific option matrices after this generic provider-behavior boundary exists?

## Risks
- If implementation assumes every provider package already auto-registers provider capability profiles, it will overstate the current baseline and may misroute Postgres or SQL Server behavior.
- If the default inheritance path changes the current fallback selection semantics, existing `AddDVault()` model-translation annotations or provider-neutral save behavior could regress even when no override is configured.
- If this ticket expands into concrete provider option matrices or release-posture commitments, it will reopen scope that the advanced-hooks planning docs explicitly keep deferred.

## Split Recommendations
- No split recommended; the task is already bounded to the provider-behavior hook surface, default inheritance, explicit provider registration, and regression coverage under parent 06EZ0NWKC9ZME5BSCJFSQEQ02R.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: expose a provider behavior hook surface that deferred capabilities can use without hard-coding provider details in core.

Acceptance Criteria:
- Provider behavior hooks can inherit defaults from the provider-neutral baseline.
- Provider packages can override behavior through explicit registration.
- Tests prove that missing provider overrides do not change existing behavior.