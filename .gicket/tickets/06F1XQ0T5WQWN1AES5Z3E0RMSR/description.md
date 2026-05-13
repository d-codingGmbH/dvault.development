## Goal

Package DVault guidance into tooling and examples that help teams adopt the library safely in real EF Core projects.

## Scope In

- Ship analyzer foundations for common modeling mistakes.
- Add Testcontainers-oriented integration helpers/examples.
- Update production adoption docs and examples after v0.8/v0.9 stabilize.

## Scope Out

- No IDE-specific extension beyond Roslyn analyzer capabilities.
- No hosted service or external SaaS dependency.
- No provider promise without a runnable test or documented limitation.

## Acceptance Criteria

- Child stories are done or intentionally superseded.
- Analyzer/test helper docs are clear about package boundaries.
- Examples use NuGet-based installation and current API names.

## Implementation Notes

- Polish adoption; do not introduce core semantics that belong in v0.8/v0.9.

## Open Questions

- none