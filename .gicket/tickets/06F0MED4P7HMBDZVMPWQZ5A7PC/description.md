## Goal

Add deterministic diagnostics that explain the configured Data Vault model and report invalid or risky configurations before runtime writes fail.

## Scope In

- Validation for duplicate logical names, missing parents, unsupported provider/profile combinations, and ambiguous typed mappings.
- Explain output for tables, columns, indexes, constraints, timestamp storage, provider profile, and strategy selection.
- Machine-readable output suitable for tests and future CLI tooling.

## Scope Out

- Full CLI command implementation unless needed for examples.
- Provider-specific optimization changes.

## Acceptance Criteria

- Diagnostics can be asserted in tests without brittle formatting.
- Human-readable output is concise enough for README/examples.
- Strategy fallback reasons are visible when provider-specific optimization is not selected.