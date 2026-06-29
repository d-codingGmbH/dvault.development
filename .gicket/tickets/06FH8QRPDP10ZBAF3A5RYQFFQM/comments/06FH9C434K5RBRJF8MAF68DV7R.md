[gicket-bot] PO-critic review contract

Summary
- Delivery contract is concrete, open questions are closed, and the repository evidence supports handing this design ticket to development.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FH8QRPDP10ZBAF3A5RYQFFQM/description.md contains `PO Handoff` = `ready_for_po_critic`, `## Open Questions` = `none`, and acceptance criteria/DoD that choose one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/`.
- docs/plans/analyzer-dotnet8-host-strategy-refinement.md exists and defines the implementation boundary: retarget `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` to `netstandard2.0`, replace SDK-local and `dotnet-format` references with package-managed references, handle `System.Text.Json`, keep one package id and asset root, and require both `.NET 8 SDK` and `.NET 10 SDK` proof lanes.
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj currently targets `net10.0` and references `Microsoft.CodeAnalysis.Workspaces` and `System.Composition.AttributedModel` from `$(MSBuildToolsPath)/DotnetTools/dotnet-format`; `src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstCodeFixProvider.cs` is the only analyzer source using code-fix and `System.Composition`, and `DataVaultTypedReadModelSourceGenerator.cs` uses `System.Text.Json`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` still forces the analyzer project reference to `TargetFramework=net10.0`, and `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj` still resolves Workspaces/composition assemblies from `dotnet-format`, matching the plan's stated cleanup boundary.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` still enforce the current `.NET 10 SDK` analyzer-host wording plus `analyzers/dotnet/cs/DCoding.Data.DVault.Analyzers.dll` and `.xml` expectations; `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, `docs/local-validation.md`, and `docs/manual-nuget-publication.md` still document the same baseline.
- `git show --stat --oneline 8bfb7f28c` shows the PO handoff commit added `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` and refreshed the ticket description/metadata; `git diff --stat develop...HEAD` shows no product or test code changes yet, which is consistent with a design-only pre-development ticket.
- Ticket comment `.gicket/tickets/06FH8QRPDP10ZBAF3A5RYQFFQM/comments/06FH99M15QGWS23VDDBX37P7EW.md` repeats the bounded scope and `Open questions - none`; later comments are orchestration and lease comments only.
- Relation files `.gicket/relations/QM/SR/06FH8QRPDP10ZBAF3A5RYQFFQM--06FH8QAVJFXANVQFXGPYVAFXSR--blocks.json` and `.gicket/relations/QM/AM/06FH8QRPDP10ZBAF3A5RYQFFQM--06FH8R33YACW00JA0GNVEDP1AM--blocks.json` show this ticket is already the upstream blocker for the downstream story and implementation task.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The reviewed companion assemblies, if needed beside the main analyzer DLL under `analyzers/dotnet/cs/`, will load cleanly on both `.NET 8 SDK` and `.NET 10 SDK` hosts without needing a later asset split.
- The current `net10.0`-only analyzer source can be backfilled to `netstandard2.0` with bounded compatibility helpers instead of reopening the package-shape decision.
- CLI proof on `.NET 8 SDK` and `.NET 10 SDK` hosts will be sufficient for the repository claim; IDE-host behavior is intentionally left as follow-up risk, not part of this ticket's blocker set.

AC / test suggestions
- When the implementation ticket lands, require a packed-package proof that a `net8.0` consumer on a `.NET 8 SDK` host restores, loads, and executes the analyzer/generator path, not only a project-reference build.
- Extend package-verifier coverage to the exact reviewed analyzer asset set under `analyzers/dotnet/cs/`, including any companion assemblies added by dependency normalization.
- Keep a `.NET 10 SDK` regression lane that exercises the same analyzer package shape so the new claim stays two-host, not `.NET 8`-only.

Implementation watchouts
- Do not turn dependency normalization into a second public analyzer package id or a `lib/<tfm>` runtime asset story; the approved design keeps one package id and one analyzer asset root.
- Because `SuppressDependenciesWhenPacking=true` is currently set, companion assembly handling must be deliberate or analyzer/code-fix loading can silently fail after retargeting.
- Remove the current `TargetFramework=net10.0` and `dotnet-format` test harness assumptions in lockstep with pack-script, verifier, and documentation updates so validation actually matches the supported host story.

Non-blocking notes
- The repository still intentionally documents `.NET 10 SDK` as the only supported analyzer-host baseline today; the plan correctly keeps that wording in place until implementation and proof land.
- This branch currently carries planning-note and ticket-metadata changes only; for this pre-development design ticket, that is consistent with scope rather than a readiness gap.

Split recommendations
- No additional split is needed in this ticket. Keep it as the bounded design upstream of story `06FH8QAVJFXANVQFXGPYVAFXSR` and task `06FH8R33YACW00JA0GNVEDP1AM`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment