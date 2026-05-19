<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the local ticket store, comment and attachment state, and live relations from .gicket, then bounded this story to request-bound save/read strategy explanation on the existing diagnostics contract; no child tickets, relation edits, attachments, or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The live parent relation is 06F2PGQ27NWVZ1B1R651S7SM4M -> 06F2PGQ6T5TGNWCBQBX3700D84 (parentOf), so this story stays inside the Epic: Observability and operations boundary.
- The live outbound blocks relations already separate downstream work into telemetry 06F2PGQBGNZPEEJE4KBET4JG24, support bundle 06F2PGQJ7THHNSYYBFFPBG4174, and v0.16 documentation 06F2PGQQJB5FJGDB16M2G7CPCM.
- The inbound blocks relation from 06F2PGP7HM8F39K3J0H5JHB3B4 (Epic: Maintenance and query operations) is historical context because that ticket is done; it is not an open blocker for this story.
- Repository evidence already establishes the intended contract surface through DataVaultDiagnosticsResult, DataVaultSaveStrategyDiagnostics, DataVaultReadStrategyDiagnostics, IDataVaultDiagnosticsService, and IDataVaultReadDiagnosticsService; this story should complete that surface rather than introduce a parallel explain API.
- The read-side explain baseline is finite and request-bound: analyze latest/as-of satellite requests through DataVaultLatestSatelliteReadRequest, PIT requests through DataVaultPitAsOfReadRequest, and bridge requests through DataVaultBridgeReadRequest.
- No child tickets, relation edits, attachments, or planning documents were materialized during this refinement pass.

### Scope In
- Human-readable explanation of whether DVault selected a provider-specific save/read strategy or used provider-neutral fallback.
- Structured save-strategy decision data for analyzed save requests, including provider identity, selected strategy metadata when applicable, candidate ordering, and fallback causes.
- Structured read-strategy decision data for analyzed latest/as-of satellite, PIT, and bridge read requests.
- Tests and public contract coverage that lock the strategy-explanation behavior and keep it reusable by downstream observability tickets.
- Source-local documentation or API snapshot updates needed to ship the explain surface safely.

### Scope Out
- Changing actual save or read dispatch semantics, provider thresholds, provider capability profiles, or persistence/read results.
- Telemetry hooks, counters, or metric export; that remains the separate story 06F2PGQBGNZPEEJE4KBET4JG24.
- Support bundle packaging, redaction, or artifact export; that remains the separate story 06F2PGQJ7THHNSYYBFFPBG4174.
- The coordinated v0.16 documentation/release wrap-up tracked by 06F2PGQQJB5FJGDB16M2G7CPCM, beyond any minimal source-local docs needed for this story.
- New provider-specific read optimizations, PIT/bridge maintenance changes, or broader operations tooling.

## Acceptance Criteria
- Analyzing a save request through IDataVaultDiagnosticsService returns structured save-strategy explanation data that identifies the provider, status, selected strategy name/priority when one is chosen, evaluated candidates in dispatcher order, and distinct fallback causes when provider-neutral fallback is used.
- Analyzing a read request through IDataVaultReadDiagnosticsService returns the analogous structured read-strategy explanation data for latest/as-of satellite, PIT, and bridge requests, including request-shape-specific decline causes where applicable.
- The human-readable diagnostics output clearly states the save-strategy status and read-strategy status, and includes the selected strategy name when a provider-specific strategy is chosen.
- Strategy explanation stays request-bound and observational only: diagnostics without a save or read request keep the corresponding strategy status NotEvaluated, and this story does not change actual save/read execution behavior.
- Automated coverage proves both selected-strategy and provider-neutral fallback cases, including candidate ordering and representative fallback causes, and any public API snapshot or documentation updates required by the changed contract are included.

## Definition of Done
- Repository tests and snapshot checks that cover the strategy-explanation contract pass for the touched surfaces.
- Any changed public diagnostics contract, display output, or XML/API documentation is updated consistently enough for downstream v0.16 documentation work to reuse it without reopening scope.
- The final story evidence shows a stable contract that downstream telemetry, support-bundle, and documentation tickets can consume without inventing new save/read decision shapes.
- Any deferred work remains explicitly outside this story and continues to live in the already-related downstream tickets instead of being hidden in implementation notes.

## Implementation Notes
- Reuse DataVaultDiagnosticsResult as the public envelope and extend or finish its SaveStrategy, ReadStrategy, and ToDisplayString behavior instead of creating a second explain artifact.
- Keep save decision analysis on IDataVaultDiagnosticsService and read decision analysis on IDataVaultReadDiagnosticsService; the repository already ratifies that split.
- Mirror the real dispatcher truth when reporting candidates and fallback causes: priority ordering, registration-order tie-breaks, provider-name matching, dirty-context gates, multi-active restrictions, unsupported PIT/bridge shape checks, and generic strategy decline should come from the same rules that runtime dispatch uses.
- Treat ReadCurrentSatelliteAsync and ReadAsOfSatelliteAsync as convenience wrappers over the existing latest-satellite request baseline instead of inventing separate read-decision categories.
- Preserve provider-neutral fallback as the compatibility baseline; the explain surface reports why optimized strategies were selected or declined, but it does not change fallback behavior.

## Open Questions
- none

## Follow-Up Questions
- What subset of the structured strategy explanation should the future support-bundle story serialize by default, and what should be redacted or collapsed?
- Which strategy-decision events or counters should the telemetry story emit, and at what aggregation/cardinality boundary?
- Should the downstream v0.16 documentation story add standalone troubleshooting examples for common fallback causes, or keep that detail limited to API/source docs and release notes?

## Risks
- Explanation logic can drift from real runtime dispatch if it duplicates strategy-selection rules instead of reusing the same gate evaluation behavior.
- Documentation drift is likely unless README and release-note updates explicitly catch up with the existing read-diagnostics surface as well as the save-diagnostics surface.
- Scope can sprawl into telemetry, support-bundle, and release-wrap work unless the existing downstream blocks relations continue to own those deliverables.

## Split Recommendations
- No additional split is recommended. The live relation set already separates downstream telemetry, support-bundle, and v0.16 documentation work into tickets 06F2PGQBGNZPEEJE4KBET4JG24, 06F2PGQJ7THHNSYYBFFPBG4174, and 06F2PGQQJB5FJGDB16M2G7CPCM.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Provide human-readable and structured explanations for selected strategies.

## Scope
- Refine and complete the work for "Explain save and read strategy decisions" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.