# DCoding.Data.DVault.Analyzers

Roslyn analyzers for DVault Code-First metadata declarations. The v1 package reports:

- `DMV1901` for unsupported `BusinessKey(...)`, `Payload(...)`, or `DrivingKey(...)` selector shapes.
- `DMV1902` for duplicate logical member declarations inside the same applicable Code-First builder lambda scope.

## Installation

Install the analyzer package in projects that declare DVault Code-First metadata through normal Roslyn analyzer package conventions:

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="0.9.0" PrivateAssets="all" />
</ItemGroup>
```

`PrivateAssets="all"` keeps the analyzer local to the project that owns the fluent declarations. The package supplies analyzer assets and does not require a runtime reference from application code.

## Scope

The current analyzer slice inspects the first direct lambda selector passed to `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)`. It expects one direct readable scalar member on the configured entity type, such as `hub.BusinessKey(customer => customer.CustomerId)` or `satellite.Payload(customer => customer.EmailAddress)`.

Composite business keys, payloads, and driving keys should use repeated single-member calls in their canonical order. Selector variables, indirect selectors, computed selectors, nested selectors, method calls, anonymous objects, and collection-valued selectors are intentionally outside the first direct-lambda slice unless a diagnostic can be reported with high confidence.

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
