## Goal

Export the configured registry/model into a deterministic model-first artifact for review and version control.

## Scope In

- Export from Code-First/registry representation.
- Stable ordering and formatting.
- Round-trip compatibility tests where in scope.

## Scope Out

- Drift reporting.
- Database schema extraction.

## Acceptance Criteria

- Export output is deterministic across runs.
- Export preserves supported logical names, keys, payloads, driving keys, PIT, bridge, naming, and timestamp storage choices.
- Unsupported runtime-only details are omitted or documented clearly.