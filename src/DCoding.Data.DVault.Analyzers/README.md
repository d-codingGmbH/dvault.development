# DCoding.Data.DVault.Analyzers

Roslyn analyzers and source generators for DVault compile-time metadata declarations. The v1 package reports:

- `DMV1901` for unsupported `BusinessKey(...)`, `Payload(...)`, or `DrivingKey(...)` selector shapes.
- `DMV1902` for duplicate logical member declarations inside the same applicable Code-First builder lambda scope.
- `DMV1950` through `DMV1955` for malformed generated mapping declarations, missing generated row bindings, invalid source members, duplicate binding order or names, and repeated link participant hub names.

The package also provides bounded code fixes for DMV1901 anonymous-object direct-member expansion and DMV1902 later-duplicate removal. Its source generator emits registry-backed typed row mappers from the public `DCoding.Data.DVault` compile-time mapping attributes; generated helpers still require callers to supply load timestamps and record sources through the existing explicit save flow.

## Installation

Install the analyzer package in projects that declare DVault Code-First metadata or compile-time generated row mappings through normal Roslyn analyzer package conventions:

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="0.12.0" PrivateAssets="all" />
</ItemGroup>
```

`PrivateAssets="all"` keeps the analyzer local to the project that owns the declarations. The package supplies analyzer assets and does not require a runtime reference from application code.

## Code-First Analyzer Scope

The analyzer slice inspects the first direct lambda selector passed to `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)`. It expects one direct readable scalar member on the configured entity type, such as `hub.BusinessKey(customer => customer.CustomerId)` or `satellite.Payload(customer => customer.EmailAddress)`.

Composite business keys, payloads, and driving keys should use repeated single-member calls in their canonical order. When an anonymous-object selector contains only direct readable scalar member accesses, the DMV1901 code fix can expand it into repeated same-verb single-member calls in source order. The DMV1901 code fix is intentionally not offered for selector variables, indirect selectors, computed selectors, nested selectors, method calls, collection-valued selectors, or mixed anonymous-object selectors that are not mechanically expandable.

The DMV1902 code fix removes the later duplicate `BusinessKey(...)`, `Payload(...)`, or `DrivingKey(...)` declaration and leaves the first declaration authoritative within the same fluent scope.

## Generated Mapper Scope

The source generator recognizes mapping declarations from `DCoding.Data.DVault` runtime attributes on one source type:

- `DataVaultHubMappingAttribute` plus ordered `DataVaultBusinessKeyBindingAttribute` values.
- `DataVaultLinkMappingAttribute` plus ordered `DataVaultLinkParticipantBindingAttribute` values whose participant hub names are unique by `StringComparer.Ordinal`.
- `DataVaultHubSatelliteMappingAttribute` plus parent hash-key, hash-diff, ordered payload, and optional ordered driving-key bindings.

Generated code implements the existing `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, or `IDataVaultSatelliteMapper<TSource>` contracts and constructs `DataVaultRegistry*SaveOperation` values. It does not execute EF models, register mappings at runtime, derive hash keys or hash diffs, or hide the caller-supplied `loadTimestamp` and `recordSource` boundary.

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
```

For MSBuild-level suppression, append the diagnostic ids to `NoWarn`:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);DMV1901;DMV1902</NoWarn>
</PropertyGroup>
```
