## Goal

Provide a deterministic comparison between the expected Data Vault model and generated/current EF table metadata so drift is visible before deployment.

## Scope In

- Table, column, key, index, timestamp storage, and provider capability differences.
- Human-readable and machine-readable report forms.
- Tests for representative drift cases.

## Scope Out

- Live database migration execution.
- Automated CI gating.

## Acceptance Criteria

- Drift reports distinguish informational differences from blocking incompatibilities.
- Reports identify affected model elements by logical and physical names.
- Basic checks do not require a live database.