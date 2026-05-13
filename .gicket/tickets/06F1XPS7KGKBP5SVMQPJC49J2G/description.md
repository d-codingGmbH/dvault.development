## Goal

Create a stable diagnostic-code baseline so guardrails, analyzers, and CI reports can refer to precise DVault findings.

## Scope In

- Define a DVault diagnostic id format such as DVLT0001.
- Create a central catalog for message text, severity, category, and remediation guidance.
- Update docs with the diagnostic contract and examples.
- Use the catalog in at least one existing validation path.

## Scope Out

- No Roslyn analyzer package in this release.
- No localization framework expansion beyond existing repository practices.

## Acceptance Criteria

- Diagnostic ids and categories are documented.
- Diagnostics include actionable remediation and affected locations where available.
- Tests cover duplicate ids and representative formatting.

## Implementation Notes

- This story intentionally blocks migration, design-time, drift, and analyzer work.

## Open Questions

- none