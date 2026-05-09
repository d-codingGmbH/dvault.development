## Goal

Expose a fluent EF Core modeling API so users can declare hubs, satellites, and links from domain entity configuration instead of hand-building the full metadata graph.

## Scope In

- Public API design for hub, satellite, and link declarations.
- Projection into existing DataVaultMetadataModel and EF shared-type table metadata.
- Tests for generated names, columns, keys, indexes, and provider profiles.

## Scope Out

- Model-first file import/export.
- Automatic writes from SaveChanges.
- Breaking changes to existing ApplyDataVaultMetadata APIs.

## Acceptance Criteria

- Domain entities can be configured as hubs with business-key selectors.
- Ordinary satellites can be declared fluently with payload selectors.
- Links can be declared fluently with deterministic participant ordering.
- Generated schema remains equivalent to the metadata-first path for covered scenarios.