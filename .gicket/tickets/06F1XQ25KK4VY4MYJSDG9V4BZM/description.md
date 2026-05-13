## Goal

Create the first provider container fixture sample and connect it to DVault integration-test conventions.

## Scope In

- Choose one reliable provider as the first fixture.
- Expose a connection string compatible with existing tests/examples.
- Document Podman/Docker command expectations.

## Scope Out

- No full provider matrix in this task.
- No mandatory CI container startup.

## Acceptance Criteria

- The sample can be run locally.
- Failure/skips are explicit when runtime is missing.
- The pattern is reusable for later providers.

## Implementation Notes

- Start with the provider most reliable in local development.

## Open Questions

- none