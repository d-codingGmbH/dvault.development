## Goal

Prove the new fluent Code-First API produces the same schema semantics as the v0.5 metadata-first API for the covered hub, link, and satellite baseline.

## Scope In

- Schema parity tests for SQLite and provider-profile projections.
- Migration-model inspection tests where useful without requiring every database server locally.
- Regression coverage for naming collisions and provider capability profile effects.

## Scope Out

- Full runtime integration tests for every external database.
- Read/write helper behavior.

## Acceptance Criteria

- Code-First and metadata-first declarations produce equivalent table, column, key, and index shapes.
- Provider-specific capability profile differences remain explicit in tests.
- Tests fail on accidental drift between Code-First and metadata-first behavior.