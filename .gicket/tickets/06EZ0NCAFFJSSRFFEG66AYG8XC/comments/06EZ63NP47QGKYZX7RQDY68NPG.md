[gicket-bot] PO-critic review contract

Summary
- Refinement is now sufficiently bounded for developer handoff: v1 comparison reporting is explicitly limited to SQLite plus opt-in PostgreSQL, the benchmark discovery contract is defined, and compatibility-only providers are kept out of scope.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The latest persisted contract answers the prior PO-critic gaps in .gicket/tickets/06EZ0NCAFFJSSRFFEG66AYG8XC/comments/06EZ55C321GXMHVGRAHZZJZTEM.md:10-15 and records Open questions = none at lines 39-40.
- Branch-history inspection via git --no-pager log --oneline --decorate --left-right --cherry-pick develop...ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting --max-count 30 showed only workflow/ticket commits on the story branch (acddce88, 154409fb, ba16e7d1, 35c3fc6a, 337a1b0a, 1e6676c2, 42e61715, d7a4194b) and no ticket implementation commits yet.
- The current benchmark surface is SQLite-only: benchmarks/DCoding.Data.DVault.Benchmarks/README.md:9-24, BenchmarkRunner.cs:7-79, BenchmarkArtifacts.cs:9-156, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:12-191 all describe/assert SQLite provider rows and the current artifact columns only.
- Repository architecture matches the refined scope boundary: docs/architecture/dvault-v1-explicit-save-service.md:47-55 says SQLite benchmark coverage is required, PostgreSQL benchmark coverage is No, and SQL Server/Oracle/MySQL are compatibility-only with no benchmark baseline in the current release matrix.
- Source evidence confirms PostgreSQL is the only current external optimized strategy: src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-20 registers PostgresDataVaultSaveStrategy, and src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs:12-27 self-gates on Npgsql; src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:14-19, src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:14-19, and src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:14-19 only call services.AddDVault().
- The refined discovery contract reuses existing repository evidence: tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs:3-35 defines DVAULT_TEST_POSTGRES_CONNECTION_STRING, and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:16-19 conditionally restores Npgsql only when that env var is set.
- Relationship/ticket evidence is aligned: the story is parentOf completed child ticket 06EZ0NCGYCADKEYGR16J5PJFS0 (.gicket/relations/XC/S0/06EZ0NCAFFJSSRFFEG66AYG8XC--06EZ0NCGYCADKEYGR16J5PJFS0--parentOf.json:3-5; .gicket/tickets/06EZ0NCGYCADKEYGR16J5PJFS0/ticket.json:3-18), and that child contract explicitly left external-provider expansion to follow-up work (.gicket/tickets/06EZ0NCGYCADKEYGR16J5PJFS0/description.md:4-18).
- The upstream blocking story relation still exists (.gicket/relations/VR/XC/06EZ0N8HW9PZAFKMM5WQD564VR--06EZ0NCAFFJSSRFFEG66AYG8XC--blocks.json:3-5), but the blocking story itself is already done (.gicket/tickets/06EZ0N8HW9PZAFKMM5WQD564VR/ticket.json:3-19), so there is no remaining product-clarity blocker from that dependency.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Whitespace-only DVAULT_TEST_POSTGRES_CONNECTION_STRING should behave the same as missing configuration and still emit skipped PostgreSQL rows.
- Env var present but PostgreSQL provider dependency unavailable should still produce deterministic skipped rows rather than a silent omission.
- Env var present and database reachable, but the PostgreSQL optimized path cannot execute for a scenario/context, should be classified consistently rather than disappearing from the artifact.

Risky assumptions
- The story assumes benchmark-side optional PostgreSQL support can be added without widening normal local dependency requirements, even though benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj:13-21 currently references only SQLite packages.
- The story assumes reusing the test-named DVAULT_TEST_POSTGRES_CONNECTION_STRING variable is acceptable UX if documentation and skipped-row reasons are explicit.
- The story assumes execution-status and skip semantics can be added to the existing markdown/CSV/JSON artifact family without destabilizing the archiveable report shape.

AC / test suggestions
- Add explicit validation that a SQLite-only run still emits PostgreSQL skipped rows with the normalized not configured reason.
- Add explicit validation for the provider dependency unavailable and connection unreachable skip categories so artifact semantics stay stable across machines.
- Assert the stable artifact shape across markdown, CSV, and JSON includes execution-status values for both executed and skipped rows.

Implementation watchouts
- Keep provider selection and skip reporting in the benchmark/documentation surface; the core dispatcher boundary in docs/architecture/dvault-v1-explicit-save-service.md:31-35 should remain provider-neutral.
- Do not reopen MySQL, Oracle, or SQL Server inside this story; the refined contract excludes them and current source surfaces are compatibility-only.
- Current benchmark tests hardcode SQLite row counts and provider text, so developer work will need coordinated artifact and test expectation updates rather than ad hoc extra rows.

Non-blocking notes
- git diff --name-only develop...ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting shows only .gicket ticket metadata/comment files on the branch, so this review is purely about the delivery contract and ticket state.

Split recommendations
- Keep SQL Server, Oracle, and MySQL benchmark expansion in separate provider tickets.
- If benchmark-specific configuration surfaces or CI provisioning become necessary, split that infrastructure work from this artifact-contract story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment