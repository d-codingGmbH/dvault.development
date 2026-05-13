## Goal

Create an optional extension point for provider-specific bulk insert paths while keeping the EF-based fallback reliable.

## Scope In

- Define a bulk insert strategy contract for Data Vault append paths.
- Keep fallback behavior unchanged when no bulk strategy is registered.
- Implement one proof provider or documented no-op strategy with benchmark hooks.
- Measure impact against existing provider-specific save strategies.

## Scope Out

- No mandatory third-party bulk library in core.
- No destructive update/delete path.
- No promise that every provider has bulk support in the first slice.

## Acceptance Criteria

- Fallback behavior remains identical.
- Strategy selection is explicit in diagnostics/logging.
- Tests cover selection and fallback.
- Benchmarks or docs state evidence boundary.

## Implementation Notes

- Leave room for provider packages to optimize independently.

## Open Questions

- none