[gicket-bot] PO-critic review contract

Summary
- Ticket contract is ready for developer handoff: the persisted contract has `PO Handoff` ready_for_po_critic, `## Open Questions` is `- none`, and the repository evidence already anchors the verifier, README, manual-publication, and CI surfaces this task must update.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git log --oneline --decorate -n 8 ticket/06F9G8FBQTAPXXS1Y4NR5QKVG8-task-update-package-verifier-and-ci-guidance-for` shows HEAD `ebd843bed` on the ticket branch above PO handoff commit `a276547da`; `git diff --name-only develop..HEAD` touches only `.gicket/tickets/06F9G8FBQTAPXXS1Y4NR5QKVG8/*` metadata plus related ticket comments/events, so this is still a pre-development handoff branch.
- docs/plans/shared-implementation-standards.md defines the v0.33 policy input as unchanged package IDs with `8.33.0` for `net8.0`/EF Core 8 and `10.33.0` for `net10.0`/EF Core 10, with no consumer-facing `0.33.0` package line and no mixed-line examples.
- `rg -n "TargetFrameworks|TargetFramework" ...` shows `src/DCoding.Data.DVault/*.csproj` and provider package csproj files target `net8.0;net10.0`, while `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` and `tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj` remain `net10.0` only, matching the clarified helper-project boundary.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs already verifies exactly seven `.nupkg` artifacts and six `.snupkg` artifacts, packaged README/XML/analyzer assets, `net8.0` and `net10.0` dependency groups, EF Core versions `8.0.27` / `10.0.8`, `Microsoft.Extensions.DependencyInjection.Abstractions` versions `8.0.2` / `10.0.8`, and rejects mixed EF Core dependency lines inside one target group.
- README.md still shows `dotnet add package ... --version 0.32.0` for the full package family, and `src/DCoding.Data.DVault.Analyzers/README.md` still shows `<PackageReference Include="DCoding.Data.DVault.Analyzers" Version="0.32.0" PrivateAssets="all" />`, which matches the contract's explicit documentation-drift callout.
- .github/workflows/ci.yml already runs restore, `bash tools/check-format.sh`, `dotnet build DVault.slnx --nologo`, filtered `dotnet test DVault.slnx --nologo --filter Category!=ProviderIntegration.ExternalOptIn`, `dotnet pack DVault.slnx --configuration Release --nologo`, and `bash tools/verify-packages.sh` with no publish step; docs/manual-nuget-publication.md still states publication remains manual and requires build/test/pack/verify/check-format evidence before publish approval.
- Related ticket state is coherent with the contract: `.gicket/tickets/06F9G8F4RQ0T7RV82M3H2H3FVG/ticket.json` is `done` for the prerequisite matrix-proof story, while `.gicket/tickets/06F9G8FJMZ3AY43YG06W2V4T8G/ticket.json` remains `todo` for broader downstream compatibility documentation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Developer-facing examples should explicitly show that consumers choose one version line end-to-end and must not mix `8.33.0` and `10.33.0` package references in one install example.
- Verifier/tests should cover the README/analyzer README drift case because the current repository still carries `0.32.0` install examples in both files.

Risky assumptions
- Implementation will reconcile the dual-line README requirement with the current `PackageVerifier.cs` README check, which today assumes a single `expectedInstallVersion` taken from the packed core package version.
- Task `06F9G8FJMZ3AY43YG06W2V4T8G` will later update broader compatibility/adopter docs so stale examples outside `README.md` and `src/DCoding.Data.DVault.Analyzers/README.md` do not remain the public baseline.
- Maintainers will understand that the existing .NET 10 SDK lane validates both `net8.0` and `net10.0` dependency groups because the verifier and analyzer helper project stay `net10.0` only.

AC / test suggestions
- Add verifier tests for missing `net8.0` or `net10.0` dependency groups, wrong `DCoding.Data.DVault` provider dependency version per target, wrong `Microsoft.EntityFrameworkCore.Relational` or `Microsoft.Extensions.DependencyInjection.Abstractions` versions, and mixed EF Core 8.x/10.x dependencies inside one target group.
- Add README-validation tests that fail on `0.32.0`, fail on a consumer-facing `0.33.0`, and fail when analyzer guidance drops the local `PrivateAssets=all` expectation.
- Keep one acceptance/test assertion that the blocking validation lane remains restore, `bash tools/check-format.sh`, build, filtered default-provider test, pack, and `bash tools/verify-packages.sh`, with no publish step.

Implementation watchouts
- Update `README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, and the packaged-README expectations in `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` together; current verifier logic still expects one install-version string.
- Preserve the manual publication boundary in `docs/manual-nuget-publication.md` and the no-publish CI lane in `.github/workflows/ci.yml`; this ticket should not introduce publish automation.
- Do not retarget `tools/DCoding.Data.DVault.PackageVerification` or `DCoding.Data.DVault.Analyzers` to `net8.0`; the contract explicitly keeps those helper projects `net10.0` only.
- Keep `DCoding.Data.DVault.Analyzers` guidance local and non-transitive; verifier behavior should continue to prove analyzer assets without treating the analyzer package as a runtime dependency.

Non-blocking notes
- `git diff --name-only develop..HEAD` shows only ticket metadata changes, so no implementation evidence exists yet on this branch; for this pre-development gate, that is a developer-handoff watchout rather than a PO blocker.
- The local comment history I inspected is workflow/refinement/lease automation only; no persisted comment reopened the contract after the current PO refinement handoff.

Split recommendations
- No split recommended. The done prerequisite story `06F9G8F4RQ0T7RV82M3H2H3FVG` already owns the matrix-proof history, and broader compatibility/adopter documentation stays correctly bounded in blocked task `06F9G8FJMZ3AY43YG06W2V4T8G`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment