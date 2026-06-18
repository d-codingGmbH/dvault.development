<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the repository-backed MySQL latest-satellite baseline: MySQL still has no latest-satellite provider strategy registration, the checked-in benchmark and test guidance still asserts provider-neutral fallback for that shape, and no branch-local implementation delta was visible from the supplied scratch ref.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current repository baseline is explicit: SQLite is the only optimized latest-satellite provider path; MySQL currently registers PIT and bridge read strategies only, not a latest-satellite `IDataVaultProviderReadStrategy`.
- For this ticket, closing the gap means either implementing a real MySQL latest-satellite strategy end to end or closing the item with explicit no-work-required or rejection documentation; partial code or evidence-only churn does not satisfy the ticket.
- No repository-local v0.41 planning or release artifact was visible in this branch, so the operative criteria surface for this refinement is the checked-in matrix, benchmark, and test contract already present in the repository.
- Benchmark evidence for this ticket is bounded to the existing provider-evidence surfaces: checked-in guidance rows and automated expectations may change, but measured MySQL timing must not be claimed unless provider-configured artifacts are actually produced.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

### Scope In
- The MySQL latest-satellite read decision only, including provider strategy registration or dispatch, diagnostics posture, fallback behavior, benchmark guidance rows, and automated coverage for that single read shape.
- The no-work-required closure path if repository-backed investigation concludes MySQL latest-satellite optimization should remain rejected in the current baseline.

### Scope Out
- MySQL PIT or bridge strategy changes beyond preserving their existing baseline.
- PostgreSQL, SQL Server, Oracle, DB2, or SQLite latest-satellite work.
- MySQL save-strategy work, staging thresholds, or non-read provider behavior.
- Any claim of measured MySQL latest-satellite performance without configured provider benchmark artifacts.

## Acceptance Criteria
- Repository evidence remains internally consistent for MySQL latest-satellite reads: either a MySQL-specific latest-satellite strategy is added and visibly registered or selected where appropriate, or the ticket lands explicit no-work-required or rejection documentation that preserves the current provider-neutral fallback baseline.
- If a MySQL latest-satellite strategy is added, provider-neutral fallback remains intact for the bounded unsupported cases already implied by the repository baseline: provider mismatch, non-hub-parent satellites, and multi-active driving keys.
- Diagnostics and automated tests cover the MySQL latest-satellite decision boundary so the repository no longer relies on implicit behavior for this shape.
- The benchmark evidence surface for MySQL latest-satellite reads is updated to match the chosen outcome: implementation updates the checked-in guidance or evidence expectations for the MySQL row, while rejection keeps the row as a no-strategy fallback case and documents why.
- No ticket outcome may regress the established PIT or bridge MySQL posture or restate skipped-placeholder guidance as measured external-provider timing.

## Definition of Done
- The code, tests, and docs baseline clearly states whether MySQL latest-satellite optimization exists or is explicitly rejected in the current release posture.
- Automated coverage proves the selected MySQL latest-satellite behavior and its fallback boundary.
- Checked-in benchmark guidance or evidence surfaces and related tests align with the selected MySQL latest-satellite outcome.
- Any no-work-required closure cites the existing evidence matrix, gap matrix, and root benchmark posture instead of leaving the ticket as an undocumented open gap.

## Implementation Notes
- Current baseline evidence: `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` registers MySQL PIT and bridge read strategies only, while `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` is the only provider extension that registers `IDataVaultProviderReadStrategy` for latest-satellite reads.
- Current code shape: `src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs` implements PIT and bridge behavior only, and `DataVaultProviderReadStrategyGateEvaluator` exposes MySQL gate evaluation for PIT and bridge but not a MySQL latest-satellite path.
- Current evidence surfaces already pin the live baseline: `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `benchmark-summary.md`, and `BenchmarkScenarioExecutionTests` all assert that MySQL latest-satellite reads have `selectedStrategy=<none>` and `providerSpecificReadStrategy=not registered for latest satellite reads`.
- The supplied scratch source and current branch showed no visible implementation delta for this ticket, so the branch still needs either the closing implementation or explicit no-work-required documentation.
- Ticket and comment snapshots in the prompt showed no human comments; live gicket read-tool retries were trust-blocked, so refinement relied on the prompt snapshot plus repository evidence.

## Open Questions
- none

## Follow-Up Questions
- If the team later wants measured MySQL latest-satellite timing rather than guidance-only evidence, should that be tracked as a separate provider-configured benchmark ticket after this closure decision lands?

## Risks
- The main delivery risk is a partial implementation that adds some MySQL-specific code but leaves benchmark guidance, diagnostics, or tests asserting the old no-strategy baseline.
- The checked-in root benchmark triplet currently keeps MySQL external-provider rows as skipped placeholders when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset, so measured performance claims remain easy to overstate unless guarded carefully.

## Split Recommendations
- No split recommended; this is one bounded provider and shape closure decision and should either land end to end or close with explicit no-work-required documentation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use v0.39 evidence and v0.41 criteria to implement or reject a MySQL latest-satellite read strategy improvement. Acceptance: tests, diagnostics, fallback, and benchmark evidence are updated, or no-work-required is documented.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Implemented MySQL latest-satellite read dispatch through `MySqlDataVaultReadStrategy` and `AddDVaultMySql()`.
- Preserved provider-neutral fallback for provider mismatch, link-parent or non-hub satellites, and multi-active driving-key latest-satellite shapes.
- Updated benchmark guidance and provider evidence/gap matrices so MySQL latest-satellite is a diagnostics-gated strategy candidate, not an unregistered capability gap, while still not claiming measured MySQL timing without `DVAULT_TEST_MYSQL_CONNECTION_STRING`.

### Verification
- `dotnet build tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --framework net10.0 --no-restore --no-dependencies --nologo` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --framework net10.0 --no-build --nologo --filter <targeted filters>` passed; the runner ignored the VSTest filter and ran the full net10 unit suite: 590 passed.
- `dotnet build tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --framework net10.0 --no-restore --no-dependencies --nologo` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --framework net10.0 --no-build --nologo --filter FullyQualifiedName~BenchmarkScenarioExecutionTests` passed; the runner ignored the VSTest filter and ran the full net10 integration suite: 215 passed, 23 skipped for missing optional provider connection strings.
- Targeted `dotnet format whitespace ... --include <changed C# files>` passed.
- Full `tools/check-format.sh` timed out after 180 seconds without output in this adapter; changed-file UTF-8, LF, trailing-whitespace, and final-newline checks passed.

### Notes
- Full solution build was attempted and produced successful outputs for the core and MySQL projects, but the solution-wide build was stopped after it became impractical on the Windows-mounted checkout. NuGet emitted NU1900 warnings because vulnerability-cache writes were blocked by the sandbox read-only cache path.
- No measured MySQL latest-satellite timing is claimed; the root optional-provider row remains skipped until a MySQL connection string is configured.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added rework coverage for MySQL latest-satellite live execution in `tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs`.
- The new opt-in test persists two satellite versions with `AddDVaultMySql()`, verifies diagnostics select `MySqlDataVaultReadStrategy`, and verifies latest/as-of rows through `ReadLatestSatelliteRowsAsync`.
- Existing benchmark posture remains unchanged: no measured MySQL timing is claimed without `DVAULT_TEST_MYSQL_CONNECTION_STRING`.

### Verification
- `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --framework net10.0 --no-restore --nologo --filter FullyQualifiedName~MySqlExplicitDataVaultSaveServiceTests` passed; Microsoft Testing Platform ignored the VSTest filter and ran the full net10 integration suite: 215 passed, 24 skipped.
- The new MySQL live test skipped in this environment because `DVAULT_TEST_MYSQL_CONNECTION_STRING` is not configured.
- `bash tools/check-format.sh` passed.
- `git diff --check -- tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs` passed.

### Notes
- Live MySQL execution remains opt-in; a configured tester should rerun the MySQL integration class with `DVAULT_TEST_MYSQL_CONNECTION_STRING` set to exercise the provider query against MySQL.
<!-- gicket-bot:developer-delivery:v1:end -->