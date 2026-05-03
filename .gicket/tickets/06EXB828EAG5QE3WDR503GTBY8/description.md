## Summary
Verify package contents locally before any publication decision.

## Current Baseline
- `dotnet pack DVault.slnx` now produces the core package and provider extension packages.
- Test/shared, benchmark, and umbrella projects must remain non-packable.

## Scope
- Check package metadata, docs, symbols, readme, dependency metadata, and package-to-package references.
- Verify all expected `.nupkg` and `.snupkg` files for the package matrix and fail on unexpected packages.

## Acceptance Criteria
- Package verification can run from command line.
- Failures describe missing or incorrect package content.
- Verification confirms aligned versions and correct dependencies from provider packages to the core package.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.