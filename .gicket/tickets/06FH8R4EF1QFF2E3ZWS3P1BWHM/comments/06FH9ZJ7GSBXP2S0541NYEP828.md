[gicket-bot] PO-critic review contract

Summary
- Delivery contract is concrete and internally consistent for pre-development handoff: it names the current net10-only baseline, the required single-asset netstandard2.0 strategy, the dual-host proof boundary, and the documentation/verifier surfaces to keep aligned.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket` returned revision `06FH9XZZAHQQKB18CH07BX9YDC` for ticket `06FH8R4EF1QFF2E3ZWS3P1BWHM`; the persisted delivery contract says `PO Handoff: ready_for_po_critic` and `## Open Questions` = `none`.
- Branch `ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package` is at `4faf8a20ccc931980eb773abb5d163df391385cb`; `git log --oneline -5` shows only PO/PO-critic workflow commits after `develop` `c04b7b0fd`, and `git diff --stat c04b7b0fd..4faf8a20c` shows only `.gicket/tickets/...` metadata changes, which is acceptable for a pre-dev handoff.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` currently targets `net10.0` and references `Microsoft.CodeAnalysis.Workspaces` and `System.Composition.AttributedModel` from `$(MSBuildToolsPath)/DotnetTools/dotnet-format`, matching the contract's stated baseline.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` keeps the analyzer reference local with `PrivateAssets="all"` but pins `SetTargetFramework="TargetFramework=net10.0"`; `tests/DCoding.Data.DVault.Tests/Integration/AnalyzerSdkHostSmokeTests.cs` currently proves `Net8ConsumerTargetCompilesGeneratedMapperOutputFromNet10AnalyzerAsset` and `Net10ConsumerTargetCompilesGeneratedMapperOutputFromNet10AnalyzerAsset`.
- `.github/workflows/ci.yml` sets up only `.NET 10 SDK` (`actions/setup-dotnet@v4` with `dotnet-version: 10.0.x`), and `docs/local-validation.md` starts with `Run validation from the repository root with a .NET 10 SDK checkout.`
- Current user-facing/package-verifier guidance is consistently on the old host boundary: `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md` all state that `DCoding.Data.DVault.Analyzers` requires a `.NET 10 SDK` host; `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` hard-code the same expected guidance string.
- `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` is present and specific: it requires one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/`, no second analyzer package id, package-managed Roslyn/Workspaces/System.Composition handling, and dual `.NET 8 SDK`/`.NET 10 SDK` proof.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Spell out that the packaged consumer smoke must restore from the packed `.nupkg` artifacts, not from an in-solution project reference or source build path, on both the `.NET 8 SDK` and `.NET 10 SDK` host lanes.
- Cover the companion-assembly edge case explicitly in test expectations: if the `netstandard2.0` analyzer needs additional Roslyn/Workspaces/System.Composition assemblies beside the main DLL, the verifier should assert the exact reviewed file set under `analyzers/dotnet/cs/`.

Risky assumptions
- The chosen single `netstandard2.0` analyzer asset plus any reviewed companion assemblies will be enough to preserve analyzer/code-fix loadability on both SDK hosts without needing a package-family split.
- The contract names `release notes` generically; repository convention strongly suggests `docs/releases/v0.50.0.md`, but that exact path is not called out in the delivery contract.

AC / test suggestions
- Keep one regression that fails if the integration project or its companion matrix tests reintroduce `SetTargetFramework="TargetFramework=net10.0"` for the compatibility proof path; `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs` currently asserts the old behavior.
- Require the analyzer test harness to stop resolving Workspaces/composition from `dotnet-format`; `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj` is currently `net10.0`-only and hard-codes those SDK-local paths.
- Have the dual-host smoke assert generated analyzer output, not only build success, so the proof stays tied to actual analyzer/source-generator execution.

Implementation watchouts
- Repository documentation has more than the four obvious surfaces: search hits show current analyzer-host wording in `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, `docs/production-adoption-checklist.md`, and release-note files such as `docs/releases/v0.50.0.md`.
- Package-verifier behavior is coupled to source docs and tests; changing only prose or only verifier constants will leave the repo inconsistent because `PackageVerifier.cs` and `PackageVerifierTests.cs` both currently encode the `.NET 10 SDK` host statement.
- The existing branch contains ticket metadata only; developers should expect all product code, CI, verifier, and doc work for this ticket to be implemented from the current baseline.

Non-blocking notes
- `gicket-read-ticket-comments` returned workflow/bot comments only; there is no separate human comment thread introducing new unresolved scope beyond the persisted delivery contract.
- The ticket is broad, but the work items are tightly coupled around one release-boundary change: analyzer package shape, packaged smoke proof, CI/local validation, verifier guards, and synchronized documentation.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment