## Goal

Implement the provider-neutral EF ModelSnapshot adapter.

## Scope In

- Extract supported DVault table, column, index, and constraint metadata from EF model metadata.
- Compare against dvault.model.v1 artifacts.
- Cover matching and drifting snapshots in tests.

## Scope Out

- No live database access.
- No migration generation.

## Acceptance Criteria

- Adapter handles supported hubs, links, satellites, PITs, and bridges.
- Unsupported metadata gaps are explicit.

## Implementation Notes

- Preserve deterministic ordering.

## Open Questions

- none