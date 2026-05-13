## Goal

Introduce live-schema comparison without forcing every provider to be complete immediately.

## Scope In

- Define a schema-read contract for tables, columns, indexes, and constraints.
- Implement at least one reliable provider path or documented no-support result.
- Add tests that do not require unavailable databases by default.

## Scope Out

- No destructive repair operations.
- No requirement that every provider has full live drift support in the first slice.

## Acceptance Criteria

- Unsupported providers return clear diagnostics.
- Supported provider tests are deterministic.
- Docs explain optional connection strings.

## Implementation Notes

- Keep it provider-neutral first.

## Open Questions

- none