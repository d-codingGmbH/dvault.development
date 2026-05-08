## Goal

Reconcile the bridge documentation with the implemented bridge baseline that now exists in source and tests.

## Context

- This follow-up preserves the PO-critic finding from 06EZ0NTV4SVAKV98C418T8A3CC without discarding the earlier completed docs ticket 06EZ0NVE88WW9PMM04NVAZHRG0.
- The earlier docs child described a deferred-only snapshot, but current source and tests now expose baseline bridge metadata translation and schema behavior.
- Parent story 06EZ0NTV4SVAKV98C418T8A3CC must not close until the durable docs no longer contradict the implemented bridge baseline.

## Scope In

- Update durable documentation, especially docs/plans/deferred-data-vault-capabilities.md and any relevant bridge/README cross-links, so it no longer claims that bridge translator output or bridge schema behavior is absent when it is implemented.
- Clearly distinguish implemented baseline bridge behavior from still-deferred advanced behavior such as provider-specific tuning, complex traversal semantics, hierarchy edge cases, PIT interactions, and runtime features not proven by source/tests.
- Keep examples aligned with current public vocabulary and implemented metadata behavior instead of inventing future APIs.

## Scope Out

- New bridge runtime implementation, provider-specific DDL optimization, or broader modeling changes.
- Rewriting the completed historical docs ticket; this task is a forward sync against current repository evidence.
- Expanding bridge scope beyond facts supported by current source and tests.

## Acceptance Criteria

- Documentation no longer states or implies that baseline bridge metadata translation/schema output is absent when current code/tests prove it exists.
- Documentation identifies which bridge pieces are implemented now and which advanced bridge capabilities remain deferred.
- The bridge example and terminology are source-backed and do not invent unsupported public APIs or provider guarantees.
- Parent story 06EZ0NTV4SVAKV98C418T8A3CC can rely on this ticket as the documentation reconciliation gate before closure.

## Definition of Done

- Durable docs are consistent with current bridge source/tests and with the parent story scope.
- The update is docs-focused and does not discard existing completed work.
- Any remaining bridge uncertainty is captured as explicit deferred/future scope, not as a contradiction in the current docs.