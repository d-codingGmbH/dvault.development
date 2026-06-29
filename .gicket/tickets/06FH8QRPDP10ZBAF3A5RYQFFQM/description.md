<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the current analyzer packaging constraints and added `docs/plans/analyzer-dotnet8-host-strategy-refinement.md`, which turns the v0.50 audit into one concrete design: keep one `DCoding.Data.DVault.Analyzers` package id, retarget the analyzer asset to `netstandard2.0`, and require explicit dependency, verifier, validation, and documentation updates before claiming pure `.NET 8 SDK` analyzer-host support.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current repository baseline is still one `net10.0` analyzer asset packed into `analyzers/dotnet/cs/`, with README and package-verifier guidance that explicitly require a `.NET 10 SDK` host for both `8.50.0` and `10.50.0` package lines.
- A ticket-bound planning note was materialized at `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` and is the authoritative refinement artifact for this ticket.
- No child tickets, relation changes, or ticket-description writes were materialized in this run; the current ticket remains a bounded design/planning item.

### Scope In
- Choose one supported analyzer package shape for future pure `.NET 8 SDK` host support and reject the unresolved alternatives.
- Document the required dependency strategy for Roslyn, `Microsoft.CodeAnalysis.Workspaces`, `System.Composition`, and `System.Text.Json`.
- Define the required analyzer package path, pack-script, package-verifier, test-lane, and documentation updates needed before the repository may claim pure `.NET 8 SDK` analyzer-host support.

### Scope Out
- Retargeting or editing product code, test projects, pack targets, or package verifier implementation in this ticket.
- Adding a second public analyzer package id or widening the coordinated nine-package family.
- Publishing packages or updating release-claim docs to state pure `.NET 8 SDK` analyzer-host support before both `.NET 8 SDK` and `.NET 10 SDK` proof lanes exist.

## Acceptance Criteria
- The ticket records one chosen analyzer strategy: one `netstandard2.0` `DCoding.Data.DVault.Analyzers` asset under `analyzers/dotnet/cs/`, not dual `net8.0`/`net10.0` analyzer assets and not a new split analyzer/code-fix package family.
- The authoritative plan explicitly covers Roslyn reference normalization, bounded Workspaces and `System.Composition` handling for the existing code-fix provider, and explicit `System.Text.Json` handling for `DataVaultTypedReadModelSourceGenerator`.
- The authoritative plan defines the required package-verifier contract change from the current single-analyzer-asset assumption to the reviewed analyzer asset set required by the chosen strategy.
- The authoritative plan defines the proof boundary: a pure `.NET 8 SDK` consumer build lane and a `.NET 10 SDK` regression lane must both pass before repository documentation may claim pure `.NET 8 SDK` analyzer-host support.
- The authoritative plan identifies the repository documentation surfaces that must be updated together: `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, and the release notes.

## Definition of Done
- The planning note `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` exists and captures the verified baseline, chosen package shape, required implementation boundary, required validation lanes, and release-surface updates.
- The ticket no longer leaves the package-shape decision open between `netstandard2.0`, dual target-specific analyzer assets, or a split code-fix package story.
- The ticket preserves the current package-line alignment rule: consumers still choose exactly one visible package-version line and keep analyzer references local with `PrivateAssets="all"`.
- The ticket makes explicit that the current `.NET 10 SDK`-only analyzer-host wording stays in place until the planned implementation and proof work land.

## Implementation Notes
- Repository evidence driving the decision includes `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`, `tools/pack-release-packages.sh`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`, and `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj`.
- The chosen strategy keeps one analyzer package id and one primary analyzer asset root because the repository already ratifies `analyzers/dotnet/cs/` plus a nine-package coordinated family; widening that surface is out of scope for this design ticket.
- The only production slice that currently needs Workspaces and `System.Composition` is `DataVaultCodeFirstCodeFixProvider.cs`, so the plan normalizes those dependencies instead of promoting a new public package split.
- The current ticket-bound refinement artifact is `docs/plans/analyzer-dotnet8-host-strategy-refinement.md`; downstream implementation can execute directly from that note.

## Open Questions
- none

## Follow-Up Questions
- If real `.NET 8 SDK` host proof shows that companion analyzer dependencies still do not load cleanly for the code-fix slice, should a later delivery ticket split code fixes into an optional package or asset set after this design baseline is implemented?
- If the team wants explicit IDE-host proof beyond CLI `.NET 8 SDK` and `.NET 10 SDK` build lanes, should that be scheduled as a separate validation follow-up rather than broaden this design ticket?

## Risks
- Retargeting to `netstandard2.0` is not a csproj-only change: analyzer sources currently use modern BCL APIs and framework assumptions that will need bounded compatibility work.
- The package-verifier and README baselines currently hard-code the `.NET 10 SDK` host claim and a flat single-analyzer-asset expectation; those guardrails must change in lockstep with implementation or they will misreport the new package shape.
- If the reviewed analyzer companion-assembly strategy under `analyzers/dotnet/cs/` proves insufficient on actual `.NET 8 SDK` or IDE hosts, the later implementation may still need a narrower asset split despite this design decision.

## Split Recommendations
- No additional split is justified inside this design ticket; use `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` as the bounded handoff artifact for the later implementation ticket that changes project references, packing, verifier coverage, tests, and docs.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Turn the v0.50 analyzer audit into a concrete implementation plan. Decide whether the supported package shape is netstandard2.0, net8.0 plus net10.0, or split analyzer/code-fix assets. Cover Roslyn, Workspaces, System.Composition, System.Text.Json, analyzer package paths, and package-verifier expectations. Non-goal: silently keeping the v0.50 net10-only baseline.