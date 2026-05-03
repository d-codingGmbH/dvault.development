[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the contract is grounded in current repository baselines for the missing first .github CI workflow, the existing formatting/package gates, and the already integrated provider-category and package-verification upstream work.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `ls -la .github` from `/mnt/c/Projects/DVault` returned `ls: cannot access '.github': No such file or directory`, matching the contract claim that this is the repository's first tracked workflow surface.
- `DVault.slnx` includes the six packable library projects, the unit/integration/shared test projects, and `tools/DCoding.Data.DVault.PackageVerification`, so the root solution is a direct repository entry point for build, test, and pack automation.
- `tools/check-format.sh` directly enforces `.editorconfig` and `.gitattributes`, runs `bash tools/check-one-member-per-file.sh`, and runs `dotnet format DVault.slnx --verify-no-changes --no-restore`; `docs/formatting.md` says the first CI workflow must call this gate as a blocking step.
- `tools/verify-packages.sh` shells into `tools/DCoding.Data.DVault.PackageVerification/...`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` verifies exactly six `.nupkg`, six `.snupkg`, packaged `README.md`, generated XML docs, nuspec license/repository metadata, and provider-to-core dependency version alignment in `bin/packages/`.
- `tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs` defines `ProviderIntegration.RequiredLocal`, `ProviderIntegration.ExternalOptIn`, and `ProviderSmoke.Default`; `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` asserts SQLite tests are required-local and Postgres live-db tests are external opt-in.
- `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs` names `DVAULT_TEST_POSTGRES_CONNECTION_STRING` and an actionable skip message; `README.md` documents the same env-var opt-in path and the default/local provider-category commands.
- `git log --oneline --decorate -n 15` shows `3e3bf4a2 [06EXB80QQHAYH61RY4X3T1E8S0] AUTO-INTEGRATION squash into develop` and `c842e2b3 [06EXB828EAG5QE3WDR503GTBY8] AUTO-INTEGRATION squash into develop`; the current ticket's relation comments also record tester and integrator success for those source tickets.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not include a concrete example of an optional non-default CI job enablement path beyond the current Postgres local opt-in (`DVAULT_TEST_POSTGRES_CONNECTION_STRING`), so any future secret-backed external-provider job should remain explicitly separate from the default workflow.

Risky assumptions
- Approval assumes the first workflow will be attached to the normal candidate-validation trigger path rather than a manual-only trigger, because the contract does not pin exact workflow trigger names.
- Approval assumes the workflow will run on a bash-capable runner image with the expected .NET SDK, because the required repository-local gates are `bash tools/check-format.sh`, `dotnet ... DVault.slnx`, and `bash tools/verify-packages.sh`.
- Approval assumes default CI continues to rely on the current provider-boundary contract where SQLite is required-local and Postgres live-db coverage is opt-in; if new external-provider env vars or jobs appear later, this ticket contract will need coordinated updates.

AC / test suggestions
- Keep the CI stages visibly mapped to the repository-root rerun commands already documented in `README.md`: `bash tools/check-format.sh`, `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `dotnet pack DVault.slnx --configuration Release --nologo`, and `bash tools/verify-packages.sh`.
- If the implementation makes the default test boundary explicit with a filter, keep that filter aligned with `tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs` and the category guidance already documented in `README.md`.

Implementation watchouts
- Do not broaden the package matrix beyond the six solution-backed libraries already enforced by `DVault.slnx` and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`.
- Do not turn on live external-provider database jobs by default; the current direct repo evidence only exposes a Postgres opt-in configuration surface, and SQL Server, Oracle, and MySQL live-db provisioning is explicitly out of scope.
- Keep failure output reproducible from repository-root commands, because the existing scripts and docs already encode the local rerun path the ticket asks CI to mirror.

Non-blocking notes
- `git diff --name-only develop...HEAD` on the current ticket branch shows only `.gicket` metadata changes, so this PO-critic review is evaluating a pre-dev handoff contract against repository state already present on `develop`, not against new implementation work on this branch.

Split recommendations
- No additional split is needed for the default workflow. Keep any future secret-backed external-provider jobs or release/publication automation as separate follow-up tickets, consistent with the persisted contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment