[gicket-bot] PO-critic review contract

Summary
- Delivery contract is specific, locally evidenced, and has no unresolved PO questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket `06FBSBWH9F415E12VRHRYQ2JJM` Delivery Contract section `## Open Questions` says `- none`, so the persisted contract does not contain unresolved PO questions.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0` and packs the analyzer under `analyzers/dotnet/cs/`, matching the claimed single-asset compatibility boundary.
- `README.md` and `src/DCoding.Data.DVault.Analyzers/README.md` both currently say analyzer consumers must build with a `.NET 10 SDK` host, including `net8.0` projects using the `8.36.0` line, and both examples keep `PrivateAssets=all`.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` contains `ExpectedAnalyzerBuildHostGuidance` and `ValidateReadmeContainsAnalyzerBuildHostGuidance(...)`, so packaged README verification already enforces the build-host guidance and rejects drift.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0` and references the analyzer project with `SetTargetFramework=TargetFramework=net10.0`, which is direct local proof of the validated `net8.0` consumer target plus `.NET 10` build-host lane.
- `tools/pack-release-packages.sh` packs runtime lines for `8.36.0/net8.0` and `10.36.0/net10.0` but packs `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` once per version line without changing its target framework.
- `docs/local-validation.md`, `docs/manual-nuget-publication.md`, and `.github/workflows/ci.yml` all set the repository validation/publication baseline to `.NET 10 SDK`.
- Branch history is consistent with the ticket narrative: `git diff --name-status main..HEAD` shows the relevant README/verifier/audit/test files already differ from `main`, while `git diff 0b1790814f88de3cc56e15e8fb97588f463d971c..HEAD` is empty and `git log --oneline --all --grep=06FBSBWH9F415E12VRHRYQ2JJM` shows only lease/handoff commits for this ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract intentionally leaves pure `.NET 8 SDK` analyzer-host support out of scope and captures it as a follow-up question rather than a current-ticket blocker.

Risky assumptions
- The Delivery Contract risk section says live relations still show this ticket blocked by `06FBSBWBT33K7Y1Z6NM71GAQ68` and blocking `06FBSBWPN112S4CGP0239K0ZT8`; local repository inspection cannot verify whether those workflow relations have changed since the persisted snapshot.
- The ticket assumes the root README and analyzer README are the only in-scope packaged documentation surfaces that must carry the analyzer build-host caveat; secondary publication docs are mentioned only as a future follow-up question.

AC / test suggestions
- Keep one explicit negative verification case in scope: packaged README guidance should fail verification if the `.NET 10 SDK` build-host requirement is removed or contradicted.
- Keep acceptance language explicit that analyzer examples must stay on one coordinated line at a time (`8.36.0` or `10.36.0`) with `PrivateAssets=all` and no consumer-facing `0.36.0` version text.

Implementation watchouts
- Do not let developers infer pure `.NET 8 SDK` analyzer-host support from the `8.36.0` package line; the only direct local validation evidence is a `net8.0` consumer target compiled with the analyzer forced to `net10.0`.
- The current ticket branch contains no repository diff after scratch ref `0b1790814f88de3cc56e15e8fb97588f463d971c`; any remaining work should be treated as net-new implementation against an already-audited baseline, not as undiscovered branch changes tied to this ticket id.

Non-blocking notes
- Ticket snapshot shows `Recent comments: <none>` and `Closure evidence amendments: <none>`.
- `docs/plans/analyzer-package-compatibility-audit.md` is already present in the repo and matches the Delivery Contract's stated authoritative rationale.

Split recommendations
- If product intent expands to supporting a pure `.NET 8 SDK` analyzer host, keep that as a separate follow-up ticket that retargets the analyzer asset and adds an explicit verification lane.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment