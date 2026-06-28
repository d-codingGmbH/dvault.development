[gicket-bot] PO-critic review contract

Summary
- Approved for dev: the persisted contract is concrete, file-backed, and free of unresolved open questions; it scopes a pre-development audit of analyzer/build-host compatibility rather than an implementation change.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Local inspection on branch `ticket/06FGX5GHPS7DEC3EJPWSKJZH28-task-audit-analyzer-roslyn-and-sdk-dependencies` at HEAD `ba914329bea5abcf7be0cd89fb93468ed2371b66` found no product-file diff vs `origin/main`; `git diff --name-only origin/main...HEAD` listed only `.gicket/**/*` paths, so this is still a metadata-only pre-development ticket-prep branch.
- `.gicket/tickets/06FGX5GHPS7DEC3EJPWSKJZH28/description.md` contains the persisted delivery contract with explicit Scope In/Out, 4 Acceptance Criteria, 4 Definition of Done items, and `## Open Questions` = `none`.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0`, sets `IncludeBuildOutput=false` and `SuppressDependenciesWhenPacking=true`, references `Microsoft.CodeAnalysis*` from `$(MSBuildToolsPath)` plus `Microsoft.CodeAnalysis.Workspaces` and `System.Composition.AttributedModel` from `$(MSBuildToolsPath)/DotnetTools/dotnet-format`, and packs the analyzer output under `analyzers/dotnet/cs/`.
- `src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstAnalyzer.cs` and `DataVaultEfCoreMisuseAnalyzer.cs` are `[DiagnosticAnalyzer]` types; `DataVaultMappingSourceGenerator.cs` and `DataVaultTypedReadModelSourceGenerator.cs` are `IIncrementalGenerator` types; `DataVaultCodeFirstCodeFixProvider.cs` is an `[ExportCodeFixProvider]` using `Microsoft.CodeAnalysis.Formatting` and `System.Composition`, matching the contract's slice split.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` targets `net8.0;net10.0` and references the analyzer project with `SetTargetFramework` forcing `TargetFramework=net10.0`; `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj` targets `net10.0` and adds `Microsoft.CodeAnalysis.CSharp.Workspaces` plus extra `System.Composition.*` references for analyzer/code-fix testing.
- `README.md`, `docs/package-compatibility.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, and `.github/workflows/ci.yml` all still enforce the `.NET 10 SDK` analyzer-host baseline and explicitly avoid claiming pure `.NET 8 SDK` analyzer consumption.
- A downstream blocked implementation ticket already exists: `.gicket/relations/28/1C/06FGX5GHPS7DEC3EJPWSKJZH28--06FGX5HRVFTMN221MK0R6AE41C--blocks.json` links this audit ticket to `06FGX5HRVFTMN221MK0R6AE41C` (`Task: Retarget or multi-target the analyzer package for supported SDK hosts`).

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- None required for developer handoff; this is an evidence-gathering audit ticket, not a direct behavior-change ticket.

Risky assumptions
- The audit must distinguish evidence-backed `no-go` conclusions from `follow-up-required` hypotheses; current repository state proves the `.NET 10 SDK` host baseline, not every alternative host strategy.
- Any future compatibility claim below the current host baseline will depend on SDK-local `$(MSBuildToolsPath)` and `DotnetTools/dotnet-format` coupling unless a follow-up ticket normalizes those references.

AC / test suggestions
- In the final audit note, explicitly separate `net8.0 consumer target + net10.0 analyzer host` from `pure .NET 8 SDK host` so the conclusion cannot be read as broader support.
- Call out which dependencies are product-package requirements versus analyzer-test-only requirements, especially `Microsoft.CodeAnalysis.CSharp.Workspaces` and the extra `System.Composition.*` assemblies in `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj`.
- Require each candidate strategy (`netstandard2.0`, `net8.0`, multi-targeted asset, split/separate asset) to end with an explicit `go`, `no-go`, or `follow-up-required` label plus a file-backed reason.

Implementation watchouts
- Do not blur runtime package target frameworks with analyzer build-host requirements; the current evidence only proves `net8.0` consumer targets under a `.NET 10 SDK` analyzer host.
- Treat the code-fix provider as the most coupled slice: it is the part directly tied to Workspaces/Formatting/System.Composition behavior, so it should not be assumed to move with analyzer/source-generator slices for free.
- Because the package suppresses dependency packing and ships one `analyzers/dotnet/cs/` asset, any broadened support claim will likely require coordinated packaging, verifier, documentation, and CI changes.

Non-blocking notes
- All observed ticket comments under `.gicket/tickets/06FGX5GHPS7DEC3EJPWSKJZH28/comments/` are automation/lease/handover records; no unresolved product discussion was present there.
- The current owner branch is ticket-metadata-only, which is consistent with the intended pre-development audit handoff.

Split recommendations
- If the audit recommends expanding supported SDK hosts, keep one follow-up for analyzer target/asset/Roslyn-reference changes and a second follow-up for CI, package-verifier, packaging, and documentation claim updates.
- If the audit confirms the code-fix provider is the only hard blocker, consider separating that slice from analyzer/source-generator assets instead of forcing every slice to retarget together.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment