## Goal

Make provider integration testing easier through Testcontainers-oriented helpers and examples.

## Scope In

- Provide examples or helper fixtures for PostgreSQL, SQL Server, MySQL, Oracle, and SQLite-equivalent local testing where feasible.
- Document Podman/Docker prerequisites and connection string handoff.
- Keep tests skippable when runtime/images are unavailable.
- Align with existing integration-test environment variables.

## Scope Out

- No requirement that users adopt Testcontainers.
- No bundling database images.
- No CI provider matrix expansion unless explicitly configured.

## Acceptance Criteria

- Examples document exact package references and commands.
- Unavailable runtime/images produce clear skip diagnostics.
- At least one provider helper is runnable locally.

## Implementation Notes

- Distinguish test helpers from benchmark containers.

## Open Questions

- none