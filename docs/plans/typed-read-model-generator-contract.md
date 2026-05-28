# Typed Read Model Generator Contract

Status: v1 generator contract
Ticket: 06F5Q922T5B21GJN49FYN6DJH0
Downstream implementation tickets: 06F5Q92AHG0ZCTVQGC6NAYVP9C, 06F5Q92R02HB7FCE1AWKXPTMRW

## Purpose

This contract fixes the v1 source-generator boundary for typed DVault read models. The generator is an ergonomic layer over stable, metadata-defined read shapes. It does not add a new read engine, runtime query compiler, provider-specific SQL generator, PIT or bridge maintenance service, or `dvault.model.v1` dialect.

The v1 generator consumes one authoritative DVault metadata source, normalizes that source into one internal descriptor model, and emits typed helpers for:

- latest and current satellite projections.
- as-of satellite projections.
- PIT as-of projections over already-maintained PIT rows.
- bridge traversal projections over already-maintained bridge rows.

Dynamic `IDataVaultReadService` request construction remains the supported non-generated path for read shapes that are not statically described by authoritative metadata.

## Authoritative Metadata Boundary

Every generated helper must be produced from one resolved metadata source. The source may begin in any supported declaration path, but it must be normalized before generation:

| Input mode | V1 normalization requirement |
| --- | --- |
| Metadata-first | Use the supplied `DataVaultMetadataModel`, `DataVaultMetadataRegistry`, or equivalent in-memory metadata object as the authority, then project it into the same translated EF/DVault metadata descriptor used by the other modes. |
| Model-first | Import the reviewed `dvault.model.v1` JSON artifact, reject unsupported schema versions and unknown fields under the existing model-first contract, project the artifact into DVault metadata, then generate from that projected metadata. |
| Code-first | Use the configured EF model after `ApplyDataVaultMetadata(...)`, `UseDataVaultMetadata(...)`, or equivalent projection has attached DVault annotations. EF compiled-model usage through `UseModel(...)` is supported only when the runtime model was initialized from a design model that already contained the DVault metadata annotations. |

The descriptor is an implementation-internal generator model, not a new public artifact format. It must preserve these values from the authoritative source:

- model-level `MetadataSourceKind` and `MetadataSourceFingerprint`.
- entity-level `ProducedName`, `MetadataName`, `EntityKind`, `ParentReferenceKind`, and `ParentReferenceName`.
- property-level `ProducedName`, `MetadataName`, `PropertyRole`, `TechnicalColumnRole`, `ProviderLogicalPropertyKind`, provider storage/value metadata, ordinal, CLR type, and nullability.
- PIT satellite declaration order, PIT segment snapshot column bindings, and multi-active driving-key names in canonical order.
- bridge kind, endpoint roles, endpoint source names, endpoint produced columns, and hierarchy `TraversalDepth`.

Generation must fail with a diagnostic when the authoritative source cannot be resolved deterministically, when more than one source is visible for the same generated scope, or when the source fingerprint expected by generated code differs from the current authoritative fingerprint.

## Generated Artifact Boundary

The generator may emit only source files in the consuming compilation. Generated code may:

- call the existing provider-neutral `IDataVaultReadService` APIs and typed projection row helpers.
- build `DataVaultLatestSatelliteReadRequest`, `DataVaultPitAsOfReadRequest`, and `DataVaultBridgeReadRequest` values for statically supported shapes.
- use stable direct EF projection patterns over generated shared-type tables when table names, column names, filter shape, ordering, and projection are known at compile time except for scalar parameters.
- use EF compiled queries only for those stable EF query expressions.

Generated code must not:

- compile arbitrary caller-owned `IDataVaultReadService` requests.
- compile runtime projector delegates or generated request objects.
- emit provider-specific SQL, raw SQL, query hints, index advice, migration operations, or provider-specific performance claims.
- maintain, rebuild, or refresh PIT or bridge rows.
- register background work, call `SaveChanges`, or alter the ingestion/save pipeline.
- expose generated DVault shared-type tables as public `DbSet<Dictionary<string, object>>` members.

## Deterministic Public Naming

Generated public API names are derived from produced DVault names, not from physical provider identifiers after provider length truncation. Exact produced names must also be preserved in generated constants or internal binding tables so diagnostics and tests can verify the mapping.

Identifier normalization for generated C# symbols is:

1. Start from the indicated produced table name, produced column name, metadata name, endpoint role, or method token.
2. Split on non-ASCII-letter and non-ASCII-digit characters and on casing transitions using the same ASCII-token behavior as the default DVault naming policy.
3. Convert tokens to PascalCase.
4. If no token remains, use `Value`.
5. If the identifier starts with a digit, prefix `Dvault`.
6. If the identifier is a C# keyword, prefix `Dvault`.
7. Resolve same-scope collisions by appending `2`, `3`, and so on in ordinal order.

The v1 generated type and member pattern is:

| Shape | Row type | Extension type | Methods |
| --- | --- | --- | --- |
| Satellite | `{SatelliteProducedName}ReadModel` | `{SatelliteProducedName}ReadExtensions` | `Read{SatelliteProducedName}CurrentAsync(...)`, `Read{SatelliteProducedName}LatestAsync(...)`, `Read{SatelliteProducedName}AsOfAsync(...)` |
| PIT | `{PitProducedName}ReadModel` | `{PitProducedName}ReadExtensions` | `Read{PitProducedName}AsOfAsync(...)` |
| Bridge | `{BridgeProducedName}ReadModel` | `{BridgeProducedName}ReadExtensions` | `Read{BridgeProducedName}{EndpointRole}Async(...)` |

`SatelliteProducedName`, `PitProducedName`, and `BridgeProducedName` are the entity `ProducedName` values normalized as C# identifiers. `EndpointRole` is `From`, `To`, `Ancestor`, or `Descendant`.

The generated namespace is the consuming project root namespace plus `.DVault.GeneratedReadModels`. If no root namespace is available, use `DVault.GeneratedReadModels`. Implementations may add internal helper types in the same namespace, but the public row and extension names above are the contract.

## Generated Method Signatures

Generated extension methods are static extension methods over `IDataVaultReadService`. They keep the caller-owned `DbContext`, hash-key request values, and cancellation token boundary explicit.

Satellite helper methods:

```csharp
Task<IReadOnlyList<SatCustomerProfileReadModel>> ReadSatCustomerProfileCurrentAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> parentHashKeys,
    CancellationToken cancellationToken = default);

Task<IReadOnlyList<SatCustomerProfileReadModel>> ReadSatCustomerProfileLatestAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> parentHashKeys,
    CancellationToken cancellationToken = default);

Task<IReadOnlyList<SatCustomerProfileReadModel>> ReadSatCustomerProfileAsOfAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> parentHashKeys,
    DateTimeOffset asOf,
    CancellationToken cancellationToken = default);
```

`Current` and `Latest` are equivalent convenience names over `DataVaultLatestSatelliteReadRequest` with no `asOf` value. `AsOf` passes an inclusive UTC cutoff through `DataVaultLatestSatelliteReadRequest`. Parent hash keys are deduplicated with `StringComparer.Ordinal` by the existing request contract.

PIT helper methods:

```csharp
Task<IReadOnlyList<PitCustomerProfileReadModel>> ReadPitCustomerProfileAsOfAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> parentHashKeys,
    DateTimeOffset asOf,
    CancellationToken cancellationToken = default);
```

The generated PIT request targets one `DataVaultPitMetadata` declaration and consumes already-maintained PIT rows. For link-parent PITs, the `parentHashKeys` parameter carries link hash keys exactly as the runtime PIT boundary defines.

Bridge helper methods:

```csharp
Task<IReadOnlyList<BridgeCustomerOrderReadModel>> ReadBridgeCustomerOrderFromAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> endpointHashKeys,
    CancellationToken cancellationToken = default);

Task<IReadOnlyList<BridgeSalesRegionHierarchyReadModel>> ReadBridgeSalesRegionHierarchyAncestorAsync(
    this IDataVaultReadService readService,
    DbContext dbContext,
    IEnumerable<string> endpointHashKeys,
    int maximumDepth,
    CancellationToken cancellationToken = default);
```

Many-to-many bridge helpers emit `From` and `To` methods and must not expose `maximumDepth`. Hierarchy bridge helpers emit `Ancestor` and `Descendant` methods and must require a non-negative `maximumDepth` parameter.

Generated code must throw the same argument exceptions as the underlying request objects for null read services, contexts, metadata, hash-key collections, empty hash keys, unsupported bridge endpoints, and invalid hierarchy depth.

## Satellite Projection Contract

Supported satellite generation targets one `DataVaultSatelliteMetadata` declaration and explicit parent hash keys. Hub-parent and link-parent satellites are supported. Multi-active satellites are supported when their driving-key metadata is deterministic and each driving key has a stable produced property binding.

Each generated satellite row type contains:

| Property source | Generated property |
| --- | --- |
| Parent hash-key technical column | non-null `string ParentHashKey` or the normalized produced parent hash-key column name when it differs from `ParentHashKey` |
| Driving keys in metadata order | non-null `string` properties named from produced driving-key property names |
| Hash diff technical column | non-null `string HashDiff` |
| Load timestamp technical column | non-null `DateTimeOffset LoadTimestamp` normalized to UTC by the existing read pipeline |
| Record source technical column | non-null `string RecordSource` |
| Payload columns in metadata order | `string` or `string?` properties named from produced payload property names, according to authoritative CLR or EF nullability |

The row type must also expose exact binding constants for the produced table name and every produced column name used by the generated projection. The generated projector must read values by the exact mapped names already exposed by `DataVaultSatelliteProjectionRow`: `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, driving-key names, and payload metadata names.

Unsupported satellite inputs are:

- missing produced table or column binding metadata.
- duplicate or colliding generated property names after deterministic identifier normalization.
- driving-key names that collide with technical projection names in the same generated row.
- payload or driving-key CLR types outside the currently translated provider-neutral string value boundary.
- stale or missing source fingerprints when generation is configured to validate a fingerprint.
- request shapes that require runtime-selected satellites, payload subsets, filter predicates, ordering, or caller-owned delegates.

## PIT Projection Contract

Supported PIT generation targets one `DataVaultPitMetadata` declaration, explicit parent hash keys, and an `asOf` timestamp. It must follow the bounded PIT read contract already documented for `IDataVaultReadService`.

Generated PIT helpers support:

- hub-parent PITs over ordinary satellites.
- hub-parent PITs over multi-active satellites only when all referenced multi-active satellites share one canonical driving-key name/order family.
- bounded link-parent PITs when every referenced satellite is unique, non-multi-active, and attached to the same declared link parent.

Generated PIT helpers do not support:

- model-first link-parent PIT artifacts.
- registry-backed PIT as-of read requests.
- link-parent multi-active PITs.
- incompatible multi-active driving-key families.
- duplicate satellite references.
- tuple filter parameters, cross-product tuple semantics, or caller-provided tuple predicates.
- missing parent declarations, missing satellite declarations, or satellites attached to a different parent.
- PIT metadata whose driving-key names collide with `ParentHashKey` or `LoadTimestamp`.

Each generated PIT row type contains:

| Property source | Generated property |
| --- | --- |
| PIT parent hash key | non-null `string ParentHashKey` or the normalized produced parent hash-key column name when it differs from `ParentHashKey` |
| PIT driving keys, when present | non-null `string` properties named from canonical driving-key produced names |
| PIT load timestamp | non-null `DateTimeOffset LoadTimestamp` normalized to UTC |
| Satellite segments in PIT declaration order | nullable segment properties named from satellite metadata names |

Each generated PIT satellite segment type contains:

| Segment source | Generated property |
| --- | --- |
| Snapshot load timestamp | non-null `DateTimeOffset SnapshotLoadTimestamp` when the segment is present |
| Hash diff | non-null `string HashDiff` when the segment is present |
| Record source | non-null `string RecordSource` when the segment is present |
| Payload columns | `string` or `string?` properties according to authoritative CLR or EF nullability |

Generated segment properties on the parent PIT row are nullable because a PIT row may have no materialized satellite snapshot for one declared segment. Within a non-null segment, required technical values use the existing `DataVaultPitSatelliteProjectionRow` required accessors.

The row type must preserve exact produced PIT table and PIT snapshot-reference column names. Snapshot-reference columns bind by `PropertyRole=SnapshotReference`, `TechnicalColumnRole=LoadTimestamp`, and `MetadataName` equal to the declared satellite name. Segment payloads bind by the referenced satellite payload metadata and produced property names, not by newly invented PIT payload names.

## Bridge Projection Contract

Supported bridge generation targets one `DataVaultBridgeMetadata` declaration and endpoint hash keys. It consumes already-maintained bridge rows and follows the bounded bridge read contract.

Generated bridge helpers support:

- many-to-many bridge traversal from `From` or `To` endpoint hash keys.
- hierarchy bridge traversal from `Ancestor` or `Descendant` endpoint hash keys with a required bounded `maximumDepth`.
- endpoint hash-key columns in deterministic generated endpoint column order.
- hierarchy `TraversalDepth` as an `int` property.

Generated bridge helpers do not support:

- bridge projection features beyond the baseline endpoint hash-key columns and hierarchy `TraversalDepth`.
- effectivity windows, path payload columns, closure-state columns, or generated relationship graph metadata.
- unbounded hierarchy traversal.
- graph traversal APIs beyond the explicit bridge helper methods.
- provider-specific bridge SQL or provider-specific maintenance strategies.
- ambiguous link participants whose endpoint roles cannot be resolved from hub references or explicit participant ordinals.

Each generated bridge row type contains:

| Property source | Generated property |
| --- | --- |
| Endpoint hash-key columns | non-null `string` properties named from produced endpoint column names, such as `OrderHashKey`, `AncestorSalesRegionHashKey`, or `DescendantSalesRegionHashKey` |
| Hierarchy depth | non-null `int TraversalDepth` for hierarchy bridges only |

The row type must preserve exact produced bridge table names, endpoint roles, endpoint source names, endpoint produced column names, and traversal-depth binding. The generated projector reads endpoint values by exact generated column names and reads hierarchy depth by `TraversalDepth`.

## Nullability And Type Mapping

Generated row properties must reflect authoritative CLR or EF nullability after DVault metadata projection:

- Technical hash keys, hash diffs, load timestamps, record sources, bridge endpoint hash keys, bridge traversal depth, and driving keys are non-null in generated rows.
- Satellite payload properties are nullable only when the authoritative metadata marks the corresponding payload property nullable.
- PIT satellite segment properties on a PIT row are nullable because the segment may be absent.
- Values inside a present PIT segment use the referenced satellite payload nullability.
- Provider storage format does not change generated CLR property types; it is consumed by existing read pipelines and diagnostics.

When nullability cannot be resolved from authoritative CLR or EF metadata, generation must choose the conservative nullable shape for payload properties and report an informational diagnostic. It must not make technical columns nullable unless the metadata shape is unsupported and generation for that shape is skipped.

## Diagnostics

The typed read model generator reserves the `DMV1960` through `DMV1969` diagnostic range in the analyzer package `SourceGeneration` category.

| Diagnostic | Severity | Meaning |
| --- | --- | --- |
| `DMV1960` | Error | The authoritative DVault metadata source cannot be resolved, more than one source is visible for one generated scope, or required `MetadataSourceKind` metadata is missing. |
| `DMV1961` | Error | The authoritative `MetadataSourceFingerprint` differs from the fingerprint embedded in generated inputs or generated helper metadata. |
| `DMV1962` | Error | A satellite read model shape is unsupported because required produced bindings, driving-key bindings, payload bindings, or deterministic generated identifiers cannot be resolved. |
| `DMV1963` | Error | A PIT read model shape is unsupported by the bounded v1 PIT baseline. |
| `DMV1964` | Error | A bridge read model shape is unsupported by the bounded v1 bridge baseline. |
| `DMV1965` | Error | A generated type, method, or property name collides after deterministic normalization and cannot be resolved without changing the public API shape. |
| `DMV1966` | Warning | Payload nullability could not be proven from authoritative CLR or EF metadata, so the generated payload property was emitted nullable. |
| `DMV1967` | Error | A requested generated helper would require dynamic runtime query construction, provider-specific SQL, runtime-selected projections, or unbounded traversal. |
| `DMV1968` | Error | A model-first `dvault.model.v1` input declares a shape outside the public artifact contract, including model-first link-parent PIT artifacts. |
| `DMV1969` | Info | A stable generated helper was skipped because the target metadata shape is valid for runtime `IDataVaultReadService` usage but outside the v1 generated helper contract. |

Diagnostics must identify the metadata source kind, metadata source fingerprint when available, logical metadata name, produced entity name, produced property name when relevant, and the unsupported shape family. Diagnostics must not include raw hash-key values, as-of timestamps, generated SQL, provider query plans, or connection details.

Existing runtime diagnostics remain responsible for read-strategy evidence, read-shape facts, provider fallback caveats, migration guardrails, model drift, and live-schema drift. The generator diagnostics above do not replace `IDataVaultReadDiagnosticsService`, `DataVaultModelDriftReporter`, or EF translation exceptions.

## Consumer Tickets

Ticket 06F5Q92AHG0ZCTVQGC6NAYVP9C implements the satellite slice of this contract:

- descriptor normalization needed by latest/current/as-of satellite helper generation.
- satellite row naming, payload nullability, driving-key projection, and stale fingerprint diagnostics.
- generated wrappers over the existing latest-satellite read path or stable direct EF projection path.

Ticket 06F5Q92R02HB7FCE1AWKXPTMRW implements the PIT and bridge slice of this contract:

- bounded PIT descriptor validation and generated PIT as-of helpers.
- bounded bridge descriptor validation and generated bridge endpoint helpers.
- PIT and bridge diagnostics for unsupported baselines, endpoint roles, hierarchy depth, and stale fingerprints.

Neither child ticket should redefine supported read-shape families, naming rules, source normalization, fingerprint behavior, or provider-specific non-goals.

## Evidence References

This contract depends on existing repository decisions:

- [DVault V1 PIT And Bridge Boundary](../architecture/dvault-v1-pit-bridge-boundary.md) for bounded PIT and bridge maintenance/read behavior, unsupported PIT/bridge cases, and provider-neutral diagnostics.
- [DVault EF Compiled Compatibility](../architecture/dvault-ef-compiled-compatibility.md) for compiled-model and stable compiled-query boundaries.
- [DVault Dotnet EF Design-Time Workflow](../architecture/dvault-dotnet-ef-design-time-workflow.md) for consumer-owned design-time model construction, support-bundle export, and drift preflight.
- [Model-First Governance Workflow](../model-first-governance.md) for the `dvault.model.v1` input contract and model-first projection boundary.
- `DataVaultAnnotationNames` for the metadata annotations that anchor generated naming, property roles, provider logical property kinds, source kind, and source fingerprint.
- `DataVaultReadServiceCurrentSatelliteExtensions`, `DataVaultReadServicePitExtensions`, and `DataVaultReadServiceBridgeExtensions` for current/as-of satellite, PIT, and bridge typed projection semantics.
