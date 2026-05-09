## Goal

Implement the fluent Code-First path for hub and ordinary satellite declarations and project it into the existing provider-aware EF metadata translator.

## Scope In

- Hub business-key selector capture.
- Satellite payload selector capture.
- Equivalent hub and satellite metadata generation.
- Tests comparing generated schema with metadata-first declarations.

## Scope Out

- Link declarations.
- Save helper generation.
- Model-first file import.

## Acceptance Criteria

- Fluent hub and satellite declarations produce deterministic tables, columns, keys, and indexes.
- Invalid selectors fail with actionable validation messages.
- Existing metadata-first tests continue to pass unchanged.