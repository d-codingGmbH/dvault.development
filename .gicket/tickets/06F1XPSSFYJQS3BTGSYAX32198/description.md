## Goal

Implement the catalog and focused tests for stable DVault diagnostic definitions.

## Scope In

- Add a diagnostic definition model.
- Test id uniqueness, severity/category validity, and documentation coverage.
- Wire one existing validation path through the catalog.

## Scope Out

- No analyzer package.
- No large validator rewrite.

## Acceptance Criteria

- Catalog definitions are deterministic and centrally discoverable.
- Focused unit tests cover the catalog rules.

## Implementation Notes

- Keep the first slice small and reusable.

## Open Questions

- none