# SQL Server Bulk Threshold Decision

Ticket: `06FE4QRC7D55RS8ZZ37ZAEJ98M`

## Decision

Keep the SQL Server provider-native save gate at:

- at least 100 total hub, link, and satellite operations;
- at least 900 total operations when the batch mixes hub or link operations with satellite work;
- no more than 500 satellite operations;
- a clean SQL Server `DbContext`;
- provider diagnostics selecting `SqlServerDataVaultSaveStrategy`.

When those requirements are not met, the save remains on the provider-neutral writer. Diagnostics must record the fallback with `selectedStrategy=<none>` and bounded fallback causes instead of claiming that the SQL Server native lane executed.

## Confirming Evidence

The current configured SQL Server mixed-batch probe is:

- [benchmark-summary.md](artifacts/benchmarks/current-sqlserver-mixed-303-probe/benchmark-summary.md)
- [benchmark-summary.csv](artifacts/benchmarks/current-sqlserver-mixed-303-probe/benchmark-summary.csv)
- [benchmark-summary.json](artifacts/benchmarks/current-sqlserver-mixed-303-probe/benchmark-summary.json)

That run used `Provider filter: sqlserver`, one measured iteration, zero warmup iterations, `ProviderDefault` load-timestamp storage, `sha256-v1-hex` hash keys, and .NET `10.0.9`.

The `provider-native-bulk-ingestion` SQL Server optimized row completed with:

- `baseline=dvault-adddvaultsqlserver-optimized`;
- `selectedStrategy=SqlServerDataVaultSaveStrategy`;
- `transfer=SqlBulkCopy`;
- `nativeBulkBoundary=100-plus-operations`;
- `mixedBatchBoundary=900-plus-operations`;
- `cleanupBoundary=temporary-staging-table`;
- `requestCount=5`;
- `hubOperations=600`;
- `linkOperations=300`;
- `satelliteOperations=3`;
- `fallbackCauses=none`;
- mean `106.978` ms for the observed run context.

The same triplet keeps the SQL Server provider-neutral comparator row explicit:

- `baseline=dvault-adddvault-fallback`;
- `executionPath=DVault provider-neutral fallback path`;
- `selectedStrategy=<none>`;
- `fallbackCauses=NoProviderSpecificStrategyRegistered`;
- mean `398.097` ms for the observed run context.

The earlier ticket-specific configured SQL Server benchmark triplet is retained as historical context:

- [benchmark-summary.md](artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-20260620/benchmark-summary.md)
- [benchmark-summary.csv](artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-20260620/benchmark-summary.csv)
- [benchmark-summary.json](artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-20260620/benchmark-summary.json)

## Historical Threshold Bundle

The historical threshold bundle remains useful for fallback vocabulary and the 500-satellite ceiling:

- [SQL Server Save Threshold Diagnostics](artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md)

That bundle records completed scale rows below, inside, and above the SQL Server native-bulk gate. The current decision supersedes its older 50-operation minimum with the measured 100-operation baseline and the 900-operation mixed-batch boundary, while retaining its 500 maximum-satellite-operation decision and fallback-row wording.

## Artifact Lane Boundary

This decision does not create a deployable SQL artifact lane. The `dvault.sql-artifact.v1` contract remains the review-only dry-run manifest schema documented in release and performance guidance. The current SQL Server artifact story is bounded to:

- SQL Server only;
- `provider-native-bulk-ingestion`;
- `SqlBulkCopy` transfer;
- temporary staging-table cleanup;
- manifest-only review output;
- no sidecar SQL payload;
- no runtime dispatch.

## Latest-Satellite Boundary

The 2026-06-20 benchmark triplet also contains SQL Server read rows. This threshold decision does not use that incidental row set to close or promote `P0.02` latest-satellite timing. SQL Server latest-satellite promotion remains a separate provider-read evidence decision so this save-threshold ticket does not widen read-optimization claims.
