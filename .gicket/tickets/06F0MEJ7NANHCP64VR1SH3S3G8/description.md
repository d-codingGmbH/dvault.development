<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded provider-neutral read-strategy dispatch task that mirrors the existing save-strategy pattern while preserving current read APIs and fallback behavior.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 baseline should mirror the existing save-strategy dispatch model: core package owns provider-neutral strategy contracts and dispatcher behavior, provider packages may register concrete strategies later, and fallback remains in the core read service pipeline.
- The initial strategy surface applies to the currently implemented public read service shapes, especially latest/as-of satellite reads, and should not require user code to select a provider path.
- Diagnostics/explain output should report read-strategy evaluation when a read request is present, including selected strategy, declined strategies, and provider-neutral fallback usage.

### Scope In
- Add provider-neutral read strategy contracts in the core DVault package for provider read optimization selection.
- Define deterministic strategy ordering using descending priority and dependency-injection registration order as the tie breaker, consistent with the documented save-strategy baseline.
- Route existing public read-service calls through provider-specific read strategy evaluation before using the provider-neutral fallback implementation.
- Preserve correct provider-neutral fallback reads when no strategy is registered or every registered strategy declines the current DbContext/request shape.
- Expose read-strategy selection and decline/fallback information through existing diagnostics/explain output surfaces.
- Add tests covering registration order, priority ordering, decline behavior, fallback behavior, and diagnostics/explain visibility.

### Scope Out
- Implementing provider-specific optimized read SQL or provider package strategy implementations.
- Changing public save strategy APIs or save dispatch behavior.
- Changing the caller-facing read-service contract so application code has to select providers explicitly.
- Adding PIT-backed read APIs, bridge traversal helpers, or new read-model feature scope beyond already implemented public read methods.
- Changing persistence semantics, schema generation, or provider capability profile selection rules except where read diagnostics need to observe the current provider/profile context.

## Acceptance Criteria
- Existing read APIs continue to work without provider-specific calls or provider-specific request types from user code.
- When one or more read strategies are registered, the dispatcher evaluates compatible strategies in descending priority and preserves DI registration order for equal priority.
- When the selected provider strategy handles a supported read request, the provider-neutral fallback path is not used for that request.
- When every registered strategy declines, or no strategy is registered, the current provider-neutral read behavior is preserved.
- Strategy declines do not fail the read unless the fallback read itself fails for an existing valid reason.
- Diagnostics/explain output identifies read-strategy evaluation status, selected strategy when applicable, declined strategies when available, and fallback usage.
- Automated tests prove registration, priority/order, decline, fallback, and diagnostics/explain behavior.

## Definition of Done
- Provider-neutral read strategy contracts and dispatcher are implemented in the core DVault package using established naming, DI, and async/cancellation conventions.
- Existing read-service tests continue to pass unchanged for provider-neutral fallback behavior.
- New focused tests cover strategy selection success, equal-priority registration order, decline-to-next-strategy behavior, decline-to-fallback behavior, no-strategy fallback behavior, and diagnostics/explain reporting.
- Public XML documentation or equivalent API comments describe the intended provider opt-in model and fallback contract for the new read strategy surface.
- No provider package is required to implement an optimized read strategy for this ticket to pass.

## Implementation Notes
- Use the existing provider-specific save strategy dispatch as the architectural template rather than introducing provider-name branching in the core read service.
- The read strategy context should carry the DbContext, read request details needed by the current public read methods, and existing provider/capability metadata needed for diagnostics; exact helper names are implementation details.
- The dispatcher should be internal to the core read service path unless the existing public API pattern requires a public contract for provider packages to implement strategies.
- Decline should be a normal strategy result, not an exception path, so unsupported providers or request shapes keep falling back cleanly.
- Diagnostics should distinguish not evaluated, selected, declined, and fallback states consistently with existing diagnostics/explain conventions.

## Open Questions
- none

## Follow-Up Questions
- Which provider packages should receive the first optimized read strategies after this hook exists?
- Should future provider-specific read optimizations cover PIT-backed reads, bridge reads, and multi-active satellite reads once those public surfaces are finalized?
- Should release documentation call out the hook as an extension point before any provider package ships an optimized read implementation?

## Risks
- The dispatch layer touches shared read-service behavior, so regression coverage needs to prove fallback output remains stable for existing latest/as-of read scenarios.
- Diagnostics wording may drift from the save-strategy explain vocabulary unless implementation reuses the established conventions.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Add a read-strategy selection surface analogous to save strategies so providers can optimize read paths while the fallback remains correct.

## Scope In

- Provider-neutral read strategy interface and selection order.
- Fallback behavior and diagnostics when a strategy declines.
- Tests for registration, decline, and fallback behavior.

## Scope Out

- Provider-specific optimization implementation.
- Changing public save strategy APIs.

## Acceptance Criteria

- Existing read APIs do not need provider-specific calls from user code.
- Strategy selection is visible in diagnostics/explain output.
- Providers can opt in incrementally without breaking fallback reads.