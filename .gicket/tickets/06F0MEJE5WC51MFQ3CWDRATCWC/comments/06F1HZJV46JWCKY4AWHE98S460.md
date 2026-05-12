[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the latest persisted contract resolves the prior hook-dependency blocker by making this ticket own the core read-strategy hook, diagnostics, SQLite strategy, tests, and benchmark proof, with Open Questions set to none and direct source evidence for the existing baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/description.md:7-15 records PO handoff ready_for_po_critic and clarifies this ticket owns the combined core read-strategy hook plus SQLite latest/as-of read slice.
- .gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/description.md:17-25 scopes in IDataVaultProviderReadStrategy/DataVaultProviderReadStrategyContext, DefaultDataVaultReadService dispatch, typed projection parity, diagnostics, SqliteDataVaultReadStrategy, registration, tests, and benchmark evidence.
- .gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/description.md:34-53 contains concrete AC/DoD for selection/fallback, typed projection parity, diagnostics, AddDVaultSqlite registration, correctness, benchmark proof, API snapshots, build/test, and smoke benchmark.
- .gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/description.md:64-65 records ## Open Questions as '- none'.
- .gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1HY0D6WTQZSMZD7QB9R2PXW.md:10-15 explicitly answers the prior PO-critic blockers by moving the missing hook, dispatcher, diagnostics, fallback, and SQLite registration into this ticket scope.
- Prior critic evidence at .gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1HWSJWJMHZ2VNDXTRWBX98M.md:17-25 shows the earlier block was dependency on an absent completed hook; the current contract now resolves that specific issue.
- git rev-parse HEAD returned 45327596cafbad9ea5123099ecdbc9088d257f84, matching the scratch-source-ref, and git status --short --branch reported only the tracked ticket branch line.
- src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:5-17 currently calls DataVaultSatelliteReadPipeline.ReadLatestReadRecordsAsync directly, and :32-43 sends the internal typed projection path directly to DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync.
- src/DCoding.Data.DVault/DataVaultSatelliteReadPipeline.cs:60-85 loads rows for parent hash-key batches, filters as-of in memory, selects latest rows in memory, and orders by parent hash key/driving-key signature.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-30 currently registers AddDVault plus SQLite provider behavior/save strategy via TryAddEnumerable, giving the cited registration pattern for the new read strategy.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:5-15 and src/DCoding.Data.DVault/DataVaultSaveService.cs:834-876 provide direct source evidence for the existing save-strategy priority/registration-order dispatch template.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:34-49, :216-222, and :630-714 provide direct source evidence for the existing save-strategy diagnostics vocabulary and selected/fallback/candidate shape the ticket asks read diagnostics to mirror.
- rg for SqliteDataVaultReadStrategy, IDataVaultProviderReadStrategy, DataVaultProviderReadStrategyContext, and read-strategy diagnostics under src/tests/benchmarks/docs returned no matches, so there is no local naming conflict and no hidden implemented hook to depend on.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md:18 and :64-74 document current read benchmarks as provider-neutral baselines with latest satellite read rows measured through ReadLatestSatelliteRowsAsync; ReadModelBenchmarks.cs:64-81 measures 100 latest profile satellite rows from 1000 seeded profile states.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking gap. During implementation, test duplicate load-timestamp ordering explicitly because the contract calls out timestamp ordering risk but does not prescribe a tie-break beyond fallback parity.
- No blocking gap. Include empty and duplicate parent-hash-key request coverage if existing request tests do not already cover it.

Risky assumptions
- The ticket is intentionally larger than the original SQLite-only idea; the contract acknowledges this risk and scopes the combined core-plus-SQLite slice tightly enough for low-assurance handoff.

AC / test suggestions
- Keep the listed unit/integration tests focused on strategy ordering, DI registration order tie-breaks, no-strategy fallback, strategy decline fallback causes, latest/as-of parity, typed projection parity, diagnostics, and AddDVault/AddDVaultSqlite registration differences.
- Benchmark artifacts should preserve command line, provider filter, iteration/warmup counts, load timestamp storage, run context, and measured rows exactly as required by the contract.

Implementation watchouts
- Do not let the public read service and internal typed projection path make separate selection decisions for supported SQLite reads.
- Preserve provider-neutral fallback behavior for unsupported shapes/providers and for AddDVault-only registrations.
- Update public API approval snapshots for additive public hook and diagnostics types.
- Keep SQLite SQL parameterized and aligned with existing naming policy and DataVaultLoadTimestampValueConverter behavior.

Non-blocking notes
- The related benchmark predecessor 06F0MEJ0NE80R7CNS982S3PKVR is done, while downstream docs ticket 06F0MEJPGG7JBFEXD693BHY07W is blocked by this ticket rather than blocking it.
- No repository writes, ticket writes, builds, or tests were run in this read-only PO-critic review.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment