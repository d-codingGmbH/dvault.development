<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Narrowed the parent to the landed 8.50.0/10.50.0 analyzer-host baseline, queued the 8.51.0/10.51.0 follow-up rewrite on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, and materialized relation cleanup for stale child blockers.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Applied a parent description rewrite on ticket 06FH8QAVJFXANVQFXGPYVAFXSR at revision 06FHCB97DQBFJGPE5SZKK8MZX4 to align the story with the landed 8.50.0/10.50.0 analyzer-host baseline.
- Removed the parent-owned blocks relation to 06FH8RP1SBVZ7K3K48ERGZSMQC at revision 06FHCBCKYJ9QCSZR4JT9SN52JW so the future roll-forward no longer gates this parent.
- Queued replay on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC owner branch for the follow-up delivery contract update as outbox mutation-b90172254935d5d4.

### Scope In
- Track the already-landed analyzer-host compatibility baseline for package lines 8.50.0 and 10.50.0: one netstandard2.0 DCoding.Data.DVault.Analyzers asset under analyzers/dotnet/cs/.
- Track repository-backed validation and guidance for packaged analyzer consumption on pure .NET 8 SDK and .NET 10 SDK build hosts, including PrivateAssets=all consumer guidance.
- Track closure alignment between the implemented repo baseline, this parent story contract, and the completed strategy, implementation, proof, and documentation child tickets.

### Scope Out
- Any 8.51.0 / 10.51.0 release-note, changelog, install-guidance, package-validation, or publish-baseline roll-forward; that work belongs to ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.
- New analyzer package ids, split code-fix packages, target-specific analyzer asset trees, or runtime lib/<tfm> assets.
- Analyzer-host compatibility claims beyond the repository-backed .NET 8 SDK and .NET 10 SDK CLI build-host boundary.

## Acceptance Criteria
- The parent contract states only the implemented baseline visible in repository evidence: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets netstandard2.0, and the analyzer package remains one asset root under analyzers/dotnet/cs/.
- Repository validation evidence for both supported analyzer hosts remains the bounded proof surface for this story: tools/pack-release-packages.sh, tools/run-analyzer-package-smoke.sh 8, tools/run-analyzer-package-smoke.sh 10, and package verification.
- Consumer guidance for this story stays on the current visible package lines 8.50.0 and 10.50.0, with local analyzer references using PrivateAssets=all and no mixed-line install guidance.
- Future 8.51.0 / 10.51.0 release-surface movement is explicitly excluded from this parent and handed to ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.

## Definition of Done
- The strategy, implementation, smoke/verifier, and documentation child tickets for the analyzer-host baseline are complete and remain consistent with current repository evidence.
- This parent no longer owns a live blocks dependency on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, and stale child-to-parent blocks removals have been materialized as applied or queued source-owner relation cleanups.
- No ticket text for this parent reintroduces the superseded .NET 10 SDK-only analyzer-host assumption or mixes the landed 8.50.0 / 10.50.0 baseline with future 8.51.0 / 10.51.0 release wording.

## Implementation Notes
- Repository evidence for the landed baseline comes from src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj, docs/package-compatibility.md, docs/plans/analyzer-dotnet8-host-strategy-refinement.md, docs/plans/analyzer-package-compatibility-audit.md, tools/pack-release-packages.sh, tools/run-analyzer-package-smoke.sh, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs.
- The prompt ticket snapshot showed pre-fix mixed baseline wording, but that snapshot is superseded by the applied parent description update at revision 06FHCB97DQBFJGPE5SZKK8MZX4.
- Follow-up ticket 06FH8RP1SBVZ7K3K48ERGZSMQC received a queued description rewrite on its owner branch (mutation-b90172254935d5d4) because branch ownership prevented direct in-branch application from this parent branch.
- Stale incoming child blocks removals were queued on the canonical child owner branches as mutation-f7489d469498b768, mutation-cf2960f5d1f39511, mutation-de636096dd1d95db, and mutation-4c86dae092d52c65.

## Open Questions
- none

## Follow-Up Questions
- Confirm replay completion for the queued follow-up description update on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC so its branch carries the authoritative 8.51.0 / 10.51.0 delivery contract.
- If future host claims need IDE or editor validation beyond CLI SDK-host proof, schedule that as a separate follow-up rather than broadening this parent story.

## Risks
- Until queued replay finishes on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, the follow-up's persisted description may temporarily lag the intended 8.51.0 / 10.51.0 delivery contract.
- A later package-line roll-forward can drift if changelog, release notes, install guidance, pack script, and package verification are not updated together on the follow-up ticket.

## Split Recommendations
- No additional split is needed; this parent is now bounded to the landed 8.50.0 / 10.50.0 baseline, and ticket 06FH8RP1SBVZ7K3K48ERGZSMQC is the single remaining carrier for the future 8.51.0 / 10.51.0 release-surface work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (superseded legacy context)

The delivery contract above is authoritative. The original draft is intentionally not retained as executable scope because it used future package-line wording that conflicts with this parent story's landed 8.50.0 / 10.50.0 baseline. Treat the parent scope, acceptance criteria, and Definition of Done in the delivery contract as the only active ticket text.

<!-- gicket-bot:developer-rework-closure:v1:start -->
## Developer Rework Closure

### Rework Summary
- Tester returned this parent story because persisted acceptance criteria, Definition of Done expectations, or checklist gates were not fully confirmed.
- This rework resolves that gap with explicit closure evidence for the already-integrated parent baseline; no product repository file change is required.
- The parent story remains scoped to the landed `8.50.0` / `10.50.0` analyzer-host baseline. Future `8.51.0` / `10.51.0` release-surface work remains outside this parent and belongs to ticket `06FH8RP1SBVZ7K3K48ERGZSMQC`.

### Acceptance Criteria Closure
- AC: `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets `netstandard2.0` and the analyzer package remains one asset root under `analyzers/dotnet/cs/`.
  - Confirmed in `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`: `<TargetFramework>netstandard2.0</TargetFramework>`.
  - Confirmed in target `AddAnalyzerPackageAssets`: the analyzer DLL, XML documentation, and approved companion assemblies are packed with `PackagePath="analyzers/dotnet/cs/"`.
  - Confirmed in packed artifacts `artifacts/packages/DCoding.Data.DVault.Analyzers.8.50.0.nupkg` and `artifacts/packages/DCoding.Data.DVault.Analyzers.10.50.0.nupkg`: both contain `analyzers/dotnet/cs/DCoding.Data.DVault.Analyzers.dll`, `analyzers/dotnet/cs/DCoding.Data.DVault.Analyzers.xml`, `Microsoft.CodeAnalysis.CSharp.Workspaces.dll`, `Microsoft.CodeAnalysis.Workspaces.dll`, `System.Composition.*.dll`, and `System.Text.Json.dll`.
  - Note: `analyzers/dotnet/cs/` is a NuGet package archive path, not a repository source directory to check in.
- AC: repository validation evidence for both supported analyzer hosts remains bounded to the pack script, SDK 8 smoke, SDK 10 smoke, and package verification.
  - Confirmed in `tools/pack-release-packages.sh`: `pack_line "8.50.0" "net8.0"` and `pack_line "10.50.0" "net10.0"`, with `pack_analyzer_line` called for each package line.
  - Confirmed in `tools/run-analyzer-package-smoke.sh`: SDK major `8` maps to `net8.0` / `8.50.0`, SDK major `10` maps to `net10.0` / `10.50.0`, `global.json` disables roll-forward, and the temporary consumer references `DCoding.Data.DVault.Analyzers` with `PrivateAssets="all"`.
  - Confirmed in `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs`: package verification enforces dual-host README guidance, `.NETStandard,Version=v2.0`, analyzer companion assets, analyzer package path, and mixed-line package rejection.
- AC: consumer guidance stays on current visible package lines `8.50.0` and `10.50.0`, with local analyzer references using `PrivateAssets=all` and no mixed-line install guidance.
  - Confirmed in `README.md` under the install/analyzer guidance around the quoted text `Build projects that reference `DCoding.Data.DVault.Analyzers` with either a `.NET 8 SDK` or `.NET 10 SDK` host.`
  - Confirmed in `docs/package-compatibility.md` under `Package Lines` and the analyzer paragraph: package lines are `8.50.0` / `net8.0` and `10.50.0` / `net10.0`, and the analyzer package ships one `netstandard2.0` asset under `analyzers/dotnet/cs/`.
  - Confirmed in `docs/manual-nuget-publication.md` under `Current Consumer Guidance`, `Required Pre-Publish Evidence`, and `Version And Dependency Alignment`.
- AC: future `8.51.0` / `10.51.0` work is excluded from this parent.
  - Confirmed by the authoritative delivery contract `Scope Out` section on this ticket and by the absence of `8.51.0` / `10.51.0` package-line changes in the repository paths inspected during rework.

### Definition Of Done Closure
- DoD: strategy, implementation, smoke/verifier, and documentation child work remains consistent with repository evidence.
  - Confirmed by `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` under `Chosen Strategy`, which names one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/`.
  - Confirmed by `docs/plans/analyzer-package-compatibility-audit.md` under `Current Decision`, `Implemented Resolution`, and `Validation Contract`.
  - Confirmed by `docs/local-validation.md`, whose first command block includes `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `bash tools/pack-release-packages.sh`, both analyzer smoke lanes, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`.
- DoD: this parent no longer owns the future roll-forward blocks dependency.
  - The tester return evidence already confirmed the relation removal event for `06FH8QAVJFXANVQFXGPYVAFXSR--06FH8RP1SBVZ7K3K48ERGZSMQC--blocks`, and this rework keeps future roll-forward scope out of the parent.
- DoD: no parent ticket text should reintroduce the superseded .NET 10 SDK-only assumption or mix the landed baseline with future release wording.
  - This developer supplement states only the current `8.50.0` / `10.50.0` baseline and treats the legacy draft as superseded by the authoritative contract.

### Checklist Gate Confirmation
- The ticket snapshot has no explicit Definition of Ready checklist items and no explicit Definition of Done checklist items: both groups show `0/0 required`.
- Because there are no checkboxes to mutate, closure is represented by this persisted AC/DoD evidence matrix plus the verification evidence below.

### Fresh Rework Verification
- `dotnet --list-sdks` showed both supported hosts available: `8.0.422` and `10.0.301`.
- Package artifact inventory under `artifacts/packages` showed exactly eighteen `.nupkg` files for the nine package ids across `8.50.0` and `10.50.0`, and sixteen `.snupkg` files for the runtime, provider, and privacy packages.
- `dotnet tools/DCoding.Data.DVault.PackageVerification/bin/Release/net10.0/DCoding.Data.DVault.PackageVerification.dll` passed and reported valid package counts, metadata, dual-line README guidance, XML docs, symbols, analyzer assets, provider/privacy dependencies, and line-specific `net8.0` / `net10.0` EF dependency groups.
- `bash tools/run-analyzer-package-smoke.sh 8` passed on .NET SDK `8.0.422` with `0 Warning(s)` and `0 Error(s)`, then printed `DVault analyzer package smoke passed.`
- `bash tools/run-analyzer-package-smoke.sh 10` passed on .NET SDK `10.0.301` with `0 Warning(s)` and `0 Error(s)`, then printed `DVault analyzer package smoke passed.`
- `bash tools/check-format.sh` passed and reported `One-member-per-file check passed for 736 C# files.` and `Formatting check passed.`

### Tester Handoff
- Re-run `bash tools/verify-packages.sh` if tester wants the scripted verifier entrypoint rather than the prebuilt verifier assembly used in this bounded rework pass.
- Re-run `bash tools/run-analyzer-package-smoke.sh 8` and `bash tools/run-analyzer-package-smoke.sh 10` to confirm the packed analyzer package loads and executes on both supported SDK hosts.
- Inspect `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` target `AddAnalyzerPackageAssets` and the package entries in `artifacts/packages/DCoding.Data.DVault.Analyzers.8.50.0.nupkg` / `artifacts/packages/DCoding.Data.DVault.Analyzers.10.50.0.nupkg` for the `analyzers/dotnet/cs/` asset-root evidence.
<!-- gicket-bot:developer-rework-closure:v1:end -->

<!-- gicket-bot:developer-rework-closure:v2:start -->
## Developer Rework Closure V2

### Purpose
- This block resolves the repeated tester return for persisted acceptance criteria, Definition of Done, and checklist confirmation.
- It supersedes the older developer rework closure block if both are present.
- No product repository file change is required for this parent story; the parent tracks the already-landed `8.50.0` / `10.50.0` analyzer-host baseline.

### Acceptance Criteria Confirmation
- Analyzer package shape: `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` contains `<TargetFramework>netstandard2.0</TargetFramework>` and target `AddAnalyzerPackageAssets` packs the analyzer DLL, XML docs, Workspaces, System.Composition, and System.Text.Json companion assemblies with `PackagePath=analyzers/dotnet/cs/`.
- Package asset path: `analyzers/dotnet/cs/` is a NuGet package archive path, not a repository source directory. The packed analyzer packages contain that path.
- Package lines: `tools/pack-release-packages.sh` keeps `pack_line 8.50.0 net8.0` and `pack_line 10.50.0 net10.0`, and packs the analyzer package once for each visible line.
- Host proof: `tools/run-analyzer-package-smoke.sh` maps SDK major `8` to `net8.0` / `8.50.0`, maps SDK major `10` to `net10.0` / `10.50.0`, disables SDK roll-forward, and uses `DCoding.Data.DVault.Analyzers` with `PrivateAssets=all`.
- Consumer guidance: `README.md`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md` document the `8.50.0` and `10.50.0` lines, local analyzer references with `PrivateAssets=all`, no mixed-line installation, and dual `.NET 8 SDK` / `.NET 10 SDK` analyzer-host support.
- Future scope: `8.51.0` / `10.51.0` work remains excluded from this parent and belongs to ticket `06FH8RP1SBVZ7K3K48ERGZSMQC`.

### Definition Of Done Confirmation
- Strategy alignment is documented in `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` under `Chosen Strategy`, which names one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/`.
- Implementation and verifier alignment are documented in `docs/plans/analyzer-package-compatibility-audit.md` under `Current Decision`, `Implemented Resolution`, and `Validation Contract`.
- Package verification is enforced by `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs`, including expected dual-host README guidance, `.NETStandard,Version=v2.0`, companion assets, analyzer archive path, and mixed-line rejection checks.
- The parent no longer owns the future roll-forward blocks dependency; tester evidence already confirmed removal of `06FH8QAVJFXANVQFXGPYVAFXSR--06FH8RP1SBVZ7K3K48ERGZSMQC--blocks`.

### Checklist Gate Confirmation
- The ticket snapshot has no explicit Definition of Ready checklist items and no explicit Definition of Done checklist items. Both checklist groups are `0/0 required`.
- Because there are no checklist boxes to mutate, this persisted evidence block is the checklist closure record for the parent story.

### Fresh Dev Verification
- Branch: `ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp` at `6adf3f0c3289`.
- Product diff check: `git diff --name-only develop...HEAD -- .` excluding `.gicket/**` and `.gicket-bot/**` returned no product repository paths.
- SDK hosts: `dotnet --list-sdks` reported `8.0.422` and `10.0.301`.
- Package verifier: `dotnet tools/DCoding.Data.DVault.PackageVerification/bin/Release/net10.0/DCoding.Data.DVault.PackageVerification.dll` passed for `artifacts/packages`, confirming exactly eighteen `.nupkg` files, sixteen `.snupkg` files, metadata, dual-line README guidance, XML docs, symbols, analyzer assets, provider/privacy dependencies, and line-specific `net8.0` / `net10.0` EF dependency groups.
- .NET 8 analyzer smoke: `bash tools/run-analyzer-package-smoke.sh 8` passed on SDK `8.0.422` with `0 Warning(s)` and `0 Error(s)`, then printed `DVault analyzer package smoke passed.`
- .NET 10 analyzer smoke: `bash tools/run-analyzer-package-smoke.sh 10` passed on SDK `10.0.301` with `0 Warning(s)` and `0 Error(s)`, then printed `DVault analyzer package smoke passed.`
- Formatting: `bash tools/check-format.sh` passed and reported `One-member-per-file check passed for 736 C# files.` and `Formatting check passed.`

### Policy Build/Test Status
- `dotnet build DVault.slnx --nologo` was attempted in this dev rework pass. It failed before build because NuGet restore tried to create a temporary file under `src/DCoding.Data/obj/` and the current execution sandbox exposed that repository output path as read-only.
- This is a local runtime/sandbox write-permission blocker, not evidence of a product implementation failure.
- `dotnet test DVault.slnx --nologo` was not run after the build command failed on the same repository write precondition.

### Tester Pointers
- Inspect `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` at `<TargetFramework>netstandard2.0</TargetFramework>` and target `AddAnalyzerPackageAssets`.
- Inspect `tools/pack-release-packages.sh` at the two `pack_line` entries for `8.50.0` / `net8.0` and `10.50.0` / `net10.0`.
- Inspect `tools/run-analyzer-package-smoke.sh` at SDK cases `8` and `10`, `rollForward disable`, and the analyzer `PrivateAssets=all` PackageReference.
- Inspect `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` at `ExpectedAnalyzerBuildHostGuidance`, `ExpectedAnalyzerTargetFrameworkMoniker`, and `ExpectedAnalyzerCompanionAssets`.
- Inspect `docs/package-compatibility.md` under `Package Lines` and `Analyzer Baseline`.
<!-- gicket-bot:developer-rework-closure:v2:end -->