<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratified the current analyzer compatibility baseline: one net10.0 analyzer asset for both package lines, verifier coverage for README/asset layout, and smoke coverage only for the supported .NET 10 SDK host path.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the v0.49.0 baseline: `DCoding.Data.DVault.Analyzers` ships one `net10.0` analyzer asset for both visible package lines (`8.50.0` and `10.50.0`), and supported consumption is a `.NET 10 SDK` build host.
- The ticket does not add or claim pure `.NET 8 SDK` analyzer-host support. The stale acceptance wording about `.NET 8 SDK-host behavior` is refined to smoke coverage for the supported `.NET 10 SDK` host, including `net8.0` consumer projects on the `8.50.0` line.
- Unsupported host combinations may be represented by deterministic package-verifier and documentation evidence; the repository does not need to invent a new pure `.NET 8 SDK` support claim or negative CI lane for this ticket.

### Scope In
- Deterministic package verification for analyzer README/build-host guidance and analyzer asset layout on both coordinated package lines.
- A bounded smoke proof for supported analyzer consumption on the repository `.NET 10 SDK` host baseline, especially the non-obvious `8.50.0` / `net8.0` consumer case.
- Alignment of README/package-verifier/test-lane behavior so unsupported pure `.NET 8 SDK` analyzer consumption is explicit rather than implied.

### Scope Out
- Retargeting `DCoding.Data.DVault.Analyzers` away from `net10.0`.
- Adding pure `.NET 8 SDK` analyzer-host support, CI, or package claims.
- Redesigning analyzer asset selection, multi-target packaging, or splitting analyzer/code-fix assets/packages.

## Acceptance Criteria
- Package verification fails if either `DCoding.Data.DVault.Analyzers` package line (`8.50.0` or `10.50.0`) is missing the expected `analyzers/dotnet/cs/` analyzer DLL/XML assets or the packaged README build-host guidance.
- Repository coverage includes a deterministic smoke proof that a consumer project can use `DCoding.Data.DVault.Analyzers` on the supported `.NET 10 SDK` host baseline, including the `8.50.0` / `net8.0` consumer line.
- The resulting docs/verifier/test evidence keeps pure `.NET 8 SDK` analyzer consumption explicitly unsupported on the current branch rather than silently assumed to work.
- The added coverage remains compatible with the normal repository validation flow (`dotnet build`, `dotnet test`, package pack, and package verify).

## Definition of Done
- Analyzer package verification coverage is present in-repo and fails closed on analyzer asset-layout or README-host-guidance drift.
- Smoke coverage for the supported analyzer host baseline is checked in and exercised by the existing validation lane.
- Any touched README or analyzer guidance text matches the verified `.NET 10 SDK` host baseline for both visible package lines.
- Relevant repository validation commands pass on the ticket branch.

## Implementation Notes
- Keep the current package shape as the v1 baseline: `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0` and packs the analyzer DLL plus XML documentation under `analyzers/dotnet/cs/`.
- `tools/pack-release-packages.sh` already packs the analyzer once per version line without a target-framework override, so both visible package lines intentionally carry the same analyzer binary shape with different versions.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` already contains analyzer README/build-host expectations; extend or ratify that existing verifier pattern rather than introducing a second packaging rule system.
- The existing repository evidence already proves the `net8.0` consumer lane against a `net10.0` analyzer asset on a `.NET 10 SDK` host baseline; new smoke coverage should build on that supported path instead of reopening the compatibility decision.
- The smoke proof should exercise the supported package-host claim, preferably through packaged analyzer consumption or an equivalent fixture that proves the same `.NET 10 SDK` host behavior for a `net8.0` consumer.

## Open Questions
- none

## Follow-Up Questions
- If product support for pure `.NET 8 SDK` analyzer hosts becomes required, should it be scheduled as a separate follow-up covering analyzer target/asset strategy, Roslyn dependency normalization, CI, and release-surface documentation?

## Risks
- A deterministic negative pure `.NET 8 SDK` host lane may remain outside the current validation baseline, so unsupported-host proof may rely on verifier/documentation evidence instead of an executed failure test.
- The analyzer project still depends on SDK-local Roslyn/Workspaces/composition assemblies, which keeps future host-support expansion higher risk until those dependencies are normalized.

## Split Recommendations
- No split is needed for the current bounded verifier/smoke/documentation-alignment work.
- If pure `.NET 8 SDK` analyzer-host support is later required, split it into one implementation ticket for retargeting or package-shape changes plus dependency normalization, then one validation/documentation/release-surface ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add deterministic verification for the analyzer build-host claim selected by this release.

Acceptance:
- Package verification checks the analyzer asset layout and README claim for both package lines.
- A small smoke project or test proves the supported .NET 8 SDK-host behavior when the audit and implementation make it feasible.
- Unsupported host combinations fail with clear package-verifier or documentation evidence rather than silent assumptions.
- The test lane remains compatible with the repository validation commands.