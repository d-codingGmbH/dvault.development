## Goal

Turn the deferred PIT and bridge metadata baseline into practical provider-neutral read helpers for common as-of snapshot and traversal scenarios.

## Scope In

- PIT-backed as-of read API contract and baseline implementation.
- Bridge traversal query helper contract and baseline implementation.
- Correctness tests over generated tables and existing metadata.

## Scope Out

- Provider-specific query tuning.
- Full graph query engine or unbounded recursive hierarchy semantics.

## Acceptance Criteria

- PIT read helpers return source-backed as-of rows for configured satellites.
- Bridge helper covers documented many-to-many and bounded hierarchy traversal baseline.
- Unsupported cases fail with clear diagnostics instead of returning incomplete data.