## Summary
Organize fast unit tests for core behavior and provider package registration.

## Current Baseline
- Core behavior now includes metadata, naming, hashing, options, provider capability contracts, and provider save strategy dispatch.
- Provider packages should be covered by fast registration tests that do not require external database servers.

## Scope
- Tag or group tests for metadata, naming, hashing, options, provider registration, and provider strategy selection boundaries.

## Acceptance Criteria
- Unit tests are fast and deterministic.
- Coverage includes edge cases from modeling stories.
- Provider package registration tests verify expected core fallback behavior and the SQLite optimized strategy registration.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.