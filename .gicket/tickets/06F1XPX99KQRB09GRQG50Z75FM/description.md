## Goal

Improve the day-to-day EF Core experience for querying DVault read models and saving insert-only data efficiently.

## Scope In

- Add ergonomic read APIs for current satellite, as-of PIT, and bridge traversal scenarios.
- Prove compiled query/model compatibility.
- Make load metadata defaults easier through opt-in interceptors.
- Define optional provider bulk insert strategy hooks.

## Scope Out

- No replacement for EF Core.
- No mandatory third-party bulk dependency.
- No provider-specific feature without fallback or explicit unsupported diagnostics.

## Acceptance Criteria

- Child stories are done or intentionally superseded.
- Benchmarks or focused tests show whether APIs improve or preserve performance.
- Docs describe when to use normal EF, read helpers, interceptors, or bulk paths.

## Implementation Notes

- This release depends on v0.8.0 lifecycle guardrails.

## Open Questions

- none