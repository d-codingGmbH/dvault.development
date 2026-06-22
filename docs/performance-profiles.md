# Performance Profiles

Status: v0.43.0 binary-adoption, allocation-hotspot, provider evidence, and tuning documentation baseline with carried-forward v0.42.0 provider gates, v0.32.0 provider-threshold evidence, DB2 hotspot evidence, and v0.31.0 decision-tree contract

This guide is the detailed performance-profile reference for the current DVault performance-guidance baseline. It carries forward the v0.31.0 adopter decision tree, the v0.32.0 provider-threshold evidence bundles for PostgreSQL, SQL Server, MySQL, and Oracle, the checked-in DB2 hotspot evidence bundle, the review-only provider-specific SQL artifact gate, the v0.42.0 provider evidence promotion rules and tuning gates, the current provider-read parity evidence posture, the v0.43.0 provider binary-vs-hex hash-key matrix, and the v0.43.0 allocation-hotspot evidence. It does not create automatic routing, absolute performance guarantees, provider service-level objectives, dashboards, hosted observability, database provisioning, scheduler templates, credential-management guidance, automatic hash-key migration, automatic PIT or bridge maintenance, raw SQL or physical-plan promises, deployable provider-specific SQL payload generation, runtime artifact dispatch, package-publication approval, or provider support outside the evidence-linked rows. The coordinated release record for the current binary-adoption, allocation, provider-bulk, provider-read parity, evidence, and tuning baseline is [DVault v0.43.0 Release Notes](releases/v0.43.0.md). The current package and optional privacy baseline is [DVault v0.44.0 Release Notes](releases/v0.44.0.md). Earlier release notes remain historical feature-introduction records.

## Evidence Baseline

Use the root benchmark artifact triplet as the quick local SQLite and skipped-provider baseline for the row names and timing values in this guide:

- [benchmark-summary.md](../benchmark-summary.md)
- [benchmark-summary.csv](../benchmark-summary.csv)
- [benchmark-summary.json](../benchmark-summary.json)

Use the DB2 hotspot artifact triplet as the provider-configured completed-timing bundle for DB2 clean-context optimized save plus supported latest-satellite, PIT, and bridge reads:

- [benchmark-summary.md](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.md)
- [benchmark-summary.csv](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.csv)
- [benchmark-summary.json](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.json)

The v0.42.0 release validation also includes a one-iteration Windows-host to Podman-DB2 run over the same scoped lanes. Treat it as confirming evidence for the current local DB2 setup, not as a wider DB2 performance promise:

- [benchmark-summary.md](../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260621/benchmark-summary.md)
- [benchmark-summary.csv](../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260621/benchmark-summary.csv)
- [benchmark-summary.json](../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260621/benchmark-summary.json)

Use the provider binary-vs-hex hash-key matrix as a scoped provider-configured storage-profile evidence bundle. It ran one iteration and no warmup on Windows/.NET 10.0.9 with provider filter `all`, four hash-key variants, completed PostgreSQL, MySQL, Oracle, and DB2 provider discovery, a skipped SQL Server lane for the local TLS/runtime setup, and failed binary rows for several external-provider save/read shapes:

- [benchmark-summary.md](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.md)
- [benchmark-summary.csv](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.csv)
- [benchmark-summary.json](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.json)
- [hash-key-footprint.md](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.md)
- [hash-key-footprint.csv](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.csv)
- [hash-key-footprint.json](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.json)

The repository-root footprint entry points expose the same footprint rows for quick local validation:

- [hash-key-footprint.md](../hash-key-footprint.md)
- [hash-key-footprint.csv](../hash-key-footprint.csv)
- [hash-key-footprint.json](../hash-key-footprint.json)

Use the allocation hotspot artifacts as bounded DVault-owned allocation evidence. The first bundle records the hotspot baseline, and the comparative before/after bundle is the primary refreshed evidence source:

- [allocation-hotspots.md](../artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/allocation-hotspots.md)
- [allocation-hotspots.csv](../artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/allocation-hotspots.csv)
- [allocation-hotspots.json](../artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/allocation-hotspots.json)
- [before/allocation-hotspots.md](../artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-20260621/before/allocation-hotspots.md)
- [before/benchmark-summary.md](../artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-20260621/before/benchmark-summary.md)
- [after/allocation-hotspots.md](../artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-20260621/after/allocation-hotspots.md)
- [after/benchmark-summary.md](../artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-20260621/after/benchmark-summary.md)

Use [Provider Optimization Evidence Matrix](plans/provider-optimization-evidence-matrix.md) as the canonical lookup surface for provider optimization row identity, evidence posture, artifact source, stop/fallback conditions, and the `dvault.provider-evidence.v1` manifest row contract. Cite evidence rows by `scenario`, `provider`, `baseline`, and `posture` instead of copying raw benchmark tables or treating planning text as measured results. The matrix distinguishes completed timing evidence, skipped optional-provider placeholders, diagnostics-only evidence, smoke-only evidence, and hash-key storage-footprint evidence so follow-up tickets do not cite those postures interchangeably.

Use [Provider Optimization Gap Matrix](plans/provider-optimization-gap-matrix.md) as the canonical follow-up recommendation surface. Its P0-P3 rows are planning and closure entries: PostgreSQL `latest-satellite-read` is a closed capability gap because the checked-in guidance row records `PostgresDataVaultReadStrategy` for supported hub-parent, non-multi-active latest-satellite requests, but that row remains a skipped placeholder until a provider-configured timing lane runs; MySQL `latest-satellite-read` is a closed scoped timing baseline when cited through the ticket `06FE4QQ9VF7B74E60CXEHSS5XW` provider-configured bundle, without claiming a provider-neutral fallback improvement comparator; SQL Server and Oracle `latest-satellite-read` rows remain evidence-gap recommendations until a dedicated latest-satellite evidence ticket promotes a provider-configured benchmark triplet; PostgreSQL, SQL Server, MySQL, and Oracle `pit-as-of-read` and `bridge-traversal-read` are closed evidence rows when cited through the checked-in v0.32.0 smoke-read bundle; SQL Server `provider-native-bulk-ingestion` is closed when cited through the configured 2026-06-20 SQL Server bulk-threshold bundle; and DB2 `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` are closed evidence rows when cited through the checked-in DB2 hotspot bundle. PostgreSQL, MySQL, and Oracle `provider-native-bulk-ingestion` rows remain evidence-gap recommendations until provider-configured benchmark triplets exist. The root quick baseline still preserves skipped PostgreSQL, SQL Server, MySQL, Oracle, and DB2 placeholders when their connection strings are unset. Gap-matrix recommendations do not promote skipped placeholders, diagnostics-only rows, smoke-only rows, storage-footprint rows, or incidental out-of-scope rows into completed provider timing evidence.

## v0.43.0 Evidence And Tuning Boundary

The v0.43.0 performance documentation baseline separates measured facts from strategy availability, skipped guidance rows, diagnostics-only posture, smoke-only posture, storage-footprint posture, allocation-hotspot posture, provider-specific starting gates, and forward-looking recommendations:

| Surface | Use it for | Do not use it for |
| --- | --- | --- |
| Root benchmark artifact triplet | Local SQLite completed timing rows, skipped optional-provider row identity, run context, and deterministic execution details. | Completed PostgreSQL, SQL Server, MySQL, Oracle, or DB2 timing claims when the root row is skipped. |
| Provider Optimization Evidence Matrix | Canonical row identity, posture semantics, artifact source, and claim boundaries for provider evidence rows. | Backlog priority, implementation ordering, or unsupported promotion of skipped, diagnostics-only, smoke-only, or storage-footprint rows. |
| Provider Optimization Gap Matrix | Follow-up recommendations for capability gaps and evidence gaps, ordered by matrix priority. | Measured timing evidence, provider capability expansion, or release promises. |
| v0.32 provider benchmark bundles and ticket-specific provider-threshold bundles | Completed external-provider timing where the linked bundle recorded completed rows with preserved run context, including PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge read rows in the smoke-read bundle, the MySQL latest-satellite row in the ticket `06FE4QQ9VF7B74E60CXEHSS5XW` bundle, and SQL Server native-bulk save rows in the 2026-06-20 bulk-threshold bundle. | Universal performance guarantees, root skipped-placeholder promotion, claims for unconfigured providers, MySQL latest-satellite improvement claims without a provider-neutral comparator, or promotion of incidental rows outside the accepted ticket scope. |
| DB2 hotspot benchmark bundle | Completed DB2 timing for the provider-neutral save comparison row, clean-context optimized save selected by `Db2DataVaultSaveStrategy`, and supported latest-satellite/PIT/bridge reads selected by `Db2DataVaultReadStrategy`. | Staged DB2 bulk, provider-native chunk execution, DB2 live-schema timing evidence, dirty-context saves, unsupported latest-satellite shapes, stale PIT/bridge maintenance, incomplete read-shape evidence, or universal DB2 performance guarantees. |
| Provider binary-vs-hex hash-key matrix | Scoped comparison of `sha256-v1` and `sha256-128-v1` in `HexString` and `Binary` storage profiles across SQLite plus configured external providers, including completed, skipped, and failed rows. | A blanket binary-storage recommendation, automatic migration guidance, or provider-specific timing claims for failed/skipped rows. |
| Allocation hotspot artifacts | DVault-owned allocation ranking for save preparation, latest-hash-diff replay filtering, stable-hash canonicalization, and digest generation with before/after context. | Caller-owned `HashDiff` generation, database setup, database write timing, provider timing claims, or universal allocation promises. |

For example, `scenario=latest-satellite-read; provider=SQLite local temporary files; baseline=dvault-adddvaultsqlite-optimized; posture=completed-timing` is a measured SQLite timing row when cited with the root triplet and run context. `scenario=latest-satellite-read; provider=MySQL external provider; baseline=dvault-adddvaultmysql-optimized; posture=completed-timing` is measured MySQL optimized latest-satellite timing only when cited with `artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.*` and its run context; it is not a provider-neutral fallback improvement comparator by itself. `scenario=pit-as-of-read; provider=PostgreSQL external provider; baseline=dvault-adddvaultpostgres-optimized; posture=completed-timing`, `scenario=pit-as-of-read; provider=SQL Server external provider; baseline=dvault-adddvaultsqlserver-optimized; posture=completed-timing`, `scenario=pit-as-of-read; provider=MySQL external provider; baseline=dvault-adddvaultmysql-optimized; posture=completed-timing`, and `scenario=pit-as-of-read; provider=Oracle external provider; baseline=dvault-adddvaultoracle-optimized; posture=completed-timing` are measured PIT evidence only when cited with the v0.32.0 smoke-read bundle. `scenario=provider-native-bulk-ingestion; provider=SQL Server external provider; baseline=dvault-adddvaultsqlserver-optimized; posture=completed-timing` is measured SQL Server save-threshold evidence only when cited with the 2026-06-20 bulk-threshold bundle. `scenario=provider-native-bulk-ingestion; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; posture=completed-timing`, `scenario=latest-satellite-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; posture=completed-timing`, `scenario=pit-as-of-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; posture=completed-timing`, and `scenario=bridge-traversal-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; posture=completed-timing` are measured DB2 evidence only when cited with the DB2 hotspot bundle. By contrast, PostgreSQL, SQL Server, and Oracle `latest-satellite-read` rows in the root quick triplet preserve strategy registration or planned-strategy facts only; they are not completed external-provider latest-satellite timing rows until a provider-configured benchmark bundle completes, and the 2026-06-20 SQL Server bulk-threshold triplet does not promote its incidental latest-satellite row. Likewise, `scenario=provider-native-bulk-ingestion; provider=PostgreSQL external provider; baseline=dvault-adddvaultpostgres-optimized; posture=skipped-placeholder` preserves row identity and planned path facts only until a provider-configured benchmark bundle completes.

The DB2 boundary remains narrower than the other provider follow-ups. DB2 completed timing is limited to the DB2 hotspot bundle's clean-context optimized save and supported latest-satellite/PIT/bridge rows; staged DB2 bulk, provider-native chunk execution, dirty-context saves, unsupported latest-satellite shapes, stale PIT/bridge maintenance, incomplete read-shape evidence, and DB2 live-schema timing evidence remain outside the current evidence baseline.

The v0.32.0 provider-threshold evidence extends that root triplet with checked-in benchmark bundles under `artifacts/benchmarks/...`:

- [v0.32.0 all-provider scale baseline](../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607/benchmark-summary.md)
- [SQL Server threshold decision](../artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md)
- [SQL Server bulk-threshold refinement](../sqlserver-threshold-decision.md), backed by the configured 2026-06-20 SQL Server bulk-threshold triplet
- [Oracle high-volume threshold evidence](../artifacts/benchmarks/v0.32.0-06F9XD2TGEYEG6S0AK86YF295M-oracle-high-volume-threshold-20260607/benchmark-summary.md)
- [PostgreSQL and MySQL small-batch evidence](../artifacts/benchmarks/v0.32.0-06F9XD33MNNVHHW232TC7T1CN8-scale-evidence-20260608/README.md)
- [v0.32.0 smoke read baseline](../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md), including completed PostgreSQL, SQL Server, MySQL, and Oracle PIT and bridge rows for their provider-optimized baselines
- [MySQL latest-satellite ticket baseline](../artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.md), including the completed optimized latest-satellite row that selected `MySqlDataVaultReadStrategy`
- [DB2 hotspot evidence](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.md), including completed DB2 clean-context save, latest-satellite, PIT, and bridge rows for the DB2 optimized baseline
- [DB2 host-to-Podman validation](../artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-20260621/benchmark-summary.md), confirming the same scoped DB2 lanes in the v0.42 release validation run
- [Provider binary-vs-hex hash-key matrix](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.md), preserving completed provider rows, skipped SQL Server setup rows, and failed provider binary-storage rows in one artifact context
- [Allocation hotspot baseline](../artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/allocation-hotspots.md) and [refreshed before/after allocation hotspot bundle](../artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-20260621/after/allocation-hotspots.md), preserving the measured DVault-owned allocation boundary

The v0.32.0 smoke read baseline is the authoritative completed-timing bundle for PostgreSQL, SQL Server, MySQL, and Oracle `pit-as-of-read` and `bridge-traversal-read` rows that selected their provider read strategies. MySQL latest-satellite timing is separate from this PIT/bridge closure and is scoped to the ticket `06FE4QQ9VF7B74E60CXEHSS5XW` bundle. The 2026-06-20 SQL Server bulk-threshold triplet is cited for save-threshold closure only; its incidental latest-satellite row does not close `P0.02`. Skipped root quick-baseline rows remain placeholders rather than timing claims.

The DB2 hotspot evidence bundle is the authoritative completed-timing bundle for DB2 `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` rows that selected the supported DB2 provider strategies. It also keeps the provider-neutral save comparison row in the same run context.

The provider binary-vs-hex bundle is the authoritative checked-in provider-configured evidence for hash-key variant participation. Treat completed rows as measured only with that bundle and its one-iteration run context. Treat PostgreSQL bytea/text operator failures, MySQL and DB2 truncation failures, Oracle reexecution/null-reference failures, and the skipped SQL Server local TLS lane as caveats and follow-up evidence, not as successful binary timing claims.

The allocation hotspot bundle is the authoritative checked-in evidence for current DVault-owned allocation ranking. The bounded optimization order is `DefaultDataVaultSaveService.AddSatellitesAsync`, then `FilterSatellitePlansAsync` and `LoadLatestSatelliteHashDiffsAsync`, then `DefaultStableHashNormalizer.NormalizeFields`, then `BuiltInStableHashService.ComputeHash`. Keep caller-owned satellite `HashDiff` generation, SQLite database setup, seeding, verification, cleanup, and database write execution outside the ranked boundary; `DbContext.SaveChangesAsync` is sampled only as context.

The benchmark runner and artifact rules are documented in [DVault Benchmarks](../benchmarks/DCoding.Data.DVault.Benchmarks/README.md) and [Performance Evidence And Benchmark Artifact Contract](plans/performance-evidence-benchmark-artifact-contract.md). Keep those linked artifacts with any copied result so the timing numbers stay attached to the run context.

The checked-in root quick baseline used:

- 3 iterations and 1 warmup iteration.
- Load timestamp storage `ProviderDefault`.
- Provider filter `all`.
- Debian GNU/Linux 13 (trixie), X64 OS and process architecture, 32 processors.
- .NET 10.0.8.
- Required provider `SQLite local temporary files`.
- Optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows emitted as `executionStatus=skipped` because `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_MYSQL_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, and `DVAULT_TEST_DB2_CONNECTION_STRING` were unset.

Treat all millisecond values below as observations from their linked run only. The v0.32 bundles record local Podman evidence where their rows are completed, but they are not universal timing promises. Rerun the benchmarks when provider, hardware, runtime, load-timestamp storage, iteration count, warmup count, dataset size, request shape, or provider configuration changes.

## Benchmark Verifier And Redaction Boundary

The repository evidence is the artifact triplet plus verifier coverage, not copied raw benchmark tables in adopter docs. The verifier expectations keep these facts bounded and reusable:

- `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` are emitted from one run and contain the same result rows.
- The checked-in run keeps the four profile categories visible: `SmallAppLocalVault`, `MediumChunkedIngestion`, `StagedProviderIngestion`, and `ReadModelHeavy`.
- Provider guidance rows stay visible for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 even when their external provider lanes are skipped, including provider-native ingestion rows and read-model rows.
- Provider-read evidence separates completed SQLite latest-satellite, PIT, and bridge timing rows, completed PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge rows in the v0.32.0 smoke-read bundle, the completed MySQL latest-satellite row in the ticket `06FE4QQ9VF7B74E60CXEHSS5XW` bundle, and completed DB2 latest-satellite/PIT/bridge rows in the DB2 hotspot bundle from root optional-provider read guidance rows that may remain skipped when connection strings are unset.
- Regression-budget guidance stays attached to the shared artifact contract instead of being inferred from one timing row.

Use redacted verifier summaries when referencing this evidence in tickets or release notes:

```text
benchmark artifact verifier: passed
artifact triplet: benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json
profile categories: SmallAppLocalVault, MediumChunkedIngestion, StagedProviderIngestion, ReadModelHeavy
required provider: SQLite local temporary files
optional provider rows: preserved as skipped when connection-string environment variables are unset
raw timings: see checked-in artifact triplet
```

## v0.31.0 Performance Decision-Tree Contract

This section is the authoritative choice order for adopter performance decisions. The runtime profile sections below preserve the four existing profile families and benchmark observations; they are supporting detail, not a second decision model. The contract is request-bound and evidence-bound: DVault keeps explicit save/read service boundaries, deterministic diagnostics, opt-in metrics, listener-driven tracing, and caller-owned PIT or bridge maintenance instead of adding automatic strategy routing.

Use the existing detail surfaces when a branch needs more than choice order:

- Benchmark evidence: [benchmark-summary.md](../benchmark-summary.md), [benchmark-summary.csv](../benchmark-summary.csv), [benchmark-summary.json](../benchmark-summary.json), [DVault Benchmarks](../benchmarks/DCoding.Data.DVault.Benchmarks/README.md), and [Performance Evidence And Benchmark Artifact Contract](plans/performance-evidence-benchmark-artifact-contract.md).
- Canonical provider row lookup: [Provider Optimization Evidence Matrix](plans/provider-optimization-evidence-matrix.md).
- Write boundary: [DVault V1 Explicit Save Service](architecture/dvault-v1-explicit-save-service.md).
- Read diagnostics and `ReadShape`: [DVault V2 Redacted Read-Plan Explain Contract](architecture/dvault-v2-redacted-read-plan-explain-contract.md).
- PIT and bridge maintenance and read boundary: [DVault V1 PIT And Bridge Boundary](architecture/dvault-v1-pit-bridge-boundary.md).
- Typed helper generation: [DVault V1 Typed PIT And Bridge Helper Contract](architecture/dvault-v1-typed-pit-bridge-helper-contract.md).
- Activity tracing and metrics relationship: [DVault V1 Activity Tracing Contract](architecture/dvault-v1-activity-tracing-contract.md).

### Ordered Write Path

Answer these questions in order before selecting a write profile:

1. Is the workload using the public DVault write boundary?

   Use `IDataVaultSaveService` for ordinary single, ordered bulk, bounded chunked, and async chunk-source saves. Stop if the proposal requires `SaveChanges` interception, provider dispatch outside the service, automatic strategy routing, stored-procedure invocation, background ingestion, CDC ingestion, scheduler behavior, or provider-specific SQL generation. Those are outside this decision tree.

2. Is the complete ordered request set already materialized?

   Choose `DataVaultBulkSaveRequest` as the baseline when the loader naturally owns the complete ordered batch and memory pressure does not require chunking. This remains the compatibility starting point for ordinary materialized explicit saves.

3. Does the loader need bounded memory while already owning materialized bounded chunks?

   Choose `DataVaultChunkedSaveRequest` when the loader can preserve explicit load timestamps, record sources, chunk order, request order inside each chunk, and caller-owned transaction behavior. Keep empty chunks as no-ops. Fall back to the materialized bulk branch when memory is acceptable, chunk overhead dominates, or telemetry shows excessive chunk count, retained-state fallback, unsupported shapes, cancellation pressure, or transaction boundaries that do not match the loader.

4. Are the bounded chunks or source rows already asynchronous?

   Choose `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` only when the caller already has an async chunk source that should be enumerated once in yielded order. Use the async helper methods only when they map an async source into the same bounded explicit save boundary. Do not claim provider-native async writes, provider-native chunk execution, background continuation, or alternate ordering semantics from this branch.

5. Is the workload an eligible clean-context provider-specific ordered bulk batch?

   Keep the same `IDataVaultSaveService` boundary, register the matching provider extension, and require `IDataVaultDiagnosticsService` evidence for the exact request before claiming provider-native behavior. PostgreSQL staged COPY and MySQL staged bulk are the documented staged-provider lanes; SQL Server remains native-bulk wording; Oracle remains direct optimized batching until measured evidence selects a staged Oracle lane; DB2 provides a clean-context optimized save path without staged bulk or provider-native chunk execution. Treat skipped optional-provider rows, missing connection strings for a new run, provider-name mismatch, dirty contexts, unsupported multi-active satellite batches, declined strategy gates, threshold failures, or missing local benchmark evidence as finite stop conditions for measured provider-specific performance claims. The fallback path is the provider-neutral writer under the same explicit service.

### Ordered Read Path

Answer these questions in order before selecting a read profile:

1. Is the workload using the public DVault read boundary?

   Use `IDataVaultReadService` for latest/current/as-of satellite, PIT as-of, and bridge traversal reads. Stop if the proposal requires raw SQL inspection, provider physical-plan promises, automatic index creation, automatic PIT or bridge maintenance, graph API inference, background refresh, or provider-specific physical tuning from DVault.

2. Is the request a latest/current or as-of satellite read?

   Start with the provider-neutral read pipeline and inspect `IDataVaultReadDiagnosticsService` output for request-bound `ReadShape` and read-strategy evidence. SQLite has completed local timing for optimized latest-satellite reads, MySQL has a completed provider-configured optimized latest-satellite timing row for the current window-function path, DB2 has completed provider-configured timing for the supported hub-parent, non-multi-active shape in the DB2 hotspot bundle, and PostgreSQL, SQL Server, and Oracle have diagnostics-gated latest-satellite strategy selection for the same supported shape. Unsupported satellite parents, multi-active unsupported shapes, incomplete `ReadShape` evidence, unknown providers, unregistered providers, and declined strategies must remain provider-neutral fallback unless a later ticket adds new benchmark-backed evidence. Do not claim a MySQL latest-satellite improvement over provider-neutral fallback without a matching comparator row.

3. Is the request a PIT as-of read?

   Confirm the PIT table is explicitly maintained before the read path depends on it. PIT-backed reads consume already-maintained rows; they do not run `IDataVaultPitMaintenanceService`, schedule refresh, or correct stale rows implicitly. Use request-bound `ReadShape` evidence and provider diagnostics before claiming an optimized strategy. Unsupported PIT shapes, unsupported providers, incomplete `ReadShape` evidence, or stale PIT maintenance evidence are explicit fallback or stop conditions.

4. Is the request a bridge traversal read?

   Confirm the bridge table is explicitly maintained before the read path depends on it. Bridge reads consume already-maintained rows; they do not run `IDataVaultBridgeMaintenanceService`, shrink deleted hierarchy paths, schedule refresh, or infer traversal APIs implicitly. Use request-bound `ReadShape` evidence and provider diagnostics before claiming an optimized strategy. Unsupported bridge shapes, unsupported providers, incomplete `ReadShape` evidence, stale bridge maintenance evidence, or destructive hierarchy changes that require full rebuild behavior are explicit fallback or stop conditions.

5. Is the provider claim PIT or bridge optimization?

   SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 are repository-proven diagnostics-gated PIT/bridge read-strategy candidate paths. PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge have completed provider-configured timing in the v0.32.0 smoke-read bundle; DB2 PIT/bridge has completed provider-configured timing in the DB2 hotspot bundle. Other external-provider PIT/bridge timing claims need their own completed provider-configured artifact lane. Unsupported providers, provider-name mismatch, missing provider registration, strategy decline, unsupported shapes, incomplete `ReadShape` evidence, and stale read-model maintenance keep the provider-neutral read pipelines.

### Design-Time Typed Helper Branch

Typed read-model helpers are a separate design-time branch after the runtime read shape is understood. They are not a fifth runtime performance profile and they do not choose providers, dispatch reads, generate SQL, perform PIT or bridge maintenance, schedule refresh, compile dynamic read requests, or widen `IDataVaultReadService` semantics.

Use generated helpers only when all of these are true:

1. The consuming project opts in with `DVaultGenerateTypedReadModels=true`.
2. The analyzer receives exactly one authoritative `dvault.support-bundle.v1` additional file.
3. Reviewed request-bound `ReadShape` evidence is present for any PIT or bridge helper the project wants to emit.
4. Representative diagnostics were supplied by application code through `DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics`; the reusable command runner did not invent representative PIT or bridge requests.

Missing, malformed, incompatible-version, non-authoritative, ambiguous, raw `dvault.model.v1`, or residual model-first input is a support-bundle boundary failure. A stale `DVaultTypedReadModelMetadataSourceFingerprint` is a fingerprint failure. Unsupported PIT or bridge evidence skips only the affected helper while other supported satellite, PIT, or bridge helpers can still generate from the same reviewed bundle.

### Diagnostics Evidence And Observability Gate

Choose or promote a branch only after the corresponding evidence surface is reviewed:

- Write selection uses `IDataVaultDiagnosticsService` for request-bound save-strategy status, provider name, selected strategy name, candidate facts, operation counts, gate requirements, and finite fallback causes.
- Read selection uses `IDataVaultReadDiagnosticsService` plus request-bound `ReadShape` for read-strategy status, selected strategy name, fallback causes, read-shape kind, provider facts, translated table identity, filter columns, and deterministic row-selection rules.
- Measured performance claims use the benchmark artifact triplet and rerun context, not copied timing values without provenance.
- Metrics are opt-in through `AddDVaultTelemetry()` and bounded `DataVaultSaveTelemetrySummary`/`DataVaultReadTelemetrySummary` values.
- Activity tracing is listener-driven through the `DCoding.Data.DVault` ActivitySource and remains a sibling observability surface, not a telemetry prerequisite.
- PIT and bridge read decisions include explicit maintenance freshness evidence before optimized read claims are made.
- Typed-helper decisions include the reviewed support bundle and request-bound `ReadShape` evidence, not runtime metadata inference.

Diagnostics, telemetry, tracing, support bundles, and adopter records must preserve the documented redaction boundaries. Do not include raw business keys, hash-key values, payload values, record sources, SQL text, query plans, provider messages, exception messages, stack traces, connection strings, credentials, support-bundle content, or full diagnostic text in performance guidance.

### Stop And Fallback Handling

Stop before documenting a branch as selected when the required evidence is missing, stale, skipped, unsupported, or outside the current public boundary. Use provider-neutral save or read behavior as the bounded fallback when diagnostics do not select an optimized strategy. Rerun local benchmarks and preserve the artifact triplet when provider, hardware, runtime, load-timestamp storage, iteration count, warmup count, dataset size, request shape, provider configuration, indexes, maintenance cadence, or transaction policy changes.

## Runtime Profile Summary

Use this table as a compact summary after applying the v0.31.0 decision-tree contract above.

| Profile | Start here when | Main starting point | Primary stop condition |
| --- | --- | --- | --- |
| Small app-local vault | The application writes ordinary hub, link, and satellite rows and needs a local SQLite or app-local proof first. | Register `AddDVault()` first, then add `AddDVaultSqlite()` only for SQLite deployments that want the provider package path. | Save/read diagnostics show provider fallback, a non-SQLite provider is selected without matching provider evidence, or the workload grows beyond the root customer-profile rows. |
| Medium chunked ingestion | The loader has an ordered source stream and must bound memory without changing load timestamps, record sources, or request order. | Keep `DataVaultBulkSaveRequest` for materialized batches; use `DataVaultChunkedSaveRequest` for already-bounded ordered loaders, starting around chunk size 10. Use the `IAsyncEnumerable<DataVaultSaveChunk>` overload or async helper methods only when the producer is already asynchronous. | Materializing the batch is acceptable, chunk overhead dominates, or chunk count/retained-state telemetry no longer matches the local workload. |
| Staged provider ingestion | The application has clean provider-specific contexts and larger eligible ordered bulk batches for PostgreSQL, SQL Server, MySQL, Oracle, or DB2. | Register `AddDVault()` plus the matching provider extension and verify save-strategy diagnostics before claiming provider-native behavior. | The linked provider evidence bundle is missing or skipped for the claim, the context is dirty, native gates decline, or the provider-local run has not been collected. |
| Read-model heavy | The application repeatedly reads latest satellites, maintained PIT rows, or maintained bridge rows. | Use `IDataVaultReadService`; add `AddDVaultSqlite()` for optimized SQLite latest-satellite and PIT/bridge reads, or `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, or `AddDVaultDb2()` for diagnostics-gated latest-satellite/PIT/bridge candidates on their supported shapes. | PIT or bridge maintenance is stale, latest-satellite reads target an unsupported shape, PIT/bridge reads target an unsupported provider, or read-shape diagnostics report fallback, unsupported shape, or incomplete evidence. |

## Small App-Local Vault

### Workload Shape

Use this profile for small application-local vaults, early local proofs, and services that first need ordinary explicit saves to be correct and observable. The checked-in SQLite evidence covers:

- `customer-profile-history`: 1 customer with 2 profile states.
- `customer-profile-bulk-insert-only`: 100 customers with 1 profile state each.
- `customer-profile-bulk-history`: 100 customers with 10 profile states each.

### Registration Guidance

Start with `AddDVault()` as the safe baseline. It registers the explicit save/read services, stable hashing, provider-neutral fallback paths, and no telemetry listener by default. This is the right first registration when the provider path is not yet selected, when the app is still proving metadata and save ordering, or when provider-specific strategy diagnostics have not been reviewed.

For SQLite applications that install the SQLite provider package, add `AddDVaultSqlite()` beside the provider-neutral registration. In the checked-in run, the SQLite rows that use `AddDVaultSqlite()` selected `SqliteDataVaultSaveStrategy` for the write-history rows and `SqliteDataVaultReadStrategy` for supported read rows. Do not treat `AddDVaultSqlite()` as a universal performance requirement for non-SQLite providers.

### Starting Point

Keep the first production proof on the explicit `IDataVaultSaveService` boundary with caller-supplied load timestamp, record source, hub/link/satellite intent, and caller-owned transaction behavior. Move from provider-neutral `AddDVault()` to `AddDVaultSqlite()` only after the configured context really uses SQLite and diagnostics confirm the SQLite strategy is selected.

### Diagnostics And Telemetry

Inspect request-bound save-strategy diagnostics through `IDataVaultDiagnosticsService` before changing registration. The useful fields are strategy status, selected strategy name, candidate count, and finite fallback causes. For read-heavy app-local paths, inspect `IDataVaultReadDiagnosticsService` read strategy and read-shape diagnostics under the [DVault V2 Redacted Read-Plan Explain Contract](architecture/dvault-v2-redacted-read-plan-explain-contract.md). Register `AddDVaultTelemetry()` only when the application wants built-in `System.Diagnostics.Metrics` for explicit save/read attempts; metric listeners, exporters, dashboards, and alerting stay application-owned.

### Supporting Rows

All values in this section are from the evidence baseline above:

| Scenario | Baseline | Mean ms | Evidence posture |
| --- | --- | ---: | --- |
| `customer-profile-history` | `dvault-adddvault-fallback` | 3.379 | Provider-neutral explicit save through `AddDVault()`. |
| `customer-profile-history` | `dvault-adddvaultsqlite-optimized` | 2.229 | SQLite optimized write path selected `SqliteDataVaultSaveStrategy`. |
| `customer-profile-bulk-insert-only` | `dvault-adddvault-fallback` | 12.886 | Provider-neutral fallback for 100 satellite operations. |
| `customer-profile-bulk-insert-only` | `dvault-adddvaultsqlite-optimized` | 4.658 | SQLite optimized write path selected for the same logical profile rows. |
| `customer-profile-bulk-history` | `dvault-adddvault-fallback` | 65.148 | Provider-neutral fallback for 1000 satellite operations across 10 requests. |
| `customer-profile-bulk-history` | `dvault-adddvaultsqlite-optimized` | 33.372 | SQLite optimized write path selected for the same logical profile history shape. |

### Stop Conditions And Rerun Triggers

Stop treating the root SQLite rows as enough evidence when the application uses a non-SQLite database, provider diagnostics report fallback, the request shape includes unsupported multi-active or dirty-context behavior, the dataset size is materially larger, or runtime/hardware differs enough to change the tuning question. Rerun the benchmark triplet with the same provider filter and changed inputs documented before copying new timing values into adopter materials.

## Medium Chunked Ingestion

### Workload Shape

Use this profile when a loader receives an ordered source sequence and cannot or should not materialize the complete request set before saving. The checked-in `customer-profile-streaming-save` rows use 20 customers, 60 ordered explicit profile-save requests, 3 profile events per customer, and one unchanged replay. The rows compare a materialized `DataVaultBulkSaveRequest`, bounded `DataVaultChunkedSaveRequest` runs, and the v0.24 async source path over `IAsyncEnumerable<DataVaultSaveChunk>`.

The v0.24 async streaming contract uses the same `DataVaultSaveChunk` payload model through an additive `IAsyncEnumerable<DataVaultSaveChunk>` save overload. That overload is for callers whose chunk producer is already asynchronous and should be enumerated once in source order. The async source benchmark row is provider-neutral bounded streaming evidence for that source shape; it is not a provider-native ingestion strategy, a provider-native async write claim, or a different ordering contract.

### Registration Guidance

Use the same explicit `IDataVaultSaveService` boundary as ordinary saves. `DataVaultChunkedSaveRequest` is the materialized input shape for bounded provider-neutral chunking, and the async streaming overload is the async source shape over the same bounded chunks. Neither shape is a provider-native chunk execution claim. Register `AddDVault()` first. Provider package registrations can still optimize eligible ordinary ordered batches, but the current chunked evidence is scoped to the provider-neutral chunked path.

### Starting Point

Keep `DataVaultBulkSaveRequest` when the loader already has the complete ordered request set materialized. Choose `DataVaultChunkedSaveRequest` when the loader has already formed bounded chunks while preserving explicit load timestamps, record sources, request order, chunk order, and caller-owned transactions. Choose `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` when those same bounded chunks are produced by an async source and should be consumed sequentially once without pre-buffering the complete source. Choose `SaveAsync<TSource>(...)`, `SaveHubsAsync(...)`, `SaveLinksAsync(...)`, or `SaveOrdinaryHubSatellitesAsync(...)` when source rows are already asynchronous and should be mapped into bounded chunks through the existing explicit save boundary.

The checked-in run supports chunk size 10 as the first bounded chunk-size candidate for this shape. Chunk size 5 is useful as a lower-memory comparison, but it increased elapsed time in the checked-in run. Retune with local data when the source event count, payload size, transaction policy, or provider changes.

### Diagnostics And Telemetry

Use save-strategy diagnostics to confirm whether the provider-neutral writer or a provider strategy handled each chunk. With `AddDVaultTelemetry()`, inspect `DataVaultSaveTelemetrySummary` for operation kind, request count, chunk count, processed chunk count, retained-state high-water count, fallback causes, unsupported-shape classifications, duration, and transaction guidance. The async streaming overload reuses the existing chunked telemetry family, including processed-chunk counts and retained-state fallback diagnostics, and does not hide background continuation after completion, fault, or cancellation. Do not expect DVault to create scheduler, file-ingestion, CDC, database, or hosting infrastructure around chunked saves.

### Supporting Rows

All values in this section are from the evidence baseline above:

| Scenario | Baseline | Mean ms | Chunk detail |
| --- | --- | ---: | --- |
| `customer-profile-streaming-save` | `dvault-adddvault-fallback/materialized-explicit-bulk` | 5.774 | 60 ordered requests in one materialized bulk request. |
| `customer-profile-streaming-save` | `dvault-adddvault-fallback/chunked-save-bounded-10` | 11.636 | 6 chunks of 10, retained-state high-water 20. |
| `customer-profile-streaming-save` | `dvault-adddvault-fallback/async-source-bounded-10` | 11.775 | 6 async-yielded chunks of 10, retained-state high-water 20, source shape `IAsyncEnumerable<DataVaultSaveChunk>`. |
| `customer-profile-streaming-save` | `dvault-adddvault-fallback/chunked-save-bounded-5` | 21.088 | 12 chunks of 5, retained-state high-water 20. |

### Stop Conditions And Rerun Triggers

Prefer the materialized bulk request when memory use is acceptable and the application can naturally build the ordered batch. Stop using chunk size 10 as the default when telemetry shows excessive chunk count, retained-state fallback, unsupported shapes, cancellation pressure, or transaction boundaries that do not match the loader. Rerun local benchmarks for the chunk sizes the application can actually use before documenting a new operational starting point.

## Staged Provider Ingestion

### Workload Shape

Use this profile for provider-eligible ordered bulk ingestion on PostgreSQL, SQL Server, MySQL, Oracle, or DB2. The root `provider-native-bulk-ingestion` rows describe 20 order-product pairs, 20 order-product links, and 3 ordered fulfillment satellite operations, including one unchanged replay, in a clean-context provider-eligible batch. PostgreSQL and MySQL also keep smaller retained-path rows below the staged threshold.

### Registration Guidance

Register `AddDVault()` and the matching provider extension:

- `AddDVaultPostgres()` for clean `Npgsql.EntityFrameworkCore.PostgreSQL` contexts.
- `AddDVaultSqlServer()` for clean SQL Server contexts.
- `AddDVaultMySql()` for clean Pomelo or official MySQL EF Core contexts.
- `AddDVaultOracle()` for clean `Oracle.EntityFrameworkCore` contexts.
- `AddDVaultDb2()` for clean `IBM.EntityFrameworkCore` contexts.

Provider-native dispatch is diagnostics-gated behind the same public save service. Dirty tracked contexts, provider-name mismatches, unsupported multi-active satellite batches, or batches outside the provider gate decline to a smaller provider-native path or the provider-neutral writer. The detailed save boundary is documented in [DVault V1 Explicit Save Service](architecture/dvault-v1-explicit-save-service.md).

### Starting Point

Use these provider boundaries as starting gates. Timing claims require the linked provider-configured artifact bundle for the cited row:

| Provider | Starting gate | Evidence posture |
| --- | --- | --- |
| PostgreSQL | Retain direct or UNNEST below 60 operations; use staged COPY at 60-plus operations. | v0.32 local Podman evidence preserves the direct/UNNEST below-threshold path and staged COPY at 60-plus operations. |
| SQL Server | Native bulk starts at 50-plus total operations and no more than 500 satellite operations. | v0.32 threshold evidence and the configured 2026-06-20 SQL Server bulk-threshold bundle keep the 50 minimum-operation and 500 maximum-satellite-operation gates. |
| MySQL | Tiny satellite-history batches fall back to provider-neutral behavior; larger eligible ordered batches use the retained multi-row or staged bulk provider paths. | v0.32 local evidence records the tiny satellite-history fallback decision and keeps staged bulk for larger eligible rows. |
| Oracle | Direct optimized batching starts at 50-plus total operations and no more than 10000 satellite operations. | v0.32 local evidence retains the direct optimized path and records `stagedOracleBulk=not-selected-no-measured-win`. |
| DB2 | Clean-context hub, link, and ordinary satellite batches can select `Db2DataVaultSaveStrategy`; no staged bulk or provider-native chunk execution is claimed. | The DB2 hotspot bundle completed the optimized clean-context save row and provider-neutral fallback comparison row; the root triplet may still keep DB2 skipped placeholders when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset. |

### Diagnostics And Telemetry

Before claiming provider-native behavior, run request-bound `IDataVaultDiagnosticsService` analysis for the exact batch and verify strategy status, selected strategy name, candidate diagnostics, operation counts, and fallback causes. Use `AddDVaultTelemetry()` for bounded save summaries after the application opts into metrics. Rerun the benchmark with the relevant provider environment variable set when the claim needs measured external-provider timings.

Bounded provider eligibility examples should show enum values and strategy facts without request data, SQL, provider errors, credentials, or connection strings:

```text
save strategy: ProviderNeutralFallback
provider: Microsoft.EntityFrameworkCore.SqlServer
selected strategy: <none>
candidate: SqlServerDataVaultSaveStrategy
candidate gate requirements: ProviderNameMismatch, DirtyDbContext
fallback causes: DirtyDbContext
raw SQL: omitted
connection string: omitted
```

### Supporting Rows

The v0.32 provider benchmark bundles are the current threshold evidence for PostgreSQL, SQL Server, MySQL, and Oracle. The DB2 hotspot bundle is the completed-timing evidence for the DB2 clean-context optimized save row. The root `benchmark-summary.csv` and `benchmark-summary.json` still keep skipped optional-provider rows visible for the quick baseline, but completed provider timing claims should cite the linked artifact bundle that produced them.

Rows to cite:

- PostgreSQL: `dvault-adddvaultpostgres-direct-or-unnest` for the below-60 retained direct/UNNEST boundary and `dvault-adddvaultpostgres-optimized` for the 60-plus staged COPY boundary.
- SQL Server: `dvault-adddvaultsqlserver-optimized` in the configured 2026-06-20 bulk-threshold bundle for the completed native bulk boundary, `SqlBulkCopy` transfer, fallback comparator, and unchanged 50/500 gates.
- MySQL: `dvault-adddvaultmysql-multi-row` for retained provider paths where selected, `dvault-adddvaultmysql-optimized` for staged bulk, and the tiny satellite-history provider-neutral fallback row for the deliberate small-batch exception.
- Oracle: `dvault-adddvaultoracle-optimized` for retained direct optimized batching and the current no-measured-win staged posture.
- DB2: `dvault-adddvaultdb2-optimized` in the DB2 hotspot bundle for the completed clean-context optimized save row with `Db2DataVaultSaveStrategy` selected and `stagedBulkBoundary=not-supported`; `dvault-adddvault-fallback` in the same bundle is the provider-neutral fallback comparison row.

DB2 completed save rows from the hotspot bundle:

| Provider | Scenario | Baseline | Mean ms | Evidence posture |
| --- | --- | --- | ---: | --- |
| DB2 external provider | `provider-native-bulk-ingestion` | `dvault-adddvault-fallback` | 372.746 | `completed-timing`; provider-neutral fallback comparison row with no provider-specific strategy selected and `NoProviderSpecificStrategyRegistered`. |
| DB2 external provider | `provider-native-bulk-ingestion` | `dvault-adddvaultdb2-optimized` | 49.930 | `completed-timing`; selected `Db2DataVaultSaveStrategy` for clean-context set-based save with no fallback causes. |

### Stop Conditions And Rerun Triggers

Stop before making a measured provider-specific performance claim when optional provider rows are skipped, connection strings are unset, provider packages are not restored for the benchmark run, the context has pending tracked changes, the operation count is below the provider gate, the satellite count exceeds SQL Server or Oracle limits, or diagnostics do not select the expected strategy. For DB2 save timing, cite the DB2 hotspot bundle for the completed clean-context row and stop before claiming staged bulk, provider-native chunk execution, dirty-context saves, or unsupported save shapes. Rerun the benchmark triplet with the relevant provider configured and preserve skipped or failed rows exactly as the artifact contract requires.

## Stored-Procedure And Provider-Specific SQL Artifact Gate

Stored procedures, generated database routines, and other provider-specific SQL artifacts are not DVault's default save or read path. The default runtime surfaces remain `IDataVaultSaveService` and `IDataVaultReadService`. Typed read-model helper generation is opt-in design-time ergonomics over those surfaces and must not be widened into provider-specific SQL generation. Do not describe the artifact lane as package registration behavior, default provider routing, runtime metadata inspection, EF interceptor behavior, or automatic migration or deployment ownership.

The v0.32 artifact lane is explicitly opted-in consumer design-time output. The current command surface is `dvault sql-artifact --output <path> [--workload provider-native-bulk-ingestion]`, and the authoritative review manifest schema is `dvault.sql-artifact.v1`. The visible implementation is a SQL Server `provider-native-bulk-ingestion` dry-run exporter that emits review-only manifest output with no deployable SQL payload files and no runtime dispatch. SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 remain the repository-wide supported-provider baseline, but the current dry-run exporter must not be documented as implemented coverage for all supported providers.

Approved artifacts are design-time outputs for reviewed consumer projects only. The consuming application owns review, storage, deployment, invocation, versioning, rollback, cleanup, transactions, credentials, environment selection, migration compatibility, and operational observability. DVault must not auto-create runtime dispatch, auto-run stored procedures or SQL artifacts, register a procedure dispatcher, or automatically synchronize artifacts with EF migrations, live schema, metadata changes, model-first import/export, or support-bundle refreshes.

Use the staged provider ingestion profile above as the comparison baseline. That profile is diagnostics-gated behind the existing save service, keeps skipped optional-provider rows visible when provider evidence is missing, and requires benchmark artifacts before making provider-specific timing claims. Stored-procedure or provider-specific artifact proposals must meet at least that evidence posture before deployable implementation tickets are accepted: run representative request-bound save/read diagnostics for the exact provider and workload, preserve the benchmark artifact triplet and run context, keep unsupported/skipped rows visible, and prove parity with explicit DVault semantics such as ordering, load timestamp, record source, hash key, hash diff, satellite latest-state behavior, PIT or bridge maintenance when relevant, cancellation, cleanup, and caller-owned transaction behavior.

The current dry-run lane and future implementation tickets must treat these items as prerequisites, not implementation details to discover after coding:

- explicit provider and representative workload.
- consumer opt-in mechanism and reviewed design-time workflow.
- generated artifact format, storage, and review rules.
- consumer-owned deployment, invocation, versioning, rollback, cleanup, transaction, credential, and environment responsibilities.
- consumer-owned migration and model-change compatibility plan that does not rely on DVault automatic synchronization.
- representative diagnostics plus benchmark evidence for the exact provider, workload, and artifact shape.
- public non-goals covering runtime dispatchers, automatic execution, EF interceptors, migration hooks, deployment automation, and default provider routing.

Tickets that lack those prerequisites should remain documentation, design, prototype, or evidence-gathering work. They must not enter implementation scope for deployable provider-specific artifact generation or execution, and they must not create unmeasured performance claims.

## Read-Model Heavy

### Workload Shape

Use this profile when read throughput or allocation is the main concern for:

- Latest satellite reads over seeded profile history.
- PIT as-of reads over explicitly maintained PIT rows.
- Bridge traversal reads over explicitly maintained bridge rows.

The checked-in rows focus on `IDataVaultReadService` latest satellite reads, `DataVaultPitAsOfReadRequest`/`DataVaultPitReadRecord`, and `DataVaultBridgeReadRequest`/`DataVaultBridgeReadRecord`. SQLite is the completed optimized latest-satellite timing row in the root benchmark triplet. MySQL latest-satellite completed timing is available from the ticket `06FE4QQ9VF7B74E60CXEHSS5XW` bundle, where `MySqlDataVaultReadStrategy` was selected for the supported current window-function path; that bundle is not a provider-neutral fallback improvement comparator by itself. PostgreSQL PIT/bridge completed timing is available from the v0.32.0 smoke-read bundle, where `PostgresDataVaultReadStrategy` was selected for supported maintained read shapes. The same bundle adds completed SQL Server PIT and bridge timing rows that selected `SqlServerDataVaultReadStrategy`, completed MySQL PIT and bridge timing rows that selected `MySqlDataVaultReadStrategy`, and completed Oracle PIT and bridge timing rows that selected `OracleDataVaultReadStrategy`; latest-satellite timing remains separate from this PIT/bridge closure. DB2 latest-satellite/PIT/bridge completed timing is available from the DB2 hotspot bundle, where `Db2DataVaultReadStrategy` was selected for supported latest-satellite and maintained PIT/bridge shapes.

### Registration Guidance

Start with `AddDVault()` and `IDataVaultReadService` for provider-neutral latest/current/as-of satellite, PIT, and bridge reads. Add `AddDVaultSqlite()` when the application uses SQLite and wants the optimized latest-satellite read strategy or PIT/bridge strategy covered by the checked-in evidence. Add `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, or `AddDVaultDb2()` when the application uses that provider and wants diagnostics-gated optimized latest-satellite strategy selection for supported hub-parent, non-multi-active shapes or the optimized PIT/bridge strategy candidates. Unsupported providers, unsupported request shapes, or incomplete generated read-model projection evidence fall back through provider-neutral read pipelines.

PIT and bridge rows are caller-owned read models. Use `IDataVaultPitMaintenanceService` after satellite ingestion and `IDataVaultBridgeMaintenanceService` after source-link ingestion when those materialized tables should be refreshed. Reads do not run PIT or bridge maintenance implicitly, schedule maintenance, delete stale hierarchy rows, or create provider-specific physical tuning. Keep the detailed boundary in [DVault V1 PIT And Bridge Boundary](architecture/dvault-v1-pit-bridge-boundary.md).

### Starting Point

Use the runtime read service for dynamic and request-built shapes. For SQLite, add the provider package after confirming the read shape is supported and diagnostics select `SqliteDataVaultReadStrategy`. For PostgreSQL, SQL Server, MySQL, Oracle, or DB2 latest-satellite reads, add the provider package after confirming the request is a hub-parent, non-multi-active satellite shape and diagnostics select `PostgresDataVaultReadStrategy`, `SqlServerDataVaultReadStrategy`, `MySqlDataVaultReadStrategy`, `OracleDataVaultReadStrategy`, or `Db2DataVaultReadStrategy`. For PIT/bridge reads, add the matching provider package after diagnostics select `PostgresDataVaultReadStrategy`, `SqlServerDataVaultReadStrategy`, `MySqlDataVaultReadStrategy`, `OracleDataVaultReadStrategy`, or `Db2DataVaultReadStrategy`. For PIT and bridge, place explicit maintenance in the ingestion workflow before read traffic depends on those rows.

### Diagnostics And Telemetry

Use `IDataVaultReadDiagnosticsService` for request-bound read strategy and read-shape diagnostics. The useful fields are strategy status, selected strategy name, fallback causes, read-shape kind, provider status, translated table identity, filter columns, and deterministic row-selection rules. The bounded payload and redaction rules are defined by [DVault V2 Redacted Read-Plan Explain Contract](architecture/dvault-v2-redacted-read-plan-explain-contract.md). Register `AddDVaultTelemetry()` when read attempt, returned-row, requested-key, duration, and finite fallback-cause metrics are needed. Do not expect raw SQL, provider query plans, automatic index creation, or provider-specific physical-design promises from these surfaces.

### Supporting Rows

All values in this quick table are from the root quick benchmark triplet. External-provider completed rows, including MySQL latest-satellite timing, PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge timing, and DB2 read timing, should be cited from their linked artifact bundles with preserved run context instead of copied into this quick table:

| Scenario | Baseline | Mean ms | Evidence posture |
| --- | --- | ---: | --- |
| `latest-satellite-read` | `dvault-adddvault-fallback` | 7.289 | Provider-neutral latest read over 100 customers and 1000 seeded profile states. |
| `latest-satellite-read` | `dvault-adddvaultsqlite-optimized` | 3.271 | SQLite optimized read path selected `SqliteDataVaultReadStrategy`. |
| `pit-as-of-read` | `dvault-adddvault-fallback` | 10.240 | Provider-neutral PIT as-of read over 100 PIT rows and 2 satellite segments. |
| `pit-as-of-read` | `dvault-adddvaultsqlite-optimized` | 10.552 | SQLite optimized PIT read path selected `SqliteDataVaultReadStrategy`. |
| `bridge-traversal-read` | `dvault-adddvault-fallback` | 1.421 | Provider-neutral bridge traversal over 1 ancestor and 100 descendant bridge rows. |
| `bridge-traversal-read` | `dvault-adddvaultsqlite-optimized` | 0.720 | SQLite optimized bridge read path selected `SqliteDataVaultReadStrategy`. |
Provider-configured PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge rows should cite [the v0.32.0 smoke-read bundle](../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md), not the skipped root quick placeholders. DB2 latest-satellite/PIT/bridge rows should cite [the DB2 hotspot bundle](../artifacts/benchmarks/06FE4QR3DD7EFZ4F35SBTFGWSR-db2-hotspot-evidence-20260620/benchmark-summary.md). Oracle PIT/bridge completed timing evidence is limited to the two Oracle rows in the v0.32.0 bundle:

| Provider | Scenario | Baseline | Mean ms | Evidence posture |
| --- | --- | --- | ---: | --- |
| PostgreSQL external provider | `pit-as-of-read` | `dvault-adddvaultpostgres-optimized` | 38.047 | `completed-timing`; selected `PostgresDataVaultReadStrategy` with `readShape=PitAsOf` and no fallback causes. |
| PostgreSQL external provider | `bridge-traversal-read` | `dvault-adddvaultpostgres-optimized` | 10.503 | `completed-timing`; selected `PostgresDataVaultReadStrategy` with `readShape=Bridge` and no fallback causes. |
| SQL Server external provider | `pit-as-of-read` | `dvault-adddvaultsqlserver-optimized` | 73.686 | `completed-timing`; selected `SqlServerDataVaultReadStrategy` with `readShape=PitAsOf` and no fallback causes. |
| SQL Server external provider | `bridge-traversal-read` | `dvault-adddvaultsqlserver-optimized` | 15.066 | `completed-timing`; selected `SqlServerDataVaultReadStrategy` with `readShape=Bridge` and no fallback causes. |
| MySQL external provider | `pit-as-of-read` | `dvault-adddvaultmysql-optimized` | 41.047 | `completed-timing`; selected `MySqlDataVaultReadStrategy` with `readShape=PitAsOf` and no fallback causes. |
| MySQL external provider | `bridge-traversal-read` | `dvault-adddvaultmysql-optimized` | 3.603 | `completed-timing`; selected `MySqlDataVaultReadStrategy` with `readShape=Bridge` and no fallback causes. |
| Oracle external provider | `pit-as-of-read` | `dvault-adddvaultoracle-optimized` | 475.258 | `completed-timing`; selected `OracleDataVaultReadStrategy` with `readShape=PitAsOf` and no fallback causes. |
| Oracle external provider | `bridge-traversal-read` | `dvault-adddvaultoracle-optimized` | 7.388 | `completed-timing`; selected `OracleDataVaultReadStrategy` with `readShape=Bridge` and no fallback causes. |
| DB2 external provider | `latest-satellite-read` | `dvault-adddvaultdb2-optimized` | 19.483 | `completed-timing`; selected `Db2DataVaultReadStrategy` with `readShape=LatestSatellite` and no fallback causes. |
| DB2 external provider | `pit-as-of-read` | `dvault-adddvaultdb2-optimized` | 55.288 | `completed-timing`; selected `Db2DataVaultReadStrategy` with `readShape=PitAsOf` and no fallback causes. |
| DB2 external provider | `bridge-traversal-read` | `dvault-adddvaultdb2-optimized` | 10.483 | `completed-timing`; selected `Db2DataVaultReadStrategy` with `readShape=Bridge` and no fallback causes. |

DB2 completed read evidence is limited to the supported shapes recorded in the hotspot bundle. DB2 skipped placeholders in the root quick triplet, diagnostics-only behavior, and smoke-only posture should not be cited as measured timing evidence.

### Stop Conditions And Rerun Triggers

Stop using the root read rows as sufficient evidence when latest-satellite reads are not using the completed SQLite timing row or a completed provider-configured artifact lane, PIT/bridge reads target a provider without a matching diagnostics-selected strategy or completed provider-configured artifact lane, read-shape diagnostics report fallback, unsupported shape, or incomplete evidence, PIT or bridge maintenance is not run before reads, bridge hierarchy deletions require full rebuild behavior, or the data shape differs materially from the seeded benchmark. For PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge timing claims, cite the v0.32.0 smoke-read artifact triplet with run context instead of the skipped root placeholders. For DB2 latest-satellite/PIT/bridge timing claims, cite the DB2 hotspot artifact triplet with run context and keep unsupported latest-satellite shapes, stale read-model maintenance, and incomplete read-shape evidence in provider-neutral fallback. Diagnostics-gated latest-satellite strategy registration does not create automatic PIT or bridge maintenance or a universal timing claim outside a recorded run context. Rerun read benchmarks and keep read-shape diagnostics with the result when changing provider, indexes, maintenance cadence, shape, dataset size, or runtime.
