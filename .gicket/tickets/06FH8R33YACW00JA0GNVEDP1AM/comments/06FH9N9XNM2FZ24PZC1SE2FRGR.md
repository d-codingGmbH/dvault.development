[gicket-bot] PO refinement contract

Summary
- Refined the implementation ticket around the already-decided single-asset `netstandard2.0` analyzer strategy from done ticket `06FH8QRPDP10ZBAF3A5RYQFFQM`; current repository evidence still shows a `net10.0` analyzer asset, SDK-local Roslyn and Workspaces references, and `.NET 10 SDK` host guidance, so this ticket now carries the implementation, proof, verifier, and documentation boundary needed before claiming pure `.NET 8 SDK` analyzer support.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Done ticket `06FH8QRPDP10ZBAF3A5RYQFFQM` already fixed the package-shape decision; this ticket implements that chosen `netstandard2.0` single-asset strategy from `docs/plans/analyzer-dotnet8-host-strategy-refinement.md`, not a fresh design fork.
- Current repository evidence still shows one `net10.0` analyzer asset packed under `analyzers/dotnet/cs/`, SDK-local Roslyn references from `$(MSBuildToolsPath)`, Workspaces and `System.Composition` references from `dotnet-format`, and README, package-verifier, and test guidance that requires a `.NET 10 SDK` host.
- No child tickets, relation changes, description updates, attachments, or new planning documents were materialized in this refinement run.

Scope In
- Retarget `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` from `net10.0` to one `netstandard2.0` analyzer asset while keeping the existing `DCoding.Data.DVault.Analyzers` package id and `analyzers/dotnet/cs/` asset root.
- Replace SDK-local Roslyn, Workspaces, and `System.Composition` file references with reviewed package-managed build inputs and a compatible analyzer packaging layout.
- Add explicit compatibility handling for `System.Text.Json` and any other `netstandard2.0` gaps used by `DataVaultTypedReadModelSourceGenerator` or other analyzer sources.
- Update pack, package-verification, analyzer tests, integration tests, and documentation so pure `.NET 8 SDK` and `.NET 10 SDK` analyzer hosts are both proven and described consistently.
- Preserve XML documentation, `DevelopmentDependency=true`, local `PrivateAssets='all'` consumer guidance, and no runtime `lib/<tfm>` dependency leakage.

Scope Out
- Introducing dual `net8.0` and `net10.0` analyzer assets under `analyzers/dotnet/cs/`.
- Creating a new public analyzer or code-fix package id or widening the coordinated nine-package family.
- Changing consumer package-line rules away from the existing aligned `8.50.0` and `10.50.0` lines.
- Claiming pure `.NET 8 SDK` analyzer support before both required proof lanes pass.

Open questions
- none

Follow-up questions
- If CLI proof passes but IDE-host loading still fails because of companion Workspaces or composition assemblies, should a later ticket isolate code-fix-specific assets or add an IDE-host validation lane?
- After the bounded CLI `.NET 8 SDK` and `.NET 10 SDK` proof lanes land, does the team want separate editor or IDE compatibility evidence before making broader support statements?

Risks
- Retargeting to `netstandard2.0` is not a csproj-only change; analyzer and generator code may need bounded compatibility helpers for APIs that currently rely on the `net10.0` BCL.
- Because `SuppressDependenciesWhenPacking=true` remains part of the analyzer package posture, missing or mismatched companion assemblies can leave the package compiling successfully but failing to load under real analyzer hosts.
- Package verifier, README text, and test harnesses currently hard-code the `.NET 10 SDK` host baseline, so partial implementation will create false-positive or false-negative validation signals.
- The code-fix slice is the main dependency-coupled area; if Workspaces and `System.Composition` normalization proves host-fragile, delivery may require a narrower follow-up after the bounded implementation lands.

Split recommendations
- No additional split is required before PO-critic review if delivery stays inside the chosen single-package `netstandard2.0` strategy. If host-specific code-fix loading problems appear after CLI proof, create a follow-up instead of widening this ticket mid-stream.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment