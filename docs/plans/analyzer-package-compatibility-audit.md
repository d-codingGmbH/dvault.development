# Analyzer Package Compatibility Audit

Ticket: `06FBSBW6HDT15D1KGVD7XBQXM8`

## Decision

For the current v0.37.0 compatibility baseline, keep `DCoding.Data.DVault.Analyzers` on one `net10.0` analyzer asset and treat the `.NET 10 SDK` as the supported build-host baseline for both the visible `8.37.0` and `10.37.0` package lines.

The current repository evidence does not prove support for consuming the analyzer package from a pure `.NET 8 SDK` baseline. If that baseline becomes a product requirement, the analyzer asset target and its verification lane must change explicitly.

## Proof

- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0`.
- The same project packs its payload under `analyzers/dotnet/cs/`, not under `lib/net8.0` or `lib/net10.0`, so the package does not expose consumer-target-specific runtime assets.
- `tools/pack-release-packages.sh` packs the analyzer project once for `8.37.0` and once for `10.37.0` without changing the analyzer target framework, so both package lines currently carry the same `net10.0` analyzer binary with different package versions.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` verifies analyzer asset presence, XML docs, symbols, and README guidance, but it does not require a separate `net8.0` analyzer asset or a line-specific analyzer dependency group.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0` and references `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` as an analyzer with `SetTargetFramework="TargetFramework=net10.0"`, which is direct local proof that the intended validation lane is a `net8.0` consumer target compiled with the `net10.0` analyzer on the repository SDK baseline.
- `README.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, and `.github/workflows/ci.yml` all set `.NET 10 SDK` as the current validation and publication baseline.
- `docs/plans/shared-implementation-standards.md` explicitly allows analyzer, tooling, benchmark, and repository helper projects to stay on `net10.0` when they are not consumer runtime packages.

## Required Follow-Up

- Make the analyzer build-host requirement explicit anywhere `8.37.0` analyzer installation guidance appears if the package is meant to stay `net10.0`-only.
- If the product requirement is instead "net8 target project plus .NET 8 SDK" compatibility, retarget the analyzer assets and add a verification lane that proves that exact baseline.
- Keep package verification and install guidance aligned with whichever compatibility claim is accepted so the `8.37.0` analyzer package is not documented more broadly than it is verified.
