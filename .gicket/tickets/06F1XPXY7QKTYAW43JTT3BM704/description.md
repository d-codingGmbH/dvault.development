## Goal

Implement the first ergonomic read-helper slice with focused tests.

## Scope In

- Choose the smallest current/as-of/bridge API surface that demonstrates the pattern.
- Add tests for returned rows or stable generated SQL.
- Add an example snippet.

## Scope Out

- No full rewrite of existing read pipelines.
- No unbounded API expansion.

## Acceptance Criteria

- API composes with existing metadata/read strategy services.
- Tests cover success and unsupported-shape diagnostics.

## Implementation Notes

- Keep the first API slice deliberately narrow.

## Open Questions

- none