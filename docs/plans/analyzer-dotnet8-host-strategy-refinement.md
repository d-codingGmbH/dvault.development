# Analyzer .NET 8 Host Strategy Refinement

Status: ticket-bound refinement note
Ticket: `06FH8QRPDP10ZBAF3A5RYQFFQM`

## Purpose

Turn the v0.50 analyzer package compatibility audit into one concrete implementation strategy for pure `.NET 8 SDK` analyzer-host support without widening the public package family or silently keeping the current `.NET 10 SDK`-only host baseline.

## Verified Repository Baseline

- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` currently targets only `net10.0`, suppresses dependency groups when packing, and packs exactly the built analyzer DLL plus XML documentation into `analyzers/dotnet/cs/`.
- The analyzer project still compiles against SDK-local Roslyn binaries from `$(MSBuildToolsPath)` and against `Microsoft.CodeAnalysis.Workspaces` plus `System.Composition.AttributedModel` from `$(MSBuildToolsPath)/DotnetTools/dotnet-format`.
- `src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstCodeFixProvider.cs` is the only production analyzer source file that consumes Workspaces/code-fix APIs and `System.Composition`.
- `src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs`, `DataVaultCodeFirstAnalyzer.cs`, and `DataVaultEfCoreMisuseAnalyzer.cs` stay on the base Roslyn analyzer and generator surface.
- `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs` consumes `System.Text.Json` and several modern BCL calls that are available on the current `net10.0` target but would need explicit dependency or compatibility handling below that baseline.
- `tools/pack-release-packages.sh` packs the analyzer project once for `8.50.0` and once for `10.50.0`, but both package lines currently receive the same `net10.0` analyzer shape because the analyzer project is not target-overridden.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` currently assume one analyzer DLL and one XML file under `analyzers/dotnet/cs/`, plus README text that explicitly requires a `.NET 10 SDK` host.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` forces the analyzer project reference to `TargetFramework=net10.0`, and `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj` still resolves Workspaces and composition assemblies from `dotnet-format`.

## Chosen Strategy

Support pure `.NET 8 SDK` and `.NET 10 SDK` analyzer hosts through one `netstandard2.0` `DCoding.Data.DVault.Analyzers` asset under the existing `analyzers/dotnet/cs/` package path.

This ticket should not introduce parallel `net8.0` and `net10.0` analyzer assets and should not split code fixes into a new package id. Keep one analyzer package id, one primary analyzer assembly, one analyzer asset root, and one aligned `PrivateAssets="all"` consumer story across both visible package-version lines.

## Why This Strategy

- The repository already ratifies one analyzer package id and one analyzer asset path. Preserving that shape minimizes release-surface churn in README, package compatibility, pack script, package verifier, and manual publication guidance.
- A dual `net8.0` plus `net10.0` analyzer asset design would require a new reviewed host-selection rule for files under `analyzers/dotnet/cs/`. The current repository evidence does not prove or document such a rule.
- Splitting analyzer and code-fix assets into separate packages would widen the coordinated package family beyond the current nine packable ids and force a separate product decision about optional versus required code-fix installation.
- The Workspaces and composition coupling is real but bounded to `DataVaultCodeFirstCodeFixProvider.cs`, so this ticket should normalize those dependencies rather than expand the public package family.
- A single `netstandard2.0` asset is the only option in the current choice set that directly targets one bounded analyzer-host compatibility story for both `.NET 8 SDK` and `.NET 10 SDK` consumers while keeping the current package identity and asset path stable.

## Required Implementation Boundary

- Retarget `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` to `netstandard2.0`.
- Replace SDK-local Roslyn and `dotnet-format` file references with explicit reviewed package-managed references.
- Keep Roslyn compile references pinned by package version rather than by local SDK layout.
- Keep the code-fix provider in the same package, but normalize its `Microsoft.CodeAnalysis.Workspaces` and `System.Composition.*` dependencies through the package-managed build and pack flow instead of the current `dotnet-format` path assumption.
- Handle `System.Text.Json` explicitly for `DataVaultTypedReadModelSourceGenerator` instead of relying on the current `net10.0` framework baseline.
- Backfill any netstandard-incompatible analyzer-source API usage with bounded compatibility helpers or equivalent code so the analyzer project compiles without restoring the `.NET 10` target assumption.
- Continue packing the main analyzer DLL and XML documentation under `analyzers/dotnet/cs/`.
- If the normalized dependency set requires companion assemblies for analyzer loading, pack that reviewed companion set beside the main analyzer assembly under `analyzers/dotnet/cs/`; do not add consumer runtime `lib/<tfm>` assets.
- Preserve `DevelopmentDependency=true`, `PrivateAssets="all"` guidance, and the local build-time analyzer package posture.

## Required Validation And Release Surfaces

- Add or update a pure `.NET 8 SDK` validation lane that proves a `net8.0` consumer project can restore, load, and execute the packed analyzer package.
- Keep a `.NET 10 SDK` validation lane so the new compatibility story remains two-host, not `.NET 8`-only.
- Remove test and build assumptions that hard-code the analyzer project to `net10.0`, including the integration-project analyzer reference override and analyzer test harness `dotnet-format` assembly paths.
- Update `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` so the package-verification contract matches the reviewed analyzer asset set for the new strategy.
- Update README, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, and the release notes so they describe pure `.NET 8 SDK` and `.NET 10 SDK` analyzer-host support consistently.
- Keep `tools/pack-release-packages.sh` on the current two visible package-version lines `8.50.0` and `10.50.0`; the analyzer package remains one package id built once per version line, not a target-specific analyzer-package fork.

## Acceptance Boundary

- The implementation plan names one supported analyzer package shape: one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/`.
- The plan explicitly covers Roslyn, Workspaces, `System.Composition`, `System.Text.Json`, analyzer package paths, and package-verifier expectations.
- The plan preserves the current nine-package coordinated family and does not invent a second analyzer package id.
- The plan requires proof on both `.NET 8 SDK` and `.NET 10 SDK` hosts before repository docs may claim pure `.NET 8 SDK` analyzer support.
- The plan keeps the current analyzer/runtime alignment rule: consumers still choose exactly one package-version line, `8.50.0` or `10.50.0`, and keep analyzer references local with `PrivateAssets="all"`.
