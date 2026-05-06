<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Re-refined the ticket after PO-critic return. The contract now limits implemented API claims to current branch source-backed DataVaultOptions resolver methods and explicitly keeps future hook API names out of scope; no child tickets, relation changes, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This is a documentation-only ticket. It does not implement product code, new runtime hooks, new public APIs, provider matrices, migrations, or provider-specific dialect behavior.
- The five advanced hook categories remain the governing v1 planning categories from docs/plans/optional-advanced-configuration-hooks.md: naming conventions, hashing behavior, record source resolution, timestamp sourcing/formatting, and provider behavior.
- Current branch source-backed API evidence is limited to DataVaultOptions.UseLoadTimestampResolver overloads for IDataVaultLoadTimestampResolver and DataVaultOptions.UseRecordSourceResolver overloads for IDataVaultRecordSourceResolver. These are the only concrete implemented configuration names this ticket should use for a custom configuration example unless development first adds or verifies other source evidence.
- Naming, hashing, provider behavior, timestamp formatting, and broader future hook APIs must be described as planned expansion boundaries, not as implemented public APIs.
- The explicit-save-service document remains planning evidence for the write boundary: load timestamp and record source are supplied or resolved at the explicit request boundary and timestamp values are normalized to UTC. It is not evidence for future hook API names.
- The existing incoming parentOf relation from 06EZ0NWKC9ZME5BSCJFSQEQ02R remains valid. There are no ticket attachments, and no child tickets, relation updates, attachments, or planning documents were created in this refinement pass.

### Scope In
- Document the five advanced hook categories, their intended use cases, deterministic defaults, and misuse boundaries.
- Document when not to use advanced hooks, including ordinary convention-first setup and explicit save request defaults.
- Include deterministic default examples and one current source-backed custom resolver configuration example using DataVaultOptions load timestamp or record-source resolver configuration.
- Document failure modes for invalid provider override, invalid timestamp behavior, and invalid record-source behavior.
- Clearly label future expansion boundaries so planned categories are not presented as implemented runtime APIs.

### Scope Out
- Implementing new runtime hooks, product code, public APIs, provider option matrices, migrations, or provider-specific dialect behavior.
- Finalizing, introducing, or requiring concrete names for future hook APIs beyond the source-observed DataVaultOptions resolver methods used for the single custom resolver example.
- Changing service registration behavior, EF metadata projection, hashing implementation, save-service behavior, or provider strategy dispatch.
- Adding provider-specific reserved-word catalogs, timestamp precision matrices, hash migration behavior, lineage catalog integration, or provider-specific option documentation.
- Creating child tickets or planning documents for this bounded documentation task.

## Acceptance Criteria
- Documentation enumerates the five advanced hook categories and explains default behavior, valid customization reasons, and misuse boundaries for each.
- Documentation states that zero-configuration remains the default and unset categories inherit deterministic defaults across machines, cultures, time zones, providers, and repeated runs.
- Documentation includes deterministic default examples and exactly one custom resolver configuration path grounded in current branch source evidence from DataVaultOptions load timestamp or record-source resolver methods.
- Documentation does not present future naming, hashing, provider behavior, timestamp-formatting, or broader hook APIs as implemented public APIs unless current branch source evidence is added or cited during development.
- Failure-mode documentation covers provider overrides that would drop required fields, change logical identity, weaken lookup behavior, lose canonical payload bytes, hide version metadata, or silently ignore meaningful unknown options.
- Failure-mode documentation covers invalid timestamp behavior, including missing required timestamps, non-UTC logical values, ambiguous offsets, non-normalized formats, unsupported precision, non-round-trippable values, local time, current culture, provider defaults, and lossy conversion.
- Failure-mode documentation covers invalid record-source behavior, including missing, empty, ambiguous, non-reproducible, generic fallback, or lineage-erasing source values.

## Definition of Done
- Updated documentation is committed under an existing docs surface and references the established planning sources where appropriate.
- The documentation can be reviewed without requiring product code changes or new runtime behavior.
- Every concrete current API/type claim in the documentation is backed by current branch source evidence; otherwise the text labels the idea as planned or future work.
- Examples are deterministic and avoid local clock, current culture, machine-specific, provider-generated, random, or process-local hidden inputs.
- Docs preserve the explicit-save-service boundary where load timestamp and record source are supplied or resolved at the request boundary.
- Any doc validation performed by repository tooling available in the branch passes, or limitations are recorded for the reviewer.

## Implementation Notes
- Use docs/plans/optional-advanced-configuration-hooks.md as the primary source for hook categories, defaults, validation expectations, and future expansion boundaries.
- Use src/DCoding.Data.DVault/DataVaultOptions.cs as current branch source evidence only for DataVaultOptions, UseLoadTimestampResolver, IDataVaultLoadTimestampResolver, UseRecordSourceResolver, and IDataVaultRecordSourceResolver.
- Use docs/architecture/dvault-v1-explicit-save-service.md for the current write-path boundary around IDataVaultSaveService, request-supplied load timestamp and record source, UTC normalization, deterministic hashing, provider-neutral fallback, and provider strategy dispatch, while avoiding unsupported future hook API names.
- Provider behavior docs should stay provider-neutral unless summarizing the existing provider capability/strategy posture; provider-specific option matrices remain future work.
- Naming and hashing sections should be written as planned advanced categories unless separate implementation evidence is added before development starts.
- Avoid invented example methods or type names for future hooks. If a conceptual example is needed, label it as non-API prose instead of compilable code.

## Open Questions
- none

## Follow-Up Questions
- When advanced hook implementation begins, decide whether the complete configuration surface is stable public API immediately or experimental for the first implementation pass.
- Choose which provider ecosystems should receive concrete provider-specific option documentation after the generic provider behavior boundary exists.
- Decide later whether timestamp customization should document deterministic test-time injection and wall-clock production behavior as separate named modes.

## Risks
- The main delivery risk is documentation overclaiming runtime support for all five hook categories; current source evidence only supports concrete resolver configuration for load timestamp and record source.
- Provider override documentation can become misleading if it implies provider-specific option matrices are approved in this ticket; keep those as future provider work.
- Code-shaped examples can accidentally create public API expectations; keep future hook names out of examples unless current source evidence exists.

## Split Recommendations
- No split is recommended; the work remains a bounded documentation refinement task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: document advanced hooks and their validation/failure behavior.

Acceptance Criteria:
- Documentation states which hooks exist, when to use them, and when not to use them.
- Examples show deterministic defaults and one custom configuration path.
- Failure modes include invalid provider override, invalid timestamp source, and invalid record-source behavior.