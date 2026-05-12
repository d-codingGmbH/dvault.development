[gicket-bot] PO refinement contract

Summary
- Expanded the ticket contract so this ticket explicitly owns the missing core provider read-strategy hook, dispatcher, diagnostics, and SQLite latest/as-of satellite strategy instead of depending on completed ticket 06F0MEJ7NANHCP64VR1SH3S3G8.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Reconciled by expanding this ticket to own the core read-strategy hook implementation and the SQLite strategy. The related done ticket remains historical closure context and is no longer a runtime dependency for this ticket's dev handoff.
- critic-item-2: `answered` - The direct source-backed contract is now: add core IDataVaultProviderReadStrategy and DataVaultProviderReadStrategyContext in src/DCoding.Data.DVault; dispatch from DefaultDataVaultReadService.ReadLatestSatelliteRowsAsync and its internal IDataVaultSatelliteProjectionReadService path; add read-strategy diagnostics mirroring DataVaultSaveStrategyDiagnostics with request-bound status, candidate, selected strategy, priority, and fallback causes; register SqliteDataVaultReadStrategy from AddDVaultSqlite using TryAddEnumerable alongside the existing save strategy registration.
- critic-item-3: `answered` - This ticket is kept out of dev handoff as a hook-dependent SQLite-only optimization and is instead refined as a combined core hook plus SQLite strategy ticket. PO-critic can review the expanded contract because open questions are empty and the missing hook is now in scope.
- critic-item-4: `answered` - The blocking finding is resolved by acknowledging that the current source has no read-strategy hook and making creation of the hook, dispatcher, diagnostics surface, fallback behavior, and AddDVaultSqlite registration part of this ticket's Scope In, Acceptance Criteria, and Definition of Done.
- critic-item-5: `answered` - The stale implementation note that told dev to align to absent completed-hook symbols is superseded. The new contract says this ticket owns the hook design following the existing save-strategy dispatch pattern, so SQLite optimization is no longer an implicit out-of-scope public API change.

Clarifications
- The v1 deliverable is one combined slice: core provider read-strategy hook plus SQLite optimized latest/as-of satellite reads for the bounded benchmarked shape.
- The completed related hook ticket is treated as historical context only; this ticket replaces its missing runtime obligation for the purposes of this implementation.
- The optimized v1 SQLite shape is ordinary latest/as-of DataVaultLatestSatelliteReadRequest satellite reads by parent hash key for the benchmarked non-multi-active hub-parent satellite shape. Unsupported read families, providers, metadata shapes, and timestamp modes must decline to fallback.
- The existing provider-neutral read behavior and public read-service entry point remain the compatibility baseline.

Scope In
- Add the core provider read-strategy hook in src/DCoding.Data.DVault, with expected public contract names IDataVaultProviderReadStrategy and DataVaultProviderReadStrategyContext unless implementation discovers an unavoidable naming conflict.
- Update DefaultDataVaultReadService so ReadLatestSatelliteRowsAsync dispatches registered read strategies by descending Priority and dependency-injection registration order before falling back to DataVaultSatelliteReadPipeline.ReadLatestReadRecordsAsync.
- Ensure the internal typed projection path used by IDataVaultSatelliteProjectionReadService does not bypass the selected strategy for supported SQLite latest/as-of reads and preserves existing typed projection behavior.
- Add request-bound read-strategy diagnostics mirroring the existing save-strategy diagnostics shape: not evaluated, provider strategy selected, provider-neutral fallback, candidates, selected strategy name/priority, and material fallback causes.
- Implement SqliteDataVaultReadStrategy in the SQLite provider package for supported latest/as-of satellite reads, using SQLite SQL to push parent-key filtering and latest/as-of row selection into the database.
- Register SqliteDataVaultReadStrategy through AddDVaultSqlite using the established TryAddEnumerable provider-registration pattern; AddDVault remains provider-neutral fallback only.
- Add focused tests for core strategy selection, fallback when no strategy is registered, decline-to-fallback behavior, SQLite latest correctness, SQLite as-of correctness, diagnostics, provider registration, and typed projection parity.
- Capture before/after benchmark evidence for the existing latest satellite read benchmark with command, provider filter, iterations, warmup, timestamp storage, run context, and measured rows.

Scope Out
- Reopening or editing related ticket 06F0MEJ7NANHCP64VR1SH3S3G8 as part of this ticket.
- Optimizing PIT reads, bridge traversal reads, registry request resolution beyond existing latest-satellite behavior, or every satellite/read shape.
- Optimizing PostgreSQL, SQL Server, MySQL, or Oracle read paths.
- Changing save strategy behavior, schema generation semantics, write benchmark contracts, or caller-facing provider selection.
- Provisioning external databases, secrets, containers, or persistent benchmark infrastructure.

Open questions
- none

Follow-up questions
- After this first combined hook and SQLite optimization lands, use the benchmark matrix to choose the next provider/read shape, if any.
- Decide later whether SQLite driving-key satellites, PIT reads, or bridge reads deserve separate provider-specific optimization tickets.
- Decide later whether provider read-strategy hooks should be promoted in external provider-author documentation beyond the API snapshot.
- Decide later whether benchmark artifacts should be archived in release notes or CI artifacts for trend comparison.

Risks
- The ticket is larger than the previous SQLite-only optimization because it now includes an additive core hook and diagnostics surface.
- Benchmark timings are machine-specific, so evidence must keep run context attached to the result.
- SQLite timestamp storage and duplicate timestamp edge cases can produce subtle parity issues if SQL ordering differs from fallback behavior.
- Adding public hook and diagnostics types requires public API snapshot updates and careful compatibility review.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 10
- definition-of-done items: 7
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment