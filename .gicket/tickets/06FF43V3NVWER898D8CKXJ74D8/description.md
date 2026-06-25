<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the current v0.47.0 analyzer compatibility baseline: keep the analyzer package on one `net10.0` asset, require a `.NET 10 SDK` build host for both `8.47.0` and `10.47.0` consumers, and treat pure `.NET 8 SDK` analyzer consumption as separate future scope.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No new planning writes or relation changes were needed for refinement; the current branch already contains `docs/plans/analyzer-package-compatibility-audit.md` plus aligned README and package-verifier guidance for the v0.47.0 baseline.
- For this ticket, the only ratified current recommendation is to keep `DCoding.Data.DVault.Analyzers` on one `net10.0` analyzer asset and make the `.NET 10 SDK` build-host requirement explicit for both coordinated package lines.
- Pure `.NET 8 SDK` analyzer consumption is not a current compatibility claim; any lower-target or multi-target analyzer asset option belongs to separate additive work only if that product requirement is explicitly adopted.

### Scope In
- Audit the current build-host compatibility baseline for `DCoding.Data.DVault.Analyzers` when `net8.0` consumers use the `8.47.0` package line.
- Record the supported analyzer asset shape, the blocker to broader host comfort, and the resulting package-verification expectations.
- Ratify the repository's current bounded recommendation from checked-in evidence instead of reopening already-fixed baseline decisions.
- Capture the recommendation, risks, and follow-up boundary so downstream work does not overstate compatibility.

### Scope Out
- Retargeting the analyzer package to `net8.0`, `netstandard2.0`, or multi-target assets in this ticket.
- Changing runtime or provider package target frameworks, dependency lines, or coordinated package-family structure.
- General analyzer feature, diagnostic, or source-generator work unrelated to build-host compatibility.
- Claiming pure `.NET 8 SDK` analyzer consumption support without an explicit new verification lane.

## Acceptance Criteria
- The ticket records the recommendation that both coordinated analyzer package lines continue to ship the same `net10.0` analyzer asset and therefore require a `.NET 10 SDK` build host.
- The ticket cites local proof from `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`, `tools/pack-release-packages.sh`, `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`, `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/manual-nuget-publication.md`, `docs/package-compatibility.md`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`.
- The ticket records the blocker to lower-friction host support: the repository does not validate pure `.NET 8 SDK` analyzer consumption, so reducing host friction beyond the documented `.NET 10 SDK` baseline requires an explicit asset-target and verification change.
- The ticket records package-verification expectations that packaged README content must include the `.NET 10 SDK` analyzer-host guidance and must not claim unsupported pure `.NET 8 SDK` compatibility.
- The ticket records the current bounded recommendation rather than leaving analyzer target options open when the repository already supports one safe default baseline.

## Definition of Done
- The PO handoff captures the recommendation, blockers, risks, verification expectations, and bounded follow-up decision.
- The audit note at `docs/plans/analyzer-package-compatibility-audit.md` remains aligned with the ticket recommendation for the `8.47.0` and `10.47.0` package lines.
- Repository installation guidance and package-compatibility documentation consistently describe the `.NET 10 SDK` build-host requirement for analyzer use on both package lines.
- Package verification continues to enforce the analyzer-host guidance so packaged README output does not drift into broader unsupported claims.

## Implementation Notes
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0` and packs analyzer DLL/XML under `analyzers/dotnet/cs/`, not consumer-runtime `lib/<tfm>` folders.
- `tools/pack-release-packages.sh` packs the analyzer project once for `8.47.0` and once for `10.47.0` without changing the analyzer target framework, so both package lines currently carry the same analyzer binary shape.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0` and pins the analyzer project reference with `SetTargetFramework="TargetFramework=net10.0"`, which is the strongest local proof of the intended host baseline.
- `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/manual-nuget-publication.md`, `docs/package-compatibility.md`, and `docs/local-validation.md` already document the `.NET 10 SDK` build-host baseline for analyzer consumption.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` checks for the required analyzer-host guidance and rejects contradictory pure `.NET 8 SDK` claims in packaged README content.
- `docs/plans/shared-implementation-standards.md` already allows analyzer, tooling, benchmark, and repository helper projects to stay on `net10.0` when they are not consumer runtime packages.

## Open Questions
- none

## Follow-Up Questions
- If the product requirement becomes `net8.0` projects built on a pure `.NET 8 SDK` host, should that be tracked as a separate compatibility commitment with its own analyzer asset-target change and smoke/verification lane?

## Risks
- The current recommendation documents rather than removes `.NET 10 SDK` build-host friction for `net8.0` consumers; teams pinned to pure `.NET 8 SDK` toolchains still need separate product guidance or future work.
- Because both coordinated package lines ship the same analyzer asset, copied installation snippets can overstate compatibility unless the host-SDK caveat stays attached everywhere README content is surfaced.
- If a future change retargets analyzer assets without extending the verification lane, the repository could regress source-generator or analyzer behavior while appearing to broaden compatibility.

## Split Recommendations
- Do not split this audit further for current refinement; the bounded default is already clear from checked-in evidence.
- Create a separate additive ticket only if the team chooses to promise pure `.NET 8 SDK` analyzer consumption or another lower-friction host baseline.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Audit whether DCoding.Data.DVault.Analyzers can reduce the .NET 10 SDK build-host friction for net8 consumers without losing source-generator/analyzer behavior. Acceptance: records viable target assets, blockers, risks, package verification changes, and recommendation.