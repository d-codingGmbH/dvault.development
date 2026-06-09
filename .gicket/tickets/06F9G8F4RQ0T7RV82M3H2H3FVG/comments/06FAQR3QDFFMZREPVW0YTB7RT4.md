[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the refined delivery contract is concrete, `## Open Questions` is `none`, and the checked-in repository baseline matches the scoped dual-target five-provider matrix.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F9G8F4RQ0T7RV82M3H2H3FVG/description.md` contains `PO Handoff` = `ready_for_po_critic` and `## Open Questions` -> `- none`.
- `git log --oneline -n 8 ticket/06F9G8F4RQ0T7RV82M3H2H3FVG-story-add-ef-core-provider-version-matrix-tests` shows the handoff commit `1b93897f0` on top of `develop` commit `a1a2a7aa7`, and `git diff --name-only develop..1b93897f0cb1` touches only `.gicket/tickets/06F9G8F4RQ0T7RV82M3H2H3FVG/*` ticket metadata files.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` targets `net8.0;net10.0` and pins `Microsoft.EntityFrameworkCore` `8.0.27` on `net8.0` and `10.0.8` on `net10.0`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` pins the five-provider test matrix from the contract: `Microsoft.EntityFrameworkCore.Sqlite` `8.0.27`/`10.0.8`, `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11`/`10.0.2`, `Oracle.EntityFrameworkCore` `8.<redacted>`/`<redacted>`, `Microsoft.EntityFrameworkCore.SqlServer` `8.0.27`/`10.0.8`, and `MySql.EntityFrameworkCore` `10.0.7` on both target lines, with the non-SQLite providers conditioned on `DVAULT_TEST_*_CONNECTION_STRING`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` removes `BenchmarkScenarioExecutionTests.cs` on `net8.0`, and `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` removes `PackageVerifierTests.cs` on `net8.0` while referencing `tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj` only on `net10.0`.
- `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` includes `BenchmarkScenarioExecutionTests` only under `#if NET10_0`, matching the helper-boundary requirement in the contract.
- `docs/plans/provider-specific-sql-artifact-contract.md` and `docs/plans/provider-read-optimization-evidence-expansion-epic.md` both enumerate the same finite provider baseline: SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Related ticket state is consistent with the contract boundaries: `.gicket/tickets/06F9G8EXXFJJ1SWWQXC2N9P2X8/ticket.json` is `done` and `.gicket/tickets/06F9G8FBQTAPXXS1Y4NR5QKVG8/ticket.json` remains `todo` for the broader verifier/CI follow-up scope.
- Repository MySQL support is broader than this ticket's narrowed matrix: `src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs` and `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` support both `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`, while the refined ticket explicitly limits this story to the checked-in `MySql.EntityFrameworkCore 10.0.7` matrix.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not include a concrete example of the exact packed artifact/framework groups the dependency-line assertion lane should inspect first; that scope is implied from the repository layout rather than spelled out in an example.
- The contract requires drift diagnostics to name the target framework, provider, and package, but it does not give a sample failure-message shape.

Risky assumptions
- Implementation will keep broader README/symbol/XML/nuspec/CI verification in ticket `06F9G8FBQTAPXXS1Y4NR5QKVG8`; the current `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` is still net10-only and focused on package artifact structure plus core-package dependency validation.
- Implementation will treat the MySQL package matrix exactly as narrowed in this ticket (`MySql.EntityFrameworkCore 10.0.7`) even though repository provider-name support remains broader (`Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`).

AC / test suggestions
- If the PO wants to remove the last bit of interpretation drift, add one explicit example naming which packed artifacts and nuspec target-framework groups must be asserted in the dependency-line proof.
- If desired, add one illustrative acceptance-criteria example of the expected drift diagnostic format: target framework + package id + expected version + actual version.

Implementation watchouts
- Do not make `BenchmarkScenarioExecutionTests.cs` part of the `net8.0` compile lane; the integration project currently removes it on `net8.0` and discovery only includes it under `#if NET10_0`.
- Do not make `PackageVerifierTests.cs` or `tools/DCoding.Data.DVault.PackageVerification` mandatory on `net8.0`; the unit project currently excludes/conditions them to `net10.0`.
- Keep live provider execution opt-in behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_MYSQL_CONNECTION_STRING`, and `DVAULT_TEST_ORACLE_CONNECTION_STRING`; the current test and benchmark projects already condition those package references on the env vars.
- Keep the artifact-proof work scoped to dependency-line assertions; do not let it expand into the broader package-verifier and CI guidance work already parked in `06F9G8FBQTAPXXS1Y4NR5QKVG8`.

Non-blocking notes
- The current ticket branch is still pre-development; `git diff --name-only develop..1b93897f0cb1` shows only ticket metadata changes, which is acceptable at this gate.
- The persisted PO refinement comment `comments/06FAQNQZ0RP2JDQNNFNRAKG6R4.md` already records the intended bounded scope, follow-up questions, and the no-split recommendation, and no later ticket comment adds a conflicting requirement.

Split recommendations
- No split recommended; the story is already bounded, and broader verifier/CI/package-guidance scope is explicitly separated into `06F9G8FBQTAPXXS1Y4NR5QKVG8`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment