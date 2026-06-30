[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current parent contract in .gicket/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/description.md has "## Open Questions" = "- none", but its "## Definition of Done" still says ticket 06FH8RP1SBVZ7K3K48ERGZSMQC must land the 8.51.0 and 10.51.0 release-note and package-validation roll-forward.
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets netstandard2.0 and packs the analyzer DLL, XML docs, Microsoft.CodeAnalysis.Workspaces, System.Composition.*, and System.Text.Json into analyzers/dotnet/cs/.
- tools/run-analyzer-package-smoke.sh defines separate SDK-major 8 and 10 lanes and creates a temporary consumer with PackageReference Include="DCoding.Data.DVault.Analyzers" Version="$package_version" PrivateAssets="all".
- .github/workflows/ci.yml installs both 8.0.x and 10.0.x SDKs and runs bash tools/run-analyzer-package-smoke.sh 8, bash tools/run-analyzer-package-smoke.sh 10, and bash tools/verify-packages.sh.
- docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, and src/DCoding.Data.DVault.Analyzers/README.md all still describe the current public baseline as package lines 8.50.0 and 10.50.0 with one netstandard2.0 analyzer asset and dual .NET 8 SDK/.NET 10 SDK host support.
- git diff --name-only develop...HEAD shows this branch changes only .gicket/tickets/06FH8QAVJFXANVQFXGPYVAFXSR metadata; the implemented repository baseline already lives outside this tracking-parent branch.

Blocking findings
- The parent mixes an already-landed 8.50.0/10.50.0 repository baseline with future 8.51.0/10.51.0 release wording, so approving it for developer handoff would leave no direct developer scope on this ticket while the real remaining scope sits on an unrefined separate ticket.

Required PO actions
- Either narrow this parent contract to the already-landed 8.50.0/10.50.0 analyzer-host baseline and remove the 8.51.0/10.51.0 landing condition from this ticket, or keep that future roll-forward in scope and wait until ticket 06FH8RP1SBVZ7K3K48ERGZSMQC is refined and completed.
- Refine ticket 06FH8RP1SBVZ7K3K48ERGZSMQC into a delivery-contract-quality follow-up if it remains the intended carrier for the 8.51.0/10.51.0 release-note, changelog, install-guidance, and package-validation updates.
- Clean up or explicitly defer the stale child-to-parent blocks/relation noise before re-submitting this tracking parent so the live relation graph matches the done child state.

Open issues ledger
- critic-item-1 [required-po-action] Either narrow this parent contract to the already-landed 8.50.0/10.50.0 analyzer-host baseline and remove the 8.51.0/10.51.0 landing condition from this ticket, or keep that future roll-forward in scope and wait until ticket 06FH8RP1SBVZ7K3K48ERGZSMQC is refined and completed.
- critic-item-2 [required-po-action] Refine ticket 06FH8RP1SBVZ7K3K48ERGZSMQC into a delivery-contract-quality follow-up if it remains the intended carrier for the 8.51.0/10.51.0 release-note, changelog, install-guidance, and package-validation updates.
- critic-item-3 [required-po-action] Clean up or explicitly defer the stale child-to-parent blocks/relation noise before re-submitting this tracking parent so the live relation graph matches the done child state.
- critic-item-4 [blocking-finding] The parent mixes an already-landed 8.50.0/10.50.0 repository baseline with future 8.51.0/10.51.0 release wording, so approving it for developer handoff would leave no direct developer scope on this ticket while the real remaining scope sits on an unrefined separate ticket.

Missing examples / edge cases
- Clarify whether this parent closes at the current 8.50.0/10.50.0 repository-backed baseline or only after the later 8.51.0/10.51.0 release surfaces exist; the current contract currently says both.
- Clarify whether relation cleanup is required before closure or is purely administrative follow-up.

Risky assumptions
- Assumes the stale incoming child relation state will not mislead automation or closure logic.
- Assumes no further PO clarification is needed even though the repository evidence still stops at the 8.50.0/10.50.0 baseline.

AC / test suggestions
- If this parent is narrowed to current state, restate the acceptance boundary explicitly as the already-landed 8.50.0/10.50.0 docs, verifier, smoke, CI, and netstandard2.0 analyzer-package baseline, with ticket 06FH8RP1SBVZ7K3K48ERGZSMQC fully out of scope.
- If future roll-forward stays in scope, require direct evidence from docs/package-compatibility.md, docs/manual-nuget-publication.md, release notes/changelog, tools/run-analyzer-package-smoke.sh, and tools/verify-packages.sh after they move to 8.51.0/10.51.0.

Implementation watchouts
- Current repository proof is deliberately bounded to one netstandard2.0 analyzer asset under analyzers/dotnet/cs/ and CLI SDK-host validation on .NET 8 SDK and .NET 10 SDK; do not broaden this ticket to IDE/editor host claims.
- Keep PrivateAssets=all analyzer guidance and no runtime lib/<tfm> leakage as explicit guardrails whenever the release-baseline follow-up updates docs or package verification.

Non-blocking notes
- Repository evidence already supports the completed analyzer-host baseline across src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, .github/workflows/ci.yml, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs.
- git log on the affected repository files shows the implementation/test/docs changes were integrated under child-ticket history such as commit d7c53bea5 [06FH8R4EF1QFF2E3ZWS3P1BWHM], which matches the parent's tracking-only role.

Split recommendations
- No new implementation split is needed; the current child split is adequate. The remaining issue is scope hygiene between this tracking parent and follow-up ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment