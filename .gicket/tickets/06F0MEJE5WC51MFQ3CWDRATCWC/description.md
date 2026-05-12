<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Expanded the ticket contract so this ticket explicitly owns the missing core provider read-strategy hook, dispatcher, diagnostics, and SQLite latest/as-of satellite strategy instead of depending on completed ticket 06F0MEJ7NANHCP64VR1SH3S3G8.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 deliverable is one combined slice: core provider read-strategy hook plus SQLite optimized latest/as-of satellite reads for the bounded benchmarked shape.
- The completed related hook ticket is treated as historical context only; this ticket replaces its missing runtime obligation for the purposes of this implementation.
- The optimized v1 SQLite shape is ordinary latest/as-of DataVaultLatestSatelliteReadRequest satellite reads by parent hash key for the benchmarked non-multi-active hub-parent satellite shape. Unsupported read families, providers, metadata shapes, and timestamp modes must decline to fallback.
- The existing provider-neutral read behavior and public read-service entry point remain the compatibility baseline.

### Scope In
- Add the core provider read-strategy hook in src/DCoding.Data.DVault, with expected public contract names IDataVaultProviderReadStrategy and DataVaultProviderReadStrategyContext unless implementation discovers an unavoidable naming conflict.
- Update DefaultDataVaultReadService so ReadLatestSatelliteRowsAsync dispatches registered read strategies by descending Priority and dependency-injection registration order before falling back to DataVaultSatelliteReadPipeline.ReadLatestReadRecordsAsync.
- Ensure the internal typed projection path used by IDataVaultSatelliteProjectionReadService does not bypass the selected strategy for supported SQLite latest/as-of reads and preserves existing typed projection behavior.
- Add request-bound read-strategy diagnostics mirroring the existing save-strategy diagnostics shape: not evaluated, provider strategy selected, provider-neutral fallback, candidates, selected strategy name/priority, and material fallback causes.
- Implement SqliteDataVaultReadStrategy in the SQLite provider package for supported latest/as-of satellite reads, using SQLite SQL to push parent-key filtering and latest/as-of row selection into the database.
- Register SqliteDataVaultReadStrategy through AddDVaultSqlite using the established TryAddEnumerable provider-registration pattern; AddDVault remains provider-neutral fallback only.
- Add focused tests for core strategy selection, fallback when no strategy is registered, decline-to-fallback behavior, SQLite latest correctness, SQLite as-of correctness, diagnostics, provider registration, and typed projection parity.
- Capture before/after benchmark evidence for the existing latest satellite read benchmark with command, provider filter, iterations, warmup, timestamp storage, run context, and measured rows.

### Scope Out
- Reopening or editing related ticket 06F0MEJ7NANHCP64VR1SH3S3G8 as part of this ticket.
- Optimizing PIT reads, bridge traversal reads, registry request resolution beyond existing latest-satellite behavior, or every satellite/read shape.
- Optimizing PostgreSQL, SQL Server, MySQL, or Oracle read paths.
- Changing save strategy behavior, schema generation semantics, write benchmark contracts, or caller-facing provider selection.
- Provisioning external databases, secrets, containers, or persistent benchmark infrastructure.

## Acceptance Criteria
- Core exposes a provider read-strategy hook with IDataVaultProviderReadStrategy and DataVaultProviderReadStrategyContext, following the existing save-strategy priority and registration-order dispatch semantics.
- DefaultDataVaultReadService.ReadLatestSatelliteRowsAsync selects a compatible registered provider read strategy for supported requests and otherwise preserves the DataVaultSatelliteReadPipeline fallback behavior.
- The typed latest satellite projection path uses the same selection/fallback decision for supported optimized reads and returns projections matching fallback semantics, including nullable payload handling and load timestamp conversion.
- Read-strategy diagnostics are available for DataVaultLatestSatelliteReadRequest analysis and report provider selected, provider-neutral fallback, candidates, selected strategy name/priority, and fallback causes without changing existing save-strategy diagnostics semantics.
- AddDVaultSqlite registers SqliteDataVaultReadStrategy; AddDVault alone does not register a provider read strategy and continues through provider-neutral fallback.
- For the supported SQLite latest/as-of satellite shape, optimized output matches fallback output for row count, metadata/table names, parent hash keys, hash diff, load timestamp, record source, payload values, and deterministic ordering.
- Unsupported shapes and providers decline cleanly and continue through the fallback path with existing behavior.
- Benchmark evidence compares the pre-optimization fallback baseline and optimized SQLite latest-satellite read on the same machine/options, and the optimized row shows a measured mean-time improvement for the selected shape.
- Existing write-path tests and benchmark smoke coverage do not show write behavior regressions.
- No public API compatibility break is introduced beyond additive hook and diagnostics surface required by this ticket.

## Definition of Done
- Core read-strategy contracts, dispatcher integration, fallback behavior, and read diagnostics are implemented and covered by unit or integration tests.
- SQLite read strategy implementation and AddDVaultSqlite registration are complete in the established provider package layout.
- Automated tests cover optimized selection, no-strategy fallback, decline-to-fallback behavior, latest read correctness, as-of read correctness, typed projection parity, diagnostics status/candidates/fallback causes, and provider registration.
- Public API approval snapshots are updated if the hook or diagnostics surface is public.
- Before/after benchmark artifacts or ticket comments include command line, provider filter, iterations/warmup, load timestamp storage, run context, and measured rows used for the optimization choice.
- dotnet build DVault.slnx and dotnet test DVault.slnx pass in the expected local configuration.
- A SQLite benchmark smoke run using the existing benchmark host completes and includes the optimized latest-satellite read row.

## Implementation Notes
- Use the existing save-strategy dispatch pattern as the source-backed architecture template: core owns contracts, selection, diagnostics, and fallback; provider packages own provider SQL and AddDVaultSqlite registration.
- Current source evidence: DefaultDataVaultReadService calls DataVaultSatelliteReadPipeline directly, so this ticket must insert the dispatcher there rather than adding provider checks to caller code.
- Current source evidence: DataVaultSatelliteReadPipeline loads rows for requested parent hash keys and picks latest rows in memory; the SQLite path should reduce that over-read for the benchmarked 100 latest rows from 1000 seeded profile states.
- Use existing naming policy and DataVaultLoadTimestampValueConverter behavior so table/column names and timestamp storage modes stay consistent with fallback.
- Keep SQL parameterized and batched consistently with existing SQLite provider code; do not concatenate parent hash key values into SQL.
- The SQLite strategy should decline unsupported shapes rather than partially reimplementing the provider-neutral pipeline.
- Diagnostics should reuse the vocabulary style of DataVaultSaveStrategyDiagnostics and should not remove or reinterpret existing save diagnostics fields.

## Open Questions
- none

## Follow-Up Questions
- After this first combined hook and SQLite optimization lands, use the benchmark matrix to choose the next provider/read shape, if any.
- Decide later whether SQLite driving-key satellites, PIT reads, or bridge reads deserve separate provider-specific optimization tickets.
- Decide later whether provider read-strategy hooks should be promoted in external provider-author documentation beyond the API snapshot.
- Decide later whether benchmark artifacts should be archived in release notes or CI artifacts for trend comparison.

## Risks
- The ticket is larger than the previous SQLite-only optimization because it now includes an additive core hook and diagnostics surface.
- Benchmark timings are machine-specific, so evidence must keep run context attached to the result.
- SQLite timestamp storage and duplicate timestamp edge cases can produce subtle parity issues if SQL ordering differs from fallback behavior.
- Adding public hook and diagnostics types requires public API snapshot updates and careful compatibility review.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement the first provider-specific read optimization selected from measured benchmark evidence.

## Scope In

- Choose the provider/read shape with the largest measured improvement potential.
- Implement optimized SQL/query path through the read strategy hook.
- Add correctness tests and before/after benchmark evidence.

## Scope Out

- Optimizing every provider.
- Changing write strategy behavior.

## Acceptance Criteria

- Optimization choice is justified by benchmark data in comments or docs.
- Fallback remains correct for unsupported shapes.
- The implementation does not regress write benchmarks or public API compatibility.