<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the already-ratified single-asset .NET 8/.NET 10 analyzer-host strategy: replace the current net10-only proof with packaged dual-host smoke coverage, update CI/local validation and package-verifier guardrails, and keep one analyzer package id and asset path.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows the current baseline is one `net10.0` analyzer asset under `analyzers/dotnet/cs/`, `.github/workflows/ci.yml` installs only `10.0.x`, and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` forces the analyzer project reference to `TargetFramework=net10.0` for the existing net8 smoke lane.
- Use `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` as the authoritative strategy for this ticket: support pure `.NET 8 SDK` and `.NET 10 SDK` hosts with one `netstandard2.0` `DCoding.Data.DVault.Analyzers` asset under the existing `analyzers/dotnet/cs/` path.
- For tester verification, `analyzers/dotnet/cs/` is the path inside the packed `.nupkg` analyzer archive, not a tracked repository directory. Verify it after `bash tools/pack-release-packages.sh` with `bash tools/verify-packages.sh`, `bash tools/run-analyzer-package-smoke.sh 8`, `bash tools/run-analyzer-package-smoke.sh 10`, or by inspecting the package archive entries.
- Deterministic proof for this ticket means a packaged-consumer restore/build/run path on a `.NET 8 SDK` host; the current in-solution project-reference smoke test is not sufficient because it compiles a `net8.0` consumer against a forced `net10.0` analyzer build.
- The visible repository package-line baseline is still `8.50.0` and `10.50.0`, not yet `8.51.0`; refine the work against those current guards and carry the same pattern forward when the next coordinated line is introduced.
- Keep the existing consumer alignment rule: one coordinated package-version line at a time, analyzer reference local with `PrivateAssets="all"`, and no second analyzer package id or target-specific package fork.

### Scope In
- Retarget `src/DCoding.Data.DVault.Analyzers` to the chosen single-asset compatibility strategy needed for pure `.NET 8 SDK` analyzer-host support, including package-managed Roslyn/Workspaces/System.Composition handling and explicit `System.Text.Json` compatibility where required.
- Replace the current net10-only analyzer-host smoke proof with deterministic packaged analyzer-consumer validation that exercises restore, build, and generated-analyzer execution on a `.NET 8 SDK` host, while preserving a corresponding `.NET 10 SDK` host proof.
- Update CI and repository validation guidance so the supported analyzer-host story is evidenced on both `.NET 8 SDK` and `.NET 10 SDK` hosts.
- Update `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and its unit tests so packaged README and analyzer-asset checks enforce the new support statement and reject stale net10-only guidance.
- Synchronize README, analyzer README, package compatibility, local validation, manual publication, and release-note text with the new dual-host analyzer support claim.

### Scope Out
- No second analyzer package id, no split code-fix package, and no separate target-specific analyzer asset tree beyond the one reviewed asset set under `analyzers/dotnet/cs/`.
- No consumer runtime `lib/<tfm>` assets or new runtime dependency contract for `DCoding.Data.DVault.Analyzers`; it remains a local build-time package.
- No widening of package-line mix rules; consumers still choose exactly one coordinated line and keep analyzer references local.
- No broader compatibility claim beyond the explicitly proved `.NET 8 SDK` and `.NET 10 SDK` analyzer-host story.

## Acceptance Criteria
- The repository produces one supported analyzer package shape for `DCoding.Data.DVault.Analyzers`: a single reviewed analyzer asset set under `analyzers/dotnet/cs/` that can be consumed by both `.NET 8 SDK` and `.NET 10 SDK` build hosts without reintroducing the old `net10.0`-only analyzer binary assumption.
- A deterministic smoke lane proves that a `net8.0` consumer project can restore the packed analyzer package, build, and execute generated analyzer output on a `.NET 8 SDK` host.
- A corresponding proof remains in place for a `.NET 10 SDK` host so the new support statement is dual-host, not `.NET 8`-only.
- CI and/or the repository validation entrypoints run the dual-host analyzer proof in a repeatable way, and `docs/local-validation.md` explains how maintainers reproduce it from the repository root.
- Package verification and its tests fail if packaged README guidance reverts to `.NET 10 SDK`-only analyzer-host language, mixed-line guidance, or other stale analyzer-host claims that contradict the new support contract.
- README, analyzer README, package compatibility, manual publication, and release notes all describe the same supported analyzer-host boundary and still preserve the one-line-at-a-time package alignment rule and `PrivateAssets="all"` guidance.

## Definition of Done
- The analyzer project no longer depends on SDK-local Roslyn or `dotnet-format` file paths as the basis of its supported package build.
- The existing integration and analyzer test projects no longer hard-code the old `TargetFramework=net10.0` analyzer-host assumption for the compatibility proof path.
- Package pack and verification flows still pass with the reviewed analyzer asset set, including XML documentation and any explicitly approved companion assemblies beside the analyzer DLL if the normalized dependency set requires them.
- All repository documentation surfaces that currently state `.NET 10 SDK`-only analyzer hosting are updated or intentionally removed so the shipped support statement is internally consistent.
- The solution has an automated regression check that would fail if the analyzer package reverts to a net10-only host baseline while still claiming pure `.NET 8 SDK` support.

## Implementation Notes
- The current `AnalyzerSdkHostSmokeTests` and the integration project reference override are evidence of the old compatibility boundary; replace them with the authoritative packaged-consumer host proof rather than only renaming the existing smoke test.
- Follow the already-ratified strategy in `docs/plans/analyzer-dotnet8-host-strategy-refinement.md`: one `netstandard2.0` analyzer asset, same package id, same `analyzers/dotnet/cs/` asset root, and no public package-family expansion.
- Keep `tools/pack-release-packages.sh` on the coordinated visible package lines and update only the analyzer build shape and validation logic needed to make those lines genuinely support dual SDK hosts.
- If the normalized analyzer dependency set requires companion assemblies for loadability, treat that file set as explicit package contract and verify it under `analyzers/dotnet/cs/`; do not convert the analyzer package into a runtime library package.
- Update `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` in lockstep with package-verifier logic so the stale-guidance guards cover both positive and negative analyzer-host cases.
- Keep repository docs and packaged README language synchronized; a source-only doc change or verifier-only change is not enough for this ticket.

## Open Questions
- none

## Follow-Up Questions
- When the coordinated package versions advance beyond the current `8.50.0` and `10.50.0` repository baseline, confirm that the same analyzer-host verifier guards and installation examples move with that version-line update.
- If dual-SDK analyzer proof materially increases main CI time, decide whether a later workflow split should move one host lane to a separate required job while preserving the same release gate.

## Risks
- Retargeting the analyzer package may surface hidden `net10.0` API usage and may require explicit companion assemblies for Workspaces or `System.Composition`; if those are missed, the package can restore yet still fail to load on the claimed host.
- The current verifier and docs are hard-coded to the `.NET 10 SDK`-only story, so partial updates across README, analyzer README, and packaged README checks could leave the repository internally inconsistent.
- A proof that still builds from project references instead of packed packages would give a false positive and would not actually protect the advertised pure `.NET 8 SDK` consumer story.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add deterministic proof that a net8.0 consumer project can restore, build, and run DVault analyzers with a .NET 8 SDK host. Update CI or local validation lanes, package verifier checks, analyzer package tests, and stale-guidance guards so unsupported net10-only analyzer assets cannot regress the 8.51.0 line.