[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9XD2M71D1XFT7FJX62KD8HM/description.md contains Open Questions: none and acceptance criteria that call for one ticket-labeled before/after benchmark-summary.md/.csv/.json bundle, SQL Server <redacted>-plus outcome explanation, preserved SqlServerMinimumOperationThreshold/SqlServerMaximumSatelliteOperationThreshold vocabulary, and dotnet test DVault.slnx --nologo plus bash tools/check-format.sh.
- git diff --name-status develop...4680b9de3febed49d1f57a5a09d20b0a6bb1fae7 changes only .gicket/tickets/06F9XD2M71D1XFT7FJX62KD8HM/*; no source, benchmark, docs, or test files are changed on the current branch yet.
- .gicket/tickets/06F9XD26D2MHVAKZ2GCZ67BEFC/comments/06FA416G38B6WRMRPP7KG9VZK8.md records the exact pre-tuning baseline path artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted> and calls out the SQL Server rows this task must explain.
- artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>/benchmark-summary.md lines 82, 88, 97, 100, and 103 show SQL Server optimized-lane rows whose executionDetail begins with DVault SQL Server staged native bulk save path even when saveStrategyStatus=ProviderNeutralFallback, selectedStrategy=<none>, and fallbackCauses=SqlServerMinimumOperationThreshold or SqlServerMaximumSatelliteOperationThreshold.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted> and <redacted> hard-code the SQL Server gates at 50 minimum total operations and 500 maximum satellite operations and bind them to the public fallback causes SqlServerMinimumOperationThreshold and SqlServerMaximumSatelliteOperationThreshold.
- benchmarks/DCoding.Data.DVault.Benchmarks/CustomerProfileBenchmarks.cs:483-528 computes executionDetail from diagnostics on the satellite-only DataVaultBulkSaveRequest after hub insertion and uses _scenario.TotalChangeCount as the satellite operation count.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs:541-572 and 602-613 prepend the planned sqlserver-optimized execution path before appending actual diagnostics, which explains the misleading wording called out by the ticket.
- docs/performance-profiles.md:251-259, src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs:69-76, and docs/architecture/dvault-v1-activity-tracing-contract.md:151-152 already document the same SQL Server 50/500 gate and preserve the existing public fallback vocabulary.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the contract says 1000-plus satellite operations, but the developer evidence comment is where the exact row IDs for the larger classes live today; implementation should keep the 1000x1, 100x10, 10000x1, 1000x10, and 10000x10 interpretations aligned in the before/after narrative.
- Non-blocking: the contract already warns that the scale benchmark measures the satellite bulk request after hub insertion; the before/after artifact write-up should keep that distinction explicit so readers do not infer gates from end-to-end row counts.

Risky assumptions
- Assuming the repo-root benchmark-summary.md/.json is the authoritative before baseline would be wrong; the root rollup still reflects skipped external providers, while the all-provider baseline lives under artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>.
- Assuming a faster SQL Server optimized-lane row proves provider-native execution would be wrong for the current 10x1 and 1000-plus rows, because the persisted executionDetail still shows ProviderNeutralFallback and selectedStrategy=<none>.
- Assuming SQL Server gate counts are based on total end-to-end hub plus satellite work would misread the benchmark; the analyzed batch is the satellite-only bulk request and the effective gate count is TotalChangeCount.

AC / test suggestions
- Keep one explicit acceptance/test check that a completed SQL Server optimized-lane row with saveStrategyStatus=ProviderNeutralFallback never persists execution detail that still claims selectedStrategy=SqlServerDataVaultSaveStrategy or DVault SQL Server staged native bulk save path as the executed outcome.
- In the before/after evidence summary, call out both 100-operation positive-control rows (customer-profile-scale-100x1 and customer-profile-scale-10x10) and at least one 1000-satellite decline row from each request-count shape (1 request and 10 requests).
- If the 500 ceiling remains, require the artifact guidance text to name SqlServerMaximumSatelliteOperationThreshold alongside selectedStrategy=<none> so the decline is explicit to consumers.

Implementation watchouts
- Start from the v0.32.0 all-provider baseline bundle recorded in ticket 06F9XD26D2MHVAKZ2GCZ67BEFC, not from the root benchmark-summary files.
- The current owner branch is metadata-only at this point; product, benchmark, test, and artifact work still needs to be produced by development.
- The misleading wording currently comes from planned executionPath text being composed before live diagnostics are appended, not from the gate evaluator or fallback enum naming.
- Any decision to relax the 500-satellite ceiling needs measured provider-native evidence, because the current 1000-plus SQL Server rows can look faster than fallback while still being fallback executions.

Non-blocking notes
- The live blocks relation from 06F9XD26D2MHVAKZ2GCZ67BEFC to this ticket remains on disk, but current ticket comments already treat it as historical and non-blocking because the prerequisite ticket is done on develop.
- No repo evidence suggests a scope split is needed; the current contract already keeps SQL Server gate tuning and misleading benchmark-detail wording in one bounded task.

Split recommendations
- No split recommended. Keep SQL Server threshold tuning and fallback-versus-executed benchmark wording together under ticket 06F9XD2M71D1XFT7FJX62KD8HM.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment