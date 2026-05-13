## Goal

Extend v0.7.0 drift tooling to compare governed DVault artifacts against EF ModelSnapshot metadata and optional live database schema metadata.

## Scope In

- Add an EF ModelSnapshot comparison adapter.
- Add a provider-neutral live schema reader abstraction with provider hooks where useful.
- Report drift with stable diagnostic codes and deterministic ordering.
- Document when ModelSnapshot comparison is sufficient and when live schema comparison is needed.

## Scope Out

- No automatic migration generation.
- No destructive database repair.
- No full provider-specific SQL diff engine.

## Acceptance Criteria

- ModelSnapshot comparison reports no drift for matching metadata.
- Missing/renamed/incompatible items include locations.
- Live schema comparison is optional and skipped clearly when unavailable.
- Docs state provider evidence boundaries.

## Implementation Notes

- Reuse v0.7.0 model artifact and drift report concepts.

## Open Questions

- none