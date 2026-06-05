# DVault V1 Typed PIT And Bridge Helper Contract

Status: v1 implemented generator contract
Ticket: 06F7Y0GT7A5QT77TADMRZBVYN8
Current public baseline: [DVault v0.26.0 Release Notes](../releases/v0.26.0.md)
Typed helper implementation baseline: [DVault v0.25.0 Release Notes](../releases/v0.25.0.md)

## Decision

The implemented typed read-model generator baseline is support-bundle-driven and emits helpers for reviewed satellite, PIT, and bounded bridge read shapes. Satellite helpers continue to cover latest/current/as-of reads. PIT and bridge helpers are now part of the current v1 generated-helper contract when the authoritative support bundle carries the request-bound ReadShape evidence described below.

Typed helpers consume exactly one authoritative `dvault.support-bundle.v1` additional file. PIT and bridge helper emission requires reviewed request-bound `readShape.pit` or `readShape.bridge` explain metadata plus the existing optional `DVaultTypedReadModelMetadataSourceFingerprint` gate. The generator must not parse raw `dvault.model.v1` files directly, inspect source-visible Code-First callbacks, infer helpers from literal metadata-first declarations, or fall back to unreviewed metadata sources.

Generated helpers are ergonomic extension methods over the existing provider-neutral `IDataVaultReadService` boundary. They construct stable metadata/read request values and project generated satellite rows, maintained PIT rows, or maintained bridge rows. They do not generate provider-specific SQL, perform PIT or bridge maintenance, schedule refresh, call `SaveChanges`, compile dynamic read requests, or widen runtime read semantics.

## Input And Fingerprint Boundary

Helper generation starts only when the project opts in through `DVaultGenerateTypedReadModels=true` and exactly one authoritative `dvault.support-bundle.v1` source is resolved. Missing, malformed, incompatible-version, non-authoritative, or ambiguous support-bundle input remains `DMV1960`. Raw or residual `dvault.model.v1` additional files are also `DMV1960` source-boundary failures until they have been imported, projected, and represented in the authoritative support bundle. A configured `DVaultTypedReadModelMetadataSourceFingerprint` that differs from the support-bundle metadata-source fingerprint remains `DMV1961`.

PIT and bridge helper emission uses support-bundle explain facts because `readShape` is request-bound. The support bundle must prove the translated table name, produced or mapped column names, parent reference, endpoint vocabulary, traversal depth requirement, deterministic ordering, and projected column groups needed for the generated helper. A support bundle that only proves a runtime metadata declaration but omits the request-bound read-shape facts is insufficient for typed PIT or bridge helper emission. Application code supplies representative request-bound diagnostics to the support-bundle verb through `DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics`; the reusable command runner does not invent representative PIT or bridge requests.

Unsupported PIT or bridge facts produce an entity-specific diagnostic and skip only the affected helper. Other supported satellite, PIT, or bridge helpers in the same support bundle continue to generate when their own evidence is valid.

## Generated Naming And Helper Surface

Generated rows use the existing typed satellite naming pattern:

- namespace: `{RootNamespace}.DVault.GeneratedReadModels` when `RootNamespace` is supplied, otherwise `DVault.GeneratedReadModels`.
- row record: `{ProducedName}ReadModel`.
- extension type: `{ProducedName}ReadExtensions`.

PIT helpers emit one as-of method:

```csharp
Read{ProducedName}AsOfAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> parentHashKeys,
    DateTimeOffset asOf,
    CancellationToken cancellationToken = default)
```

The method constructs a `DataVaultPitAsOfReadRequest` from generated PIT metadata, delegates to `IDataVaultReadService`, and returns `Task<IReadOnlyList<{ProducedName}ReadModel>>`.

Many-to-many bridge helpers emit endpoint-specific traversal methods:

```csharp
Read{ProducedName}FromAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> endpointHashKeys,
    CancellationToken cancellationToken = default)

Read{ProducedName}ToAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> endpointHashKeys,
    CancellationToken cancellationToken = default)
```

Hierarchy bridge helpers emit bounded endpoint-specific traversal methods:

```csharp
Read{ProducedName}AncestorAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> endpointHashKeys,
    int maximumDepth,
    CancellationToken cancellationToken = default)

Read{ProducedName}DescendantAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> endpointHashKeys,
    int maximumDepth,
    CancellationToken cancellationToken = default)
```

Bridge methods construct `DataVaultBridgeReadRequest` with the matching `DataVaultBridgeTraversalEndpoint` value: `From`, `To`, `Ancestor`, or `Descendant`. Hierarchy methods must require an explicit bounded `maximumDepth`; the generator must not provide an unbounded hierarchy overload.

## Supported PIT Shapes

Typed PIT helpers may be emitted only for runtime PIT shapes already proven by the repository PIT boundary:

- hub-parent PITs over ordinary satellites.
- hub-parent PITs over multi-active satellites when all referenced multi-active satellites share one canonical driving-key name/order family.
- bounded link-parent PITs when every referenced satellite is unique, non-multi-active, and attached to the same declared link parent. For link-parent PITs, `ParentHashKey` carries the link hash key.

The support-bundle evidence must identify the PIT parent, produced PIT table, parent hash-key column, PIT `LoadTimestamp` column, optional canonical driving-key columns, included PIT segments, and nullable snapshot-reference timestamp columns. The generated helper reads maintained PIT state only and does not trigger PIT rebuilds, parent maintenance, latest-satellite fallback, or extra payload projection.

Unsupported PIT residual shapes include missing read-shape evidence, raw support-bundle gaps, link-parent multi-active PITs, incompatible multi-active driving-key families, tuple-filter requests, cross-product multi-active semantics, unbounded PIT tuple expansion, PIT shapes requiring satellite payload joins, unsupported CLR or nullability facts, redaction gaps, provider-specific read semantics, and any shape that requires runtime query construction beyond the existing read-service request.

## Supported Bridge Shapes

Typed bridge helpers may be emitted only for runtime bridge shapes already proven by the repository bridge boundary:

- many-to-many bridge traversal from the closed `From` and `To` endpoint vocabulary.
- hierarchy bridge traversal from the closed `Ancestor` and `Descendant` endpoint vocabulary with required bounded `maximumDepth`.

The support-bundle evidence must identify the bridge kind, produced bridge table, endpoint roles, endpoint hash-key columns in generated order, selected filter endpoints, deterministic ordering, and hierarchy `TraversalDepth` column when the bridge is a hierarchy.

Unsupported bridge residual shapes include missing read-shape evidence, endpoint vocabularies outside `From`, `To`, `Ancestor`, and `Descendant`, hierarchy traversal without bounded `maximumDepth`, graph/path APIs, path payload contracts, closure-state contracts, delete-aware maintenance expectations, unbounded traversal, provider-specific traversal semantics, unsupported CLR or nullability facts, redaction gaps, and any shape that requires runtime query construction beyond the existing read-service request.

## Generated Projection And Constants

PIT read models project PIT table columns only:

- `string ParentHashKey` for the required parent or link hash key.
- `DateTimeOffset LoadTimestamp` for the required selected PIT row load timestamp.
- required `string` canonical driving-key members when the supported PIT shape has tuple-aware rows.
- nullable `DateTimeOffset?` snapshot-reference timestamp members per included PIT segment.

PIT helpers must not project satellite payload members, hash diffs, record sources, or materialized satellite rows.

Bridge read models project bridge row columns only:

- non-null `string` endpoint hash-key members in generated column order.
- required `int TraversalDepth` for hierarchy bridges.

Every generated read model carries the compatibility constants established by typed satellite helpers:

- `ProducedTableName`.
- `MetadataSourceKind`.
- `MetadataSourceFingerprint`.
- `{MemberName}ProducedColumnName` for each generated member.
- `{MemberName}MappedName` for each generated member.

Constant values come from the authoritative support-bundle produced or mapped names. Deterministic generated type, method, property, or constant name collisions remain `DMV1965`.

## Diagnostics And Skip Behavior

Diagnostics must name the affected metadata item and the bounded reason:

- `DMV1960` for missing, invalid, incompatible-version, non-authoritative, or ambiguous support-bundle input, and for raw or residual `dvault.model.v1` additional files outside the projected support-bundle contract.
- `DMV1961` for metadata-source fingerprint drift.
- `DMV1963` for PIT metadata that lacks the bounded helper evidence or declares an unsupported PIT shape.
- `DMV1964` for bridge metadata that lacks the bounded helper evidence or declares an unsupported bridge shape.
- `DMV1965` for deterministic generated name collisions.
- `DMV1967` for shapes that require dynamic runtime query construction, provider SQL, runtime projection selection, unbounded traversal, tuple expansion, or payload joins outside this contract.
- `DMV1968` remains reserved for future model-first-specific typed helper outcomes; current raw or residual model-first source-boundary failures use `DMV1960`.
- `DMV1969` for valid runtime metadata shapes intentionally skipped because they remain outside the generated helper boundary.

PIT or bridge diagnostics skip only the affected helper. They must not suppress unrelated satellite helpers or unrelated valid PIT/bridge helpers in the same support bundle.

## Non-Goals

This contract does not add:

- raw `dvault.model.v1` generator parsing.
- source-visible Code-First or metadata-first declaration inference.
- provider-specific SQL generation.
- custom LINQ or query providers.
- dynamic request compilation.
- automatic PIT or bridge maintenance.
- read-time refresh, scheduling, or `SaveChanges` orchestration.
- graph or path APIs beyond the existing many-to-many and hierarchy bridge read boundary.
- new public runtime read primitives.

## Evidence

Repository evidence for this contract:

- [DCoding.Data.DVault.Analyzers README](../../src/DCoding.Data.DVault.Analyzers/README.md) documents the implemented support-bundle-driven satellite, PIT, and bridge typed read-model generator baseline.
- [DVault v0.25.0 Release Notes](../releases/v0.25.0.md) records the historical ReadShape and typed helper implementation baseline carried forward by the current release.
- [DVault V1 PIT And Bridge Boundary](dvault-v1-pit-bridge-boundary.md) defines the supported runtime PIT and bridge read shapes.
- [DVault V2 Redacted Read-Plan Explain Contract](dvault-v2-redacted-read-plan-explain-contract.md) defines request-bound `readShape.pit` and `readShape.bridge` support-bundle evidence.
- [DataVaultBridgeTraversalEndpoint.cs](../../src/DCoding.Data.DVault/DataVaultBridgeTraversalEndpoint.cs) defines the closed bridge endpoint vocabulary.
- [DataVaultTypedReadModelSourceGeneratorTests.cs](../../tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs) covers generated PIT and bridge helper output, bridge helper delegation through `IDataVaultReadService`, required hierarchy `maximumDepth`, and PIT/bridge skip diagnostics.
