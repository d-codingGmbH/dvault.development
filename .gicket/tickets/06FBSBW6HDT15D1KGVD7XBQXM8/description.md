<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around a net10.0-only analyzer baseline for 8.36.0/net8.0 consumers on the repository's .NET 10 SDK build-host contract, and wrote docs/plans/analyzer-package-compatibility-audit.md with the audit proof.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Checked-in evidence supports the 8.36.0 analyzer package as a net8.0 consumer-target story compiled on the documented .NET 10 SDK baseline; the repository does not currently prove a pure .NET 8 SDK analyzer-consumption baseline.
- The analyzer package project targets only net10.0 and packs analyzer assets under analyzers/dotnet/cs/, so the current 8.36.0 and 10.36.0 analyzer lines differ by package version, not by consumer-target-specific analyzer binaries.
- Created planning note docs/plans/analyzer-package-compatibility-audit.md capturing the decision, proof, and follow-up boundaries.

### Scope In
- Audit the current analyzer package target, packaging layout, and validation story for net8.0 consumers using the 8.36.0 package line.
- Ratify whether the current net10.0 analyzer asset is acceptable as the supported baseline or whether retargeting is required for the claimed compatibility surface.
- Define the documentation and verification work needed so the accepted compatibility claim matches what the repository actually proves.

### Scope Out
- Changing runtime or provider package target frameworks.
- Broad analyzer feature work unrelated to package-host compatibility.
- Claiming support for a pure .NET 8 SDK analyzer-consumption baseline unless the analyzer assets and verification lane are explicitly changed to prove it.

## Acceptance Criteria
- The ticket records an explicit compatibility decision for DCoding.Data.DVault.Analyzers on the 8.36.0 line, including whether the package remains a net10.0 analyzer asset or must be retargeted.
- The decision cites concrete local proof from the analyzer csproj, pack script, package verification code/tests, integration-project analyzer reference, and repository validation surfaces.
- If the analyzer stays net10.0-only, README.md, src/DCoding.Data.DVault.Analyzers/README.md, and package-verification expectations explicitly state the supported build-host SDK baseline for net8.0 consumers.
- If the product requirement is instead net8.0-project plus .NET 8 SDK compatibility, follow-up implementation retargets the analyzer assets and adds verification that proves that exact baseline.
- The final install guidance and verification lane do not promise a broader compatibility story than the repository actually validates.

## Definition of Done
- The audit decision and proof are preserved in the ticket handoff and the planning note at docs/plans/analyzer-package-compatibility-audit.md.
- The accepted compatibility claim is reflected in analyzer installation guidance and in the package-verification or smoke-test lane that enforces that claim.
- Existing follow-up tasks stay aligned with the chosen outcome: 06FBSBWBT33K7Y1Z6NM71GAQ68 for implementation or SDK gating, and 06FBSBWH9F415E12VRHRYQ2JJM for documentation and verification alignment.
- A reviewer can trace the chosen baseline to checked-in repository evidence without reopening method-level implementation questions.

## Implementation Notes
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets only net10.0, while tools/pack-release-packages.sh packs that same analyzer project for both 8.36.0 and 10.36.0 without changing target framework.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs validates analyzer asset presence and README guidance but does not require a separate net8.0 analyzer asset or a host-SDK compatibility smoke test.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj already provides bounded local proof for the intended current baseline by multi-targeting net8.0;net10.0 and forcing the analyzer project reference to TargetFramework=net10.0.
- README.md, docs/local-validation.md, docs/manual-nuget-publication.md, .github/workflows/ci.yml, and docs/plans/shared-implementation-standards.md consistently establish .NET 10 SDK as the repository validation baseline and allow analyzer/helper projects to remain net10.0.
- This PO refinement used checked-in repository evidence only; no executable build or package smoke run was added in this read/write-planning-only pass.

## Open Questions
- none

## Follow-Up Questions
- If product guidance needs to support net8.0 projects built on a pure .NET 8 SDK, should that become a separate compatibility commitment instead of broadening the current net10.0-host baseline implicitly?
- Should live ticket relations be normalized so this story tracks both existing follow-up tasks explicitly, given that the current live relation state only blocks 06FBSBWBT33K7Y1Z6NM71GAQ68?

## Risks
- Current public installation guidance shows the 8.36.0 analyzer package for net8.0 projects without an explicit build-host SDK requirement, so leaving the docs unchanged would overstate the verified compatibility baseline.
- Package verification currently proves version-aligned analyzer docs and asset presence but not a host-SDK compatibility lane, so future changes could silently drift from the accepted compatibility claim.
- The current live relation state is asymmetric: this story blocks the implementation task 06FBSBWBT33K7Y1Z6NM71GAQ68, while the documentation task 06FBSBWH9F415E12VRHRYQ2JJM exists without a live relation on the story.

## Split Recommendations
- Keep the current split: 06FBSBWBT33K7Y1Z6NM71GAQ68 owns any analyzer asset-target change or explicit SDK gate, and 06FBSBWH9F415E12VRHRYQ2JJM owns README and package-verification alignment.
- Do not create additional child tickets unless the team chooses to support a pure .NET 8 SDK analyzer-consumption baseline as a distinct compatibility promise.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Determine whether DCoding.Data.DVault.Analyzers targeting only net10.0 is acceptable for consumers installing the 8.x package line with net8 projects and common SDK baselines. Acceptance: documented decision with proof, including whether analyzer assets should target netstandard2.0, multi-target, or remain net10.0 with an explicit SDK requirement.