## Goal

Create focused tests proving supported compiled EF Core paths.

## Scope In

- Add one compiled model test for metadata registration.
- Add one compiled query test for a supported read path.
- Capture limitations in assertions or docs.

## Scope Out

- No broad benchmark matrix.
- No provider-specific code unless required for the fixture.

## Acceptance Criteria

- Tests are deterministic and run in the normal suite.
- Failures provide actionable diagnostics.

## Implementation Notes

- Start with provider-neutral tests.

## Open Questions

- none