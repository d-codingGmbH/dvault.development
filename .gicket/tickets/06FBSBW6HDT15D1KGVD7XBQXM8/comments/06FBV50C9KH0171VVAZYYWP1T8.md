[gicket-bot] PO-critic review contract

Summary
- The contract is ready for developer handoff: the accepted net10.0 analyzer and .NET 10 SDK baseline is explicit, proof paths are concrete, and Open Questions is none; remaining README/verifier alignment is implementation follow-through, not a PO-refinement blocker.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSBW6HDT15D1KGVD7XBQXM8/description.md contains PO handoff ready_for_po_critic and an Open Questions section with - none.
- docs/plans/analyzer-package-compatibility-audit.md records the decision to keep DCoding.Data.DVault.Analyzers on one net10.0 asset and treat .NET 10 SDK as the supported build-host baseline for both 8.36.0 and 10.36.0.
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets only net10.0 and packs output under analyzers/dotnet/cs/.
- tools/pack-release-packages.sh packs the same analyzer project for 8.36.0 and 10.36.0 through pack_analyzer_line without changing target framework.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj multi-targets net8.0;net10.0 and references the analyzer project with SetTargetFramework=TargetFramework=net10.0.
- docs/local-validation.md and .github/workflows/ci.yml both require a .NET 10 SDK checkout or setup for the validation lane.
- git -C /mnt/c/Projects/DVault diff --name-only develop..HEAD on branch ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n changes only .gicket ticket artifacts and docs/plans/analyzer-package-compatibility-audit.md; it does not touch README.md, src/DCoding.Data.DVault.Analyzers/README.md, or tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs.
- README.md and src/DCoding.Data.DVault.Analyzers/README.md currently label 8.36.0 as net8.0 / EF Core 8 guidance, while tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs ValidateReadme checks target-framework, EF-line, version, and PackageReference snippets but not explicit .NET 10 SDK host-baseline wording.
- .gicket/tickets/06FBSBW6HDT15D1KGVD7XBQXM8/comments/*.md currently contains only gicket-bot workflow and PO-refinement comments; no later human comment reopens the scope or decision.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A consumer-facing example that says 8.36.0 on a net8.0 project is supported only when the build host uses the .NET 10 SDK would make the boundary easier to implement consistently across docs and packaged README text.
- An explicit out-of-scope example for net8.0 projects built on a pure .NET 8 SDK would reduce the chance that follow-up work accidentally broadens the support promise.

Risky assumptions
- The story assumes the remaining README and package-verifier alignment will be completed before release; current checked-in install guidance still reads broader than the verified host-SDK baseline.
- The story assumes the current live relation path 06FBSBW6HDT15D1KGVD7XBQXM8 -> 06FBSBWBT33K7Y1Z6NM71GAQ68 -> 06FBSBWH9F415E12VRHRYQ2JJM is sufficient traceability even though the story has no direct live relation to 06FBSBWH9F415E12VRHRYQ2JJM.
- The accepted outcome assumes pure .NET 8 SDK analyzer-consumption support is not a product promise unless a later ticket explicitly adds retargeting and proof.

AC / test suggestions
- Have the follow-up verification lane assert that packaged README guidance for the analyzer 8.36.0 line mentions the required .NET 10 SDK host baseline, not just the net8.0 target framework and version number.
- Keep at least one validation example that distinguishes consumer target framework from build-host SDK so net8.0 is not read as proof of pure .NET 8 SDK analyzer compatibility.

Implementation watchouts
- DCoding.Data.DVault.Analyzers.csproj plus analyzers/dotnet/cs packaging means both package lines currently ship the same analyzer asset; follow-up wording must not imply separate net8-specific analyzer binaries.
- tools/pack-release-packages.sh only varies the analyzer package version between 8.36.0 and 10.36.0; any future pure .NET 8 SDK claim needs more than versioned packaging text.
- PackageVerifier.cs currently validates line-specific README snippets and analyzer asset presence, not host-SDK compatibility wording.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj proves the current intended lane as net8.0 consumer target plus net10.0 analyzer reference; it is not proof of a pure .NET 8 SDK host baseline.

Non-blocking notes
- The prompt snapshot said recent comments were <none>, but .gicket/tickets/06FBSBW6HDT15D1KGVD7XBQXM8/comments now contains only bot workflow and PO-refinement comments.

Split recommendations
- Keep the current split: 06FBSBWBT33K7Y1Z6NM71GAQ68 for asset-target or SDK-gate implementation and 06FBSBWH9F415E12VRHRYQ2JJM for README and package-verification alignment.
- Optionally add a direct live relation from story 06FBSBW6HDT15D1KGVD7XBQXM8 to 06FBSBWH9F415E12VRHRYQ2JJM if the team wants the relation graph to match the delivery-contract text more directly.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment