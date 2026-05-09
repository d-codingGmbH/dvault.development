## Goal

Decide whether YAML is supported directly or through documented conversion, then implement the chosen bounded path without destabilizing the package dependency surface.

## Scope In

- Dependency and maintenance assessment for YAML support.
- Either direct YAML parsing or a documented JSON-first conversion boundary.
- Tests or documentation for the selected path.

## Scope Out

- Parallel YAML-only semantics.
- Unbounded dependency additions.

## Acceptance Criteria

- The decision is explicit and documented.
- If YAML is implemented, it maps to the same validated model as JSON.
- If YAML is deferred, the JSON-first path remains complete and the limitation is clear.