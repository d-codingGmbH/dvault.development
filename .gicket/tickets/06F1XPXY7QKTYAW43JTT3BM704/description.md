<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Reconciled the ticket as stale/duplicate of the current repository baseline. The refined contract no longer asks dev to implement the first typed latest/as-of satellite helper slice; it asks PO-critic to verify retirement/closure as already satisfied by existing source, tests, docs, and release evidence.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The previous delivery contract is stale and should be superseded at ticket level.
- This ticket is a duplicate/already-satisfied implementation request against the current branch baseline, not a candidate for dev handoff.
- Suggested title: Retire duplicate first typed latest/as-of satellite read-helper ticket.
- No new source, test, docs, or API snapshot work is requested by this ticket.

### Scope In
- Mark this ticket as already satisfied/duplicate of the current typed latest/as-of satellite read-helper baseline.
- Preserve repository evidence that the typed latest/as-of helper, raw latest-row escape hatch, registry-backed metadata usage, provider read-strategy diagnostics, tests, and documentation/release notes already cover the requested slice.
- Prevent dev handoff for duplicate implementation work from this ticket.

### Scope Out
- No new public typed latest/as-of satellite helper implementation.
- No new registry-backed adapter implementation.
- No new diagnostics, provider strategy, test, documentation, or API snapshot work solely for this ticket.
- No PIT-backed typed read-helper work.
- No bridge traversal helper work.
- No reflection-based DTO binding or auto-mapping work.

## Acceptance Criteria
- PO-critic can verify that the ticket has been reconciled as already satisfied/duplicate rather than retained as an implementation task.
- The ticket contract identifies no remaining dev delta and contains no instructions to add the first helper slice again.
- The retirement rationale cites repository evidence for typed latest/as-of satellite reads, registry-backed metadata/read usage, provider read-strategy diagnostics, focused tests, documentation, and release/API baseline coverage.
- PIT, bridge, and reflection-based binding split recommendations are explicitly retired from this ticket and left only as possible independent future work.

## Definition of Done
- The ticket-level contract is updated to supersede the stale implementation request with an already-satisfied/duplicate retirement rationale.
- PO-critic responses address critic-item-1 through critic-item-5 exactly once each with visible evidence.
- No blocking open questions remain for PO-critic review.
- No developer implementation handoff is requested for this ticket.

## Implementation Notes
- Do not implement or modify source code for this ticket; the current branch baseline already contains the requested helper slice.
- Use docs/releases/v0.6.0.md and docs/releases/v0.7.0.md as product/release evidence for the typed latest/as-of read baseline.
- Use DataVaultDiagnosticsIntegrationTests and BenchmarkScenarioExecutionTests as test evidence for provider read-strategy composition and latest-satellite read coverage.
- Use the PO-critic finding itself as authoritative evidence that source, tests, docs, and API snapshot coverage are already present in the current source tree.

## Open Questions
- none

## Follow-Up Questions
- If a future gap is found in typed latest/as-of satellite reads, should it be raised as a new narrowly scoped defect against the specific missing behavior rather than reopening this implementation ticket?
- Should product backlog cleanup add a separate administrative convention for closing duplicate/already-satisfied automation tickets when repository evidence has overtaken the original scope?

## Risks
- none

## Split Recommendations
- Retired for this ticket: do not split PIT-backed typed read helpers from this duplicate retirement ticket; raise independent future work only if a new PIT gap is identified.
- Retired for this ticket: do not split bridge traversal typed helpers from this duplicate retirement ticket; raise independent future work only if a new bridge gap is identified.
- Retired for this ticket: do not split reflection-based DTO binding from this duplicate retirement ticket; keep it out of scope unless a separate product decision creates a new API family.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement the first ergonomic read-helper slice with focused tests.

## Scope In

- Choose the smallest current/as-of/bridge API surface that demonstrates the pattern.
- Add tests for returned rows or stable generated SQL.
- Add an example snippet.

## Scope Out

- No full rewrite of existing read pipelines.
- No unbounded API expansion.

## Acceptance Criteria

- API composes with existing metadata/read strategy services.
- Tests cover success and unsupported-shape diagnostics.

## Implementation Notes

- Keep the first API slice deliberately narrow.

## Open Questions

- none