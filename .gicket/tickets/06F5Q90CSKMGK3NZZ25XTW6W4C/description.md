Goal: Complete focused PIT and bridge behavior that belongs in a Data Vault EF Core library while leaving scheduling and orchestration to consumers.

Acceptance criteria:
- Adds missing PIT/bridge model support only where it remains explicit, service-based, and metadata-driven.
- Keeps provider-neutral behavior correct before provider-specific optimization.
- Does not add background jobs, cron abstractions, dashboarding, or deployment tooling.