<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the parent automated-test-strategy story against the current repository baseline; the existing child split (06EXB80FPE3REH11RQ1YR6BW1G, 06EXB80QQHAYH61RY4X3T1E8S0) remains sufficient and no new planning artifacts were required.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already establishes the v1 test layout under tests/DCoding.Data.DVault.Tests with Unit, Integration, and Shared slices, so the story should ratify that structure rather than invent a new taxonomy.
- This parent story already has child relations to 06EXB80FPE3REH11RQ1YR6BW1G and 06EXB80QQHAYH61RY4X3T1E8S0 and currently blocks 06EXB8202A88KJJP7WEGBESBYM; refinement should treat those links as the active delivery split.
- Visible repository evidence shows local SQLite-focused integration coverage and a Postgres-specific opt-in integration configuration pattern; external-provider checks stay optional and must not be part of the default no-service test run.
- Provider packages without configured external database coverage still need bounded smoke coverage such as registration, API surface, discovery, or package-load validation.

### Scope In
- Document and enforce the v1 automated test categories for DVault: unit coverage, local SQLite-backed integration coverage, shared provider-test support, and provider-package smoke coverage.
- Cover the repository baseline called out in the ticket: metadata/model translation, stable hashing behavior, EF model building, convention-first registration, explicit save flows, SQLite integration behavior, and provider registration/package smoke checks.
- Keep opt-in external provider verification within scope only as configuration-gated coverage that is clearly separated from default local automation.

### Scope Out
- Requiring PostgreSQL, SQL Server, Oracle, or MySQL servers for the default automated test run.
- Expanding this story into provider-specific performance tuning, non-MVP Data Vault capabilities, or broad product-code changes outside test strategy and test coverage.
- CI environment provisioning or rollout policy beyond making the intended default-versus-opt-in test boundary explicit.

## Acceptance Criteria
- The ticket documents the repository test taxonomy using the existing Unit, Integration, and Shared baseline and explains which checks count as local default coverage versus opt-in external-provider coverage.
- Default automated test execution through the repository solution does not require external services and remains valid with only local prerequisites.
- Required local coverage explicitly includes the AddDVault fallback path, the AddDVaultSqlite optimized provider path where behavior differs, stable hashing/normalization behavior, EF metadata/model translation, and SQLite save/schema behavior.
- Provider-package coverage makes clear which packages currently have only local smoke coverage and which checks require an explicitly configured external database environment.

## Definition of Done
- The agreed test categories and boundaries are reflected in repository-facing documentation, ticket notes, or test project organization closely enough that a developer can tell what belongs in default automation versus opt-in runs.
- The resulting tests and supporting artifacts continue to align with the repository entry point in DVault.slnx and the shared implementation standards referenced by this story.
- Any work delivered under this parent story preserves the existing child-ticket split and keeps downstream dependencies accurate.

## Implementation Notes
- Use the existing tests/DCoding.Data.DVault.Tests structure as the implementation baseline instead of creating a parallel test tree.
- Treat SQLite as the required local integration provider baseline because the visible repository test inventory already contains dedicated SQLite integration coverage and the main translator baseline is SQLite-oriented.
- Use provider-package smoke checks for MySQL, Oracle, SQL Server, and Postgres packages when no live external harness is configured; examples include registration-path validation, API surface snapshots, and provider discovery/package-load checks.
- Keep external-provider integration tests explicitly gated by environment configuration so the default repository test path stays deterministic for unattended local and CI automation.
- The existing child tickets 06EXB80FPE3REH11RQ1YR6BW1G and 06EXB80QQHAYH61RY4X3T1E8S0 should remain the implementation vehicles underneath this parent story.

## Open Questions
- none

## Follow-Up Questions
- After the parent story lands, should SQL Server, Oracle, and MySQL each get their own opt-in external integration harness ticket, or should they remain smoke-only until provider priorities change?
- Does blocked ticket 06EXB8202A88KJJP7WEGBESBYM need a later CI or developer-documentation follow-up to describe how optional provider runs are invoked when environments are available?

## Risks
- Because the story mixes strategy documentation with broad test implementation scope, it can sprawl unless contributors keep the work constrained to the existing child-ticket split.
- External-provider expectations may become inconsistent across packages if the repository does not clearly label which checks are smoke-only and which are true configured integration tests.

## Split Recommendations
- No additional split is required in this refinement pass because the parent story already has two child tickets; keep new provider-specific live-database work in separate future tickets instead of widening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Define and implement a balanced unit and integration test strategy.

## Current Baseline
- Required local coverage should include the core `AddDVault` fallback path and the `AddDVaultSqlite` optimized provider path where behavior differs.
- PostgreSQL, SQL Server, Oracle, and MySQL external database checks must remain opt-in unless the environment is explicitly configured.
- Provider packages that do not yet have external integration coverage still need registration and package smoke coverage.

## Scope
- Cover metadata, hashing, EF model building, SQLite integration, provider registration, and optional external provider checks.

## Acceptance Criteria
- Test categories are documented.
- Default tests do not require external services.
- Provider-package tests make clear which checks are local smoke checks and which checks require a configured database server.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.