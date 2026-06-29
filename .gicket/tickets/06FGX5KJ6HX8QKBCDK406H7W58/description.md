<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Supported analyzer consumption remains a .NET 10 SDK build host with either the 8.50.0/net8.0 or 10.50.0/net10.0 consumer package line; pure .NET 8 SDK analyzer-host support is not part of this ticket.
- DCoding.Data.DVault.Analyzers remains a local build-time package only; analyzer references stay local with PrivateAssets="all" and must not be described as runtime or transitive dependencies.
- This ticket updates analyzer-compatibility documentation to the v0.50.0 documentation baseline while intentionally leaving release-note and changelog cross-references on the current v0.49.0 targets until ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands.
- Persisted relation state currently links this ticket to 06FGX6DSX1SRQ1Y22DP53629S8 through outgoing relation 06FGX5KJ6HX8QKBCDK406H7W58--06FGX6DSX1SRQ1Y22DP53629S8--blocks.

### Scope In
- Update README analyzer/package compatibility wording to the v0.50.0 documentation baseline while keeping consumer package lines 8.50.0 and 10.50.0.
- Update src/DCoding.Data.DVault.Analyzers/README.md to the same release-label, build-host, and PrivateAssets="all" guidance.
- Update docs/package-compatibility.md and docs/manual-nuget-publication.md to the v0.50.0 analyzer compatibility baseline, including the one net10.0 analyzer asset and .NET 10 SDK host statement.
- Update package-verifier guidance and tests so packaged README expectations preserve the .NET 10 SDK host baseline and reject 0.50.0 or mixed-line install claims.
- Normalize stale labels in the touched surfaces, including stale README/manual-publication headings, while keeping release-note/changelog links on their current v0.49.0 targets during this ticket.

### Scope Out
- Creating or updating CHANGELOG.md or docs/releases/v0.50.0.md; that work remains owned by ticket 06FGX6DSX1SRQ1Y22DP53629S8.
- Retargeting README, docs/package-compatibility.md, or docs/manual-nuget-publication.md release-note/changelog links to v0.50.0 before ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands.
- Retargeting the analyzer package to net8.0 or netstandard2.0.
- Adding pure .NET 8 SDK CI, pack, or package-verification lanes.
- Changing analyzer/runtime package shape beyond documentation and verifier expectation updates.
- Runtime, provider, or analyzer feature-code changes unrelated to documentation/verifier alignment.

## Acceptance Criteria
- README, analyzer README, package compatibility, and manual publication docs all state that v0.50.0 is the release label and that 8.50.0 / 10.50.0 are the consumer package versions; no 0.50.0 install or PackageReference example remains in scope.
- README, docs/package-compatibility.md, and docs/manual-nuget-publication.md keep any release-note/changelog cross-reference on the existing v0.49.0 artifact during this ticket and do not introduce a docs/releases/v0.50.0.md or CHANGELOG.md retarget before ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands.
- All in-scope analyzer guidance states that both consumer package lines ship one net10.0 analyzer asset and require a .NET 10 SDK build host, including net8.0 consumers on the 8.50.0 line.
- README and analyzer README keep analyzer references local with PrivateAssets="all" and do not imply runtime-package or transitive-package usage.
- Package-verifier guidance and tests enforce the same build-host matrix and flag unsupported pure .NET 8 SDK analyzer claims or stale/planning release-version install fragments.
- Manual publication and package compatibility guidance remain aligned with tools/pack-release-packages.sh and PackageVerifier expectations for both visible package lines without taking ownership of release-note/changelog artifact updates.

## Definition of Done
- The in-scope documentation surfaces and package-verifier guidance are updated together and reviewed for wording consistency.
- Repository guidance no longer conflates the v0.50.0 release label with a consumer-facing 0.50.0 package version.
- Analyzer compatibility wording is consistent across human-facing docs and package-verifier expectations.
- Touched headings and in-scope wording no longer carry stale v0.49.0 or v0.47 labels, except for intentionally preserved v0.49.0 release-note/changelog cross-references that remain deferred to ticket 06FGX6DSX1SRQ1Y22DP53629S8.

## Implementation Notes
- docs/plans/analyzer-package-compatibility-audit.md is the authoritative evidence source for the analyzer compatibility baseline: one net10.0 analyzer asset and no supported pure .NET 8 SDK analyzer-host claim on the current branch.
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets only net10.0, and tools/pack-release-packages.sh packs the analyzer line twice by version without changing target framework, so both package lines ship the same analyzer binary shape.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs already enforce the .NET 10 SDK host baseline and reject contradictory .NET 8 SDK claims; this ticket should carry those guards forward to the v0.50.0 wording.
- Repository text still carries multiple v0.49.0 references in README, src/DCoding.Data.DVault.Analyzers/README.md, docs/package-compatibility.md, and docs/manual-nuget-publication.md, and docs/manual-nuget-publication.md also still carries a stale Current v0.47 Dependency Matrix heading that should be corrected during the same pass.

## Open Questions
- none

## Follow-Up Questions
- After ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands, should the release-note owner run one final documentation sweep to move all remaining v0.49.0 release-note/changelog cross-references to the new v0.50.0 artifacts together?
- If pure .NET 8 SDK analyzer-host support becomes a product requirement later, should it be owned as two follow-up tickets: analyzer asset/target work first, then CI/package-verifier/documentation rollout?

## Risks
- Until ticket 06FGX6DSX1SRQ1Y22DP53629S8 lands, the repository will intentionally keep v0.49.0 release-note/changelog links next to v0.50.0 analyzer wording in the touched documentation surfaces; reviewers need to treat that as planned split ownership, not an accidental regression.
- PackageVerifier guards packaged README content, but the broader documentation set can still drift unless the in-scope docs are reviewed together in the same change.

## Split Recommendations
- Keep the current split: this ticket owns analyzer-compatibility documentation and verifier alignment, while ticket 06FGX6DSX1SRQ1Y22DP53629S8 owns CHANGELOG.md, docs/releases/v0.50.0.md, and the eventual release-note/changelog link retarget.
- If pure .NET 8 SDK analyzer-host support is later required, split it into one implementation ticket for analyzer asset/target/dependency changes and one follow-up ticket for CI, package verification, and documentation rollout.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update analyzer documentation after the compatibility implementation and verification are known.

Acceptance:
- README, Analyzer README, package compatibility docs, manual publication docs, and package verifier guidance state the exact build-host support matrix.
- The docs distinguish release label v0.50.0 from package versions 8.50.0 and 10.50.0.
- Docs keep analyzer references local with PrivateAssets="all" and do not imply runtime package usage.
- No stale .NET 10-only warning remains if .NET 8 SDK host support is actually verified; no .NET 8 support claim appears if the audit rejects it.