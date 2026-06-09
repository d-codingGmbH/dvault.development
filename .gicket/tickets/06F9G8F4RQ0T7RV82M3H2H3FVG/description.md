<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around the already-visible dual-target repository baseline, keeping the work focused on deterministic provider-version and package-dependency-line proof while treating multitargeting as completed prerequisite scope and broader verifier/CI expansion as sibling follow-up work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already pins the v1 matrix in src/DCoding.Data.DVault/DCoding.Data.DVault.csproj and the Unit and Integration test projects, so this story should lock that visible matrix with deterministic tests instead of reopening provider or version selection.
- The supported-provider test baseline for this story is the finite five-provider set already documented in repository planning material: SQLite, PostgreSQL, Oracle, SQL Server, and MySQL.
- MySQL coverage for this story follows the checked-in MySql.EntityFrameworkCore 10.0.7 package on both target frameworks; Pomelo-specific proof is not part of this bounded v1 ticket.
- External-provider database execution remains opt-in behind the existing connection-string switches; default local validation must stay runnable without containers or live external databases.
- Broader package verifier metadata, symbols, README, XML docs, and CI/manual-guidance expansion remains with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8; this story only needs the EF/provider matrix and dependency-line proof.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass because the existing evidence already bounded the story.

### Scope In
- Deterministic repository tests that assert the exact EF/provider package version matrix for net8.0 and net10.0 across the core and provider-support validation path.
- Deterministic package-artifact dependency checks proving packed outputs expose the intended 8.x or 10.x EF/provider dependency line for the corresponding target framework.
- Coverage that preserves the existing net8 helper exclusions and the existing external-provider opt-in gates while still validating the matrix.
- Clear failure diagnostics that identify the drifting provider, package version, and target framework when the matrix changes unexpectedly.

### Scope Out
- Retargeting benchmarks, tools/DCoding.Data.DVault.PackageVerification, analyzers, or analyzer tests to net8.0.
- Broader package verification concerns such as README, symbols, XML docs, nuspec metadata, or CI/manual publication guidance already owned by 06F9G8FBQTAPXXS1Y4NR5QKVG8.
- New runtime provider behavior, new supported providers, or reopening the completed multitarget project-set decision from done ticket 06F9G8EXXFJJ1SWWQXC2N9P2X8.
- Mandatory live MySQL, PostgreSQL, Oracle, or SQL Server execution in the default local test lane.

## Acceptance Criteria
- A deterministic test lane asserts that the net8.0 line resolves Microsoft.EntityFrameworkCore.Sqlite 8.0.27, Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11, Oracle.EntityFrameworkCore 8.23.26200, Microsoft.EntityFrameworkCore.SqlServer 8.0.27, and MySql.EntityFrameworkCore 10.0.7 where the corresponding opt-in provider references are enabled.
- A parallel deterministic test lane asserts that the net10.0 line resolves Microsoft.EntityFrameworkCore.Sqlite 10.0.8, Npgsql.EntityFrameworkCore.PostgreSQL 10.0.2, Oracle.EntityFrameworkCore 10.23.26200, Microsoft.EntityFrameworkCore.SqlServer 10.0.8, and MySql.EntityFrameworkCore 10.0.7 where the corresponding opt-in provider references are enabled.
- Deterministic package inspection proves the produced packable artifacts expose the intended EF/provider dependency group for each target line and do not mix EF Core 8 and EF Core 10 dependencies inside one target-framework group.
- Default no-connection local coverage remains runnable without external databases, while live external-provider database tests stay behind the existing DVAULT_TEST_*_CONNECTION_STRING opt-in switches.
- The new coverage does not make BenchmarkScenarioExecutionTests.cs, PackageVerifierTests.cs, benchmarks/DCoding.Data.DVault.Benchmarks, or tools/DCoding.Data.DVault.PackageVerification mandatory on the net8 compile path.
- Failure output from the new coverage identifies the drifting package, provider, and target framework clearly enough to diagnose version-matrix regressions without manual diffing.

## Definition of Done
- The ticket contract fixes one bounded provider/version matrix and leaves no remaining PO ambiguity about the five-provider baseline or the exact required versions for each target line.
- Repository tests fail deterministically on version drift in project references or packed dependency groups for either target line.
- The default local validation path still works without containers or external databases, and external-provider proof remains opt-in.
- Sibling verifier and CI work can extend broader package checks later without renegotiating the matrix or helper-project boundary established by this story.

## Implementation Notes
- Repository evidence already shows the target-conditioned matrix in src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj, and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj.
- The integration project already removes BenchmarkScenarioExecutionTests.cs on net8 and conditions the benchmark project reference to net10 only; matrix coverage must respect that helper exclusion rather than pulling benchmark support into net8 scope.
- The unit project already removes PackageVerifierTests.cs on net8 and conditions tools/DCoding.Data.DVault.PackageVerification to net10 only; artifact dependency proof must not pull that helper project into the net8 compile path.
- ProviderIntegrationCategoryDiscoveryTests.cs already conditions BenchmarkScenarioExecutionTests under NET10_0; any new coverage should preserve the existing discovery contract rather than broadening default external-provider requirements.
- Done ticket 06F9G8EXXFJJ1SWWQXC2N9P2X8 should be treated as completed historical prerequisite evidence, while ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 remains the bounded home for broader verifier and CI guidance.
- This refinement pass did not materialize child tickets, relation updates, attachments, or planning documents because the current repository and ticket evidence already supported a bounded contract.

## Open Questions
- none

## Follow-Up Questions
- Should ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 later absorb these dependency-line assertions into the reusable package verifier and CI guidance once the focused matrix tests land?
- Should a later compatibility task add explicit Pomelo.EntityFrameworkCore.MySql matrix proof, since repository policy supports both MySQL provider names but the checked-in test matrix currently uses MySql.EntityFrameworkCore?
- After the matrix tests land, should release or adoption documentation call out the exact opt-in external-provider verification commands, or is the existing fixture guidance sufficient?

## Risks
- Because several provider PackageReferences are conditioned on connection-string properties, matrix coverage that relies only on live external-provider execution could miss drift when those opt-in properties are absent; at least one deterministic non-live assertion path is required.
- Package-artifact dependency proof can overlap with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 if implementation expands beyond EF/provider dependency groups into broader nuspec, README, symbol, or metadata verification.
- Repository policy supports a broader MySQL provider-name baseline than this story's checked-in MySql.EntityFrameworkCore matrix, so Pomelo-specific drift would remain outside this ticket unless separately scheduled.

## Split Recommendations
- No additional split is required; the story is bounded if it stays focused on exact provider-version assertions and package dependency-line proof, while broader verifier and CI coverage remains with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add deterministic build/test coverage proving the intended EF provider matrix for net8.0 and net10.0 and proving that produced package artifacts resolve to the intended 8.x or 10.x dependency line. Required test package versions are Microsoft.EntityFrameworkCore.Sqlite 8.0.27 and 10.0.8, MySql.EntityFrameworkCore 10.0.7 for both targets, Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11 and 10.0.2, Oracle.EntityFrameworkCore 8.23.26200 and 10.23.26200, and Microsoft.EntityFrameworkCore.SqlServer 8.0.27 and 10.0.8. Keep external provider database tests opt-in where connection strings or containers are required.