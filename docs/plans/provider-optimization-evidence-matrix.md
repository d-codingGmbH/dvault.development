# Provider Optimization Evidence Matrix

Status: v1 evidence contract
Ticket: 06FBSC3N7ZFVQW3AV2JJ8T7Q7W

## Purpose

This document is the canonical lookup surface for DVault provider optimization evidence rows. Later tickets should cite these matrix rows by scenario, provider, baseline, and evidence posture instead of restating benchmark notes or mixing measured timing evidence with skipped, diagnostics-only, smoke-only, storage-footprint, or docs-only contract guidance.

The matrix reuses the existing benchmark artifact contract vocabulary from [Performance Evidence And Benchmark Artifact Contract](performance-evidence-benchmark-artifact-contract.md). It does not add benchmark fields, change benchmark schemas, or widen successful timing claims beyond completed rows in checked-in artifact bundles. PIT full-rebuild maintenance rows are a separate row family from `pit-as-of-read` and `bridge-traversal-read`; read rows prove strategy selection over already-maintained read models and cannot be cited as PIT maintenance timing evidence. The root quick benchmark triplet remains the SQLite-local and skipped optional-provider baseline. The 2026-06-23 provider optimization closure bundle is the current completed-timing source for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 provider-native save rows plus latest-satellite, PIT, and bridge read rows. PostgreSQL, SQL Server, and MySQL PIT full-rebuild maintenance availability is source and test backed only until a dedicated `pit-full-rebuild-maintenance` artifact triplet exists for the exact provider lane. Earlier v0.32.0 smoke-read, SQL Server bulk-threshold, MySQL latest-satellite, and DB2 hotspot bundles remain historical run-context evidence. Provider binary-vs-hex hash-key storage participation is cited from the checked-in ticket `06FE4R1N2ADN77NDFDP4GR7020` bundle, which preserves completed, skipped, and failed rows in one provider-configured run context. Staged DB2 bulk, provider-native chunk execution, dirty-context saves, unsupported read shapes, write-side bridge-maintenance push-down, and failed binary hash-key storage rows remain outside completed timing evidence.

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
- Treat `pit-full-rebuild-maintenance` rows as the only PIT full-rebuild maintenance timing row family. Do not promote `pit-as-of-read` or `bridge-traversal-read` rows into maintenance evidence.
- Require every completed PIT maintenance timing claim to cite the scenario, provider, baseline or comparator, selected provider strategy or provider-neutral fallback posture, bounded fallback causes when present, run context, and preserved benchmark artifact triplet.
- Keep PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite tuning limited to the already documented hub-parent, non-multi-active shapes.
- Fall back to provider-neutral save or read behavior when the matching `DVAULT_TEST_*` connection string is unset, provider diagnostics do not select the expected strategy, the provider mismatches, the context is dirty for provider save work, the read shape is unsupported or incomplete, PIT/bridge maintenance is stale, or the row lacks completed provider-configured benchmark evidence.
- Do not promote maintained-bridge read rows into write-side bridge-maintenance push-down claims. Bridge maintenance push-down needs its own source seam, diagnostics vocabulary, parity coverage, and benchmark artifact lane.

Provider-specific tuning thresholds are starting gates, not universal promises:

| Provider | v0.42 starting gate | Promotion boundary |
| --- | --- | --- |
| PostgreSQL | Retain direct or UNNEST below 60 operations; use staged COPY at 60-plus operations. | Closed with the 2026-06-23 configured PostgreSQL closure rows; skipped root rows remain row identity and planned-path guidance only. |
| SQL Server | Native bulk starts at 100-plus total operations; mixed hub/link batches start at 900-plus total operations; no more than 500 satellite operations. | Closed with the 2026-06-23 configured SQL Server closure rows; stop when operation count is below 100, mixed hub/link batches are below 900 total operations, satellite count exceeds 500, the context is dirty, or diagnostics do not select `SqlServerDataVaultSaveStrategy`. |
| MySQL | Retain multi-row behavior below 100 operations; staged bulk is bounded to satellite-only 100-plus batches or mixed 100-to-303-operation batches; large mixed hub/link batches above 303 operations deliberately use provider-neutral fallback; tiny satellite-history fallback remains provider-neutral. | Closed with the 2026-06-23 configured MySQL closure rows; stop when candidate gates decline, diagnostics do not show the expected selected strategy or deliberate fallback, or the mixed batch is above the staged window. |
| Oracle | Direct optimized batching starts at 50-plus total operations and no more than 10000 satellite operations; read commands prefetch LOB payloads. | Closed with the 2026-06-23 configured Oracle closure rows; do not claim staged Oracle bulk unless new completed evidence selects it, and stop when Oracle operation or satellite gates fail. |
| DB2 | Clean-context set-based save with the measured 1000-row command cap. | Closed with the 2026-06-23 configured DB2 closure rows; do not claim staged DB2 bulk, provider-native chunk execution, dirty-context save support, or unsupported read shapes. |

## Authoritative Sources

- Root quick baseline: [benchmark-summary.md](../../benchmark-summary.md), [benchmark-summary.csv](../../benchmark-summary.csv), and [benchmark-summary.json](../../benchmark-summary.json).
- 2026-06-23 provider optimization closure bundle: [README.md](../../artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/README.md), PostgreSQL [benchmark-summary.md](../../artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/postgres-podman-live/benchmark-summary.md), SQL Server [benchmark-summary.md](../../artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/sqlserver-live/benchmark-summary.md), MySQL [benchmark-summary.md](../../artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/mysql-live/benchmark-summary.md), Oracle [benchmark-summary.md](../../artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/oracle-lob-prefetch/benchmark-summary.md), and DB2 [benchmark-summary.md](../../artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/db2-rowcap-1000/benchmark-summary.md).
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
- Closed fallback vocabularies: [DataVaultSaveStrategyFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultSaveStrategyFallbackCauseKind.cs), [DataVaultReadStrategyFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs), [DataVaultChunkedSaveStateFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultChunkedSaveStateFallbackCauseKind.cs), [DataVaultPitMaintenanceStrategyFallbackCauseKind.cs](../../src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs), and [SqlServerPitMaintenanceFallbackCauseKind.cs](../../src/DCoding.Data.DVault.SqlServer/SqlServerPitMaintenanceFallbackCauseKind.cs).

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
| `workloadShape` | string or null | Required. Use a bounded workload label such as `provider-native-bulk-ingestion` for save/workload rows or `pit-full-rebuild-maintenance` for PIT maintenance rows; otherwise `null`. |
| `readShape` | string or null | Required. Use the closed read-shape vocabulary for read rows. PIT maintenance rows must use `null` because maintenance evidence is not read evidence. |
| `selectedPath` | string or null | Required. Use the selected `executionPath` only when the row completed or diagnostics selected a provider path; otherwise `null`. |
| `plannedPath` | string or null | Required. Use the planned `executionPath` for skipped placeholder or docs-only candidate rows; otherwise `null`. |
| `selectedStrategy` | string or null | Required. Use the selected provider strategy only when the row completed or diagnostics selected it; convert `<none>` to `null`. Provider-neutral PIT maintenance comparator rows use `null` and identify the provider-neutral fallback posture through `selectedPath` or `plannedPath`. |
| `plannedStrategy` | string or null | Required. Use the planned provider strategy for skipped placeholder or docs-only candidate rows; convert `<none>` to `null`. |
| `fallbackCauses` | array of strings | Required. Use an empty array when no bounded fallback cause applies; never serialize `none` as a cause. PIT maintenance rows use bounded PIT maintenance fallback enum names here. |
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
- `fallbackCauses`: enum names from `DataVaultSaveStrategyFallbackCauseKind`, `DataVaultReadStrategyFallbackCauseKind`, `DataVaultChunkedSaveStateFallbackCauseKind`, `DataVaultPitMaintenanceStrategyFallbackCauseKind`, and `SqlServerPitMaintenanceFallbackCauseKind`; use `[]` when the detail token says `none`.

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
- PIT maintenance timing rows use `scenario=pit-full-rebuild-maintenance`, `workloadShape=pit-full-rebuild-maintenance`, and `readShape=null`. Their `executionDetail` must include `maintenanceScope=FullRebuild`, a selected provider strategy or `selectedStrategy=<none>` for provider-neutral fallback posture, and `fallbackCauses` populated from the bounded PIT maintenance fallback vocabularies when a provider path is declined.
- PIT maintenance boundary tokens such as `maintenanceScope` and `pitShapeBoundary` are allowed inputs for `resultSummary.summary` and docs text, but they do not create new top-level manifest fields in v1.

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
        "artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/db2-rowcap-1000/benchmark-summary.md",
        "artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/db2-rowcap-1000/benchmark-summary.csv",
        "artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/db2-rowcap-1000/benchmark-summary.json"
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
        "iterations": 5,
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
- PostgreSQL evidence includes completed provider-configured timing rows in the 2026-06-23 closure bundle for fallback save, retained direct/UNNEST save, staged `COPY` save, latest-satellite, PIT, and bridge rows.
- SQL Server evidence includes completed provider-configured timing rows in the 2026-06-23 closure bundle for fallback save, `SqlBulkCopy` save, latest-satellite, PIT, and bridge rows.
- MySQL evidence includes completed provider-configured timing rows in the 2026-06-23 closure bundle for fallback save, retained multi-row save, bounded staged save, deliberate large mixed provider-neutral fallback, latest-satellite, PIT, and bridge rows.
- Oracle evidence includes completed provider-configured timing rows in the 2026-06-23 closure bundle for fallback save, direct optimized save, latest-satellite, PIT, and bridge rows. Oracle latest/PIT/bridge timings include the ODP.NET LOB-prefetch read-command tuning.
- DB2 evidence includes completed provider-configured timing rows in the 2026-06-23 closure bundle for fallback save, clean-context optimized save selected by `Db2DataVaultSaveStrategy`, and supported latest-satellite/PIT/bridge read rows selected by `Db2DataVaultReadStrategy`. DB2 save evidence uses the measured 1000-row command cap and still excludes staged DB2 bulk, provider-native chunk execution, dirty-context save claims, unsupported latest-satellite shapes, stale PIT/bridge maintenance, and incomplete read-shape evidence.
- PIT full-rebuild maintenance evidence is not completed by read evidence. The row family is limited to the provider-neutral comparator, PostgreSQL, SQL Server, and official-provider MySQL lanes described below until sibling benchmark tickets land preserved artifact triplets.
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
| `provider-native-bulk-ingestion` | PostgreSQL external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | 2026-06-23 closure bundle | Provider-neutral comparison row completed with mean `133.453` ms, `selectedStrategy=<none>`, and `NoProviderSpecificStrategyRegistered`. |
| `provider-native-bulk-ingestion` | PostgreSQL external provider | `dvault-adddvaultpostgres-direct-or-unnest` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | PostgreSQL retained direct or UNNEST save path below the 60-operation staged boundary completed with mean `20.022` ms. |
| `provider-native-bulk-ingestion` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | PostgreSQL staged bulk save path using `COPY` at 60-plus operations completed with mean `43.757` ms. |
| `provider-native-bulk-ingestion` | SQL Server external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | 2026-06-23 closure bundle | Provider-neutral comparison row completed with mean `184.997` ms, `selectedStrategy=<none>`, and `NoProviderSpecificStrategyRegistered`. |
| `provider-native-bulk-ingestion` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | SQL Server native bulk row completed with mean `149.490` ms, `SqlServerDataVaultSaveStrategy`, `SqlBulkCopy`, `nativeBulkBoundary=100-plus-operations`, `mixedBatchBoundary=900-plus-operations`, and `cleanupBoundary=temporary-staging-table`. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | 2026-06-23 closure bundle | Provider-neutral comparison row completed with mean `183.331` ms, `selectedStrategy=<none>`, and `NoProviderSpecificStrategyRegistered`. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvaultmysql-multi-row` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Retained MySQL multi-row path completed with mean `15.827` ms. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvaultmysql-staged` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Bounded MySQL staged bulk path using `MySqlStagedDataVaultSaveStrategy` completed with mean `26.055` ms. |
| `provider-native-bulk-ingestion` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Large 903-operation mixed batch deliberately completed on provider-neutral fallback with mean `145.601` ms and `MySqlLargeMixedProviderNeutralFallback`. |
| `provider-native-bulk-ingestion` | Oracle external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | 2026-06-23 closure bundle | Provider-neutral comparison row completed with mean `302.278` ms, `selectedStrategy=<none>`, and `NoProviderSpecificStrategyRegistered`. |
| `provider-native-bulk-ingestion` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Oracle direct optimized batching completed with mean `92.537` ms; staged Oracle bulk remains `not-selected-no-measured-win`. |
| `provider-native-bulk-ingestion` | DB2 external provider | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | 2026-06-23 closure bundle | Provider-neutral comparison row completed with mean `132.811` ms, `selectedStrategy=<none>`, and `NoProviderSpecificStrategyRegistered`. |
| `provider-native-bulk-ingestion` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | DB2 clean-context optimized save selected `Db2DataVaultSaveStrategy` and completed with mean `101.037` ms after the 1000-row command-cap tuning. Staged DB2 bulk, provider-native chunk execution, dirty-context saves, and unsupported save shapes are not claimed. |
| DB2 clean-context save smoke | DB2 external provider | `AddDVaultDb2()` / `Db2DataVaultSaveStrategy` | `db2-optimized-dvault` | `diagnostics-only` and `smoke-only` | v0.34.0 release notes and DB2 smoke evidence | DB2 supports optimized clean-context hub, link, and ordinary satellite saves through provider diagnostics and opt-in smoke coverage. This supports strategy behavior but should not be cited instead of the 2026-06-23 closure bundle for measured timing. |

The root benchmark triplet keeps optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows visible as skipped placeholders when their connection strings are unset. Completed external-provider timing claims for the closed provider optimization rows must cite the 2026-06-23 closure bundle linked from [Performance Profiles](../performance-profiles.md), not skipped root rows alone.

## Read Matrix

| Scenario | Provider | Baseline | Strategy family | Posture | Canonical row source | Claim boundary |
| --- | --- | --- | --- | --- | --- | --- |
| `latest-satellite-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral latest read over seeded profile states. |
| `latest-satellite-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized latest-satellite read selected `SqliteDataVaultReadStrategy`. |
| `pit-as-of-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral PIT as-of read over explicitly maintained PIT rows. |
| `pit-as-of-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized PIT read selected `SqliteDataVaultReadStrategy`. |
| `bridge-traversal-read` | SQLite local temporary files | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `completed-timing` | Root benchmark triplet | Provider-neutral bridge traversal over explicitly maintained bridge rows. |
| `bridge-traversal-read` | SQLite local temporary files | `dvault-adddvaultsqlite-optimized` | `sqlite-optimized-dvault` | `completed-timing` | Root benchmark triplet | SQLite optimized bridge read selected `SqliteDataVaultReadStrategy`. |
| `latest-satellite-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `15.068` ms, `PostgresDataVaultReadStrategy`, `readShape=LatestSatellite`, and no fallback causes. |
| `pit-as-of-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `21.324` ms, `PostgresDataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes. |
| `bridge-traversal-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `9.002` ms, `PostgresDataVaultReadStrategy`, `readShape=Bridge`, and no fallback causes. |
| `latest-satellite-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `20.337` ms, `SqlServerDataVaultReadStrategy`, `readShape=LatestSatellite`, and no fallback causes. |
| `pit-as-of-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `59.163` ms, `SqlServerDataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes. |
| `bridge-traversal-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `9.523` ms, `SqlServerDataVaultReadStrategy`, `readShape=Bridge`, and no fallback causes. |
| `latest-satellite-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `13.878` ms, `MySqlDataVaultReadStrategy`, `readShape=LatestSatellite`, and no fallback causes. |
| `pit-as-of-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `14.461` ms, `MySqlDataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes. |
| `bridge-traversal-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `3.083` ms, `MySqlDataVaultReadStrategy`, `readShape=Bridge`, and no fallback causes. |
| `latest-satellite-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `18.783` ms after ODP.NET LOB-prefetch tuning, `OracleDataVaultReadStrategy`, `readShape=LatestSatellite`, and no fallback causes. |
| `pit-as-of-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `26.857` ms after ODP.NET LOB-prefetch tuning, `OracleDataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes. |
| `bridge-traversal-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `3.922` ms, `OracleDataVaultReadStrategy`, `readShape=Bridge`, and no fallback causes. |
| `latest-satellite-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `14.615` ms, `Db2DataVaultReadStrategy`, `readShape=LatestSatellite`, and no fallback causes. |
| `pit-as-of-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `27.207` ms, `Db2DataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes. |
| `bridge-traversal-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle | Completed with mean `4.831` ms, `Db2DataVaultReadStrategy`, `readShape=Bridge`, and no fallback causes. |
| Latest-satellite/PIT/bridge read smoke | DB2 external provider | `AddDVaultDb2()` / `Db2DataVaultReadStrategy` | `db2-optimized-dvault` | `diagnostics-only` and `smoke-only` | DB2 provider registration and DB2 smoke evidence | DB2 registers diagnostics-gated latest-satellite/PIT/bridge read dispatch and opt-in representative smoke coverage. This supports strategy behavior but should not be cited instead of the 2026-06-23 closure bundle for measured timing. |

## PIT Full-Rebuild Maintenance Row Contract

The PIT maintenance timing row family uses scenario `pit-full-rebuild-maintenance`. It is distinct from `pit-as-of-read` and `bridge-traversal-read`: PIT and bridge read rows only prove reads over already-maintained rows, while maintenance rows prove execution of `IDataVaultPitMaintenanceService.RebuildAsync(...)` for one generated PIT table.

Completed PIT maintenance timing claims must have `posture=completed-timing`, `workloadShape=pit-full-rebuild-maintenance`, `readShape=null`, `maintenanceScope=FullRebuild` in `executionDetail`, a preserved benchmark artifact triplet, and the same run context required by the shared benchmark artifact contract. Skipped, unconfigured, diagnostics-only, smoke-only, docs-only guidance, or placeholder rows are not maintenance timing claims.

| Lane | Scenario | Provider | Baseline/comparator | Strategy family | Required strategy or fallback posture | Supported shape boundary | Current claim posture |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Provider-neutral comparator | `pit-full-rebuild-maintenance` | Provider-neutral DVault comparator | `dvault-adddvault-fallback` | `provider-neutral-dvault-fallback` | `selectedStrategy=<none>` with selected or planned path identifying provider-neutral full-rebuild maintenance. | Full rebuild through the default provider-neutral `IDataVaultPitMaintenanceService` for repository-supported PIT declarations. | Contract row only until sibling provider-neutral benchmark artifacts land; not timing evidence. |
| PostgreSQL PIT full rebuild | `pit-full-rebuild-maintenance` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` compared with `dvault-adddvault-fallback` | `postgres-optimized-dvault` | `PostgresDataVaultPitMaintenanceStrategy` when selected; otherwise provider-neutral fallback with bounded `DataVaultPitMaintenanceStrategyFallbackCauseKind` causes. | Clean Npgsql-backed full rebuilds of ordinary hub-parent PITs, shared-driving-key multi-active hub-parent PITs, and link-parent non-multi-active PITs. | Contract row only until PostgreSQL benchmark artifacts land; not timing evidence. |
| SQL Server PIT full rebuild | `pit-full-rebuild-maintenance` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` compared with `dvault-adddvault-fallback` | `sqlserver-optimized-dvault` | `SqlServerDataVaultPitMaintenanceService` when selected; otherwise provider-neutral fallback with bounded `SqlServerPitMaintenanceFallbackCauseKind` causes. | Clean ordinary hub-parent PIT full rebuilds only; maintain-parents, multi-active PITs, link-parent PITs, dirty contexts, provider mismatch, and no-savepoint caller transactions are fallback or non-goal cases. | Contract row only until SQL Server benchmark artifacts land; not timing evidence. |
| MySQL PIT full rebuild | `pit-full-rebuild-maintenance` | MySQL external provider | `dvault-adddvaultmysql-optimized` compared with `dvault-adddvault-fallback` | `mysql-optimized-dvault` | `MySqlDataVaultPitMaintenanceStrategy` when selected; otherwise provider-neutral fallback with bounded `DataVaultPitMaintenanceStrategyFallbackCauseKind` causes. | Clean ordinary hub-parent PIT full rebuilds on the official `MySql.EntityFrameworkCore` provider only; Pomelo, maintain-parents, multi-active PITs, link-parent PITs, dirty contexts, provider mismatch, incomplete maintenance-shape evidence, and caller transactions without verified savepoints are fallback or non-goal cases. | Source/test-backed contract row only until MySQL benchmark artifacts land; not timing evidence. |

The required artifact triplet for each completed row is `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` under the sibling benchmark ticket's preserved artifact label. The row must preserve scenario identity, provider, baseline/comparator identity, selected strategy or provider-neutral fallback posture, bounded fallback causes when present, dataset and shape context, iteration and warmup counts, provider filter, optional-provider configuration status, runtime and hardware context, timing/allocation metrics, and persisted outcome. A row that lacks the artifact triplet, run context, or bounded execution-detail tokens remains guidance only even when diagnostics prove a strategy candidate.

## Deferred Bridge Maintenance Push-Down

Bridge maintenance push-down is intentionally not a completed provider-evidence row in this matrix. The completed `bridge-traversal-read` rows prove read-strategy selection over explicitly maintained bridge rows; they do not prove provider-specific execution of `RebuildBridgeAsync(...)` or `MaintainBridgeAsync(...)`.

The current evidence supports a defer recommendation. PostgreSQL has a provider-specific PIT maintenance strategy for clean full rebuilds of ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs. SQL Server has a narrower PIT maintenance service replacement for clean ordinary hub-parent full rebuilds, with maintain-parents, multi-active, link-parent, mismatched-provider, dirty-context, and no-savepoint requests falling back to provider-neutral maintenance. MySQL has a provider-specific PIT maintenance strategy for clean official-provider ordinary hub-parent full rebuilds only, with Pomelo, maintain-parents, multi-active, link-parent, mismatched-provider, dirty-context, incomplete-shape, and unverified-savepoint requests falling back to provider-neutral maintenance. Bridge maintenance remains the provider-neutral service surface. A later bridge-maintenance row can be added only after the repository carries a core/provider bridge-maintenance seam, bridge-specific gate and fallback diagnostics, parity tests for the existing many-to-many and hierarchy maintenance semantics, and a preserved provider-configured benchmark artifact triplet. Until then, downstream work should keep bridge push-down out of implementation scope and treat `06FE4RKGASKV6F7DF0RD1WTAV4` as the immediate documentation follow-on.

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

PIT full-rebuild maintenance claims must stop or fall back when the required artifact row is skipped, the provider connection string is missing, the provider package is not registered, diagnostics do not select the expected PIT maintenance strategy, or any of these bounded fallback causes apply:

- `ProviderNameMismatch`
- `UnknownOrUnregisteredProviderName`
- `NoProviderSpecificStrategyRegistered`
- `DirtyDbContext`
- `UnsupportedPitShape`
- `IncompleteMaintenanceShapeEvidence`
- `StrategyDeclined`
- `CurrentTransactionSavepointUnavailable`
- `UnsupportedPitParent`
- `MultiActivePitUnsupported`
- `MaintainParentsUnsupported`

`MaintainParentsUnsupported` identifies the SQL Server and MySQL parent-maintenance fallback path and is not a completed `pit-full-rebuild-maintenance` timing row. PIT `MaintainParentsAsync(...)`, bridge maintenance push-down, Oracle PIT maintenance, DB2 PIT maintenance implementation, and provider expansion beyond PostgreSQL, SQL Server, and official-provider MySQL stay outside this matrix slice.

Provider-specific threshold facts remain part of the stop conditions: PostgreSQL staged COPY starts at 60-plus operations, SQL Server native bulk starts at 100-plus operations, mixed SQL Server hub/link batches start at 900-plus operations, SQL Server accepts at most 500 satellite operations, MySQL native multi-row starts at 50-plus operations for eligible batches, MySQL staged bulk starts at 100-plus operations but large hub/link-containing batches above 303 operations return to provider-neutral fallback, Oracle direct optimized batching starts at 50-plus operations and accepts at most 10000 satellite operations, and DB2 has no staged bulk, provider-native chunk execution, dirty-context save support, or unsupported latest-satellite shape support.

## Citation Examples

Use this form in follow-up tickets:

```text
matrix row: scenario=provider-native-bulk-ingestion; provider=PostgreSQL external provider; baseline=dvault-adddvaultpostgres-optimized; posture=skipped-placeholder
claim: root quick-baseline row identity only when PostgreSQL is unconfigured; measured closure claims must cite artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/postgres-podman-live/benchmark-summary.*
```

```text
matrix row: scenario=latest-satellite-read; provider=SQLite local temporary files; baseline=dvault-adddvaultsqlite-optimized; posture=completed-timing
claim: measured SQLite latest-satellite read evidence, valid only with the root benchmark triplet and run context
```

```text
matrix row: scenario=latest-satellite-read; provider=MySQL external provider; baseline=dvault-adddvaultmysql-optimized; posture=completed-timing
claim: measured MySQL optimized latest-satellite read evidence, valid only with artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/mysql-live/benchmark-summary.* and its run context
```

```text
matrix row: scenario=pit-as-of-read; provider=Oracle external provider; baseline=dvault-adddvaultoracle-optimized; posture=completed-timing
claim: measured Oracle PIT read evidence, valid only with artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/oracle-lob-prefetch/benchmark-summary.* and its run context
```

```text
matrix row: scenario=pit-as-of-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; posture=completed-timing
claim: measured DB2 PIT read evidence, valid only with artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/db2-rowcap-1000/benchmark-summary.* and its run context
```

```text
matrix row: scenario=pit-as-of-read; provider=PostgreSQL external provider; baseline=dvault-adddvaultpostgres-optimized; posture=completed-timing
claim: measured PostgreSQL PIT read evidence, valid only with artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/postgres-podman-live/benchmark-summary.* and its run context
```

```text
matrix row: scenario=pit-full-rebuild-maintenance; provider=PostgreSQL external provider; baseline=dvault-adddvaultpostgres-optimized; posture=completed-timing
claim: measured PostgreSQL PIT full-rebuild maintenance evidence only after a sibling benchmark ticket preserves a completed artifact triplet with maintenanceScope=FullRebuild, selectedStrategy=PostgresDataVaultPitMaintenanceStrategy, bounded fallbackCauses, and run context; do not cite pit-as-of-read rows for this claim
```
