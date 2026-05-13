## Goal

Add the provider bulk strategy contract and verify fallback behavior.

## Scope In

- Introduce provider-neutral strategy interfaces/options.
- Add tests for no strategy, unsupported strategy, and selected strategy.
- Ensure diagnostics/logging make the selected path visible.

## Scope Out

- No provider-specific bulk implementation unless needed as a fake/test strategy.
- No benchmark expansion unless cheap and deterministic.

## Acceptance Criteria

- Fallback tests pass.
- Contract is documented enough for provider packages.
- Existing save strategy behavior does not regress.

## Implementation Notes

- Keep core package dependency-free.

## Open Questions

- none