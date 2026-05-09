## Goal

Implement the JSON import path for the versioned model-first artifact and produce deterministic diagnostics for invalid documents.

## Scope In

- Parser for the v1 model-first JSON schema.
- Semantic validation beyond raw JSON shape.
- Tests for unknown version, missing references, duplicate names, unsupported capability combinations, and naming conflicts.

## Scope Out

- YAML support.
- Export tooling.
- Provider-specific read optimization.

## Acceptance Criteria

- Valid JSON artifacts produce a registry-compatible model.
- Invalid artifacts return structured diagnostics without partial model application.
- Parser errors are stable enough for CLI/build integration later.