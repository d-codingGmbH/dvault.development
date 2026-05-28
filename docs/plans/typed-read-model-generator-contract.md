# Typed Read Model Generator Contract

Status: superseded historical planning context
Ticket: 06F5Q922T5B21GJN49FYN6DJH0
Current boundary: v0.22.0 satellite-only typed read helpers

## Supersession

This document preserves the historical typed-read generator planning path and records the shipped v0.22.0 boundary. It is not an active promise to emit PIT or bridge typed helpers.

The authoritative v0.22.0 contract is the coordinated satellite-only generator and hash-governance baseline in:

- [DVault v0.22.0 Release Notes](../releases/v0.22.0.md).
- [DCoding.Data.DVault.Analyzers README](../../src/DCoding.Data.DVault.Analyzers/README.md).
- [Model-First Governance Workflow](../model-first-governance.md).
- [DataVaultTypedReadModelSourceGeneratorTests.cs](../../tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs).
- [Stable Hashing Contract](stable-hashing-contract.md).

The earlier version of this planning document described generated PIT and bridge helper emission. Epic 06F5Q91V0YGSA6SH9WDS02GH0M explicitly supersedes that design for the shipped v0.22.0 boundary. PIT and bridge reads remain runtime `IDataVaultReadService` and diagnostics surfaces until a separate additive ticket changes that public contract.

## Current Generated-Helper Boundary

Typed read-model generation is opt-in with `DVaultGenerateTypedReadModels=true`. The generator consumes exactly one authoritative `dvault.support-bundle.v1` additional file exported after Code-First, metadata-first, or model-first declarations have been projected into EF and DVault metadata. A consuming project may pin `DVaultTypedReadModelMetadataSourceFingerprint` when reviewed generator input must fail on metadata-source drift.

The generator does not parse raw `dvault.model.v1` additional files, source-visible Code-First callbacks, or literal metadata-first `DataVaultMetadataModel` declarations directly. Those inputs must first pass through the shared EF/DVault projection descriptor and be represented in the authoritative support bundle.

Generated helpers are limited to stable satellite shapes:

- hub-parent satellites.
- link-parent satellites.
- deterministic multi-active satellites whose driving keys and payload values are strings after projection.

For each supported satellite, the generated surface includes `{SatelliteProducedName}ReadModel` plus `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` extension methods over `IDataVaultReadService`. The helpers are ergonomics over the existing provider-neutral read service and projected support-bundle metadata.

Because that generated surface delegates to the existing read-service metadata request path, the shipped v0.22.0 helper contract targets the v1 default or read-service-compatible produced table and column shape represented by the support bundle. Custom produced-name read binding remains a future additive contract; projects that intentionally override produced names should use dynamic `IDataVaultReadService` requests or consumer-owned compiled EF queries until that contract lands.

## Explicit Non-Goals

The v0.22.0 generator does not emit:

- PIT typed helpers.
- bridge typed helpers.
- provider-specific SQL, query hints, migration operations, or provider-specific performance claims.
- dynamic read-request compilation or caller-owned projector compilation.
- automatic support-bundle routing, publication, storage, attachment, or approval workflow.
- raw `dvault.model.v1` parser behavior inside the generator.
- automatic satellite hashDiff generation or changes to `sha256-v1` hash semantics.

PIT and bridge shapes must use existing runtime read surfaces or surface as `DMV196x` diagnostics when they appear in generator input. Consumer-owned compiled EF queries remain the documented direct-query alternative for fixed shapes; generated helper emission does not expand that runtime boundary.

## Descriptor And Naming Requirements

The support-bundle explain descriptor is an implementation-internal generator model, not a new public artifact format. It must preserve the metadata-source kind and fingerprint, produced entity and property names, parent references, property roles, provider logical/value metadata, ordinals, CLR type names, and EF nullability needed to generate exact satellite bindings.

Generated public API names are derived from produced DVault names, not from physical provider identifiers after provider length truncation. Exact produced names must also be preserved in generated constants or internal binding tables so diagnostics and tests can verify the mapping.

Generated rows are emitted under `{RootNamespace}.DVault.GeneratedReadModels` when MSBuild supplies `RootNamespace`, otherwise under `DVault.GeneratedReadModels`. Implementations may add internal helper types in the same namespace, but the public row and extension names remain satellite-specific.

## Satellite Projection Contract

Each supported satellite row contains:

| Property source | Generated property |
| --- | --- |
| Parent hash-key technical column | non-null `string ParentHashKey` or the normalized produced parent hash-key column name when it differs from `ParentHashKey` |
| Driving keys in metadata order | non-null `string` properties named from produced driving-key property names |
| Hash diff technical column | non-null `string HashDiff` |
| Load timestamp technical column | non-null `DateTimeOffset LoadTimestamp` normalized to UTC by the existing read pipeline |
| Record source technical column | non-null `string RecordSource` |
| Payload columns in metadata order | `string` or `string?` properties named from produced payload property names, according to authoritative CLR or EF nullability |

The row type must expose exact binding constants for the produced satellite table name and every produced column name used by the generated projection. The generated projector reads values by the exact mapped names already exposed by `DataVaultSatelliteProjectionRow`: `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, driving-key names, and payload metadata names.

Unsupported satellite inputs include missing produced table or column binding metadata, duplicate or colliding generated property names after deterministic identifier normalization, driving-key names that collide with technical projection names in the same generated row, non-string driving-key or payload values, stale or missing configured source fingerprints, and request shapes that require runtime-selected satellites, payload subsets, filter predicates, ordering, provider SQL, or caller-owned delegates.

## Generated Method Shape

Generated extension methods are static extension methods over `IDataVaultReadService`. They keep the caller-owned `DbContext`, hash-key request values, optional as-of timestamp, and cancellation token boundary explicit.

Satellite helper methods follow this shape:

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

`Current` and `Latest` are equivalent convenience names over `DataVaultLatestSatelliteReadRequest` with no `asOf` value. `AsOf` passes an inclusive UTC cutoff through that same runtime request boundary.

## Diagnostics

The typed read-model generator reserves the `DMV1960` through `DMV1969` diagnostic range in the analyzer package `SourceGeneration` category.

| Diagnostic | Outcome |
| --- | --- |
| `DMV1960` | Missing, invalid, non-authoritative, or ambiguous `dvault.support-bundle.v1` metadata source. |
| `DMV1961` | Configured `DVaultTypedReadModelMetadataSourceFingerprint` does not match the resolved support-bundle metadata-source fingerprint. |
| `DMV1962` | Satellite shape cannot be generated, including missing bindings, non-string driving-key or payload members, and reserved generated projection-name collisions. |
| `DMV1963` | PIT metadata appears in the support bundle; the generator reports it as unsupported for helper emission. |
| `DMV1964` | Bridge metadata appears in the support bundle; the generator reports it as unsupported for helper emission. |
| `DMV1965` | Deterministic generated type, method, property, or helper name collision. |
| `DMV1966` | Payload nullability cannot be proven from the support-bundle descriptor, so the generated payload property falls back to nullable. |
| `DMV1967` | The shape would require dynamic runtime query construction, provider SQL, runtime projection selection, or unbounded traversal. |
| `DMV1968` | A model-first source appears in the projected support-bundle evidence but is outside the generator's helper contract. |
| `DMV1969` | A valid runtime metadata shape is skipped because it is outside the v1 generated-helper boundary. |

Diagnostics must identify the metadata source kind, metadata source fingerprint when available, logical metadata name, produced entity name, produced property name when relevant, and the unsupported shape family. Diagnostics must not include raw hash-key values, as-of timestamps, generated SQL, provider query plans, or connection details.

## Historical Planning Notes

Child ticket 06F5Q92AHG0ZCTVQGC6NAYVP9C landed the satellite helper implementation slice for v0.22.0. The prior PIT and bridge helper planning text associated with child ticket 06F5Q92R02HB7FCE1AWKXPTMRW is historical context only for this epic and is not part of the shipped v0.22.0 generated-helper contract.

If future product work requires generated PIT or bridge helpers, it should define a new additive contract and implementation ticket rather than reopening this superseded planning scope.
