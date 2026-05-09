## Goal

Implement typed explicit save helpers that build and submit save requests while preserving the deliberate explicit write boundary.

## Scope In

- Hub and ordinary satellite save helpers.
- Link save helper for configured relationships.
- Bulk helper for prepared domain batches.
- Regression tests against existing save-service behavior.

## Scope Out

- SaveChanges interception.
- Model-first generation.
- Provider-specific save strategy changes.

## Acceptance Criteria

- Typed save helpers call the existing provider strategy/fallback pipeline.
- Helpers do not hook or override DbContext.SaveChanges.
- Error messages identify the model element and source object that could not be mapped.