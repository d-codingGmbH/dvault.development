## Summary
Plan and enforce the one public/protected member per file rule where practical.

## Current Baseline
- The rule must be checked across the core source project and the provider extension projects.
- Provider packages may contain small registration classes, but any exception must be documented rather than silently expanding file scope.

## Scope
- Use existing analyzers or a lightweight custom check.
- Apply the check to packable source projects without flagging generated or non-packable build artifacts unnecessarily.

## Acceptance Criteria
- Violations are reported with actionable paths.
- Generated or unavoidable exceptions are documented.
- Provider package source files are covered by the same policy as the core package.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.