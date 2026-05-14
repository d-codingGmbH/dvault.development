<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the epic around the existing four-child split, ratified the explicit-save and provider-fallback repository baselines, and tightened acceptance around read ergonomics, compiled-query proof, opt-in interceptors, and optional bulk hooks; no ticket, relation, or planning-document writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Live relation state already materializes four child tickets via `parentOf`: `06F1XPXJW79K94G4WG86AG2X6M`, `06F1XPYA9MD0T9C4651ND8KX0W`, `06F1XPZAJBSSNN6HY1CHAQPH74`, and `06F1XQ03MADSPQD0AJN6R50D44`.
- The epic currently has seven outgoing `blocks` relations and one incoming `blocks` relation from `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`; this refinement leaves live relation state unchanged.
- Ticket attachment metadata shows one persisted plan artifact, `v0.9.0-read-runtime-performance-plan.md`; no new attachment or planning document was written in this pass.
- Repository evidence fixes the v1 default write boundary as explicit `IDataVaultSaveService`, with interceptor work remaining opt-in and provider-specific optimization flowing through existing provider strategy dispatch and fallback rather than a new default persistence path.
- Released docs already establish `IDataVaultReadService` as the narrow latest/as-of baseline; this epic extends that baseline into ergonomic current, point-in-time, and bridge scenarios instead of reopening direct EF support.

### Scope In
- Ergonomic EF-friendly APIs for current/latest satellite reads, as-of point-in-time reads, and bridge traversal scenarios.
- Proof of compiled-query and compiled-model compatibility for the supported read-helper surface.
- Opt-in load-metadata defaulting and interceptor ergonomics that preserve the explicit-save default.
- Optional provider bulk insert strategy hooks aligned to the existing provider optimization boundary.

### Scope Out
- Replacing EF Core or removing direct EF querying and persistence as a supported path.
- Making SaveChanges interception or other convenience plumbing the default write path.
- Mandating any third-party bulk package.
- Provider-specific optimizations or capabilities without provider-neutral fallback or explicit unsupported diagnostics.
- Broader new read-model feature families outside the bounded current/latest, point-in-time, and bridge scenarios of this epic.

## Acceptance Criteria
- Supported caller-facing APIs exist for current/latest satellite reads, as-of point-in-time reads, and bridge traversal reads, with documented inputs, outputs, and failure behavior.
- Compiled EF Core query and compiled model coverage demonstrates that the supported read-helper surface works as intended, or documents any explicit unsupported combinations.
- Load metadata defaulting can be enabled through an opt-in interceptor or convenience path without changing explicit `IDataVaultSaveService` as the default write boundary.
- Optional provider bulk insert hooks are defined against the existing provider-strategy and capability-profile boundary, with provider-neutral fallback or explicit unsupported diagnostics when no optimized hook applies.
- Focused benchmarks or regression tests show the new APIs preserve or improve the targeted performance scenarios versus the existing direct EF and narrow read-service baseline.
- README, architecture, and release-facing docs tell users when to use direct EF, the read helpers, opt-in interceptors, and provider-specific bulk paths.

## Definition of Done
- The four existing child tickets are completed or intentionally superseded, and epic relation state still reflects the chosen execution split.
- Benchmark or test evidence for the covered read and runtime scenarios is checked in or otherwise attached to the relevant work items.
- Documentation and public API naming are aligned with the existing bridge, read, and save architecture baselines visible in the repository.
- No remaining blocker for this epic is merely a workflow-label or handoff-state concern.

## Implementation Notes
- Keep the default write boundary from `docs/architecture/dvault-v1-explicit-save-service.md`: explicit `IDataVaultSaveService` stays the default, and any interceptor-based convenience remains additive and opt-in.
- Use the existing provider-neutral read direction rather than inventing a parallel surface: released docs already cover latest and as-of reads, while visible `DataVaultBridge*` source establishes the bridge-read naming and projection conventions for this epic.
- Bridge read work should stay bounded to generated endpoint hash-key columns plus hierarchy `TraversalDepth`, matching the visible validation and exact-name projection contract in `src/DCoding.Data.DVault`.
- Compiled-query and compiled-model proof should target the supported read-helper entry points and record explicit exceptions instead of silently relying on incidental EF behavior.
- Provider bulk insert hooks should reuse the existing `IDataVaultProviderSaveStrategy` and capability-profile dispatch model so unsupported providers keep the provider-neutral fallback contract.
- No bounded ticket write, relation write, attachment write, or planning-document write was materialized during this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- After the bounded v0.9.0 scope lands, should a later release add richer bridge and point-in-time typed projection helpers beyond endpoint hash keys and `TraversalDepth`?
- Should a later release publish a broader cross-provider benchmark matrix once the bounded provider-neutral and provider-hook baselines are stable?

## Risks
- Performance evidence can mislead if it covers only one provider or only happy-path data volumes; the epic should keep claims scoped to the measured scenarios.
- Interceptor convenience can blur the explicit-save guidance unless docs clearly preserve explicit `IDataVaultSaveService` as the default path.
- Provider bulk hooks may create inconsistent expectations across providers unless unsupported and fallback behavior is explicit and tested.

## Split Recommendations
- No additional split is recommended now; execution should continue through the four existing child tickets already linked by `parentOf`.
- If compiled-query and compiled-model proof grows into provider-by-provider certification instead of one supported baseline with explicit exceptions, split that certification work into a separate follow-up rather than expanding this epic.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Improve the day-to-day EF Core experience for querying DVault read models and saving insert-only data efficiently.

## Scope In

- Add ergonomic read APIs for current satellite, as-of PIT, and bridge traversal scenarios.
- Prove compiled query/model compatibility.
- Make load metadata defaults easier through opt-in interceptors.
- Define optional provider bulk insert strategy hooks.

## Scope Out

- No replacement for EF Core.
- No mandatory third-party bulk dependency.
- No provider-specific feature without fallback or explicit unsupported diagnostics.

## Acceptance Criteria

- Child stories are done or intentionally superseded.
- Benchmarks or focused tests show whether APIs improve or preserve performance.
- Docs describe when to use normal EF, read helpers, interceptors, or bulk paths.

## Implementation Notes

- This release depends on v0.8.0 lifecycle guardrails.

## Open Questions

- none