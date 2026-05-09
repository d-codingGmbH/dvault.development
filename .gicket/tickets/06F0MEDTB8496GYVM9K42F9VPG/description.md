## Goal

Add durable model-first specifications and advanced read-model helpers after v0.6 establishes a friendlier Code-First and registry baseline.

## Scope In

- Versioned model-first schema and import/export path.
- Validation and drift reports for model governance.
- PIT-backed as-of reads and bridge traversal helper baseline.
- Benchmark-driven provider-aware read optimization hooks.

## Scope Out

- Replacing the v0.6 Code-First path.
- Hiding Data Vault semantics behind a broad ORM abstraction.
- Automatic database/container provisioning.

## Acceptance Criteria

- A model-first artifact can be validated and projected into the same registry/EF metadata surface used by Code-First.
- Read helpers are source-backed by implemented PIT/bridge metadata behavior.
- Provider read optimization work is benchmarked and gated by correctness tests.