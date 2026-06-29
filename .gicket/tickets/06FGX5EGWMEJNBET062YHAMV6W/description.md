<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh repository inspection shows the current branch already documents a pure .NET 8 SDK analyzer-host no-go; this refinement narrows the story to ratifying the existing .NET 10 SDK build-host baseline and keeping any true .NET 8 host enablement as follow-up split work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current branch evidence does not support a pure `.NET 8 SDK` analyzer-host claim. For this story, the bounded v1 outcome is the explicit no-go contract and support-matrix wording, not analyzer retargeting.
- Supported analyzer consumption remains one `net10.0` analyzer asset on a `.NET 10 SDK` build host for both coordinated package lines, including `net8.0` consumer projects on `8.50.0`.
- No bounded ticket writes, relation changes, attachments, or planning-document writes were materialized in this refinement run.

### Scope In
- Ratify the current supported analyzer-host baseline for `DCoding.Data.DVault.Analyzers`: one `net10.0` analyzer asset consumed on a `.NET 10 SDK` build host for `8.50.0` and `10.50.0`.
- Keep package-verifier and documentation surfaces explicit that pure `.NET 8 SDK` analyzer consumption is not a current compatibility claim.
- Capture the bounded blocker facts from current repository evidence: analyzer target/framework, SDK-local Roslyn/Workspaces/composition references, analyzer asset layout, dependency suppression, and absence of a `.NET 8 SDK` validation lane.

### Scope Out
- Retargeting the analyzer package to `net8.0` or `netstandard2.0`, adding multi-target analyzer assets, or splitting code-fix assets/packages in this ticket.
- Adding analyzer runtime dependencies to consumer applications or widening the DVault runtime surface.
- Introducing a new `.NET 8 SDK` CI or package-verification lane as part of this ticket's current no-go outcome.

## Acceptance Criteria
- The ticket contract states that the current supported analyzer-host matrix is a `.NET 10 SDK` build host for both `8.50.0` and `10.50.0`, with `DCoding.Data.DVault.Analyzers` shipped as one `net10.0` analyzer asset.
- The no-go rationale for pure `.NET 8 SDK` hosts is documented from current repository evidence: the analyzer project targets only `net10.0`, packs one `analyzers/dotnet/cs/` asset, suppresses dependency metadata, uses SDK-local Roslyn/Workspaces/composition references, and lacks a `.NET 8 SDK` validation lane.
- Package-verification and consumer-documentation guidance remain aligned to that matrix and explicitly reject contradictory pure `.NET 8 SDK` analyzer-host claims.
- Any future attempt to support pure `.NET 8 SDK` hosts is expressed as bounded follow-up work rather than implied inside this story.

## Definition of Done
- The PO handoff captures the repository-backed no-go contract instead of promising pure `.NET 8 SDK` analyzer support on the current branch.
- The contract leaves no unresolved PO-level baseline question about analyzer asset shape, build-host matrix, or coordinated package-line versions for this story.
- Recommended future work is split into bounded implementation and proof/documentation tracks so a later dev handoff can proceed without reopening scope.

## Implementation Notes
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` currently targets only `net10.0`, sets `IncludeBuildOutput=false` and `SuppressDependenciesWhenPacking=true`, and packs the analyzer DLL and XML under `analyzers/dotnet/cs/`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0` but its analyzer `ProjectReference` forces `TargetFramework=net10.0`, so the visible `net8.0` consumer lane still consumes the `net10.0` analyzer asset.
- `README.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `.github/workflows/ci.yml`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` all reinforce the `.NET 10 SDK` build-host baseline and do not prove a pure `.NET 8 SDK` host lane.
- The code-fix provider remains the highest-risk slice for any future retarget because the production code and analyzer test harness both depend on Workspaces and `System.Composition` from the SDK-local `dotnet-format` layout.

## Open Questions
- none

## Follow-Up Questions
- If pure `.NET 8 SDK` analyzer consumption becomes a product requirement, should the preferred technical direction be a single `net8.0` asset, a broader `netstandard2.0` asset, or a split that isolates code fixes from analyzers and generators?
- If that work is approved, should it be scheduled as two tickets: first analyzer asset/dependency strategy, then `.NET 8 SDK` proof plus CI/package-verifier/documentation updates?

## Risks
- Treating this as a direct implementation story without a split would hide two coupled workstreams: analyzer retargeting/dependency normalization and new `.NET 8 SDK` validation plus release-surface updates.
- Any attempt to claim pure `.NET 8 SDK` analyzer support before changing verifier and CI evidence would contradict the repository's current documentation and package checks.
- The code-fix provider's Workspaces and `System.Composition` coupling is the main technical risk for future host retargeting.

## Split Recommendations
- Follow-up ticket 1: retarget or split `DCoding.Data.DVault.Analyzers` for a supported `.NET 8 SDK` host, including Roslyn/Workspaces/composition/`System.Text.Json` dependency strategy and an explicit code-fix packaging decision.
- Follow-up ticket 2: after the analyzer asset strategy lands, add `.NET 8 SDK` proof surfaces across CI, package verifier, pack/release validation, analyzer README, root README, package-compatibility guidance, local validation, manual publication guidance, and release notes.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define and deliver the smallest compatible analyzer packaging path for .NET 8 build hosts if feasible. The current baseline requires a .NET 10 SDK host even for net8.0 consumer projects; this story should either remove that friction for the 8.x package line or produce a precise, tested no-go contract.

Acceptance:
- Analyzer Roslyn, code-fix, source-generator, and MSBuild asset dependencies are audited against a pure .NET 8 SDK build-host baseline.
- If feasible, the analyzer package ships an asset layout that can be consumed by net8.0 projects on a .NET 8 SDK host without adding runtime dependencies or widening the DVault runtime surface.
- If not feasible, the blocker is documented with exact API/tooling constraints and package verifier guidance stays explicit.
- Package verification and docs reflect exactly the supported build-host matrix for 8.50.0 and 10.50.0.

Non-goals:
- Supporting arbitrary old compiler versions.
- Adding analyzer runtime dependencies to consumer applications.
- Rewriting the analyzers or generators outside compatibility needs.