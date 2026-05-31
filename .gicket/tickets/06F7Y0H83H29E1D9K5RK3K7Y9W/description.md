# Goal
Generate typed PIT read helpers for supported reviewed metadata shapes.

# Scope In
- Generate helper methods and projection models for supported PIT read shapes.
- Call existing IDataVaultReadService PIT APIs internally.
- Respect fingerprint, diagnostics, nullability, redaction, and unsupported-shape behavior.

# Acceptance Criteria
- Generated PIT helpers compile, run against existing PIT read service behavior, and have snapshot coverage.
- Unsupported PIT shapes emit deterministic diagnostics.