# Provider Optimization Evidence Matrix

Status: v1 evidence contract
Ticket: 06FBSC3N7ZFVQW3AV2JJ8T7Q7W

## Purpose

This document is the canonical lookup surface for DVault provider optimization evidence rows. Later tickets should cite these matrix rows by scenario, provider, baseline, and evidence posture instead of restating benchmark notes or mixing measured timing evidence with skipped, diagnostics-only, smoke-only, or storage-footprint evidence.

The matrix reuses the existing benchmark artifact contract vocabulary from [Performance Evidence And Benchmark Artifact Contract](performance-evidence-benchmark-artifact-contract.md). It does not add benchmark fields, change benchmark schemas, rerun benchmarks, add provider implementations, add a DB2 benchmark lane, or widen hash-key storage claims beyond the checked-in SQLite-local evidence bundle.

## Evidence Postures

| Posture | Meaning |
| --- | --- |
| `completed-timing` | A checked-in benchmark row completed and may support a timing claim only with its artifact triplet and run context. |
| `skipped-placeholder` | A checked-in optional-provider row is present with `executionStatus=skipped`, a skip reason, `iterations=0`, blank or null metrics, deterministic execution detail, and `persistedOutcome=not executed`. It preserves row identity and planned strategy facts but is not timing evidence. |
| `diagnostics-only` | Repository diagnostics, capability profiles, and provider registration prove a bounded strategy candidate or fallback condition, but no benchmark timing row is claimed. |
| `smoke-only` | Opt-in live or smoke coverage proves representative execution behavior when configured, but it is not a measured benchmark lane. |
| `storage-footprint` | SQLite-local hash-key storage sidecars record physical storage footprint facts and scoped benchmark rows for hash-key variants. They are not cross-provider timing claims. |

## Authoritative Sources

- Root quick baseline: [benchmark-summary.md](../../benchmark-summary.md), [benchmark-summary.csv](../../benchmark-summary.csv), and [benchmark-summary.json](../../benchmark-summary.json).
- Benchmark artifact rules: [Performance Evidence And Benchmark Artifact Contract](performance-evidence-benchmark-artifact-contract.md).
- Save boundary and provider save posture: [DVault V1 Explicit Save Service](../architecture/dvault-v1-explicit-save-service.md).
- Read boundary and provider read posture: [DVault V1 PIT And Bridge Boundary](../architecture/dvault-v1-pit-bridge-boundary.md).
- Adopter performance guidance: [Performance Profiles](../performance-profiles.md).
- DB2 release posture: [DVault v0.34.0 Release Notes](../releases/v0.34.0.md).
- DB2 opt-in smoke posture: [Db2DataVaultSmokeTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs).
- Hash-key storage contract: [Hash Key Storage Profile Contract](hash-key-storage-profile-contract.md).
- SQLite hash-key storage evidence: [hash-key-footprint.md](../../hash-key-footprint.md) and [06F9GF66B10J4K7RBDTJ9NQRQC hash-key storage matrix bundle](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/).
- Row verifier coverage: [BenchmarkScenarioExecutionTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs).
- Closed fallback vocabularies: [DataVaultSaveStrategyFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultSaveStrategyFallbackCauseKind.cs), [DataVaultReadStrategyFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs), and [DataVaultChunkedSaveStateFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultChunkedSaveStateFallbackCauseKind.cs).

## Global Claim Rules

- Cite matrix rows with scenario, provider, baseline, and posture. Do not cite `skipped-placeholder`, `diagnostics-only`, `smoke-only`, or `storage-footprint` rows as measured provider performance.
- Keep timing claims attached to the artifact triplet, run context, provider filter, load-timestamp storage, iteration count, warmup count, hardware, runtime, dataset size, request shape, provider configuration, and skip/failure rows.
- SQLite is the only repository-proven optimized latest-satellite provider path in the current baseline.
- PostgreSQL, SQL Server, MySQL, Oracle, and DB2 are diagnostics-gated PIT/bridge read-strategy candidates. Their non-SQLite latest-satellite requests remain provider-neutral unless a later ticket adds new benchmark-backed strategy evidence.
- DB2 evidence is limited to diagnostics-gated clean-context save behavior, diagnostics-gated PIT/bridge read behavior, and opt-in live smoke evidence until a DB2 benchmark lane is added. DB2 latest-satellite optimization and DB2 live-schema reading remain unsupported in the current baseline.
- Binary-vs-hex storage comparisons are SQLite-local to the checked-in hash-key storage bundle unless a future provider-specific bundle is added.

## Save Matrix

| Scenario | Provider | Baseline | Strategy family | Posture | Canonical row source | Claim boundary |
| --- | --- | --- | --- | --- | --- | --- |
| `customer-profile-history`, `customer-profile-bulk-insert-only`, `customer-profile-bulk-history`, `order-product-fulfillment-history` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral explicit save through `AddDVault()`; no provider-specific strategy selected. |
| `customer-profile-history`, `customer-profile-bulk-insert-only`, `customer-profile-bulk-history`, `order-product-fulfillment-history` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized save path selected `SqliteDataVaultSaveStrategy`. |
| `customer-profile-streaming-save` | SQLite local temporary files | `dvault-adddvault-fallback/materialized-explicit-bulk` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Materialized `DataVaultBulkSaveRequest` over 60 ordered explicit requests. |
| `customer-profile-streaming-save` | SQLite local temporary files | `dvault-adddvault-fallback/chunked-save-bounded-10` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | `DataVaultChunkedSaveRequest` with 6 chunks of 10 and retained-state telemetry. |
| `customer-profile-streaming-save` | SQLite local temporary files | `dvault-adddvault-fallback/async-source-bounded-10` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | `IAsyncEnumerable<DataVaultSaveChunk>` source with 6 yielded chunks of 10. |
| `customer-profile-streaming-save` | SQLite local temporary files | `dvault-adddvault-fallback/chunked-save-bounded-5` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | `DataVaultChunkedSaveRequest` with 12 chunks of 5; cite chunk overhead and retained-state facts with telemetry context. |
| `provider-native-bulk-ingestion` | PostgreSQL external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `skipped-placeholder` | Root benchmark triplet | Optional provider-neutral comparison row; skipped when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset. |
| `provider-native-bulk-ingestion` | PostgreSQL external provider | `dvault-adddvaultpostgres-direct-or-unnest` | `postgres-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned PostgreSQL retained direct or UNNEST save path below the 60-operation staged boundary. |
| `provider-native-bulk-ingestion` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned PostgreSQL staged bulk save path using `COPY` at 60-plus operations. |
| `provider-native-bulk-ingestion` | SQL Server external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `skipped-placeholder` | Root benchmark triplet | Optional provider-neutral comparison row; skipped when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset. |
| `provider-native-bulk-ingestion` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned SQL Server native bulk path using `SqlBulkCopy`; native gate is 50-plus operations and at most 500 satellite operations. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `skipped-placeholder` | Root benchmark triplet | Optional provider-neutral comparison row; skipped when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvaultmysql-multi-row` | `mysql-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned retained MySQL multi-row path below the staged boundary. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned MySQL staged bulk path using temporary staging tables at 60-plus operations. |
| `provider-native-bulk-ingestion` | Oracle external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `skipped-placeholder` | Root benchmark triplet | Optional provider-neutral comparison row; skipped when `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset. |
| `provider-native-bulk-ingestion` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned Oracle direct optimized batching; staged Oracle bulk remains `not-selected-no-measured-win`. |
| DB2 clean-context save | DB2 external provider | `AddDVaultDb2()` / `Db2DataVaultSaveStrategy` | `db2-optimized-dvault` | `diagnostics-only` and `smoke-only` | v0.34.0 release notes and DB2 smoke evidence | DB2 supports optimized clean-context hub, link, and ordinary satellite saves through provider diagnostics and opt-in smoke coverage. No DB2 benchmark lane, timing row, staged bulk lane, provider-native chunk execution, or live-schema reader is claimed. |

The root benchmark triplet keeps optional PostgreSQL, SQL Server, MySQL, and Oracle rows visible as skipped placeholders. Completed external-provider timing claims, where needed, must cite the provider-threshold bundles linked from [Performance Profiles](../performance-profiles.md), not these skipped root rows alone.

## Read Matrix

| Scenario | Provider | Baseline | Strategy family | Posture | Canonical row source | Claim boundary |
| --- | --- | --- | --- | --- | --- | --- |
| `latest-satellite-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral latest read over seeded profile states. |
| `latest-satellite-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized latest-satellite read selected `SqliteDataVaultReadStrategy`. This is the only repository-proven optimized latest-satellite provider path. |
| `pit-as-of-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral PIT as-of read over explicitly maintained PIT rows. |
| `pit-as-of-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized PIT read selected `SqliteDataVaultReadStrategy`. |
| `bridge-traversal-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral bridge traversal over explicitly maintained bridge rows. |
| `bridge-traversal-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized bridge read selected `SqliteDataVaultReadStrategy`. |
| `latest-satellite-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records `providerSpecificReadStrategy=not registered for latest satellite reads`; no PostgreSQL latest-satellite optimization claim. |
| `pit-as-of-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `PostgresDataVaultReadStrategy` for diagnostics-gated PIT reads. |
| `bridge-traversal-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `PostgresDataVaultReadStrategy` for diagnostics-gated bridge reads. |
| `latest-satellite-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records `providerSpecificReadStrategy=not registered for latest satellite reads`; no SQL Server latest-satellite optimization claim. |
| `pit-as-of-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `SqlServerDataVaultReadStrategy` for diagnostics-gated PIT reads. |
| `bridge-traversal-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `SqlServerDataVaultReadStrategy` for diagnostics-gated bridge reads. |
| `latest-satellite-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records `providerSpecificReadStrategy=not registered for latest satellite reads`; no MySQL latest-satellite optimization claim. |
| `pit-as-of-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `MySqlDataVaultReadStrategy` for diagnostics-gated PIT reads. |
| `bridge-traversal-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `MySqlDataVaultReadStrategy` for diagnostics-gated bridge reads. |
| `latest-satellite-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records `providerSpecificReadStrategy=not registered for latest satellite reads`; no Oracle latest-satellite optimization claim. |
| `pit-as-of-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `OracleDataVaultReadStrategy` for diagnostics-gated PIT reads. |
| `bridge-traversal-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `OracleDataVaultReadStrategy` for diagnostics-gated bridge reads. |
| Latest-satellite read | DB2 external provider | `AddDVaultDb2()` provider-neutral latest read | `provider-neutral-dvault-fallback` | `diagnostics-only` and `smoke-only` | v0.34.0 release notes | DB2 latest/as-of satellite reads use the provider-neutral latest pipeline; no provider-native latest-satellite read strategy is registered. |
| PIT/bridge read | DB2 external provider | `AddDVaultDb2()` / `Db2DataVaultReadStrategy` | `db2-optimized-dvault` | `diagnostics-only` and `smoke-only` | v0.34.0 release notes and DB2 smoke evidence | DB2 registers diagnostics-gated PIT/bridge read dispatch and opt-in representative smoke coverage. No DB2 benchmark lane or timing row exists. |

## Hash-Key Storage Matrix

| Variant | Provider | Algorithm | Storage profile | Posture | Canonical source | Claim boundary |
| --- | --- | --- | --- | --- | --- | --- |
| `sha256-v1-hex` | SQLite local temporary files | `sha256-v1` | `HexString` | `storage-footprint` | Hash-key storage bundle and root footprint summary | Default 32-byte digest stored as 64 lowercase hex characters in SQLite `TEXT`. |
| `sha256-v1-binary` | SQLite local temporary files | `sha256-v1` | `Binary` | `storage-footprint` | Hash-key storage bundle and root footprint summary | Explicit opt-in physical binary storage for a 32-byte digest in SQLite `BLOB`; public boundaries still use lowercase hex strings. |
| `sha256-128-v1-hex` | SQLite local temporary files | `sha256-128-v1` | `HexString` | `storage-footprint` | Hash-key storage bundle and root footprint summary | 16-byte digest stored as 32 lowercase hex characters in SQLite `TEXT`. |
| `sha256-128-v1-binary` | SQLite local temporary files | `sha256-128-v1` | `Binary` | `storage-footprint` | Hash-key storage bundle and root footprint summary | Explicit opt-in physical binary storage for a 16-byte digest in SQLite `BLOB`; public boundaries still use lowercase hex strings. |

The authoritative footprint sidecars are [hash-key-footprint.md](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.md), [hash-key-footprint.csv](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.csv), and [hash-key-footprint.json](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.json). The storage-profile contract keeps logical hash-key values as canonical lowercase hexadecimal strings at request, save, read, diagnostics, and support-bundle boundaries.

## Stop And Fallback Conditions

Save claims must stop or fall back when any required benchmark row is skipped, a required provider connection string is missing, the provider package is not registered, diagnostics do not select the expected strategy, or any of these bounded fallback causes apply:

- `ProviderNameMismatch`
- `UnknownOrUnregisteredProviderName`
- `NoProviderSpecificStrategyRegistered`
- `DirtyDbContext`
- `MultiActiveSatelliteOperations`
- `SqlServerMinimumOperationThreshold`
- `SqlServerMaximumSatelliteOperationThreshold`
- `MySqlMinimumOperationThreshold`
- `OracleMinimumOperationThreshold`
- `StrategyDeclined`
- `OracleMaximumSatelliteOperationThreshold`
- `StagedProviderBulkDirtyDbContext`
- `StagedProviderBulkUnsupportedShape`
- `StagedProviderBulkTransactionParticipationUnsupported`
- `StagedProviderBulkCleanupFailed`
- `StagedProviderBulkProviderLimitation`
- `MySqlTinySatelliteHistoryProviderNeutralFallback`

Chunked-save claims must also stop or fall back on `RetainedSatelliteSeriesLimitReached` when retained satellite continuity state exceeds the bounded in-memory limit.

Read claims must stop or fall back when PIT or bridge rows are not explicitly maintained before read traffic, required read-shape evidence is missing, diagnostics do not select the expected strategy, non-SQLite latest-satellite optimization is requested, or any of these bounded fallback causes apply:

- `ProviderNameMismatch`
- `UnknownOrUnregisteredProviderName`
- `NoProviderSpecificStrategyRegistered`
- `UnsupportedSatelliteParent`
- `MultiActiveSatelliteUnsupported`
- `StrategyDeclined`
- `UnsupportedPitShape`
- `UnsupportedBridgeShape`
- `IncompleteReadShapeEvidence`
- `StaleReadModelMaintenance`

Provider-specific threshold facts remain part of the stop conditions: PostgreSQL staged COPY starts at 60-plus operations, SQL Server native bulk starts at 50-plus operations and accepts at most 500 satellite operations, MySQL native and staged candidates apply only when their candidate gates are met, Oracle direct optimized batching starts at 50-plus operations and accepts at most 10000 satellite operations, and DB2 has no staged bulk, provider-native chunk execution, or benchmark lane in the current baseline.

## Citation Examples

Use this form in follow-up tickets:

```text
matrix row: scenario=provider-native-bulk-ingestion; provider=PostgreSQL external provider; baseline=dvault-adddvaultpostgres-optimized; posture=skipped-placeholder
claim: row identity and planned staged COPY boundary only; not measured timing evidence from the root baseline
```

```text
matrix row: scenario=latest-satellite-read; provider=SQLite local temporary files; baseline=dvault-adddvaultsqlite-optimized; posture=completed-timing
claim: measured SQLite latest-satellite read evidence, valid only with the root benchmark triplet and run context
```

```text
matrix row: provider=DB2 external provider; baseline=AddDVaultDb2()/Db2DataVaultReadStrategy; posture=diagnostics-only and smoke-only
claim: diagnostics-gated PIT/bridge support and opt-in smoke behavior only; no DB2 timing evidence
```
