## Summary
Make undocumented public/protected APIs visible during builds.

## Current Baseline
- XML documentation coverage must include the core package and provider extension packages.
- The new provider registration APIs are consumer-facing and should not bypass docs warnings.

## Scope
- Configure documentation warnings or analyzers for public/protected APIs in packable projects.
- Keep generated or internal-only code exceptions explicit.

## Acceptance Criteria
- Build fails or warns clearly for missing docs.
- Generated XML docs are included in pack output for every packable package.
- Provider registration extension methods and public provider contracts are covered.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.