<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the SQL Server threshold task against the completed v0.32.0 baseline, ratified the current 50-minimum/500-satellite gate baseline, and narrowed the work to measured SQL Server tuning plus removal of misleading planned-path diagnostics; no persistent planning writes were applied.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Prerequisite evidence ticket 06F9XD26D2MHVAKZ2GCZ67BEFC is done; its v0.32.0 artifact bundle is the evidence source for this task, so the live incoming blocks relation is historical rather than a blocker.
- Repository baseline already fixes the SQL Server gates at 50 minimum total operations and 500 maximum satellite operations through DataVaultDiagnostics, telemetry explanations, performance guidance, and the activity-tracing fallback contract.
- The scale benchmarks analyze only the satellite bulk request after hub rows are written, so 10x1 = 10 satellite operations, 100x1 and 10x10 = 100, and 1000x1 and 100x10 = 1000; that already explains why only the 100-operation rows currently select SQL Server.
- The concrete ambiguity to resolve is in benchmark detail generation: scale rows still prepend SQL Server staged native bulk execution wording even when diagnostics report saveStrategyStatus=ProviderNeutralFallback and selectedStrategy=<none>.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- Capture comparable SQL Server before/after evidence under the shared benchmark artifact contract and reuse the completed v0.32.0 baseline bundle as the pre-tuning reference.
- Re-evaluate the SQL Server 50-minimum and 500-maximum-satellite save gates for the customer-profile scale rows and any directly comparable SQL Server provider-native bulk rows where measured evidence justifies a change.
- Fix benchmark and diagnostics wording so completed rows that actually fell back do not claim that the SQL Server staged/native path executed.
- Preserve or extend tests around SQL Server gate evaluation, telemetry and diagnostics explanations, benchmark execution-detail reporting, and save-path semantics.

### Scope Out
- Changing PostgreSQL, MySQL, or Oracle thresholds owned by sibling tickets under story 06F9XD1T3TJK7NEBYNVT2JEPZW.
- Inventing a new benchmark artifact format or replacing the shared before/after benchmark-summary triplet contract.
- New provider packages, DB2 work, or Podman orchestration changes.
- Forcing SQL Server provider-native dispatch for batches above 500 satellite operations without measured evidence and preserved semantics.

### Benchmark Environment
- Use a local Podman SQL Server container named `sqlserver` from `mcr.microsoft.com/mssql/server:2025-latest`, published as `1433:1433`, with `ACCEPT_EULA=Y`, `MSSQL_PID=Developer`, and the local benchmark SA password `Local-Secret-12345!`.
- If the container is missing, recreate it with `podman run -d --name sqlserver -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD="Local-Secret-12345!" -e MSSQL_PID=Developer -p 1433:1433 mcr.microsoft.com/mssql/server:2025-latest`.
- For SQL Server-only benchmark runs, clear `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_MYSQL_CONNECTION_STRING`, and `DVAULT_TEST_ORACLE_CONNECTION_STRING` first so provider selection is not polluted by another local database.
- Start the container with `podman start sqlserver`, set `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` to `Server=127.0.0.1,1433;Database=master;User Id=sa;Password=Local-Secret-12345!;TrustServerCertificate=True;Encrypt=False;Connection Timeout=5`, and pass the matching non-secret MSBuild marker `-p:DVAULT_TEST_SQLSERVER_CONNECTION_STRING=Configured`.
- The expected benchmark shape is `dotnet run --project benchmarks\DCoding.Data.DVault.Benchmarks\DCoding.Data.DVault.Benchmarks.csproj --configuration Release -p:DVAULT_TEST_SQLSERVER_CONNECTION_STRING=Configured -- --provider sqlserver --scale --iterations 5 --warmup 1 --output artifacts\benchmarks\<ticket-labeled-sqlserver-output>`.

## Acceptance Criteria
- A SQL Server before/after evidence bundle exists under one ticket-labeled artifacts/benchmarks path with before and after benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json files produced with comparable run inputs and explicitly tied back to the completed v0.32.0 baseline bundle.
- The before/after evidence explains the SQL Server scale outcomes at 10, 100, and 1000-plus satellite operations and records whether the 50 minimum gate or 500 satellite ceiling changed or stayed unchanged.
- Completed SQL Server optimized-lane rows no longer present planned-path wording as if SqlServerDataVaultSaveStrategy executed when diagnostics show ProviderNeutralFallback; the actual selected strategy and fallback causes remain visible in diagnostics and artifacts.
- If the 500-satellite ceiling stays in place, diagnostics and guidance clearly explain that the SQL Server candidate declined and provider-neutral fallback executed; if it changes, the before/after evidence shows a measured win over provider-neutral fallback for the newly eligible rows.
- Tests cover transaction participation, cancellation, idempotency, row ordering, hash key/hash diff, load timestamp, record source behavior, and the SQL Server gate and diagnostic-reporting changes, and dotnet test DVault.slnx --nologo plus bash tools/check-format.sh pass.

## Definition of Done
- The ticket leaves one authoritative answer for SQL Server scale rows: which rows are intentionally provider-neutral fallback, which rows actually use provider-native dispatch, and why.
- The public fallback vocabulary stays SqlServerMinimumOperationThreshold and SqlServerMaximumSatelliteOperationThreshold; any messaging improvement reuses those causes instead of inventing new public enums.
- Benchmark execution-detail generation and its tests no longer contradict actual SQL Server strategy status for the scale rows.
- The preserved or adjusted 500-satellite posture is backed by measured SQL Server before/after evidence rather than by planned strategy-family wording alone.
- No sibling provider ticket needs reopening to interpret SQL Server-specific findings from this task.

## Implementation Notes
- DataVaultProviderSaveStrategyGateEvaluator currently hard-codes SQL Server at MinimumSqlServerOptimizedBatchOperationCount = 50 and MaximumSqlServerOptimizedSatelliteOperationCount = 500, and the matching gate requirements, threshold facts, telemetry explanations, performance-profile guidance, and activity-tracing fallback names already rely on that same baseline.
- CustomerProfileBulkDataVaultBenchmark analyzes only the satellite DataVaultBulkSaveRequest after hub insertion, so the effective SQL Server gate count is TotalChangeCount satellite operations; the current positive-control scale rows are customer-profile-scale-100x1 and customer-profile-scale-10x10, both at 100 operations.
- The completed v0.32.0 evidence already shows the exact rows driving this task: customer-profile-scale-10x1 reports SqlServerMinimumOperationThreshold, while the 1000-plus-satellite rows report SqlServerMaximumSatelliteOperationThreshold even though their execution detail still starts with DVault SQL Server staged native bulk save path.
- The misleading planned-path wording currently comes from benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs and CustomerProfileBenchmarks.cs through BenchmarkExecutionDetails.CreatePlanned and CreateSaveStrategyDetail; align those surfaces with actual diagnostics rather than only the requested strategy family.
- Keep the implementation bounded to SQL Server write-path selection and reporting. No persistent planning writes were applied during refinement.

## Open Questions
- none

## Follow-Up Questions
- After SQL Server tuning lands, should a later docs or release ticket promote one SQL Server before/after bundle into the root checked-in benchmark-summary rollup, or should the root rollup stay a lightweight shared baseline?
- If the 500-satellite ceiling proves intentionally protective, should a later UX or docs pass add friendlier benchmark-facing wording beyond the bounded diagnostics strings required here?

## Risks
- The current scale benchmark detail generator can keep misleading SQL Server staged native bulk wording even when diagnostics prove provider-neutral fallback, which can hide whether a threshold change actually altered execution.
- The 1000-plus-satellite rows currently look better than provider-neutral fallback even while remaining fallback executions, so changing the 500 ceiling without verifying actual provider-native semantics could create false performance conclusions.
- Because the benchmark writes hubs separately from the analyzed satellite bulk request, anyone reasoning from total end-to-end row counts instead of satellite-operation counts can misread why specific scale rows cross or miss the SQL Server gate.
- Live SQL Server evidence still depends on the shared Podman sqlserver endpoint, so environment drift can blur threshold conclusions with infrastructure noise.

## Split Recommendations
- No new split is justified. SQL Server threshold tuning and SQL Server fallback-versus-executed diagnostics wording are one bounded refinement surface under story 06F9XD1T3TJK7NEBYNVT2JEPZW.
- If a later release or documentation pass needs broader artifact-lane wording changes after all provider-tuning tickets finish, keep that as follow-up work on 06F8KZVRARQPG482YKCQ686PNM instead of widening this task now.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Investigate and tune SQL Server save strategy thresholds where the all-provider benchmark shows optimized rows falling back.

Observed seed evidence:
- `customer-profile-scale-10x1` reports `SqlServerMinimumOperationThreshold`.
- Larger SQL Server optimized rows report `SqlServerMaximumSatelliteOperationThreshold`.
- Some rows include both optimized-path wording and fallback diagnostics; reconcile the diagnostics so users can tell whether the provider strategy really executed or declined.

Scope:
- Review SQL Server provider save strategy threshold constants, eligibility gates, and diagnostics detail.
- Tune large-batch eligibility only where the staged/native path is measurably better than provider-neutral fallback and still preserves semantics.
- If the current maximum threshold is intentionally protective, keep it and improve diagnostics/guidance instead of forcing an unsafe provider path.

Podman test environment:
- Use the existing `sqlserver` Podman container for opt-in integration checks and benchmark before/after evidence.
- Run the benchmark harness through the same v0.32.0 evidence path created by the baseline task.

Acceptance criteria:
- SQL Server before/after artifacts show the effect of any changed threshold or document why no threshold change is safe.
- Diagnostics clearly distinguish selected provider strategy, provider strategy decline, and provider-neutral fallback.
- Transaction participation, cancellation behavior, idempotency, row ordering, hash key/hash diff, load timestamp, and record source behavior remain covered by tests.
- `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` pass.
