<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratified the existing SQLite-local and Postgres-configured test baseline, bounded SQL Server/Oracle/MySQL to smoke coverage in v1, and left no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already separates tests into existing Unit and Integration areas inside tests/DCoding.Data.DVault.Tests; this ticket refines execution categories within that layout instead of introducing a new test-project split.
- Visible integration coverage is SQLite-heavy with a Postgres configuration/schema path already present; no existing SQL Server, Oracle, or MySQL external integration fixture is visible in the branch snapshot, so those providers are bounded to smoke coverage in v1 unless equivalent configuration already appears during implementation.
- SQLite remains required local integration coverage and must distinguish the core AddDVault() fallback path from the optimized AddDVaultSqlite() registration path so the expected provider-specific behavior is explicit.
- This ticket already blocks 06EXB82RW6PV2NFG088G6BPFHC, so the default-versus-opt-in category contract defined here should be treated as the source of truth for downstream test and pipeline work.

### Scope In
- Category and filter the existing SQLite integration tests as required local coverage in the current integration test project.
- Add an opt-in category boundary and actionable skip behavior for external-provider integration tests that depend on live database configuration.
- Provide default-run smoke coverage for provider packages that should not require live database access, including registration/package checks for providers without external configuration scaffolding.
- Update test discovery or runner expectations so default repository validation executes required SQLite integration and provider smoke coverage without selecting unconfigured external database tests.

### Scope Out
- Creating new SQL Server, Oracle, or MySQL live database fixtures, credentials, containers, or CI infrastructure in this ticket.
- Provider-specific performance, tuning, or optimization test work.
- Production API or runtime behavior changes beyond what is needed to support test categorization and coverage clarity.
- Reorganizing the repository into new test assemblies when category/filter work within the existing layout is sufficient.

## Acceptance Criteria
- Default repository test runs keep SQLite integration coverage in scope and identify it as required local coverage rather than optional external-provider work.
- External-provider integration tests that need live database configuration are behind an opt-in category and skip with actionable missing-configuration messages when that configuration is absent.
- The existing Postgres configuration path is the v1 template for configured external-provider tests, and provider packages without equivalent live-database configuration receive default-run smoke coverage instead.
- SQLite-focused tests demonstrate representative sample scenarios and the distinction between core AddDVault() behavior and the optimized AddDVaultSqlite() path.

## Definition of Done
- Test code, filters, and any supporting discovery assertions are updated consistently across the existing Unit and Integration layout.
- Default repository validation passes in an environment with no external database configuration while still running required SQLite integration and provider smoke coverage.
- Opt-in external-provider test runs remain discoverable and make their enabling configuration clear through code or test output.
- Shared charter standards continue to be satisfied.

## Implementation Notes
- Use tests/DCoding.Data.DVault.Tests/Integration as the primary surface for provider integration category work; do not assume a new external-provider test assembly.
- Reuse the visible PostgresIntegrationTestConfiguration pattern as the baseline for live-database opt-in behavior and keep missing-configuration skip messaging consistent across external providers.
- Use the existing provider package boundaries in src/DCoding.Data.DVault.Sqlite, .Postgres, .SqlServer, .Oracle, and .MySql for smoke coverage, rather than requiring live database access for every provider in this ticket.
- Keep the category contract stable enough to unblock downstream work on 06EXB82RW6PV2NFG088G6BPFHC.

## Open Questions
- none

## Follow-Up Questions
- When SQL Server, Oracle, or MySQL gain real external fixtures, should each provider follow the same opt-in configuration contract established here or introduce provider-specific configuration shapes?
- After categories are stable, should opt-in external-provider runs remain manual/on-demand only, or should later CI work add scheduled jobs for configured environments?

## Risks
- If runner filters and test categories drift apart, default runs may either miss required SQLite coverage or accidentally include external-provider tests.
- If skip messages are inconsistent across providers, missing external configuration can look like silent test avoidance instead of an intentional opt-in boundary.

## Split Recommendations
- No new split is recommended; current repository evidence keeps this ticket bounded to category/filtering work inside the existing test layout.
- Existing blocking follow-up work already covers adjacent slices such as unit-test category work (06EXB80FPE3REH11RQ1YR6BW1G) and opt-in Postgres switch work (06EXB7JEF55Y007XK28DAD1E2R), so this ticket should stay focused on the shared provider-integration category contract.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Separate required local provider checks from opt-in external database checks.

## Current Baseline
- SQLite has a required local integration path and an optimized provider package registration path through `AddDVaultSqlite`.
- PostgreSQL, SQL Server, Oracle, and MySQL provider packages exist, but external database integration checks must be opt-in unless configured.

## Scope
- Default CI runs SQLite integration tests and provider registration smoke tests.
- PostgreSQL, SQL Server, Oracle, and MySQL external integration tests run only when configured.
- Tests make clear when `AddDVaultSqlite` is required for optimized SQLite behavior versus the core `AddDVault` fallback path.

## Acceptance Criteria
- Skipped external provider tests explain missing configuration.
- SQLite tests validate the sample scenarios and the optimized provider path.
- Provider packages without external database configuration have registration/package smoke coverage.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.