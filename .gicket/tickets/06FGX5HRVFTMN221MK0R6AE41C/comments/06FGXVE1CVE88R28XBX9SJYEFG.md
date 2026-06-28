[gicket-bot] PO refinement contract

Summary
- Refined this as the implementation follow-up to the analyzer package compatibility audit: for the 8.50.0 and 10.50.0 package lines, keep one net10.0 analyzer asset and the existing .NET 10 SDK build-host baseline instead of retargeting or multi-targeting for pure .NET 8 SDK hosts. No child split or planning write was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the compatibility decision: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets only net10.0, packs analyzer assets under analyzers/dotnet/cs/, and suppresses normal runtime dependency publication with IncludeBuildOutput=false, SuppressDependenciesWhenPacking=true, and DevelopmentDependency=true.
- The selected outcome for this ticket is the documented no-go path from docs/plans/analyzer-package-compatibility-audit.md, not analyzer multi-targeting: both visible package lines should continue to ship the same net10.0 analyzer binary shape and require a .NET 10 SDK build host.
- The current repository already encodes this host baseline in the analyzer README, docs/package-compatibility.md, tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and the integration test analyzer reference with SetTargetFramework=TargetFramework=net10.0; this ticket should carry that exact contract forward to 8.50.0 / 10.50.0.

Scope In
- Advance the analyzer-related package-line/version surfaces from the current 8.49.0 / 10.49.0 baseline to 8.50.0 / 10.50.0 wherever analyzer packaging, verifier expectations, tests, and public analyzer guidance depend on those exact versions.
- Keep DCoding.Data.DVault.Analyzers as one net10.0 analyzer asset packaged under analyzers/dotnet/cs/ for both coordinated package lines.
- Preserve local build-time analyzer consumption semantics, including PrivateAssets="all" guidance and no runtime dependency leakage.
- Keep analyzer, source-generator, integration, and package-verification coverage aligned with the chosen analyzer host baseline.

Scope Out
- Adding a separate net8.0 analyzer asset or multi-targeting the analyzer project.
- Claiming, documenting, or validating pure .NET 8 SDK analyzer-host support.
- Changing analyzer diagnostics, generator behavior, or runtime package dependency baselines beyond what is required to keep the current packaging contract coherent.
- Broader release automation, publication approval, or unrelated package-family feature work.

Open questions
- none

Follow-up questions
- If DVault later needs pure .NET 8 SDK analyzer-host support, open a separate bounded ticket for analyzer target/asset strategy plus Roslyn reference normalization, followed by a validation/documentation follow-up as outlined in the compatibility audit.

Risks
- The analyzer-host decision is already repository-backed, but the version surfaces are duplicated across packaging, docs, and tests; partial updates will leave pack script, package verifier, and README guidance inconsistent.
- A naive implementation could accidentally broaden support claims to pure .NET 8 SDK hosts even though the audit explicitly rejected that claim.
- If the 8.50.0 / 10.50.0 version uplift lands piecemeal across multiple tickets, merge ordering can create temporary verifier or documentation failures unless the touched version surfaces stay coordinated.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment