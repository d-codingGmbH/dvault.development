[gicket-bot] PO-critic review contract

Summary
- Ticket 06F2PGNZBRNCQ1SV2KKP6F3BA8 is specific enough for developer handoff: the delivery contract is concrete, `## Open Questions` is `none`, related dependency state is coherent, and local repository evidence matches the stated benchmark-validity gap.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGNZBRNCQ1SV2KKP6F3BA8/description.md` contains `## Open Questions` -> `- none`, plus six acceptance-criteria bullets and four definition-of-done bullets.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs` lines 241-276 still create optimized optional-provider rows for `CustomerProfileDataVaultBenchmark`, `CustomerProfileBulkDataVaultBenchmark`, `OrderProductDataVaultBenchmark`, and non-SQLite read rows (`LatestSatelliteReadBenchmark`, `PitAsOfReadBenchmark`, `BridgeTraversalReadBenchmark`) for every optional provider.
- An `rg` search for `Analyze(`, `SelectedStrategyName`, `ProviderStrategySelected`, `FallbackCauses`, `IDataVaultDiagnosticsService`, and `diagnostics` under `benchmarks/DCoding.Data.DVault.Benchmarks` returned no matches, which supports the contract claim that benchmark code does not currently prove selected provider strategy.
- `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs` lines 21-50 and 53-116 build a bounded external-provider batch with `PairCount = 20`, call `diagnostics.Analyze(...)`, and assert `AssertProviderStrategySelected(...)`, giving the repo a direct strategy-proof baseline to mirror.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` lines 81-98 and <redacted> define the live gates referenced by the ticket: dirty `DbContext` and multi-active satellites decline optimization; SQL Server requires at least 50 total operations and at most 500 satellite operations; MySQL and Oracle require at least 50 total operations.
- `benchmarks/DCoding.Data.DVault.Benchmarks/CustomerProfileBenchmarks.cs` lines 639-647 define `ChangeHeavy` as `100 customers, 10 profile states each`, matching the ticket's SQL Server gate-risk warning.
- `src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs` plus an `rg` search for `IDataVaultProviderReadStrategy` implementations under `src/` found only SQLite provider read-strategy implementation/registration, matching the contract's non-SQLite read caveat.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` lines 58-64 and `artifacts/benchmarks/baseline-2026-05-08-scale-5/benchmark-summary.json` lines 2-39 show the artifact contract already carries `benchmark-summary.md`, `.csv`, `.json`, execution status, skip reason, provider discovery state, and machine/runtime context.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example of a below-threshold external-provider row that must be labeled fallback or skipped instead of native-optimized would make later review faster, but the current contract is still sufficient.
- An explicit example of how SQL Server `ChangeHeavy` should appear in artifacts when it exceeds the 500-satellite gate would reduce ambiguity, but it is not required to start development.
- An explicit example of non-SQLite read rows being present in artifacts while excluded from provider-native bulk claims would help documentation consistency, but the current scope language already points developers in that direction.

Risky assumptions
- Assuming the stale clarification about 'only the bot claim and lease comments' is harmless drift from later automation; the current comment set contains additional bot handoff/run-report comments but no human unresolved review feedback.
- Assuming developers will reuse the existing provider-eligible assertion shape from `ExternalProviderBulkSaveAssertions.cs` or an equivalent gate-proven batch rather than inventing a benchmark-only shape.

AC / test suggestions
- Keep one acceptance/test expectation that every provider-specific optimized benchmark row has observable strategy-selection proof, not just an optimized label.
- Keep one acceptance/test expectation that rows outside provider gates are explicitly marked fallback or skipped across `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`.
- Keep one acceptance/test expectation that artifact regression checks verify `executionStatus`, `skipReason`, provider discovery metadata, and machine/runtime context together.

Implementation watchouts
- `BenchmarkRunner.cs` still emits optimized optional-provider read rows for PostgreSQL, SQL Server, MySQL, and Oracle; those rows must stay outside native bulk-write evidence unless labeling is explicit.
- The current `ChangeHeavy` dataset is 100 customers x 10 profile states, so it cannot serve as native SQL Server proof under the current >500 satellite-operation gate.
- The benchmark project currently shows no direct diagnostics/selected-strategy proof hooks, so developer work must preserve the contract requirement that optimized rows cannot silently measure provider-neutral fallback execution.

Non-blocking notes
- The persisted comment set under `.gicket/tickets/06F2PGNZBRNCQ1SV2KKP6F3BA8/comments/` now includes PO refinement, run-report, handoff, relation-follow-up, and po-critic claim bot comments; it is no longer limited to claim/lease entries.
- The contract note that branch head `66d72e4a9` contained only metadata is historically stale because the branch head is now `f53547c26`, but `git diff --name-only develop..ticket/06F2PGNZBRNCQ1SV2KKP6F3BA8-story-benchmark-fallback-and-native-bulk-ingesti` still shows metadata-only changes.
- No ticket-level PO refinement gap remains around status, labels, assignees, or open questions before developer handoff.

Split recommendations
- No split is needed for this handoff; the story remains bounded to write-path benchmark validity.
- If non-SQLite read benchmarking or broader publication scope is wanted later, track it in a fresh follow-on ticket instead of widening 06F2PGNZBRNCQ1SV2KKP6F3BA8.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment