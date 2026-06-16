<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence shows the accepted SQL Server bulk save improvement is already present, with strategy registration, gate thresholds, smoke coverage, diagnostics and fallback coverage, and benchmark contract updates; no split or persistent planning write was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Visible repository evidence supports the accepted-and-implemented path, not the spike-rejected path; treat this ticket as ratifying or closing the already-landed SQL Server provider-native bulk save implementation.
- For this ticket, 'benchmark evidence updated' means the repository preserves SQL Server `provider-native-bulk-ingestion` benchmark row identity, execution-detail guidance, and verifier coverage; checked-in optional-provider timing may still remain `skipped-placeholder` when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset.
- Live ticket comments, relations, and attachments were not re-read because the bounded `gicket-read-*` calls were trust-blocked; this refinement relies on the supplied ticket snapshot plus bounded repository evidence, and no relation or planning writes were materialized.

### Scope In
- SQL Server provider-native bulk save behavior through `AddDVaultSqlServer()` and `SqlServerDataVaultSaveStrategy`.
- The bounded SQL Server save gate and fallback contract: provider-name match, clean `DbContext`, no multi-active satellite operations, minimum 50 total operations, and maximum 500 satellite operations.
- Unit, integration, smoke, and benchmark-verifier coverage that proves SQL Server strategy selection, fallback causes, transaction and cancellation behavior, and benchmark execution-detail facts for `provider-native-bulk-ingestion`.

### Scope Out
- New SQL Server latest-satellite optimization; the visible baseline still records no provider-specific latest-satellite read strategy.
- Completed SQL Server PIT or bridge timing evidence or broader SQL Server read-strategy expansion beyond the already-visible diagnostics-gated candidate baseline.
- Cross-provider optimization work for PostgreSQL, MySQL, Oracle, or DB2.
- Provisioning an opt-in external SQL Server environment or requiring checked-in completed timing rows when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unavailable.

## Acceptance Criteria
- The ticket is satisfied only against the existing SQL Server save boundary: `AddDVaultSqlServer()` registers `SqlServerDataVaultSaveStrategy`, and the provider-specific path is selected only for clean SQL Server contexts that meet the native bulk gate.
- Diagnostics and fallback coverage prove the SQL Server save path fails closed for provider mismatch, dirty contexts, multi-active satellite operations, batches below 50 total operations, and batches above 500 satellite operations.
- SQL Server smoke and integration coverage prove representative hub, link, satellite, ordered bulk, transaction-participation, and cancellation behavior for the provider-specific save path when the optional SQL Server provider is configured.
- The benchmark contract and verifier preserve SQL Server `provider-native-bulk-ingestion` rows and execution-detail facts, including `transfer=SqlBulkCopy` and `nativeBulkBoundary=50-plus-operations`, without requiring checked-in completed timing when the SQL Server connection string is unset.

## Definition of Done
- PO handoff text treats the SQL Server bulk improvement as an already-landed bounded implementation and does not send development back to rediscover or redesign the strategy.
- Closure or later handoff text does not overclaim scope beyond the proven baseline; in particular it does not claim SQL Server latest-satellite optimization or completed optional-provider timing results that the current repository does not prove.
- No additional split, child ticket, relation rewrite, or planning document is required from the visible repository evidence for this ticket to proceed to PO-critic review.

## Implementation Notes
- Repository evidence for the landed save implementation and registration is in `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs` and `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`.
- Representative SQL Server execution coverage already exists in `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs`, gate and threshold coverage exists in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs`, and benchmark row and guidance coverage exists in `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs`, `benchmark-summary.md`, and `docs/plans/provider-optimization-gap-matrix.md`.
- A bounded `git diff --name-only 9cd77c080cff2a547ee25bedcc15f0c45b21cdee..HEAD` returned no paths, so this ticket branch currently carries no additional repository delta beyond the existing baseline.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

## Open Questions
- none

## Follow-Up Questions
- When a configured SQL Server environment is available, should a later evidence-gap ticket replace the checked-in SQL Server `skipped-placeholder` benchmark rows with completed `provider-native-bulk-ingestion` timing artifacts?
- Should later backlog work create separate SQL Server tickets for latest-satellite optimization or configured PIT and bridge timing evidence, both of which remain explicit follow-up gaps rather than scope for this ticket?

## Risks
- Because the bounded `gicket-read-ticket`, `gicket-read-ticket-comments`, `gicket-read-ticket-relations`, and `gicket-read-ticket-attachments` calls were trust-blocked, live persisted relation, comment, and attachment state was not independently re-verified in this run.
- The checked-in benchmark baseline still shows SQL Server optional-provider rows as skipped when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset; reviewers must not reinterpret the current repository as already carrying completed SQL Server timing evidence.
- If the ticket is treated as a fresh implementation task instead of a closure-oriented ratification of landed code, the next workflow step risks duplicate development against an already-proven baseline.

## Split Recommendations
- No split recommended; current repository evidence already bounds this ticket to ratifying or closing the landed SQL Server native bulk save implementation and its existing diagnostics, smoke, and benchmark-contract coverage.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement the accepted SQL Server bulk improvement, if the spike recommends one. Acceptance: provider strategy tests, diagnostics/fallback coverage, and benchmark evidence are updated; close with no-work-required if the spike rejects implementation.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Decision: `already_satisfied_on_branch`

Dev verification confirms the checked-out ticket branch already contains the accepted SQL Server provider-native bulk save implementation. No repository edits were made in this dev pass.

Repository evidence:
- `AddDVaultSqlServer()` registers the SQL Server capability profile and `SqlServerDataVaultSaveStrategy`.
- `SqlServerDataVaultSaveStrategy` uses the SQL Server provider gate with the 50 total-operation minimum and 500 satellite-operation maximum.
- Diagnostics, SQL Server smoke coverage, benchmark verifier coverage, `benchmark-summary.md`, and the provider gap matrix preserve the bounded SQL Server bulk-save contract and the skipped-placeholder timing boundary when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset.

Verification:
- Targeted no-restore test execution was attempted, but the local NuGet cache was missing `Microsoft.EntityFrameworkCore.Analyzers` 8.0.28 and 10.0.9, producing NETSDK1064 before tests ran.
- Rerun `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh` in a restored workspace.

Scope note:
- This handoff does not claim SQL Server latest-satellite optimization or completed SQL Server optional-provider benchmark timing.
<!-- gicket-bot:developer-delivery:v1:end -->