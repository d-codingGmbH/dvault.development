# DVault V1 PIT And Bridge Boundary

Status: v1 implementation note
Ticket: 06F5Q91M0PM17RP43ZQRPBDXP0
Current public baseline: [DVault v0.46.0 Release Notes](../releases/v0.46.0.md)
DB2 provider package baseline: [DVault v0.34.0 Release Notes](../releases/v0.34.0.md)
Read-optimization expansion baseline: [DVault v0.28.0 Release Notes](../releases/v0.28.0.md)
PIT/bridge feature-introduction baseline: [DVault v0.21.0 Release Notes](../releases/v0.21.0.md)

## Decision

DVault v1 treats PIT and bridge tables as explicit read models. Application code owns when those read models are maintained, and `IDataVaultReadService` consumes the already-maintained rows for PIT as-of and bridge traversal reads.

`AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, and `AddDVaultDb2()` register repository-proven diagnostics-gated optimized latest-satellite, PIT, and bridge read strategy candidates for their supported shapes. Strategy registration is a dispatch and diagnostics fact, not a completed timing claim by itself. Unsupported providers, declined request shapes, incomplete read-shape evidence, and stale PIT/bridge maintenance evidence keep the provider-neutral read pipelines. Neither read path performs maintenance, schedules background work, updates rows during `SaveChanges`, or turns PIT/bridge metadata into automatic orchestration.

## PIT Maintenance Boundary

`IDataVaultPitMaintenanceService` maintains one `DataVaultPitMetadata` declaration at a time:

- `RebuildAsync(...)` recomputes the complete generated PIT table from persisted hub- or link-parent satellite history.
- `MaintainParentsAsync(...)` recomputes complete PIT history for explicit parent hash keys and replaces the targeted parents' PIT rows so late-arriving satellite history can correct earlier snapshots.
- Empty parent-hash-key requests are no-ops.
- Registry-backed callers can use `DataVaultRegistryPitRebuildRequest` and `DataVaultRegistryPitParentMaintenanceRequest` to resolve a PIT by exact logical name or exact `DataVaultMetadataClrMapping.Pit(...)` CLR mapping from `UseDataVaultMetadata()`.

PIT maintenance is explicit caller work after ingestion. Saves, reads, EF `SaveChanges`, provider startup, and background scheduling do not refresh PIT rows implicitly.

The accepted provider push-down baseline is intentionally asymmetric:

- `AddDVaultPostgres()` registers `IDataVaultProviderPitMaintenanceStrategy` through `PostgresDataVaultPitMaintenanceStrategy`. The default PIT maintenance service can select that provider strategy for clean Npgsql-backed full rebuilds of ordinary hub-parent PITs, shared-driving-key multi-active hub-parent PITs, and link-parent non-multi-active PITs.
- `AddDVaultSqlServer()` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`. That service selects the SQL Server provider path only for clean full rebuilds of ordinary hub-parent PITs.
- PostgreSQL and SQL Server provider paths both stay request-gated. Provider-name mismatch, dirty tracked contexts, unsupported PIT shapes, incomplete provider evidence, and provider-specific guard failures fall back to the provider-neutral maintenance implementation.
- SQL Server `MaintainParentsAsync(...)`, SQL Server multi-active PITs, SQL Server link-parent PITs, and SQL Server caller transactions that cannot provide rollback-clean savepoint behavior use provider-neutral maintenance. SQL Server full-rebuild faults and cancellations preserve the pre-rebuild rows rather than leaving a partially refreshed PIT table.

No provider registration turns PIT maintenance into automatic work. Applications still decide when rebuilds or parent maintenance run, and PIT reads still consume only rows already maintained before the read request.

## PIT Read Boundary

PIT reads target one `DataVaultPitMetadata` declaration, explicit parent hash keys, and an `asOf` timestamp. `ReadPitRowsAsync(...)` returns raw `DataVaultPitReadRecord` rows. `ReadPitAsync(...)` maps selected rows through a caller-owned projector with exact-name access to `ParentHashKey`, canonical driving-key names when present, `LoadTimestamp`, and declared satellite segments.

The runtime metadata path supports:

- hub-parent PITs over ordinary satellites.
- hub-parent PITs over multi-active satellites only when all referenced multi-active satellites share one canonical driving-key name/order family.
- bounded link-parent PITs when every referenced satellite is unique, non-multi-active, and attached to the same declared link parent. For link-parent PITs, `ParentHashKey` carries the link hash key.

For ordinary PITs, one selected PIT row is returned per requested parent. For the bounded multi-active hub-parent baseline, reads keep the parent-hash-key request surface and return one visible row per parent and driving-key tuple. Tuple filters, incompatible driving-key families, and cross-product tuple semantics remain outside the boundary.

The public `dvault.model.v1` PIT artifact shape remains hub-parent-only and continues to use the `hub` field. Runtime link-parent PIT maintenance and reads do not imply model-first link-parent PIT artifacts. Registry-backed PIT coverage is maintenance-request resolution only; this boundary does not expose a registry-backed PIT as-of read request.

## Bridge Maintenance Boundary

`IDataVaultBridgeMaintenanceService` maintains one `DataVaultBridgeMetadata` declaration at a time:

- `RebuildBridgeAsync(...)` recomputes the generated bridge table from persisted source-link rows.
- `MaintainBridgeAsync(...)` inserts missing rows from the current source-link state.
- Many-to-many bridges maintain one row per distinct endpoint pair.
- Hierarchy bridges maintain one row per distinct ancestor/descendant pair, store the minimum positive hop count, treat direct edges as depth `1`, and do not add implicit self rows.
- Registry-backed callers can use `DataVaultRegistryBridgeMaintenanceRequest` to resolve the bridge by logical name from `UseDataVaultMetadata()`.

`MaintainBridgeAsync(...)` is not delete-aware. For hierarchy bridges it can lower an existing `TraversalDepth` when a newly materialized shorter path is available and leaves equal or longer alternate paths unchanged. It does not remove obsolete rows or increase persisted depths after topology shrinkage. Use `RebuildBridgeAsync(...)` when destructive hierarchy changes require row removal or increased `TraversalDepth`.

## Bridge Maintenance Push-Down Posture

Bridge rebuild and maintenance push-down stays deferred from this boundary. The current provider-specific maintenance work is limited to PIT maintenance: PostgreSQL contributes `PostgresDataVaultPitMaintenanceStrategy`, while SQL Server replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService` for a narrower ordinary hub-parent full-rebuild gate. Bridge maintenance remains the provider-neutral `IDataVaultBridgeMaintenanceService` surface, and this contract does not expose an `IDataVaultProviderBridgeMaintenanceStrategy` counterpart.

Maintained-bridge read evidence proves provider read-strategy selection over already-maintained bridge rows. It does not prove write-side bridge-maintenance push-down value, SQL shape, fallback vocabulary, or parity with the broader bridge maintenance semantics above. Any later bridge push-down lane would first need new core dispatch, provider registration, bridge-specific gate/fallback diagnostics, parity tests for existing maintenance semantics, and a preserved benchmark artifact triplet before provider SQL can be promoted.

The reopen threshold is concrete hotspot evidence that provider-neutral bridge maintenance, not bridge reads, is a material bottleneck after the bounded PIT provider-maintenance prototypes. A first reopened slice should stay limited to a single many-to-many full rebuild before hierarchy rebuild push-down, incremental or delete-aware maintenance, provider expansion, deployment artifacts, and support-bundle orchestration are considered as separate non-goals.

## Bridge Read Boundary

Bridge reads target one `DataVaultBridgeMetadata` declaration and filter by endpoint hash keys. Many-to-many bridges support `DataVaultBridgeTraversalEndpoint.From` and `DataVaultBridgeTraversalEndpoint.To`. Hierarchy bridges support `DataVaultBridgeTraversalEndpoint.Ancestor` and `DataVaultBridgeTraversalEndpoint.Descendant`, require a bounded `maximumDepth`, and expose `TraversalDepth` on hierarchy rows.

`ReadBridgeRowsAsync(...)` returns `DataVaultBridgeReadRecord` values with endpoint hash keys in generated column order. Typed bridge projectors use exact generated column names such as `OrderHashKey`, `AncestorSalesRegionHashKey`, `DescendantSalesRegionHashKey`, and `TraversalDepth`.

## Provider Dispatch And Diagnostics

The public read request contract is provider-neutral. `AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, and `AddDVaultDb2()` register optimized read dispatch for supported hub-parent, non-multi-active latest-satellite reads plus supported PIT and bridge read shapes. `AddDVault()` without a provider-specific read strategy, unsupported providers, declined request shapes, incomplete generated read-model projection evidence, and observable stale-maintenance signals such as pending tracked changes fall back to the provider-neutral read path.

`IDataVaultReadDiagnosticsService` is the diagnostics boundary for read strategy and read-shape evidence. Request-bound diagnostics keep provider strategy selection in `ReadStrategy` and add `ReadShape` facts for translated PIT or bridge table identity, filter columns, deterministic row-selection and ordering rules, expected key/index access paths, and provider fallback caveats. The bounded payload and support-bundle redaction rules are defined by [DVault V2 Redacted Read-Plan Explain Contract](dvault-v2-redacted-read-plan-explain-contract.md). Diagnostics do not expose raw hash-key values, as-of values, request keys, generated SQL, or provider query plans.

Support-bundle export can include already-supplied representative read diagnostics through the consumer-owned design-time command host. The generic command runner does not invent representative PIT or bridge requests.

## Evidence

Focused integration coverage:

- [DataVaultPitReadServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs) covers SQLite PIT as-of reads, provider strategy selection, provider-neutral fallback diagnostics, and PIT read-shape facts.
- [DataVaultPitMaintenanceServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs) covers PIT rebuild, parent maintenance, late-arriving correction, shared-driving-key multi-active PITs, link-parent runtime PITs, and registry-backed PIT maintenance requests.
- [PostgresProviderCapabilityTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs) covers PostgreSQL PIT maintenance strategy registration and the approved full-rebuild gate for ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs.
- [PostgresPitMaintenanceServiceTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs) covers configured PostgreSQL full rebuild behavior for the supported PIT shapes.
- [SqlServerDataVaultPitMaintenanceServiceTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs) covers the SQL Server ordinary hub-parent full-rebuild gate plus provider-neutral fallback for maintain-parents, provider mismatch, dirty contexts, link-parent PITs, multi-active PITs, and no-savepoint caller transactions.
- [SqlServerDataVaultSmokeTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs) covers configured SQL Server ordinary PIT rebuild parity and rollback-clean behavior on failure and cancellation.
- [DataVaultProviderReadStrategyTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs) covers PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite candidate gates plus PostgreSQL, SQL Server, MySQL, Oracle, and DB2 PIT/bridge candidate gates for provider match, supported shape selection, unsupported-shape fallback, incomplete-evidence fallback, and stale-maintenance-signal fallback causes.
- [DataVaultRelationalPitBridgeReadStrategyParityTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs) executes PostgreSQL, SQL Server, Oracle, and DB2 latest-satellite candidate reads plus PostgreSQL, SQL Server, MySQL, Oracle, and DB2 PIT/bridge candidate read paths and compares row plus typed projection results with the provider-neutral `AddDVault()` fallback path.
- [DataVaultBridgeReadServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs) covers SQLite bridge reads, bounded hierarchy depth, registry-backed bridge read shape, and provider-neutral fallback behavior.
- [DataVaultBridgeMaintenanceServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs) covers many-to-many bridge maintenance, hierarchy depth behavior, rebuild after topology shrink/delete scenarios, cycle handling without implicit self rows, and registry-backed bridge maintenance.
- [DataVaultDiagnosticsIntegrationTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs) covers SQLite read strategy diagnostics, read-shape diagnostics, registry-backed read-shape equivalence, and aggregate preflight representative diagnostics.
- [ExplicitDataVaultSaveServiceTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs) covers provider package registration for SQLite/PostgreSQL latest-satellite/PIT/bridge read strategies and relational PIT/bridge read strategy registrations.
- [MySqlProviderCapabilityTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs) covers MySQL latest-satellite read strategy registration and generated window-function SQL shape.
- [DVaultPostgresServiceCollectionExtensions.cs](../../src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs) registers `PostgresDataVaultPitMaintenanceStrategy` as an `IDataVaultProviderPitMaintenanceStrategy`.
- [DVaultSqlServerServiceCollectionExtensions.cs](../../src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs) replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`.
- Provider package service-collection extensions under `src/DCoding.Data.DVault.Sqlite`, `src/DCoding.Data.DVault.Postgres`, `src/DCoding.Data.DVault.SqlServer`, `src/DCoding.Data.DVault.MySql`, `src/DCoding.Data.DVault.Oracle`, and `src/DCoding.Data.DVault.Db2` register the current provider read strategy candidates.

Benchmark evidence:

- [benchmark-summary.md](../../benchmark-summary.md)
- [benchmark-summary.csv](../../benchmark-summary.csv)
- [benchmark-summary.json](../../benchmark-summary.json)
- [artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.md](../../artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.md)
- [artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.csv](../../artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.csv)
- [artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.json](../../artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.json)
- [artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md)
- [artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.csv](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.csv)
- [artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.json](../../artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.json)

The shared benchmark artifact contract is [Performance Evidence And Benchmark Artifact Contract](../plans/performance-evidence-benchmark-artifact-contract.md). The relevant completed timing rows include SQLite `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` from the root quick triplet plus PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite, PIT, and bridge rows from the [2026-06-23 provider optimization closure bundle](../../artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/README.md). Those provider-configured rows selected `PostgresDataVaultReadStrategy`, `SqlServerDataVaultReadStrategy`, `MySqlDataVaultReadStrategy`, `OracleDataVaultReadStrategy`, and `Db2DataVaultReadStrategy` for supported latest-satellite and maintained PIT/bridge shapes. Optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 root quick read rows remain visible as skipped rows when their connection-string environment variables are unset; those root placeholders preserve row identity and planned strategy facts but are not completed external-provider timing evidence.

Use [Provider Optimization Evidence Matrix](../plans/provider-optimization-evidence-matrix.md) as the canonical read-row lookup for scenario, provider, baseline, evidence posture, authoritative artifact source, and finite stop/fallback conditions. This PIT/bridge boundary remains the behavior contract; the matrix is the citation surface for downstream provider optimization evidence.

## Migration, Drift, And Compatibility Context

Migration and drift guidance remains centralized in [DVault Dotnet EF Design-Time Workflow](dvault-dotnet-ef-design-time-workflow.md) and [Model-First Governance Workflow](../model-first-governance.md). PIT and bridge declarations can participate in the same configured-model validation, reviewed-artifact import, drift comparison, migration guardrail, and support-bundle lanes when the consumer supplies those inputs.

Compiled-model, compiled-query, and pooled-context guidance remains in [DVault EF Compiled Compatibility](dvault-ef-compiled-compatibility.md). That note does not widen PIT/bridge read dispatch or maintenance behavior; dynamic `IDataVaultReadService` PIT and bridge requests remain the documented read-service path.

## Unsupported In V1

- Automatic PIT or bridge maintenance.
- Read-time PIT or bridge refresh.
- Background schedulers, triggers, or implicit EF `SaveChanges` orchestration.
- Provider-specific PIT maintenance beyond the current PostgreSQL strategy and SQL Server service-replacement gates.
- Bridge maintenance push-down without bridge-maintenance hotspot evidence, a core/provider bridge-maintenance seam, bridge-specific fallback diagnostics, and benchmark-backed parity proof.
- Completed non-SQLite optimized latest-satellite timing claims without a provider-configured benchmark artifact.
- Completed DB2 PIT or bridge timing claims without a provider-configured benchmark artifact.
- Registry-backed PIT as-of read requests.
- Model-first link-parent PIT artifacts.
- Link-parent multi-active PITs.
- Incompatible multi-active driving-key-family PITs.
- Cross-product tuple semantics or tuple-filter request parameters.
- Delete-aware bridge maintenance through `MaintainBridgeAsync(...)`.
- Graph traversal APIs beyond bounded bridge read helpers.
- Effectivity-specific APIs, path payload columns, or closure-state columns.
- Provider-specific physical-design tuning, automatic index creation, raw SQL evidence, or provider query-plan advice.
