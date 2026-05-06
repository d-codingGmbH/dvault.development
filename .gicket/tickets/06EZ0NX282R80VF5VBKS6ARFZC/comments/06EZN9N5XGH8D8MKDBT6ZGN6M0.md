[gicket-bot] PO refinement contract

Summary
- Ticket 06EZ0NX282R80VF5VBKS6ARFZC is already a bounded child of 06EZ0NWKC9ZME5BSCJFSQEQ02R, and repository evidence supports refining it as a provider-behavior hook task with default inheritance plus explicit provider-package override registration; no child tickets, relation changes, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The only persisted comments are bot claim/lease entries; there are no human comments adding scope or conflicting decisions.
- The ticket already has one incoming `parentOf` relation from 06EZ0NWKC9ZME5BSCJFSQEQ02R, so this task remains part of the advanced-hooks decomposition and does not need further split materialization.
- For this ticket, default inheritance means the provider-behavior hook remains optional and inherits the existing core baseline when unset; it must not make normal `AddDVault()` setup require extra configuration.
- Current repository evidence already establishes the visible baseline: `AddDVault(Action<DataVaultOptions>)` is the optional advanced DI surface, `DataVaultProviderCapabilityProfileSelection` provides capability fallback, and `IDataVaultProviderSaveStrategy` plus the core save service already use explicit registration with provider-neutral fallback.

Scope In
- Expose one provider-behavior hook contract that deferred capabilities can consume instead of hard-coding provider names or provider-specific branches in core capability code.
- Keep the hook optional and default-inheriting so an unset provider-behavior hook preserves the current baseline behavior.
- Allow provider packages to supply explicit provider-behavior overrides through the existing registration style used for provider packages and optional DVault services.
- Add regression coverage proving that missing or incompatible provider overrides leave existing behavior unchanged, and that explicitly registered overrides are selected only when their registration path is in place.

Scope Out
- Do not implement PIT, bridge, multi-active, or other deferred capability families in this ticket.
- Do not redefine naming, hashing, record source, or timestamp semantics through the provider-behavior hook; those remain separate hook categories.
- Do not make advanced provider configuration required for ordinary `AddDVault()` or `ApplyDataVaultMetadata()` usage.
- Do not use this ticket to introduce broad provider-specific option matrices, migrations, DDL policy, or a wider provider release posture rewrite.
- Do not broaden provider-name capability-profile commitments beyond the registration/profile behavior already visible in the repository unless that is strictly required for the bounded hook surface itself.

Open questions
- none

Follow-up questions
- After the generic provider-behavior hook boundary lands, should its first public surface be documented as stable immediately or treated as experimental for one release cycle?
- Which provider ecosystems, if any, should receive deeper provider-specific option matrices after this generic provider-behavior boundary exists?

Risks
- If implementation assumes every provider package already auto-registers provider capability profiles, it will overstate the current baseline and may misroute Postgres or SQL Server behavior.
- If the default inheritance path changes the current fallback selection semantics, existing `AddDVault()` model-translation annotations or provider-neutral save behavior could regress even when no override is configured.
- If this ticket expands into concrete provider option matrices or release-posture commitments, it will reopen scope that the advanced-hooks planning docs explicitly keep deferred.

Split recommendations
- No split recommended; the task is already bounded to the provider-behavior hook surface, default inheritance, explicit provider registration, and regression coverage under parent 06EZ0NWKC9ZME5BSCJFSQEQ02R.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment