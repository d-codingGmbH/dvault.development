## Goal

Implement the baseline PIT-backed as-of read service using generated Data Vault tables and existing provider-neutral EF query capabilities.

## Scope In

- Reads by hub hash key and as-of timestamp.
- Joins from PIT rows to configured satellites.
- Tests for timestamp storage options and empty or missing snapshot states.

## Scope Out

- Provider-specific read optimization.
- Bridge traversal reads.

## Acceptance Criteria

- Correctness is proven independently from provider-specific optimization.
- The implementation uses registry/model metadata rather than duplicated table-name construction where available.
- Unsupported PIT shapes return deterministic diagnostics.