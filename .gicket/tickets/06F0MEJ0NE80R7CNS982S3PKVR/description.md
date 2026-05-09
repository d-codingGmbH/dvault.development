## Goal

Establish comparable read-performance baselines before implementing provider-specific read optimizations.

## Scope In

- Latest satellite read benchmark.
- PIT as-of read benchmark.
- Bridge traversal read benchmark.
- Provider matrix for all providers that can be run locally or skipped deterministically.

## Scope Out

- Implementing optimizations.
- Provisioning secrets or persistent local database state.

## Acceptance Criteria

- Benchmark output includes classic or expected baselines where meaningful.
- Skipped providers explain exactly which configuration is missing.
- Results are summarized so optimization choices are visible.