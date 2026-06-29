# Analyzer Package Compatibility Audit

Ticket: `06FGX5GHPS7DEC3EJPWSKJZH28`

## Decision

Keep `DCoding.Data.DVault.Analyzers` on one `net10.0` analyzer asset for the current v0.50.0 documentation compatibility baseline. Supported analyzer consumption remains a `.NET 10 SDK` build host for both visible package lines, including `net8.0` consumer projects on the `8.50.0` line.

Pure `.NET 8 SDK` analyzer consumption is a no-go for the current branch. The repository proves a `net8.0` consumer target compiled with the `net10.0` analyzer asset on the `.NET 10 SDK` host baseline; it does not prove that a `.NET 8 SDK` host can restore, load, or run the analyzer package.

If pure `.NET 8 SDK` consumption becomes required, raise a bounded implementation follow-up for analyzer target and asset strategy plus Roslyn reference normalization, followed by a validation and documentation follow-up for CI, package verification, release packaging, README, analyzer README, package compatibility, and local validation guidance.

## Project And Package Evidence

- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0`.
- The analyzer project sets `IncludeBuildOutput=false`, `SuppressDependenciesWhenPacking=true`, and `DevelopmentDependency=true`, so the package is a local build-time analyzer package and does not publish normal `lib/<tfm>` runtime assets or NuGet dependency groups for the analyzer's Roslyn references.
- The analyzer project references `Microsoft.CodeAnalysis` and `Microsoft.CodeAnalysis.CSharp` from `$(MSBuildToolsPath)`, then references `Microsoft.CodeAnalysis.Workspaces` and `System.Composition.AttributedModel` from `$(MSBuildToolsPath)/DotnetTools/dotnet-format`.
- The `AddAnalyzerPackageAssets` target packs `$(TargetPath)` and the generated XML documentation under `analyzers/dotnet/cs/`.
- `tools/pack-release-packages.sh` calls `pack_analyzer_line` once for `8.50.0` and once for `10.50.0`, but it does not pass a target framework override to the analyzer project. Both package lines therefore receive the same `net10.0` analyzer binary shape with different package versions.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` verifies the analyzer README host guidance and analyzer entries under `analyzers/dotnet/cs/`, including the analyzer DLL and XML documentation. It does not require a separate `net8.0` analyzer asset or a pure `.NET 8 SDK` host lane.

## Dependency Surface By Slice

| Slice | Source files | Required Roslyn or SDK-local assemblies | Notes |
| --- | --- | --- | --- |
| Diagnostic analyzers | `DataVaultCodeFirstAnalyzer.cs`, `DataVaultEfCoreMisuseAnalyzer.cs`, diagnostic catalog files | `Microsoft.CodeAnalysis`, `Microsoft.CodeAnalysis.CSharp` from `$(MSBuildToolsPath)` | Both concrete analyzers are `[DiagnosticAnalyzer(LanguageNames.CSharp)]` types. They use syntax, semantic, diagnostics, and operation APIs. They do not use Workspaces, code-fix APIs, or `System.Composition`. |
| Mapping source generator | `DataVaultMappingSourceGenerator.cs`, `DataVaultMappingDiagnosticCatalog.cs` | `Microsoft.CodeAnalysis`, `Microsoft.CodeAnalysis.CSharp` from `$(MSBuildToolsPath)` | The generator is an `IIncrementalGenerator` using `SyntaxProvider`, symbols, diagnostics, `SourceProductionContext`, and `SourceText`. It does not use Workspaces or `System.Composition`. |
| Typed read-model source generator | `DataVaultTypedReadModelSourceGenerator.cs`, `DataVaultTypedReadModelDiagnosticCatalog.cs` | `Microsoft.CodeAnalysis`, `Microsoft.CodeAnalysis.CSharp` from `$(MSBuildToolsPath)` plus framework-provided `System.Text.Json` under the current `net10.0` target | The generator is an `IIncrementalGenerator` using `AdditionalTextsProvider`, analyzer config options, `SourceProductionContext`, `SourceText`, and JSON parsing. A lower target would need explicit package or framework handling for `System.Text.Json` instead of relying on the current `net10.0` target. |
| Code-fix provider | `DataVaultCodeFirstCodeFixProvider.cs` | `Microsoft.CodeAnalysis`, `Microsoft.CodeAnalysis.CSharp`, `Microsoft.CodeAnalysis.Workspaces`, and `System.Composition.AttributedModel` from SDK-local paths | This is the only Workspaces/System.Composition-coupled production slice. It is an `[ExportCodeFixProvider]` and `[Shared]` `CodeFixProvider` that uses `CodeAction`, `CodeFixes`, `Document`, `Formatter`, and MEF composition attributes. |
| Analyzer/code-fix tests | `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj` | `Microsoft.CodeAnalysis`, `Microsoft.CodeAnalysis.CSharp`, `Microsoft.CodeAnalysis.Workspaces`, `Microsoft.CodeAnalysis.CSharp.Workspaces`, and multiple `System.Composition.*` assemblies from `$(MSBuildToolsPath)/DotnetTools/dotnet-format` | The test project targets only `net10.0`. Its extra Workspaces and composition references are test-only but prove the current code-fix test harness is also coupled to the SDK-local dotnet-format layout. |

## Pure .NET 8 SDK Blockers And Assumptions

- The shipped analyzer assembly is built for `net10.0`. A pure `.NET 8 SDK` host cannot be claimed to load that asset.
- Roslyn and Workspaces references are not NuGet-declared analyzer package dependencies. They are resolved from the local SDK's `$(MSBuildToolsPath)` and `DotnetTools/dotnet-format` directories during build and tests.
- `SuppressDependenciesWhenPacking=true` prevents the package from carrying dependency metadata that could normalize Roslyn, Workspaces, composition, or JSON dependencies for a lower target.
- The package contains one analyzer asset path under `analyzers/dotnet/cs/`; it does not carry target-specific analyzer binaries or a verifier-enforced selection rule for `.NET 8 SDK` hosts.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0`, but its analyzer `ProjectReference` uses `SetTargetFramework="TargetFramework=net10.0"`. That proves the current `net8.0` consumer lane still consumes the `net10.0` analyzer asset.
- `.github/workflows/ci.yml` installs `10.0.x` and `docs/local-validation.md` tells maintainers to run validation from a `.NET 10 SDK` checkout. There is no current `.NET 8 SDK` CI or package-verification lane.

## Viability Options

| Option | Status | Assessment |
| --- | --- | --- |
| Keep one `net10.0` analyzer asset with `.NET 10 SDK` host guidance | go | This is the only fully evidenced state on the current branch. Documentation, package verification, pack script behavior, tests, and CI are aligned to this baseline. |
| Retarget to one `netstandard2.0` analyzer asset | follow-up-required | This is the broadest plausible analyzer-host compatibility strategy, but it requires an implementation ticket. The project target must change, SDK-local Roslyn references must become explicit compatible package references or another reviewed reference strategy, `System.Text.Json` must be handled for the typed read-model generator, and the code-fix Workspaces/System.Composition dependency must be normalized or separated. |
| Retarget to one `net8.0` analyzer asset | follow-up-required | This could satisfy a pure `.NET 8 SDK` host claim only after retargeting and validation. It is narrower than `netstandard2.0` and still requires Roslyn reference normalization, code-fix dependency decisions, packaging updates, and a `.NET 8 SDK` validation lane. |
| Multi-target analyzer assets | follow-up-required | Ordinary runtime `lib/<tfm>` multi-targeting does not by itself solve analyzer host selection because this package ships analyzer assets from `analyzers/dotnet/cs/`. A follow-up would need to design the exact analyzer asset layout, package verification rules, and host selection evidence before documenting support. |
| Separate analyzer/source-generator and code-fix assets or packages | follow-up-required | This is the most targeted way to isolate the only Workspaces/System.Composition-coupled production slice. It could let diagnostic analyzers and source generators move independently from the code-fix provider, but it needs a product and packaging decision about whether code fixes remain in the same package, a separate analyzer asset, or a separate package. |

## Recommendation For Follow-Up

Do not claim pure `.NET 8 SDK` analyzer support from the current branch. Keep the existing `.NET 10 SDK` build-host wording until an implementation ticket changes and validates the analyzer asset target.

If support is required, split the work:

1. Retarget or split the analyzer package and normalize Roslyn, Workspaces, composition, and JSON dependencies. Treat the code-fix provider as the key coupling point because it is the only production slice requiring Workspaces and `System.Composition`.
2. Add proof and release-surface updates: `.NET 8 SDK` CI/package-verification lane, analyzer package verifier expectations, `tools/pack-release-packages.sh`, README, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, and release notes.

Until both follow-ups land, analyzer consumption remains documented on a `.NET 10 SDK` build host for both `8.50.0` and `10.50.0`.
