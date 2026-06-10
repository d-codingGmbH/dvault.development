# DVault V1 PIT And Bridge Boundary

Status: v1 implementation note
Ticket: 06F5Q91M0PM17RP43ZQRPBDXP0
Current public baseline: [DVault v0.34.0 Release Notes](../releases/v0.34.0.md)
Read-optimization expansion baseline: [DVault v0.28.0 Release Notes](../releases/v0.28.0.md)
PIT/bridge feature-introduction baseline: [DVault v0.21.0 Release Notes](../releases/v0.21.0.md)

## Decision

DVault v1 treats PIT and bridge tables as explicit read models. Application code owns when those read models are maintained, and `IDataVaultReadService` consumes the already-maintained rows for PIT as-of and bridge traversal reads.

`AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, and `AddDVaultDb2()` register repository-proven diagnostics-gated optimized PIT/bridge read strategy candidates. SQLite also remains the only optimized latest-satellite read provider path. Unsupported providers, non-SQLite latest-satellite requests, declined request shapes, incomplete read-shape evidence, and stale PIT/bridge maintenance evidence keep the provider-neutral read pipelines. Neither read path performs maintenance, schedules background work, updates rows during `SaveChanges`, or turns PIT/bridge metadata into automatic orchestration.

## PIT Maintenance Boundary

`IDataVaultPitMaintenanceService` maintains one `DataVaultPitMetadata` declaration at a time:

- `RebuildAsync(...)` recomputes the complete generated PIT table from persisted hub- or link-parent satellite history.
- `MaintainParentsAsync(...)` recomputes complete PIT history for explicit parent hash keys and replaces the targeted parents' PIT rows so late-arriving satellite history can correct earlier snapshots.
- Empty parent-hash-key requests are no-ops.
- Registry-backed callers can use `DataVaultRegistryPitRebuildRequest` and `DataVaultRegistryPitParentMaintenanceRequest` to resolve a PIT by exact logical name or exact `DataVaultMetadataClrMapping.Pit(...)` CLR mapping from `UseDataVaultMetadata()`.

PIT maintenance is explicit caller work after ingestion. Saves, reads, EF `SaveChanges`, provider startup, and background scheduling do not refresh PIT rows implicitly.

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

## Bridge Read Boundary

Bridge reads target one `DataVaultBridgeMetadata` declaration and filter by endpoint hash keys. Many-to-many bridges support `DataVaultBridgeTraversalEndpoint.From` and `DataVaultBridgeTraversalEndpoint.To`. Hierarchy bridges support `DataVaultBridgeTraversalEndpoint.Ancestor` and `DataVaultBridgeTraversalEndpoint.Descendant`, require a bounded `maximumDepth`, and expose `TraversalDepth` on hierarchy rows.

`ReadBridgeRowsAsync(...)` returns `DataVaultBridgeReadRecord` values with endpoint hash keys in generated column order. Typed bridge projectors use exact generated column names such as `OrderHashKey`, `AncestorSalesRegionHashKey`, `DescendantSalesRegionHashKey`, and `TraversalDepth`.

## Provider Dispatch And Diagnostics

The public read request contract is provider-neutral. `AddDVaultSqlite()` registers optimized SQLite read dispatch for supported latest-satellite, PIT, and bridge read shapes. `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, and `AddDVaultDb2()` register optimized PIT and bridge read strategy candidates for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 without adding latest-satellite read dispatch. `AddDVault()` without a provider-specific read strategy, unsupported providers, non-SQLite latest-satellite requests, declined request shapes, incomplete generated read-model projection evidence, and observable stale-maintenance signals such as pending tracked changes fall back to the provider-neutral read path.

`IDataVaultReadDiagnosticsService` is the diagnostics boundary for read strategy and read-shape evidence. Request-bound diagnostics keep provider strategy selection in `ReadStrategy` and add `ReadShape` facts for translated PIT or bridge table identity, filter columns, deterministic row-selection and ordering rules, expected key/index access paths, and provider fallback caveats. The bounded payload and support-bundle redaction rules are defined by [DVault V2 Redacted Read-Plan Explain Contract](dvault-v2-redacted-read-plan-explain-contract.md). Diagnostics do not expose raw hash-key values, as-of values, request keys, generated SQL, or provider query plans.

Support-bundle export can include already-supplied representative read diagnostics through the consumer-owned design-time command host. The generic command runner does not invent representative PIT or bridge requests.

## Evidence

Focused integration coverage:

- [DataVaultPitReadServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs) covers SQLite PIT as-of reads, provider strategy selection, provider-neutral fallback diagnostics, and PIT read-shape facts.
- [DataVaultPitMaintenanceServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs) covers PIT rebuild, parent maintenance, late-arriving correction, shared-driving-key multi-active PITs, link-parent runtime PITs, and registry-backed PIT maintenance requests.
- [DataVaultProviderReadStrategyTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs) covers PostgreSQL, SQL Server, MySQL, Oracle, and DB2 PIT/bridge candidate gates for provider match, supported shape selection, unsupported-shape fallback, incomplete-evidence fallback, and stale-maintenance-signal fallback causes.
- [DataVaultRelationalPitBridgeReadStrategyParityTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs) executes PostgreSQL, SQL Server, MySQL, Oracle, and DB2 PIT/bridge candidate read paths against maintained rows and compares row plus typed projection results with the provider-neutral `AddDVault()` fallback path.
- [DataVaultBridgeReadServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs) covers SQLite bridge reads, bounded hierarchy depth, registry-backed bridge read shape, and provider-neutral fallback behavior.
- [DataVaultBridgeMaintenanceServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs) covers many-to-many bridge maintenance, hierarchy depth behavior, rebuild after topology shrink/delete scenarios, cycle handling without implicit self rows, and registry-backed bridge maintenance.
- [DataVaultDiagnosticsIntegrationTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs) covers SQLite read strategy diagnostics, read-shape diagnostics, registry-backed read-shape equivalence, and aggregate preflight representative diagnostics.
- [ExplicitDataVaultSaveServiceTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs) covers provider package registration for SQLite latest-satellite/PIT/bridge read strategies and relational PIT/bridge read strategy registrations.
- Provider package service-collection extensions under `src/DCoding.Data.DVault.Sqlite`, `src/DCoding.Data.DVault.Postgres`, `src/DCoding.Data.DVault.SqlServer`, `src/DCoding.Data.DVault.MySql`, `src/DCoding.Data.DVault.Oracle`, and `src/DCoding.Data.DVault.Db2` register the current provider read strategy candidates.

Benchmark evidence:

- [benchmark-summary.md](../../benchmark-summary.md)
- [benchmark-summary.csv](../../benchmark-summary.csv)
- [benchmark-summary.json](../../benchmark-summary.json)
- [artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.md](../../artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.md)
- [artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.csv](../../artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.csv)
- [artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.json](../../artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.json)

The shared benchmark artifact contract is [Performance Evidence And Benchmark Artifact Contract](../plans/performance-evidence-benchmark-artifact-contract.md). The relevant completed timing rows are SQLite `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read`. Optional PostgreSQL, SQL Server, MySQL, and Oracle read rows remain visible as skipped rows when their connection-string environment variables are unset; PIT/bridge rows carry `selectedStrategy`/`plannedReadStrategy` guidance, while non-SQLite latest-satellite rows explicitly record that no provider-specific latest-satellite read strategy is registered.

## Migration, Drift, And Compatibility Context

Migration and drift guidance remains centralized in [DVault Dotnet EF Design-Time Workflow](dvault-dotnet-ef-design-time-workflow.md) and [Model-First Governance Workflow](../model-first-governance.md). PIT and bridge declarations can participate in the same configured-model validation, reviewed-artifact import, drift comparison, migration guardrail, and support-bundle lanes when the consumer supplies those inputs.

Compiled-model, compiled-query, and pooled-context guidance remains in [DVault EF Compiled Compatibility](dvault-ef-compiled-compatibility.md). That note does not widen PIT/bridge read dispatch or maintenance behavior; dynamic `IDataVaultReadService` PIT and bridge requests remain the documented read-service path.

## Unsupported In V1

- Automatic PIT or bridge maintenance.
- Read-time PIT or bridge refresh.
- Background schedulers, triggers, or implicit EF `SaveChanges` orchestration.
- Provider-specific PIT or bridge maintenance strategies.
- Non-SQLite optimized latest-satellite read claims.
- Registry-backed PIT as-of read requests.
- Model-first link-parent PIT artifacts.
- Link-parent multi-active PITs.
- Incompatible multi-active driving-key-family PITs.
- Cross-product tuple semantics or tuple-filter request parameters.
- Delete-aware bridge maintenance through `MaintainBridgeAsync(...)`.
- Graph traversal APIs beyond bounded bridge read helpers.
- Effectivity-specific APIs, path payload columns, or closure-state columns.
- Provider-specific physical-design tuning, automatic index creation, raw SQL evidence, or provider query-plan advice.
