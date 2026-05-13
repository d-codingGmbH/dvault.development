## Goal

Add the first migration-operation validator and tests for DVault schema invariants.

## Scope In

- Create fixtures for AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn operations.
- Map findings to diagnostic catalog entries.
- Keep output deterministic for CI comparison.

## Scope Out

- No provider DDL parser.
- No runtime database access.

## Acceptance Criteria

- Tests cover safe and unsafe operations.
- Output contains diagnostic id, severity, location, and remediation text.

## Implementation Notes

- Keep provider-neutral first.

## Open Questions

- none