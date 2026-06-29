<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the implementation ticket around the already-decided single-asset `netstandard2.0` analyzer strategy from done ticket `06FH8QRPDP10ZBAF3A5RYQFFQM`; current repository evidence still shows a `net10.0` analyzer asset, SDK-local Roslyn and Workspaces references, and `.NET 10 SDK` host guidance, so this ticket now carries the implementation, proof, verifier, and documentation boundary needed before claiming pure `.NET 8 SDK` analyzer support.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Done ticket `06FH8QRPDP10ZBAF3A5RYQFFQM` already fixed the package-shape decision; this ticket implements that chosen `netstandard2.0` single-asset strategy from `docs/plans/analyzer-dotnet8-host-strategy-refinement.md`, not a fresh design fork.
- Current repository evidence still shows one `net10.0` analyzer asset packed under `analyzers/dotnet/cs/`, SDK-local Roslyn references from `$(MSBuildToolsPath)`, Workspaces and `System.Composition` references from `dotnet-format`, and README, package-verifier, and test guidance that requires a `.NET 10 SDK` host.
- No child tickets, relation changes, description updates, attachments, or new planning documents were materialized in this refinement run.

### Scope In
- Retarget `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` from `net10.0` to one `netstandard2.0` analyzer asset while keeping the existing `DCoding.Data.DVault.Analyzers` package id and `analyzers/dotnet/cs/` asset root.
- Replace SDK-local Roslyn, Workspaces, and `System.Composition` file references with reviewed package-managed build inputs and a compatible analyzer packaging layout.
- Add explicit compatibility handling for `System.Text.Json` and any other `netstandard2.0` gaps used by `DataVaultTypedReadModelSourceGenerator` or other analyzer sources.
- Update pack, package-verification, analyzer tests, integration tests, and documentation so pure `.NET 8 SDK` and `.NET 10 SDK` analyzer hosts are both proven and described consistently.
- Preserve XML documentation, `DevelopmentDependency=true`, local `PrivateAssets='all'` consumer guidance, and no runtime `lib/<tfm>` dependency leakage.

### Scope Out
- Introducing dual `net8.0` and `net10.0` analyzer assets under `analyzers/dotnet/cs/`.
- Creating a new public analyzer or code-fix package id or widening the coordinated nine-package family.
- Changing consumer package-line rules away from the existing aligned `8.50.0` and `10.50.0` lines.
- Claiming pure `.NET 8 SDK` analyzer support before both required proof lanes pass.

## Acceptance Criteria
- The analyzer project builds and packs as one `netstandard2.0` `DCoding.Data.DVault.Analyzers` asset plus XML documentation under `analyzers/dotnet/cs/` for both `8.50.0` and `10.50.0` package lines, with no runtime `lib/<tfm>` assets introduced.
- The analyzer build no longer depends on `$(MSBuildToolsPath)` or `$(MSBuildToolsPath)/DotnetTools/dotnet-format` file references for Roslyn, Workspaces, or `System.Composition`; the reviewed dependency set is normalized through explicit package-managed inputs.
- The existing code-fix provider stays in the same package, and any required companion analyzer assemblies are packed beside the main analyzer assembly under `analyzers/dotnet/cs/` without leaking consumer runtime dependencies.
- `DataVaultTypedReadModelSourceGenerator` and the remaining analyzer and source-generator code compile and preserve current behavior under the new target, including explicit `System.Text.Json` or equivalent compatibility handling where the current `net10.0` baseline was previously implicit.
- The current net10-only test assumptions are removed or replaced, including the integration test analyzer `ProjectReference` target override and the analyzer test harness `dotnet-format` assembly path assumptions, and a pure `.NET 8 SDK` host lane plus a `.NET 10 SDK` regression lane both prove analyzer restore, load, and execution.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs`, `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, and release notes are updated so packaged guidance and verifier expectations match the shipped analyzer asset set and the new two-host support claim.

## Definition of Done
- The repository implements the single-asset `netstandard2.0` strategy already documented in `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` and no longer relies on the old `net10.0`-only analyzer-host assumption in project, pack, test, or verification configuration.
- Documented validation proves the packed analyzer works on a pure `.NET 8 SDK` host and still works on a `.NET 10 SDK` host.
- Package verification and its unit tests pass against the reviewed analyzer asset set, companion assemblies if any, XML docs, and updated README host guidance.
- Consumer guidance still keeps analyzer references local with `PrivateAssets='all'`, keeps one package id and one analyzer asset root, and does not introduce runtime dependency leakage.

## Implementation Notes
- Use `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` as the authoritative design baseline; do not reopen the already-closed choice between `netstandard2.0`, dual target-specific analyzer assets, or a split public code-fix package.
- Current evidence shows `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` still targets `net10.0`, packs only the analyzer DLL and XML doc through `AddAnalyzerPackageAssets`, and suppresses dependency groups while consuming Roslyn, Workspaces, and composition assemblies from SDK-local paths.
- `DataVaultCodeFirstCodeFixProvider.cs` is the bounded production slice that currently requires Workspaces and `System.Composition`; `DataVaultTypedReadModelSourceGenerator.cs` is the bounded slice that currently relies on `System.Text.Json` plus modern BCL availability.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` still forces the analyzer `ProjectReference` to `TargetFramework=net10.0`, and `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj` still resolves Workspaces and `System.Composition.*` from `dotnet-format`; both need to move with the implementation.
- `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/manual-nuget-publication.md`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` currently encode the `.NET 10 SDK` analyzer-host baseline and must change in lockstep with proof.

## Open Questions
- none

## Follow-Up Questions
- If CLI proof passes but IDE-host loading still fails because of companion Workspaces or composition assemblies, should a later ticket isolate code-fix-specific assets or add an IDE-host validation lane?
- After the bounded CLI `.NET 8 SDK` and `.NET 10 SDK` proof lanes land, does the team want separate editor or IDE compatibility evidence before making broader support statements?

## Risks
- Retargeting to `netstandard2.0` is not a csproj-only change; analyzer and generator code may need bounded compatibility helpers for APIs that currently rely on the `net10.0` BCL.
- Because `SuppressDependenciesWhenPacking=true` remains part of the analyzer package posture, missing or mismatched companion assemblies can leave the package compiling successfully but failing to load under real analyzer hosts.
- Package verifier, README text, and test harnesses currently hard-code the `.NET 10 SDK` host baseline, so partial implementation will create false-positive or false-negative validation signals.
- The code-fix slice is the main dependency-coupled area; if Workspaces and `System.Composition` normalization proves host-fragile, delivery may require a narrower follow-up after the bounded implementation lands.

## Split Recommendations
- No additional split is required before PO-critic review if delivery stays inside the chosen single-package `netstandard2.0` strategy. If host-specific code-fix loading problems appear after CLI proof, create a follow-up instead of widening this ticket mid-stream.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Apply the selected analyzer target and package layout so the analyzer can load under a pure .NET 8 SDK host. Normalize Roslyn/Workspaces/composition references or split code-fix assets as needed. Preserve analyzer/source-generator behavior, XML docs, PrivateAssets guidance, and no runtime lib dependency leakage.