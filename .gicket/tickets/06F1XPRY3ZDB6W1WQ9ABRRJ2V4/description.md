## Goal

Make DVault safer inside the normal EF Core development lifecycle before adding heavier runtime features.

## Scope In

- Introduce stable DVault diagnostic codes.
- Validate generated EF migrations for Data Vault invariants.
- Expose design-time services for dotnet-ef workflows.
- Compare governed model artifacts against EF ModelSnapshot and optional live schema metadata.

## Scope Out

- No automatic migration execution.
- No provider-specific online migration engine.
- No breaking changes to existing v0.7.0 runtime APIs unless explicitly documented and guarded.

## Acceptance Criteria

- Child stories are done or intentionally superseded.
- Release documentation explains the lifecycle guardrail workflow.
- The release can be validated without package publishing credentials.

## Implementation Notes

- Tracking epic; implementation belongs in child stories and tasks.

## Open Questions

- none