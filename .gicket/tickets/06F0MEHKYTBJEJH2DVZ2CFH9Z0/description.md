## Goal

Implement the documented provider-neutral bridge traversal read baseline for many-to-many and bounded hierarchy bridge rows.

## Scope In

- Reads over generated bridge tables.
- Deterministic handling of traversal depth and endpoint hash-key columns.
- Tests for empty bridges, missing endpoints, and unsupported shapes.

## Scope Out

- Provider-specific optimization.
- PIT read implementation.

## Acceptance Criteria

- The implementation is correct before provider-specific tuning is attempted.
- Diagnostics clearly identify unsupported bridge metadata combinations.
- Existing bridge schema tests continue to pass.