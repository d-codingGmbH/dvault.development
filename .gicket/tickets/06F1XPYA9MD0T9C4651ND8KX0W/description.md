## Goal

Make it clear and tested how DVault behaves with EF Core compiled models and compiled queries.

## Scope In

- Add compatibility tests for compiled models with DVault metadata annotations.
- Add compiled query examples for representative read APIs.
- Document supported patterns and known limitations.
- Benchmark where stable evidence is available.

## Scope Out

- No requirement to support every dynamic query shape as compiled.
- No provider-specific compiled model generator.

## Acceptance Criteria

- Compiled model tests pass for supported metadata paths.
- Compiled query examples work or fail with documented diagnostics.
- Normal non-compiled EF usage does not regress.

## Implementation Notes

- Let evidence guide whether additional optimization is needed.

## Open Questions

- none