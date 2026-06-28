<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this as an evidence-backed audit ticket. Current repository state already ratifies a single net10.0 analyzer asset and a .NET 10 SDK build-host baseline for both package lines, so the ticket should document the exact Roslyn/source-generator/code-fix/package couplings and decide whether a separate implementation ticket is warranted. No child tickets, relation writes, description writes, attachments, or planning documents were materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- docs/plans/analyzer-package-compatibility-audit.md already establishes the current v0.49.0 baseline: DCoding.Data.DVault.Analyzers ships one net10.0 analyzer asset and the repository does not validate pure .NET 8 SDK analyzer consumption.
- This ticket should refine that baseline into a dependency audit, not reopen the current support claim. The expected outcome is a file-backed go/no-go assessment for pure .NET 8 SDK hosts and the minimal bounded follow-up if support is required.
- Fresh repository inspection confirmed the baseline is consistent across packaging, tests, README/docs, package verification, and CI rather than being an isolated project-file quirk.

### Scope In
- Audit src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj for target framework, Roslyn assembly references, Workspaces/System.Composition references, and packaging behavior under analyzers/dotnet/cs/.
- Separate dependency findings for the diagnostic analyzers, the two source generators, and the code-fix provider instead of treating the package as one undifferentiated slice.
- Document the current proof points for the .NET 10 SDK host baseline across tests, package scripts, package verification, README/docs, and CI.
- Produce a clear next-step recommendation: keep the current baseline with documentation only, or raise bounded implementation follow-up for retargeting plus validation.

### Scope Out
- Retargeting or multi-targeting the analyzer package in this ticket.
- Changing runtime package target frameworks, EF Core version lines, or provider package dependency policy.
- Adding a new .NET 8 SDK CI lane, package verifier behavior, or packaging layout as part of this audit ticket.
- Claiming pure .NET 8 SDK analyzer support without a later implementation and validation lane.

## Acceptance Criteria
- The audit lists the exact dependency surface for each slice: diagnostic analyzers, source generators, and code-fix provider, including which Roslyn or SDK-local assemblies each slice requires.
- The audit cites the current blockers or assumptions for pure .NET 8 SDK host consumption, including the net10.0 analyzer target, MSBuildToolsPath and DotnetTools/dotnet-format HintPath references, the single analyzers/dotnet/cs packaged asset, the net10.0 analyzer integration lane, and the .NET 10-only validation baseline.
- The audit states whether netstandard2.0, net8.0, multi-targeted analyzer assets, or separate analyzer assets are viable next steps from current evidence, and marks each option as go, no-go, or follow-up-required.
- The result gives the next implementation ticket a concrete recommendation instead of an open-ended investigation.

## Definition of Done
- An authoritative audit note exists on the ticket or approved planning surface and cites the inspected repository files that support its conclusion.
- The note identifies any source-level or packaging-level changes that would be required before pure .NET 8 SDK analyzer consumption could be documented as supported.
- The note records downstream surfaces that must change if the host baseline changes: README, src/DCoding.Data.DVault.Analyzers/README.md, docs/package-compatibility.md, docs/local-validation.md, tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and CI expectations.
- The ticket closes with the current baseline explicit: until a follow-up implementation ticket lands, analyzer consumption remains documented on a .NET 10 SDK host for both 8.49.0 and 10.49.0 package lines.

## Implementation Notes
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets net10.0, sets IncludeBuildOutput=false and SuppressDependenciesWhenPacking=true, and packs $(TargetPath) into analyzers/dotnet/cs/ rather than producing target-framework-specific lib assets.
- That same project binds Microsoft.CodeAnalysis and Microsoft.CodeAnalysis.CSharp from $(MSBuildToolsPath), and binds Microsoft.CodeAnalysis.Workspaces plus System.Composition.AttributedModel from $(MSBuildToolsPath)/DotnetTools/dotnet-format, so the package is coupled to SDK-local Roslyn and dotnet-format layout rather than NuGet-declared Roslyn dependencies.
- DataVaultCodeFirstAnalyzer.cs and DataVaultEfCoreMisuseAnalyzer.cs are analyzer-only slices; DataVaultMappingSourceGenerator.cs and DataVaultTypedReadModelSourceGenerator.cs are source-generator slices; DataVaultCodeFirstCodeFixProvider.cs is the only code-fix slice and is the part that pulls in Workspaces, Formatting, and System.Composition export behavior.
- tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj adds Microsoft.CodeAnalysis.CSharp.Workspaces and additional System.Composition assemblies for analyzer/code-fix test coverage, while tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj forces the analyzer reference to TargetFramework=net10.0 even for the net8.0 consumer lane.
- tools/pack-release-packages.sh packs the analyzer project once per visible package line without changing its target framework, and tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs plus README/docs explicitly enforce the .NET 10 SDK host wording instead of a pure .NET 8 SDK claim.
- .github/workflows/ci.yml and docs/local-validation.md currently validate the repository on a .NET 10 SDK baseline only, which means the existing automated proof is net8.0 consumer target plus net10.0 analyzer host, not pure .NET 8 SDK host compatibility.

## Open Questions
- none

## Follow-Up Questions
- If pure .NET 8 SDK analyzer consumption becomes a product requirement, is the preferred compatibility target one netstandard2.0 analyzer asset, one net8.0 analyzer asset, or a multi-targeted or split-asset strategy?
- If the code-fix provider remains the only Workspaces/System.Composition-coupled slice, should a future implementation keep it in the same package asset or allow a separate asset/package boundary from the analyzers and source generators?
- Should a future implementation ticket add a dedicated .NET 8 SDK CI and package-verification lane before any README claim changes?

## Risks
- The visible 8.49.0 package line can be misread as .NET 8 SDK host support unless the audit preserves the current .NET 10 SDK host wording already enforced in README and PackageVerifier.
- The SDK-local HintPath references to MSBuildToolsPath and DotnetTools/dotnet-format make analyzer and test resolution sensitive to SDK layout, so a retargeting effort can fail even before source-level API issues are addressed.
- Current validation proves a net8.0 consumer target compiled with the net10.0 analyzer asset; it does not prove pure .NET 8 SDK host compatibility.

## Split Recommendations
- If implementation follow-up is approved, split it into one ticket for analyzer target and asset strategy plus Roslyn reference normalization, and a second ticket for CI, package-verifier, packaging, and documentation updates required by the chosen host baseline.
- If the audit finds the code-fix provider to be the only hard blocker for a lower analyzer target, consider a separate follow-up slice for code-fix packaging rather than forcing the analyzer and source-generator paths to move together.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Inspect DCoding.Data.DVault.Analyzers for APIs, referenced Roslyn assemblies, code-fix dependencies, and packaging assumptions that force the current .NET 10 SDK host baseline.

Acceptance:
- The audit identifies whether netstandard2.0, net8.0, multi-targeting, or separate analyzer assets are viable.
- Source generator, diagnostic analyzer, and code-fix provider dependencies are checked separately.
- The audit lists any APIs that block .NET 8 SDK consumption and points to the affected source files or package assets.
- The result gives the implementation ticket a clear go/no-go path.