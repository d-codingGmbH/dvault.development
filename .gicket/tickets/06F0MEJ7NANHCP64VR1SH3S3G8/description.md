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