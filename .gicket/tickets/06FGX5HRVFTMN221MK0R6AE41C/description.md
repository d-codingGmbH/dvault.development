<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this as the implementation follow-up to the analyzer package compatibility audit: for the 8.50.0 and 10.50.0 package lines, keep one net10.0 analyzer asset and the existing .NET 10 SDK build-host baseline instead of retargeting or multi-targeting for pure .NET 8 SDK hosts. No child split or planning write was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the compatibility decision: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets only net10.0, packs analyzer assets under analyzers/dotnet/cs/, and suppresses normal runtime dependency publication with IncludeBuildOutput=false, SuppressDependenciesWhenPacking=true, and DevelopmentDependency=true.
- The selected outcome for this ticket is the documented no-go path from docs/plans/analyzer-package-compatibility-audit.md, not analyzer multi-targeting: both visible package lines should continue to ship the same net10.0 analyzer binary shape and require a .NET 10 SDK build host.
- The current repository already encodes this host baseline in the analyzer README, docs/package-compatibility.md, tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and the integration test analyzer reference with SetTargetFramework=TargetFramework=net10.0; this ticket should carry that exact contract forward to 8.50.0 / 10.50.0.

### Scope In
- Advance the analyzer-related package-line/version surfaces from the current 8.49.0 / 10.49.0 baseline to 8.50.0 / 10.50.0 wherever analyzer packaging, verifier expectations, tests, and public analyzer guidance depend on those exact versions.
- Keep DCoding.Data.DVault.Analyzers as one net10.0 analyzer asset packaged under analyzers/dotnet/cs/ for both coordinated package lines.
- Preserve local build-time analyzer consumption semantics, including PrivateAssets="all" guidance and no runtime dependency leakage.
- Keep analyzer, source-generator, integration, and package-verification coverage aligned with the chosen analyzer host baseline.

### Scope Out
- Adding a separate net8.0 analyzer asset or multi-targeting the analyzer project.
- Claiming, documenting, or validating pure .NET 8 SDK analyzer-host support.
- Changing analyzer diagnostics, generator behavior, or runtime package dependency baselines beyond what is required to keep the current packaging contract coherent.
- Broader release automation, publication approval, or unrelated package-family feature work.

## Acceptance Criteria
- For the 8.50.0 and 10.50.0 package lines, the packed analyzer package still publishes the analyzer DLL and XML documentation under analyzers/dotnet/cs/ and does not introduce runtime lib/<tfm> assets or dependency leakage.
- The analyzer project remains a single net10.0 build target, and both coordinated package lines explicitly preserve the .NET 10 SDK build-host guidance instead of claiming pure .NET 8 SDK analyzer consumption.
- Analyzer install guidance for both package lines continues to show PrivateAssets="all" and aligned package versions in the analyzer README, root/package compatibility guidance, and package verification expectations.
- Release-pack and package-verification surfaces are updated together for 8.50.0 / 10.50.0 so the package matrix and README checks stay internally consistent.
- Existing analyzer/source-generator tests and package verification tests covering the analyzer host baseline continue to pass.

## Definition of Done
- All repository constants, docs, and verifier/test expectations in this ticket's analyzer scope that currently hard-code 8.49.0 / 10.49.0 are advanced coherently to 8.50.0 / 10.50.0.
- The integration/analyzer test wiring continues to consume the analyzer via a net10.0 build target and still keeps the reference local-only.
- No source or doc surface in scope contradicts the chosen .NET 10 SDK host baseline or implies unsupported pure .NET 8 SDK analyzer compatibility.
- The no-go rationale remains recorded in docs instead of being left implicit in code-only behavior.

## Implementation Notes
- Use docs/plans/analyzer-package-compatibility-audit.md as the authoritative decision source; the selected path is keep one net10.0 analyzer asset rather than retargeting or multi-targeting.
- Current repository evidence shows the critical touchpoints: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj, tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, src/DCoding.Data.DVault.Analyzers/README.md, docs/package-compatibility.md, README.md, tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj.
- The integration project already expresses the supported host pattern with an analyzer ProjectReference using OutputItemType=Analyzer, ReferenceOutputAssembly=false, PrivateAssets=all, and SetTargetFramework=TargetFramework=net10.0; preserve that pattern unless a separate ticket reopens the host decision.
- Because the current branch baseline is still v0.49.0 / 8.49.0 / 10.49.0, treat this as a coordinated analyzer-host-contract carry-forward during the next package-line uplift, not as an isolated analyzer csproj experiment.

## Open Questions
- none

## Follow-Up Questions
- If DVault later needs pure .NET 8 SDK analyzer-host support, open a separate bounded ticket for analyzer target/asset strategy plus Roslyn reference normalization, followed by a validation/documentation follow-up as outlined in the compatibility audit.

## Risks
- The analyzer-host decision is already repository-backed, but the version surfaces are duplicated across packaging, docs, and tests; partial updates will leave pack script, package verifier, and README guidance inconsistent.
- A naive implementation could accidentally broaden support claims to pure .NET 8 SDK hosts even though the audit explicitly rejected that claim.
- If the 8.50.0 / 10.50.0 version uplift lands piecemeal across multiple tickets, merge ordering can create temporary verifier or documentation failures unless the touched version surfaces stay coordinated.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement the package/project changes selected by the analyzer compatibility audit.

Acceptance:
- The analyzer package contains the supported analyzer asset(s) for the 8.50.0 and 10.50.0 package lines.
- Analyzer references remain local build-time references with PrivateAssets guidance and no runtime dependency leak.
- Existing analyzer and source-generator tests still pass.
- Any intentional no-go outcome updates the code/package surface minimally and records the reason in docs instead of making unsupported claims.