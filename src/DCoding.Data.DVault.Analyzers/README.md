# DCoding.Data.DVault.Analyzers

Roslyn analyzers and source generators for DVault compile-time metadata declarations. The v1 package reports:

- `DMV1901` for unsupported `BusinessKey(...)`, `Payload(...)`, or `DrivingKey(...)` selector shapes.
- `DMV1902` for duplicate logical member declarations inside the same applicable Code-First builder lambda scope.
- `DMV1910` for exposing DVault generated shared-type tables as `DbSet<Dictionary<string, object>>` members on a `DbContext`.
- `DMV1911` for direct EF write calls against DVault generated shared-type `DbSet<Dictionary<string, object>>` sets.
- `DMV1912` for source-visible caller-owned DVault model-shape variation whose visible model-cache key omits the varying discriminator.
- `DMV1913` for source-visible `UseModel(...)` compiled-model selection on variable-shape DVault contexts.
- `DMV1914` for direct `AddDbContextPool<TContext>(...)` registration of variable-shape DVault contexts.
- `DMV1950` through `DMV1955` for malformed generated mapping declarations, missing generated row bindings, invalid source members, duplicate binding order or names, and repeated link participant hub names.
- `DMV1960` through `DMV1969` for typed read-model generator metadata-source, fingerprint, unsupported-shape, deterministic-name, nullability-fallback, and skipped-helper outcomes.

The package also provides bounded code fixes for DMV1901 anonymous-object direct-member expansion and DMV1902 later-duplicate removal. Its mapping source generator emits registry-backed typed row mappers from the public `DCoding.Data.DVault` compile-time mapping attributes; generated save helpers still require callers to supply load timestamps and record sources through the existing explicit save flow. Its typed read-model source generator is a separate opt-in support-bundle-driven surface for satellite latest/current/as-of helpers, PIT as-of helpers, and bounded bridge traversal helpers.

The lifecycle diagnostics `DMV1912` through `DMV1914` are high-confidence EF Core misuse diagnostics for direct source-visible evidence only. They align with the root README section "Isolate EF model cache entries" and with `docs/architecture/dvault-ef-compiled-compatibility.md`; they do not add runtime guards, runtime behavior changes, compiled-model generation, provider-specific lifecycle guarantees, cross-assembly inference, or whole-application inference.

## Installation

Install the analyzer package in projects that declare DVault Code-First metadata, compile-time generated row mappings, or support-bundle-driven typed satellite, PIT, and bridge read helpers through normal Roslyn analyzer package conventions. Use the same already-published version as the rest of the coordinated DVault package family; this documentation baseline does not by itself confirm package publication.

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="0.29.0" PrivateAssets="all" />
</ItemGroup>
```

`PrivateAssets="all"` keeps the analyzer local to the project that owns the declarations. The package supplies analyzer assets and does not require a runtime reference from application code.

## Code-First Analyzer Scope

The analyzer slice inspects the first direct lambda selector passed to `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)`. It expects one direct readable scalar member on the configured entity type, such as `hub.BusinessKey(customer => customer.CustomerId)` or `satellite.Payload(customer => customer.EmailAddress)`.

Composite business keys, payloads, and driving keys should use repeated single-member calls in their canonical order. When an anonymous-object selector contains only direct readable scalar member accesses, the DMV1901 code fix can expand it into repeated same-verb single-member calls in source order. The DMV1901 code fix is intentionally not offered for selector variables, indirect selectors, computed selectors, nested selectors, method calls, collection-valued selectors, or mixed anonymous-object selectors that are not mechanically expandable.

The DMV1902 code fix removes the later duplicate `BusinessKey(...)`, `Payload(...)`, or `DrivingKey(...)` declaration and leaves the first declaration authoritative within the same fluent scope.

## EF Core Misuse Analyzer Scope

The EF Core misuse analyzer keeps the generated DVault table boundary explicit. Generated hub, link, and satellite tables are EF shared-type entities; applications should not publish them as `DbSet<Dictionary<string, object>>` members on their `DbContext`. Use `IDataVaultSaveService` for writes and keep generated table access behind explicit `context.Set<Dictionary<string, object>>(producedName)` query shapes.

`DMV1910` reports exposed `DbContext` properties or fields whose type is `DbSet<Dictionary<string, object>>` when the member source visibly resolves a DVault generated table name through `Set<Dictionary<string, object>>(producedName)`. `DMV1911` reports direct mutating calls such as `Add(...)`, `AddRange(...)`, `Update(...)`, `Remove(...)`, or `Attach(...)` on source-visible generated shared-type sets. The rule intentionally does not report arbitrary non-DVault dictionary shared-type tables, documented read-only query shapes over `context.Set<Dictionary<string, object>>(producedName)`, including `AsNoTracking()` and compiled-query projections, or a local source scope that visibly opts into `UseDataVaultSaveChangesMetadataInterceptor(...)`.

The analyzer does not attempt whole-application DI inference and does not treat `UseDataVaultSaveChangesMetadataInterceptor(...)` as a replacement for the explicit save boundary. That interceptor remains an opt-in metadata filler for tracked generated rows; ordinary hub, link, and satellite writes should flow through `IDataVaultSaveService`. The runtime `UseDataVaultSaveChangesGuardInterceptor(...)` is a separate opt-in blocking or warning guard for applications that want SaveChanges-time enforcement; it is not enabled by `AddDVault()` and does not broaden the analyzer into arbitrary dataflow or provider-specific SQL analysis.

`DMV1912` reports when direct `ApplyDataVaultMetadata(...)` model projection visibly varies by caller-owned context state or branches and the directly visible `IModelCacheKeyFactory` path omits that same discriminator. Registry-backed `UseDataVaultMetadata()`, `UseDataVaultMetadata(DataVaultMetadataModel)`, `UseDataVaultMetadata(DataVaultMetadataRegistry)`, and `UseDataVaultMetadata(DataVaultModelImportResult)` are non-diagnostic because DVault carries the metadata-source kind and deterministic metadata fingerprint into EF model-cache isolation.

`DMV1913` reports direct `UseModel(...)` compiled-model selection when the same source visibly applies that runtime model to a DVault context whose realized model shape can vary. Fixed-shape contexts and the documented design-model-to-runtime-model lane remain non-diagnostic because they select one realized DVault model shape. Read-only EF compiled queries over generated shared-type tables remain non-diagnostic because they compile a stable query expression rather than selecting an EF compiled model for a context.

`DMV1914` reports direct `AddDbContextPool<TContext>(...)` registration when the target DVault context visibly has model-shape variation. Options-only pooled contexts with one fixed metadata source, provider configuration, naming, schema, and profile remain non-diagnostic. Pooled factories, helper-expanded registrations, and cross-assembly inference stay outside this analyzer slice.

The lifecycle diagnostics are intentionally limited to direct syntax and semantic facts in the analyzed source: visible instance members read in `OnModelCreating(...)`, direct branches around DVault model projection, direct `ReplaceService<IModelCacheKeyFactory, ...>()` paths, directly visible returned cache-key shapes, and direct `UseModel(...)` or `AddDbContextPool<TContext>(...)` registrations. When the source does not make both the variable model shape and unsafe lifecycle path visible, the analyzer skips instead of guessing.

## Generated Mapper Scope

The source generator recognizes mapping declarations from `DCoding.Data.DVault` runtime attributes on one source type:

- `DataVaultHubMappingAttribute` plus ordered `DataVaultBusinessKeyBindingAttribute` values.
- `DataVaultLinkMappingAttribute` plus ordered `DataVaultLinkParticipantBindingAttribute` values whose participant hub names are unique by `StringComparer.Ordinal`.
- `DataVaultHubSatelliteMappingAttribute` plus parent hash-key, hash-diff, ordered payload, and optional ordered driving-key bindings.

Generated code implements the existing `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, or `IDataVaultSatelliteMapper<TSource>` contracts and constructs `DataVaultRegistry*SaveOperation` values. It does not execute EF models, register mappings at runtime, derive hash keys or hash diffs, or hide the caller-supplied `loadTimestamp` and `recordSource` boundary.

## Typed Read-Model Generator Scope

The typed read-model source generator emits satellite latest/current/as-of helpers, bounded PIT as-of helpers, and bounded bridge traversal helpers from one authoritative `dvault.support-bundle.v1` JSON additional file. Enable it explicitly in the owning project with `DVaultGenerateTypedReadModels=true`; this keeps application and test projects that contain multiple sample metadata models from accidentally generating colliding public helpers. The support bundle must be produced from runtime diagnostics after Code-First, metadata-first, or model-first declarations have been projected into EF/DVault metadata. Generated satellite helpers use its `diagnostics.explain` metadata source kind, fingerprint, produced entity/property names, parent references, property roles, provider logical/value metadata, ordinals, CLR type names, and EF nullability. Generated PIT helpers additionally require request-bound `diagnostics.readShape.pit` facts for PIT identity, parent reference, referenced satellites, snapshot-reference columns, and canonical driving-key columns. Generated bridge helpers require request-bound `diagnostics.readShape.bridge` facts for bridge kind, endpoint vocabulary, endpoint columns, selected filter endpoint, deterministic ordering, and hierarchy depth projection when applicable. Source-visible Code-First callbacks, literal metadata-first `DataVaultMetadataModel` declarations, and raw `dvault.model.v1` JSON additional files are not parsed directly by this generator because they have not yet passed through the shared EF/DVault projection descriptor.

Generated rows are emitted under `{RootNamespace}.DVault.GeneratedReadModels` when MSBuild supplies `RootNamespace`, otherwise under `DVault.GeneratedReadModels`. For each supported satellite, the generator emits `{SatelliteProducedName}ReadModel` and `{SatelliteProducedName}ReadExtensions` with `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` methods over `IDataVaultReadService`. The helpers construct stable `DataVaultSatelliteMetadata` and `DataVaultLatestSatelliteReadRequest` values and project through the existing `DataVaultSatelliteProjectionRow` exact-name accessors.

For each supported PIT, the generator emits `{PitProducedName}ReadModel` and `{PitProducedName}ReadExtensions` with `Read...AsOfAsync` over `IDataVaultReadService`. The helper constructs stable `DataVaultPitMetadata` and `DataVaultPitAsOfReadRequest` values, delegates to `ReadPitRowsAsync`, and projects PIT-table columns only: `ParentHashKey`, `LoadTimestamp`, canonical PIT driving keys when the read shape proves one shared multi-active driving-key family, and nullable snapshot-reference timestamps for each included PIT segment.

For each supported bridge, the generator emits `{BridgeProducedName}ReadModel` and `{BridgeProducedName}ReadExtensions` over `IDataVaultReadService`. Many-to-many bridges emit `Read...FromAsync` and `Read...ToAsync` helpers. Hierarchy bridges emit `Read...AncestorAsync` and `Read...DescendantAsync` helpers that require an explicit `int maximumDepth`; the generator does not emit an unbounded hierarchy traversal overload. Bridge helpers construct stable `DataVaultBridgeMetadata` and `DataVaultBridgeReadRequest` values, delegate through the runtime read boundary, and project bridge-row endpoint columns plus `TraversalDepth` for hierarchy rows.

The v1 generator supports hub-parent, link-parent, and deterministic multi-active satellites whose driving keys and payload values are strings after projection into the support-bundle explain descriptor. Payload nullability follows the projected CLR/EF nullability facts; when a payload descriptor omits nullability, the generated property is nullable and `DMV1966` is reported. PIT helper emission is limited to hub-parent ordinary PITs, hub-parent PITs whose multi-active satellites share one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on one declared link parent. Bridge helper emission is limited to maintained many-to-many `From`/`To` traversal and maintained hierarchy `Ancestor`/`Descendant` traversal with required `maximumDepth`. Generation stops or skips helpers with `DMV196x` diagnostics for missing or ambiguous support-bundle metadata sources, stale configured fingerprints, unsupported non-string members, deterministic name collisions, helper requests that would require dynamic query construction or provider-specific SQL, unbounded traversal, and shapes outside the v1 generated-helper contract.

The support-bundle-driven satellite, PIT, and bridge helper contract is documented in [DVault V1 Typed PIT And Bridge Helper Contract](../../docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md).

### Typed Read-Model Diagnostics

| Code | Outcome |
| --- | --- |
| `DMV1960` | Missing, invalid, non-authoritative, or ambiguous `dvault.support-bundle.v1` metadata source. |
| `DMV1961` | Configured `DVaultTypedReadModelMetadataSourceFingerprint` does not match the resolved support-bundle metadata-source fingerprint. |
| `DMV1962` | Satellite shape cannot be generated, including non-string driving-key or payload members and reserved generated projection-name collisions. |
| `DMV1963` | PIT metadata or request-bound PIT read-shape evidence is incomplete or outside the bounded generated-helper baseline. |
| `DMV1964` | Bridge metadata or request-bound bridge read-shape evidence is incomplete or outside the bounded generated-helper baseline. |
| `DMV1965` | Deterministic generated type, method, property, or helper name collision. |
| `DMV1966` | Payload nullability cannot be proven from the support-bundle descriptor, so the generated payload property falls back to nullable. |
| `DMV1967` | The shape would require dynamic runtime query construction, provider SQL, runtime projection selection, or unbounded traversal. |
| `DMV1968` | A raw or residual model-first source appears outside the projected support-bundle helper contract. Complete model-first support bundles with request-bound `ReadShape` evidence can still emit supported helpers. |
| `DMV1969` | A valid runtime metadata shape is skipped because it is outside the v1 generated-helper boundary. |

## Suppression

Suppress DMV diagnostics only where the consuming project intentionally accepts a pattern outside this analyzer slice. Use standard C# and Roslyn analyzer suppression mechanisms.

For a narrow local exception, use a pragma around the intentional declaration:

```csharp
#pragma warning disable DMV1901
satellite.Payload(emailSelector);
#pragma warning restore DMV1901
```

For a project or path-level policy, configure analyzer severities in `.editorconfig`:

```ini
dotnet_diagnostic.DMV1901.severity = none
dotnet_diagnostic.DMV1902.severity = none
dotnet_diagnostic.DMV1910.severity = none
dotnet_diagnostic.DMV1911.severity = none
dotnet_diagnostic.DMV1912.severity = none
dotnet_diagnostic.DMV1913.severity = none
dotnet_diagnostic.DMV1914.severity = none
```

For MSBuild-level suppression, append the diagnostic ids to `NoWarn`:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);DMV1901;DMV1902;DMV1910;DMV1911;DMV1912;DMV1913;DMV1914</NoWarn>
</PropertyGroup>
```
