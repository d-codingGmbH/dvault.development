## Goal

Provide EF Core-friendly read helper APIs for common Data Vault read cases.

## Scope In

- Design APIs for current satellite, as-of timestamp, and bridge traversal reads.
- Keep APIs composable with EF Core query patterns where practical.
- Document metadata requirements and limitations.
- Add representative tests and examples.

## Scope Out

- No hidden materialized view maintenance.
- No custom query provider.
- No promise that every helper remains fully provider-translatable in v1.

## Acceptance Criteria

- Examples compile and run in tests or examples.
- Unsupported shapes fail with clear diagnostics.
- Docs compare helpers to lower-level read pipelines.

## Implementation Notes

- Prefer explicit extension methods over broad magic.

## Open Questions

- none