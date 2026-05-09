## Goal

Design a PIT-backed read API that builds on existing PIT metadata and remains compatible with typed read projections.

## Scope In

- API shape for as-of snapshot reads.
- Handling of multiple satellites, missing PIT rows, and timestamp storage modes.
- Diagnostics for unsupported multi-active or bridge interactions.

## Scope Out

- Implementation.
- Provider-specific optimization.

## Acceptance Criteria

- The contract is documented with examples before implementation.
- It does not conflict with the existing latest/as-of satellite read service.
- Tests or fixtures capture expected request and response shapes.