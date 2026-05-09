## Goal

Let teams export configured models and compare expected model artifacts against generated/current Data Vault metadata so governance issues are visible before release.

## Scope In

- Export from registry/Code-First model to model-first artifact.
- Drift report comparing expected model versus generated EF/table metadata.
- Documentation for model governance workflow.

## Scope Out

- Database migration execution.
- CI publishing automation.

## Acceptance Criteria

- Exported artifacts are deterministic.
- Drift reports identify added, removed, renamed, or incompatible table/column/index/constraint elements.
- The workflow can be used manually without release credentials.