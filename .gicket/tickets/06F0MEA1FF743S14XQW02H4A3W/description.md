## Goal

Add fluent link declarations for relationships between configured hubs while preserving deterministic participant ordering and existing link naming conventions.

## Scope In

- Relationship-name support.
- Participant resolution from configured hubs.
- Projection to DataVaultLinkMetadata and EF shared-type metadata.
- Tests for two-participant and multi-participant links.

## Scope Out

- Hub/satellite fluent projection.
- Typed save helpers.
- Provider-specific SQL changes.

## Acceptance Criteria

- Link configuration fails clearly when a participant hub is missing or ambiguous.
- Generated link schema matches the metadata-first equivalent.
- Relationship indexes and primary keys remain provider-aware through the existing translator path.