## Summary
Write the manual release checklist for future NuGet publishing.

## Current Baseline
- The release checklist must treat core plus provider extension packages as one aligned package family.
- Publishing remains manual and should avoid accidental partial publication of only one package.

## Scope
- Document required quality evidence, versioning, changelog, package validation, package publish order, and publish approval.
- Document the distinction between current source/development setup and future NuGet-first consumer installation guidance.

## Acceptance Criteria
- Docs state that publishing waits for sufficient quality and feature coverage.
- Manual steps avoid accidental publication.
- Checklist covers aligned versions and dependencies for `DCoding.Data.DVault` and all provider extension packages.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.