# Provider Optimization Evidence Matrix

Status: v1 evidence contract
Ticket: 06FBSC3N7ZFVQW3AV2JJ8T7Q7W

## Purpose

This document is the canonical lookup surface for DVault provider optimization evidence rows. Later tickets should cite these matrix rows by scenario, provider, baseline, and evidence posture instead of restating benchmark notes or mixing measured timing evidence with skipped, diagnostics-only, smoke-only, or storage-footprint evidence.

The matrix reuses the existing benchmark artifact contract vocabulary from [Performance Evidence And Benchmark Artifact Contract](performance-evidence-benchmark-artifact-contract.md). It does not add benchmark fields, change benchmark schemas, add provider implementations, or widen successful timing claims beyond completed rows in checked-in artifact bundles. The root quick benchmark triplet remains the SQLite-local and skipped optional-provider baseline. PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge completed timing is cited from the checked-in provider-configured v0.32.0 smoke-read bundle only; SQL Server provider-native bulk timing is additionally cited from the ticket-specific 2026-06-20 configured SQL Server bulk-threshold bundle; MySQL latest-satellite completed timing is cited from the checked-in ticket `06FE4QQ9VF7B74E60CXEHSS5XW` bundle only, and that bundle does not by itself claim a provider-neutral fallback improvement comparator. DB2 clean-context optimized save plus supported latest-satellite, PIT, and bridge completed timing is cited from the provider-configured DB2 hotspot evidence bundle for ticket `06FE4QR3DD7EFZ4F35SBTFGWSR`. Provider binary-vs-hex hash-key storage participation is cited from the checked-in ticket `06FE4R1N2ADN77NDFDP4GR7020` bundle, which preserves completed, skipped, and failed rows in one provider-configured run context. PostgreSQL, SQL Server, and Oracle latest-satellite timing remains outside the PIT/bridge, save-threshold, MySQL latest-satellite, and DB2 hotspot closures; staged DB2 bulk, provider-native chunk execution, DB2 live-schema timing evidence, dirty-context saves, unsupported read shapes, and failed binary hash-key storage rows remain outside completed timing evidence.

## Evidence Postures

| Posture | Meaning |
| --- | --- |
| `completed-timing` | A checked-in benchmark row completed and may support a timing claim only with its artifact triplet and run context. |
| `skipped-placeholder` | A checked-in optional-provider row is present with `executionStatus=skipped`, a skip reason, `iterations=0`, blank or null metrics, deterministic execution detail, and `persistedOutcome=not executed`. It preserves row identity and planned strategy facts but is not timing evidence. |
| `diagnostics-only` | Repository diagnostics, capability profiles, and provider registration prove a bounded strategy candidate or fallback condition, but no benchmark timing row is claimed. |
| `smoke-only` | Opt-in live or smoke coverage proves representative execution behavior when configured, but it is not a measured benchmark lane. |
| `storage-footprint` | Hash-key storage sidecars record physical storage footprint facts and scoped benchmark rows for hash-key variants. They are not successful provider timing claims for skipped or failed rows. |

## v0.42 Promotion Gates

For the v0.42 provider performance evidence and tuning baseline, downstream work must apply these gates before promoting a provider claim:

- Cite the matrix row identity with `scenario`, `provider`, `baseline`, and `posture`.
- Treat only `completed-timing` rows with a preserved provider-configured artifact triplet and run context as measured timing evidence.
- Keep `skipped-placeholder`, `diagnostics-only`, `smoke-only`, and `storage-footprint` rows out of measured timing claims.
- Keep PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite tuning limited to the already documented hub-parent, non-multi-active shapes.
- Fall back to provider-neutral save or read behavior when the matching `DVAULT_TEST_*` connection string is unset, provider diagnostics do not select the expected strategy, the provider mismatches, the context is dirty for provider save work, the read shape is unsupported or incomplete, PIT/bridge maintenance is stale, or the row lacks completed provider-configured benchmark evidence.
- Do not promote maintained-bridge read rows into write-side bridge-maintenance push-down claims. Bridge maintenance push-down needs its own source seam, diagnostics vocabulary, parity coverage, and benchmark artifact lane.

Provider-specific tuning thresholds are starting gates, not universal promises:

| Provider | v0.42 starting gate | Promotion boundary |
| --- | --- | --- |
| PostgreSQL | Retain direct or UNNEST below 60 operations; use staged COPY at 60-plus operations. | Promote only with completed configured evidence for the cited row; skipped root rows remain row identity and planned-path guidance. |
| SQL Server | Native bulk starts at 50-plus total operations and no more than 500 satellite operations. | Stop when operation count is below 50, satellite count exceeds 500, the context is dirty, or diagnostics do not select `SqlServerDataVaultSaveStrategy`. |
| MySQL | Retain multi-row behavior for smaller eligible batches and staged bulk for larger eligible batches; tiny satellite-history fallback remains provider-neutral. | Stop when candidate gates decline, diagnostics do not select the strategy, or the row is only skipped/diagnostics/smoke posture. |
| Oracle | Direct optimized batching starts at 50-plus total operations and no more than 10000 satellite operations. | Do not claim staged Oracle bulk unless new completed evidence selects it; stop when Oracle operation or satellite gates fail. |
| DB2 | Clean-context set-based save only. | Promote only the completed DB2 hotspot bundle rows for clean-context save plus supported latest-satellite/PIT/bridge reads; do not claim staged DB2 bulk, provider-native chunk execution, DB2 live-schema timing evidence, dirty-context save support, or unsupported read shapes. |

## Authoritative Sources

- Root quick baseline: [benchmark-summary.md](../../benchmark-summary.md), [benchmark-summary.csv](../../benchmark-summary.csv), and [benchmark-summary.json](../../benchmark-summary.json).
- v0.32.0 smoke-read provider-configured PIT/bridge evidence for PostgreSQL, SQL Server, MySQL, and Oracle: [benchmark-summary.md](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md), [benchmark-summary.csv](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.csv), and [benchmark-summary.json](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.json).
- MySQL latest-satellite provider-configured evidence for ticket `06FE4QQ9VF7B74E60CXEHSS5XW`: [benchmark-summary.md](../../artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.md), [benchmark-summary.csv](../../artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.csv), and [benchmark-summary.json](../../artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.json).
- SQL Server bulk-threshold provider-configured evidence: [sqlserver-threshold-decision.md](../../sqlserver-threshold-decision.md), [benchmark-summary.md](../../artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-20260620/benchmark-summary.md), [benchmark-summary.csv](../../artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-20260620/benchmark-summary.csv), and [benchmark-summary.json](../../artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-20260620/benchmark-summary.json).
- DB2 hotspot provider-configured evidence for clean-context save plus supported latest-satellite/PIT/bridge reads: [benchmark-summary.md](../../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.md), [benchmark-summary.csv](../../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.csv), and [benchmark-summary.json](../../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.json).
- DB2 host-to-Podman release validation for the same scoped DB2 lanes: [benchmark-summary.md](../../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260621/benchmark-summary.md), [benchmark-summary.csv](../../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260621/benchmark-summary.csv), and [benchmark-summary.json](../../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260621/benchmark-summary.json).
- Provider binary-vs-hex hash-key matrix for ticket `06FE4R1N2ADN77NDFDP4GR7020`: [benchmark-summary.md](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.md), [benchmark-summary.csv](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.csv), [benchmark-summary.json](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.json), [hash-key-footprint.md](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.md), [hash-key-footprint.csv](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.csv), and [hash-key-footprint.json](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.json).
- Benchmark artifact rules: [Performance Evidence And Benchmark Artifact Contract](performance-evidence-benchmark-artifact-contract.md).
- Save boundary and provider save posture: [DVault V1 Explicit Save Service](../architecture/dvault-v1-explicit-save-service.md).
- Read boundary and provider read posture: [DVault V1 PIT And Bridge Boundary](../architecture/dvault-v1-pit-bridge-boundary.md).
- Adopter performance guidance: [Performance Profiles](../performance-profiles.md).
- DB2 release posture: [DVault v0.34.0 Release Notes](../releases/v0.34.0.md).
- DB2 opt-in smoke posture: [Db2DataVaultSmokeTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs).
- Hash-key storage contract: [Hash Key Storage Profile Contract](hash-key-storage-profile-contract.md).
- Hash-key storage evidence entry point: [hash-key-footprint.md](../../hash-key-footprint.md), [hash-key-footprint.csv](../../hash-key-footprint.csv), [hash-key-footprint.json](../../hash-key-footprint.json), and the carried-forward [06F9GF66B10J4K7RBDTJ9NQRQC SQLite hash-key storage matrix bundle](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/).
- Row verifier coverage: [BenchmarkScenarioExecutionTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs).
- Closed fallback vocabularies: [DataVaultSaveStrategyFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultSaveStrategyFallbackCauseKind.cs), [DataVaultReadStrategyFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs), and [DataVaultChunkedSaveStateFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultChunkedSaveStateFallbackCauseKind.cs).

## Provider Evidence Manifest V1

When benchmark or documentation work needs a machine-readable provider-evidence row, use this manifest shape instead of parsing prose-only benchmark notes or copying a bespoke table shape. The manifest is a contract for shared row data; this document does not add a new exporter, replace `benchmark-summary.md`, `benchmark-summary.csv`, or `benchmark-summary.json`, or create runtime manifest discovery.

The document schema version is `dvault.provider-evidence.v1`. A manifest document contains exactly these top-level fields in this order:

| Field | Type | Rule |
| --- | --- | --- |
| `schemaVersion` | string | Required. Must be `dvault.provider-evidence.v1`. |
| `rows` | array | Required. Sort rows by `scenario`, `provider`, `baseline`, and `evidencePosture` with ordinal string comparison. |

Each `rows[]` item uses deterministic camelCase fields in this order:

| Field | Type | Null or omission rule |
| --- | --- | --- |
| `scenario` | string | Required. Use the benchmark `scenarioName` value when the row is benchmark-backed. Use the matrix scenario label for docs-owned rows. |
| `provider` | string | Required. Reuse the provider display names from the benchmark artifact or matrix. |
| `baseline` | string | Required. Use the benchmark `baselineName` value or the documented baseline label for docs-owned rows. |
| `strategyFamily` | string or null | Required. Use the benchmark `strategyFamily` value when present; use `null` only when a docs-only row has no strategy-family label. |
| `datasetSize` | string or null | Required. Use the benchmark row value when present; use `null` for docs-only rows that do not describe a benchmark dataset. |
| `changeRatio` | string or null | Required. Use the benchmark row value when present; use `null` for docs-only rows that do not describe a benchmark change-ratio input. |
| `sourceArtifacts` | array of strings | Required. Use repository-relative paths. Benchmark-backed rows list the artifact triplet; docs-only rows list their documentation, diagnostics, smoke, or storage-footprint sources. |
| `evidencePosture` | string | Required. Use one of the closed posture values below. |
| `executionStatus` | string or null | Required. Use `completed`, `skipped`, or `failed` for benchmark-backed rows. Use `null` for docs-only rows without a benchmark execution. |
| `skipReason` | string or null | Required. Use the benchmark skip or failure reason for skipped and failed rows; otherwise `null`. |
| `workloadShape` | string or null | Required. Use a bounded workload label such as `provider-native-bulk-ingestion` for save/workload rows; otherwise `null`. |
| `readShape` | string or null | Required. Use the closed read-shape vocabulary for read rows; otherwise `null`. |
| `selectedPath` | string or null | Required. Use the selected `executionPath` only when the row completed or diagnostics selected a provider path; otherwise `null`. |
| `plannedPath` | string or null | Required. Use the planned `executionPath` for skipped placeholder or docs-only candidate rows; otherwise `null`. |
| `selectedStrategy` | string or null | Required. Use the selected provider strategy only when the row completed or diagnostics selected it; convert `<none>` to `null`. |
| `plannedStrategy` | string or null | Required. Use the planned provider strategy for skipped placeholder or docs-only candidate rows; convert `<none>` to `null`. |
| `fallbackCauses` | array of strings | Required. Use an empty array when no bounded fallback cause applies; never serialize `none` as a cause. |
| `resultSummary` | object | Required. Use the bounded result summary shape below. |

The `resultSummary` object uses these fields in this order:

| Field | Type | Rule |
| --- | --- | --- |
| `iterations` | number or null | Use the benchmark row iteration count for benchmark-backed rows. Use `null` for docs-only rows. |
| `metricState` | string | Use `present`, `not-executed`, or `not-applicable`. Only `present` rows may support timing or allocation claims. |
| `persistedOutcome` | string or null | Use the benchmark `persistedOutcome` value for benchmark-backed rows; use `null` for docs-only rows. |
| `summary` | string | Required bounded summary text. Do not include raw request values, credentials, machine paths, stack traces, provider messages, or arbitrary prose copied from benchmark output. |

Closed vocabularies:

- `evidencePosture`: `completed-timing`, `skipped-placeholder`, `diagnostics-only`, `smoke-only`, `storage-footprint`.
- `executionStatus`: `completed`, `skipped`, `failed`, or `null` for docs-only rows.
- `readShape`: `LatestSatellite`, `PitAsOf`, `Bridge`, or `null`.
- `metricState`: `present`, `not-executed`, `not-applicable`.
- `fallbackCauses`: enum names from `DataVaultSaveStrategyFallbackCauseKind`, `DataVaultReadStrategyFallbackCauseKind`, and `DataVaultChunkedSaveStateFallbackCauseKind`; use `[]` when the detail token says `none`.

### Source Mapping

Benchmark-backed rows map from the existing artifact row fields without changing the benchmark triplet:

- `scenarioName` -> `scenario`
- `provider` -> `provider`
- `baselineName` -> `baseline`
- `strategyFamily` -> `strategyFamily`
- `datasetSize` -> `datasetSize`
- `changeRatio` -> `changeRatio`
- `executionStatus` -> `executionStatus`
- `skipReason` -> `skipReason`
- `iterations` -> `resultSummary.iterations`
- `persistedOutcome` -> `resultSummary.persistedOutcome`

Provider facts map from deterministic `executionDetail` tokens emitted by `BenchmarkExecutionDetails`, not arbitrary prose. Split the detail string on `; ` and read only named `key=value` tokens:

- `executionPath` supplies `selectedPath` for completed rows and `plannedPath` for skipped placeholder rows.
- `selectedStrategy` supplies `selectedStrategy` for completed rows when not `<none>`. For skipped provider guidance rows, it supplies `plannedStrategy` when `plannedReadStrategy` is absent and the value is not `<none>`.
- `plannedReadStrategy` supplies `plannedStrategy` for skipped read rows when not `<none>`.
- `readShape` supplies `readShape`.
- `fallbackCauses`, `readShapeFallbackCauses`, and `stagedProviderBulkFallbackCauses` supply `fallbackCauses`; split pipe-delimited values and treat `none` as `[]`.
- Boundary tokens such as `transfer`, `nativeBulkBoundary`, `stagedBulkBoundary`, `smallBatchBoundary`, `oracleBulkBoundary`, `stagedOracleBulk`, `db2SaveBoundary`, `cleanupBoundary`, and `providerSpecificReadStrategy` are allowed inputs for `resultSummary.summary` and docs text, but they do not create new top-level manifest fields in v1.

Docs-owned rows use the same shape with `executionStatus=null`, `iterations=null`, `metricState=not-applicable`, and `persistedOutcome=null` unless a checked-in benchmark artifact backs the row. Do not cite docs-owned `diagnostics-only`, `smoke-only`, or `storage-footprint` rows as measured timing evidence.

### Representative Rows

```json
{
  "schemaVersion": "dvault.provider-evidence.v1",
  "rows": [
    {
      "scenario": "latest-satellite-read",
      "provider": "SQLite local temporary files",
      "baseline": "dvault-adddvaultsqlite-optimized",
      "strategyFamily": "sqlite-optimized-dvault",
      "datasetSize": "100 customers, 10 profile states each",
      "changeRatio": "90% repeat-change history latest read",
      "sourceArtifacts": [
        "benchmark-summary.md",
        "benchmark-summary.csv",
        "benchmark-summary.json"
      ],
      "evidencePosture": "completed-timing",
      "executionStatus": "completed",
      "skipReason": null,
      "workloadShape": null,
      "readShape": "LatestSatellite",
      "selectedPath": "DVault SQLite optimized path",
      "plannedPath": null,
      "selectedStrategy": "SqliteDataVaultReadStrategy",
      "plannedStrategy": null,
      "fallbackCauses": [],
      "resultSummary": {
        "iterations": 3,
        "metricState": "present",
        "persistedOutcome": "100 latest profile satellite rows read from 1000 seeded profile states",
        "summary": "completed SQLite optimized latest-satellite timing row"
      }
    },
    {
      "scenario": "provider-native-bulk-ingestion",
      "provider": "PostgreSQL external provider",
      "baseline": "dvault-adddvaultpostgres-optimized",
      "strategyFamily": "postgres-optimized-dvault",
      "datasetSize": "20 order-product pairs, 3 fulfillment satellite operations",
      "changeRatio": "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
      "sourceArtifacts": [
        "benchmark-summary.md",
        "benchmark-summary.csv",
        "benchmark-summary.json"
      ],
      "evidencePosture": "skipped-placeholder",
      "executionStatus": "skipped",
      "skipReason": "not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.",
      "workloadShape": "provider-native-bulk-ingestion",
      "readShape": null,
      "selectedPath": null,
      "plannedPath": "DVault PostgreSQL staged bulk save path",
      "selectedStrategy": null,
      "plannedStrategy": "PostgresDataVaultSaveStrategy",
      "fallbackCauses": [],
      "resultSummary": {
        "iterations": 0,
        "metricState": "not-executed",
        "persistedOutcome": "not executed",
        "summary": "planned staged COPY boundary only; not measured timing evidence"
      }
    },
    {
      "scenario": "pit-as-of-read",
      "provider": "DB2 external provider",
      "baseline": "dvault-adddvaultdb2-optimized",
      "strategyFamily": "db2-optimized-dvault",
      "datasetSize": "100 customers, 100 PIT rows, 2 satellite segments",
      "changeRatio": "as-of read after latest profile/status snapshots",
      "sourceArtifacts": [
        "artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.md",
        "artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.csv",
        "artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.json"
      ],
      "evidencePosture": "completed-timing",
      "executionStatus": "completed",
      "skipReason": null,
      "workloadShape": null,
      "readShape": "PitAsOf",
      "selectedPath": "DVault DB2 optimized PIT read path",
      "plannedPath": null,
      "selectedStrategy": "Db2DataVaultReadStrategy",
      "plannedStrategy": null,
      "fallbackCauses": [],
      "resultSummary": {
        "iterations": 1,
        "metricState": "present",
        "persistedOutcome": "100 PIT as-of rows read across profile and status satellite snapshots",
        "summary": "completed DB2 optimized PIT timing row for a supported maintained PIT shape"
      }
    },
    {
      "scenario": "pit-as-of-read",
      "provider": "DB2 external provider",
      "baseline": "AddDVaultDb2() / Db2DataVaultReadStrategy",
      "strategyFamily": "db2-optimized-dvault",
      "datasetSize": null,
      "changeRatio": null,
      "sourceArtifacts": [
        "docs/releases/v0.34.0.md",
        "tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs"
      ],
      "evidencePosture": "diagnostics-only",
      "executionStatus": null,
      "skipReason": null,
      "workloadShape": null,
      "readShape": "PitAsOf",
      "selectedPath": null,
      "plannedPath": "diagnostics-gated DB2 PIT read candidate",
      "selectedStrategy": null,
      "plannedStrategy": "Db2DataVaultReadStrategy",
      "fallbackCauses": [],
      "resultSummary": {
        "iterations": null,
        "metricState": "not-applicable",
        "persistedOutcome": null,
        "summary": "docs-owned diagnostics posture; no DB2 benchmark timing row claimed"
      }
    }
  ]
}
```

## Global Claim Rules

- When a follow-up ticket needs a provider-evidence manifest, populate `dvault.provider-evidence.v1` rows from the contract above instead of inventing parallel fields or scraping human-only markdown tables.
- Cite matrix rows with scenario, provider, baseline, and posture. Do not cite `skipped-placeholder`, `diagnostics-only`, `smoke-only`, or `storage-footprint` rows as measured provider performance.
- Keep timing claims attached to the artifact triplet, run context, provider filter, load-timestamp storage, iteration count, warmup count, hardware, runtime, dataset size, request shape, provider configuration, and skip/failure rows.
- SQLite remains the completed optimized latest-satellite timing row in the root quick triplet.
- MySQL has a completed provider-configured optimized latest-satellite timing row in `artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.*` with `MySqlDataVaultReadStrategy` selected. That row is a scoped completed timing baseline, not a provider-neutral fallback improvement claim by itself.
- DB2 has a completed provider-configured optimized latest-satellite timing row in `artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.*`; DB2 timing is limited to the provider-configured DB2 hotspot bundle and its supported hub-parent, non-multi-active shape.
- PostgreSQL, SQL Server, and Oracle have diagnostics-gated latest-satellite read-strategy registration; their root optional-provider latest-satellite rows remain skipped placeholders until the corresponding `DVAULT_TEST_*_CONNECTION_STRING` is configured, so no measured latest-satellite timing is claimed for those providers here.
- PostgreSQL PIT/bridge reads have completed provider-configured timing rows in `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.*` with `PostgresDataVaultReadStrategy` selected for the supported maintained shapes. The root quick triplet still keeps PostgreSQL PIT/bridge rows as skipped placeholders when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset.
- SQL Server PIT/bridge reads have completed provider-configured timing rows in the same v0.32.0 smoke-read bundle with `SqlServerDataVaultReadStrategy` selected for the supported maintained shapes. SQL Server latest-satellite timing remains outside this PIT/bridge closure.
- MySQL PIT/bridge reads have completed provider-configured timing rows in the same v0.32.0 smoke-read bundle with `MySqlDataVaultReadStrategy` selected for the supported maintained shapes. MySQL latest-satellite timing is separate from that PIT/bridge closure and is cited from the ticket `06FE4QQ9VF7B74E60CXEHSS5XW` bundle; the older v0.32 smoke-read latest-satellite row remains historical provider-neutral fallback evidence with `selectedStrategy=<none>`.
- Oracle PIT/bridge reads have completed provider-configured timing rows in the same v0.32.0 smoke-read bundle with `OracleDataVaultReadStrategy` selected for the supported maintained shapes. Oracle latest-satellite timing remains outside this PIT/bridge closure; the v0.32 smoke-read latest-satellite row selected provider-neutral fallback with `selectedStrategy=<none>`.
- DB2 evidence includes completed provider-configured timing rows in `artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.*` for the provider-neutral save comparison row, the clean-context optimized save row selected by `Db2DataVaultSaveStrategy`, and supported latest-satellite/PIT/bridge read rows selected by `Db2DataVaultReadStrategy`. Staged DB2 bulk, provider-native chunk execution, DB2 live-schema timing evidence, dirty-context save claims, unsupported latest-satellite shapes, stale PIT/bridge maintenance, and incomplete read-shape evidence remain outside completed DB2 timing evidence.
- Binary-vs-hex storage comparisons are SQLite-local for the footprint baseline and provider-configured for the ticket `06FE4R1N2ADN77NDFDP4GR7020` bundle. Cite only completed rows as timing evidence, and keep failed or skipped rows as caveats and follow-up signals.

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
| `provider-native-bulk-ingestion` | SQL Server external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | SQL Server bulk-threshold bundle | Provider-neutral comparison row completed in the configured SQL Server 2026-06-20 bundle with `selectedStrategy=<none>` and `fallbackCauses=NoProviderSpecificStrategyRegistered`. Root quick rows may still be skipped when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset. |
| `provider-native-bulk-ingestion` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | SQL Server bulk-threshold bundle | SQL Server native bulk row completed with `SqlServerDataVaultSaveStrategy`, `SqlBulkCopy`, `nativeBulkBoundary=50-plus-operations`, and `cleanupBoundary=temporary-staging-table`; the 50/500 gate remains unchanged. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `skipped-placeholder` | Root benchmark triplet | Optional provider-neutral comparison row; skipped when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvaultmysql-multi-row` | `mysql-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned retained MySQL multi-row path below the staged boundary. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned MySQL staged bulk path using temporary staging tables at 60-plus operations. |
| `provider-native-bulk-ingestion` | Oracle external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `skipped-placeholder` | Root benchmark triplet | Optional provider-neutral comparison row; skipped when `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset. |
| `provider-native-bulk-ingestion` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Planned Oracle direct optimized batching; staged Oracle bulk remains `not-selected-no-measured-win`. |
| `provider-native-bulk-ingestion` | DB2 external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | DB2 hotspot bundle | Provider-neutral DB2 comparison row completed with no provider-specific strategy selected and `NoProviderSpecificStrategyRegistered`; cite only as fallback comparison evidence. |
| `provider-native-bulk-ingestion` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | DB2 hotspot bundle | Completed DB2 clean-context optimized save row selected `Db2DataVaultSaveStrategy`; staged DB2 bulk, provider-native chunk execution, dirty-context saves, and unsupported save shapes are not claimed. |
| DB2 clean-context save smoke | DB2 external provider | `AddDVaultDb2()` / `Db2DataVaultSaveStrategy` | `db2-optimized-dvault` | `diagnostics-only` and `smoke-only` | v0.34.0 release notes and DB2 smoke evidence | DB2 supports optimized clean-context hub, link, and ordinary satellite saves through provider diagnostics and opt-in smoke coverage. This supports strategy behavior but should not be cited instead of the DB2 hotspot bundle for measured timing. |

The root benchmark triplet keeps optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows visible as skipped placeholders when their connection strings are unset. Completed external-provider timing claims, where needed, must cite the provider-threshold bundles or DB2 hotspot bundle linked from [Performance Profiles](../performance-profiles.md), not skipped root rows alone.

## Read Matrix

| Scenario | Provider | Baseline | Strategy family | Posture | Canonical row source | Claim boundary |
| --- | --- | --- | --- | --- | --- | --- |
| `latest-satellite-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral latest read over seeded profile states. |
| `latest-satellite-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized latest-satellite read selected `SqliteDataVaultReadStrategy`. |
| `pit-as-of-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral PIT as-of read over explicitly maintained PIT rows. |
| `pit-as-of-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized PIT read selected `SqliteDataVaultReadStrategy`. |
| `bridge-traversal-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral bridge traversal over explicitly maintained bridge rows. |
| `bridge-traversal-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized bridge read selected `SqliteDataVaultReadStrategy`. |
| `latest-satellite-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `PostgresDataVaultReadStrategy` for diagnostics-gated latest-satellite reads. The skipped placeholder is row identity and planned strategy evidence only, not completed PostgreSQL timing. |
| `pit-as-of-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Provider-configured row completed with `PostgresDataVaultReadStrategy` selected for a supported maintained PIT shape. The root quick triplet remains a skipped placeholder when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset and is not the completed timing source. |
| `bridge-traversal-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Provider-configured row completed with `PostgresDataVaultReadStrategy` selected for a supported maintained bridge shape. The root quick triplet remains a skipped placeholder when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset and is not the completed timing source. |
| `latest-satellite-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row records planned `SqlServerDataVaultReadStrategy` for diagnostics-gated latest-satellite reads. The 2026-06-20 SQL Server bulk-threshold triplet includes an incidental latest-satellite row, but this matrix keeps `P0.02` unpromoted until a dedicated latest-satellite evidence ticket accepts that read lane. |
| `pit-as-of-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Provider-configured row completed with `SqlServerDataVaultReadStrategy` selected for a supported maintained PIT shape. The root quick triplet remains a skipped placeholder when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset and is not the completed timing source. |
| `bridge-traversal-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Provider-configured row completed with `SqlServerDataVaultReadStrategy` selected for a supported maintained bridge shape. The root quick triplet remains a skipped placeholder when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset and is not the completed timing source. |
| `latest-satellite-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | Ticket `06FE4QQ9VF7B74E60CXEHSS5XW` MySQL latest-satellite bundle | Provider-configured row completed with `selectedStrategy=MySqlDataVaultReadStrategy`, `plannedReadStrategy=MySqlDataVaultReadStrategy`, `readShape=LatestSatellite`, and no fallback causes. The root quick-baseline row may remain skipped when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset, and the ticket bundle is not a provider-neutral fallback improvement comparator by itself. |
| `pit-as-of-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Provider-configured smoke-read row completed with `selectedStrategy=MySqlDataVaultReadStrategy`; timing claims stay scoped to that artifact bundle and its run context, while the root quick-baseline row may remain skipped when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset. |
| `bridge-traversal-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Provider-configured smoke-read row completed with `selectedStrategy=MySqlDataVaultReadStrategy`; timing claims stay scoped to that artifact bundle and its run context, while the root quick-baseline row may remain skipped when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset. |
| `latest-satellite-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `skipped-placeholder` | Root benchmark triplet | Guidance row preserves Oracle latest-satellite row identity and planned strategy posture, but no measured Oracle latest-satellite timing is claimed while the optional provider row is skipped; the v0.32 smoke-read row selected provider-neutral fallback with `selectedStrategy=<none>`. |
| `pit-as-of-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Oracle configured PIT read completed with `OracleDataVaultReadStrategy` selected over explicitly maintained PIT rows. Latest-satellite reads remain outside this PIT/bridge closure for Oracle. |
| `bridge-traversal-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | v0.32.0 smoke-read bundle | Oracle configured bridge read completed with `OracleDataVaultReadStrategy` selected over explicitly maintained bridge rows. Latest-satellite reads remain outside this PIT/bridge closure for Oracle. |
| `latest-satellite-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | DB2 hotspot bundle | Completed DB2 latest-satellite row selected `Db2DataVaultReadStrategy` for the supported hub-parent, non-multi-active shape; provider mismatch, unsupported parents, and multi-active shapes remain provider-neutral fallback. |
| `pit-as-of-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | DB2 hotspot bundle | Completed DB2 PIT row selected `Db2DataVaultReadStrategy` over a supported maintained PIT shape; incomplete read-shape evidence or stale PIT maintenance remains provider-neutral fallback. |
| `bridge-traversal-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | DB2 hotspot bundle | Completed DB2 bridge row selected `Db2DataVaultReadStrategy` over a supported maintained bridge shape; incomplete read-shape evidence, stale maintenance, or unsupported bridge shapes remain provider-neutral fallback. |
| Latest-satellite/PIT/bridge read smoke | DB2 external provider | `AddDVaultDb2()` / `Db2DataVaultReadStrategy` | `db2-optimized-dvault` | `diagnostics-only` and `smoke-only` | DB2 provider registration and DB2 smoke evidence | DB2 registers diagnostics-gated latest-satellite/PIT/bridge read dispatch and opt-in representative smoke coverage. This supports strategy behavior but should not be cited instead of the DB2 hotspot bundle for measured timing. |

## Deferred Bridge Maintenance Push-Down

Bridge maintenance push-down is intentionally not a completed provider-evidence row in this matrix. The completed `bridge-traversal-read` rows prove read-strategy selection over explicitly maintained bridge rows; they do not prove provider-specific execution of `RebuildBridgeAsync(...)` or `MaintainBridgeAsync(...)`.

The current evidence supports a defer recommendation. PostgreSQL has a provider-specific PIT maintenance strategy, while bridge maintenance remains the provider-neutral service surface. A later bridge-maintenance row can be added only after the repository carries a core/provider bridge-maintenance seam, bridge-specific gate and fallback diagnostics, parity tests for the existing many-to-many and hierarchy maintenance semantics, and a preserved provider-configured benchmark artifact triplet. Until then, downstream work should keep bridge push-down out of implementation scope and treat `06FE4RKGASKV6F7DF0RD1WTAV4` as the immediate documentation follow-on.

## Hash-Key Storage Matrix

| Variant | Provider | Algorithm | Storage profile | Posture | Canonical source | Claim boundary |
| --- | --- | --- | --- | --- | --- | --- |
| `sha256-v1-hex` | SQLite local temporary files | `sha256-v1` | `HexString` | `storage-footprint` | Hash-key storage bundle and root footprint summary | Default 32-byte digest stored as 64 lowercase hex characters in SQLite `TEXT`. |
| `sha256-v1-binary` | SQLite local temporary files | `sha256-v1` | `Binary` | `storage-footprint` | Hash-key storage bundle and root footprint summary | Explicit opt-in physical binary storage for a 32-byte digest in SQLite `BLOB`; public boundaries still use lowercase hex strings. |
| `sha256-128-v1-hex` | SQLite local temporary files | `sha256-128-v1` | `HexString` | `storage-footprint` | Hash-key storage bundle and root footprint summary | 16-byte digest stored as 32 lowercase hex characters in SQLite `TEXT`. |
| `sha256-128-v1-binary` | SQLite local temporary files | `sha256-128-v1` | `Binary` | `storage-footprint` | Hash-key storage bundle and root footprint summary | Explicit opt-in physical binary storage for a 16-byte digest in SQLite `BLOB`; public boundaries still use lowercase hex strings. |
| `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, `sha256-128-v1-binary` | PostgreSQL external provider | `sha256-v1` / `sha256-128-v1` | `HexString` / `Binary` | `completed-timing` for completed rows; failed rows are caveats | Provider binary-vs-hex bundle | Completed rows are measured only with the ticket bundle. Failed binary rows record PostgreSQL `bytea = text` operator incompatibilities and are not successful timing evidence. |
| `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, `sha256-128-v1-binary` | SQL Server external provider | `sha256-v1` / `sha256-128-v1` | `HexString` / `Binary` | `skipped-placeholder` | Provider binary-vs-hex bundle | The local SQL Server lane was skipped because the configured instance required encryption unsupported by the local runtime. It preserves row identity, not timing evidence. |
| `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, `sha256-128-v1-binary` | MySQL external provider | `sha256-v1` / `sha256-128-v1` | `HexString` / `Binary` | `completed-timing` for completed rows; failed rows are caveats | Provider binary-vs-hex bundle | Completed rows are measured only with the ticket bundle. Failed binary rows record provider column-width truncation and are not successful timing evidence. |
| `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, `sha256-128-v1-binary` | Oracle external provider | `sha256-v1` / `sha256-128-v1` | `HexString` / `Binary` | `completed-timing` for completed rows; failed rows are caveats | Provider binary-vs-hex bundle | Completed rows are measured only with the ticket bundle. Failed rows record Oracle reexecution and null-reference failures and are not successful timing evidence. |
| `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, `sha256-128-v1-binary` | DB2 external provider | `sha256-v1` / `sha256-128-v1` | `HexString` / `Binary` | `completed-timing` for completed rows; failed rows are caveats | Provider binary-vs-hex bundle | Completed rows are measured only with the ticket bundle. Failed binary rows record DB2 string truncation for several save/latest/PIT shapes, while bridge binary rows completed. |

The authoritative SQLite footprint sidecars are [hash-key-footprint.md](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.md), [hash-key-footprint.csv](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.csv), and [hash-key-footprint.json](../../artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.json). The provider-configured binary-vs-hex bundle adds [benchmark-summary.md](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.md), [benchmark-summary.csv](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.csv), [benchmark-summary.json](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.json), [hash-key-footprint.md](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.md), [hash-key-footprint.csv](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.csv), and [hash-key-footprint.json](../../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.json), with root [hash-key-footprint.md](../../hash-key-footprint.md), [hash-key-footprint.csv](../../hash-key-footprint.csv), and [hash-key-footprint.json](../../hash-key-footprint.json) as quick validation entry points. The storage-profile contract keeps logical hash-key values as canonical lowercase hexadecimal strings at request, save, read, diagnostics, and support-bundle boundaries.

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

Read claims must stop or fall back when PIT or bridge rows are not explicitly maintained before read traffic, required read-shape evidence is missing, diagnostics do not select the expected strategy, unsupported latest-satellite provider optimization is requested, or any of these bounded fallback causes apply:

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

Provider-specific threshold facts remain part of the stop conditions: PostgreSQL staged COPY starts at 60-plus operations, SQL Server native bulk starts at 50-plus operations and accepts at most 500 satellite operations, MySQL native and staged candidates apply only when their candidate gates are met, Oracle direct optimized batching starts at 50-plus operations and accepts at most 10000 satellite operations, and DB2 has no staged bulk, provider-native chunk execution, dirty-context save support, unsupported latest-satellite shape support, or DB2 live-schema timing/provisioning guarantee.

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
matrix row: scenario=latest-satellite-read; provider=MySQL external provider; baseline=dvault-adddvaultmysql-optimized; posture=completed-timing
claim: measured MySQL optimized latest-satellite read evidence, valid only with artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.* and its run context; do not cite it as a provider-neutral fallback improvement comparator by itself
```

```text
matrix row: scenario=pit-as-of-read; provider=Oracle external provider; baseline=dvault-adddvaultoracle-optimized; posture=completed-timing
claim: measured Oracle PIT read evidence, valid only with the v0.32.0 smoke read benchmark triplet and run context
```

```text
matrix row: scenario=pit-as-of-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; posture=completed-timing
claim: measured DB2 PIT read evidence, valid only with artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.* and its run context
```

```text
matrix row: scenario=pit-as-of-read; provider=PostgreSQL external provider; baseline=dvault-adddvaultpostgres-optimized; posture=completed-timing
claim: measured PostgreSQL PIT read evidence, valid only with artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.* and its run context
```
