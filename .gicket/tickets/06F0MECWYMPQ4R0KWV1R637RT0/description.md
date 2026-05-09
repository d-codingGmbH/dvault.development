## Goal

Help users understand what DVault generated, which strategy is active, and how to start from a working sample without reading the whole test suite.

## Scope In

- Model validation/explain output.
- Runnable starter examples for Code-First and typed workflows.
- README/release docs that compare metadata-first and Code-First paths.

## Scope Out

- CI-driven package publishing automation.
- Full database provisioning automation for every provider.

## Acceptance Criteria

- Users can inspect generated tables, columns, indexes, constraints, provider profile, and selected strategies.
- Examples can be run locally with minimal setup and no secrets committed.
- Documentation states when to use low-level metadata APIs versus convenience APIs.