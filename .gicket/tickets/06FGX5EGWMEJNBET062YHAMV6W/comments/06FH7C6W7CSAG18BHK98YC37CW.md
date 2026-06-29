[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the contract is clear, `## Open Questions` is `none`, and direct repository plus ticket evidence consistently define the current no-go baseline as one `net10.0` analyzer asset on a `.NET 10 SDK` host for both `8.50.0` and `10.50.0`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FGX5EGWMEJNBET062YHAMV6W/description.md contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` = `none`.
- `git symbolic-ref --short HEAD` shows branch `ticket/06FGX5EGWMEJNBET062YHAMV6W-story-make-analyzer-consumption-viable-for-net-8` at HEAD `b571a3392`; `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06FGX5EGWMEJNBET062YHAMV6W/...` metadata files.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets `net10.0`, sets `IncludeBuildOutput=false` and `SuppressDependenciesWhenPacking=true`, references Roslyn from `$(MSBuildToolsPath)` plus Workspaces and `System.Composition.AttributedModel` from `$(MSBuildToolsPath)/DotnetTools/dotnet-format`, and packs only `analyzers/dotnet/cs/` assets.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0` but forces `SetTargetFramework=TargetFramework=net10.0` for the analyzer `ProjectReference`.
- `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `.github/workflows/ci.yml`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` all preserve the `.NET 10 SDK` analyzer-host baseline and reject pure `.NET 8 SDK` claims.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocker: the contract could show one concrete supported-path example as a `net8.0` consumer project plus `.NET 10 SDK` host, but the cited integration project evidence already anchors that matrix.

Risky assumptions
- The ticket title still reads as positive `.NET 8 SDK` host enablement, so downstream roles must follow the delivery contract rather than the title wording.

AC / test suggestions
- Keep acceptance and test evidence explicit that supported analyzer consumption is a `.NET 10 SDK` host plus one `net10.0` analyzer asset for both `8.50.0` and `10.50.0`.
- If downstream work touches package surfaces, re-check `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `.github/workflows/ci.yml`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` together so no pure `.NET 8 SDK` claim slips in.

Implementation watchouts
- Do not reopen analyzer retargeting, multi-targeting, or a new `.NET 8 SDK` validation lane inside this story; the refined scope is the repository-backed no-go contract.
- Branch history currently shows ticket metadata only; any downstream code or doc changes should be tied to a specific uncovered gap, not inferred from the story title alone.

Non-blocking notes
- The PO refinement comment `06FH79Q582RKC84KRA1GNKZ3NC.md` and the persisted description are aligned on scope, risks, open-question closure, and the no-go analyzer-host decision.

Split recommendations
- If pure `.NET 8 SDK` analyzer-host support is later reopened as a product requirement, keep it split into an analyzer asset or dependency strategy ticket and a separate proof, CI, package-verifier, and documentation ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment