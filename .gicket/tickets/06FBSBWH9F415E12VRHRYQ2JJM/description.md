<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to ratify the audited compatibility outcome: keep DCoding.Data.DVault.Analyzers on the single net10.0 analyzer asset/.NET 10 SDK build-host baseline, align the root and analyzer README guidance to that boundary, and keep package verification enforcing it so net8 projects are not misled.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The compatibility outcome is already evidenced locally: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets only net10.0 and packs the analyzer under analyzers/dotnet/cs/ for both coordinated package lines.
- The ticket is a documentation-and-verification alignment task, not a request to retarget analyzer assets or broaden support beyond the verified .NET 10 SDK build-host baseline.
- Direct local proof for the intended supported lane already exists in tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, which multi-targets net8.0/net10.0 but forces the analyzer project reference to TargetFramework=net10.0.
- Path semantics: `net8.0/net10.0` is a context only compatibility-lane shorthand, not a repository-relative path and not an output path.
- Required repository output paths for this ticket are `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs`.
- No child-ticket, relation, description, attachment, or planning-document write was needed for this refinement because docs/plans/analyzer-package-compatibility-audit.md already provides the authoritative compatibility evidence.

### Scope In
- Keep root README installation guidance and src/DCoding.Data.DVault.Analyzers/README.md explicit that 8.36.0 is the net8.0 package line but analyzer consumption still uses a .NET 10 SDK build host.
- Keep analyzer package examples local with PrivateAssets="all" and aligned to the same 8.36.0 or 10.36.0 coordinated package line as the runtime/provider packages.
- Keep package verification aligned with the accepted compatibility claim so packaged README guidance cannot silently drift into broader unsupported promises.

### Scope Out
- Retargeting the analyzer project or packaged analyzer asset from net10.0 to net8.0.
- Claiming or proving support for analyzer consumption from a pure .NET 8 SDK host baseline.
- Changing runtime/provider package targeting, EF Core line selection, or broader package family versioning decisions outside the analyzer-guidance contract.

## Acceptance Criteria
- The root README and src/DCoding.Data.DVault.Analyzers/README.md both state that DCoding.Data.DVault.Analyzers follows the 8.36.0 and 10.36.0 coordinated package lines, but net8.0 projects that reference the analyzer still build with a .NET 10 SDK host.
- The in-scope analyzer installation examples keep PrivateAssets="all", do not mix package lines, do not use a consumer-facing 0.36.0 version, and do not imply validated pure .NET 8 SDK analyzer consumption.
- Package verification fails when packaged README guidance omits or contradicts the .NET 10 SDK build-host requirement for analyzer usage, and it continues to validate the current single analyzer-asset contract rather than implying a separate net8.0 analyzer asset.

## Definition of Done
- The in-scope documentation surfaces and package-verification expectations describe the same compatibility boundary with no contradictory analyzer-install guidance.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs covers the analyzer build-host guidance and the coordinated 8.36.0/10.36.0 examples that the packaged README surfaces must preserve.
- No in-scope consumer documentation claims that a pure .NET 8 SDK host is currently validated for DCoding.Data.DVault.Analyzers consumption.

## Implementation Notes
- Treat docs/plans/analyzer-package-compatibility-audit.md as the authoritative rationale unless a separate ticket explicitly changes analyzer targeting or the supported build-host baseline.
- PackageVerifier.cs already contains ExpectedAnalyzerBuildHostGuidance and packaged README checks; if README wording changes, update verifier expectations in the same ticket so documentation and verification stay lockstep.
- README.md and docs/local-validation.md already establish the repository validation baseline as a .NET 10 SDK checkout; this ticket should align analyzer-install guidance to that existing repository baseline rather than reopen it.
- Because the analyzer package is packed from a net10.0 project and shipped under analyzers/dotnet/cs/, the verification emphasis here is preventing documentation drift, not adding new consumer-runtime TFM promises.

## Open Questions
- none

## Follow-Up Questions
- If product intent later expands to "net8 target project plus pure .NET 8 SDK host" support, should a follow-up ticket retarget the analyzer asset and add an explicit verification lane for that exact baseline?
- Should the same build-host caveat be echoed in secondary release/publication docs beyond the packaged README surfaces, or is the current root/analyzer README boundary sufficient for v0.36.x?

## Risks
- As long as the analyzer remains a single net10.0 asset, any future documentation or package-metadata change that implies pure .NET 8 SDK support will overstate what the repository currently verifies.
- Live ticket relations still show this ticket blocked by 06FBSBWBT33K7Y1Z6NM71GAQ68 and blocking 06FBSBWPN112S4CGP0239K0ZT8, so delivery sequencing can still depend on external ticket flow even though PO refinement is complete.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update src/DCoding.Data.DVault.Analyzers/README.md, root README guidance, and package verification if the analyzer asset shape or SDK requirement changes. Acceptance: analyzer installation guidance is not misleading for net8 projects.