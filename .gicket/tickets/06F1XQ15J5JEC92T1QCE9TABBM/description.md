## Goal

Warn developers about common DVault modeling mistakes while they write EF Core configuration code.

## Scope In

- Create an analyzer package project or documented package boundary.
- Implement initial rules for missing business keys, suspicious satellite configuration, duplicate metadata names, or unsupported declaration patterns.
- Reuse stable diagnostic ids where possible.
- Add tests and packaging metadata.

## Scope Out

- No IDE extension outside Roslyn conventions.
- No analyzer for every DVault rule in the first release.
- No runtime behavior changes.

## Acceptance Criteria

- Analyzer package builds and packs with metadata.
- At least two useful diagnostics are covered by tests.
- Rules avoid noisy false positives.
- Docs show installation and suppression guidance.

## Implementation Notes

- Prefer high-confidence rules over broad heuristics.

## Open Questions

- none