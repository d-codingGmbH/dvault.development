## Goal

Add typed read projections for latest/as-of satellite rows on top of IDataVaultReadService so users can consume DTOs rather than dictionaries or raw records for common reads.

## Scope In

- Projection contract for typed satellite DTOs.
- Latest and as-of reads by parent hash key.
- Tests for missing payload columns, nullable payloads, driving keys, and timestamp storage modes.

## Scope Out

- PIT-backed read models.
- Bridge traversal reads.
- Provider-specific read strategy tuning.

## Acceptance Criteria

- Typed reads reuse the existing provider-neutral read service path.
- Projection failures are deterministic and actionable.
- Existing DataVaultSatelliteReadRecord reads remain available.