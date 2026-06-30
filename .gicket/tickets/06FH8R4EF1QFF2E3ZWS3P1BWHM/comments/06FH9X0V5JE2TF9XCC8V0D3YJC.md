[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the already-ratified single-asset .NET 8/.NET 10 analyzer-host strategy: replace the current net10-only proof with packaged dual-host smoke coverage, update CI/local validation and package-verifier guardrails, and keep one analyzer package id and asset path.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows the current baseline is one `net10.0` analyzer asset under `analyzers/dotnet/cs/`, `.github/workflows/ci.yml` installs only `10.0.x`, and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` forces the analyzer project reference to `TargetFramework=net10.0` for the existing net8 smoke lane.
- Use `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` as the authoritative strategy for this ticket: support pure `.NET 8 SDK` and `.NET 10 SDK` hosts with one `netstandard2.0` `DCoding.Data.DVault.Analyzers` asset under the existing `analyzers/dotnet/cs/` path.
- Deterministic proof for this ticket means a packaged-consumer restore/build/run path on a `.NET 8 SDK` host; the current in-solution project-reference smoke test is not sufficient because it compiles a `net8.0` consumer against a forced `net10.0` analyzer build.
- The visible repository package-line baseline is still `8.50.0` and `10.50.0`, not yet `8.51.0`; refine the work against those current guards and carry the same pattern forward when the next coordinated line is introduced.
- Keep the existing consumer alignment rule: one coordinated package-version line at a time, analyzer reference local with `PrivateAssets="all"`, and no second analyzer package id or target-specific package fork.

Scope In
- Retarget `src/DCoding.Data.DVault.Analyzers` to the chosen single-asset compatibility strategy needed for pure `.NET 8 SDK` analyzer-host support, including package-managed Roslyn/Workspaces/System.Composition handling and explicit `System.Text.Json` compatibility where required.
- Replace the current net10-only analyzer-host smoke proof with deterministic packaged analyzer-consumer validation that exercises restore, build, and generated-analyzer execution on a `.NET 8 SDK` host, while preserving a corresponding `.NET 10 SDK` host proof.
- Update CI and repository validation guidance so the supported analyzer-host story is evidenced on both `.NET 8 SDK` and `.NET 10 SDK` hosts.
- Update `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and its unit tests so packaged README and analyzer-asset checks enforce the new support statement and reject stale net10-only guidance.
- Synchronize README, analyzer README, package compatibility, local validation, manual publication, and release-note text with the new dual-host analyzer support claim.

Scope Out
- No second analyzer package id, no split code-fix package, and no separate target-specific analyzer asset tree beyond the one reviewed asset set under `analyzers/dotnet/cs/`.
- No consumer runtime `lib/<tfm>` assets or new runtime dependency contract for `DCoding.Data.DVault.Analyzers`; it remains a local build-time package.
- No widening of package-line mix rules; consumers still choose exactly one coordinated line and keep analyzer references local.
- No broader compatibility claim beyond the explicitly proved `.NET 8 SDK` and `.NET 10 SDK` analyzer-host story.

Open questions
- none

Follow-up questions
- When the coordinated package versions advance beyond the current `8.50.0` and `10.50.0` repository baseline, confirm that the same analyzer-host verifier guards and installation examples move with that version-line update.
- If dual-SDK analyzer proof materially increases main CI time, decide whether a later workflow split should move one host lane to a separate required job while preserving the same release gate.

Risks
- Retargeting the analyzer package may surface hidden `net10.0` API usage and may require explicit companion assemblies for Workspaces or `System.Composition`; if those are missed, the package can restore yet still fail to load on the claimed host.
- The current verifier and docs are hard-coded to the `.NET 10 SDK`-only story, so partial updates across README, analyzer README, and packaged README checks could leave the repository internally inconsistent.
- A proof that still builds from project references instead of packed packages would give a false positive and would not actually protect the advertised pure `.NET 8 SDK` consumer story.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment