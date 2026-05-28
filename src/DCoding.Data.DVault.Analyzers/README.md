# DCoding.Data.DVault.Analyzers

Roslyn analyzers and source generators for DVault compile-time metadata declarations. The v1 package reports:

- `DMV1901` for unsupported `BusinessKey(...)`, `Payload(...)`, or `DrivingKey(...)` selector shapes.
- `DMV1902` for duplicate logical member declarations inside the same applicable Code-First builder lambda scope.
- `DMV1910` for exposing DVault generated shared-type tables as `DbSet<Dictionary<string, object>>` members on a `DbContext`.
- `DMV1911` for direct EF write calls against DVault generated shared-type `DbSet<Dictionary<string, object>>` sets.
- `DMV1950` through `DMV1955` for malformed generated mapping declarations, missing generated row bindings, invalid source members, duplicate binding order or names, and repeated link participant hub names.
- `DMV1960` through `DMV1969` for typed read-model generator metadata-source, fingerprint, unsupported-shape, deterministic-name, nullability-fallback, and skipped-helper outcomes.

The package also provides bounded code fixes for DMV1901 anonymous-object direct-member expansion and DMV1902 later-duplicate removal. Its source generator emits registry-backed typed row mappers from the public `DCoding.Data.DVault` compile-time mapping attributes; generated helpers still require callers to supply load timestamps and record sources through the existing explicit save flow.

## Installation

Install the analyzer package in projects that declare DVault Code-First metadata or compile-time generated row mappings through normal Roslyn analyzer package conventions:

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="0.21.0" PrivateAssets="all" />
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

## Generated Mapper Scope

The source generator recognizes mapping declarations from `DCoding.Data.DVault` runtime attributes on one source type:

- `DataVaultHubMappingAttribute` plus ordered `DataVaultBusinessKeyBindingAttribute` values.
- `DataVaultLinkMappingAttribute` plus ordered `DataVaultLinkParticipantBindingAttribute` values whose participant hub names are unique by `StringComparer.Ordinal`.
- `DataVaultHubSatelliteMappingAttribute` plus parent hash-key, hash-diff, ordered payload, and optional ordered driving-key bindings.

Generated code implements the existing `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, or `IDataVaultSatelliteMapper<TSource>` contracts and constructs `DataVaultRegistry*SaveOperation` values. It does not execute EF models, register mappings at runtime, derive hash keys or hash diffs, or hide the caller-supplied `loadTimestamp` and `recordSource` boundary.

## Typed Satellite Read-Model Generator Scope

The typed read-model source generator emits satellite-only latest/current/as-of helpers from one authoritative `dvault.support-bundle.v1` JSON additional file. Enable it explicitly in the owning project with `DVaultGenerateTypedReadModels=true`; this keeps application and test projects that contain multiple sample metadata models from accidentally generating colliding public helpers. The support bundle must be produced from runtime diagnostics after Code-First, metadata-first, or model-first declarations have been projected into EF/DVault metadata. Generated helpers use its `diagnostics.explain` metadata source kind, fingerprint, produced entity/property names, parent references, property roles, provider logical/value metadata, ordinals, CLR type names, and EF nullability. Source-visible Code-First callbacks, literal metadata-first `DataVaultMetadataModel` satellite declarations, and raw `dvault.model.v1` JSON additional files are not parsed directly by this generator because they have not yet passed through the shared EF/DVault projection descriptor.

Generated rows are emitted under `{RootNamespace}.DVault.GeneratedReadModels` when MSBuild supplies `RootNamespace`, otherwise under `DVault.GeneratedReadModels`. For each supported satellite, the generator emits `{SatelliteProducedName}ReadModel` and `{SatelliteProducedName}ReadExtensions` with `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` methods over `IDataVaultReadService`. The helpers construct stable `DataVaultSatelliteMetadata` and `DataVaultLatestSatelliteReadRequest` values and project through the existing `DataVaultSatelliteProjectionRow` exact-name accessors.

The v1 generator supports hub-parent, link-parent, and deterministic multi-active satellites whose driving keys and payload values are strings after projection into the support-bundle explain descriptor. Payload nullability follows the projected CLR/EF nullability facts; when a payload descriptor omits nullability, the generated property is nullable and `DMV1966` is reported. Generation stops with `DMV196x` diagnostics for missing or ambiguous support-bundle metadata sources, stale configured fingerprints, unsupported non-string members, deterministic name collisions, and helper requests that would require dynamic query construction or provider-specific SQL.

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
```

For MSBuild-level suppression, append the diagnostic ids to `NoWarn`:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);DMV1901;DMV1902;DMV1910;DMV1911</NoWarn>
</PropertyGroup>
```
