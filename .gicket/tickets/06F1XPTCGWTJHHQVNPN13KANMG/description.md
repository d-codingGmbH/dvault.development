## Goal

Detect risky or invalid EF migration operations before teams apply schema changes to Data Vault tables.

## Scope In

- Inspect generated EF migration operations for hubs, links, satellites, PITs, and bridges.
- Report dangerous drops, lossy renames, missing technical columns, missing uniqueness/index contracts, and insert-only violations.
- Expose reusable API plus CLI/CI-friendly report path.
- Use stable DVault diagnostic codes.

## Scope Out

- No automatic migration execution or rollback.
- No provider-specific DDL rewrite engine.
- No full database diff implementation.

## Acceptance Criteria

- Safe migrations pass without findings.
- Dangerous operations produce stable diagnostics and remediation guidance.
- The guardrail can run without a live database.
- Docs show pre-integration usage.

## Implementation Notes

- Prefer EF Core migration operation metadata over SQL string parsing.

## Open Questions

- none