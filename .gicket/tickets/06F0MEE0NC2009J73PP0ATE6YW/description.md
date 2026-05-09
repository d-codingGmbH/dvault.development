## Goal

Define and implement a versioned model-first specification that can describe DVault hubs, links, satellites, PIT tables, bridges, naming options, and provider-relevant choices outside C# code.

## Scope In

- dvault.model schema versioning and validation rules.
- JSON parser and validation diagnostics.
- YAML ingestion decision and implementation boundary.
- Projection into registry and EF metadata.

## Scope Out

- Runtime model mutation.
- Code generation beyond import/projection.

## Acceptance Criteria

- Invalid model artifacts fail with line/path-oriented diagnostics where feasible.
- Imported models produce the same registry/metadata semantics as Code-First for covered scenarios.
- The format is stable enough to document and version.