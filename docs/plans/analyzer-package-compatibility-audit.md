# Analyzer Package Compatibility Audit

Ticket: `06FGX5GHPS7DEC3EJPWSKJZH28`
Status: superseded by `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` and ticket `06FH8R4EF1QFF2E3ZWS3P1BWHM`

## Current Decision

`DCoding.Data.DVault.Analyzers` uses one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/` for both visible package lines. Analyzer references stay local with `PrivateAssets="all"`, and supported analyzer consumption covers `.NET 8 SDK` and `.NET 10 SDK` build hosts.

The package still has one package id and one analyzer asset root. It does not introduce target-specific analyzer asset trees, a second analyzer package id, a split code-fix package, or runtime `lib/<tfm>` assets for the analyzer package.

## Implemented Resolution

- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets `netstandard2.0`.
- Roslyn, Workspaces, `System.Composition`, and `System.Text.Json` references are package-managed rather than resolved from SDK-local `dotnet-format` paths.
- The analyzer package packs the primary analyzer DLL, XML documentation, and approved companion assemblies under `analyzers/dotnet/cs/`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` consumes the analyzer project without forcing an old target-framework override.
- `tools/run-analyzer-package-smoke.sh` restores, builds, and runs a temporary generated-mapper consumer against the packed runtime and analyzer packages on a selected SDK host.
- CI installs both SDK hosts and runs the package smoke for `8` and `10` after packing release package lines.
- Package verification checks the dual-host README statement, analyzer asset path, companion assemblies, and the `netstandard2.0` target marker on the analyzer DLL.

## Dependency Surface By Slice

| Slice | Source files | Normalized dependencies | Notes |
| --- | --- | --- | --- |
| Diagnostic analyzers | `DataVaultCodeFirstAnalyzer.cs`, `DataVaultEfCoreMisuseAnalyzer.cs`, diagnostic catalog files | `Microsoft.CodeAnalysis.CSharp` | Diagnostic analyzers use Roslyn syntax, semantic, diagnostics, and operation APIs. |
| Mapping source generator | `DataVaultMappingSourceGenerator.cs`, `DataVaultMappingDiagnosticCatalog.cs` | `Microsoft.CodeAnalysis.CSharp` | The generator uses incremental source-generator APIs and emits mapper helpers for public runtime mapping attributes. |
| Typed read-model source generator | `DataVaultTypedReadModelSourceGenerator.cs`, `DataVaultTypedReadModelDiagnosticCatalog.cs` | `Microsoft.CodeAnalysis.CSharp`, `System.Text.Json` | JSON support is explicit for the lowered analyzer target instead of relying on a newer framework target. |
| Code-fix provider | `DataVaultCodeFirstCodeFixProvider.cs` | `Microsoft.CodeAnalysis.CSharp.Workspaces`, `System.Composition.*` | The code-fix provider remains in the same package and uses package-managed Workspaces and MEF composition dependencies. |
| Analyzer/code-fix tests | `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj` | Matching package-managed Roslyn, Workspaces, and `System.Composition.*` references | The test harness no longer depends on SDK-local `dotnet-format` assembly paths. |

## Validation Contract

The current validation evidence for analyzer package compatibility is:

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
bash tools/pack-release-packages.sh
bash tools/run-analyzer-package-smoke.sh 8
bash tools/run-analyzer-package-smoke.sh 10
bash tools/verify-packages.sh
bash tools/check-format.sh
```

The smoke script intentionally uses packed packages rather than an in-solution project-reference shortcut. This proves the consumer restore/build/run path that release packages actually expose.
