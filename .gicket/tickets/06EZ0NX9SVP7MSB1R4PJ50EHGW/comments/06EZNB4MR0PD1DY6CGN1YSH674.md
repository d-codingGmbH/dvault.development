[gicket-bot] PO refinement contract

Summary
- Re-refined the ticket after PO-critic return. The contract now limits implemented API claims to current branch source-backed DataVaultOptions resolver methods and explicitly keeps future hook API names out of scope; no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Resolved by restating the delivery contract: documentation may claim existing configuration API only where current branch source evidence supports it. The only approved current custom configuration examples are DataVaultOptions.UseLoadTimestampResolver overloads for IDataVaultLoadTimestampResolver and DataVaultOptions.UseRecordSourceResolver overloads for IDataVaultRecordSourceResolver. The remaining advanced hook categories must be described as planning categories or future expansion, not as implemented APIs.
- critic-item-2: `answered` - Resolved by removing the unsupported inference from the contract. Current branch source evidence is limited to the DataVaultOptions resolver methods and resolver interface types observed in source. Planned naming, hashing, provider behavior, and broader timestamp/record-source hooks are not to be presented as an existing public API surface unless development verifies additional source before editing docs.
- critic-item-3: `answered` - Resolved by making future API naming explicitly out of scope. The contract no longer asks developers to finalize names for future hook APIs or to rely on unnamed existing APIs. Documentation should avoid concrete names for future naming, hashing, provider, timestamp-formatting, or broader hook APIs, except for the source-observed DataVaultOptions resolver methods used in the single current example.

Clarifications
- This is a documentation-only ticket. It does not implement product code, new runtime hooks, new public APIs, provider matrices, migrations, or provider-specific dialect behavior.
- The five advanced hook categories remain the governing v1 planning categories from docs/plans/optional-advanced-configuration-hooks.md: naming conventions, hashing behavior, record source resolution, timestamp sourcing/formatting, and provider behavior.
- Current branch source-backed API evidence is limited to DataVaultOptions.UseLoadTimestampResolver overloads for IDataVaultLoadTimestampResolver and DataVaultOptions.UseRecordSourceResolver overloads for IDataVaultRecordSourceResolver. These are the only concrete implemented configuration names this ticket should use for a custom configuration example unless development first adds or verifies other source evidence.
- Naming, hashing, provider behavior, timestamp formatting, and broader future hook APIs must be described as planned expansion boundaries, not as implemented public APIs.
- The explicit-save-service document remains planning evidence for the write boundary: load timestamp and record source are supplied or resolved at the explicit request boundary and timestamp values are normalized to UTC. It is not evidence for future hook API names.
- The existing incoming parentOf relation from 06EZ0NWKC9ZME5BSCJFSQEQ02R remains valid. There are no ticket attachments, and no child tickets, relation updates, attachments, or planning documents were created in this refinement pass.

Scope In
- Document the five advanced hook categories, their intended use cases, deterministic defaults, and misuse boundaries.
- Document when not to use advanced hooks, including ordinary convention-first setup and explicit save request defaults.
- Include deterministic default examples and one current source-backed custom resolver configuration example using DataVaultOptions load timestamp or record-source resolver configuration.
- Document failure modes for invalid provider override, invalid timestamp behavior, and invalid record-source behavior.
- Clearly label future expansion boundaries so planned categories are not presented as implemented runtime APIs.

Scope Out
- Implementing new runtime hooks, product code, public APIs, provider option matrices, migrations, or provider-specific dialect behavior.
- Finalizing, introducing, or requiring concrete names for future hook APIs beyond the source-observed DataVaultOptions resolver methods used for the single custom resolver example.
- Changing service registration behavior, EF metadata projection, hashing implementation, save-service behavior, or provider strategy dispatch.
- Adding provider-specific reserved-word catalogs, timestamp precision matrices, hash migration behavior, lineage catalog integration, or provider-specific option documentation.
- Creating child tickets or planning documents for this bounded documentation task.

Open questions
- none

Follow-up questions
- When advanced hook implementation begins, decide whether the complete configuration surface is stable public API immediately or experimental for the first implementation pass.
- Choose which provider ecosystems should receive concrete provider-specific option documentation after the generic provider behavior boundary exists.
- Decide later whether timestamp customization should document deterministic test-time injection and wall-clock production behavior as separate named modes.

Risks
- The main delivery risk is documentation overclaiming runtime support for all five hook categories; current source evidence only supports concrete resolver configuration for load timestamp and record source.
- Provider override documentation can become misleading if it implies provider-specific option matrices are approved in this ticket; keep those as future provider work.
- Code-shaped examples can accidentally create public API expectations; keep future hook names out of examples unless current source evidence exists.

Split recommendations
- No split is recommended; the work remains a bounded documentation refinement task.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment